using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace IEM.Areas.ASMS.Models
{
    public interface SupQueryRepository
    {
        DataSet GetSupplierQuery();
        DataSet GetSupplierQueryReport(string CreatedDateFrom, string CreatedDateTo, int deptgid, string supname, string suppanno);
        DataSet GetDeptList();
        DataSet GetSupplierQuerysearch(string CreatedDateFrom, string CreatedDateTo, int deptgid, String supcode, String supname, string suppanno);
        
    }
}