using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace IEM.Areas.EOW.Models
{
    public interface RetentionExtendRepository
    {
        IEnumerable<Eow_RetentionExtend> RetentionReleaseGrid();
        IEnumerable<Eow_RetentionExtend> RetentionReleaseGrid(string releasedate,string ecfdate,string ecfno,string invoiceno,string suppcode,string suppname,string extenddate);
        Eow_RetentionExtend GetRetentionExtendRecordById(int invoicegid);
        DataTable GetSerialNo();
        DataTable GetRetentioninformation(int invoicegid);
        DataTable CheckData(int invoivegid);
        string Insertextenddetails(Eow_RetentionExtend Extendinformation);
    }
}