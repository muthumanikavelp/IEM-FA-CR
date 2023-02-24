using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using System.Web;


namespace IEM.Areas.ASMS.Models
{
    public interface ASMSIRepository
    {
        IEnumerable<SEModel> GetSEModelPending(string filter, int reviewer);
        SEModel GetSEModelPendingById(int SESEModel);
        IEnumerable<SEModel> GetSEModelCompleted(string filter, int reviewe);
        SEModel GetSEModelCompletedById(int SESEModel);
        string GetLoginUserById(int gid);
        IEnumerable<SERate> GetSErate();
        IEnumerable<SERate> GetSEratedrop();
        IEnumerable<SERate> Getscorebygrp(int id);
        string GetChildRate(int questiongid, int rategid); 
        string GetSupbyId(int supID);            
        string InsertReview(SEModel model);
        string GetStatusexcel(string supdata, string valatate,string action);
        string UpdateOwner(DataTable datatable);
        IEnumerable<SupplierHeader> GetStatusGridSupplierDetails();
        List<SearchEmployee> GetEmployeeList();
        string UpdateOnwer(SupplierHeader model);        
        DataTable GetReviwerDetails(int revirewid);
        IEnumerable<ReportModel> Getcategory();
        IEnumerable<ReportModel> Getservicetype();
        IEnumerable<ReportModel> Getcity();
         IEnumerable<ReportModel> Getstate();
         IEnumerable<ReportModel> Getcontract();
        List<ReportModel> GetReport(string ownerName, string ownerCode, string vendorName, string vendorCode
            , string contractType, string categoryName, string contractEndDate, string serviceTypeName
              , string employeeCode, string employeeName, string city, string state,string search );
    }
}