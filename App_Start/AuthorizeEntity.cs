using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEM.App_Start
{
    public class AuthorizeEntity
    {
        public Int64 employee_gid { get; set; }
        public String employee_code { get; set; }
        public string employee_name { get; set; }
        public string role_code { get; set; }
        public Int64 menu_gid { get; set; }
        ArrayList myArrayList = new ArrayList();
    }
}