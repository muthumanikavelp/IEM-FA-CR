using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Common;

namespace IEM.Areas.MASTERS.Models
{
    public class QueryTool
    {
        public string sqlquery_gid { get; set; }
        public string sqlquery_name { get; set; }
        public string sqlquery_sql { get; set; }
    }
}