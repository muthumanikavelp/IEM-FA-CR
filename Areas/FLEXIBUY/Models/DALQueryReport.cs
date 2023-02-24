using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using IEM.Common;
using System.Globalization;


namespace IEM.Areas.FLEXIBUY.Models
{
    public class DALQueryReport:IQueryReport
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        ErrorLog objErrorLog = new ErrorLog();
        private void Getcon()
        {
            if(con.State==ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }

         public IEnumerable<EFlexiQuery> GetFlexiQuery()
        {
             try
             {
                 List<EFlexiQuery> Lists= new List<EFlexiQuery>();
                 EFlexiQuery list;
                 Getcon();
                 DataTable dt = new DataTable();
                 cmd = new SqlCommand("pr_fb_trn_trackerreport", con);
                 cmd.Parameters.AddWithValue("@action", "getqueryresult");
                 cmd.CommandType = CommandType.StoredProcedure;
                 da = new SqlDataAdapter(cmd);
                 da.Fill(dt);
                 if(dt.Rows.Count>0)
                 {
                     for(int i=0;i<dt.Rows.Count;i++)
                     {
                         list = new EFlexiQuery();
                         list.DocId = Convert.ToInt32(dt.Rows[i]["DocGid"]);
                         list.RefNumber = Convert.ToString(dt.Rows[i]["DocRefNumber"]);
                         list.DocDate = Convert.ToString(dt.Rows[i]["DocDate"]);
                         list.DocAmount = Convert.ToDecimal(dt.Rows[i]["DocAmount"]);
                         list.Description = Convert.ToString(dt.Rows[i]["DocDescription"]);
                         list.Status = Convert.ToString(dt.Rows[i]["Status"]);
                         list.DocType = Convert.ToString(dt.Rows[i]["DocType"]);
                         list.FinalApprovalDate = Convert.ToString(dt.Rows[i]["FinalApprovalDate"]);
                         list.ApprovalPending = Convert.ToString(dt.Rows[i]["PendingWithWhome"]);
                         list.QueueDate = Convert.ToString(dt.Rows[i]["QueueDate"]);
                         list.Aging = Convert.ToString(dt.Rows[i]["Aging"]);
                         Lists.Add(list);
                     }
                 }

                 return Lists;

             }
             catch(Exception ex)
             {
                 objErrorLog.WriteErrorLog("GetFlexiQuery() Method"+ ex.Message.ToString(), ex.ToString());
                 throw ex;
             }
             finally
             {
                 con.Close();
             }
        }


         public List<EFlexiQuery> GetDocList() 
         {
             try
             {
                 List<EFlexiQuery> objdoctype = new List<EFlexiQuery>();
                 EFlexiQuery objModel;
                 Getcon();
                 DataTable dt = new DataTable();
                 SqlCommand cmd = new SqlCommand("pr_fb_trn_trackerreport", con);
                 cmd.Parameters.AddWithValue("@action", "GetDocType");
                 cmd.CommandType = CommandType.StoredProcedure;
                 SqlDataAdapter da = new SqlDataAdapter(cmd);
                 da = new SqlDataAdapter(cmd);
                 da.Fill(dt);
                 for (int i = 0; i < dt.Rows.Count; i++)
                 {
                     objModel = new EFlexiQuery();
                     objModel.doctypeid = Convert.ToInt32(dt.Rows[i]["doctypeid"].ToString());
                     objModel.docname = Convert.ToString(dt.Rows[i]["docname"].ToString());
                     objdoctype.Add(objModel);
                 }
                 return objdoctype;
             }
             catch (Exception ex)
             {
                 throw ex;
             }
             finally
             {
                 con.Close();
             }
         }


         public IEnumerable<EFlexiQuery> GetFlexiQuery_List(string FromDate, string ToDate, string RefNo,string DocNo)
         {
             //  throw new NotImplementedException();

             try
             {
                 string fdate = "";
                 string todate = "";
                 List<EFlexiQuery> Lists = new List<EFlexiQuery>();
                 EFlexiQuery list;
                 Getcon();
                 DataTable dt = new DataTable();
                 if (FromDate != "" && ToDate != "")
                 {
                     DateTime dt1 = new DateTime();
                     dt1 = Convert.ToDateTime(FromDate);
                     fdate = dt1.ToString("yyyy-MM-dd");
                     dt1 = Convert.ToDateTime(ToDate);
                     todate = dt1.ToString("yyyy-MM-dd");
                 }
            
                 cmd = new SqlCommand("pr_fb_trn_trackerreport", con);
                 cmd.Parameters.AddWithValue("@FromDate", fdate.Trim()); 
                 cmd.Parameters.AddWithValue("@ToDate", todate.Trim()); 
                 cmd.Parameters.AddWithValue("@RefNo",RefNo);
                 cmd.Parameters.AddWithValue("@Doctype", DocNo);
                 cmd.Parameters.AddWithValue("@action", "GetQueryFilter");
                 cmd.CommandType = CommandType.StoredProcedure;
                 da = new SqlDataAdapter(cmd);
                 da.Fill(dt);
                 if (dt.Rows.Count > 0)
                 {
                     for (int i = 0; i < dt.Rows.Count; i++)
                     {
                         list = new EFlexiQuery();
                         list.DocId = Convert.ToInt32(dt.Rows[i]["DocGid"]);
                         list.RefNumber = Convert.ToString(dt.Rows[i]["DocRefNumber"]);
                         list.DocDate = Convert.ToString(dt.Rows[i]["DocDate"]);
                         list.DocAmount = Convert.ToDecimal(dt.Rows[i]["DocAmount"]);
                         list.Description = Convert.ToString(dt.Rows[i]["DocDescription"]);
                         list.Status = Convert.ToString(dt.Rows[i]["Status"]);
                         list.DocType = Convert.ToString(dt.Rows[i]["DocType"]);
                         list.FinalApprovalDate = Convert.ToString(dt.Rows[i]["FinalApprovalDate"]);
                         list.ApprovalPending = Convert.ToString(dt.Rows[i]["PendingWithWhome"]);
                         list.QueueDate = Convert.ToString(dt.Rows[i]["QueueDate"]);
                         list.Aging = Convert.ToString(dt.Rows[i]["Aging"]);

                         Lists.Add(list);
                     }
                 }

                 return Lists;
             }
             catch (Exception ex)
             {
                 objErrorLog.WriteErrorLog("GetFlexiQuery_List() Method" + ex.Message.ToString(), ex.ToString());
                 throw ex;
             }
             finally
             {
                 con.Close();
             }
         }


         public DataTable GetQueryExport(string FromDate, string ToDate, string RefNo, string DocNo)
         {
             //  throw new NotImplementedException();

             try
             {
                 string fdate = "";
                 string todate = "";
                 List<EFlexiQuery> Lists = new List<EFlexiQuery>();
                 EFlexiQuery list;
                  
                 if (FromDate != "" && ToDate != "")
                 {
                     DateTime dt1 = new DateTime();
                     dt1 = Convert.ToDateTime(FromDate);
                     fdate = dt1.ToString("yyyy-MM-dd");
                     dt1 = Convert.ToDateTime(ToDate);
                     todate = dt1.ToString("yyyy-MM-dd");
                 }
             
                 Getcon();
                 DataTable dt = new DataTable();
                 cmd = new SqlCommand("pr_fb_trn_trackerreport", con);
                 cmd.Parameters.AddWithValue("@FromDate", fdate.Trim());
                 cmd.Parameters.AddWithValue("@ToDate", todate.Trim());
                 cmd.Parameters.AddWithValue("@RefNo", RefNo);
                 cmd.Parameters.AddWithValue("@Doctype", DocNo);
                 cmd.Parameters.AddWithValue("@action", "GetQueryFilter");
                 cmd.CommandType = CommandType.StoredProcedure;
                 da = new SqlDataAdapter(cmd);
                 da.Fill(dt);
                
                 return dt;
             }
             catch (Exception ex)
             {
                 objErrorLog.WriteErrorLog("GetFlexiQuery_List() Method" + ex.Message.ToString(), ex.ToString());
                 throw ex;
             }
             finally
             {
                 con.Close();
             }
         }



    }
}