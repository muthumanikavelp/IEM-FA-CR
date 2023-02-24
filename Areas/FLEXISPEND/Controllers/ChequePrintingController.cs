using IEM.Areas.FLEXISPEND.Models;
using IEM.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PrintCheque;
using System.Configuration;
using IEM.App_Start;
namespace IEM.Areas.FLEXISPEND.Controllers
{

    [NoDirectAccess]
    public class ChequePrintingController : Controller
    {
        #region Declaration
        dbLib db = new dbLib();
        proLib plib = new proLib();
        DataTable dt = new DataTable();
        #endregion

        //
        // GET: /FLEXISPEND/ChequePrinting/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult FetchChequePrinting(string BatchNo, string PayDateFrom, string PayDateTo, string PVNoFrom, string PVNoTo, string ClaimType, string PVAmountFrom, string PVAmountTo, string EmpNameId,
        string EmpCodeId, string SuppCodeId, string SuppNameId)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetChequePrint(BatchNo, PVNoFrom, PVNoTo, plib.ConvertDate(PayDateFrom), plib.ConvertDate(PayDateTo), ClaimType, (PVAmountFrom == null || PVAmountFrom.Trim() == "") ? "0" : PVAmountFrom, (PVAmountTo == null || PVAmountTo.Trim() == "") ? "0" : PVAmountTo, EmpNameId, EmpCodeId, SuppCodeId, SuppNameId, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }


