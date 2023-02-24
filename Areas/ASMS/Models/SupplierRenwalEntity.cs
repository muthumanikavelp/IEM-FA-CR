using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IEM.Areas.ASMS.Models
{
    public class SupplierRenwalEntity
    {

    }
    public class SupOrganizationType
    {
        public int _OrganizationTypeID { get; set; }
        [Required(ErrorMessage = "Please Enter OrganizationName Name.")]
        public string _OrganizationName { get; set; }
    }

    public class SupplierContractRenewal
    {
        public List<SupplierContractRenewal> SupplierRenewal { get; set; }
        public string SupplierheadGid { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string SupplierClassfication { get; set; }
        public string RquestType { get; set; }
        public string RquestStatus { get; set; }
        public string SupplierStatus { get; set; }
        public string ActivityCount { get; set; }
        public string Cont_startDate { get; set; }
        public string Cont_EndDate { get; set; }
        public string Cont_EndDateold { get; set; }
        public string Cont_EndDateNew { get; set; }
        public string AttachmentType { get; set; }
        public string Description { get; set; }
        // public HttpPostedFileBase file { get; set; }
        public string file { get; set; }
        public string filename { get; set; }

    }

    public class SupplierTriggerRenewal
    {
        public List<SupplierTriggerRenewal> SupplierTrigger { get; set; }
        public string subject { get; set; }
        public string Trigger_before { get; set; }
        public string[] DetailsTriggermsg { get; set; }
        public string Message { get; set; }
        public string SendAlert { get; set; }
        public string InsertDate { get; set; }
        public string Insertby { get; set; }
        public string Slno { get; set; }
        public string status { get; set; }

    }

    public class SupplierActivation
    {
        public List<SupplierActivation> AttchList { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }

        public string ActivityCount { get; set; }
        public string ActiheaderGid { get; set; }

        public SelectList SupplierStatus { get; set; }
        public int SupplierStatusID { get; set; }
        public string SupplierStatusName { get; set; }

        public SelectList SupplierClassification { get; set; }
        public int SupplierClassificationID { get; set; }
        public string SupplierClassificationName { get; set; }

        public SelectList RequestType { get; set; }
        public int RequestTypeID { get; set; }
        public string RequestTypeName { get; set; }

        public SelectList RequestStatus { get; set; }
        public int RequestStatusID { get; set; }
        public string RequestStatusName { get; set; }

        public List<SupplierActivation> SupplierActiveList { get; set; }

        public DateTime Fromdate { get; set; }
        public string Fdate { get; set; }
        public string Todate { get; set; }
        public string AttachFileName { get; set; }
        public string AttachFileType { get; set; }
        public string Attachlength { get; set; }
        public int Attachid { get; set; }
        public string Attachdate { get; set; }
        public string Insertby { get; set; }

      
        public string ActiveReason { get; set; }
        
        public string comments { get; set; }
        public string Approver { get; set; }

        public SelectList ApproverList { get; set; }
        public Int32 Approverid { get; set; }
        public string ApproverName { get; set; }

        public string Today { get; set; }

        public string FilesToBeUploaded { get; set; }
        public string Supplierheadergid { get; set; }

        public string[] arrAttachfile { get; set; }
        //public string SupplierClassification { get; set; }
        //public string RequestType { get; set; }
        //public string RequestStatus { get; set; }

        public string Approval { get; set; }
        public string Reject { get; set; }
        public Int16 skipFlag { get; set; }//Ramya
        public string Remarks { get; set; }

        public int CurrenStrage { get; set; }

        public string queueto { get; set; }

        public string cblname { get; set; }
        public string oldname { get; set; }
        public string newname { get; set; }
        public List<SupplierActivation> SupplierHeaderList { get; set; }
        public string r_status { get; set; }
        public string reg_date { get; set; }
        public string employee { get; set; }

        public string QueueStatus { get; set; }
        public HttpPostedFileBase AMyFile { get; set; }
        public string Alogstatus { get; set; }

        public string _IsExistsFlagApprover { get; set; }
        public string _IsExistsApproverName { get; set; }
    }

    public class SupplierDeActvation
    {
      public   HttpPostedFileBase MyFile { get; set; }
        public string DSupplierCode { get; set; }
        public string DSupplierName { get; set; }
        public string DSupplierStatusName { get; set; }

        public int DCurrenStrage { get; set; }
        
        public string Dqueueto { get; set; }

        public SelectList DSupplierClassification { get; set; }
        public int DSupplierClassificationID { get; set; }
        public string DSupplierClassificationName { get; set; }

        public string DRequestTypeName { get; set; }

        public string DRequestStatusName { get; set; }
        public List<SupplierDeActvation> SupplierDeActiveList { get; set; }

        public string DSupplierheadergid { get; set; }

        public string DActivityCount { get; set; }

        public Int16 DskipFlag { get; set; }//Ramya

        public string FilesToBeUploaded { get; set; }

        public string DeActiveReason { get; set; }
        public string Decomments { get; set; }

        public string[] arrDeAttachfile { get; set; }
        public string DTodate { get; set; }
        public string DAttachFileName { get; set; }
        public string DAttachFileType { get; set; }
        public string DAttachlength { get; set; }
        public string DAttachdate { get; set; }
        public int DAttachid { get; set; }
        public string DInsertby { get; set; }
        public List<SupplierDeActvation> DSupplierActiveList { get; set; }
        public List<SupplierDeActvation> DSupplierHeaderList { get; set; }
        public SelectList DApproverList { get; set; }
        public string DApprovar { get; set; }
        public int DApproverid { get; set; }
        public string DApproverName { get; set; }
        public string DFdate { get; set; }
        public List<SupplierDeActvation> DAttchList { get; set; }
        public string DApproval { get; set; }
        public string DReject { get; set; }
        public string DRemarks { get; set; }
        public string Dr_status { get; set; }
        public string Dreg_date { get; set; }
        public string Demployee { get; set; }
        public string DQueueStatus { get; set; }
        public string logstatus { get; set; }

        public string _IsExistsFlagApprover { get; set; }
        public string _IsExistsApproverName { get; set; }
    }

    public class SupplierR_Modification
    {
        public string RSupplierCode { get; set; }
        public string RSupplierName { get; set; }
        public string RSupplierStatusName { get; set; }


        public SelectList RSupplierClassification { get; set; }
        public int RSupplierClassificationID { get; set; }
        public string RSupplierClassificationName { get; set; }

        public string RRequestTypeName { get; set; }

        public string RRequestStatusName { get; set; }

        public List<SupplierR_Modification> SupplierRActiveList { get; set; }

        public string RSupplierheadergid { get; set; }

        public string RActivityCount { get; set; }

        //public string FilesToBeUploaded { get; set; }

        //public string DeActiveReason { get; set; }
        //public string Decomments { get; set; }

        public string[] arrRAttachfile { get; set; }
    }

    public class SupervisoryApproval
    {
        public List<SupervisoryApproval> SuppervisoryApproval { get; set; }
        public List<SupervisoryApproval> VMUChecker { get; set; }
        public List<SupervisoryApproval> FunctionalHeadApproval { get; set; }
        public List<SupervisoryApproval> VMUApprover { get; set; }

        public bool Approved { get; set; }
        public bool Reject { get; set; }
        public string Remarks { get; set; }
        
        public string Sup_RequestType { get; set; }
        public string Sup_Status { get; set; }
        public string Sup_ApprovalType { get; set; }
        public string Sup_ApproverID { get; set; }
        public string Sup_ApproverName { get; set; }
        public string Sup_ApproverRemarks { get; set; }

        public string VMUc_RequestType { get; set; }
        public string VMUc_Status { get; set; }
        public string VMUc_ApprovalType { get; set; }
        public string VMUc_ApproverID { get; set; }
        public string VMUc_ApproverName { get; set; }
        public string VMUc_ApproverRemarks { get; set; }


        public string FHA_RequestType { get; set; }
        public string FHA_Status { get; set; }
        public string FHA_ApprovalType { get; set; }
        public string FHA_ApproverID { get; set; }
        public string FHA_ApproverName { get; set; }
        public string FHA_ApproverRemarks { get; set; }


        public string VMUa_RequestType { get; set; }
        public string VMUa_Status { get; set; }
        public string VMUa_ApprovalType { get; set; }
        public string VMUa_ApproverID { get; set; }
        public string VMUa_ApproverName { get; set; }
        public string VMUa_ApproverRemarks { get; set; }

        public string Sup_ApprovalGroup { get; set; }
        public int Sup_Currenlevel { get; set; }
        public string QueueToType { get; set; }
        public string ApprovalMandatory { get; set; }


    }

    public class LoginPage
    {
        public string employeeId { get; set; }
        public string passward { get; set; }
     //   public string Proxytype { get; set; }
        
    }
    //public class GetProxy
    //{
    //    public string employee_code { get; set; }
    //    public string employee_name { get; set; }
    //    public int proxy_to { get; set; }
    //    public int employee_gid { get; set; }
    //}
    public class SupplierQuery
    {
        public string qSupplierheadGid { get; set; }
        public string qSupplierCode { get; set; }
        public string qSupplierName { get; set; }
        public string qSupplierStatus { get; set; }
        public string qCont_startDate { get; set; }
        // public string qCont_EndDate { get; set; }
        //  public string qCont_EndDateold { get; set; }
        public string qCont_EndDateNew { get; set; }

        public string categoryname { get; set; }
        public string organizationtypename { get; set; }
        public string servicetypename { get; set; }
        public string employeecode { get; set; }
        public string employeename { get; set; }

        public string qActionDateFrom { get; set; }
        public string qRequesttype { get; set; }
        public string qRequeststatus { get; set; }

        public SelectList qDepartment { get; set; }
        public string qDepartmentid { get; set; }
        public string  qDepartmentname { get; set; }

        public SelectList qRequestTypeList { get; set; }
        public string qRequestTypeid { get; set; }
        public string qRequestTypeName { get; set; }

        public string qActiondateTo { get; set; }

        public List<SupplierQuery> qSupplierQueryList { get; set; }

        public SelectList categorydata { get; set; }
        public SelectList organizationtypedata { get; set; }
        public SelectList servicetypedata { get; set; }
        public SelectList Stutasdata { get; set; }
    }

    public class supasmscategory
    {
        public int categoryId { get; set; }
        public string categoryName { get; set; }
    }
    public class supasmsStutas
    {
        public string StutasId { get; set; }
        public string StutasName { get; set; }
    }
    public class supasmsorganizationtype
    {
        public int organizationtypeId { get; set; }
        public string organizationtypeName { get; set; }
    }

    public class supasmsservicetype
    {
        public int servicetypeId { get; set; }
        public string servicetypeName { get; set; }
    }
}