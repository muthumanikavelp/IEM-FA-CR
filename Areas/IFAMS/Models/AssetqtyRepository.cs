using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;


namespace IEM.Areas.IFAMS.Models
{
    public interface AssetqtyRepository_M
    {
        string GetStatusExcel(string assetdata, string valid, string action);
        string UpdateAQty(DataTable exldata, string filename);
        string UpdateASer(DataTable exldata, string filename);
    }
}