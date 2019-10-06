using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VMS.Models
{
    public class TrainerProfile
    {
        public int ProfileId { get; set; }
        public int ReqId { get; set; }
        public string TrainerName { get; set; } 
        public String VendorId { get; set; }
        public String SMEId { get; set; }
        public String Feedback { get; set; }
        public int Rating { get; set; }

        public HttpPostedFileBase Profile { get; set; }
    }
}