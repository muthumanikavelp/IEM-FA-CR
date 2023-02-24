using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using IEM.Common;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
namespace IEM.Helper
{
    public class CustomValidateAntiForgeryToken : FilterAttribute, IAuthorizationFilter
    {
        ErrorLog objErrorLog = new ErrorLog();
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            string antiForgeryCookie = "";
            //try
            //{
            //if (filterContext.HttpContext.Request.HttpMethod == "POST" && (!filterContext.HttpContext.Request.Url.AbsolutePath.Equals("/iem")) &&
            //    (!filterContext.HttpContext.Request.Url.AbsolutePath.Equals("/")) && (!filterContext.HttpContext.Request.Url.AbsolutePath.Contains("/Loginpage/empLoginPage")) &&
            //    (!filterContext.HttpContext.Request.Url.AbsolutePath.Equals("/IEM/IEMCommon/Welcome")))
            //{
            Boolean Flag = true;
            string url = filterContext.HttpContext.Request.Url.AbsolutePath.ToUpper();
            if (url.Contains("/LOGINPAGE/EMPLOGINPAGE") || url.Equals("/"))
            {
                Flag = false;
            }
            if (System.Configuration.ConfigurationManager.AppSettings["CSRF"] == "Enable")
            {
                var regexItem = new Regex(@"[~`!@#$%^&*()-+=|\{}':;.,<>/?]");
                //var regexItem = new Regex("^[a-zA-Z0-9]*$");
                /*string ab = "ramya*^()";
                string cd = "ramya 100 ";
                var regexItem = new Regex(@"[~`!@#$%^&*()-+=|\{}':;.,<>/?]");
                Match match = Regex.Match(cd, "[^a-z0-9]",
           RegexOptions.IgnoreCase);
                if(match.Success)
                {
                    string key = match.Value;
                    Console.Write(key);
                    match = match.NextMatch();
                } 
             
               
                if(regexItem.IsMatch(ab))
                {
                    Console.WriteLine("string contains special char"); 
                }
                else
                {
                    Console.WriteLine("string not contains special char"); 
                }
                if (regexItem.IsMatch(cd))
                {
                    Console.WriteLine("string contains special char"); 
                }
                else
                {
                    Console.WriteLine("string not contains special char"); 
                }
                if (filterContext.HttpContext.Request.Cookies.ToString().Contains("/[^a-zA-Z0-9.&$#%]/g"))
                {
                    filterContext.Result = new RedirectResult("~/Loginpage/empLoginPage");
                }
                else
                {
                    object a = filterContext.HttpContext.Request.Cookies;
                }
                //Ramya
               /* var form = filterContext.HttpContext.Request.Form;
                var dictionary = form.AllKeys.ToDictionary(k => k, k => form[k]);
                //var jsonPostedData = JsonConvert.SerializeObject(dictionary);
                //int count = dictionary.Count;
                Boolean regexpflag = dictionary.ContainsValue("*");
                Boolean regexpflag1 = dictionary.ContainsValue("p");
                if (regexpflag==true)
                {
                    filterContext.Result = new RedirectResult("~/Loginpage/empLoginPage");
                }

                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                     var viewResult = new JsonResult();
                    if (viewResult.Data!=null)
                    {
                        if (viewResult.Data.ToString().Contains("*") == true)
                        {
                            filterContext.Result = new RedirectResult("~/Loginpage/empLoginPage");
                        }
                    } 
                    if (filterContext.RouteData.Values.ToString().Contains("*") == true)
                    {
                        filterContext.Result = new RedirectResult("~/Loginpage/empLoginPage");
                    }
                    
                    
                    //viewResult.Data = (new { IsSuccess = "Unauthorized", description = "You are not authorized to perform this Action." });
                    //filterContext.Result = viewResult;
                }*/

               /* for (int i = 0; i <= count;i++ )
                {
                    string val=dictionary.
                }*/
                    /*if (filterContext.HttpContext.Request.UrlReferrer == null)
                    {*/
                        // if (!url.Contains("FLEXISPEND") && !url.Contains("FLEXIBUY/REPORT") && !url.Contains("HELPER") && !url.Contains("POUCH"))
                        if (url.Contains("EOW") || url.Contains("FLEXIBUY"))
                        {
                            //return false;
                            /*      if (filterContext.HttpContext.Request.Form["Login"] == null)
                                  {*/
                                      if (filterContext.HttpContext.Request.HttpMethod != "GET")
                                      {
                                          if (filterContext.RouteData.Values.ToString().Contains("*") == true)
                                          {
                                              filterContext.Result = new RedirectResult("~/Loginpage/empLoginPage");
                                          }
                                          //int count=filterContext.HttpContext.Request.Params.Count;
                                          //for (int i = 0; i < count;i++ )
                                          //{
                                          //    if(regexItem.IsMatch(filterContext.HttpContext.Request.Params[i]))
                                          //    {
                                          //        filterContext.Result = new RedirectResult("~/Loginpage/empLoginPage");
                                          //    }
                                          //}
                                              /*antiForgeryCookie = filterContext.HttpContext.Request.Cookies[AntiForgeryConfig.CookieName].Name.ToString();
                                              if (filterContext.HttpContext.Request.IsAjaxRequest())
                                              {
                                                  antiForgeryCookie = filterContext.HttpContext.Request.Cookies[AntiForgeryConfig.CookieName].Name.ToString();
                                                  objErrorLog.WriteErrorLog(antiForgeryCookie, antiForgeryCookie);
                                                  AntiForgery.Validate(filterContext.HttpContext.Request.Cookies[antiForgeryCookie].Value, filterContext.HttpContext.Request.Headers["__RequestVerificationToken"]);
                                              }
                                              else
                                              {
                                                  AntiForgery.Validate();
                                              }*/
                                              /*if (filterContext.HttpContext.Request.Cookies.ToString().Contains("/[^a-zA-Z0-9.&$#%]/g"))
                                              {
                                                  filterContext.Result = new RedirectResult("~/Loginpage/empLoginPage");
                                              }*/

                                      }
                                  }
                             // } 
                        /*}
                        else if (Flag == true)
                        {
                            filterContext.Result = new RedirectResult("~/Loginpage/empLoginPage");
                        }*/
                    }

                //catch (Exception ex)
                //{
                //    //System.IO.File.AppendAllText(System.Configuration.ConfigurationManager.AppSettings["Antiforgerylog"], "\n Catch:" + ex.Message + "\n Cookie:" + filterContext.HttpContext.Request.Cookies[antiForgeryCookie].Value + "\n Form:" + filterContext.HttpContext.Request.Headers["__RequestVerificationToken"] + "\n DATE&TIME" + DateTime.Now.ToString());
                //    objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                //    //throw new HttpAntiForgeryException("Anti forgery token cookie not found" + ex.Message);                  
                //    //throw new ArgumentNullException(ex.Message);
                //}
            }
        }
    }
 