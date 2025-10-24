using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;
using HealthTrack360.Models;
using System.Net;
using System.Net.Mail;


namespace HealthTrack360.Controllers
{
    public class HomeController : Controller
    {
        string conStr = ConfigurationManager.ConnectionStrings["HealthTracker360"].ConnectionString;

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Features() => View();
        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Contact(string name, string email, string message)
        {
            // 1. Log to database
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                string query = "INSERT INTO ContactLogs (Name, Email, Message) VALUES (@Name, @Email, @Message)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Message", message);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            // 2. Send email alert
            var mail = new MailMessage();
            mail.To.Add("rvishali1997@gmail.com"); // ✅ Your support email
            mail.Subject = "New Contact Message from " + name;
            mail.Body = $"Name: {name}\nEmail: {email}\n\nMessage:\n{message}";
            mail.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("rvishali1997@gmail.com", "bpgg vdbw tfjv odpf");
            smtp.EnableSsl = true;

            smtp.Send(mail);

            ViewBag.Message = "Thank you for contacting us, " + name + ". We'll get back to you soon!";
            return View();
        }

        public ActionResult PrivacyPolicy()
        {
            return View();
        }
        public ActionResult TryDemo()
        {
            var samplePatient = new Patient { FullName = "John Doe", DOB = new DateTime(1990, 1, 1) };
            var sampleVisit = new Visit { VisitDate = DateTime.Today.AddDays(-5), DoctorName = "Dr. Smith" };
            var sampleVital = new Vital { BloodPressure = "120/80" };
            var sampleAlert = new Alert { Description = "High BP", TriggeredAt = DateTime.Today };

            ViewBag.Patient = samplePatient;
            ViewBag.Visit = sampleVisit;
            ViewBag.Vital = sampleVital;
            ViewBag.Alert = sampleAlert;

            return View();
            
        }
        public ActionResult Demo()
        {
            return RedirectToAction("Dashboard", "Recruiter");
        }
        
    }
}