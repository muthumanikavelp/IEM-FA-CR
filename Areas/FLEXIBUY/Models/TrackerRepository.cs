using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace IEM.Areas.FLEXIBUY.Models
{
    public interface TrackerRepository
    {
        DataSet GetTrackerScreenData();
        DataSet GetTrackerScreenDataPO(string docgid = null);
        DataSet GetTrackerScreenDataGRN(string docgid = null);
        DataSet GetTrackerGRNConfirm(string docgid = null);
        DataSet GetTrackerScreenDataCBF(string docgid = null, string doctype = null); 
    } 
}