using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace IEM.Areas.IFAMS.Models
{
    public interface IfamsQueryRepository
    {
        IEnumerable<IfamsQueryEntity> GetQueryDetails();
        IEnumerable<IfamsQueryEntity> GetQueryDetails(string barcode, string assetid, string location, string assetcat, string assetsub, string capdate, string ecfno, string invoiceno, string assetstatus, string assetvalue, string pono, string cbfno);
        IEnumerable<Assetcategory> Assetcategory();
        IEnumerable<Assetsubcategory> Assetsubcategory();
        IEnumerable<AssetStatusName> AssetStatusName();
        IEnumerable<GetLocation> GetLocation();

        //IFAMES uat

        DataSet GetTrackerScreenData();
    }
}