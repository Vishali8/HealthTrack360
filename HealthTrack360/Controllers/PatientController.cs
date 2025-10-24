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
    public class PatientController : Controller
    {
        // GET: Patient
        string conStr = ConfigurationManager.ConnectionStrings["HealthTracker360"].ConnectionString;

        public ActionResult Index()
        {

            if (Session["RecruiterEmail"] == null)
                return RedirectToAction("Index", "Login");

            List<Patient> patients = new List<Patient>();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Patients", con);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    patients.Add(new Patient
                    {
                        PatientID = Convert.ToInt32(rdr["PatientID"]),
                        FullName = rdr["FullName"].ToString(),
                        Gender = rdr["Gender"].ToString(),
                        DOB = Convert.ToDateTime(rdr["DOB"]),
                        ContactNumber = rdr["ContactNumber"].ToString(),
                        Email = rdr["Email"].ToString()
                    });
                }
            }
            return View(patients);
        }
        public ActionResult Create()
        {
            if (Session["RecruiterEmail"] == null)
                return RedirectToAction("Index", "Login");

            ViewBag.Title = "Add";
            return View();
        }

        [HttpPost]
        public ActionResult Create(Patient patient)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Patients (FullName, Gender, DOB, ContactNumber, Email) VALUES (@FullName, @Gender, @DOB, @ContactNumber, @Email)", con);
                cmd.Parameters.AddWithValue("@FullName", patient.FullName);
                cmd.Parameters.AddWithValue("@Gender", patient.Gender);
                cmd.Parameters.AddWithValue("@DOB", patient.DOB);
                cmd.Parameters.AddWithValue("@ContactNumber", patient.ContactNumber);
                cmd.Parameters.AddWithValue("@Email", patient.Email);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            if (Session["RecruiterEmail"] == null)
                return RedirectToAction("Index", "Login");

            Patient patient = new Patient();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Patients WHERE PatientID = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    patient.PatientID = id;
                    patient.FullName = rdr["FullName"].ToString();
                    patient.Gender = rdr["Gender"].ToString();
                    patient.DOB = Convert.ToDateTime(rdr["DOB"]);
                    patient.ContactNumber = rdr["ContactNumber"].ToString();
                    patient.Email = rdr["Email"].ToString();
                }
            }
            return View(patient);
        }

        [HttpPost]
        public ActionResult Edit(Patient patient)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Patients SET FullName=@FullName, Gender=@Gender, DOB=@DOB, ContactNumber=@ContactNumber, Email=@Email WHERE PatientID=@PatientID", con);
                cmd.Parameters.AddWithValue("@FullName", patient.FullName);
                cmd.Parameters.AddWithValue("@Gender", patient.Gender);
                cmd.Parameters.AddWithValue("@DOB", patient.DOB);
                cmd.Parameters.AddWithValue("@ContactNumber", patient.ContactNumber);
                cmd.Parameters.AddWithValue("@Email", patient.Email);
                cmd.Parameters.AddWithValue("@PatientID", patient.PatientID);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        public ActionResult PatientDetails(int id)
        {
            if (Session["RecruiterEmail"] == null)
                return RedirectToAction("Index", "Login");
            var model = new PatientDetails
            {
                Visits = new List<Visit>(),
                Vitals = new List<Vital>()
            };

            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT p.*, v.*, L.* FROM Patients p JOIN Visits v ON p.PatientID = v.PatientID JOIN Vitals L ON L.VisitID = v.VisitID LEFT JOIN Alerts a ON a.VisitID = v.VisitID WHERE p.PatientID = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (model.Patient == null)
                    {
                        model.Patient = new Patient
                        {
                            PatientID = rdr.GetInt32(0),
                            FullName = rdr.GetString(1),
                            DOB = rdr.GetDateTime(2)
                        };
                    }

                    var visitId = rdr.GetInt32(3);
                    if (!model.Visits.Exists(v => v.VisitID == visitId))
                    {
                        model.Visits.Add(new Visit
                        {
                            VisitID = visitId,
                            VisitDate = rdr.GetDateTime(4),
                            DoctorName = rdr.GetString(5)
                        });
                    }
                    model.Vitals.Add(new Vital
                    {
                        VitalID = rdr.GetInt32(6),
                        BloodPressure = rdr.GetString(7),
                        SugarLevel = rdr.GetDecimal(8),
                        PulseRate = rdr.GetInt32(6),
                        RecordedAt = rdr.GetDateTime(4),
                        VisitID = visitId
                    });
                }
                con.Close();
            }
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            if (Session["RecruiterEmail"] == null)
                return RedirectToAction("Index", "Login");

            Patient patient = new Patient();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Patients WHERE PatientID = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    patient.PatientID = id;
                    patient.FullName = rdr["FullName"].ToString();
                    patient.Gender = rdr["Gender"].ToString();
                    patient.DOB = Convert.ToDateTime(rdr["DOB"]);
                    patient.ContactNumber = rdr["ContactNumber"].ToString();
                    patient.Email = rdr["Email"].ToString();
                }
            }
            return View(patient); // ✅ This shows the Delete.cshtml page
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Patients WHERE PatientID = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}