        //added by sugumar 2016-07-15
        [HttpPost]
        public JsonResult SetChequePrinting(string ChqDetails, string BankId, string ChqBookNo, string ChqNoFrom, string ChqNoTo, string ChqCount)
        {
            try
            {
                Session["TGRCHQPNT"] = null;
                Session["GetPrinterdet"] = null;
                Session["Bankid"] = null;
                string Data1 = "";
                DataSet ds = db.SetChequePrint(ChqDetails, BankId, ChqBookNo, ChqNoFrom, ChqNoTo, ChqCount, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    if (dt.Rows[0]["Clear"].ToString().ToLower() == "true" || dt.Rows[0]["Clear"].ToString() == "1")
                    {
                        if (ds.Tables.Count > 2 && ds.Tables[1].Rows.Count > 0 && ds.Tables[2].Rows.Count > 0)
                        {
                            DataTable _tmpdata = ds.Tables[1];
                            _tmpdata.TableName = "ChequePrint";
                            Session["TGRCHQPNT"] = _tmpdata;
                            Session["GetPrinterdet"] = ds;
                            Session["Bankid"] = BankId;
                        }
                    }

                    return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }
        // end 

        //[HttpPost]
        //public JsonResult SetChequePrinting(string ChqDetails, string BankId, string ChqBookNo, string ChqNoFrom, string ChqNoTo, string ChqCount)
        //{
        //    try
        //    {
        //        Session["TGRCHQPNT"] = null;
        //        string Data1 = "";
        //        DataSet ds = db.SetChequePrint(ChqDetails, BankId, ChqBookNo, ChqNoFrom, ChqNoTo, ChqCount, plib.LoginUserId);
        //        if (ds != null)
        //        {
        //            dt = ds.Tables[0];
        //            if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

        //            if (dt.Rows[0]["Clear"].ToString().ToLower() == "true" || dt.Rows[0]["Clear"].ToString() == "1")
        //            {
        //                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
        //                {
        //                    DataTable _tmpdata = ds.Tables[1];
        //                    _tmpdata.TableName = "ChequePrint";
        //                    Session["TGRCHQPNT"] = _tmpdata;
        //                }
        //            }

        //            return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
        //        }
        //        else { return null; }
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        //trigger the printer.
        public JsonResult PrintChequeList()
        {
            try
            {
                string file = "";
                file = Path.Combine(Server.MapPath(Url.Content("~/Template")), "chqPrint.xml");

                //if file not exist on directory. create it newly
                if (!System.IO.File.Exists(file))
                {
                    FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter _fsstr = new StreamWriter(fs);
                    _fsstr.WriteLine(string.Empty);
                    _fsstr.Flush(); _fsstr.Close();
                }

                //clear the previous data's
                StreamWriter _tmpstr = new StreamWriter(file, false, System.Text.Encoding.Default);
                _tmpstr.WriteLine(string.Empty);
                _tmpstr.Flush(); _tmpstr.Close();

                //write the printing content to XML File.
                DataTable _downloadingData = Session["TGRCHQPNT"] as DataTable;
                if (_downloadingData != null)
                {
                    DataTable dt = new DataTable();
                    dt = _downloadingData;
                    dt.WriteXml(file);
                }
                try
                {
                    var webClient = new System.Net.WebClient();
                    webClient.DownloadFileAsync(new Uri(file), plib.ChequePrintDownloadUrl + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml");
                }
                catch (Exception ex) { }
            }
            catch { }
            return Json("");
        }

        private static string[] GetFileNames(string path, string filter)
        {
            string[] files = Directory.GetFiles(path, filter);
            for (int i = 0; i < files.Length; i++)
                files[i] = Path.GetFileName(files[i]);
            return files;
        }

        //Print Dialog Box
        //public ActionResult PrintCheque()
        //{
        //    DataTable dtDet = Session["TGRCHQPNT"] as DataTable;
        //    string AmtInWord = "", Date = "", DateFormat = "";
        //    var amtList = new List<string>();

        //    List<ChequePrint> result = new List<ChequePrint>();
        //    if (dtDet != null)
        //    {
        //        for (int i = 0; i < dtDet.Rows.Count; i++)
        //        {
        //            ChequePrint rec = new ChequePrint();
        //            AmtInWord = "";
        //            rec.Amount0 = "";
        //            rec.Amount1 = "";

        //            //AmtInWord = dtDet.Rows[i]["AmountInWords"].ToString();
        //            //try
        //            //{
        //            //    int partLenth = 45;
        //            //    string sentence = AmtInWord;
        //            //    string[] words = sentence.Split(' ');
        //            //    var parts = new Dictionary<int, string>();
        //            //    string part = string.Empty;
        //            //    int partCounter = 0;
        //            //    foreach (var word in words)
        //            //    {
        //            //        if (part.Length + word.Length < partLenth)
        //            //        {
        //            //            part += string.IsNullOrEmpty(part) ? word : " " + word;
        //            //        }
        //            //        else
        //            //        {
        //            //            parts.Add(partCounter, part);
        //            //            part = word;
        //            //            partCounter++;
        //            //        }
        //            //    }
        //            //    parts.Add(partCounter, part);

        //            //    if (parts.Count > 0)
        //            //    {
        //            //        rec.Amount0 = parts[0];
        //            //    }
        //            //    if (parts.Count > 1)
        //            //    {
        //            //        rec.Amount1 = parts[1];
        //            //    }
        //            //}
        //            //catch (Exception ex)
        //            //{
        //            //    rec.Amount0 = AmtInWord;
        //            //    rec.Amount1 = "";
        //            //}

        //            Date = dtDet.Rows[i]["CHQDate"].ToString();
        //            DateFormat = string.Format(" {0}  {1}  {2}  {3}  {4}  {5}  {6}  {7}", Date[0], Date[1],Date[3],Date[4],Date[6],Date[7],Date[8], Date[9]);
        //            rec.Date = DateFormat;

        //            rec.Amount0 = dtDet.Rows[i]["AmountInWords"].ToString();
        //            rec.Pay = dtDet.Rows[i]["Beneficiary"].ToString();
                    
        //            rec.Rupees = dtDet.Rows[i]["PvAmount"].ToString();
        //            rec.PVNo = dtDet.Rows[i]["PvNo"].ToString();
        //            result.Add(rec);
        //        }
        //    }

        //    return View(result);
        //}


        //added by sugumar 2016-07-15
        //Print Dialog Box
        public FileResult PrintCheque()
        {
            DataTable dtDet = Session["TGRCHQPNT"] as DataTable;
            string Bankid = Session["Bankid"].ToString();
            var amtList = new List<string>();
            DataSet ds = Session["GetPrinterdet"] as DataSet;
            string FileLocation = ConfigurationManager.AppSettings["ChequePrintingNew"];
            var picture = ""; var contentType = ""; var fileName = "";
            if (ds != null)
            {
                clsPrintSettings obj;
                if (ds.Tables[2].Rows.Count > 0)
                {
                    obj = new clsPrintSettings(ds);
                    obj.PrintCheque();
                    obj.Close();
                    //obj.PrintBuffer();
                    picture = FileLocation + "cheque.txt";
                    contentType = "text/plain";
                    fileName = "ChequeDetails.txt";

                }
            }
            return File(picture, contentType, fileName);
        }
        // end 

        public List<string> SplitAmount(string AmountInWords, int totalCount)
        {
            var indexes = new List<int>();
            var lastFoundIndex = 0;
            while ((lastFoundIndex = AmountInWords.IndexOf(' ', lastFoundIndex + 1)) != -1)
            {
                indexes.Add(lastFoundIndex);
            }

            int intNum = totalCount;
            int index;
            var newList = new List<string>();
            while ((index = indexes.Where(x => x > intNum - totalCount && x <= intNum).LastOrDefault()) != 0)
            {
                var firstIndex = newList.Count == 0 ? 0 : index;
                var lastIndex = firstIndex + totalCount >= AmountInWords.Length ? AmountInWords.Length - totalCount : intNum;
                newList.Add(AmountInWords.Substring(intNum - totalCount, lastIndex));
                intNum += totalCount;
            }
            return newList;
        }

        //file = plib.ChequePrintDownloadUrl + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml";
        ////try
        ////{
        ////    string filePaths = Server.MapPath(Url.Content("~/Template/ChqPrint"));
        ////    if (Directory.Exists(filePaths))
        ////    {
        ////        Directory.Delete(filePaths, true);
        ////    }

        ////    Directory.CreateDirectory(filePaths);
        ////}
        ////catch (Exception ex){ }
    }
}
