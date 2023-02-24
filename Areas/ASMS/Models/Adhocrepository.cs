using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEM.Areas.ASMS.Models
{


    public interface IAdhocrepository
    {
        IEnumerable<adhoc_model> getadhoclist();
        IEnumerable<adhoc_model> getadhoclist(string filter_code, string filter_name);
        adhoc_model GetAdhocById(int ClassificationId);
        string Deleteadhoc(string ClassificationId);
        string getstategstcode(string stateid);
        string Updateadhoc(adhoc_model Classifications);
        IEnumerable<adhoc_model> GetAdhochistoryById(int ClassificationId);
        
    }
}