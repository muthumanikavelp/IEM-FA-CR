using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace IEM.Areas.IFAMS.Models
{
    public  interface AssetCCRepository
    {
        //Maker
       // IEnumerable<AssetCCModel> GetAccDetail(string id);
        //string getTheUser(string groupCode);
        string GetStatusExcel(string assetdata, string valid, string action);
        string UpdateACC(DataTable exldata, string filename);
    }
}