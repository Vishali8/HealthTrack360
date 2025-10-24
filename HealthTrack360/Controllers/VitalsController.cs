using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using HealthTrack360.Models;
using System.Data.SqlClient;

namespace HealthTrack360.Controllers
{
    public class VitalsController : Controller
    {
        string conStr = ConfigurationManager.ConnectionStrings["HealthTracker360"].ConnectionString;

        public ActionResult Index(int visitId)
        {
            List<Vital> vitalsList = new List<Vital>();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Vitals WHERE VisitID = @visitId", con);
                cmd.Parameters.AddWithValue("@visitId", visitId);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    vitalsList.Add(new Vital
                    {
                        VitalID = Convert.ToInt32(rdr["VitalID"]),
                        VisitID = visitId,
                        BloodPressure = rdr["BloodPressure"].ToString(),
                        SugarLevel = Convert.ToDecimal(rdr["SugarLevel"]),
                        PulseRate = Convert.ToInt32(rdr["PulseRate"]),
                        RecordedAt = Convert.ToDateTime(rdr["RecordedAt"]),
                        Alerts = new List<Alert>()
                    }); 
                }
            }

            // Load alerts for each vitals entry
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                foreach (var v in vitalsList)
                {
                    SqlCommand cmd = new SqlCommand("SELECT AlertID, AlertType, Description, TriggeredAt FROM Alerts WHERE VitalID = @vitalId", con);
                    cmd.Parameters.AddWithValue("@vitalId", v.VitalID);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        v.Alerts.Add(new Alert
                        {
                            AlertID = Convert.ToInt32(rdr["AlertID"]),
                            AlertType = rdr["AlertType"].ToString(),
                            Description = rdr["Description"].ToString(),
                            TriggeredAt = Convert.ToDateTime(rdr["TriggeredAt"])
                        });
                    }
                    rdr.Close();
                }
            }
            ViewBag.VisitID = visitId;
            return View(vitalsList);
        }


        public ActionResult Create(int visitId)
        {
            ViewBag.VisitID = visitId;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Vital vitals)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(@"INSERT INTO Vitals 
                (VisitID, BloodPressure, SugarLevel, PulseRate, RecordedAt)
                VALUES (@VisitID, @BloodPressure, @SugarLevel, @PulseRate, @RecordedAt)", con);

                cmd.Parameters.AddWithValue("@VisitID", vitals.VisitID);
                cmd.Parameters.AddWithValue("@BloodPressure", vitals.BloodPressure);
                cmd.Parameters.AddWithValue("@SugarLevel", vitals.SugarLevel);
                cmd.Parameters.AddWithValue("@PulseRate", vitals.PulseRate);
                cmd.Parameters.AddWithValue("@RecordedAt", DateTime.Now);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index", "Vitals", new { visitId = vitals.VisitID });
        }

        public ActionResult Edit(int id)
        {
            Vital vitals = new Vital();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Vitals WHERE VitalID = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    vitals.VitalID = id;
                    vitals.VisitID = Convert.ToInt32(rdr["VisitID"]);
                    vitals.BloodPressure = rdr["BloodPressure"].ToString();
                    vitals.SugarLevel = Convert.ToDecimal(rdr["SugarLevel"]);
                    vitals.PulseRate = Convert.ToInt32(rdr["PulseRate"]);
                    vitals.RecordedAt = Convert.ToDateTime(rdr["RecordedAt"]);
                }
            }
            return View(vitals);
        }

        [HttpPost]
        public ActionResult Edit(Vital vitals)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(@"UPDATE Vitals SET 
            BloodPressure = @BloodPressure,
            SugarLevel = @SugarLevel,
            PulseRate = @PulseRate,
            RecordedAt = @RecordedAt
            WHERE VitalID = @VitalID", con);

                cmd.Parameters.AddWithValue("@BloodPressure", vitals.BloodPressure);
                cmd.Parameters.AddWithValue("@SugarLevel", vitals.SugarLevel);
                cmd.Parameters.AddWithValue("@PulseRate", vitals.PulseRate);
                cmd.Parameters.AddWithValue("@RecordedAt", vitals.RecordedAt);
                cmd.Parameters.AddWithValue("@VitalID", vitals.VitalID);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index", new { visitId = vitals.VisitID });
        }


        public ActionResult Delete(int id)
        {
            Vital vitals = new Vital();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Vitals WHERE VitalID = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    vitals.VitalID = id;
                    vitals.VisitID = Convert.ToInt32(rdr["VisitID"]);
                    vitals.BloodPressure = rdr["BloodPressure"].ToString();
                    vitals.SugarLevel = Convert.ToDecimal(rdr["SugarLevel"]);
                    vitals.PulseRate = Convert.ToInt32(rdr["PulseRate"]);
                    vitals.RecordedAt = Convert.ToDateTime(rdr["RecordedAt"]);
                }
            }
            return View(vitals);
        }

        [HttpPost]
        public ActionResult Delete(Vital vitals)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Vitals WHERE VitalID = @id", con);
                cmd.Parameters.AddWithValue("@id", vitals.VitalID);
                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index", new { visitId = vitals.VisitID });
        }

    }
}