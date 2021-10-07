using Build1.Models;
using Build3.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Build1.Controllers
{
    public class TasksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Organizator, Admin")]
        public ActionResult New(int id)
        {
            Task task = new Task();
            ViewBag.IdP = id;
            Proiect proiect = db.Proiects.Find(id);

            if (proiect.IdUser == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(task);
            }
            else
            {
                TempData["message"] = "Nu puteti adauga taskuri unui proiect care nu va apartine!";
                return Redirect("/Proiects/Index/");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Organizator, Admin")]
        public ActionResult New(Task task)
        {
            

            if (ModelState.IsValid)
            {
                Proiect proiect = db.Proiects.Find(task.IdProiect);
                task.DataTask = DateTime.Now;
                db.Tasks.Add(task);
                db.SaveChanges();
                TempData["message"] = "Taskul a fost adaugat cu succes!";
                return Redirect("/Proiects/Show/" + task.IdProiect);
            }
            else
            {
                ViewBag.IdP = task.IdProiect;
                return View(task);
            }

        }

        [Authorize(Roles = "Membru , Organizator, Admin")]
        public ActionResult Show(int id)
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
            if (TempData.ContainsKey("CommCorect"))
            {
                ViewBag.CommCorect = TempData["CommCorect"];
            }
            ViewBag.IdCurent = User.Identity.GetUserId();
            Task task = db.Tasks.Find(id);
            ViewBag.Org = task.Proiect.IdUser;

            var ok = 0;
            if (User.Identity.GetUserId() == task.Proiect.IdUser) ok = 1;
            else
            {
                foreach( var rep in task.Rapartizari)
                {
                    if (User.Identity.GetUserId() == rep.IdUser) ok = 1;
                }
            }

            ViewBag.Permisiuni = ok;

            return View(task);
        }

        [Authorize(Roles = "Membru , Organizator, Admin")]
        public ActionResult End(int id)
        {
            Task task = db.Tasks.Find(id);
            task.DataSfarsitTask = DateTime.Now;
            task.Status = "Terminat";
            db.SaveChanges();
            TempData["message"] = "Taskul a fost terminat!";
            return RedirectToAction("Show/" + id);
        }

        [Authorize(Roles = "Organizator, Admin")]
        public ActionResult Edit(int id)
        {
            Task task = db.Tasks.Find(id);
            Proiect proiect = task.Proiect;


            if (proiect.IdUser == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(task);
            }
            else
            {
                TempData["message"] = "Nu puteti modifica taskul unui proiect care nu va apartine!";
                return Redirect("/Proiects/Index/");
            }            
        }


        [HttpPut]
        [Authorize(Roles = "Organizator, Admin")]
        public ActionResult Edit(int id, Task requestTask)
        {

            if (ModelState.IsValid)
            {
                Task task = db.Tasks.Find(id);
                if (TryUpdateModel(task))
                {
                    task.NumeTask = requestTask.NumeTask;
                    task.DescriereTask = requestTask.DescriereTask;
                    task.Status = requestTask.Status;
                    db.SaveChanges();
                    TempData["message"] = "Taskul a fost modificat!";
                    return RedirectToAction("Show/" + id);
                }
                else
                {
                    return View(requestTask);
                }
            }
            else
            {
                return View(requestTask);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Organizator, Admin")]
        public ActionResult Delete(int id)
        {

            Task task = db.Tasks.Find(id);

            if (task.Proiect.IdUser == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                db.Tasks.Remove(task);
                db.SaveChanges();
                TempData["message"] = "Taskul a fost sters";
                return RedirectToAction("Show/" + task.IdProiect, "Proiects");
            }
            else
            {
                TempData["message"] = "Nu puteti modifica un task la un proiect care nu va apartine!";
                return Redirect("/Home/Index");
            }
        }






        [Authorize(Roles = "Organizator, Admin")]
        public ActionResult AddRepartizare(int id)
        {
            Task task = db.Tasks.Find(id);

            if (task.Proiect.IdUser == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {

                Repartizare repartizare = new Repartizare();

                ViewBag.IdT = id;

                var UseriProiect = new List<ApplicationUser>();

                foreach (var u in task.Proiect.Contracte)
                {
                    if (u.User.Id != task.Proiect.IdUser )
                    {
                        UseriProiect.Add(u.User);
                    }
                }

                var Useri = new List<SelectListItem>();
                int ok = 1;
                foreach (var u in UseriProiect)
                {
                    ok = 0;
                    if (u.Repartizari != null)
                    {
                        foreach (var r in u.Repartizari)
                        {
                            if (r.IdTask == id)
                            {
                                ok = 1;
                            }
                        }
                    }
                    if (ok == 0)
                    {
                        Useri.Add(new SelectListItem
                        {
                            Value = u.Id.ToString(),
                            Text = u.UserName.ToString()
                        });
                    }
                }

                ViewBag.useri = Useri;
                return View(repartizare);
            }
            else
            {
                TempData["message"] = "Nu puteti modifica un task la un proiect care nu va apartine!";
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        public ActionResult AddRepartizare(Repartizare repartizare)
        {
            if (ModelState.IsValid)
            {
                Task task = db.Tasks.Find(repartizare.IdTask);
                db.Repartizari.Add(repartizare);
                db.SaveChanges();
                TempData["message"] = "Membrul a fost repartizat cu succes!";
                return Redirect("/Tasks/Show/" + repartizare.IdTask);
            }
            else
            {
                return View(repartizare);
            }

        }

        [HttpDelete]
        [Authorize(Roles = "Organizator, Admin")]
        public ActionResult DeleteRepartizare(int id)
        {
            Repartizare repartizare = db.Repartizari.Find(id);
            Task task = db.Tasks.Find(repartizare.IdTask);

            if ((task.Proiect.IdUser == User.Identity.GetUserId() || User.IsInRole("Admin")))
            {
                db.Repartizari.Remove(repartizare);
                db.SaveChanges();
                TempData["message"] = "Utilizatorul a fost eliminat de la task!";
                return RedirectToAction("/Show/" + task.IdTask);
            }
            else
            {
                TempData["message"] = "Nu puteti elimina acest utilizator de la task!";
                return RedirectToAction("/Show/" + task.IdTask);
            }
        }


    }
}