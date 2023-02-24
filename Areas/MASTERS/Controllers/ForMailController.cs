using IEM.Areas.EOW.Controllers;
using IEM.Areas.MASTERS.Models;
using IEM.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using IEM.Areas.EOW.Models;
using IEM.Helper;
using System.Text.RegularExpressions;

namespace IEM.Areas.MASTERS.Controllers
{
    public class ForMailController : Controller
    {
        //
        // GET: /EOW/ForMailEntity/
        private ForMailRepository objModel;
        CmnFunctions objCmnFunctions = new CmnFunctions();
        CommonIUD objCommonIUD = new CommonIUD();

        dbLib dbTransaction = new dbLib();
        proLib pL = new proLib();

        public ForMailController()
            : this(new ForMailDataModel())
        {

        }
        public ForMailController(ForMailRepository objM)
        {
            objModel = objM;
        }
        [HttpGet]
        public ActionResult Index()
        {
            string tblname = "";
            ForMailEntity Mail = new ForMailEntity();
            Mail.Template = new SelectList(GetTemplate().ToList(), "TemplateId", "TemplateName");
            Mail.Moduledata = new SelectList(objModel.GetModuleType().ToList(), "ModuleId", "ModuleName");
            Mail.TransactionType = new SelectList(GetTransactionType().ToList(), "TransactionTypeId", "TransactionTypeName");
            Mail.WorkFlowType = new SelectList(GetWorkFlowType().ToList(), "WorkFlowId", "WorkFlowName");
            Mail.TriggerType = new SelectList(GetTriggerType().ToList(), "TriggerTypeId", "TriggerTypeName");
            Mail.TriggerFreType = new SelectList(GetTriggerFreType().ToList(), "TriggerTypeFreId", "TriggerTypeFreName");
            Mail.AudienceType = new SelectList(GetAudienceType().ToList(), "AudienceId", "AudienceName");
            Mail.ToType = new SelectList(GetToType().ToList(), "ToTypeId", "ToTypeName");
            Mail.ToGetType = new SelectList(GetToGetType().ToList(), "ToGetTypeId", "ToGetTypeName");
            Mail.Attachment = new SelectList(GetAttachment().ToList(), "AttachmentId", "AttachmentName");
            Mail.Tables = new SelectList(objModel.GetTables().ToList(), "TablesId", "TablesName");
            Mail.Tablescol = new SelectList(objModel.GetTablescol(tblname).ToList(), "TablescolId", "TablescolName");
            // Mail.Content = "supplier Code is : []    supplier name is : []    supplier Contract From is : []   supplier Contract To is : []   ";
            Mail.Content = "";
            ViewBag.DashBoardheaderser = Mail;
            return View();
        }
        [HttpPost]
        public ActionResult Index(string ddlTemplateName)
        {
            return View();
        }
        public IEnumerable<ForMailEntity> GetTemplate()
        {
            List<ForMailEntity> objparenttax = new List<ForMailEntity>();
            objparenttax.Add(new ForMailEntity { TemplateId = "0", TemplateName = "-- Select --", });
            objparenttax.Add(new ForMailEntity { TemplateId = "1", TemplateName = "Template-1", });
            objparenttax.Add(new ForMailEntity { TemplateId = "2", TemplateName = "Template-2", });
            objparenttax.Add(new ForMailEntity { TemplateId = "3", TemplateName = "Template-3", });
            return objparenttax;
        }
        public IEnumerable<ForMailEntity> GetTransactionType()
        {
            List<ForMailEntity> objparenttax = new List<ForMailEntity>();
            objparenttax.Add(new ForMailEntity { TransactionTypeId = "0", TransactionTypeName = "-- Select --", });
            objparenttax.Add(new ForMailEntity { TransactionTypeId = "1", TransactionTypeName = "ARF", });
            objparenttax.Add(new ForMailEntity { TransactionTypeId = "2", TransactionTypeName = "ECF", });
            objparenttax.Add(new ForMailEntity { TransactionTypeId = "3", TransactionTypeName = "PO", });
            objparenttax.Add(new ForMailEntity { TransactionTypeId = "4", TransactionTypeName = "WO", });
            return objparenttax;
        }
        public IEnumerable<ForMailEntity> GetTriggerType()
        {
            List<ForMailEntity> objparenttax = new List<ForMailEntity>();
            objparenttax.Add(new ForMailEntity { TriggerTypeId = "0", TriggerTypeName = "-- Select --", });
            objparenttax.Add(new ForMailEntity { TriggerTypeId = "A", TriggerTypeName = "On Approval", });
            objparenttax.Add(new ForMailEntity { TriggerTypeId = "S", TriggerTypeName = "On Submission", });
            objparenttax.Add(new ForMailEntity { TriggerTypeId = "R", TriggerTypeName = "On Rejection", });
            return objparenttax;
        }
        public IEnumerable<ForMailEntity> GetTriggerFreType()
        {
            List<ForMailEntity> objparenttax = new List<ForMailEntity>();
            objparenttax.Add(new ForMailEntity { TriggerTypeFreId = "0", TriggerTypeFreName = "-- Select --", });
            objparenttax.Add(new ForMailEntity { TriggerTypeFreId = "I", TriggerTypeFreName = "Immediate", });
            objparenttax.Add(new ForMailEntity { TriggerTypeFreId = "A", TriggerTypeFreName = "Auto Alert", });
            return objparenttax;
        }

