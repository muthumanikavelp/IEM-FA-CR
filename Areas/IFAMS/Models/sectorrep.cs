using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEM.Areas.IFAMS.Models
{
    public interface sectorrep
    {
        IEnumerable<sectorreport> Getsectorreport();
        IEnumerable<sectorreport> Getsectorexcelreport();
        IEnumerable<sectorreport> Getsectorwisereport();
        IEnumerable<tdsreport> Gettdsreport(string fromdate,string todate);
        //IEnumerable<tdsreport> Getstaxreport();
        IEnumerable<wctreport> getwctrep(string fromdate,string todate);
        IEnumerable<servicetareport> getservicerep(string fromdate, string todate);
        IEnumerable<valreport> getvalrep(string fromdate, string todate);
        
    }
}