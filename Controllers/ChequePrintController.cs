using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using System.Data;
using IEM.Helper;
using IEM.Areas.FLEXISPEND.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXISPEND.Controllers
{
    [NoDirectAccess]
    public class ChequePrintController : ApiController
    {

        #region Declaration
        dbLib db = new dbLib();
        proLib plib = new proLib();
        DataTable dt = new DataTable();
        #endregion

        [HttpPost]
        public List<ChequeResult> FetchCheque(ChequeSearch chequesearch)
        {
            try
            {
                List<ChequeResult> result = new List<ChequeResult>();
                DataSet ds = db.GetChequePrint(chequesearch.BatchNo, chequesearch.PVNoFrom, chequesearch.PVNoTo, chequesearch.PayDateFrom, chequesearch.PayDateTo, chequesearch.ClaimType, (chequesearch.PVAmountFrom == null || chequesearch.PVAmountFrom.Trim() == "") ? "0" : chequesearch.PVAmountFrom, (chequesearch.PVAmountTo == null || chequesearch.PVAmountTo.Trim() == "") ? "0" : chequesearch.PVAmountTo, chequesearch.EmpNameId, chequesearch.EmpCodeId, chequesearch.SuppCodeId, chequesearch.SuppNameId, chequesearch.UId);
                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                        result.Add(new ChequeResult()
                        {
                            ClaimType = dr["ClaimType"].ToString(),
                            EmployeeSupplierCode = dr["EmployeeSupplierCode"].ToString(),
                            EmployeeSupplierId = dr["EmployeeSupplierId"].ToString(),
                            EmployeeSupplierName = dr["EmployeeSupplierName"].ToString(),
                            PaymentBatchNo = dr["PaymentBatchNo"].ToString(),
                            PayMode = dr["PayMode"].ToString(),
                            PVAmount = dr["PVAmount"].ToString(),
                            PVDate = dr["PVDate"].ToString(),
                            PvId = dr["PvId"].ToString(),
                            PVNo = dr["PVNo"].ToString(),
                            SlNo = dr["SlNo"].ToString()
                        });
                    return result;
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public ChequePrintResult SetChequePrinting(ChequePrinting chequeprint)
        {
            ChequePrintResult result = new ChequePrintResult();
            try
            {
                List<ChequeList> list = new List<ChequeList>();
                string Date = "", DateFormat = "";
                DataSet ds = db.SetChequePrint(chequeprint.ChqDetails, chequeprint.BankId, chequeprint.ChqBookNo, chequeprint.ChqNoFrom, chequeprint.ChqNoTo, chequeprint.ChqCount, chequeprint.UId);
                if (ds != null)
                {
                    result.Message = ds.Tables[0].Rows[0]["Message"].ToString();
                    result.Clear = ds.Tables[0].Rows[0]["Clear"].ToString();
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        Date = row["CHQDate"].ToString();
                        DateFormat = string.Format("{0} {1} {2} {3} {4} {5} {6} {7}", Date[0], Date[1], Date[3], Date[4], Date[6], Date[7], Date[8], Date[9]);
                        list.Add(new ChequeList() { CHQDate = DateFormat, Beneficiary = row["Beneficiary"].ToString(), AmountInWords = row["AmountInWords"].ToString(), PvAmount = row["PvAmount"].ToString(), PvNo = row["PvNo"].ToString() });
                    }
                        
                    result.chequelist = list;
                    return result;
                }
                else { return result; }
            }
            catch
            {
                return result;
            }
        }

        [HttpPost]
        public List<AutoSuggesion> SupplierAutoComplete(ChequeSearch chequesearch)
        {
            try
            {
                List<AutoSuggesion> result = new List<AutoSuggesion>();
                DataSet ds = db.GetAutoComplete(chequesearch.SuppNameId, "4", chequesearch.UId);
                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        result.Add(new AutoSuggesion()
                        {
                            Id = dr["Id"].ToString(),
                            Combo = dr["Text"].ToString(),
                        });
                    return result;
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public List<AutoSuggesion> SupplierCodeAutoComplete(ChequeSearch chequesearch)
        {
            try
            {
                List<AutoSuggesion> result = new List<AutoSuggesion>();
                DataSet ds = db.GetAutoComplete(chequesearch.SuppNameId, "5", chequesearch.UId);
                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        result.Add(new AutoSuggesion()
                        {
                            Id = dr["Id"].ToString(),
                            Combo = dr["Text"].ToString(),
                        });
                    return result;
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public List<AutoSuggesion> EmployeeAutoComplete(ChequeSearch chequesearch)
        {
            try
            {
                List<AutoSuggesion> result = new List<AutoSuggesion>();
                DataSet ds = db.GetAutoComplete(chequesearch.SuppNameId, "2", chequesearch.UId);
                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        result.Add(new AutoSuggesion()
                        {
                            Id = dr["Id"].ToString(),
                            Combo = dr["Text"].ToString(),
                        });
                    return result;
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public List<AutoSuggesion> EmployeeCodeAutoComplete(ChequeSearch chequesearch)
        {
            try
            {
                List<AutoSuggesion> result = new List<AutoSuggesion>();
                DataSet ds = db.GetAutoComplete(chequesearch.SuppNameId, "3", chequesearch.UId);
                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        result.Add(new AutoSuggesion()
                        {
                            Id = dr["Id"].ToString(),
                            Combo = dr["Text"].ToString(),
                        });
                    return result;
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public List<AutoSuggesion> BankAutoComplete(ChequeSearch chequesearch)
        {
            try
            {
                List<AutoSuggesion> result = new List<AutoSuggesion>();
                DataSet ds = db.GetMaster("11", chequesearch.UId);
                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        result.Add(new AutoSuggesion()
                        {
                            Id = dr["Id"].ToString(),
                            Combo = dr["Combo"].ToString(),
                        });
                    return result;
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }
    }
}
