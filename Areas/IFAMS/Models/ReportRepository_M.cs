using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace IEM.Areas.IFAMS.Models
{
    public interface ReportRepository_M 
    {
        IEnumerable<AssetReportModel> GetSalereport();
        IEnumerable<AssetReportModel> GetTransreport();
        IEnumerable<AssetReportModel> GetWAreport();
        IEnumerable<AssetReportModel> GetACCreport();
        IEnumerable<AssetReportModel> GetPIDreport();
      //  IEnumerable<AssetReportModel> GetFAreport();
     //   IEnumerable<AssetReportModel> GetFAreport(string depfd, string deptd);
        DataTable GetFAreport(string depfd, string deptd ,string dummy );
        IEnumerable<AssetReportModel> FAAutobranch(string term);
        IEnumerable<AssetReportModel> Autoassetid(string term);
        IEnumerable<AssetReportModel> WOAAutoserialno(string term);
        IEnumerable<AssetReportModel> TOAAutoassetid(string term);
        IEnumerable<AssetReportModel> TOAAutoserialno(string term);
        IEnumerable<AssetReportModel> SOAAutoassetid(string term);
        IEnumerable<AssetReportModel> SOAAutoserialno(string term);
        IEnumerable<AssetReportModel> SOAAutocheqno(string term);
        IEnumerable<AssetReportModel> SOAAutoinvoiceno(string term);
        IEnumerable<AssetReportModel> ACCAutoassetid(string term);
        IEnumerable<AssetReportModel> ACCAutoOassetcode(string term);
        IEnumerable<AssetReportModel> ACCAutoNassetcode(string term);
        IEnumerable<AssetReportModel> ACCAutoGrpid(string term);
        IEnumerable<AssetReportModel> PHYAutoassetid(string term);
        IEnumerable<AssetReportModel> PHYAutogrpid(string term);
        IEnumerable<AssetReportModel> Autophyid(string term);
        DataSet GetAssetClearingDetl();
        DataSet GetFASummaryDetail();
        string GroupIDupdate();
        IEnumerable<AssetReportModel> GetFAreportDetails(string FINYEAR);
        IEnumerable<AssetReportModel> getfatable(DataTable fatable);
        IEnumerable<AssetReportModel> getFATableNew(DataTable fatable);
        IEnumerable<AssetReportModel> getFATableDetailed(DataTable fatable);
        DataTable getfadep(string fromdate, string tdate , string grpid, string brcode, string glcode);
        DataTable getfadepDetailed(string fromdate, string todate, string grpid, string brcode, string glcode);
        DataTable GetTransferangular();
        void deletefatemp();
        DataTable SearchAssetClear(string DateFrom ,string DateTo);
        
    }
}