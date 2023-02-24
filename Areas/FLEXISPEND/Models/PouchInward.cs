using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEM.Areas.FLEXISPEND.Models
{
    public class PouchInward
    {
        public Int64 SlNo { get; set; }
        public Int32 Id { get; set; }
        public string ReceivedFrom { get; set; }
        public string ReceivedDate { get; set; }
        public string SenderType { get; set; }
        public string SenderTypeCode { get; set; }
        public Int32 SenderId { get; set; }
        public string Sender { get; set; }
        public Int32 CourierId { get; set; }
        public string Courier { get; set; }
        public string AWBNo { get; set; }
        public Int32 Noofdocs { get; set; }
        public string Remarks { get; set; }
        public string KeyedBy { get; set; }
        public Int32 ISEnable { get; set; }
    }
}