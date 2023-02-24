using IEM.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IEM.Areas.FLEXISPEND.Models
{
    public class FSRepository
    {
        dbLib db = new dbLib();

        //Load Document Type's
        public List<DocType> MasterDocumentType(string _UsrId)
        {
            try
            {
                List<DocType> result = new List<DocType>();
                DataSet ds = db.GetMaster("1", _UsrId);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        result.Add(new DocType { Id = dr["Id"].ToString(), Text = dr["Combo"].ToString() });
                    }
                    return result;
                }
                else { return null; }
            }
            catch { return null; }
        }

        //Load Document Type's
        public List<DocType> LoadDocumentType(string _UsrId)
        {
            try
            {
                List<DocType> result = new List<DocType>();
                DataTable dt = GetDocType("0");
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        result.Add(new DocType { Id = dr["Id"].ToString(), Text = dr["Combo"].ToString() });
                    }
                    return result;
                }
                else { return null; }
            }
            catch { return null; }
        }

        //Pullout Receipt Document Type Dropdown        
        public DataTable GetDocType(string ViewType)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Id", typeof(string));
                dt.Columns.Add("Combo", typeof(string));
                if (ViewType != "1")
                    dt.Rows.Add(new object[] { "", "-Select One-" });
                dt.Rows.Add(new object[] { "E", "ECF/ARF" });
                //dt.Rows.Add(new object[] { "A", "ARF" });
                dt.Rows.Add(new object[] { "C", "Cheque" });
                dt.Rows.Add(new object[] { "M", "Memo" });
                dt.Rows.Add(new object[] { "L", "Letter" });
                dt.Rows.Add(new object[] { "O", "Others" });
                return dt;
            }
            catch { return null; }
        }

        //Courier details Auto Complete
        public List<string> GetAutoCompleteCourier(string txt, string _UsrId)
        {
            try
            {
                List<string> result = new List<string>();
                DataSet ds = db.GetAutoComplete(txt, "1", _UsrId);
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

        //Employee Auto Complete
        public List<string> GetAutoCompleteEmployee(string txt, string _UsrId)
        {
            try
            {
                List<string> result = new List<string>();
                DataSet ds = db.GetAutoComplete(txt, "2", _UsrId);
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

        //Employee Code Auto Complete
        public List<string> GetAutoCompleteEmployeeCode(string txt, string _UsrId)
        {
            try
            {
                List<string> result = new List<string>();
                DataSet ds = db.GetAutoComplete(txt, "3", _UsrId);
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
            catch
            {
                return null;
            }
        }


        public List<string> GetAutoCompleteGLCode(string txt, string _UsrId)
        {
            try
            {
                List<string> result = new List<string>();
                DataSet ds = db.GetAutoComplete(txt, "23", _UsrId);
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
            catch
            {
                return null;
            }
        }
        
        //Supplier Name Auto Complete
        public List<string> GetAutoCompleteSupplier(string txt, string _UsrId)
        {
            try
            {
                List<string> result = new List<string>();
                DataSet ds = db.GetAutoComplete(txt, "4", _UsrId);
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

        //Supplier Code Auto Complete
        public List<string> GetAutoCompleteSupplierCode(string txt, string _UsrId)
        {
            try
            {
                List<string> result = new List<string>();
                DataSet ds = db.GetAutoComplete(txt, "5", _UsrId);
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

        //Supplier Code & Name Combained Auto Complete
        public List<string> GetAutoCompleteSupplierAll(string txt, string _UsrId)
        {
            try
            {
                List<string> result = new List<string>();
                DataSet ds = db.GetAutoComplete(txt, "10", _UsrId);
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

        //Branch Auto Complete
        public List<string> GetAutoCompleteBranch(string txt, string _UsrId)
        {
            try
            {
                List<string> result = new List<string>();
                DataSet ds = db.GetAutoComplete(txt, "9", _UsrId);
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

        //Branch Auto Complete
        public List<string> GetAutoCompleteBranchPouch(string txt, string _UsrId)
        {
            try
            {
                List<string> result = new List<string>();
                DataSet ds = db.GetAutoComplete(txt, "22", _UsrId);
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

        //load Bank Master
        public List<DocType> LoadBankMaster(string _UsrId)
        {
            try
            {
                List<DocType> result = new List<DocType>();
                DataSet ds = db.GetMaster("11", _UsrId);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        result.Add(new DocType { Id = dr["Id"].ToString(), Text = dr["Combo"].ToString() });
                    }
                    return result;
                }
                else { return null; }
            }
            catch { return null; }
        }


        public List<DocType> LoadPayModeMaster(string _UsrId)
        {
            try
            {
                List<DocType> result = new List<DocType>();
                DataSet ds = db.GetMaster("50", _UsrId);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        result.Add(new DocType { Id = dr["Id"].ToString(), Text = dr["Combo"].ToString() });
                    }
                    return result;
                }
                else { return null; }
            }
            catch { return null; }
        }


        //DocType For Check List Master.
        public List<ParentDetails> GetDocumentTypeCheckList(DataTable dtParent, DataTable dtChild)
        {
            List<ParentDetails> result = new List<ParentDetails>();
            DataTable dt = null;
            if (dtParent != null && dtChild != null)
            {
                for (int i = 0; i < dtParent.Rows.Count; i++)
                {
                    ParentDetails parent = new ParentDetails();
                    parent.GroupName = dtParent.Rows[i]["Combo"].ToString();
                    dt = LoadChildList(dtChild, dtParent.Rows[i]["Id"].ToString());

                    List<ChildDetails> childArray = new List<ChildDetails>();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow cdr in dt.Rows)
                        {
                            ChildDetails child = new ChildDetails();
                            child.Id = cdr["Id"].ToString();
                            child.ParentId = cdr["ParentId"].ToString();
                            child.Name = cdr["Combo"].ToString();
                            childArray.Add(child);
                        }
                    }
                    parent.ChildGroup = childArray;
                    result.Add(parent);
                }
            }
            return result;
        }
        public DataTable LoadChildList(DataTable dt, string FilterId)
        {
            DataRow[] dr = dt.Select("ParentId = " + FilterId);
            DataTable tdt = dt.Copy();
            tdt.Rows.Clear();
            foreach (DataRow tdr in dr)
            {
                tdt.ImportRow(tdr);
            }
            return tdt;
        }

        //Get ClaimType Dropdown 
        //0-Select all
        //1-WIth out select all
        public List<DocType> LoadClaimType(string ViewType)
        {
            try
            {
                List<DocType> result = new List<DocType>();
                DataTable dt = GetClaimType(ViewType);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        result.Add(new DocType { Id = dr["Id"].ToString(), Text = dr["Combo"].ToString() });
                    }
                    return result;
                }
                else { return null; }
            }
            catch { return null; }
        }

        public DataTable GetClaimType(string ViewType)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Id", typeof(string));
                dt.Columns.Add("Combo", typeof(string));
                if (ViewType != "1")
                    dt.Rows.Add(new object[] { "", "-Select All-" });
                dt.Rows.Add(new object[] { "E", "Employee" });
                dt.Rows.Add(new object[] { "S", "Supplier" });
                return dt;
            }
            catch { return null; }
        }

        //Load Payment Mode Type
        public List<DocType> MasterPayMode(string _UsrId)
        {
            try
            {
                List<DocType> result = new List<DocType>();
                DataSet ds = db.GetMaster("12", _UsrId);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        result.Add(new DocType { Id = dr["Id"].ToString(), Text = dr["Combo"].ToString() });
                    }
                    return result;
                }
                else { return null; }
            }
            catch { return null; }
        }

        //Load Payment Mode Type
        public List<DocType> MasterAdvanceMode(string _UsrId)
        {
            try
            {
                List<DocType> result = new List<DocType>();
                DataSet ds = db.GetMaster("19", _UsrId);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        result.Add(new DocType { Id = dr["Id"].ToString(), Text = dr["Combo"].ToString() });
                    }
                    return result;
                }
                else { return null; }
            }
            catch { return null; }
        }

        //Find the Employee Role --& identify Checker
        public string GetEmployeeRole(string _UsrId)
        {
            try
            {
                DataSet ds = db.GetEmployeeRole(_UsrId);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    var q = from myRow in ds.Tables[0].AsEnumerable()
                            where myRow.Field<string>("Role") == "AUDIT MAKER"
                            select new DocType
                            {
                                Text = myRow.Field<string>("Role")
                            };
                    return q.ToList().Count > 0 ? "N" : "Y";
                }
                else { return "Y"; }
            }
            catch { return "Y"; }
        }

        //Find the Employee Role --& identify Checker
        public string GetEmployeeRoleChecker(string _UsrId)
        {
            try
            {
                DataSet ds = db.GetEmployeeRole(_UsrId);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    var q = from myRow in ds.Tables[0].AsEnumerable()
                            where myRow.Field<string>("Role").ToUpper().Replace(" ", "") == "AUDITCHECKER"
                            select new DocType
                            {
                                Text = myRow.Field<string>("Role")
                            };
                    return q.ToList().Count > 0 ? "Y" : "N";
                }
                else { return "Y"; }
            }
            catch { return "Y"; }
        }

        //Load Amort cycle
        public List<DocType> LoadAmortCycle(string _UsrId)
        {
            try
            {
                List<DocType> result = new List<DocType>();
                DataSet ds = db.GetMaster("23", _UsrId);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        result.Add(new DocType { Id = dr["Id"].ToString(), Text = dr["Combo"].ToString() });
                    }
                    return result;
                }
                else { return null; }
            }
            catch { return null; }
        }

        //[GL UPLOADS]Load Upload Type
        public List<DocType> LoadGLUploadTypes(string _UsrId)
        {
            try
            {
                List<DocType> result = new List<DocType>();
                DataSet ds = db.GetMaster("28", _UsrId);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        result.Add(new DocType { Id = dr["Id"].ToString(), Text = dr["Combo"].ToString() });
                    }
                    return result;
                }
                else { return null; }
            }
            catch { return null; }
        }

        //[GL UPLOADS]Load Upload Type
        public List<DocType> LoadGLUploadTypesCancellation(string UploadDate, string UploadTypeId, string ViewType, string _UsrId)
        {
            try
            {
                List<DocType> result = new List<DocType>();
                DataSet ds = db.GetGLUploadsCancellation(UploadDate, UploadTypeId, ViewType, _UsrId);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        result.Add(new DocType { Id = dr["Id"].ToString(), Text = dr["Combo"].ToString() });
                    }
                    return result;
                }
                else { return null; }
            }
            catch { return null; }
        }

        //Load Receipt Source Type's
        public List<DocType> LoadReceiptSource(string _UsrId)
        {
            try
            {
                List<DocType> result = new List<DocType>();
                DataSet ds = db.GetMaster("29", _UsrId);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        result.Add(new DocType { Id = dr["Id"].ToString(), Text = dr["Combo"].ToString() });
                    }
                    return result;
                }
                else { return null; }
            }
            catch { return null; }
        }

        //Load Receipt Mode Type
        public List<DocType> LoadReceiptMode(string _UsrId)
        {
            try
            {
                List<DocType> result = new List<DocType>();
                DataSet ds = db.GetMaster("30", _UsrId);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        result.Add(new DocType { Id = dr["Id"].ToString(), Text = dr["Combo"].ToString() });
                    }
                    return result;
                }
                else { return null; }
            }
            catch { return null; }
        }

        //GL A/C Number
        public List<string> LoadGLACNumber(string txt, string _UsrId)
        {
            try
            {
                List<string> result = new List<string>();
                DataSet ds = db.GetAutoComplete(txt, "21", _UsrId);
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

        //Supplier Receipt Auto Complete
        public List<string> GetAutoReceiptSupplier(string txt, string _UsrId)
        {
            try
            {
                List<string> result = new List<string>();
                DataSet ds = db.GetAutoComplete(txt, "10", _UsrId);
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

        //Employee Receipt Auto Complete
        public List<string> GetAutoReceiptEmployee(string txt, string _UsrId)
        {
            try
            {
                List<string> result = new List<string>();
                DataSet ds = db.GetAutoComplete(txt, "13", _UsrId);
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

        //Activity Type
        public List<DocType> LoadActivityTypes(string _UsrId)
        {
            try
            {
                List<DocType> result = new List<DocType>();
                DataSet ds = db.GetMaster("31", _UsrId);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    result.Add(new DocType { Id = "All", Text = "All" });

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        result.Add(new DocType { Id = dr["Id"].ToString(), Text = dr["Combo"].ToString() });
                    }
                    return result;
                }
                else { return null; }
            }
            catch { return null; }
        }

        //Provision Mapping
        public List<string> GetAutoCompleteFCBusiness(string txt, string _UsrId)
        {
            try
            {
                List<string> result = new List<string>();
                DataSet ds = db.GetAutoComplete("", "14", txt, _UsrId);
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

        public List<string> GetAutoCompleteCCUnit(string txt, string _UsrId)
        {
            try
            {
                List<string> result = new List<string>();
                DataSet ds = db.GetAutoComplete("", "15", txt, _UsrId);
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

        //Load Document Type's
        public List<DocType> LoadEFTPayMode(string _UsrId)
        {
            try
            {
                List<DocType> result = new List<DocType>();
                DataSet ds = db.GetMaster("42", _UsrId);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        result.Add(new DocType { Id = dr["Id"].ToString(), Text = dr["Combo"].ToString() });
                    }
                    return result;
                }
                else { return null; }
            }
            catch { return null; }
        }

        #region Poobalan Code
        //Product Code Auto Complete
        public List<string> GetAutoCompleteProductCode(string txt, string _UsrId)
        {
            try
            {
                List<string> result = new List<string>();
                DataSet ds = db.GetAutoComplete(txt, "7", _UsrId);
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

        //Location Code Auto Complete
        public List<string> GetAutoCompleteLocationCode(string txt, string Code, string _UsrId)
        {
            try
            {
                List<string> result = new List<string>();
                DataSet ds = db.GetAutoComplete(txt, "8", Code, _UsrId);
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
        #endregion


        public List<HSNCode> LoadHSNCode(string _UsrId)
        {
            try
            {
                List<HSNCode> result = new List<HSNCode>();
                DataSet ds = db.GetGSTMaster("3", "", _UsrId);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        result.Add(new HSNCode { Id = dr["Id"].ToString(), Text = dr["Value"].ToString(), Desc = dr["Value1"].ToString() });
                    }
                    return result;
                }
                else { return null; }
            }
            catch { return null; }
        }
    }

    public class DocType
    {
        public string Id { get; set; }
        public string Text { get; set; }
    }

    public class HSNCode
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Desc { get; set; }
    }
}