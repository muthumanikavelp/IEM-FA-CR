using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IEM.Areas.ASMS.Models
{
    public class SupplierEntity
    {

    }

    public class SupplierHeader
    {
        public int _HeaderID { get; set; }
        public string _SupplierType { get; set; }
        public string _SupplierCode { get; set; }
        public string _SupplierName { get; set; }
        public string _CompanyRegNo { get; set; }
        public int _NoOfDirectors { get; set; }
        public string _website { get; set; }
        public string _IsActiveContract { get; set; }
        public string _ReasonForNoContract { get; set; }
        public string _ContractFrom { get; set; }
        public string _ContractTo { get; set; }
        public string _ApproxSpend { get; set; }
        public string _ActualSpend { get; set; }
        public string _EmailID { get; set; }
        public string _RequestType { get; set; }
        public string _Requeststatus { get; set; }
        public string _SupplierStatus { get; set; }

        public string _RenewalDate { get; set; }
        public int _OwnerId { get; set; }
        public string _OwnerCode { get; set; }
        public string _OwnerName { get; set; }
        //public List<SupplierType> lstSupplierType { get; set; }
        //public string _SupplierTypeSelected { get; set; }

        public int _ServiceTypeID { get; set; }
        public int selectedServiceID { get; set; }
        public string _ServiceTypeName { get; set; }
        public SelectList lstServiceType { get; set; }

        public int _OrganizationTypeID { get; set; }
        public int selectedOrganizationID { get; set; }
        public string _OrganizationTypeName { get; set; }
        public SelectList lstOrganizationType { get; set; }

        public int _SupplierCategoryID { get; set; }
        public int SelectedSupplierCategoryID { get; set; }
        public string _SupplierCategoryName { get; set; }
        public SelectList lstSupplierCategory { get; set; }

        public int _SupContactTypeID { get; set; }
        public int SelectedSupContactTypeID { get; set; }
        public string _SupContactTypeName { get; set; }
        public SelectList lstSupContactType { get; set; }
        public int _CurrentApprovalStage { get; set; }

        public int _NoofYearsIBusiness { get; set; }
        public int _NoofYearsOfAssociation { get; set; }

        public int _NoOfFactories { get; set; }
        public int _NoOfOffices { get; set; }

        public int _TotalEmployees { get; set; }
        public int _PermanentEmployees { get; set; }
        public int _ContractEmployees { get; set; }

        public string _IssueAppointmentLetters { get; set; }
        public string _PerformIntegrityChecks { get; set; }
        public string _IntegrityCheckDesc { get; set; }
        public string _Payminwages { get; set; }
        public string _EmployLabourbelow18Yrs { get; set; }
        public string _EmpPFRegNo { get; set; }
        public string _ShopRegNo { get; set; }
        public string _EmpStateRegNo { get; set; }

        public string _Raiser { get; set; }
        public string _Remarks { get; set; }
        public string _ApprovalDate { get; set; }
        public string _ApprovalStage { get; set; }

        public string _TaxisMSMED { get; set; }

        public string _CurrentApprovalStageName { get; set; }
        public string _IsAllowApproverToEdit { get; set; }

        public string _Activitycount { get; set; }//thiru
        public string _AgeingBucket { get; set; }//thiru
        public string _PanNo { get; set; }
        public string _SupplierTypeRemarks { get; set; }
        public string _IsExistsFlagApprover { get; set; }
        public string _IsExistsApproverName { get; set; }
        public int _SupplierInsertBy { get; set; }

        public string _LockedByCode { get; set; }
        public string _LockedByName { get; set; }
        public string _LockedDate { get; set; }
    }

    public class AgeingBucket
    {
        public SelectList AgeingBucketData { get; set; }
        public string _AgeingBucketValue { get; set; }
        public string _AgeingBucketText { get; set; }
    }

    public class DirectorModel
    {
        public int _DirectorsID { get; set; }
        public string _DirectorsName { get; set; }
    }
    public class SupplierType
    {
        public string _SupplierTypeValue { get; set; }
        public string _SupplierTypeText { get; set; }
    }
    public class SupplierProfile
    {
        public int _SupProfileID { get; set; }
        public int _NoofYearsIBusiness { get; set; }
        public int _NoofYearsOfAssociation { get; set; }

        public int _NoOfFactories { get; set; }
        public int _NoOfOffices { get; set; }

        public int _TotalEmployees { get; set; }
        public int _PermanentEmployees { get; set; }
        public int _ContractEmployees { get; set; }
    }

    public class CustomersModel
    {
        public int _CustomerID { get; set; }
        public string _CustomerName { get; set; }
        public string _CustomerServiceName { get; set; }
        public string _CustomerContactPerson { get; set; }
        public string _CustomerMobileNo { get; set; }
        public string _CustomerPhoneNo { get; set; }
        public string _CustomerAgeOfProduct { get; set; }
    }
    public class SupplierServiceDetails
    {
        public int _SupServiceDetailsID { get; set; }
        public string _SupServiceDetailsName { get; set; }

    }
    public class SupplierBranchDetails
    {
        public int _SupBranchID { get; set; }
        public string _SupBranchName { get; set; }


        public int _CityID { get; set; }
        public int selectedCityID { get; set; }
        public string _CityName { get; set; }
        public SelectList lstCity { get; set; }
    }
    public class SubContractorDetails
    {
        public int _SubContractorID { get; set; }
        public string _SubContractorName { get; set; }
        public string _SubContractorServiceName { get; set; }

    }
    public class SupAwardDetails
    {
        public int _SupAwardID { get; set; }
        public string _SupAwardName { get; set; }
        public string _SupAwardDesc { get; set; }

    }
    public class ClientDetails
    {
        public int _ClientID { get; set; }
        public string _ClientName { get; set; }
        public string _ClientAgeOfProduct { get; set; }
        public string _ClientAddress { get; set; }
        public string _ClientMobileNo { get; set; }
        public string _ClientPhoneNo { get; set; }
    }
    public class SupAttachment
    {
        public int _SupAttachmentID { get; set; }
        public int _SupDocumentGroupID { get; set; }
        public string _SupDocumentGroupName { get; set; }
        public int _SupDocumentNameID { get; set; }
        public string _SupDocumentName { get; set; }
        public string _SupAttachedFileName { get; set; }
        public string _SupAttachDescription { get; set; }
        public string _SupAttachDate { get; set; }
        public string _SupAttachedActualFileName { get; set; }

        public int _DocumentGroupID { get; set; }
        public int selectedDocumentGroupID { get; set; }
        public string _DocumentGroupName { get; set; }
        public SelectList lstDocumentGroup { get; set; }

        public int _DocumentNameID { get; set; }
        public int selectedDocumentNameID { get; set; }
        public string _DocumentName { get; set; }
        public SelectList lstDocumentName { get; set; }

        public string _tempFileName { get; set; }
        public string _AttachmentFor { get; set; }

        public string ischeckingmaker { get; set; }

    }
    public class SupplierContactDetails
    {
        public int _SupContactDetailsID { get; set; }
        public string _Address1 { get; set; }
        public string _Address2 { get; set; }
        public string _Address3 { get; set; }

        public int _AddressTypeID { get; set; }
        public int selectedAddressTypeID { get; set; }
        public string _AddressTypeName { get; set; }
        public SelectList lstAddressType { get; set; }

        public int _CountryID { get; set; }
        public int selectedCountryID { get; set; }
        public string _CountryName { get; set; }
        public SelectList lstCountry { get; set; }

        public int _StateID { get; set; }
        public int selectedStateID { get; set; }
        public string _StateName { get; set; }
        public SelectList lstState { get; set; }

        public int _CityID { get; set; }
        public int selectedCityID { get; set; }
        public string _CityName { get; set; }
        public SelectList lstCity { get; set; }

        //Pandiaraj 03/07/2017 District Add
        public int _DistrictID { get; set; }
        public int selectedDistrictID { get; set; }
        public string _DistrictName { get; set; }
        public SelectList lstDistrict { get; set; }

        public int _PinCode { get; set; }
        public string _PhoneNo { get; set; }
        public string _Fax { get; set; }
        public int Pincodeid { get; set; }
        public string Pincode { get; set; }
        public SelectList lstPincode { get; set; }
        public int selectedPincodeID { get; set; }
    }
    public class SupplierContactPersonDetails
    {
        public int _SupContactPersonID { get; set; }
        public string _SupContactPersonName { get; set; }
        public string _SupContactPersonLocation { get; set; }

        public int _DesignationID { get; set; }
        public int selectedDesignationID { get; set; }
        public string _DesignationName { get; set; }
        public SelectList lstDesignation { get; set; }

        public string _SupContactPersonMobileNo { get; set; }
        public string _SupContactPersonPhoneNo { get; set; }
        public string _SupContactPersonEmailId { get; set; }

    }
    public class SupplierTaxDetails
    {
        public int _TaxDetailsID { get; set; }
        public string _TaxRegNo { get; set; }
        public string _TaxRate { get; set; }
        public string _TaxisMSMED { get; set; }

        public int _TaxDocumentGroupID { get; set; }
        public int selectedTaxDocumentGroupID { get; set; }
        public string _TaxDocumentGroupName { get; set; }
        public SelectList lstTaxDocumentGroup { get; set; }

        public int _TaxDocumentNameID { get; set; }
        public int selectedTaxDocumentNameID { get; set; }
        public string _TaxDocumentName { get; set; }
        public SelectList lstTaxDocumentName { get; set; }

        public int _TaxTypeID { get; set; }
        public int selectedTaxTypeNameID { get; set; }
        public string _TaxTypeName { get; set; }
        public SelectList lstTaxTypeName { get; set; }
        public string _TaxTypeCode { get; set; }
        public int _TdsID { get; set; }

        public int _TaxAttachmentID { get; set; }
        public string _TaxAttachmentFileName { get; set; }
        public string _TaxAttachmentDate { get; set; }
        public string _TaxAttachmentDescription { get; set; }
        public string _TaxAttachmentActualFileName { get; set; }

        public int _TdsServiceTypeID { get; set; }
        public int selectedTdsServiceTypeID { get; set; }
        public string _TdsServiceTypeName { get; set; }
        public string _TdsServiceTypeSection { get; set; }
        public SelectList lstTdsServiceType { get; set; }

        public string _IsExemption { get; set; }
        public string _TDSRate { get; set; }

        public string _ExemptedRate { get; set; }
        public string _ExemptionPeriodFrom { get; set; }
        public string _ExemptionPeriodTo { get; set; }
        public string _ExemptionThresholdValue { get; set; }
        public string _ExemptionAttachedFileName { get; set; }
        public string _ExemptionDescription { get; set; }
        public string _ExemptionCertificateNo { get; set; }
        public string _ExemptionAttachedActualFileName { get; set; }

        public int _TdsDocumentGroupID { get; set; }
        public int selectedTdsDocumentGroupID { get; set; }
        public string _TdsDocumentGroupName { get; set; }
        public SelectList lstTdsDocumentGroup { get; set; }

        public int _TdsDocumentNameID { get; set; }
        public int selectedTdsDocumentNameID { get; set; }
        public string _TdsDocumentName { get; set; }
        public SelectList lstTdsDocumentName { get; set; }

        public int _TdsAttachmentID { get; set; }
        public string _TdsAttachmentFileName { get; set; }
        public string _TdsAttachmentActualFileName { get; set; }
        public string _TdsAttachmentDate { get; set; }
        public string _TdsAttachmentDescription { get; set; }

        public int _VatID { get; set; }
        public string _VatCity { get; set; }
        public string _VatRate { get; set; }

        public int _VatStateID { get; set; }
        public int selectedVatStateID { get; set; }
        public string _VatStateName { get; set; }
        public SelectList lstVatState { get; set; }

        public int _IsAllowAdd { get; set; }
        public string _TaxReceivableFlag { get; set; }

    }
    public class SearchEmployee
    {
        public int _EmployeeGid { get; set; }
        public string _EmployeeCode { get; set; }
        public string _EmployeeName { get; set; }
        public string _EmployeeFor { get; set; }
    }
    public class PaymentDetails
    {
        public int _PaymentID { get; set; }

        public int _PaymodeID { get; set; }
        public int selectedPaymodeID { get; set; }
        public string _PaymodeName { get; set; }
        public SelectList lstPaymode { get; set; }

        public int _AccountTypeID { get; set; }
        public int selectedAccountTypeID { get; set; }
        public string _AccountTypeName { get; set; }
        public SelectList lstAccountType { get; set; }

        public int _BankID { get; set; }
        public int selectedBankID { get; set; }
        public string _BankName { get; set; }
        public SelectList lstBank { get; set; }

        public string _BankBranch { get; set; }
        public string _IfscCode { get; set; }
        public string _AccountNo { get; set; }
        public string _BeneficiaryName { get; set; }
    }
    public class SupActivity
    {
        public int _ActivityID { get; set; }
        public string _Activitytype { get; set; }
        public string _DescOfActivityType { get; set; }

        public int _CategoryID { get; set; }
        public int selectedcategoryID { get; set; }
        public string _CategoryName { get; set; }
        public SelectList lstCategory { get; set; }

        public int _SubCategoryID { get; set; }
        public int selectedSubcategoryID { get; set; }
        public string _SubCategoryName { get; set; }
        public SelectList lstSubCategory { get; set; }

        public string _FidelityInsuranceReqd { get; set; }

        public int _BiddingID { get; set; }
        public int selectedBiddingID { get; set; }
        public string _BiddingName { get; set; }
        public SelectList lstBidding { get; set; }

        public int _OIC { get; set; }
        public string _OICName { get; set; }

        public int _ActivityNameID { get; set; }
        public int selectedActivityNameID { get; set; }
        public string _ActivityName { get; set; }
        public SelectList lstActivity { get; set; }

        public string _ProjectContractSpend { get; set; }
        public string _Saves { get; set; }
        public string _ActivityStartDate { get; set; }
        public string _ActivityEndDate { get; set; }
        public int _ContactPerson { get; set; }
        public string _ContactPersonName { get; set; }


        public string _ActivityAttachmentsID { get; set; }
        public string _ActivityAttachmentsFileName { get; set; }
        public string _AttachmentDescription { get; set; }
        public string _AttachmentDate { get; set; }

        public int _DocNameID { get; set; }
        public int selectedDocNameID { get; set; }
        public string _DocName { get; set; }
        public SelectList lstDocName { get; set; }

        public string _Reason { get; set; }
    }

    public class SupOtherDetails
    {
        public int _OthersID { get; set; }
        public string _IssueAppointmentLetters { get; set; }
        public string _PerformIntegrityChecks { get; set; }
        public string _IntegrityCheckDesc { get; set; }
        public string _Payminwages { get; set; }
        public string _EmployLabourbelow18Yrs { get; set; }
        public string _EmpPFRegNo { get; set; }
        public string _ShopRegNo { get; set; }
        public string _EmpStateRegNo { get; set; }

        public int _RelationshipID { get; set; }
        public string _RelationName { get; set; }
        public string _RelationshipName { get; set; }
    }
    public class ApprovalModel
    {
        public int _QueueID { get; set; }
        public string _SubmitDate { get; set; }
        public int _QueueFrom { get; set; }
        public string _QueueTo { get; set; }
        public string _QueueToType { get; set; }
        public string _ActionRemarks { get; set; }
        public int _QueueCurrentLevel { get; set; }
        public string _ActionFor { get; set; }
        public string _ActionDate { get; set; }
        public int _ActionBy { get; set; }
        public string _ActionStatus { get; set; }
        public string _SkipFlag { get; set; }

        public string _RequestType { get; set; }
        public string _NextApprovalGroup { get; set; }
        public string _NextpproverID { get; set; }
        public string _NextApprovalIsMandatory { get; set; }

    }
    public class DashBoard
    {
        public string _DashBoardRequestType { get; set; }
        public string _DashBoardRequestStatus { get; set; }
        public int _DraftCount { get; set; }
        public int _InprocessCount { get; set; }
        public int _ApprovedCount { get; set; }
        public int _RejectedCount { get; set; }
        public int _WaitingForApprovalCount { get; set; }

        public string _StatusName { get; set; }
        public int _StatusCount { get; set; }
    }
    public class ModificationSummary
    {
        public string _ColumnName { get; set; }
        public string _ModifiedValue { get; set; }
        public string _OldValue { get; set; }
    }

    public class EntityGstvendor
    {
        //  ,,,,,suppliergst_isremoved,suppliergst_insertby
        public Int32 suppliergst_gid { get; set; }
        public string suppliergst_app { get; set; }
        public string suppliergst_vertical { get; set; }
        public Int32 suppliergst_stateid { get; set; }
        public string suppliergst_state { get; set; }
        public string suppliergst_threshold { get; set; }
        public string suppliergst_tin { get; set; }
        public string suppliergst_status { get; set; }



        public SelectList GetState { get; set; }
        public SelectList GetStateById { get; set; }
        public int selectedstate_gid { get; set; }
        public string IsChecker { get; set; }

    }

}