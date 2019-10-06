using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VMS.Models;
using VMS.DAL;
using System.IO;

namespace VMS.Controllers
{
    [Authorize]
    public class AppController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            ViewBag.errMsg = "";
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Home(Login login)
        {
            string userType;
            userType = DBServices.GetUserType(login);
            if (userType.Equals("A") || userType.Equals("V"))
            {
                Session["userType"] = userType;
                Session["userId"] = login.UserId;
                FormsAuthentication.SetAuthCookie(login.UserId, true);
                List<TrainerReq> trainerReqs = DBServices.GetTrainerReqs();
                return View("ViewTrainerReqs", trainerReqs);
            }
            else if (userType.Equals("S"))
            {
                Session["userType"] = userType;
                Session["userId"] = login.UserId;
                FormsAuthentication.SetAuthCookie(login.UserId, true);
                List<TrainerProfile> trainerProfiles = DBServices.GetTrainerProfilesSME(login.UserId);
                return View("ViewTrainerProfilesSME", trainerProfiles);
            }
            else
            {
                ViewBag.ErrMsg = "Invalid Credentials";
                return View("Login");
            }
        }

        [HttpGet]
        [Route("TrainerReqForm/{id?}")]
        public ActionResult TrainerReqForm(string id)
        {
            int reqId = Convert.ToInt32(id);
            if (reqId == 0)
            {
                return View();
            }
            else
            {
                TrainerReq trainerReq = DBServices.GetTrainerReq(reqId);
                return View(trainerReq);
            }
        }

        [HttpPost]
        public ActionResult TrainerReq(TrainerReq trainerReq)
        {
            int statusCode;
            if (trainerReq.ReqId == 0)
            {
                statusCode = DBServices.AddTrainerReq(trainerReq);

            }
            else
            {
                statusCode = DBServices.EditTrainerReq(trainerReq);
            }
            if (statusCode == 0)
            {
                @ViewBag.ErrTryAgain = "Try Again";
                return View("TrainerReqForm", trainerReq);
            }
            else
            {
                List<TrainerReq> trainerReqs = DBServices.GetTrainerReqs();
                return View("ViewTrainerReqs", trainerReqs);
            }
        }


        [HttpGet]
        public ActionResult ViewProfiles(string id)
        {
            int reqId = Convert.ToInt32(id);
            string userId = Session["userId"].ToString();
            if (Session["userType"].ToString() == "A")
            {
                List<TrainerProfile> trainerProfiles = DBServices.GetTrainerProfilesAdmin(reqId);
                return View("ViewTrainerProfilesAdmin", trainerProfiles);
            }
            else
            {
                ViewBag.reqId = reqId;
                List<TrainerProfile> trainerProfiles = DBServices.GetTrainerProfilesVendor(reqId, userId);
                return View("ViewTrainerProfilesVendor", trainerProfiles);
            }
        }

        [HttpGet]
        [Route("AddTrainerProfile")]
        public ActionResult AddTrainerProfile(string id)
        {
            TrainerProfile trainerProfile = new TrainerProfile();
            trainerProfile.ReqId = Convert.ToInt32(id);
            return View(trainerProfile);
        }

        [HttpPost]
        public ActionResult TrainerProfile(TrainerProfile trainerProfile)
        {
            trainerProfile.VendorId = Session["userId"].ToString();
            int statusCode = DBServices.AddTrainerProfile(trainerProfile);
            if (statusCode == 0)
            {
                @ViewBag.ErrTryAgain = "Try Again";
                return View("AddTrainerProfile", trainerProfile);
            }
            else
            {
                string path, directory, fileName;
                path = Server.MapPath("~");
                path = Directory.GetParent(path).FullName;
                if (trainerProfile.Profile != null && trainerProfile.Profile.ContentLength > 0)
                {
                    directory = path + @"\Profiles\";
                    fileName = trainerProfile.ProfileId + ".docx";
                    trainerProfile.Profile.SaveAs(Path.Combine(directory, fileName));

                }
                List<TrainerReq> trainerReqs = DBServices.GetTrainerReqs();
                return View("ViewTrainerReqs", trainerReqs);
            }

        }

        [HttpGet]
        [Route("SetSMEForm")]
        public ActionResult SetSMEForm(string id)
        {
            int profileId = Convert.ToInt32(id);
            TrainerProfile trainerProfile = DBServices.GetTrainerProfile(profileId);
            trainerProfile.SMEs = DBServices.GetSMEs();
            return View(trainerProfile);
        }

        [HttpPost]
        public ActionResult SetSME(TrainerProfile trainerProfile)
        {
            int statusCode = DBServices.SetSME(trainerProfile);
            if (statusCode == 0)
            {
                @ViewBag.ErrTryAgain = "Try Again";
                trainerProfile.SMEs = DBServices.GetSMEs();
                return View("SetSMEForm", trainerProfile);
            }
            else
            {
                List<TrainerReq> trainerReqs = DBServices.GetTrainerReqs();
                return View("ViewTrainerReqs", trainerReqs);
            }
        }
        [HttpGet]
        [Route("SetFeedbackForm")]
        public ActionResult SetFeedbackForm(string id)
        {
            int profileId = Convert.ToInt32(id);
            TrainerProfile trainerProfile = DBServices.GetTrainerProfile(profileId);
            return View(trainerProfile);
        }


        [HttpPost]
        public ActionResult SetFeedback(TrainerProfile trainerProfile)
        {
            int statusCode = DBServices.SetFeedback(trainerProfile);
            if (statusCode == 0)
            {
                @ViewBag.ErrTryAgain = "Try Again";
                return View("SetFeedbackForm", trainerProfile);
            }
            else
            {
                List<TrainerProfile> trainerProfiles = DBServices.GetTrainerProfilesSME(Session["userId"].ToString());
                return View("ViewTrainerProfilesSME", trainerProfiles);
            }
        }
    }
}