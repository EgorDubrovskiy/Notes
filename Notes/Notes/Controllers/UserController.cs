using Notes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Notes.Controllers
{
    public class UserController : Controller
    {
        DBContext db = new DBContext();

        [HttpPost]
        public void Exit() => Session["MainEmail"] = null;

        public ActionResult Home()
        {
            if (Session["MainEmail"] == null) return Redirect("~/Home/Index");

            string MainEmail = Session["MainEmail"].ToString();
            int UserId = DB.db.Users.Where(a => a.MainEmail == MainEmail).FirstOrDefault().Id;

            List<Note> notes = db.Notes.Where(a=>a.UserId==UserId).OrderByDescending(a=>a.Id).ToList();

            ViewBag.Notes = notes;
            return View();
        }

        [HttpPost]
        public JsonResult IsRestoringEmail(string Email) => Json(db.Users.Any(a => a.RestoringEmail == Email));

        [HttpPost]
        public JsonResult IsMainEmail(string Email) => Json(db.Users.Any(a => a.MainEmail == Email));

        [HttpPost]
        public JsonResult IsSecretKeyByMainEmail(string Email) => Json(db.Users.Any(a => a.MainEmail == Email && a.SecretKey != ""));

        [HttpPost]
        public JsonResult IsPassByMainEmail(string Pass, string Email)
        {
            Aes aes = new Aes(Aes.SAMPLE_KEY, Aes.SAMPLE_IV);
            Pass = aes.EncryptToBase64String(Pass);

            return Json(db.Users.Any(a => a.MainEmail == Email && a.Password == Pass));
        }

        [HttpPost]
        public JsonResult IsValidRecapcha(string response) => Json(ReCaptchaClass.Validate(response) == "True" ? true : false);

        [HttpPost]
        public async Task Registration(string MainEmail, string RestoringEmail, string Password)
        {
            Aes aes = new Aes(Aes.SAMPLE_KEY, Aes.SAMPLE_IV);
            string EncryptToBase64Password = aes.EncryptToBase64String(Password);

            Random random = new Random();

            User user = new User
            {
                MainEmail = MainEmail,
                RestoringEmail = RestoringEmail,
                Password = EncryptToBase64Password,
                SecretKey = aes.EncryptToBase64String(MyRandom.RandomString(random.Next(5, 10)))
            };

            db.Users.Add(user);
            db.SaveChanges();

            //отправка письма для подтверждения на почту
            string Link = "<a href=\"http://localhost/Notes/User/ConfReg?MainEmail=" + MainEmail +
            "&SecretKey=" + user.SecretKey+"\"><b>ссылка</b></a>";

            string Message = "<h3>Для подтверждения регистрации на сайте Notes перейдите по ссылке: " +
            Link+"</h3>";

            await Email.SendMessageAsync(Config.EmailAddress, "Админ сайта Notes ",Config.EmailPassword,
                MainEmail, "Уважаемый пользователь", "Уважаемый пользователь", Message);
        }

        private int GetIdByMainEmail(string MainEmail)
        {
            List<User> user = db.Users.Where(a=>a.MainEmail == MainEmail).ToList();
            if (user.Count == 0) return -1;
            else return user[0].Id;
        }

        //Подтверждение регистрации при переходе по ссылки с почты
        public ActionResult ConfReg(string MainEmail)
        {
            //обычным методом не достать переменную из url тк там есть символ - "+" который пропускаеться
            string SecretKey = Request.RawUrl.Substring(Request.RawUrl.IndexOf("SecretKey") + 10);

            int UserId = GetIdByMainEmail(MainEmail);
            if (UserId == -1) return View("~/Views/Shared/Error.cshtml");
            if (SecretKey != db.Users.Find(UserId).SecretKey) return View("~/Views/Shared/Error.cshtml");

            //удаляем секретный ключ из бд
            db.Database.ExecuteSqlCommand("UPDATE Users SET SecretKey='' WHERE Id=" + UserId);

            //сохраняем пользователя в сессию
            Session["MainEmail"] = MainEmail;

            return Redirect("~/User/Home");
        }

        public ActionResult Login(string MainEmail)
        {
            Session["MainEmail"] = MainEmail;
            return Redirect("~/User/Home");
        }

        //отрпавка письма для восстановления пароля
        public async Task SendMesForPassRec(string RestoringEmail)
        {
            Aes aes = new Aes(Aes.SAMPLE_KEY, Aes.SAMPLE_IV);
            string SecretKey = aes.EncryptToBase64String(MyRandom.RandomString(new Random().Next(5, 10)));

            //отправка письма для подтверждения на почту
            string Link = "<a href=\"http://localhost/Notes/User/PassRec?RestoringEmail=" + RestoringEmail +
            "&SecretKey=" + SecretKey + "\"><b>ссылка</b></a>";

            string Message = "<h3>Для восстановления пароля на сайте Notes перейдите по ссылке: " +
            Link + "</h3>";

            await Email.SendMessageAsync(Config.EmailAddress, "Админ сайта Notes ", Config.EmailPassword,
                RestoringEmail, "Уважаемый пользователь", "Уважаемый пользователь", Message);

            db.Database.ExecuteSqlCommand("UPDATE Users SET SecretKey='"+ SecretKey + "' WHERE RestoringEmail='" + RestoringEmail+"'");

        }

        public ActionResult PassRec(string RestoringEmail)
        {
            //обычным методом не достать переменную из url тк там есть символ - "+" который пропускаеться
            string SecretKey = Request.RawUrl.Substring(Request.RawUrl.IndexOf("SecretKey") + 10);
            if (db.Users.Any(a => a.RestoringEmail == RestoringEmail && a.SecretKey == SecretKey))
                return View("~/Views/User/PassChange.cshtml");
            else return View("~/Views/Shared/Error.cshtml");
        }

        public void SaveNewPass(string NewPassword, string RestoringEmail)
        {
            Aes aes = new Aes(Aes.SAMPLE_KEY, Aes.SAMPLE_IV);
            NewPassword = aes.EncryptToBase64String(NewPassword);

            db.Database.ExecuteSqlCommand("UPDATE Users SET Password='"+ NewPassword + 
                "' WHERE RestoringEmail='" + RestoringEmail + "'");

            db.Database.ExecuteSqlCommand("UPDATE Users SET SecretKey='' WHERE RestoringEmail='" + RestoringEmail + "'");

            string Message = "<h3>Пароль успешно изменён!</h3>";
            Email.SendMessageAsync(Config.EmailAddress, "Админ сайта Notes ", Config.EmailPassword,
                RestoringEmail, "Уважаемый пользователь", "Уважаемый пользователь", Message);
        }

    }
}