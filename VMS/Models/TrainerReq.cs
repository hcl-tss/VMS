using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace VMS.Models
{
    public class TrainerReq
    {
        [DisplayName("Requirement ID")]
        public int ReqId { get; set; }
        [DisplayName("Description")]
        public string ReqDesc { get; set; }
        [DisplayName("Cut-Off Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CutOffDate { get; set; }
    }
}