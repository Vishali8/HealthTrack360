using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;
using HealthTrack360.Models;


namespace HealthTrack360.Controllers
{

    public class LoginController : Controller
    {
        // GET: Login

        string conStr = ConfigurationManager.ConnectionStrings["HealthTracker360"].ConnectionString;
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Message = TempData["SuccessMessage"];
            return View();
        }
        [HttpPost]
        public ActionResult Index(string email, string password)
        {
            bool isValid = false;
            int patientId = 0;
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                string query = "SELECT COUNT(*) FROM Recruiter WHERE Email = @Email AND Password = @Password";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                if (count > 0)
                {
                    isValid = true;
                    string pquery = "SELECT PatientID FROM Patient p JOIN Recruiter r ON p.Email = r.Email WHERE r.Email = @Email";
                    SqlCommand pcmd = new SqlCommand(pquery, conn);
                    var result = pcmd.ExecuteScalar();
                    if (result != null)
                        patientId = Convert.ToInt32(result);
                }
                conn.Close();                
            }

            if (isValid)
            {
                Session["RecruiterEmail"] = email;
                Session["PatientId"] = patientId;
                return RedirectToAction("PatientDetails", "Patient");
            }

            ViewBag.Error = "Invalid email or password.";
            return View();
        }
    }
}