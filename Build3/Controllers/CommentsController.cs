using Build1.Models;
using Build3.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Build1.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpDelete]
        [Authorize(Roles = "Membru , Organizator, Admin")]
        public ActionResult Delete(int id)
        {
            Comment comm = db.Comments.Find(id);
            db.Comments.Remove(comm);
            db.SaveChanges();
            TempData["message"] = "Comentariul a fost sters!";
            return Redirect("/Tasks/Show/" + comm.IdTask);
        }

        
        [HttpPost]
        [Authorize(Roles = "Membru , Organizator, Admin")]
        public ActionResult New(Comment comm)
        {
            if (ModelState.IsValid)
            {
                comm.DataComment = DateTime.Now;
                comm.IdUser = User.Identity.GetUserId();
                comm.Task = db.Tasks.Find(comm.IdTask);
                db.Comments.Add(comm);
                db.SaveChanges();
                TempData["message"] = "Comentariul a fost adaugat!";
                return Redirect("/Tasks/Show/" + comm.IdTask);
            }
            else
            {
                TempData["CommCorect"] = 0;
                return Redirect("/Tasks/Show/" + comm.IdTask);
            }
        }

        [Authorize(Roles = "Membru , Organizator, Admin")]
        public ActionResult Edit(int id)
        {
            Comment comm = db.Comments.Find(id);
            return View(comm);
        }

        [Authorize(Roles = "Membru , Organizator, Admin")]
        [HttpPut]
        public ActionResult Edit(int id, Comment requestComment)
        {
            if (ModelState.IsValid)
            {
                Comment comm = db.Comments.Find(id);
                if (TryUpdateModel(comm))
                {
                    comm.ContinutComment = requestComment.ContinutComment;
                    db.SaveChanges();
                    TempData["message"] = "Comentariul a fost modificat!";
                }
                return Redirect("/Tasks/Show/" + comm.IdTask);
            }
            else
            {
                return View(requestComment);
            }
        }
    }
}