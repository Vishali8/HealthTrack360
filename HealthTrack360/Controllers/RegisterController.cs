using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using HealthTrack360.Models;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
namespace HealthTrack360.Controllers
{
    public class RegisterController : Controller
    {
        string conStr = ConfigurationManager.ConnectionStrings["HealthTracker360"].ConnectionString;

        // GET: Register
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string email, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                ViewBag.Error = "Passwords do not match.";
                return View();
            }

            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();

                // ✅ Check if email already exists
                string checkQuery = "SELECT COUNT(*) FROM Recruiters WHERE Email = @Email";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@Email", email);
                int exists = (int)checkCmd.ExecuteScalar();

                if (exists > 0)
                {
                    ViewBag.Error = "Email already registered.";
                    conn.Close();
                    return View();
                }

                // ✅ Insert new recruiter
                string hashedPassword = HashPassword(password);
                string insertQuery = "INSERT INTO Recruiters (Email, Password) VALUES (@Email, @Password)";
                SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                insertCmd.Parameters.AddWithValue("@Email", email);
                insertCmd.Parameters.AddWithValue("@Password", hashedPassword);
                insertCmd.ExecuteNonQuery();

                conn.Close();
            }

            TempData["SuccessMessage"] = "Registration successful. Please log in.";
            return RedirectToAction("Index", "Login");
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
    }
}