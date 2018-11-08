using Notes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Notes.Filters;

namespace Notes.Controllers
{
    public class AdminController: Controller
    {
        public ActionResult Index(string Key)
        {
            if (Session["AdminKey"] != null)
                if(Session["AdminKey"].ToString() == Config.PrKey)
                    return View("Adminka");
            if (Config.PrKey == Key)
            {
                Session["AdminKey"] = Key;
                return View("Adminka");
            }
            return View();
        }

        //users начало
        [AdminAuthorize]
        public ActionResult Users()
        {
            return View();
        }
        [AdminAuthorize]
        public ActionResult EditUser(int id)
        {
            var User = DB.db.Users.Find(id);
            ViewBag.User = User;
            return View();
        }
        [AdminAuthorize]
        [HttpPost]
        public ActionResult EditUser(User user)
        {
            var userNew = DB.db.Users.Find(user.Id);
            userNew.MainEmail = user.MainEmail;
            userNew.Password = user.Password;
            userNew.SecretKey = user.SecretKey;
            userNew.RestoringEmail = user.RestoringEmail;
            DB.db.SaveChanges();
            return RedirectPermanent("~/Admin/Users");
        }
        [AdminAuthorize]
        public ActionResult DeleteUser(int id)
        {
            DB.db.Users.Remove(DB.db.Users.Find(id));
            DB.db.SaveChanges();
            return RedirectPermanent("~/Admin/Users");
        }
        [AdminAuthorize]
        [HttpPost]
        public ActionResult AddUser(User user)
        {
            Aes aes = new Aes(Aes.SAMPLE_KEY, Aes.SAMPLE_IV);
            user.Password = aes.EncryptToBase64String(user.Password);

            DB.db.Users.Add(user);
            DB.db.SaveChanges();
            return RedirectPermanent("~/Admin/Users");
        }
        [AdminAuthorize]
        public ActionResult AddUser()
        {
            return View();
        }
        //users конец

        //notes начало
        [AdminAuthorize]
        public ActionResult Notes()
        {
            return View();
        }
        [AdminAuthorize]
        public ActionResult EditNote(int id)
        {
            var note = DB.db.Notes.Find(id);
            ViewBag.Note = note;
            return View();
        }
        [AdminAuthorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditNote(Note note)
        {
            var noteNew = DB.db.Notes.Find(note.Id);
            noteNew.Title = note.Title;
            noteNew.Description = note.Description;
            noteNew.Date = note.Date;
            noteNew.UserId = note.UserId;
            DB.db.SaveChanges();
            return RedirectPermanent("~/Admin/Notes");
        }
        [AdminAuthorize]
        public ActionResult DeleteNote(int id)
        {
            DB.db.Notes.Remove(DB.db.Notes.Find(id));
            DB.db.SaveChanges();
            return RedirectPermanent("~/Admin/Notes");
        }
        [AdminAuthorize]
        [HttpPost]
        public ActionResult AddNote(Note note)
        {
            note.User = DB.db.Users.Find(note.UserId);
            DB.db.Notes.Add(note);
            DB.db.SaveChanges();
            return RedirectPermanent("~/Admin/Notes");
        }
        [AdminAuthorize]
        public ActionResult AddNote()
        {
            return View();
        }
        //notes конец

    }
}