        public IEnumerable<ForMailEntity> GetAudienceType()
        {
            List<ForMailEntity> objparenttax = new List<ForMailEntity>();
            objparenttax.Add(new ForMailEntity { AudienceId = "0", AudienceName = "-- Select --", });
            objparenttax.Add(new ForMailEntity { AudienceId = "I", AudienceName = "Internal", });
            objparenttax.Add(new ForMailEntity { AudienceId = "E", AudienceName = "External", });
            objparenttax.Add(new ForMailEntity { AudienceId = "B", AudienceName = "Both", });
            return objparenttax;
        }
        public IEnumerable<ForMailEntity> GetToType()
        {
            List<ForMailEntity> objparenttax = new List<ForMailEntity>();
            objparenttax.Add(new ForMailEntity { ToTypeId = "0", ToTypeName = "-- Select --", });
            objparenttax.Add(new ForMailEntity { ToTypeId = "To", ToTypeName = "To", });
            objparenttax.Add(new ForMailEntity { ToTypeId = "Cc", ToTypeName = "Cc", });
            objparenttax.Add(new ForMailEntity { ToTypeId = "Bcc", ToTypeName = "Bcc", });
            return objparenttax;
        }
        public IEnumerable<ForMailEntity> GetToGetType()
        {
            List<ForMailEntity> objparenttax = new List<ForMailEntity>();
            objparenttax.Add(new ForMailEntity { ToGetTypeId = "0", ToGetTypeName = "-- Select --", });
            objparenttax.Add(new ForMailEntity { ToGetTypeId = "1", ToGetTypeName = "Maker", });
            objparenttax.Add(new ForMailEntity { ToGetTypeId = "2", ToGetTypeName = "Checker", });
            objparenttax.Add(new ForMailEntity { ToGetTypeId = "3", ToGetTypeName = "Authorizer", });
            return objparenttax;
        }
        public IEnumerable<ForMailEntity> GetWorkFlowType()
        {
            List<ForMailEntity> objparenttax = new List<ForMailEntity>();
            objparenttax.Add(new ForMailEntity { WorkFlowId = "0", WorkFlowName = "-- Select --", });

            return objparenttax;
        }
        public IEnumerable<ForMailEntity> GetAttachment()
        {
            List<ForMailEntity> objparenttax = new List<ForMailEntity>();
            objparenttax.Add(new ForMailEntity { AttachmentId = "0", AttachmentName = "-- Select --", });
            objparenttax.Add(new ForMailEntity { AttachmentId = "1", AttachmentName = "PO", });
            objparenttax.Add(new ForMailEntity { AttachmentId = "2", AttachmentName = "WO", });
            objparenttax.Add(new ForMailEntity { AttachmentId = "3", AttachmentName = "Payment", });
            return objparenttax;
        }
        public IEnumerable<ForMailEntity> GetTablesnew()
        {
            List<ForMailEntity> objparenttax = new List<ForMailEntity>();
            objparenttax.Add(new ForMailEntity { TablesId = "0", TablesName = "-- Select --", });
            objparenttax.Add(new ForMailEntity { TablesId = "1", TablesName = "PO", });
            objparenttax.Add(new ForMailEntity { TablesId = "2", TablesName = "WO", });
            objparenttax.Add(new ForMailEntity { TablesId = "3", TablesName = "Payment", });
            return objparenttax;
        }
        [HttpPost]
        public JsonResult GetuserCategory(ForMailEntity ForMailEntity)
        {
            return Json(objModel.getuserfunction(ForMailEntity.TransactionTypeName, ForMailEntity.ModuleName), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetWorkFlow(ForMailEntity ForMailEntity)
        {
            return Json(objModel.GetWorkFlow(ForMailEntity.TransactionTypeName, ForMailEntity.ModuleName), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetTransaction(ForMailEntity ForMailEntity)
        {
            return Json(objModel.GetTransactionType(ForMailEntity.TransactionTypeId), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetuserCategorynew(ForMailEntity ForMailEntity)
        {
            return Json(objModel.getuserdata(ForMailEntity.TransactionTypeId), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetuserCategoryfillchk(ForMailEntity ForMailEntity)
        {
            return Json(objModel.GetTablescol(ForMailEntity.TransactionTypeId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult _ForMailEntityCreate(ForMailEntity ForMailEntityCreate)
        {
            string msg = "";
            try
            {
                if (ModelState.IsValid)
                {
                    msg = objModel.Insertmailtemplate(ForMailEntityCreate);
                }
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public ActionResult Templatedetails()
        {
            Session["Mailexport"] = "";
            List<ForMailEntity> lstDashBoard = new List<ForMailEntity>();
            lstDashBoard = objModel.gettemplatedetails().ToList();
            Session["Mailexport"] = lstDashBoard;
            return View(lstDashBoard);
        }
        [HttpGet]
        public PartialViewResult TemplatedetailsEdit(int id)
        {
            Session["edittempgidid"] = id.ToString();
            List<ForMailEntity> objobjMExpense = new List<ForMailEntity>();
            objobjMExpense = objModel.SelectMailalertbyid(id.ToString()).ToList();

            string tblname = "";
            ForMailEntity Mail = new ForMailEntity();
            Mail.TemplateName = objobjMExpense[0].TemplateName.ToString();

            ViewBag.Module = objobjMExpense[0].ModuleName.ToString();
            Mail.Moduledata = new SelectList(objModel.GetModuleType().ToList(), "ModuleId", "ModuleName", objobjMExpense[0].ModuleName.ToString());

            ViewBag.TransactionTypeId = objobjMExpense[0].TransactionTypeId.ToString();
            Mail.TransactionType = new SelectList(objModel.GetTransactionType(objobjMExpense[0].ModuleName.ToString()).ToList(), "TransactionTypeId", "TransactionTypeName");

            ViewBag.TriggerTypeName = objobjMExpense[0].TriggerTypeName.ToString();
            Mail.TriggerType = new SelectList(GetTriggerType().ToList(), "TriggerTypeId", "TriggerTypeName", objobjMExpense[0].TriggerTypeName.ToString());

            ViewBag.TriggerTypeFreName = objobjMExpense[0].TriggerTypeFreName.ToString();
            Mail.TriggerFreType = new SelectList(GetTriggerFreType().ToList(), "TriggerTypeFreId", "TriggerTypeFreName", objobjMExpense[0].TriggerTypeFreName.ToString());

            ViewBag.WorkFlowId = objobjMExpense[0].WorkFlowId.ToString();
            Mail.WorkFlowType = new SelectList(objModel.GetWorkFlow(objobjMExpense[0].TransactionTypeName.ToString(), objobjMExpense[0].ModuleName.ToString()).ToList(), "WorkFlowId", "WorkFlowName");

            ViewBag.AudienceName = objobjMExpense[0].AudienceName.ToString();
            Mail.AudienceType = new SelectList(GetAudienceType().ToList(), "AudienceId", "AudienceName", objobjMExpense[0].AudienceName.ToString());

            Mail.ToType = new SelectList(GetToType().ToList(), "ToTypeId", "ToTypeName");
            Mail.ToGetType = new SelectList(objModel.getuserfunction(objobjMExpense[0].TransactionTypeName.ToString(), objobjMExpense[0].ModuleName.ToString()).ToList(), "ToGetTypeId", "ToGetTypeName");
            Mail.AttachmentName = objobjMExpense[0].AttachmentName.ToString();
            Mail.Includeflg = objobjMExpense[0].Includeflg.ToString();
            ViewBag.TablesName = objobjMExpense[0].TablesName.ToString();

            Mail.Tables = new SelectList(objModel.getuserdata(objobjMExpense[0].TransactionTypeId.ToString()).ToList(), "TablescolId", "TablescolName");
            Mail.Tablescol = new SelectList(objModel.GetTablescol(objobjMExpense[0].TablesName.ToString()).ToList(), "TablesId", "TablesName");

            Hashtable ht = new Hashtable();
            DataTable dt = new DataTable();
            string chkal = "";
            dt = objModel.selectmailfields(id.ToString());
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string getvalues = dt.Rows[i]["mailtemplatefield_mailfield_gid"].ToString();
                    if (!ht.ContainsValue(getvalues))
                    {
                        ht.Add(i.ToString(), getvalues);
                        if (chkal == "")
                        {
                            chkal = getvalues.ToString();
                        }
                        else
                        {
                            chkal = chkal + "," + getvalues.ToString();
                        }
                    }
                }
            }
            ViewBag.checkval = chkal;
            ViewBag.ckeckedvalues = ht;

            Mail.Subject = objobjMExpense[0].Subject.ToString();
            Mail.Content = objobjMExpense[0].Content.ToString();
            Mail.Signature = objobjMExpense[0].Signature.ToString();
            Mail.TOid = objobjMExpense[0].TOid.ToString();
            Mail.CCid = objobjMExpense[0].CCid.ToString();
            Mail.BCCid = objobjMExpense[0].BCCid.ToString();
            ViewBag.DashBoardheaderser = Mail;
            return PartialView();
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult templateEdit(ForMailEntity objMExpenseEdit)
        {
            string msg = "";
            msg = objModel.Updatetemplate(objMExpenseEdit, Session["edittempgidid"].ToString());
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult templateDelete(ForMailEntity EmployeeeExpensemodel)
        {
            string EmployeeePaymentGID = EmployeeeExpensemodel.TemplateId;
            string delamt = objModel.Deletetemplate(EmployeeePaymentGID);
            return Json(EmployeeeExpensemodel, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Testmail()
        {
            Session["error"] = "";
            return View();
        }
        [HttpPost]
        public ActionResult Testmail(string Enablessl, string txtxname, string Module, string TransactionType, string Gid, string request, string sendmailid, string sendpw, string smptMailserver, string smptMailPort)
        {
            try
            {
                //Module = "ASMS";
                //TransactionType = "ONBOARDING";
                //gid = "1118";
                //request = "S";
                //string requestalt = sendusermail("ASMS", "ONBOARDING", "1118", "S", txtxname);
                string requestalt = sendusermailtest(Enablessl, txtxname, Module, TransactionType, Gid, request, "0", sendmailid, sendpw, smptMailserver, smptMailPort);
                Session["error"] = requestalt.ToString();
                // Response.Write("<script>alert(" + requestalt + ");</script>");
                return View();
            }
            catch (Exception ex)
            {
                string sst = ex.ToString();
                //Response.Write("<script>alert('err......" + sst + "....');</script>");
            }
            finally
            {

            }
            return View();
        }


        public string sendusermailtest(string Enablessl, string to, string Module, string TransactionType, string gid, string request, string nextdata2,
            string sendmailid, string sendpw, string smptMailserver, string smptMailPort)
        {
            string sst = "";
            try
            {
                string send_mail = "";
                string to_mail = "";
                string send_pw = "";
                string smpt_Mail = "";
                int smpt_Mailport = 0;
                string appurl = "";
                string rejurl = "";
                string ssll = "";
                ssll = Enablessl.ToString();
                //send_mail = ConfigurationManager.AppSettings["sendmail"].ToString();
                //send_pw = ConfigurationManager.AppSettings["sendpw"].ToString();
                //smpt_Mail = ConfigurationManager.AppSettings["smptMail"].ToString();
                //smpt_Mailport = Convert.ToInt16(ConfigurationManager.AppSettings["smptMailPort"].ToString());

                send_mail = sendmailid.ToString();
                send_pw = sendpw.ToString();
                smpt_Mail = smptMailserver.ToString();
                smpt_Mailport = Convert.ToInt32(smptMailPort.ToString());

                appurl = ConfigurationManager.AppSettings["Approvedclaimurl"].ToString();
                rejurl = ConfigurationManager.AppSettings["Rejectclaimurl"].ToString();

                List<ForMailEntity> objForMailEntityloop = new List<ForMailEntity>();
                List<ForMailEntity> objForMailEntity = new List<ForMailEntity>();
                objForMailEntity = objModel.SelectMailtemplate(Module, TransactionType, gid, request, nextdata2).ToList();
                if (objForMailEntity.Count > 0)
                {
                    string myString = objForMailEntity[0].Content.ToString();

                    objForMailEntityloop = objModel.Getmailselectdfield(objForMailEntity[0].TemplateId.ToString()).ToList();
                    if (objForMailEntityloop.Count > 0)
                    {
                        DataTable dtreqid = new DataTable();
                        dtreqid = objModel.Selectbyid(Module, objForMailEntity[0].TransactionTypeName.ToString(), gid, objForMailEntity[0].TriggerTypeName.ToString());
                        if (dtreqid.Rows.Count > 0)
                        {
                            for (int tr = 0; objForMailEntityloop.Count > tr; tr++)
                            {
                                string col = objForMailEntityloop[tr].ToGetTypeName.ToString();
                                string colval = Convert.ToString(dtreqid.Rows[0][col].ToString());
                                string val = "[" + objForMailEntityloop[tr].ToGetTypeId.ToString() + "]";
                                myString = myString.Replace(val, colval);
                            }
                        }

                        SmtpClient Mail = new SmtpClient(smpt_Mail, smpt_Mailport);
                        MailMessage mailmsg = new MailMessage();

                        string fromids = to.ToString(); ;
                        string[] multi1 = fromids.Split(',');
                        MailAddress mfrom = new MailAddress(send_mail.ToString());
                        mailmsg.From = mfrom;

                        string ids = to.ToString();
                        string[] multi = ids.Split(',');
                        foreach (string str in multi)
                        {
                            mailmsg.To.Add(new MailAddress(str));
                        }

                        mailmsg.Subject = objForMailEntity[0].Subject.ToString();
                        mailmsg.IsBodyHtml = true;
                        mailmsg.Body = "<br >  ";
                        mailmsg.Body = mailmsg.Body + myString;
                        if (objForMailEntity[0].Includeflg.ToString() == "Y")
                        {
                            string ActivationUrl = appurl + "?UserID=" + Encode(send_mail.ToString()) + "&Email=" + Encode(send_pw.ToString()) + "&Gid=" + Encode(gid.ToString());
                            string ActivationUrlreject = rejurl + "?UserID=" + Encode(send_mail.ToString()) + "&Email=" + Encode(send_pw.ToString()) + "&Gid=" + Encode(gid.ToString());

                            string appove = "<br /><table style='width: 100%; padding-left: 0%;'>";
                            appove += " <tr><td><br /><br /><br /><br /></td></tr>";
                            appove += "<tr><td>Click Below to Submit Your Request...........<br><hr></td></tr>";
                            appove += "<tr><td style='padding-left: 20%;'>";
                            appove += "    <a data-ved='0CAcQjRw' href='" + ActivationUrl + "' name='bvcbvc' jsaction='mousedown:irc.rl;keydown:irc.rlk;irc.il;' style='color: #FFF;background-color: #449D44;border-color: #398439;text-decoration: none;display: inline-block;padding: 6px 12px;margin-bottom: 0px;font-size: 14px;font-weight: 400;line-height: 1.42857;text-align: center;white-space: nowrap;vertical-align: middle;cursor: pointer;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;font-family: inherit;text-transform: none;overflow: visible;margin: 0px;font: inherit;' id='irc_mila'>Approve</a>";
                            appove += "  &nbsp;&nbsp;&nbsp;&nbsp;";
                            appove += "      <a data-ved='0CAcQjRw' href='" + ActivationUrlreject + "' name='bvcbvc' jsaction='mousedown:irc.rl;keydown:irc.rlk;irc.il;' style='color: #FFF;background-color: #EC971F;border-color: #D58512;text-decoration: none;display: inline-block;padding: 6px 12px;margin-bottom: 0px;font-size: 14px;font-weight: 400;line-height: 1.42857;text-align: center;white-space: nowrap;vertical-align: middle;cursor: pointer;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;font-family: inherit;text-transform: none;overflow: visible;margin: 0px;font: inherit;' id='irc_milr'>Reject</a>";
                            appove += "  </td>";
                            appove += "</tr>";
                            appove += "</table>";
                            mailmsg.Body = mailmsg.Body + appove;
                        }
                        Mail.EnableSsl = Convert.ToBoolean(ssll);
                        NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                        Mail.UseDefaultCredentials = false;
                        Mail.Credentials = credentials;
                        Mail.Send(mailmsg);
                        return "Sucess";
                    }
                }
                else
                {
                    SmtpClient Mail = new SmtpClient(smpt_Mail, smpt_Mailport);
                    MailMessage mailmsg = new MailMessage();

                    string fromids = to.ToString(); ;
                    string[] multi1 = fromids.Split(',');
                    MailAddress mfrom = new MailAddress(send_mail.ToString());
                    mailmsg.From = mfrom;

                    string ids = to.ToString();
                    string[] multi = ids.Split(',');
                    foreach (string str in multi)
                    {
                        mailmsg.To.Add(new MailAddress(str));
                    }

                    mailmsg.Subject = "Hi.......Thirumalai S";
                    mailmsg.IsBodyHtml = true;
                    mailmsg.Body = "<br > how are you ?  ";
                    mailmsg.Body = mailmsg.Body;

                    string ActivationUrl = appurl + "?UserID=" + Encode(send_mail.ToString()) + "&Email=" + Encode(send_pw.ToString()) + "&Gid=" + Encode(gid.ToString());
                    string ActivationUrlreject = rejurl + "?UserID=" + Encode(send_mail.ToString()) + "&Email=" + Encode(send_pw.ToString()) + "&Gid=" + Encode(gid.ToString());

                    string appove = "<br /><table style='width: 100%; padding-left: 0%;'>";
                    appove += " <tr><td><br /><br /><br /><br /></td></tr>";
                    appove += "<tr><td>Click Below to Submit Your Request...........<br><hr></td></tr>";
                    appove += "<tr><td style='padding-left: 20%;'>";
                    appove += "    <a data-ved='0CAcQjRw' href='" + ActivationUrl + "' name='bvcbvc' jsaction='mousedown:irc.rl;keydown:irc.rlk;irc.il;' style='color: #FFF;background-color: #449D44;border-color: #398439;text-decoration: none;display: inline-block;padding: 6px 12px;margin-bottom: 0px;font-size: 14px;font-weight: 400;line-height: 1.42857;text-align: center;white-space: nowrap;vertical-align: middle;cursor: pointer;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;font-family: inherit;text-transform: none;overflow: visible;margin: 0px;font: inherit;' id='irc_mila'>Approve</a>";
                    appove += "  &nbsp;&nbsp;&nbsp;&nbsp;";
                    appove += "      <a data-ved='0CAcQjRw' href='" + ActivationUrlreject + "' name='bvcbvc' jsaction='mousedown:irc.rl;keydown:irc.rlk;irc.il;' style='color: #FFF;background-color: #EC971F;border-color: #D58512;text-decoration: none;display: inline-block;padding: 6px 12px;margin-bottom: 0px;font-size: 14px;font-weight: 400;line-height: 1.42857;text-align: center;white-space: nowrap;vertical-align: middle;cursor: pointer;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;font-family: inherit;text-transform: none;overflow: visible;margin: 0px;font: inherit;' id='irc_milr'>Reject</a>";
                    appove += "  </td>";
                    appove += "</tr>";
                    appove += "</table>";
                    mailmsg.Body = mailmsg.Body + appove;

                    Mail.EnableSsl = Convert.ToBoolean(ssll);
                    NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                    Mail.UseDefaultCredentials = false;
                    Mail.Credentials = credentials;
                    Mail.Send(mailmsg);
                    return "Sucess";
                }
            }
            catch (Exception ex)
            {
                sst = ex.ToString();
            }
            finally
            {

            }
            return sst;
        }
        public string sendusermail(string Module, string TransactionType, string gid, string request, string workflow, string _TmpECFId = "0")
        {
            try
            {
                string doctype = "";
                string clamtype = "";
                string ecfstatus = "0";
                string ecfsubjectno = "";
                string ids = "";
                string tomaigiv = "";
                string send_mail = "";
                string to_mail = "";
                string send_pw = "";
                string smpt_Mail = "";
                int smpt_Mailport = 0;
                string appurl = "";
                string rejurl = "";
                string ssll = "";
                string supplierheadergid = "0";

                string parheadergid = "0";
                string prheadergid = "0";
                string cbfheadergid = "0";
                string poheadergid = "0";
                string woheadergid = "0";
                string runningmode = "";
                string secondaryrequest = "";

                //START  --Added For Approval Summary [Gopi]
                string _FileName = "", _RefFlag = "0", _RefGId = "0";
                // _RefGId = _TmpECFId == "0" ? gid : _TmpECFId;
                _RefGId = _TmpECFId;
                if (Module == "FB")
                {
                    if (TransactionType == "CBF")
                    {
                        _RefFlag = "2";
                    }
                    if (TransactionType == "PO")
                    {
                        _RefFlag = "8";
                    }
                    if (TransactionType == "PAR")
                    {
                        _RefFlag = "9";
                    }
                    if (TransactionType == "PR")
                    {
                        _RefFlag = "5";
                    }
                    if (TransactionType == "WO")
                    {
                        _RefFlag = "10";
                    }
                }
                if (Module == "ASMS")
                {
                    _RefFlag = "6";
                }
                if (Module == "EOW")
                {
                    _RefFlag = "1";
                }

                if (_RefFlag != "0" && (_RefGId != null && _RefGId != "0"))
                {
                    DataSet dsFileName = dbTransaction.GetAttachmentASNew(_RefFlag, _RefGId, objCmnFunctions.GetLoginUserGid().ToString());
                    if (dsFileName != null && dsFileName.Tables.Count > 0)
                    {
                        if (dsFileName.Tables[0].Rows.Count > 0)
                        {
                            _FileName = dsFileName.Tables[0].Rows[0]["FileName"].ToString();
                        }
                    }
                }
                //END


                runningmode = ConfigurationManager.AppSettings["LoginFor"].ToString();
                if (runningmode.ToLower().ToString() != "development")
                {
                    ssll = ConfigurationManager.AppSettings["ssl"].ToString();
                    send_mail = ConfigurationManager.AppSettings["sendmail"].ToString();
                    send_pw = ConfigurationManager.AppSettings["sendpw"].ToString();
                    smpt_Mail = ConfigurationManager.AppSettings["smptMail"].ToString();
                    smpt_Mailport = Convert.ToInt16(ConfigurationManager.AppSettings["smptMailPort"].ToString());

                    appurl = ConfigurationManager.AppSettings["Approvedclaimurl"].ToString();
                    rejurl = ConfigurationManager.AppSettings["Rejectclaimurl"].ToString();

                    List<ForMailEntity> objForMailEntityloop = new List<ForMailEntity>();
                    List<ForMailEntity> objForMailEntity = new List<ForMailEntity>();
                    if (Module == "FB")
                    {
                        //if (TransactionType.ToUpper() != "PAR")
                        //{
                        workflow = objModel.GetWorkFlowFB(gid);
                        //}
                    }
                    if (Module == "FS")
                    {
                        doctype = objModel.selectdocsubtype(gid, "");
                    }
                    int secmaillimit = 1;
                    if (Module == "EOW")
                    {
                        if (request == "A")
                        {
                            secmaillimit = 2;
                            secondaryrequest = "A";
                            request = "S";
                        }

                    }
                    for (int secondmail = 0; secondmail < secmaillimit; secondmail++)
                    {
                        if (secondaryrequest == "A" && secondmail == 1)
                        {
                            request = "A";
                        }
                        objForMailEntity = objModel.SelectMailtemplate(Module, TransactionType, gid, request, workflow).ToList();

                        if (objForMailEntity.Count > 0)
                        {

                            string myString = objForMailEntity[0].Content.ToString();
                            string data = objForMailEntity[0].TOid.ToString();
                            string[] mailsplit = data.Split(',');
                            if (data.Trim() != "")
                            {
                                for (int ii = 0; ii < mailsplit.Length; ii++)
                                {
                                    string ordata = mailsplit[ii].ToString();

                                    string[] mailsplit1 = ordata.Split('@');
                                    if (Module == "EOW" && mailsplit[ii].ToString() == "RAISER" && secondmail == 0)
                                    {

                                    }
                                    else
                                    {
                                        if (ordata != "")
                                        {
                                            if (mailsplit1.Length > 1)
                                            {
                                                if (tomaigiv == "")
                                                {
                                                    tomaigiv = ordata;
                                                }
                                                else
                                                {
                                                    tomaigiv = tomaigiv + "," + ordata;
                                                }
                                            }
                                            else
                                            {
                                                if (tomaigiv == "")
                                                {
                                                    tomaigiv = objModel.SelectMailIdsForRoleGroup(mailsplit[ii].ToString(), gid, workflow, Module, TransactionType);
                                                }
                                                else
                                                {
                                                    tomaigiv = tomaigiv + "," + objModel.SelectMailIdsForRoleGroup(mailsplit[ii].ToString(), gid, workflow, Module, TransactionType);
                                                }
                                            }
                                        }
                                    }


                                }
                            }

                            string ccmailids = "";
                            string ccdata = objForMailEntity[0].CCid.ToString();
                            if (ccdata.Trim() != "")
                            {
                                string[] mailsplitcc = ccdata.Split(',');
                                for (int jj = 0; jj < mailsplitcc.Length; jj++)
                                {
                                    string ccdata1 = mailsplitcc[jj].ToString();
                                    string[] mailsplitcc1 = ccdata1.Split('@');
                                    if (Module == "EOW" && mailsplitcc[jj].ToString() == "RAISER" && secondmail == 0)
                                    {

                                    }
                                    else
                                    {
                                        if (ccdata1 != "")
                                        {
                                            if (mailsplitcc1.Length > 1)
                                            {
                                                if (ccmailids == "")
                                                {
                                                    ccmailids = ccdata1;
                                                }
                                                else
                                                {
                                                    ccmailids = ccmailids + "," + ccdata1;
                                                }
                                            }
                                            else
                                            {
                                                if (ccmailids == "")
                                                {
                                                    ccmailids = objModel.SelectMailIdsForRoleGroup(mailsplitcc[jj].ToString(), gid, workflow, Module, TransactionType);
                                                }
                                                else
                                                {
                                                    ccmailids = ccmailids + "," + objModel.SelectMailIdsForRoleGroup(mailsplitcc[jj].ToString(), gid, workflow, Module, TransactionType);
                                                }
                                            }
                                        }
                                    }

                                }
                            }

                            string bccmailids = "";
                            string bccdata = objForMailEntity[0].BCCid.ToString();
                            if (bccdata.Trim() != "")
                            {
                                string[] mailsplitbcc = bccdata.Split(',');
                                for (int kk = 0; kk < mailsplitbcc.Length; kk++)
                                {
                                    string bccdata1 = mailsplitbcc[kk].ToString();
                                    string[] mailsplitbcc1 = bccdata1.Split('@');
                                    if (Module == "EOW" && mailsplitbcc[kk].ToString() == "RAISER" && secondmail == 0)
                                    {

                                    }
                                    else
                                    {
                                        if (bccdata1 != "")
                                        {
                                            if (mailsplitbcc1.Length > 1)
                                            {
                                                if (bccmailids == "")
                                                {
                                                    bccmailids = bccdata1;
                                                }
                                                else
                                                {
                                                    bccmailids = bccmailids + "," + bccdata1;
                                                }
                                            }
                                            else
                                            {
                                                if (bccmailids == "")
                                                {
                                                    bccmailids = objModel.SelectMailIdsForRoleGroup(mailsplitbcc[kk].ToString(), gid, workflow, Module, TransactionType);
                                                }
                                                else
                                                {
                                                    bccmailids = bccmailids + "," + objModel.SelectMailIdsForRoleGroup(mailsplitbcc[kk].ToString(), gid, workflow, Module, TransactionType);
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            objForMailEntityloop = objModel.Getmailselectdfield(objForMailEntity[0].TemplateId.ToString()).ToList();
                            if (objForMailEntityloop.Count > 0)
                            {
                                DataTable dtreqid = new DataTable();
                                if (Module == "FS")
                                {
                                    dtreqid = objModel.Selectbyid(Module, objForMailEntity[0].TransactionTypeName.ToString(), gid, doctype.ToString());
                                }
                                else
                                {
                                    dtreqid = objModel.Selectbyid(Module, objForMailEntity[0].TransactionTypeName.ToString(), gid, objForMailEntity[0].TriggerTypeName.ToString());
                                }
                                if (dtreqid.Rows.Count > 0)
                                {
                                    if (Module == "ASMS")
                                    {
                                        supplierheadergid = dtreqid.Rows[0]["supplierheader_gid"].ToString();
                                    }
                                    else if (Module == "FB")
                                    {
                                        if (TransactionType == "PAR")
                                        {
                                            parheadergid = dtreqid.Rows[0]["parheader_gid"].ToString();
                                        }
                                        else if (TransactionType == "PR")
                                        {
                                            prheadergid = dtreqid.Rows[0]["prheader_gid"].ToString();
                                        }
                                        else if (TransactionType == "CBF")
                                        {
                                            cbfheadergid = dtreqid.Rows[0]["cbfheader_gid"].ToString();
                                        }
                                        else if (TransactionType == "PO")
                                        {
                                            poheadergid = dtreqid.Rows[0]["poheader_gid"].ToString();
                                        }
                                        else if (TransactionType == "WO")
                                        {
                                            woheadergid = dtreqid.Rows[0]["poheader_gid"].ToString();
                                        }
                                    }
                                    else if (Module == "EOW")
                                    {
                                        supplierheadergid = dtreqid.Rows[0]["ecf_gid"].ToString();
                                        ecfstatus = dtreqid.Rows[0]["ecf_status"].ToString();
                                        ecfsubjectno = dtreqid.Rows[0]["ecf_no"].ToString();
                                        clamtype = dtreqid.Rows[0]["ecf_supplier_employee"].ToString();
                                    }
                                    else if (Module == "FS")
                                    {
                                        supplierheadergid = dtreqid.Rows[0]["ecf_gid"].ToString();
                                        ecfstatus = dtreqid.Rows[0]["ecf_status"].ToString();
                                    }

                                    for (int tr = 0; objForMailEntityloop.Count > tr; tr++)
                                    {
                                        string col = objForMailEntityloop[tr].ToGetTypeName.ToString();
                                        string colval = Convert.ToString(dtreqid.Rows[0][col].ToString());
                                        string val = "[" + objForMailEntityloop[tr].ToGetTypeId.ToString() + "]";
                                        myString = myString.Replace(val, colval);
                                    }
                                }

                                SmtpClient Mail = new SmtpClient(smpt_Mail, smpt_Mailport);
                                MailMessage mailmsg = new MailMessage();

                                //START  --Add Attachment[Gopi]
                                if (_FileName.Trim() != "")
                                {
                                    try
                                    {
                                        mailmsg.Attachments.Add(new Attachment(pL.ApprovalSummaryDownloadUrl + _FileName));
                                    }
                                    catch { }
                                }
                                //END


                                //   string fromids = objModel.USERMailIDFrom(Module, objForMailEntity[0].TransactionTypeName.ToString(), gid, objForMailEntity[0].TriggerTypeName.ToString());
                                MailAddress mfrom = new MailAddress(send_mail.ToString());
                                mailmsg.From = mfrom;
                                string isfinalapprover = objForMailEntity[0].IsFinalApprover.ToString();
                                DataTable dtMailTo = new DataTable();
                                DataTable dtNextApproverLink = new DataTable();
                                string NextApproverRoleGroupName = "N";
                                if (Module == "FS")
                                {
                                    objForMailEntity[0].Includeflg = "N";
                                    dtMailTo = objModel.USERMailIDTofs(gid, "","");
                                }
                                else
                                {
                                    if (Module == "EOW" && ecfstatus == "10" || ecfstatus == "20")
                                    {
                                        if (secondaryrequest != "A" || secondmail == 0)
                                        {
                                            dtMailTo = objModel.USERMailIDTocentralteam(gid);
                                            dtNextApproverLink = objModel.GetNextApproverLink(Module, objForMailEntity[0].TransactionTypeName.ToString(), gid);
                                            NextApproverRoleGroupName = objModel.GetNextApproverLink1(Module, objForMailEntity[0].TransactionTypeName.ToString(), gid);
                                        }
                                    }
                                    else
                                    {
                                        if (secondaryrequest != "A" || secondmail == 0)
                                        {
                                            dtMailTo = objModel.USERMailIDTo(Module, objForMailEntity[0].TransactionTypeName.ToString(), gid, objForMailEntity[0].TriggerTypeName.ToString(), isfinalapprover.ToString().ToUpper().Trim());
                                            dtNextApproverLink = objModel.GetNextApproverLink(Module, objForMailEntity[0].TransactionTypeName.ToString(), gid);
                                            NextApproverRoleGroupName = objModel.GetNextApproverLink1(Module, objForMailEntity[0].TransactionTypeName.ToString(), gid);
                                        }
                                    }
                                }
                                if (secondaryrequest != "A" || secondmail == 0)
                                {
                                    for (int maildt = 0; maildt < dtMailTo.Rows.Count; maildt++)
                                    {
                                        string str = dtMailTo.Rows[maildt]["employee_office_email"].ToString();
                                        string mailidtousercode = dtMailTo.Rows[maildt]["employee_code"].ToString();

                                        string[] multinewTO = str.Split(',');
                                        foreach (string strTO in multinewTO)
                                        {
                                            mailmsg.To.Add(new MailAddress(strTO.Trim()));
                                        }
                                        if (ccmailids.Trim() != "")
                                        {
                                            string[] ccmailadd = ccmailids.Split(',');
                                            foreach (string str1 in ccmailadd)
                                            {
                                                mailmsg.CC.Add(new MailAddress(str1));
                                            }
                                        }
                                        if (bccmailids.Trim() != "")
                                        {
                                            string[] bccmailadd = bccmailids.Split(',');
                                            foreach (string str2 in bccmailadd)
                                            {
                                                mailmsg.Bcc.Add(new MailAddress(str2));
                                            }
                                        }

                                        if (Module == "EOW")
                                        {
                                            if (objForMailEntity[0].AttachmentFlag == "Y")
                                            {
                                                byte[] bytes = null;
                                                bytes = challanPrintmail(supplierheadergid, clamtype);
                                                mailmsg.Attachments.Add(new Attachment(new MemoryStream(bytes), ecfsubjectno + ".pdf"));
                                            }
                                            mailmsg.Subject = objForMailEntity[0].Subject.ToString() + " " + ecfsubjectno;
                                        }
                                        else
                                        {
                                            mailmsg.Subject = objForMailEntity[0].Subject.ToString();
                                        }

                                        mailmsg.IsBodyHtml = true;
                                        mailmsg.Body = "<br >  ";
                                        mailmsg.Body = mailmsg.Body + myString;

                                        if (Module == "EOW" && ecfstatus == "10" || ecfstatus == "20")
                                        {

                                        }
                                        else if (Module == "FB")
                                        {

                                        }
                                        else
                                        {
                                            if (objForMailEntity[0].Includeflg.ToString() == "Y")
                                            {
                                                string ActivationUrl = appurl + "?module=" + Encode(Module) + "&usercode=" + Encode(mailidtousercode) + "&queuegid=" + Encode(gid) + "&lnkfor=1";
                                                string ActivationUrlreject = rejurl + "?module=" + Encode(Module) + "&usercode=" + Encode(mailidtousercode) + "&queuegid=" + Encode(gid) + "&lnkfor=0";
                                                string appove = "<br /><table style='width: 100%; padding-left: 0%;'>";
                                                appove += "<tr><td>Click Below to Submit Your Request...........<br><hr></td></tr>";
                                                if (Module == "FB")
                                                {
                                                    NextApproverRoleGroupName = objModel.CheckIsFinalApproverFB(gid);
                                                }
                                                if (NextApproverRoleGroupName == "N" || Module == "EOW")
                                                {
                                                    appove += "<tr><td style='padding-left: 20%;'>";
                                                    appove += "  <br /> <a data-ved='0CAcQjRw' href='" + ActivationUrl + "' name='bvcbvc' jsaction='mousedown:irc.rl;keydown:irc.rlk;irc.il;' style='color: #FFF;background-color: #449D44;border-color: #398439;text-decoration: none;display: inline-block;padding: 6px 12px;margin-bottom: 0px;font-size: 14px;font-weight: 400;line-height: 1.42857;text-align: center;white-space: nowrap;vertical-align: middle;cursor: pointer;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;font-family: inherit;text-transform: none;overflow: visible;margin: 0px;font: inherit;' id='irc_mila'>Approve</a>";
                                                    appove += "   &nbsp;&nbsp;&nbsp;&nbsp;";
                                                    appove += "   <a data-ved='0CAcQjRw' href='" + ActivationUrlreject + "' name='bvcbvc' jsaction='mousedown:irc.rl;keydown:irc.rlk;irc.il;' style='color: #FFF;background-color: #EC971F;border-color: #D58512;text-decoration: none;display: inline-block;padding: 6px 12px;margin-bottom: 0px;font-size: 14px;font-weight: 400;line-height: 1.42857;text-align: center;white-space: nowrap;vertical-align: middle;cursor: pointer;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;font-family: inherit;text-transform: none;overflow: visible;margin: 0px;font: inherit;' id='irc_milr'>Reject</a>";
                                                    appove += "  </td>";
                                                    appove += "</tr>";
                                                }
                                                else
                                                {
                                                    if (dtNextApproverLink.Rows.Count > 0)
                                                    {
                                                        appove += "<tr><td>Select " + NextApproverRoleGroupName + "<br></td></tr>";
                                                        for (int nxt = 0; nxt < dtNextApproverLink.Rows.Count; nxt++)
                                                        {
                                                            string empgid = dtNextApproverLink.Rows[nxt]["employee_gid"].ToString();
                                                            string empcode = dtNextApproverLink.Rows[nxt]["employee_code"].ToString().ToUpper().Trim();
                                                            string empname = dtNextApproverLink.Rows[nxt]["employee_name"].ToString().ToUpper().Trim();
                                                            string emp = empcode + "-" + empname;
                                                            string newurl = ActivationUrl + "&nextapprover=" + empgid;
                                                            appove += " <tr><td> <a  href='" + newurl + "' name='bvcbvc' >Click Here To Select - " + emp + " </a></td></tr>";
                                                        }
                                                        string IsMandatoryApprover = objModel.GetNextApproverIsMandatory(TransactionType, Module, gid);
                                                        string newurl1 = ActivationUrl + "&nextapprover=0";
                                                        appove += "<tr><td style='padding-left: 20%;'>";
                                                        if (IsMandatoryApprover == "N")
                                                        {
                                                            appove += " <br /> <a data-ved='0CAcQjRw' href='" + newurl1 + "' name='bvcbvc' jsaction='mousedown:irc.rl;keydown:irc.rlk;irc.il;' style='color: #FFF;background-color: #449D44;border-color: #398439;text-decoration: none;display: inline-block;padding: 6px 12px;margin-bottom: 0px;font-size: 14px;font-weight: 400;line-height: 1.42857;text-align: center;white-space: nowrap;vertical-align: middle;cursor: pointer;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;font-family: inherit;text-transform: none;overflow: visible;margin: 0px;font: inherit;' id='irc_mila'>Skip  " + NextApproverRoleGroupName + "</a>";
                                                            appove += "   &nbsp;&nbsp;&nbsp;&nbsp;";
                                                        }
                                                        appove += "   <a data-ved='0CAcQjRw' href='" + ActivationUrlreject + "' name='bvcbvc' jsaction='mousedown:irc.rl;keydown:irc.rlk;irc.il;' style='color: #FFF;background-color: #EC971F;border-color: #D58512;text-decoration: none;display: inline-block;padding: 6px 12px;margin-bottom: 0px;font-size: 14px;font-weight: 400;line-height: 1.42857;text-align: center;white-space: nowrap;vertical-align: middle;cursor: pointer;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;font-family: inherit;text-transform: none;overflow: visible;margin: 0px;font: inherit;' id='irc_milr'>Reject</a>";
                                                        appove += "  </td>";
                                                        appove += "</tr>";
                                                    }
                                                }

                                                appove += "</table>";
                                                mailmsg.Body = mailmsg.Body + appove;
                                            }
                                        }
                                        string regardstag = "<br /><table style='width: 100%; padding-left: 0%;'>";
                                        regardstag += "<tr><td style='font-weight:bold;font-size: 18px;'>Assuring Best Service Always,<br></td></tr>";
                                        regardstag += "<tr><td>";
                                        regardstag += "<span style='color: black;background-color: transparent;font-size: 18px;font-weight: bold;line-height: 1.42857;'>" + objForMailEntity[0].Signature.ToString() + "</a>";
                                        regardstag += "  </td>";
                                        regardstag += "</tr>";
                                        regardstag += "</table>";
                                        mailmsg.Body = mailmsg.Body + regardstag;


                                        Mail.EnableSsl = Convert.ToBoolean(ssll);
                                        NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                                        Mail.UseDefaultCredentials = false;
                                        Mail.Credentials = credentials;
                                        Mail.Send(mailmsg);

                                        if (Module == "ASMS")
                                        {
                                            string sentmailflag = objModel.mailsentmessage(objForMailEntity[0].TemplateId.ToString(), "6", supplierheadergid, "1", DateTime.Now.ToString("dd-MMM-yyyy").ToString(), str.ToString(), "", "", objForMailEntity[0].Subject.ToString(), objForMailEntity[0].Content.ToString(), objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "Y", objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "N");

                                        }
                                        else if (Module == "FB")
                                        {
                                            if (TransactionType == "PAR")
                                            {
                                                string sentmailflag = objModel.mailsentmessage(objForMailEntity[0].TemplateId.ToString(), "9", parheadergid, "1", DateTime.Now.ToString("dd-MMM-yyyy").ToString(), str.ToString(), mailmsg.CC.ToString(), mailmsg.Bcc.ToString(), objForMailEntity[0].Subject.ToString(), objForMailEntity[0].Content.ToString(), objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "Y", objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "N");
                                            }
                                            else if (TransactionType == "PR")
                                            {
                                                string sentmailflag = objModel.mailsentmessage(objForMailEntity[0].TemplateId.ToString(), "4", prheadergid, "1", DateTime.Now.ToString("dd-MMM-yyyy").ToString(), str.ToString(), mailmsg.CC.ToString(), mailmsg.Bcc.ToString(), objForMailEntity[0].Subject.ToString(), objForMailEntity[0].Content.ToString(), objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "Y", objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "N");
                                            }
                                            else if (TransactionType == "CBF")
                                            {
                                                string sentmailflag = objModel.mailsentmessage(objForMailEntity[0].TemplateId.ToString(), "2", cbfheadergid, "1", DateTime.Now.ToString("dd-MMM-yyyy").ToString(), str.ToString(), mailmsg.CC.ToString(), mailmsg.Bcc.ToString(), objForMailEntity[0].Subject.ToString(), objForMailEntity[0].Content.ToString(), objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "Y", objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "N");
                                            }
                                            else if (TransactionType == "PO")
                                            {
                                                string sentmailflag = objModel.mailsentmessage(objForMailEntity[0].TemplateId.ToString(), "8", poheadergid, "1", DateTime.Now.ToString("dd-MMM-yyyy").ToString(), str.ToString(), mailmsg.CC.ToString(), mailmsg.Bcc.ToString(), objForMailEntity[0].Subject.ToString(), objForMailEntity[0].Content.ToString(), objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "Y", objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "N");
                                            }
                                            else if (TransactionType == "WO")
                                            {
                                                string sentmailflag = objModel.mailsentmessage(objForMailEntity[0].TemplateId.ToString(), "10", woheadergid, "1", DateTime.Now.ToString("dd-MMM-yyyy").ToString(), str.ToString(), mailmsg.CC.ToString(), mailmsg.Bcc.ToString(), objForMailEntity[0].Subject.ToString(), objForMailEntity[0].Content.ToString(), objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "Y", objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "N");
                                            }
                                        }
                                        else if (Module == "EOW")
                                        {
                                            string sentmailflag = objModel.mailsentmessage(objForMailEntity[0].TemplateId.ToString(), "1", supplierheadergid, ecfstatus.ToString(), DateTime.Now.ToString("dd-MMM-yyyy").ToString(), str.ToString(), "", "", objForMailEntity[0].Subject.ToString(), objForMailEntity[0].Content.ToString(), objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "Y", objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "N");
                                        }
                                        else if (Module == "FS")
                                        {
                                            string sentmailflag = objModel.mailsentmessage(objForMailEntity[0].TemplateId.ToString(), "1", supplierheadergid, ecfstatus.ToString(), DateTime.Now.ToString("dd-MMM-yyyy").ToString(), str.ToString(), "", "", objForMailEntity[0].Subject.ToString(), objForMailEntity[0].Content.ToString(), objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "Y", objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "N");
                                        }
                                        string[] multinewtore = str.Split(',');
                                        foreach (string strtore in multinewtore)
                                        {
                                            mailmsg.To.Remove(new MailAddress(strtore.Trim()));
                                        }
                                        string[] ccmailremove = ccmailids.Split(',');
                                        if (ccmailremove.Length > 0)
                                        {
                                            foreach (string strtore in ccmailremove)
                                            {
                                                if (!string.IsNullOrEmpty(strtore.Trim()))
                                                {
                                                    mailmsg.CC.Remove(new MailAddress(strtore.Trim()));
                                                }
                                            }
                                        }
                                        string[] bccmailremov = bccmailids.Split(',');
                                        if (bccmailremov.Length > 0)
                                        {
                                            foreach (string strtore in bccmailremov)
                                            {
                                                if (!string.IsNullOrEmpty(strtore.Trim()))
                                                {
                                                    mailmsg.Bcc.Remove(new MailAddress(strtore.Trim()));
                                                }
                                            }
                                        }
                                    }
                                }
                                SmtpClient Mail1 = new SmtpClient(smpt_Mail, smpt_Mailport);
                                MailMessage mailmsg1 = new MailMessage();

                                //START  --Add Attachment[Gopi]
                                if (_FileName.Trim() != "")
                                {
                                    try
                                    {
                                        mailmsg1.Attachments.Add(new Attachment(pL.ApprovalSummaryDownloadUrl + _FileName));
                                    }
                                    catch { }
                                }
                                //END

                                MailAddress mfrom1 = new MailAddress(send_mail.ToString());
                                mailmsg1.From = mfrom1;
                                if (tomaigiv.Trim() != "")
                                {
                                    string[] multinew = tomaigiv.Split(',');
                                    foreach (string str in multinew)
                                    {
                                        mailmsg1.To.Add(new MailAddress(str));
                                    }
                                }
                                if (ccmailids.Trim() != "")
                                {
                                    string[] ccmailadd = ccmailids.Split(',');
                                    foreach (string str1 in ccmailadd)
                                    {
                                        mailmsg1.CC.Add(new MailAddress(str1));
                                    }
                                }
                                if (bccmailids.Trim() != "")
                                {
                                    string[] bccmailadd = bccmailids.Split(',');
                                    foreach (string str2 in bccmailadd)
                                    {
                                        mailmsg1.Bcc.Add(new MailAddress(str2));
                                    }
                                }
                                if (Module == "EOW")
                                {
                                    if (objForMailEntity[0].AttachmentFlag == "Y")
                                    {
                                        byte[] bytes = null;
                                        bytes = challanPrintmail(supplierheadergid, clamtype);
                                        mailmsg1.Attachments.Add(new Attachment(new MemoryStream(bytes), ecfsubjectno + ".pdf"));
                                    }
                                    mailmsg1.Subject = objForMailEntity[0].Subject.ToString() + " " + ecfsubjectno;
                                    if (ecfstatus == "1" || ecfstatus == "64")
                                    {
                                        if (secondaryrequest != "A" || secondmail == 1)
                                        {
                                            DataTable dtMailToctall = new DataTable();
                                            dtMailToctall = objModel.USERMailIDTocentralteamall(gid);
                                            for (int maildtct = 0; maildtct < dtMailToctall.Rows.Count; maildtct++)
                                            {
                                                string strct = dtMailToctall.Rows[maildtct]["employee_office_email"].ToString();

                                                string[] multinewTOct = strct.Split(',');
                                                foreach (string strTOct in multinewTOct)
                                                {
                                                    mailmsg1.To.Add(new MailAddress(strTOct.Trim()));
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    mailmsg1.Subject = objForMailEntity[0].Subject.ToString();
                                }

                                mailmsg1.IsBodyHtml = true;
                                mailmsg1.Body = "<br >  ";
                                mailmsg1.Body = mailmsg1.Body + myString;
                               
                                string regardstag1 = "<br /><table style='width: 100%; padding-left: 0%;'>";
                                regardstag1 += "<tr><td style='font-weight:bold;font-size: 18px;'>Assuring Best Service Always<br></td></tr>";
                                regardstag1 += "<tr><td>";
                                regardstag1 += "<span style='color: black;background-color: transparent;font-size: 18px;font-weight: bold;line-height: 1.42857;'>" + objForMailEntity[0].Signature.ToString() + "</a>";
                                regardstag1 += "  </td>";
                                regardstag1 += "</tr>";
                                regardstag1 += "</table>";
                                mailmsg1.Body = mailmsg1.Body + regardstag1;

                                Mail1.EnableSsl = Convert.ToBoolean(ssll);
                                NetworkCredential credentials1 = new NetworkCredential(send_mail, send_pw);
                                Mail1.UseDefaultCredentials = false;
                                Mail1.Credentials = credentials1;
                                string sentmail = "";
                                if (mailmsg1.To != null && mailmsg1.To.Count > 0)
                                {
                                    Mail1.Send(mailmsg1);
                                    sentmail = "Sucess";
                                }

                                if (sentmail == "Sucess")
                                {
                                    if (Module == "ASMS")
                                    {
                                        sentmail = objModel.mailsentmessage(objForMailEntity[0].TemplateId.ToString(), "6", supplierheadergid, "1", objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), mailmsg1.To.ToString(), mailmsg1.CC.ToString(), mailmsg1.Bcc.ToString(), objForMailEntity[0].Subject.ToString(), objForMailEntity[0].Content.ToString(), objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "Y", objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "N");
                                        return sentmail;
                                    }
                                    else if (Module == "FB")
                                    {
                                        if (TransactionType == "PAR")
                                        {
                                            sentmail = objModel.mailsentmessage(objForMailEntity[0].TemplateId.ToString(), "9", parheadergid, "1", objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), mailmsg1.To.ToString(), mailmsg1.CC.ToString(), mailmsg1.Bcc.ToString(), objForMailEntity[0].Subject.ToString(), objForMailEntity[0].Content.ToString(), objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "Y", objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "N");
                                            return sentmail;
                                        }
                                        else if (TransactionType == "PR")
                                        {
                                            sentmail = objModel.mailsentmessage(objForMailEntity[0].TemplateId.ToString(), "4", prheadergid, "1", objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), mailmsg1.To.ToString(), mailmsg1.CC.ToString(), mailmsg1.Bcc.ToString(), objForMailEntity[0].Subject.ToString(), objForMailEntity[0].Content.ToString(), objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "Y", objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "N");
                                            return sentmail;
                                        }
                                        else if (TransactionType == "CBF")
                                        {
                                            sentmail = objModel.mailsentmessage(objForMailEntity[0].TemplateId.ToString(), "2", cbfheadergid, "1", objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), mailmsg1.To.ToString(), mailmsg1.CC.ToString(), mailmsg1.Bcc.ToString(), objForMailEntity[0].Subject.ToString(), objForMailEntity[0].Content.ToString(), objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "Y", objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "N");
                                            return sentmail;
                                        }
                                        else if (TransactionType == "PO")
                                        {
                                            sentmail = objModel.mailsentmessage(objForMailEntity[0].TemplateId.ToString(), "8", poheadergid, "1", objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), mailmsg1.To.ToString(), mailmsg1.CC.ToString(), mailmsg1.Bcc.ToString(), objForMailEntity[0].Subject.ToString(), objForMailEntity[0].Content.ToString(), objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "Y", objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "N");
                                            return sentmail;
                                        }
                                        else if (TransactionType == "WO")
                                        {
                                            sentmail = objModel.mailsentmessage(objForMailEntity[0].TemplateId.ToString(), "10", woheadergid, "1", objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), mailmsg1.To.ToString(), mailmsg1.CC.ToString(), mailmsg1.Bcc.ToString(), objForMailEntity[0].Subject.ToString(), objForMailEntity[0].Content.ToString(), objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "Y", objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "N");
                                            return sentmail;
                                        }
                                    }
                                    else if (Module == "EOW")
                                    {
                                        string sentmailflag = objModel.mailsentmessage(objForMailEntity[0].TemplateId.ToString(), "1", supplierheadergid, ecfstatus.ToString(), DateTime.Now.ToString("dd-MMM-yyyy").ToString(), mailmsg1.To.ToString(), "", "", objForMailEntity[0].Subject.ToString(), objForMailEntity[0].Content.ToString(), objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "Y", objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "N");
                                    }
                                }
                            }
                        }
                        else
                        {
                            return "template Not match";
                        }
                    }
                    //end line
                }
                else
                {
                    return "not sent";
                }
                return "Sucess";
            }
            catch (Exception ex)
            {
                
                ErrorLog objErrorLog = new ErrorLog();
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "faild";
            }
            finally
            {

            }
        }
        public string Encode(string encodeMe)
        {
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(encodeMe);
            return Convert.ToBase64String(encoded);
        }
        public static string Decode(string decodeMe)
        {
            byte[] encoded = Convert.FromBase64String(decodeMe);
            return System.Text.Encoding.UTF8.GetString(encoded);
        }
        [HttpGet]
        public ActionResult Approvedclaim()
        {
            string Email, UserID, Gid;
            int i = 0;
            if ((Request.QueryString["UserID"] != null) & (Request.QueryString["Email"] != null) & (Request.QueryString["Gid"] != null))
            {
                UserID = Decode(Request.QueryString["UserID"]);
                Email = Decode(Request.QueryString["Email"]);
                Gid = Decode(Request.QueryString["Gid"]);
                //string message = objModel.Mailgidappved(UserID, Email, Gid);
                if (Session["gidapp"] != null)
                {
                    Session["gidapp"] = "thirumalai";
                    Response.Write("<script>alert('claim Approved..........');window.close();</script>");
                }
                else
                {
                    Response.Write("<script>alert('claim Already Approved..........');window.close();</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('ERROR..........');window.close();</script>");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Rejectclaim()
        {
            string Email, UserID, Gid;
            int i = 0;
            if ((Request.QueryString["UserID"] != null) & (Request.QueryString["Email"] != null) & (Request.QueryString["Gid"] != null))
            {
                UserID = Decode(Request.QueryString["UserID"]);
                Email = Decode(Request.QueryString["Email"]);
                Gid = Decode(Request.QueryString["Gid"]);
                if (Session["gidapp"] != null)
                {
                    Session["gidapp"] = "thirumalai";
                    Response.Write("<script>alert('claim Rejected..........');window.close();</script>");
                }
                else
                {
                    Response.Write("<script>alert('claim Already Rejected..........');window.close();</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('ERROR..........');window.close();</script>");
            }
            return View();
        }
        public string sendusermailfs(string Module, string TransactionType, string gid, string request, string[] toaddress, string workflow)
        {
            try
            {
                //Module = "FS";

                //TransactionType = "Physical Receipting";
                //gid = "211546";

                //TransactionType = "Inex Submission";
                //gid = "211946";

                //TransactionType = "Prepaid Adjustment - Vendor Claim";
                //gid= "220269";

                //TransactionType = "Prepaid Adjustment - Employee Claim";
                //gid = "211938";

                //TransactionType = "Payment Alert Vendor Claim";
                //gid = "7";

                //TransactionType = "Payment Alert Employee Claim";
                //gid = "6";

                //TransactionType = "Cheque Dispatch";
                //gid = "382";

                //TransactionType = "PPX O/s - Gentle Reminder";
                //gid = "221031";

                //TransactionType = "PPX O/s - Reminder 1";
                //gid = "221031";

                //TransactionType = "PPX O/s - Reminder 2";
                //gid = "221031";

                //request = "S";
                //toaddress = new string[] { "thirumalai@flexicodeindia.com" };
                //workflow = "0";
                string PVId = "0";
                if (TransactionType == "Payment Alert Employee Claim" || TransactionType == "Payment Alert Vendor Claim")
                {
                    PVId = workflow;
                    workflow = "0";
                }

                string sentmail = "";
                string doctype = request;
                string ecfstatus = "0";
                string tomaigiv = "";
                string send_mail = "";
                string send_pw = "";
                string smpt_Mail = "";
                int smpt_Mailport = 0;
                string ssll = "";
                string supplierheadergid = "0";
                DataTable dtreqid = new DataTable();
                string ecfno = "";
                string runningmode = "";
                string mysubject = "";
                string pvno = "";
                string Footertext = "";
                DataTable Bouncdt = new DataTable();
                runningmode = ConfigurationManager.AppSettings["LoginFor"].ToString();
                ErrorLog objErrorLog = new ErrorLog();
              
                if (runningmode.ToLower().ToString() != "development")
                {
                    dbLib db = new dbLib();
                   // DataSet dtss = db.getuploaddata("1", "1234");
                    ssll = ConfigurationManager.AppSettings["ssl"].ToString();
                    send_mail = ConfigurationManager.AppSettings["sendmail"].ToString();
                    send_pw = ConfigurationManager.AppSettings["sendpw"].ToString();
                    smpt_Mail = ConfigurationManager.AppSettings["smptMail"].ToString();
                    smpt_Mailport = Convert.ToInt16(ConfigurationManager.AppSettings["smptMailPort"].ToString());
                    List<ForMailEntity> objForMailEntityloop = new List<ForMailEntity>();
                    List<ForMailEntity> objForMailEntity = new List<ForMailEntity>();
                    objForMailEntity = objModel.SelectMailtemplate(Module, TransactionType, gid, request, workflow).ToList();
                    
                    //DataSet dtsss = db.getuploaddata("1", objForMailEntity.Count.ToString());
                     if (objForMailEntity.Count > 0)
                    {
                        string fillhtmltable = "";
                        string myString = objForMailEntity[0].Content.ToString();
                        int start = objForMailEntity[0].Content.IndexOf("START FOOTER", 0);
                        int end = objForMailEntity[0].Content.IndexOf("END FOOTER", 0);
                        if (start > 0 && end > 0)
                        {
                            Footertext = objForMailEntity[0].Content.Substring((start + 12), (end - start) - 12);
                            myString = myString.Replace("START FOOTER", "");
                            myString = myString.Replace("END FOOTER", "");
                            myString = myString.Replace(Footertext, "");
                        }
                        string data = objForMailEntity[0].TOid.ToString();
                        string[] mailsplit = data.Split(',');
                        if (data.Trim() != "")
                        {
                            for (int ii = 0; ii < mailsplit.Length; ii++)
                            {
                                string ordata = mailsplit[ii].ToString();

                                string[] mailsplit1 = ordata.Split('@');

                                if (ordata != "")
                                {
                                    if (mailsplit1.Length > 1)
                                    {
                                        if (tomaigiv == "")
                                        {
                                            tomaigiv = ordata;
                                        }
                                        else
                                        {
                                            tomaigiv = tomaigiv + "," + ordata;
                                        }
                                    }
                                    else
                                    {
                                        if (tomaigiv == "")
                                        {
                                            tomaigiv = objModel.SelectMailIdsForRoleGroup(mailsplit[ii].ToString(), gid, workflow, Module, TransactionType);
                                        }
                                        else
                                        {
                                            tomaigiv = tomaigiv + "," + objModel.SelectMailIdsForRoleGroup(mailsplit[ii].ToString(), gid, workflow, Module, TransactionType);
                                        }
                                    }
                                }

                            }
                        }

                        string ccmailids = "";
                        string ccdata = objForMailEntity[0].CCid.ToString();
                        if (ccdata.Trim() != "")
                        {
                            string[] mailsplitcc = ccdata.Split(',');
                            for (int jj = 0; jj < mailsplitcc.Length; jj++)
                            {
                                string ccdata1 = mailsplitcc[jj].ToString();
                                string[] mailsplitcc1 = ccdata1.Split('@');
                                if (ccdata1 != "")
                                {
                                    if (mailsplitcc1.Length > 1)
                                    {
                                        if (ccmailids == "")
                                        {
                                            ccmailids = ccdata1;
                                        }
                                        else
                                        {
                                            ccmailids = ccmailids + "," + ccdata1;
                                        }
                                    }
                                    else
                                    {
                                        if (ccmailids == "")
                                        {
                                            ccmailids = objModel.SelectMailIdsForRoleGroup(mailsplitcc[jj].ToString(), gid, workflow, Module, TransactionType);
                                            objErrorLog.WriteErrorLog("Get cc mail id", "cc"); 
                                        }
                                        else
                                        {
                                            ccmailids = ccmailids + "," + objModel.SelectMailIdsForRoleGroup(mailsplitcc[jj].ToString(), gid, workflow, Module, TransactionType);
                                            objErrorLog.WriteErrorLog("Get cc mail id", "cc1");
                                        }
                                    }
                                }

                            }
                        }

                        string bccmailids = "";
                        string bccdata = objForMailEntity[0].BCCid.ToString();
                        if (bccdata.Trim() != "")
                        {
                            string[] mailsplitbcc = bccdata.Split(',');
                            for (int kk = 0; kk < mailsplitbcc.Length; kk++)
                            {
                                string bccdata1 = mailsplitbcc[kk].ToString();
                                string[] mailsplitbcc1 = bccdata1.Split('@');
                                if (bccdata1 != "")
                                {
                                    if (mailsplitbcc1.Length > 1)
                                    {
                                        if (bccmailids == "")
                                        {
                                            bccmailids = bccdata1;
                                        }
                                        else
                                        {
                                            bccmailids = bccmailids + "," + bccdata1;
                                        }
                                    }
                                    else
                                    {
                                        if (bccmailids == "")
                                        {
                                            bccmailids = objModel.SelectMailIdsForRoleGroup(mailsplitbcc[kk].ToString(), gid, workflow, Module, TransactionType);
                                        }
                                        else
                                        {
                                            bccmailids = bccmailids + "," + objModel.SelectMailIdsForRoleGroup(mailsplitbcc[kk].ToString(), gid, workflow, Module, TransactionType);
                                        }
                                    }
                                }

                            }
                        }

                        objForMailEntityloop = objModel.Getmailselectdfield(objForMailEntity[0].TemplateId.ToString()).ToList();
                        if (objForMailEntityloop.Count > 0)
                        {
                            if (TransactionType == "Payment Alert Vendor Claim" || TransactionType == "Payment Alert Employee Claim" || TransactionType == "Cheque Dispatch")
                            {
                                DataTable paymentData= new DataTable();
                                if (TransactionType == "Cheque Dispatch")
                                {
                                    paymentData = objModel.ChequeStatusUpdateDetails(gid);
                                }
                                else
                                {
                                    paymentData = objModel.EftErapaymentAlert(PVId);
                                }
                                if(paymentData.Rows.Count > 0)
                                {
                                    if (TransactionType == "Payment Alert Vendor Claim")
                                    {
                                        fillhtmltable += "<table style='width: 100%; padding-left: 0%;border-collapse: collapse;'>";
                                        fillhtmltable += "<tr>";
                                        fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>S. No.</td>";
                                        for (int tr = 0; objForMailEntityloop.Count > tr; tr++)
                                        {
                                            string col = objForMailEntityloop[tr].ToGetTypeId.ToString();
                                            string colnew = objForMailEntityloop[tr].ToGetTypeName.ToString();
                                            
                                            if (col == "VENDOR NAME")
                                            {
                                                string colval = Convert.ToString(paymentData.Rows[0][colnew].ToString());
                                                string val = "[" + objForMailEntityloop[tr].ToGetTypeId.ToString() + "]";
                                                myString = myString.Replace(val, colval);
                                           }
                                            if (col == "INVOICE DESCRIPTION")
                                            {
                                                string colval = Convert.ToString(paymentData.Rows[0][colnew].ToString());
                                                string val = "[" + objForMailEntityloop[tr].ToGetTypeId.ToString() + "]";
                                                myString = myString.Replace(val, colval);
                                            }
                                            if (col == "BENEFICIARY A/C NO")
                                            {
                                                string colval = Convert.ToString(paymentData.Rows[0][colnew].ToString());
                                                string val = "[" + objForMailEntityloop[tr].ToGetTypeId.ToString() + "]";
                                                myString = myString.Replace(val, colval);
                                            }
                                            if (col == "PV NO")
                                            {
                                                string colval = Convert.ToString(paymentData.Rows[0][colnew].ToString());
                                                string val = "[" + objForMailEntityloop[tr].ToGetTypeId.ToString() + "]";
                                                pvno = val;
                                                myString = myString.Replace(val, colval);
                                            }
                                            if (col == "BENEFICIARY BANKNAME")
                                            {
                                                string colval = Convert.ToString(paymentData.Rows[0][colnew].ToString());
                                                string val = "[" + objForMailEntityloop[tr].ToGetTypeId.ToString() + "]";
                                                myString = myString.Replace(val, colval);
                                            }
                                            if (col == "CLEARED DATE")
                                            {
                                                string colval = Convert.ToString(paymentData.Rows[0][colnew].ToString());
                                                string val = "[" + objForMailEntityloop[tr].ToGetTypeId.ToString() + "]";
                                                myString = myString.Replace(val, colval);
                                            }
                                            //if (col == "INVOICE AMOUNT")
                                            //{
                                            //    string colval = Convert.ToString(paymentData.Rows[0][colnew].ToString());
                                            //    string val = "[" + objForMailEntityloop[tr].ToGetTypeId.ToString() + "]";
                                            //    myString = myString.Replace(val, colval);
                                            //}
                                            if (col == "ARF NO")
                                            {
                                                myString = myString.Replace("[ARF NO]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>REF NO / A/C No</td>";
                                            }
                                            if (col == "PAY MODE")
                                            {
                                                myString = myString.Replace("[PAY MODE]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>PAY MODE</td>";
                                            }
                                            if (col == "ECF NO")
                                            {
                                                myString = myString.Replace("[ECF NO]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>ECF No</td>";
                                            }
                                            if (col == "PROFORMA INVOICE NO")
                                            {
                                                myString = myString.Replace("[PROFORMA INVOICE NO]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>Proforma Invoice No / Invoice No</td>";
                                            }
                                            if (col == "INVOICE NO")
                                            {
                                                myString = myString.Replace("[INVOICE NO]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>Invoice No</td>";
                                            }
                                            if (col == "PO/ WO NO")
                                            {
                                                myString = myString.Replace("[PO/ WO NO]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>PO/ WO No</td>";
                                            }
                                            if (col == "BRANCH CODE")
                                            {
                                                myString = myString.Replace("[BRANCH CODE]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>Branch Code</td>";
                                            }
                                            if (col == "BRANCH NAME")
                                            {
                                                myString = myString.Replace("[BRANCH NAME]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>Branch Name</td>";
                                            }
                                            if (col == "INVOICE AMOUNT")
                                            {
                                                myString = myString.Replace("[INVOICE AMOUNT]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>Invoice Amount</td>";
                                            }
                                            if (col == "TDS AMOUNT")
                                            {
                                                myString = myString.Replace("[TDS AMOUNT]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>TDS Amount</td>";
                                            }
                                            if (col == "WCT AMOUNT")
                                            {
                                                myString = myString.Replace("[WCT AMOUNT]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>WCT Amount</td>";
                                            }
                                            if (col == "PV AMOUNT")
                                            {
                                                string colval = Convert.ToString(paymentData.Rows[0]["PV Amount"].ToString());
                                                string val = "[" + objForMailEntityloop[tr].ToGetTypeId.ToString() + "]";
                                                myString = myString.Replace(val, colval);

                                            }
                                            if (col == "NET AMOUNT")
                                            {
                                                myString = myString.Replace("[NET AMOUNT]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>Net Amount</td>";
                                            }
                                           
                                        }
                                        fillhtmltable += "</tr>";
                                        int a = 0;
                                        for (int trcol = 0; paymentData.Rows.Count > trcol; trcol++)
                                        {
                                            a++;
                                            fillhtmltable += "<tr>";
                                            fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>" + a + "</td>";
                                            for (int tr = 0; objForMailEntityloop.Count > tr; tr++)
                                            {
                                                string colval = "";
                                                string col = objForMailEntityloop[tr].ToGetTypeId.ToString();
                                                string colnew = objForMailEntityloop[tr].ToGetTypeName.ToString();

                                                if (col == "ECF NO")
                                                {
                                                    colval = Convert.ToString(paymentData.Rows[trcol][colnew].ToString());
                                                    ecfno = colval;
                                                    pvno = Convert.ToString(paymentData.Rows[trcol]["pvno"].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                                if (col == "ARF NO")
                                                {

                                                    colval = Convert.ToString(paymentData.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                                if (col == "PAY MODE")
                                                {

                                                    colval = Convert.ToString(paymentData.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                                if (col == "PROFORMA INVOICE NO")
                                                {
                                                    colval = Convert.ToString(paymentData.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                                if (col == "INVOICE NO")
                                                {
                                                    colval = Convert.ToString(paymentData.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                                if (col == "PO/ WO NO")
                                                {
                                                    colval = Convert.ToString(paymentData.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                                if (col == "BRANCH CODE")
                                                {
                                                    colval = Convert.ToString(paymentData.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                                if (col == "BRANCH NAME")
                                                {
                                                    colval = Convert.ToString(paymentData.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                                if (col == "INVOICE AMOUNT")
                                                {
                                                    colval = Convert.ToString(paymentData.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                                if (col == "TDS AMOUNT")
                                                {
                                                    colval = Convert.ToString(paymentData.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                                if (col == "WCT AMOUNT")
                                                {
                                                    colval = Convert.ToString(paymentData.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                                if (col == "NET AMOUNT")
                                                {
                                                    colval = Convert.ToString(paymentData.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                            }
                                            fillhtmltable += "</tr>";
                                        }

                                        fillhtmltable += "</table><br />";
                                    }
                                    else if (TransactionType == "Payment Alert Employee Claim")
                                    {
                                        fillhtmltable += "<table style='width: 100%; padding-left: 0%;border-collapse: collapse;'>";
                                        fillhtmltable += "<tr>";
                                        fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>S. No.</td>";
                                        for (int tr = 0; objForMailEntityloop.Count > tr; tr++)
                                        {
                                            string col = objForMailEntityloop[tr].ToGetTypeId.ToString();
                                            string colnew = objForMailEntityloop[tr].ToGetTypeName.ToString();

                                            if (col == "RAISER NAME")
                                            {
                                                string colval = Convert.ToString(paymentData.Rows[0][colnew].ToString());
                                                string val = "[" + objForMailEntityloop[tr].ToGetTypeId.ToString() + "]";
                                                myString = myString.Replace(val, colval);
                                            }
                                            if (col == "CLEARED DATE")
                                            {
                                                string colval = Convert.ToString(paymentData.Rows[0][colnew].ToString());
                                                string val = "[" + objForMailEntityloop[tr].ToGetTypeId.ToString() + "]";
                                                myString = myString.Replace(val, colval);
                                            }
                                            if (col == "BENEFICIARY A/C NO")
                                            {
                                                string colval = Convert.ToString(paymentData.Rows[0][colnew].ToString());
                                                string val = "[" + objForMailEntityloop[tr].ToGetTypeId.ToString() + "]";
                                                myString = myString.Replace(val, colval);
                                            }
                                            if (col == "BENEFICIARY BANKNAME")
                                            {
                                                string colval = Convert.ToString(paymentData.Rows[0][colnew].ToString());
                                                string val = "[" + objForMailEntityloop[tr].ToGetTypeId.ToString() + "]";
                                                myString = myString.Replace(val, colval);
                                            }
                                            if (col == "NET AMOUNT")
                                            {
                                                string colval = Convert.ToString(paymentData.Rows[0]["PV Amount"].ToString());
                                                string val = "[" + objForMailEntityloop[tr].ToGetTypeId.ToString() + "]";
                                                myString = myString.Replace(val, colval);
                                            }
                                            if (col == "ECF NO")
                                            {
                                                myString = myString.Replace("[ECF NO]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>ECF No</td>";
                                            }
                                            if (col == "BRANCH CODE")
                                            {
                                                myString = myString.Replace("[BRANCH CODE]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>Branch Code</td>";
                                            }
                                            if (col == "BRANCH NAME")
                                            {
                                                myString = myString.Replace("[BRANCH NAME]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>Branch Name</td>";
                                            }
                                            if (col == "NET AMOUNT")
                                            {
                                                myString = myString.Replace("[NET AMOUNT]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>Net Amount</td>";
                                            }
                                            if (col == "PAY MODE")
                                            {
                                                myString = myString.Replace("[PAY MODE]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>Pay Mode</td>";
                                            }
                                        }
                                        fillhtmltable += "</tr>";
                                        int b = 0;
                                        for (int trcol = 0; paymentData.Rows.Count > trcol; trcol++)
                                        {
                                            b++;
                                            fillhtmltable += "<tr>";
                                            fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>" + b + "</td>";
                                            for (int tr = 0; objForMailEntityloop.Count > tr; tr++)
                                            {
                                                string colval = "";
                                                string col = objForMailEntityloop[tr].ToGetTypeId.ToString();
                                                string colnew = objForMailEntityloop[tr].ToGetTypeName.ToString();

                                                if (col == "ECF NO")
                                                {
                                                    colval = Convert.ToString(paymentData.Rows[trcol][colnew].ToString());
                                                    ecfno = colval;
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                                if (col == "BRANCH CODE")
                                                {
                                                    colval = Convert.ToString(paymentData.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                                if (col == "BRANCH NAME")
                                                {
                                                    colval = Convert.ToString(paymentData.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                                if (col == "NET AMOUNT")
                                                {
                                                    colval = Convert.ToString(paymentData.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                                if (col == "PAY MODE")
                                                {
                                                    colval = Convert.ToString(paymentData.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                            }
                                            fillhtmltable += "</tr>";
                                        }
                                        fillhtmltable += "</table><br />";
                                    }
                                    else if (TransactionType == "Cheque Dispatch")
                                    {
                                        fillhtmltable += "<table style='width: 100%; padding-left: 0%;border-collapse: collapse; margin:auto 0;'>";
                                        fillhtmltable += "<tr>";
                                        fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>S. No.</td>";
                                        for (int tr = 0; objForMailEntityloop.Count > tr; tr++)
                                        {
                                            string col = objForMailEntityloop[tr].ToGetTypeId.ToString();
                                            string colnew = objForMailEntityloop[tr].ToGetTypeName.ToString();
                                            if (col == "CHEQUE NO")
                                            {
                                                string colval = Convert.ToString(paymentData.Rows[0][colnew].ToString());
                                                string val = "[" + objForMailEntityloop[tr].ToGetTypeId.ToString() + "]";
                                                myString = myString.Replace(val, colval);
                                            }
                                            if (col == "DESPATCH DATE")
                                            {
                                                string colval = Convert.ToString(paymentData.Rows[0][colnew].ToString());
                                                string val = "[" + objForMailEntityloop[tr].ToGetTypeId.ToString() + "]";
                                                myString = myString.Replace(val, colval);
                                            }
                                            if (col == "CHEQUE AMOUNT")
                                            {
                                                string colval = Convert.ToString(paymentData.Rows[0]["PV Amount"].ToString());
                                                string val = "[" + objForMailEntityloop[tr].ToGetTypeId.ToString() + "]";
                                                myString = myString.Replace(val, colval);
                                            }
                                            if (col == "AWB NO")
                                            {
                                                string colval = Convert.ToString(paymentData.Rows[0]["Awdno"].ToString());
                                                string val = "[" + objForMailEntityloop[tr].ToGetTypeId.ToString() + "]";
                                                myString = myString.Replace(val, colval);
                                            }

                                            if (col == "BRANCH CODE HEAD")
                                            {
                                                string colval = Convert.ToString(paymentData.Rows[0]["branchlcode"].ToString());
                                                string val = "[" + objForMailEntityloop[tr].ToGetTypeId.ToString() + "]";
                                                myString = myString.Replace(val, colval);
                                            }
                                            if (col == "COURIER NAME")
                                            {
                                                string colval = Convert.ToString(paymentData.Rows[0][colnew].ToString());
                                                string val = "[" + objForMailEntityloop[tr].ToGetTypeId.ToString() + "]";
                                                myString = myString.Replace(val, colval);
                                            }
                                            if (col == "VENDOR NAME HEAD")
                                            {
                                                string colval = Convert.ToString(paymentData.Rows[0][colnew].ToString());
                                                string val = "[" + objForMailEntityloop[tr].ToGetTypeId.ToString() + "]";
                                                myString = myString.Replace(val, colval);
                                            }
                                            if (col == "VENDOR NAME")
                                            {
                                                myString = myString.Replace("[VENDOR NAME]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>VENDOR NAME</td>";
                                            }
                                            if (col == "ECF NO")
                                            {
                                                myString = myString.Replace("[ECF NO]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>ECF /ARF NO</td>";
                                            }
                                            if (col == "PROFORMA INVOICE NO")
                                            {
                                                myString = myString.Replace("[PROFORMA INVOICE NO]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>INVOICE /PROFORMA INVOICE NO</td>";
                                            }
                                            if (col == "INVOICE NO")
                                            {
                                                myString = myString.Replace("[INVOICE NO]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>INVOICE NO</td>";
                                            }
                                            if (col == "PO/ WO NO")
                                            {
                                                myString = myString.Replace("[PO/ WO NO]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>PO/ WO No</td>";
                                            }
                                            if (col == "BRANCH CODE")
                                            {
                                                myString = myString.Replace("[BRANCH CODE]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>Branch Code</td>";
                                            }
                                            if (col == "BRANCH NAME")
                                            {
                                                myString = myString.Replace("[BRANCH NAME]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>Branch Name</td>";
                                            }
                                            if (col == "INVOICE AMOUNT")
                                            {
                                                myString = myString.Replace("[INVOICE AMOUNT]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>Invoice Amount</td>";
                                            }
                                            if (col == "TDS AMOUNT")
                                            {
                                                myString = myString.Replace("[TDS AMOUNT]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>TDS Amount</td>";
                                            }
                                            if (col == "WCT AMOUNT")
                                            {
                                                myString = myString.Replace("[WCT AMOUNT]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>WCT Amount</td>";
                                            }
                                            if (col == "NET AMOUNT")
                                            {
                                                //string colval = Convert.ToString(paymentData.Rows[0]["ECF Amount"].ToString());
                                                //string val = "[" + objForMailEntityloop[tr].ToGetTypeId.ToString() + "]";
                                                //myString = myString.Replace(val, colval);
                                                myString = myString.Replace("[NET AMOUNT]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>Net Amount</td>";
                                            }

                                        }
                                           fillhtmltable += "</tr>";
                                        int b = 0;
                                        for (int trcol = 0; paymentData.Rows.Count > trcol; trcol++)
                                        {
                                             b++;
                                            fillhtmltable += "<tr>";
                                            fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>" + b + "</td>";
                                            for (int tr = 0; objForMailEntityloop.Count > tr; tr++)
                                            {
                                                string colval = "";
                                                string col = objForMailEntityloop[tr].ToGetTypeId.ToString();
                                                string colnew = objForMailEntityloop[tr].ToGetTypeName.ToString();

                                                if (col == "ECF NO")
                                                {
                                                    colval = Convert.ToString(paymentData.Rows[trcol][colnew].ToString());
                                                    ecfno = colval;
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                                if (col == "VENDOR NAME")
                                                {
                                                    colval = Convert.ToString(paymentData.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                                if (col == "PROFORMA INVOICE NO")
                                                {
                                                    colval = Convert.ToString(paymentData.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                                if (col == "INVOICE NO") //ramya added
                                                {
                                                    colval = Convert.ToString(paymentData.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                                if (col == "PO/ WO NO")
                                                {
                                                    colval = Convert.ToString(paymentData.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                                if (col == "BRANCH CODE")
                                                {
                                                    colval = Convert.ToString(paymentData.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                                if (col == "BRANCH NAME")
                                                {
                                                    colval = Convert.ToString(paymentData.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                                if (col == "INVOICE AMOUNT")
                                                {
                                                    colval = Convert.ToString(paymentData.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                                if (col == "TDS AMOUNT")
                                                {
                                                    colval = Convert.ToString(paymentData.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                                if (col == "WCT AMOUNT")
                                                {
                                                    colval = Convert.ToString(paymentData.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                                if (col == "NET AMOUNT")
                                                {
                                                    colval = Convert.ToString(paymentData.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                            }
                                            fillhtmltable += "</tr>";
                                        }
                                        fillhtmltable += "</table><br />";
                                    }
                                }
                            }
                            //else if (TransactionType == "Cheque Dispatch")
                            //{
                            //    DataTable chequeDetail = objModel.ChequeStatusUpdateDetails(gid);
                            //    for (int tr = 0; objForMailEntityloop.Count > tr; tr++)
                            //    {
                            //        string col = objForMailEntityloop[tr].ToGetTypeName.ToString();
                            //        ecfno = chequeDetail.Rows[0]["pvno"].ToString();
                            //        string colval = Convert.ToString(chequeDetail.Rows[0][col].ToString() == null ? "" : chequeDetail.Rows[0][col].ToString());
                            //        string val = "[" + objForMailEntityloop[tr].ToGetTypeId.ToString() + "]";
                            //        myString = myString.Replace(val, colval);
                            //    }
                            //}
                            else
                            {
                               
                                dtreqid = objModel.Selectbyid(Module, objForMailEntity[0].TransactionTypeName.ToString(), gid, doctype.ToString());
                                if (dtreqid.Rows.Count > 0)
                                {
                                    if (TransactionType == "Payment Advice Email Alert")
                                    {
                                        supplierheadergid = dtreqid.Rows[0]["ecf_gid"].ToString();
                                    }
                                    else if (TransactionType == "Petty Cash Email Alert")
                                    {
                                        supplierheadergid = dtreqid.Rows[0]["EcfId"].ToString();
                                    }
                                    else if (TransactionType == "EFT Rejection Email Alert")
                                    {
                                        supplierheadergid = dtreqid.Rows[0]["PvId"].ToString();
                                    }
                                    else if (TransactionType == "Inex Submission")
                                    {
                                        supplierheadergid = dtreqid.Rows[0]["ECFId"].ToString();
                                        ecfno = dtreqid.Rows[0]["Doc No"].ToString();
                                        Bouncdt = objModel.GetBounchresone(supplierheadergid);
                                    }
                                    else if (TransactionType == "PPX O/s - Gentle Reminder")
                                    {
                                        supplierheadergid = dtreqid.Rows[0]["ECFId"].ToString();
                                    }
                                    else if (TransactionType == "PPX O/s - Reminder 1")
                                    {
                                        supplierheadergid = dtreqid.Rows[0]["ECFId"].ToString();
                                    }
                                    else if (TransactionType == "PPX O/s - Reminder 2")
                                    {
                                        supplierheadergid = dtreqid.Rows[0]["ECFId"].ToString();
                                    }
                                    else if (TransactionType == "Physical Receipting")
                                    {
                                        supplierheadergid = dtreqid.Rows[0]["ecf_gid"].ToString();
                                         ecfno = dtreqid.Rows[0]["ECF No"].ToString();
                                    }
                                    else if (TransactionType == "Prepaid Adjustment - Vendor Claim")
                                    {
                                        ecfno = dtreqid.Rows[0]["ECF No"].ToString();
                                    }
                                    else
                                    {
                                        supplierheadergid = gid;
                                    }


                                    if (TransactionType == "Prepaid Adjustment - Vendor Claim")
                                    {
                                        fillhtmltable += "<table style='width: 100%; padding-left: 0%;margin-top:10px;margin-bottom:10px;border-collapse: collapse;'>";
                                        fillhtmltable += "<tr>";
                                        fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>S. No.</td>";
                                        for (int tr = 0; objForMailEntityloop.Count > tr; tr++)
                                        {
                                            string col = objForMailEntityloop[tr].ToGetTypeId.ToString();
                                            string colnew = objForMailEntityloop[tr].ToGetTypeName.ToString();
                                            if (col == "RAISER NAME")
                                            {
                                                string colval = Convert.ToString(dtreqid.Rows[0][colnew].ToString());
                                                string val = "[" + objForMailEntityloop[tr].ToGetTypeId.ToString() + "]";
                                                myString = myString.Replace(val, colval);
                                            }
                                            if (col == "VENDOR/EMPLOYEE NAME")
                                            {
                                                myString = myString.Replace("[VENDOR/EMPLOYEE NAME]", "");

                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>Vendor Name</td>";
                                            }
                                            if (col == "INVOICE NO")
                                            {
                                                myString = myString.Replace("[INVOICE NO]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>Invoice No</td>";
                                            }
                                            if (col == "INVOICE DATE")
                                            {
                                                myString = myString.Replace("[INVOICE DATE]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>Invoice Dt</td>";
                                            }
                                            if (col == "INVOICE AMOUNT")
                                            {
                                                myString = myString.Replace("[INVOICE AMOUNT]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>InvoiceAmt </td>";
                                            }
                                            if (col == "TDS AMOUNT")
                                            {
                                                myString = myString.Replace("[TDS AMOUNT]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>TDS</td>";
                                            }
                                            if (col == "WCT AMOUNT")
                                            {
                                                myString = myString.Replace("[WCT AMOUNT]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>WCT</td>";
                                            }
                                            if (col == "PPX AMOUNT")
                                            {
                                                myString = myString.Replace("[PPX AMOUNT]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>PPX Adjustment</td>";
                                            }
                                            if (col == "RETENTION AMOUNT")
                                            {
                                                myString = myString.Replace("[RETENTION AMOUNT]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>Retention</td>";
                                            }
                                            if (col == "PPX NUMBER")
                                            {
                                                myString = myString.Replace("[PPX NUMBER]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>PPX Number</td>";
                                            }
                                        }
                                        fillhtmltable += "</tr>";
                                        int c = 0;
                                        for (int trcol = 0; dtreqid.Rows.Count > trcol; trcol++)
                                        {
                                            c++;
                                            fillhtmltable += "<tr>";
                                            fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>" + c + "</td>";
                                            for (int tr = 0; objForMailEntityloop.Count > tr; tr++)
                                            {
                                                string colval = "";
                                                string col = objForMailEntityloop[tr].ToGetTypeId.ToString();
                                                string colnew = objForMailEntityloop[tr].ToGetTypeName.ToString();

                                                if (col == "VENDOR/EMPLOYEE NAME")
                                                {
                                                    colval = Convert.ToString(dtreqid.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + "</td>";
                                                }
                                                if (col == "INVOICE NO")
                                                {
                                                    colval = Convert.ToString(dtreqid.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                                if (col == "INVOICE DATE")
                                                {
                                                    colval = Convert.ToString(dtreqid.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                                if (col == "INVOICE AMOUNT")
                                                {
                                                    colval = Convert.ToString(dtreqid.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                                if (col == "TDS AMOUNT")
                                                {
                                                    colval = Convert.ToString(dtreqid.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + "</td>";
                                                }
                                                if (col == "WCT AMOUNT")
                                                {
                                                    colval = Convert.ToString(dtreqid.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + "</td>";
                                                }
                                                if (col == "PPX AMOUNT")
                                                {
                                                    colval = Convert.ToString(dtreqid.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                                if (col == "RETENTION AMOUNT")
                                                {
                                                    colval = Convert.ToString(dtreqid.Rows[0][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + "</td>";
                                                }
                                                if (col == "PPX NUMBER")
                                                {
                                                    colval = Convert.ToString(dtreqid.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                            }
                                            fillhtmltable += "</tr>";
                                        }

                                        fillhtmltable += "</table><br />";
                                    }
                                    else if (TransactionType == "Prepaid Adjustment - Employee Claim")
                                    {
                                        fillhtmltable += "<table style='width: 100%; padding-left: 0%;margin-top:10px;margin-bottom:10px;border-collapse: collapse;'>";
                                        fillhtmltable += "<tr>";
                                        fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>S. No.</td>";
                                        for (int tr = 0; objForMailEntityloop.Count > tr; tr++)
                                        {
                                            string col = objForMailEntityloop[tr].ToGetTypeId.ToString();
                                            string colnew = objForMailEntityloop[tr].ToGetTypeName.ToString();
                                            if (col == "RAISER NAME")
                                            {
                                                string colval = Convert.ToString(dtreqid.Rows[0][colnew].ToString());
                                                string val = "[" + objForMailEntityloop[tr].ToGetTypeId.ToString() + "]";
                                                myString = myString.Replace(val, colval);
                                            }
                                            if (col == "ECF NO")
                                            {
                                                myString = myString.Replace("[ECF NO]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>ECF No</td>";
                                            }
                                            if (col == "ECF AMOUNT")
                                            {
                                                myString = myString.Replace("[ECF AMOUNT]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>ECF Amount</td>";
                                            }
                                            if (col == "PPX AMOUNT")
                                            {
                                                myString = myString.Replace("[PPX AMOUNT]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>PPX Adjustment</td>";
                                            }
                                            if (col == "PPX NUMBER")
                                            {
                                                myString = myString.Replace("[PPX NUMBER]", "");
                                                fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>PPX Number</td>";
                                            }
                                        }
                                        fillhtmltable += "</tr>";
                                        int d = 0;
                                        for (int trcol = 0; dtreqid.Rows.Count > trcol; trcol++)
                                        {
                                            d++;
                                            fillhtmltable += "<tr>";
                                            fillhtmltable += "<td style='font-weight:bold;font-size: 12px;border: 1px solid black;'>" + d + "</td>";
                                            for (int tr = 0; objForMailEntityloop.Count > tr; tr++)
                                            {
                                                string colval = "";
                                                string col = objForMailEntityloop[tr].ToGetTypeId.ToString();
                                                string colnew = objForMailEntityloop[tr].ToGetTypeName.ToString();

                                                if (col == "ECF NO")
                                                {
                                                    colval = Convert.ToString(dtreqid.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                                if (col == "ECF AMOUNT")
                                                {
                                                    colval = Convert.ToString(dtreqid.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                                if (col == "PPX AMOUNT")
                                                {
                                                    colval = Convert.ToString(dtreqid.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                                if (col == "PPX NUMBER")
                                                {
                                                    colval = Convert.ToString(dtreqid.Rows[trcol][colnew].ToString());
                                                    fillhtmltable += "<td style='font-size: 12px;border: 1px solid black;'>" + colval + " </td>";
                                                }
                                            }
                                            fillhtmltable += "</tr>";
                                        }

                                        fillhtmltable += "</table><br />";
                                    }
                                    else
                                    {
                                        for (int tr = 0; objForMailEntityloop.Count > tr; tr++)
                                        {
                                            string bounchres = "";
                                            string col = objForMailEntityloop[tr].ToGetTypeName.ToString();
                                            string colval = Convert.ToString(dtreqid.Rows[0][col].ToString());
                                            string val = "[" + objForMailEntityloop[tr].ToGetTypeId.ToString() + "]";
                                            if (val == "[BOUNCE REASONS]")
                                            {
                                                if (Bouncdt.Rows.Count > 0)
                                                {
                                                    for (int rt = 0; Bouncdt.Rows.Count > rt; rt++)
                                                    {
                                                        string newline=Environment.NewLine;
                                                        int count = rt + 1;
                                                        bounchres += "<br>" + count + "." + Bouncdt.Rows[rt]["Reason"].ToString() + newline + "</br>";
                                                       
                                                    }
                                                    colval = bounchres;
                                                    myString = myString.Replace(val, colval);
                                                }
                                            }
                                            else
                                            {
                                                myString = myString.Replace(val, colval);
                                            }
                                        }
                                    }
                                }
                            }

                           
                            SmtpClient Mail = new SmtpClient(smpt_Mail, smpt_Mailport);
                            MailMessage mailmsg = new MailMessage();
                            MailAddress mfrom = new MailAddress(send_mail.ToString());
                            mailmsg.From = mfrom;
                            string isfinalapprover = objForMailEntity[0].IsFinalApprover.ToString();

                            DataTable dtMailTo = new DataTable();
                            objForMailEntity[0].Includeflg = "N";
                            dtMailTo = objModel.USERMailIDTofs(gid, TransactionType,PVId);
                            string StrRaiser = ""; string toemailid = "";
                            //for (int maildt = 0; maildt < dtMailTo.Rows.Count; maildt++)
                            //for (int maildt = 0; maildt < toaddress.Length; maildt++)
                            //{
                                //string str = dtMailTo.Rows[maildt]["employee_office_email"].ToString();
                                //string mailidtousercode = dtMailTo.Rows[maildt]["employee_code"].ToString();

                                //string str = toaddress[maildt].ToString();
                                //string mailidtousercode = dtMailTo.Rows[maildt]["employee_code"].ToString();

                               // mailmsg.To.Add(new MailAddress(str));
                                if (ccmailids.Trim() != "")
                                {
                                    string[] ccmailadd = ccmailids.Split(',');
                                    foreach (string str1 in ccmailadd)
                                    {
                                        if (!string.IsNullOrEmpty(str1))
                                        {
                                            mailmsg.CC.Add(new MailAddress(str1));
                                        }
                                    }
                                }
                                if (bccmailids.Trim() != "")
                                {
                                    string[] bccmailadd = bccmailids.Split(',');
                                    foreach (string str2 in bccmailadd)
                                    {
                                        if (!string.IsNullOrEmpty(str2))
                                        {
                                            mailmsg.Bcc.Add(new MailAddress(str2));
                                        }
                                    }
                                }

                                if (tomaigiv.Trim() != "" && TransactionType != "Cheque Dispatch")
                                {
                                    //Ramya Modified
                                    string[] multinew = tomaigiv.Split(',');

                                    foreach (string strto in multinew)
                                    {
                                        mailmsg.To.Add(new MailAddress(strto));
                                        StrRaiser = strto.ToString();
                                        if (toemailid=="")
                                        {
                                            toemailid = StrRaiser;
                                        }
                                        else
                                        {
                                            toemailid += "," + StrRaiser;
                                        }
                                        
                                    }

                                    if (dtMailTo.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dtMailTo.Rows.Count; i++)
                                        {
                                            StrRaiser = dtMailTo.Rows[i]["employee_office_email"].ToString();
                                            Regex regex = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
                                            Match match = regex.Match(StrRaiser);

                                            if (match.Success)
                                            {
                                                //StrRaiser = strto.ToString();
                                                mailmsg.To.Add(new MailAddress(StrRaiser));
                                                toemailid += "," + StrRaiser;
                                            }
                                            else
                                            {

                                            }
                                        }

                                    }
                                    
                                        //if (!dtreqid.Columns.Contains("ECF No"))
                                        //{
                                        //    ecfno = dtreqid.Rows[0]["ECF No"].ToString();
                                        //}
                                        // for subject 
                                        if (TransactionType == "Payment Alert Employee Claim" || TransactionType == "Payment Alert Vendor Claim")
                                        {
                                            mailmsg.Subject = objForMailEntity[0].Subject.ToString() + " " + pvno;
                                        }
                                        else
                                        {
                                            mailmsg.Subject = objForMailEntity[0].Subject.ToString() + " " + ecfno;
                                        }
                                        

                                        mailmsg.IsBodyHtml = true;
                                        mailmsg.Body = "<br >  ";
                                        if (TransactionType == "Prepaid Adjustment - Vendor Claim" || TransactionType == "Prepaid Adjustment - Employee Claim")
                                        {
                                            var SplitMyString = myString.Split(new string[] { "<p>Warm" }, StringSplitOptions.None);
                                            mailmsg.Body = mailmsg.Body + SplitMyString[0] + fillhtmltable + "<p>Warm " + SplitMyString[1];
                                        }
                                        else
                                        {
                                            mailmsg.Body = mailmsg.Body + myString + fillhtmltable + Footertext;
                                        }


                                        string regardstag = "<br /><table style='width: 100%; padding-left: 0%;'>";
                                        //DataTable adddt = new DataTable();
                                        //adddt = objModel.Getaddress();
                                        if (StrRaiser != "" && StrRaiser != "NA")
                                        {
                                            string mainsubj = objForMailEntity[0].Signature.ToString();
                                            string[] str = mainsubj.Split('.');
                                            int count = str.Length;
                                            if (count > 0)
                                            {
                                                if (TransactionType == "Payment Alert Vendor Claim" || TransactionType == "Payment Alert Employee Claim")
                                                {
                                                    string noet1 = "Kindly acknowledge on receipt of this mail to gnsa.epu.query@fullertonindia.com,";
                                                    string notes = "<tr><td style='color: black;background-color: transparent;font-size: 15px;line-height: 1.42857; text-overflow: ellipsis;'>" + noet1 + "</td></tr>";
                                                    mailmsg.Body = mailmsg.Body + notes;
                                                    regardstag += "<tr>";
                                                    for (int add = 0; count > add; add++)
                                                    {
                                                        if (add == count - 1)
                                                        {
                                                            regardstag += "<tr><td style='color: black;background-color: transparent;font-size: 15px;line-height: 1.42857; text-overflow: ellipsis;'>" + str[add] + "</td></tr>";
                                                        }
                                                        else
                                                        {
                                                            regardstag += "<tr><td style='color: black;background-color: transparent;font-size: 15px;line-height: 1.42857; text-overflow: ellipsis;'>" + str[add] + "," + "</td></tr>";
                                                        }
                                                    }
                                                    regardstag += "</tr>";
                                                    regardstag += "</table>";
                                                    mailmsg.Body = mailmsg.Body + regardstag;
                                                }
                                                else
                                                {
                                                    regardstag += "<tr>";
                                                    for (int add = 0; count > add; add++)
                                                    {
                                                        if (add == count - 1)
                                                        {
                                                            regardstag += "<tr><td style='color: black;background-color: transparent;font-size: 15px;line-height: 1.42857; text-overflow: ellipsis;'>" + str[add] + "</td></tr>";
                                                        }
                                                        else
                                                        {
                                                            regardstag += "<tr><td style='color: black;background-color: transparent;font-size: 15px;line-height: 1.42857; text-overflow: ellipsis;'>" + str[add] + "," + "</td></tr>";
                                                        }
                                                    }
                                                    regardstag += "</tr>";
                                                    regardstag += "</table>";
                                                    mailmsg.Body = mailmsg.Body + regardstag;
                                                }
                                            }
                                            else
                                            {
                                                //regardstag += "<tr><td style='font-weight:bold;font-size: 18px;'>Assuring Best Service Always<br></td></tr>";
                                                regardstag += "<tr>";
                                                // for (int add = 0; adddt.Rows.Count > add; add++)
                                                //{
                                                regardstag += "<tr><td style='color: black;background-color: transparent;font-size: 15px;line-height: 1.42857; text-overflow: ellipsis;'>" + objForMailEntity[0].Signature.ToString() + "</td></tr>";
                                                //}
                                                //regardstag += objForMailEntity[0].Signature.ToString();
                                                //regardstag += "  </td>";
                                                regardstag += "</tr>";
                                                regardstag += "</table>";
                                                mailmsg.Body = mailmsg.Body + regardstag;
                                            }

                                            Mail.EnableSsl = Convert.ToBoolean(ssll);
                                            NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                                            Mail.UseDefaultCredentials = false;
                                            Mail.Credentials = credentials;
                                            Mail.Send(mailmsg);
                                            //mailmsg.To.Remove(new MailAddress(str));
                                            string mailcont = objForMailEntity[0].Content.ToString();
                                            mailcont = mailcont.Replace("'", "");
                                            sentmail = "Sucess";
                                            string sentmailflag = objModel.mailsentmessage(objForMailEntity[0].TemplateId.ToString(), "1", gid, ecfstatus.ToString(), DateTime.Now.ToString("dd-MMM-yyyy").ToString(), toemailid.ToString(), ccmailids,bccmailids, objForMailEntity[0].Subject.ToString(), mailcont, objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "Y", objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "N");
                                        }
                                        else
                                        {
                                            sentmail = "Email id is Invalid";
                                        }
                                   
                                }


                                for (int maildt = 0; maildt < toaddress.Length; maildt++)
                                {
                                    //string str = dtMailTo.Rows[maildt]["employee_office_email"].ToString();
                                    //string mailidtousercode = dtMailTo.Rows[maildt]["employee_code"].ToString();
                                    string str = "";
                                    if (TransactionType == "Cheque Dispatch")
                                    {
                                        string[] split = toaddress[0].Split(',');
                                        str = split[0];
                                    }
                                    else
                                    {
                                        if (TransactionType != "Payment Alert Employee Claim" && TransactionType != "Payment Alert Vendor Claim")
                                        {
                                            string[] split = toaddress[maildt].Split(',');
                                            str = split[0];
                                        }
                                    }
                                   
                                    //string mailidtousercode = dtMailTo.Rows[maildt]["employee_code"].ToString();
                                    if (str != "NA" && str != "" && str != null)
                                    {
                                        mailmsg.To.Add(new MailAddress(str));
                                        mailmsg.Subject = objForMailEntity[0].Subject.ToString() + " " + ecfno;

                                        mailmsg.IsBodyHtml = true;
                                        mailmsg.Body = "<br >  ";
                                        if (TransactionType == "Payment Alert Vendor Claim" || TransactionType == "Payment Alert Employee Claim")
                                        {
                                            var SplitMyString = myString.Split(new string[] { "Kindly acknowledge" }, StringSplitOptions.None);
                                            
                                            mailmsg.Body = mailmsg.Body + SplitMyString[0] + fillhtmltable + " Kindly acknowledge " + SplitMyString[0];
                                        }
                                        else
                                        {
                                            mailmsg.Body = mailmsg.Body + myString + fillhtmltable;
                                            string name = mailmsg.Body.Trim();
                                        }
                                         string mainsubj = objForMailEntity[0].Signature.ToString();
                                        string[] str1 = mainsubj.Split('.');
                                        string regardstag = "<br /><table style='width: 100%; padding-left: 0%;'>";
                                        int count = str1.Length;
                                        if (count > 0)
                                        {
                                            regardstag += "<tr>";
                                            for (int add = 0; count > add; add++)
                                            {
                                                if (add == count - 1)
                                                {
                                                    regardstag += "<tr><td style='color: black;background-color: transparent;font-size: 15px;line-height: 1.42857; text-overflow: ellipsis;'>" + str1[add] + "</td></tr>";
                                                }
                                                else
                                                {
                                                    regardstag += "<tr><td style='color: black;background-color: transparent;font-size: 15px;line-height: 1.42857; text-overflow: ellipsis;'>" + str1[add] + "," + "</td></tr>";
                                                }
                                            }
                                            regardstag += "</tr>";
                                            regardstag += "</table>";
                                            mailmsg.Body = mailmsg.Body + regardstag;
                                        }
                                        else
                                        {
                                            
                                            // regardstag += "<tr><td style='font-weight:bold;font-size: 18px;'>Assuring Best Service Always<br></td></tr>";
                                            regardstag += "<tr><td>";
                                            regardstag += "<span style='color: black;background-color: transparent;font-size: 18px;line-height: 1.42857;'>" + objForMailEntity[0].Signature.ToString() + "</a>";
                                            regardstag += "  </td>";
                                            regardstag += "</tr>";
                                            regardstag += "</table>";
                                            mailmsg.Body = mailmsg.Body + regardstag;
                                        }
                                    
                                        Mail.EnableSsl = Convert.ToBoolean(ssll);
                                        NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                                        Mail.UseDefaultCredentials = false;

                                        Mail.Credentials = credentials;
                                        Mail.Send(mailmsg);
                                        // mailmsg.To.Remove(new MailAddress(str));

                                        sentmail = "Sucess";
                                        string sentmailflag = objModel.mailsentmessage(objForMailEntity[0].TemplateId.ToString(), "1", supplierheadergid, ecfstatus.ToString(), DateTime.Now.ToString("dd-MMM-yyyy").ToString(), str.ToString(), "", "", objForMailEntity[0].Subject.ToString(), objForMailEntity[0].Content.ToString(), objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "Y", objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()).ToString(), "N");
                                    }                                    
                                }
                            return sentmail;
                        }
                    }
                    else
                    {
                        return "template Not match";
                    }
                }
                else
                {
                    return "not sent";  
                }
                return "Sucess";
            }
            catch (Exception ex)
            {
                ErrorLog objErrorLog = new ErrorLog();
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); 
                return "faild";
            }
            finally
            {

            }
        }
        public byte[] challanPrintmail(string ecfgid, string type)
        {
            PrintModel EOWDataModel = new PrintModel();
            byte[] bytes = null;
            try
            {
                iTextSharp.text.Font fontTitle = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle1 = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle11 = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle2 = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle3 = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitlenote = FontFactory.GetFont("Arial", 6, iTextSharp.text.Font.NORMAL, BaseColor.RED);
                using (MemoryStream ms = new MemoryStream())
                {
                    Document document = new Document(PageSize.A4, 10F, 10F, 10F, 0F);
                    PdfWriter writer = PdfWriter.GetInstance(document, ms);
                    document.Open();

                    printdatamodel objModel = new printdatamodel();
                    if (type == "S")
                    {
                        objModel = EOWDataModel.SelectEmployeeSearch(ecfgid, "S");
                    }
                    else
                    {
                        objModel = EOWDataModel.SelectPrintGeneral(ecfgid, "E");
                    }

                    document.NewPage();
                    PdfPTable HeaderTable = new PdfPTable(10);
                    HeaderTable.WidthPercentage = 100;

                    //--------------------------------------------ADD Invoice Details---------------------------------------
                    // BackgroundColor = BaseColor.LIGHT_GRAY 
                    PdfPCell Headerlogo = new PdfPCell();
                    string Headerlogoath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/Content/images"), "Fullerton.bmp");
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Headerlogoath);
                    image.WidthPercentage = 90;
                    Headerlogo.AddElement(image);
                    Headerlogo.HorizontalAlignment = Element.ALIGN_CENTER;
                    Headerlogo.VerticalAlignment = Element.ALIGN_MIDDLE;
                    Headerlogo.Colspan = 3;
                    Headerlogo.Rowspan = 2;

                    string ecfno = objModel.Ecf_No;
                    PdfContentByte cb = new PdfContentByte(writer);
                    Barcode128 code128 = new Barcode128();
                    code128.CodeType = Barcode.CODE128_UCC;
                    code128.Code = ecfno;
                    iTextSharp.text.Image image128 = code128.CreateImageWithBarcode(cb, null, null);
                    image128.ScaleAbsolute(110f, 155.25f);

                    PdfPCell ECFNo = new PdfPCell();
                    ECFNo.HorizontalAlignment = Element.ALIGN_CENTER;
                    ECFNo.VerticalAlignment = Element.ALIGN_MIDDLE;
                    ECFNo.AddElement(image128);
                    HeaderTable.AddCell(new PdfPCell(new Phrase(objModel.Header, fontTitle2)) { Colspan = 7, Rowspan = 2, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    HeaderTable.AddCell(Headerlogo);

                    PdfPTable t = new PdfPTable(9);
                    t.WidthPercentage = 100;

                    t.AddCell(new PdfPCell(new Phrase(objModel.Ecf_No, fontTitle3)) { Rowspan = 3, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                    t.AddCell(new PdfPCell(new Phrase("Employee ID", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.EmployeeId, fontTitle1)) { Colspan = 2 });
                    t.AddCell(new PdfPCell(new Phrase("Invoice Type", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.Invoice_type, fontTitle1)));

                    t.AddCell(new PdfPCell(new Phrase("ECF Date", fontTitle1)) { Colspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                    t.AddCell(new PdfPCell(new Phrase("Name", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.EmployeeName, fontTitle1)) { Colspan = 2 });
                    t.AddCell(new PdfPCell(new Phrase("Expense Type", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.Expense_type, fontTitle1)));

                    t.AddCell(new PdfPCell(new Phrase(objModel.Ecf_Date, fontTitle3)) { Rowspan = 2, Colspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                    t.AddCell(new PdfPCell(new Phrase("Title", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.Title, fontTitle1)) { Colspan = 2 });
                    t.AddCell(new PdfPCell(new Phrase("Amort", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.Ecf_Amort, fontTitle1)));

                    t.AddCell(new PdfPCell(ECFNo) { Rowspan = 3, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingLeft = 7f, PaddingBottom = 7f, PaddingTop = 7f, PaddingRight = 7f });

                    t.AddCell(new PdfPCell(new Phrase("Department", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.Department, fontTitle1)) { Colspan = 2 });
                    t.AddCell(new PdfPCell(new Phrase("For Ex", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.Ecf_forex, fontTitle1)));

                    t.AddCell(new PdfPCell(new Phrase("ECF Amount", fontTitle1)) { Colspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                    t.AddCell(new PdfPCell(new Phrase("Location Code", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.LocationCode, fontTitle1)) { Colspan = 2 });
                    t.AddCell(new PdfPCell(new Phrase("Utility", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.Ecf_Utility, fontTitle1)));

                    t.AddCell(new PdfPCell(new Phrase(objModel.Ecf_Amount, fontTitle3)) { Rowspan = 2, Colspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                    t.AddCell(new PdfPCell(new Phrase("PO / WO No.", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.Invoice_powono, fontTitle1)) { Colspan = 2 });
                    t.AddCell(new PdfPCell(new Phrase("Payment Adjst", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.Ecf_PaymentAdjst, fontTitle1)));

                    //--------------------------------------------ADD Invoice Details---------------------------------------
                    Font font8 = FontFactory.GetFont("ARIAL", 7);
                    PdfPTable InvoiceTable = new PdfPTable(5);
                    PdfPCell InvoicePCell = null;
                    if (type == "S")
                    {
                        InvoiceTable.AddCell(new PdfPCell(new Phrase("Invoice Details ", fontTitle11)) { Colspan = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                        InvoiceTable.AddCell(new PdfPCell(new Phrase("Vendor Code", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                        InvoiceTable.AddCell(new PdfPCell(new Phrase("Vendor Name", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                        InvoiceTable.AddCell(new PdfPCell(new Phrase("Invoice No.", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                        InvoiceTable.AddCell(new PdfPCell(new Phrase("Invoice Date", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                        InvoiceTable.AddCell(new PdfPCell(new Phrase("Invoice Amount", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                        DataTable dtinvoice = new DataTable();
                        dtinvoice = EOWDataModel.GetSupplieinvoice(ecfgid);
                        decimal tolamtinvoice = 0;
                        for (int rows = 0; rows < dtinvoice.Rows.Count; rows++)
                        {
                            InvoicePCell = new PdfPCell(new Phrase(new Chunk(dtinvoice.Rows[rows]["supplierheader_suppliercode"].ToString(), font8)));
                            InvoiceTable.AddCell(InvoicePCell);
                            InvoicePCell = new PdfPCell(new Phrase(new Chunk(dtinvoice.Rows[rows]["supplierheader_name"].ToString(), font8)));
                            InvoiceTable.AddCell(InvoicePCell);
                            InvoicePCell = new PdfPCell(new Phrase(new Chunk(dtinvoice.Rows[rows]["invoice_no"].ToString(), font8)));
                            InvoiceTable.AddCell(InvoicePCell);
                            InvoicePCell = new PdfPCell(new Phrase(new Chunk(dtinvoice.Rows[rows]["invoice_date"].ToString(), font8)));
                            InvoiceTable.AddCell(InvoicePCell);
                            InvoiceTable.AddCell(new PdfPCell(new Phrase(objCmnFunctions.GetINRAmount(dtinvoice.Rows[rows]["invoice_amount"].ToString()), font8)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });
                            if (tolamtinvoice == 0)
                            {
                                tolamtinvoice = Convert.ToDecimal(dtinvoice.Rows[rows]["invoice_amount"].ToString());
                            }
                            else
                            {
                                tolamtinvoice = tolamtinvoice + Convert.ToDecimal(dtinvoice.Rows[rows]["invoice_amount"].ToString());
                            }
                        }
                        InvoiceTable.AddCell(new PdfPCell(new Phrase("TOTAL FROM INVOICE TOTAL", fontTitle11)) { Colspan = 4, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE });
                        InvoiceTable.AddCell(new PdfPCell(new Phrase(objCmnFunctions.GetINRAmount(tolamtinvoice.ToString()), fontTitle11)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });
                        InvoiceTable.SpacingBefore = 15f;
                        InvoiceTable.WidthPercentage = 100;
                    }
                    //--------------------------------------------ADD Expense Details---------------------------------------
                    PdfPTable ExpenseTable = new PdfPTable(8);
                    PdfPCell ExpensePCell = null;
                    ExpenseTable.AddCell(new PdfPCell(new Phrase("Expense / Asset Details", fontTitle11)) { Colspan = 8, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    ExpenseTable.AddCell(new PdfPCell(new Phrase("Nature of Expense / Asset Description", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    ExpenseTable.AddCell(new PdfPCell(new Phrase("Category", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    ExpenseTable.AddCell(new PdfPCell(new Phrase("Sub Category", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    ExpenseTable.AddCell(new PdfPCell(new Phrase("Product Code", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    ExpenseTable.AddCell(new PdfPCell(new Phrase("Business Segment", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    ExpenseTable.AddCell(new PdfPCell(new Phrase("Cost Center", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    ExpenseTable.AddCell(new PdfPCell(new Phrase("Branch Code", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    ExpenseTable.AddCell(new PdfPCell(new Phrase("Debit Amount", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                    DataTable dtDebit = new DataTable();
                    dtDebit = EOWDataModel.GetprintExpensesupplier(ecfgid.ToString());
                    decimal tolamtDebit = 0;
                    for (int rows = 0; rows < dtDebit.Rows.Count; rows++)
                    {
                        if (Convert.ToString(dtDebit.Rows[rows]["ecfdebitline_category_type"].ToString()) == "A")
                        {
                            ExpensePCell = new PdfPCell(new Phrase(new Chunk("", font8)));
                            ExpenseTable.AddCell(ExpensePCell);
                            ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["assetcategory_name"].ToString(), font8)));
                            ExpenseTable.AddCell(ExpensePCell);
                            ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["asset_description"].ToString(), font8)));
                            ExpenseTable.AddCell(ExpensePCell);
                        }
                        else
                        {
                            ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["expnature_name"].ToString(), font8)));
                            ExpenseTable.AddCell(ExpensePCell);
                            ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["expcat_name"].ToString(), font8)));
                            ExpenseTable.AddCell(ExpensePCell);
                            ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["expsubcat_name"].ToString(), font8)));
                            ExpenseTable.AddCell(ExpensePCell);
                        }
                        ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["ecfdebitline_product_code"].ToString(), font8)));
                        ExpenseTable.AddCell(ExpensePCell);
                        ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["ecfdebitline_fc_code"].ToString(), font8)));
                        ExpenseTable.AddCell(ExpensePCell);
                        ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["ecfdebitline_cc_code"].ToString(), font8)));
                        ExpenseTable.AddCell(ExpensePCell);
                        ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["ecfdebitline_ou_code"].ToString(), font8)));
                        ExpenseTable.AddCell(ExpensePCell);
                        ExpenseTable.AddCell(new PdfPCell(new Phrase(objCmnFunctions.GetINRAmount(dtDebit.Rows[rows]["ecftravel_amount"].ToString()), font8)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });
                        if (tolamtDebit == 0)
                        {
                            tolamtDebit = Convert.ToDecimal(dtDebit.Rows[rows]["ecftravel_amount"].ToString());
                        }
                        else
                        {
                            tolamtDebit = tolamtDebit + Convert.ToDecimal(dtDebit.Rows[rows]["ecftravel_amount"].ToString());
                        }
                    }
                    ExpenseTable.AddCell(new PdfPCell(new Phrase("TOTAL FROM DEBIT TOTAL", fontTitle11)) { Colspan = 7, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE });
                    ExpenseTable.AddCell(new PdfPCell(new Phrase(objCmnFunctions.GetINRAmount(tolamtDebit.ToString()), fontTitle11)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });
                    ExpenseTable.SpacingBefore = 15f;
                    ExpenseTable.WidthPercentage = 100;

                    //--------------------------------------------ADD Payment Details---------------------------------------
                    PdfPTable PaymentTable = new PdfPTable(5);
                    PdfPCell PaymentPCell = null;
                    PaymentTable.AddCell(new PdfPCell(new Phrase("Payment Details", fontTitle11)) { Colspan = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    PaymentTable.AddCell(new PdfPCell(new Phrase("Pay To", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    PaymentTable.AddCell(new PdfPCell(new Phrase("Beneficiary Name", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    PaymentTable.AddCell(new PdfPCell(new Phrase("Pay Mode", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    PaymentTable.AddCell(new PdfPCell(new Phrase("Bank A/C No./Payment Ref No.", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    PaymentTable.AddCell(new PdfPCell(new Phrase("Credit Amount", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                    DataTable dtPayment = new DataTable();
                    dtPayment = EOWDataModel.GetprintPayment(ecfgid.ToString());
                    decimal tolamtPayment = 0;
                    for (int rows = 0; rows < dtPayment.Rows.Count; rows++)
                    {
                        string empsup = dtPayment.Rows[rows]["ecf_supplier_employee"].ToString();
                        if (empsup == "E")
                        {
                            empsup = "Employee";
                        }
                        else
                        {
                            empsup = "Supplier";
                        }
                        PaymentPCell = new PdfPCell(new Phrase(new Chunk(empsup, font8)));
                        PaymentTable.AddCell(PaymentPCell);
                        PaymentPCell = new PdfPCell(new Phrase(new Chunk(dtPayment.Rows[rows]["ecfcreditline_beneficiary"].ToString(), font8)));
                        PaymentTable.AddCell(PaymentPCell);
                        PaymentPCell = new PdfPCell(new Phrase(new Chunk(dtPayment.Rows[rows]["ecfcreditline_pay_mode"].ToString(), font8)));
                        PaymentTable.AddCell(PaymentPCell);
                        PaymentPCell = new PdfPCell(new Phrase(new Chunk(dtPayment.Rows[rows]["ecfcreditline_ref_no"].ToString(), font8)));
                        PaymentTable.AddCell(PaymentPCell);
                        PaymentTable.AddCell(new PdfPCell(new Phrase(objCmnFunctions.GetINRAmount(dtPayment.Rows[rows]["ecfcreditline_amount"].ToString()), font8)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });

                        if (tolamtPayment == 0)
                        {
                            tolamtPayment = Convert.ToDecimal(dtPayment.Rows[rows]["ecfcreditline_amount"].ToString());
                        }
                        else
                        {
                            tolamtPayment = tolamtPayment + Convert.ToDecimal(dtPayment.Rows[rows]["ecfcreditline_amount"].ToString());
                        }
                    }
                    PaymentTable.AddCell(new PdfPCell(new Phrase("TOTAL FROM CREDIT TOTAL", fontTitle11)) { Colspan = 4, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE });
                    PaymentTable.AddCell(new PdfPCell(new Phrase(objCmnFunctions.GetINRAmount(tolamtPayment.ToString()), fontTitle11)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });
                    PaymentTable.SpacingBefore = 10f;
                    PaymentTable.WidthPercentage = 100;

                    PdfPTable tableamtdesc = new PdfPTable(2);
                    tableamtdesc.WidthPercentage = 100;
                    tableamtdesc.SpacingBefore = 10f;
                    tableamtdesc.AddCell(new PdfPCell(new Phrase("Amount in Words", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableamtdesc.AddCell(new PdfPCell(new Phrase("ECF Description", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableamtdesc.AddCell(new PdfPCell(new Phrase(objModel.Ecf_Amountinword, font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 10f });
                    tableamtdesc.AddCell(new PdfPCell(new Phrase(objModel.Ecf_dcsc, font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 10f });

                    PdfPTable tableDeclar = new PdfPTable(2);
                    tableDeclar.WidthPercentage = 100;
                    tableDeclar.SpacingBefore = 10f;
                    tableDeclar.AddCell(new PdfPCell(new Phrase("Declaration", fontTitle11)) { Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    tableDeclar.AddCell(new PdfPCell(new Phrase("Raiser Declaration", fontTitle11)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = PdfPCell.LEFT_BORDER });
                    tableDeclar.AddCell(new PdfPCell(new Phrase("Verifier Declaration", fontTitle11)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER });
                    tableDeclar.AddCell(new PdfPCell(new Phrase("I confirm that I have incurred the expenditure wholly and exclusively for business purposes only.", font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 2f, Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER });
                    tableDeclar.AddCell(new PdfPCell(new Phrase("I have verified the expense claim and approved on the basis that this Expense is incurred wholly and exclusively for Business Purposes.", font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 2f, Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER });

                    //--------------------------------------------ADD Authorization Approvals Details---------------------------------------

                    PdfPTable AuthorizationTable = new PdfPTable(5);
                    PdfPCell AuthorizationPCell = null;
                    AuthorizationTable.AddCell(new PdfPCell(new Phrase("ECF Authorization Details", fontTitle11)) { Colspan = 8, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    AuthorizationTable.AddCell(new PdfPCell(new Phrase("Emp Code", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    AuthorizationTable.AddCell(new PdfPCell(new Phrase("Emp Name", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    AuthorizationTable.AddCell(new PdfPCell(new Phrase("Action Date", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    AuthorizationTable.AddCell(new PdfPCell(new Phrase("Status", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    AuthorizationTable.AddCell(new PdfPCell(new Phrase("Remarks", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                    EOW_DataModel objd = new EOW_DataModel();
                    List<ApproverHistry> lstaHistory = new List<ApproverHistry>();
                    lstaHistory = objd.GetecfappHistory(ecfgid.ToString(), ecfgid.ToString()).ToList();

                    for (int rows = 0; rows < lstaHistory.Count; rows++)
                    {
                        AuthorizationPCell = new PdfPCell(new Phrase(new Chunk(lstaHistory[rows].empcode, font8)));
                        AuthorizationTable.AddCell(AuthorizationPCell);
                        AuthorizationPCell = new PdfPCell(new Phrase(new Chunk(lstaHistory[rows].empname, font8)));
                        AuthorizationTable.AddCell(AuthorizationPCell);
                        AuthorizationPCell = new PdfPCell(new Phrase(new Chunk(lstaHistory[rows].statusdate, font8)));
                        AuthorizationTable.AddCell(AuthorizationPCell);
                        AuthorizationPCell = new PdfPCell(new Phrase(new Chunk(lstaHistory[rows].status, font8)));
                        AuthorizationTable.AddCell(AuthorizationPCell);
                        AuthorizationPCell = new PdfPCell(new Phrase(new Chunk(lstaHistory[rows].remarks, font8)));
                        AuthorizationTable.AddCell(AuthorizationPCell);
                    }
                    AuthorizationTable.SpacingBefore = 15f;
                    AuthorizationTable.WidthPercentage = 100;


                    //PdfPTable tableSign = new PdfPTable(4);
                    //tableSign.WidthPercentage = 100;
                    //tableSign.SpacingBefore = 10f;
                    //tableSign.AddCell(new PdfPCell(new Phrase("EPU Authentication", fontTitle11)) { Colspan = 4, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    //tableSign.AddCell(new PdfPCell(new Phrase("", font8)) { Colspan = 4, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = 10f, Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER });
                    //tableSign.AddCell(new PdfPCell(new Phrase("Emp ID & Emp Name ", font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 30f, Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER });

                    //tableSign.AddCell(new PdfPCell(new Phrase("Maker ID", font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 30f, Border = PdfPCell.BOTTOM_BORDER });
                    //tableSign.AddCell(new PdfPCell(new Phrase("Authoriser ID", font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 30f, Border = PdfPCell.BOTTOM_BORDER });
                    //tableSign.AddCell(new PdfPCell(new Phrase("Verifier ID & Name", font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 30f, Border = PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER });

                    PdfPTable tablenote = new PdfPTable(1);
                    tablenote.WidthPercentage = 100;
                    tablenote.SpacingBefore = 10f;
                    tablenote.AddCell(new PdfPCell(new Phrase("* This is an electronically generated form and does not require signature. The IDs mentioned there in tantamounts to signature of the relevant employees", fontTitlenote)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = 0 });

                    document.Add(HeaderTable);
                    document.Add(t);
                    if (type == "S")
                    {
                        document.Add(InvoiceTable);
                    }
                    document.Add(ExpenseTable);
                    document.Add(PaymentTable);
                    document.Add(tableamtdesc);
                    document.Add(tableDeclar);
                    document.Add(AuthorizationTable);
                    //document.Add(tableSign);
                    document.Add(tablenote);
                    document.Close();
                    writer.Close();

                    bytes = ms.ToArray();
                    return bytes;
                }
            }
            catch (Exception ex)
            {
                return bytes;
            }
            return bytes;
        }


         public ActionResult excelexport()
        {
            List<ForMailEntity> MailList = new List<ForMailEntity>();
            MailList = (List<ForMailEntity>)Session["Mailexport"]; 



            DataTable dt = new DataTable();
            dt.Columns.Add("S.No");
            dt.Columns.Add("Template Name");
            dt.Columns.Add("Module Name");
            dt.Columns.Add("Transaction Type");
            dt.Columns.Add("Trigger Type");
            dt.Columns.Add("Stage Type");



            for (int i = 0; i < MailList.Count; i++)
            {
                dt.Rows.Add(i + 1,
                    MailList[i].TemplateName.ToString(),
                    MailList[i].ModuleName.ToString(),
                    MailList[i].TransactionTypeName.ToString(),
                    MailList[i].TriggerTypeName.ToString(),
                    MailList[i].WorkFlowName.ToString()              
                                                           );
            }


            string attachment = "attachment; filename=BranchReport.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            string tab = "";
            string strquery = "";

            if (dt.Rows.Count > 0)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    Response.Write(tab + dc.ColumnName);
                    tab = "\t";
                }
                Response.Write("\n");
                int k;
                foreach (DataRow dr in dt.Rows)
                {
                    tab = "";
                    for (k = 0; k < dt.Columns.Count; k++)
                    {
                       
                            Response.Write(tab + dr[k].ToString());
                            tab = "\t";
                       
                    }
                    Response.Write("\n");
                }
                Response.End();

            }
            return RedirectToAction("Index");



        }
    }
}
