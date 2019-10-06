using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VMS.Models
{
    public class TrainerReq
    {
        public int ReqId { get; set; }
        public string ReqDesc { get; set; }
        public DateTime CutOffDate { get; set; }
    }
}