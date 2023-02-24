using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;


namespace IEM.Areas.FLEXIBUY.Models
{
    public interface IQueryReport
    {
        IEnumerable<EFlexiQuery> GetFlexiQuery();
        List<EFlexiQuery> GetDocList();
        IEnumerable<EFlexiQuery> GetFlexiQuery_List(string FromDate, string ToDate, string RefNo, string DocList);
        DataTable GetQueryExport(string FromDate, string ToDate, string RefNo, string DocList);
    }
}