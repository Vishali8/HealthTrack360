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
    public class VisitController : Controller
    {
        // GET: Visit
        string conStr = ConfigurationManager.ConnectionStrings["HealthTracker360"].ConnectionString;
        public ActionResult Index(int? patientId)
        {
            
            List<Visit> visits = new List<Visit>();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = patientId.HasValue
            ? "SELECT v.*, p.FullName FROM Visits v JOIN Patients p ON v.PatientID = p.PatientID WHERE v.PatientID = @pid"
            : "SELECT v.*, p.FullName FROM Visits v JOIN Patients p ON v.PatientID = p.PatientID";
                SqlCommand cmd = new SqlCommand(query, con);
                if (patientId.HasValue)
                    cmd.Parameters.AddWithValue("@pid", patientId.Value);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    visits.Add(new Visit
                    {
                        VisitID = Convert.ToInt32(rdr["VisitID"]),
                        VisitDate = Convert.ToDateTime(rdr["VisitDate"]),
                        Department = rdr["Department"].ToString(),
                        DoctorName = rdr["DoctorName"].ToString(),
                        Reason = rdr["Reason"].ToString(),
                        PatientID = Convert.ToInt32(rdr["PatientID"]),
                        Patient = new Patient { FullName = rdr["FullName"].ToString() }
                    });
                }
            }
            ViewBag.PatientID = patientId;
            return View(visits);
        }
        public ActionResult Create(int? patientId)
        {
            List<SelectListItem> patients = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT PatientID, FullName FROM Patients", con);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    patients.Add(new SelectListItem
                    {
                        Value = rdr["PatientID"].ToString(),
                        Text = rdr["FullName"].ToString(),
                        Selected = patientId.HasValue && patientId.Value == Convert.ToInt32(rdr["PatientID"])
                    });
                }
            }
            ViewBag.PatientList = patients;
            return View();
        }
        [HttpPost]
        public ActionResult Create(Visit visit)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Visits (VisitDate, Department, DoctorName, Reason, PatientID) VALUES (@VisitDate, @Department, @DoctorName, @Reason, @PatientID)", con);
                cmd.Parameters.AddWithValue("@VisitDate", visit.VisitDate);
                cmd.Parameters.AddWithValue("@Department", visit.Department);
                cmd.Parameters.AddWithValue("@DoctorName", visit.DoctorName);
                cmd.Parameters.AddWithValue("@Reason", visit.Reason);
                cmd.Parameters.AddWithValue("@PatientID", visit.PatientID);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            Visit visit = new Visit();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT v.*, p.FullName, p.Gender, p.DOB, p.ContactNumber, p.Email FROM Visits v JOIN Patients p ON v.PatientID = p.PatientID WHERE v.VisitID = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    visit.VisitID = id;
                    visit.VisitDate = Convert.ToDateTime(rdr["VisitDate"]);
                    visit.Department = rdr["Department"].ToString();
                    visit.DoctorName = rdr["DoctorName"].ToString();
                    visit.Reason = rdr["Reason"].ToString();
                    visit.PatientID = Convert.ToInt32(rdr["patientID"]);
                    visit.Patient = new Patient
                    {
                        FullName = rdr["FullName"].ToString(),
                        Gender = rdr["Gender"].ToString(),
                        DOB = Convert.ToDateTime(rdr["DOB"]),
                        ContactNumber = rdr["ContactNumber"].ToString(),
                        Email = rdr["Email"].ToString()
                    };
                }
            }            
            return View(visit);
        }

        [HttpPost]
        public ActionResult Edit(Visit visit)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Visits SET VisitDate=@VisitDate, Department=@Department, DoctorName=@DoctorName, Reason=@Reason WHERE VisitID=@VisitID", con);
                cmd.Parameters.AddWithValue("@VisitDate", visit.VisitDate);
                cmd.Parameters.AddWithValue("@Department", visit.Department);
                cmd.Parameters.AddWithValue("@DoctorName", visit.DoctorName);
                cmd.Parameters.AddWithValue("@Reason", visit.Reason);                
                cmd.Parameters.AddWithValue("@VisitID", visit.VisitID);
                
                con.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index", new { patientId = visit.PatientID });
        }

        public ActionResult Delete(int id)
        {Visit visit = new Visit();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT v.*, p.FullName  FROM Visits v JOIN Patients p ON v.PatientID = p.PatientID WHERE v.VisitID = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    visit.VisitID = id;
                    visit.Department = rdr["Department"].ToString();
                    visit.DoctorName = rdr["DoctorName"].ToString();
                    visit.VisitDate= Convert.ToDateTime(rdr["VisitDate"]);
                    visit.Reason = rdr["Reason"].ToString();
                    visit.PatientID = Convert.ToInt32(rdr["PatientID"]);
                    visit.Patient = new Patient { FullName = rdr["FullName"].ToString() };
                }
            }
            return View(visit); // ✅ This shows the Delete.cshtml page
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Visits WHERE VisitID = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}