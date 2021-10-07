using Build1.Models;
using Build3.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Build1.Controllers
{
    public class ProiectsController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Membru , Organizator, Admin")]
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                var proiects = db.Proiects;
                ViewBag.Proiects = proiects;
            }
            else
            {
                List<Proiect> proiects = new List<Proiect>();
                ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                foreach ( var contract in user.Contracte)
                {
                    proiects.Add(db.Proiects.Find(contract.IdProiect));
                }
                ViewBag.Proiects = proiects;
            }
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
            return View();
        }

        [Authorize(Roles = "Membru , Organizator, Admin")]
        public ActionResult New()
        {
            Proiect proiect = new Proiect();
            proiect.IdUser = User.Identity.GetUserId();
            return View(proiect);
        }

        [HttpPost]
        [Authorize(Roles = "Membru , Organizator, Admin")]
        public ActionResult New(Proiect proiect)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            var id = User.Identity.GetUserId();

            proiect.IdUser = id;
            proiect.DataProiect = DateTime.Now;
            if (ModelState.IsValid)
            {
                if (User.IsInRole("Membru"))
                {
                    UserManager.RemoveFromRole(id, "Membru");
                    UserManager.AddToRole(id, "Organizator");
                }
                Contract contract = new Contract();
                contract.IdUser = proiect.IdUser;
                contract.IdProiect = proiect.IdProiect;
                db.Contracts.Add(contract);
                db.Proiects.Add(proiect);
                db.SaveChanges();
                TempData["message"] = "Proiectul a fost adaugat cu succes!";
                return RedirectToAction("Index");
            }
            else
            {
                return View(proiect);
            }

        }

        [Authorize(Roles = "Membru , Organizator, Admin")]
        public ActionResult Show(int id)
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
            Proiect proiect = db.Proiects.Find(id);
            ViewBag.Org = proiect.IdUser;
            ViewBag.IdCurent = User.Identity.GetUserId();
            return View(proiect);
        }

        [Authorize(Roles = "Organizator, Admin")]
        public ActionResult Edit(int id)
        {

            Proiect proiect = db.Proiects.Find(id);

            if (proiect.IdUser == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(proiect);
            }
            else
            {
                TempData["message"] = "Nu puteti modifica un proiect care nu va apartine!";
                return RedirectToAction("Index");
            }

        }


        [HttpPut]
        [Authorize(Roles = "Organizator, Admin")]
        public ActionResult Edit(int id, Proiect requestProiect)
        {

            if (ModelState.IsValid)
            {
                Proiect proiect = db.Proiects.Find(id);

                if (proiect.IdUser == User.Identity.GetUserId() || User.IsInRole("Admin"))
                {
                    
                    if (TryUpdateModel(proiect))
                    {
                        proiect.NumeProiect = requestProiect.NumeProiect;
                        db.SaveChanges();
                        TempData["message"] = "Proiectul a fost modificat!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(requestProiect);
                    }
                }
                else
                {
                    TempData["message"] = "Nu puteti modifica un proiect care nu va apartine!";
                    return RedirectToAction("Index");
                }

            }
            else
            {
                return View(requestProiect);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Organizator, Admin")]
        public ActionResult Delete(int id)
        {
            Proiect proiect = db.Proiects.Find(id);

            if (proiect.IdUser == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                db.Proiects.Remove(proiect);
                db.SaveChanges();
                TempData["message"] = "Proiectul a fost sters";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu puteti sterge un proiect care nu va apartine!";
                return RedirectToAction("Index");
            }


        }

        [Authorize(Roles = "Organizator, Admin")]
        public ActionResult AddUser(int id)
        {
            Proiect proiect = db.Proiects.Find(id);

            if (proiect.IdUser == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {

                Contract contract = new Contract();
                ViewBag.IdP = id;

                var Useri = new List<SelectListItem>();
                int ok;
                foreach (var u in db.Users.ToList())
                {
                    ok = 0;
                    if (u.Contracte != null)
                    {
                        foreach (var con in u.Contracte)
                        {
                            if (con.IdProiect == id)
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
                return View(contract);
            }
            else
            {
                TempData["message"] = "Nu puteti modifica un proiect care nu va apartine!";
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        public ActionResult AddUser(Contract contract)
        {
            if (ModelState.IsValid)
            {
                Proiect proiect = db.Proiects.Find(contract.IdProiect);
                db.Contracts.Add(contract);
                db.SaveChanges();
                TempData["message"] = "Membrul a fost adaugat cu succes!";
                return Redirect("/Proiects/Show/" + contract.IdProiect);
            }
            else
            {
                return View(contract);
            }

        }

        [HttpDelete]
        [Authorize(Roles = "Organizator, Admin")]
        public ActionResult DeleteContract(int id)
        {
            Contract contract = db.Contracts.Find(id);
            Proiect proiect = db.Proiects.Find(contract.IdProiect);

            if ((proiect.IdUser == User.Identity.GetUserId() || User.IsInRole("Admin")) && (contract.IdUser != proiect.IdUser))
            {
                db.Contracts.Remove(contract);
                db.SaveChanges();
                TempData["message"] = "Utilizatorul a fost eliminat din proiect!";
                return RedirectToAction("/Show/" + proiect.IdProiect);
            }
            else
            {
                TempData["message"] = "Nu puteti elimina acest utilizator din proiect!";
                return RedirectToAction("/Show/" + proiect.IdProiect);
            }
        }
    }
}