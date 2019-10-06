using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace VMS.Models
{
    public class TrainerReq
    {
        [DisplayName("Requirement ID")]
        public int ReqId { get; set; }
        [DisplayName("Description")]
        public string ReqDesc { get; set; }
        [DisplayName("Cut-Off Date")]
        public DateTime CutOffDate { get; set; }
    }
}