using Notes.Models;
using System.Web.Mvc;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Configuration;
using System.Threading.Tasks;
using System;
using Notes.Controllers;
using System.Web.Helpers;

namespace Заметки.Controllers
{
    public class NoteController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Add()
        {
            string Title = Request.Form["Title"];
            string Description = Request.Unvalidated().Form["Description"];
            string Date = Request.Form["Date"];

            DateTime date = Convert.ToDateTime(Date);
            string MainEmail = Session["MainEmail"].ToString();
            int UserId = DB.db.Users.Where(a => a.MainEmail == MainEmail).FirstOrDefault().Id;
            Note NewNote = new Note
            {
                Title = Title,
                Description = Description,
                Date = date,
                UserId = UserId
            };
            DB.db.Notes.Add(NewNote);
            DB.db.SaveChanges();
            return Json(NewNote.Id);
        }

        [HttpPost]
        public JsonResult GetNote()
        {
            int Id = int.Parse(Request.Form["Id"].ToString());
            return Json(DB.db.Notes.Find(Id).Clone());
        }

        [HttpPost]
        public void Edit()
        {
            int Id = int.Parse(Request.Form["Id"]);
            string Title = Request.Form["Title"];
            string Description = Request.Unvalidated().Form["Description"];
            string Date = Request.Form["Date"];

            var note = DB.db.Notes.Find(Id);
            note.Title = Title;
            note.Description = Description;
            note.Date = Convert.ToDateTime(Date);
            DB.db.SaveChanges();
        }


        [HttpPost]
        public void Delete(int Id)
        {
            Note note = DB.db.Notes.Find(Id);
            DB.db.Notes.Remove(note);
            DB.db.SaveChanges();
        }

    }
}