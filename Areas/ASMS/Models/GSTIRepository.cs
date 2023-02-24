using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace IEM.Areas.ASMS.Models
{
    public interface GSTIRepository
    {
        IEnumerable<SupplierHeader> GetSupHeaderDetailssearch(string supname, string supcode, string aging);
        IEnumerable<AgeingBucket> GetAgeingBucket();
        void InsertSupplierHeader(SupplierHeader supHeader);   
        void DeleteSupplierHeader(int SupHeaderId); 
        void UpdateSupplierHeader(SupplierHeader supHeader);
        Int64 GetSupplierHeaderGid(SupplierHeader supHeader); 
        
        IEnumerable<DirectorModel> GetDirector();
        DirectorModel GetDirectorById(int DirectorId);
        void InsertDirector(DirectorModel Directors);
        void DeleteDirector(int DirectorId); 
        void UpdateDirector(DirectorModel Directors);
        string DirectorIsExists(string Director);

        int GetDirectorsCount();

      //  List<SupplierType> GetSupplierType();

        List<SupplierHeader> GetOrganizationType();

        List<SupplierHeader> GetServiceType();
        List<SupplierHeader> GetContactType();
        List<SupplierHeader> GetSupplierCategory();
        List<SearchEmployee> GetEmployeeList();   

        IEnumerable<CustomersModel> GetCustomer();
        CustomersModel GetCustomerById(int CustomerId);
        void InsertCustomer(CustomersModel Customers);
        void DeleteCustomer(int CustomerId);
        void UpdateCustomer(CustomersModel Customers);
        string CustomerIsExists(string Customer);

        IEnumerable<SupplierServiceDetails> GetSupplierServiceDetails();
        SupplierServiceDetails GetSupplierServiceDetailsById(int SupplierServiceDetailsId);
        void InsertSupplierServiceDetails(SupplierServiceDetails SupplierServiceDetail);
        void DeleteSupplierServiceDetails(int SupplierServiceDetailsId);
        void UpdateSupplierServiceDetails(SupplierServiceDetails SupplierServiceDetail);

        IEnumerable<SubContractorDetails> GetSubContractorDetails();
        SubContractorDetails GetSubContractorDetailsById(int SubContractorDetailsId);
        void InsertSubContractorDetails(SubContractorDetails SubContractorDetail);
        void DeleteSubContractorDetails(int SubContractorDetailsId);
        void UpdateSubContractorDetails(SubContractorDetails SubContractorDetail);

        IEnumerable<SupAttachment> GetSupAttachment();   
        SupAttachment GetSupAttachmentById(int SupAttachmentId);
        void InsertSupAttachment(SupAttachment SupAttachments);
        void DeleteSupAttachment(int SupAttachmentId);
        void UpdateSupAttachment(SupAttachment SupAttachments);

        int GetSupAttachmentGID();    

        List<SupAttachment> GetDocumentGroup(); 
        List<SupAttachment> GetDocumentName(int DocGroupID);

        IEnumerable<SupplierBranchDetails> GetSupplierBranchDetails(); 
        SupplierBranchDetails GetSupplierBranchDetailsById(int SupplierBranchDetailsId);
        void InsertSupplierBranchDetails(SupplierBranchDetails SupplierBranchDetail);
        void DeleteSupplierBranchDetails(int SupplierBranchDetailsId);
        void UpdateSupplierBranchDetails(SupplierBranchDetails SupplierBranchDetail);

        IEnumerable<SupAwardDetails> GetSupAwardDetails();
        SupAwardDetails GetSupAwardDetailsById(int SupAwardDetailsId);
        void InsertSupAwardDetails(SupAwardDetails SupAwardDetail);
        void DeleteSupAwardDetails(int SupAwardDetailsId); 
        void UpdateSupAwardDetails(SupAwardDetails SupAwardDetail);

        IEnumerable<ClientDetails> GetClient();
        ClientDetails GetClientById(int ClientId);   
        void InsertClient(ClientDetails Clients);
        void DeleteClient(int ClientId);
        void UpdateClient(ClientDetails Clients);
        string ClientIsExists(string Client);

        void InsertSupplierProfile(SupplierProfile supProfile); 
        void DeleteSupplierProfile(int SupProfileId);  
        void UpdateSupplierProfile(SupplierProfile supProfile); 
        Int64 GetSupplierProfileGid(SupplierProfile supProfile);

        List<SupplierContactDetails> GetCountry();
        List<SupplierContactDetails> GetAddressType();
        List<SupplierContactDetails> GetState(int CountryID); 
        List<SupplierContactDetails> GetCity(int StateID,int CountryID);
        List<SupplierContactDetails> GetDistrictpincode(string pincode);

        IEnumerable<SupplierContactDetails> GetSupContactDetails();
       // SupplierContactDetails GetSupContactDetailsById(int SupContactDetailsId);
        void InsertSupContactDetails(SupplierContactDetails SupContactDetails);
        void DeleteSupContactDetails(int SupContactDetailsId);
        void UpdateSupContactDetails(SupplierContactDetails SupContactDetails);

        IEnumerable<SupplierContactPersonDetails> GetSupContactPersonDetails();
        void InsertSupContactPersonDetails(SupplierContactPersonDetails SupContactPersonDetails);
        void DeleteSupContactPersonDetails(int SupContactPersonDetailsId);
        void UpdateSupContactPersonDetails(SupplierContactPersonDetails SupContactPersonDetails);

        List<SupplierContactPersonDetails> GetDesignation();

        IEnumerable<SupplierTaxDetails> GetSupTaxDetails();
        void InsertSupTaxDetails(SupplierTaxDetails SupTaxDetails);
        void DeleteSupTaxDetails(int SupTaxDetailsId);
        void UpdateSupTaxDetails(SupplierTaxDetails SupTaxDetails);

        List<SupplierTaxDetails> GetTaxType();
        List<SupplierTaxDetails> GetTaxDocuments();

        List<SupplierTaxDetails> GetTaxSubType(int taxName);

        List<SupplierTaxDetails> GetTdsSection(int ServiceTypeID);

        IEnumerable<SupplierTaxDetails> GetVatDetails();
        void InsertVatDetails(SupplierTaxDetails VatDetails);
        void DeleteVatDetails(int VatDetailsId);
        void UpdateVatDetails(SupplierTaxDetails VatDetails);

        IEnumerable<SupplierTaxDetails> GetTdsDetails();
        void InsertTdsDetails(SupplierTaxDetails TdsDetails);
        void DeleteTdsDetails(int TdsDetailsId);
        void UpdateTdsDetails(SupplierTaxDetails TdsDetails);

        List<PaymentDetails> GetPaymode();
        List<PaymentDetails> GetBank();
        List<PaymentDetails> GetAccountType();

        IEnumerable<PaymentDetails> GetPaymentDetails();
        void InsertPaymentDetails(PaymentDetails PaymentDetails);
        void DeletePaymentDetails(int PaymentDetailsId);
        void UpdatePaymentDetails(PaymentDetails PaymentDetails);

        List<SupActivity> GetActivityDocName();
        List<SupActivity> GetBidding();
        List<SupActivity> GetActivityName();
        List<SupActivity> GetCategory(string IsProdorService); 
        List<SupActivity> GetSubcategory(int categoryId);

        IEnumerable<SupActivity> GetActivityDetails();
        void InsertActivityDetails(SupActivity ActivityDetails);
        void DeleteActivityDetails(int ActivityDetailsId);
        void UpdateActivityDetails(SupActivity ActivityDetails);

        IEnumerable<SupOtherDetails> GetEmpRelationship();
        void InsertEmpRelationship(SupOtherDetails EmpRelationship);
        void DeleteEmpRelationship(int EmpRelationshipId);
        void UpdateEmpRelationship(SupOtherDetails EmpRelationship);

        void InsertSupOthers(SupOtherDetails SupOthers);
        void UpdateSupOthers(SupOtherDetails SupOthers);
        int GetOthersID();

        int GetEmpGid(string EmpCode);

        void UpdateSupHeaderGidDirectors();

        IEnumerable<SupplierHeader> GetSupHeaderDetails();
        IEnumerable<SupplierHeader> GetSupHeaderDetailsDashboard(string requesttype,string requeststatus);
        SupplierHeader GetSupHeaderDetailsByID();

        int GetDirectorsCountForEdit();
        void DeleteInvalidDirectors();
        int GetSupervisor();
        List<SearchEmployee> GetEmployeeListForApproval();
        IEnumerable<DashBoard> GetDashboardForMyApproval();

        IEnumerable<SupplierTaxDetails> GetTaxAttachment();
        void InsertTaxAttachment(SupplierTaxDetails TaxAttachments);
        void DeleteTaxAttachment(int TaxAttachmentId);
        void UpdateTaxAttachment(SupplierTaxDetails TaxAttachments);

        void UpdateMSMED(SupplierTaxDetails TaxDetails);

        List<ApprovalModel> GetNextApprovalStage(string reqtype = null, int curlevel = 0);
        string SubmitToNextQueue(string queueto = "", string requesttype = "", string remarks = "", int actionstatus = 1, int skipflag = 0);
        DataTable GetSupplierName(int supplierGid);
        IEnumerable<SupplierHeader> GetRequestHistory(int SupHeadGid);
        IEnumerable<SupplierHeader> GetAllRequestHistory();

        List<SupplierTaxDetails> GetVatState();
        DataTable GetCurrentApprovalStageDetails();

        IEnumerable<DashBoard> GetDashboardForSecondGrid();
        IEnumerable<SupplierHeader> GetMyRequestsGridDetails();
        IEnumerable<SupplierHeader> GetForMyApprovalGridDetails();
        IEnumerable<SupplierHeader> GetStatusGridSupplierDetails();
        List<SupplierHeader> GetSupplierNameList();
        List<SupActivity> GetContactPersonNameList();

        IEnumerable<ModificationSummary> GetModSummaySupplierHeader();
        IEnumerable<ModificationSummary> GetModSummayDirectors();
        IEnumerable<ModificationSummary> GetModSummayServicesHistory();
        IEnumerable<ModificationSummary> GetModSummayBranchDetails();
        IEnumerable<ModificationSummary> GetModSummayManpowerDetails();
        IEnumerable<ModificationSummary> GetModSummayProductServiceDetails();
        IEnumerable<ModificationSummary> GetModSummaySubContractorDetails();
        IEnumerable<ModificationSummary> GetModSummayCustomerDetails();
        IEnumerable<ModificationSummary> GetModSummayBranchCityDetails();
        IEnumerable<ModificationSummary> GetModSummayAwardDetails(); 
        IEnumerable<ModificationSummary> GetModSummayClientDetails(); 
        IEnumerable<ModificationSummary> GetModSummaySupContactDetails();
        IEnumerable<ModificationSummary> GetModSummayContactPersonDetails();

        IEnumerable<ModificationSummary> GetModSummayTaxDetails();
        IEnumerable<ModificationSummary> GetModSummayTaxSubTypeDetails();
        IEnumerable<ModificationSummary> GetModSummayPaymentDetails();
        IEnumerable<ModificationSummary> GetModSummayActivityDetails();
        IEnumerable<ModificationSummary> GetModSummaySupAttachmentsDetails();
        IEnumerable<ModificationSummary> GetModSummaySupOthersDetails();
        IEnumerable<ModificationSummary> GetModSummayEmpRelationshipDetails();

        IEnumerable<SupplierHeader> GetFinancialReviewSummary();
        List<SupplierHeader> GetSupplierPanNoList();
        string GetCityName(int Id);
        void UpdateFinReviewStatus();

        IEnumerable<SupplierTaxDetails> GetTdsAttachment();
        SupplierTaxDetails GetTdsAttachmentById(int TdsAttachmentId);
        void InsertTdsAttachment(SupplierTaxDetails TdsAttachments);
        void DeleteTdsAttachment(int TdsAttachmentId);
        void UpdateTdsAttachment(SupplierTaxDetails TdsAttachments);
        int CheckSupNameIsExists(string SupplierName,string PanNo);
        int CheckSupPanNoIsExists(string SupplierPanNo);

        List<SupplierContactDetails> GetAllState();
        List<SupplierContactDetails> GetAllCity();
        List<SupplierBranchDetails> GetAllCityBranch();
        string GetSupIdForMail(int id);
        string IsExistsFlagApprover();
        DataTable GetMailDetails();
        int ChkEmpIsFinanceReviewer();
        string GetSupplierNameForPayment();
        IEnumerable<SupplierHeader> IsSupplierLocked(string SupplierCode);
        void LockMySupplier(string SupplierCode);
        void ReleaseMySupplier(string SupplierCode);

        List<SupAttachment> GetDocumentNameAll(); 
        List<SupAttachment> GetDocumentGroupById(int DocNameID);
        string GetSupplierPanNumber();
        string DeleteSupplier(string suppliercode = null);

        DataTable getcontracttodate();

        List<EntityGstvendor> getvendor();         
        IEnumerable<EntityGstvendor> GetState();
        IEnumerable<EntityGstvendor> getmaker(); 

        EntityGstvendor GetStateById(int StateId);
        string Insertvendor(EntityGstvendor Classifications);
        EntityGstvendor getVendorid(int PinId);

        string Updatevendor(EntityGstvendor Classifications);
        string DeleteVendor(int PinId);


       // List<SupplierContactDetails> Getcitylist(int Pincodeid);
        List<SupplierContactDetails> GetAllDistrict(); //pandiaraj 03/07/2017
    }

}