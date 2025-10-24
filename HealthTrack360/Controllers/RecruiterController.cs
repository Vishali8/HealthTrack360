using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using HealthTrack360.Models;
using System.Data.SqlClient;
using System.Configuration;


namespace HealthTrack360.Controllers
{
    public class RecruiterController : Controller
    {
        string conStr = ConfigurationManager.ConnectionStrings["HealthTracker360"].ConnectionString;

        // GET: Recruiter
        private HealthContext db = new HealthContext();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string email, string password)
        {
            bool isValid = false;

            using (SqlConnection conn = new SqlConnection(conStr))
            {
                string query = "SELECT COUNT(*) FROM Recruiter WHERE Email = @Email AND Password = @Password";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                conn.Close();

                isValid = count > 0;
            }

            if (isValid)
            {
                Session["RecruiterEmail"] = email;
                return RedirectToAction("Dashboard", "Recruiter"); // 👈 Redirect to Dashboard
            }

            ViewBag.Error = "Invalid email or password.";
            return View();
        }

        public ActionResult Dashboard()
        {
            // Fetch summary data
            var totalPatients = db.Patients.Count();
            DateTime sevenDaysAgo = DateTime.Today.AddDays(-7);
            var recentVisits = db.Visits
                .Where(v => DbFunctions.TruncateTime(v.VisitDate) >= sevenDaysAgo)
                .Count();
            var activeAlerts = db.Alerts.Count();

            // Pass data to view
            ViewBag.TotalPatients = totalPatients;
            ViewBag.RecentVisits = recentVisits;
            ViewBag.ActiveAlerts = activeAlerts;
            ViewBag.RecruiterEmail = Session["RecruiterEmail"];

            return View();
        }
    }
}