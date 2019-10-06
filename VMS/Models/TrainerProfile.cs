using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace VMS.Models
{
    public class TrainerProfile
    {
        [DisplayName("Profile ID")]
        public int ProfileId { get; set; }
        [DisplayName("Requirement ID")]
        public int ReqId { get; set; }
        [DisplayName("Trainer Name")]
        public string TrainerName { get; set; }
        [DisplayName("Description")]
        public string ReqDesc { get; set; }
        [DisplayName("Vendor")]
        public String VendorId { get; set; }
        [DisplayName("SME")]
        public String SMEId { get; set; }
        public String Feedback { get; set; }
        public int Rating { get; set; }

        public HttpPostedFileBase Profile { get; set; }
        public String getProfileLink()
        {
            return "http://vijayragavan-001-site1.etempurl.com/Profiles/" + ProfileId + ".docx";
        }

        public List<string> SMEs { get; set; }
    }
}