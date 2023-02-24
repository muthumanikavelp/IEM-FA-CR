using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IEM.Models
{
    public class IEMModel
    {
    }
    public class MenuModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string url { get; set; }
        public int ParentId { get; set; }
        public int SortOrder { get; set; }
    }
    public class LoginPage
    {
        public string employeeId { get; set; }
        public string passward { get; set; }
        public int employee_gid { get; set; }
        public string employee_code { get; set; }
        public string employee_name { get; set; }
        public SelectList GetProxyDetails { get; set; }
        public int employeegid { get; set; }
        public string empname { get; set; }
    }
    public class GetProxyDetails
    {
        public string employeegid { get; set; }
        public string empname { get; set; }
    }
    public class RaiseTicketEntity
    {
        public int TicketId { get; set; }
        public string TicketNumber { get; set; } 
        public int TicketFromID { get; set; } 
        public int TicketToID { get; set; }
        public string TicketFromName { get; set; }
        public string TicketToName { get; set; }
        public string TicketContent { get; set; } 
        public string TicketDate { get; set; }
        public string DeleteDateFrom { get; set; }
        public string DeleteDateTo{ get; set; }
        public string ReplyFlag { get; set; }
        public string RefFlag { get; set; }
        public string ItemGid { get; set; }
        public string ItemRefNumber { get; set; }
        public string TicketFromGender { get; set; }
    }
    public class NewRaiseTicketList
    {
        public List<RaiseTicketEntity> lstTickets { get; set; }
        public List<RaiseTicketEntity> lstReplies { get; set; } 
    }
    public class PolicyDataModel
    {
        public string PolicyName { get; set; }
        public string FileName { get; set; }
        public string DownloadPath { get; set; }
        public int PolicyGid { get; set; } 
    }
    public static class Emp
    {
        public static string empcodeST { get; set; }
        public static int empgidST { get; set; }
        public static string IPAddress { get; set; }
        public static string BrowserName { get; set; }
        public static int SessionStatus { get; set; }  
    }
}