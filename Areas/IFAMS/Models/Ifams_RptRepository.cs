using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace IEM.Areas.IFAMS.Models
{
    public interface Ifams_RptRepository
    {
        DataSet GetSplitSummary();
        DataSet GetMergeSummary();
        DataSet GetAssetDetails(string LocationCode = null, string capdate = null);
        DataSet GetReductionSummary();
        DataTable GetBatchtally(string DateFrom);
        DataSet GetEntries(string ECFNO);
        DataSet TrantoOgl(string DateFrom, string todate);
        DataSet BasetoTran(string DateFrom, string todate);
        DataSet FATrantoOgl(string DateFrom, string todate);
        DataSet CentralECFReport(string DateFrom, string todate, string dep, Int64 user);
        List<Ifams_RptEntity> GetCTECFReport(string DateFrom, string todate, string dep, Int64 user);
        List<Ifams_RptEntity> Autodepsummary(string term, Int64 user);
    }
}