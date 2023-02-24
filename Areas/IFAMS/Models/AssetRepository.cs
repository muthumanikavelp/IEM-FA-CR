using IEM.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace IEM.Areas.IFAMS.Models
{
    public class AssetRepository
    {
        dbLib db = new dbLib();
        public List<string> GetAutoCompletevendor(string txt, string _UsrId)
        {
            try
            {
                List<string> result = new List<string>();
                DataSet ds = db.GetVendorAutoComplete(txt, "24", _UsrId);
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        result.Add(string.Format("{0}~{1}", dr[0], dr[1]));
                    }
                }
                ds.Dispose();
                return result;
            }
            catch { return null; }
        }
    }
}