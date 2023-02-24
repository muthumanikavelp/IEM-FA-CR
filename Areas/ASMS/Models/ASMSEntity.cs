using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.MASTERS.Models;

namespace IEM.Areas.ASMS.Models
{
    public class ASMSEntity
    {

    }
    public class SheetData
    {
        public int SheetId { get; set; }
        public string SheetName { get; set; }
    }

    public class SERate
    {
        public SelectListItem seScoreModel { get; set; }
        public SelectList seRateModel { get; set; }
        public Int32 seRateID { get; set; }
        public string seRateName { get; set; }
        public string seRatescore { get; set; }
    }

    public class SEModel
    {
        public SelectListItem seScoreModel { get; set; }
        public SelectList seRateModel { get; set; }
        public Int32 seRateID { get; set; }
        public string seRateName { get; set; }
        public string seRateScore { get; set; }
        public List<SERate> seRatList1 { get; set; }
        public List<SERate> seRateList6 { get; set; }
        public Int32 yearDiff { get; set; }
        public Int32 sNo { get; set; }
        public Int32 seId { get; set; }
        public string seStatus { get; set; }
        public int seYear { get; set; }
        public SelectList year { get; set; }
        public List<SEModel> modelPList { get; set; }
        public List<SEModel> modelCList { get; set; }


        public Int32 seSupID { get; set; }
        public string seSupName { get; set; }
        public string seSupCode { get; set; }
        public string ownerName { get; set; }
        public string ownerCode { get; set; }
        public string ownerNameNew { get; set; }
        public string ownerCodeNew { get; set; }

        public SelectList seSubCategoryModel { get; set; }
        public Int32 seSubCategoryID { get; set; }
        public string seSubCategoryName { get; set; }
        public List<SESubCategoryModel> seSubcatModelList { get; set; }
        public SelectList strSheet { get; set; }
        public SelectList seCategoryModel { get; set; }
        public Int32 seCategoryID { get; set; }
        public string seCategoryName { get; set; }
        public List<SECategoryModel> seCatModelList { get; set; }
        public string[] rate { get; set; }

        public SelectList seReviewerModel { get; set; }
        public Int32 seReviewerID { get; set; }
        public string seReviewerCode { get; set; }
        public string seReviewerName { get; set; }
        public Int32 seProcurementID { get; set; }
        public string seProcurementCode { get; set; }
        public string seProcurementName { get; set; }
        public string seReviewerComments { get; set; }
        public string seOverallScore { get; set; }
        public string seOverAllRating { get; set; }
        public string seReviewYear { get; set; }

        // public SelectList seChildRate { get; set; }
        public Int32 seChildRateID { get; set; }
        public Int32 seChildRate_RateID { get; set; }
        public Int32 seChildRate_QuestionID { get; set; }
        public string seChildRateName { get; set; }
        public List<SEModel> seChildRateModelList { get; set; }
        public string SheetName { get; set; }
    }
    public class ReportModel
    {
        public Int32 vendorId { get; set; }
        public Int32 stateID { get; set; }
        public Int32 cityID { get; set; }
        public Int32 ownerID { get; set; }
        public Int32 procruCoId { get; set; }
        public Int32 categoryID { get; set; }
        public Int32 serviceId { get; set; }
        public Int32 AgreementId { get; set; }        
        public string ownerDepartment { get; set; }
        public string ownerName { get; set; }
        public string ownerEmployeeCode { get; set; }
        public string ownerEmailID { get; set; }
        public string coordinatorDepartment { get; set; }
        public string coordinatorName { get; set; }
        public string coordinatorcode { get; set; }
        public string coordinatorEmailID { get; set; }
        public string categoryName { get; set; }
        public string procurableNonProcurablevendor { get; set; }
        public string natureofServicesCategory { get; set; }
        public string natureofServicesSubCategory { get; set; }
        public string vendorName { get; set; }
        public string vendorCode { get; set; }
        public string vendorAddress { get; set; }
        public string vendorState { get; set; }
        public string vendorCity { get; set; }
        public string vendorEmpanelmentDate { get; set; }
        public string vendorCoordinatorName { get; set; }
        public string vendorMobileNumber { get; set; }
        public string vendorOfficeNumber { get; set; }
        public string vendorEmailID { get; set; }
        public string vendorWebsiteAddress { get; set; }
        public string typeofAgreement { get; set; }
        public string originalAgreementStatus { get; set; }
        public string agreementEndDate { get; set; }
        public string agreementRenewalStatus { get; set; }
        public string finalvendorStatus { get; set; }
        public string vendorValidityDate { get; set; }
        public string paymenttobeProcessed { get; set; }
        public string pANNumber { get; set; }
        public string procurementName { get; set; }
        public string procurementEmployeeCode { get; set; }
        public string procurementEmailID { get; set; }
        public string remarks { get; set; }
        public List<ReportModel> reportList { get; set; }

        public SelectList categoryddl { get; set; }
        public SelectList stateddl { get; set; }
        public SelectList servicetypeddl { get; set; }
        public SelectList cityddl { get; set; }
        public SelectList contractddl { get; set; }

    }

    public class adhoc_model
    {

        public Int64 Auditgid { get; set; }
        public string Audit_supplierheader_gid { get; set; }
        public string Audit_suppliercode { get; set; }
        public string Audit_suppliername { get; set; }
        public string Audit_suppliergstin { get; set; }
        public Int64 Audit_stategid { get; set; }
        public string audit_type { get; set; }
        public string Audit_remarks { get; set; }
        public string Audit_isremoved { get; set; }
        public Int64 Audit_insertby { get; set; }
        public DateTime Audit_insertdate { get; set; }
        public Int64 Audit_updateby { get; set; }
        public DateTime Audit_updatedate { get; set; }
        public string statename { get; set; }
        public Int64 supplierheader_gid { get; set; }
        public string gststatecode { get; set; }
        public string user { get; set; }

    }



}