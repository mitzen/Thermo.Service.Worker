using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using AzCloudApp.MessageProcessor.Core.Thermo.DataStore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service.ThermoDataModel.Models;
using Thermo.Web.WebApi.Model;
using Thermo.Web.WebApi.Model.MobileAPIModel;

namespace Thermo.Web.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class MobileAPIController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly ThermoDataContext db;

        public MobileAPIController(IOptions<AppSettings> appSettings, ThermoDataContext thermoDataContext)
        {
            _appSettings = appSettings.Value;
            db = thermoDataContext;
        }

        [HttpPost("GetAttendanceRecords")]
        public dynamic GetAttendanceRecords([FromBody] QueryAttendanceRecordRequest model)
        {
            try
            {
                List<dynamic> records = new List<dynamic>();
                DateTime d1 = DateTime.Parse(model.QueryDate.ToString());
                DateTime d2 = DateTime.Parse(model.QueryDate.ToString()).AddDays(1);
                var deList = (from d in db.Company_Device where d.CompanyId == model.CompanyId select new { d.DeviceId }).ToList();
                var company = db.Company.Where(x => x.Nid == model.CompanyId).FirstOrDefault();
                var devices = deList.Distinct();
                foreach (var d in devices)
                {
                    /*  if (model.NextId == 0)
                      {
                          if (company.MaskRequired)
                          {
                              var temp = (from x in db.AttendanceRecord
                                          where x.DeviceId.Trim() == d.DeviceId.Trim() && x.TimeStamp > d1 && x.TimeStamp <= d2
                                          && ((x.PersonId.Trim().Length > 0) ||
                                          (x.PersonId.Trim().Length == 0 &&
                                          (x.BodyTemperature > company.MaxTempThreshold || x.BodyTemperature < company.MinTempThreshold
                                          || x.Respirator == 0)))
                                          select x).OrderByDescending(x => x.TimeStamp).ToList();
                              records.AddRange(temp);
                          }
                          else {
                              var temp = (from x in db.AttendanceRecord
                                          where x.DeviceId.Trim() == d.DeviceId.Trim() && x.TimeStamp > d1 && x.TimeStamp <= d2
                                          && ((x.PersonId.Trim().Length > 0) ||
                                          (x.PersonId.Trim().Length == 0 &&
                                          (x.BodyTemperature > company.MaxTempThreshold || x.BodyTemperature < company.MinTempThreshold)))
                                          select x).OrderByDescending(x => x.TimeStamp).ToList();
                              records.AddRange(temp);
                          }
                      }*/
                    //15.08.2020 Allow all records to be shown
                    var temp = (from x in db.AttendanceRecord
                                where x.DeviceId.Trim() == d.DeviceId.Trim() && x.TimeStamp > d1 && x.TimeStamp <= d2
                                select x).OrderByDescending(x => x.TimeStamp).ToList();
                    records.AddRange(temp);

                }

                return records;
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("GetUserAttendanceRecord")]
        public dynamic GetUserAttendanceRecord([FromBody] QueryAttendanceRecordRequest model)
        {
            try
            {
                List<dynamic> records = new List<dynamic>();
                DateTime d1 = DateTime.Parse(model.QueryDate.ToString());
                DateTime d2 = DateTime.Parse(model.QueryDate.ToString()).AddDays(1);
                var deList = (from d in db.Company_Device where d.CompanyId == model.CompanyId select new { d.DeviceId }).ToList();
                var devices = deList.Distinct();

                foreach (var d in devices)
                {

                   var temp = db.AttendanceRecord.Where(x => x.DeviceId.Trim() == d.DeviceId.Trim() && x.TimeStamp > d1 && x.TimeStamp <= d2 && x.PersonId.Trim() == model.PersonId.Trim());
                   records.AddRange(temp);
                    
                }
                return records;
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("updateFirebase")]
        public dynamic UpdateFirebase([FromBody] UpdateFirebase model)
        {
            try
            {
                var u = (from p in db.Users
                                 where p.Username == model.Username
                                 select p).SingleOrDefault();
                List<string> tokenList = new List<string>();
                if (u.FirebaseToken != null && u.FirebaseToken.Trim().Length > 0) {
                    tokenList = u.FirebaseToken.Split(' ').ToList();
                }
                bool alreadyExists = tokenList.Any(x => x == model.FirebaseToken);
                if (!alreadyExists && model.UpdateType != "LOGOUT")
                {
                    tokenList.Add(model.FirebaseToken);
                }
                else if (model.UpdateType == "LOGOUT" && alreadyExists) {
                    tokenList.Remove(model.FirebaseToken);
                }

                u.FirebaseToken = String.Join(" ", tokenList.ToArray());
                //  u.FirebaseToken = model.UpdateType == "LOGOUT" ? "": model.FirebaseToken;

                db.SaveChanges();
                return null;
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("GetCompanySettings")]
        public dynamic GetCompanySettings([FromBody] UpdateFirebase model)
        {
            try
            {
                var user = db.Users.Where(x => x.Username == model.Username).FirstOrDefault();
                var record = db.Company.Where(x => x.Nid ==user.CompanyId).FirstOrDefault();
                return record;
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
