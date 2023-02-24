using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;

namespace IEM.Areas.MASTERS.Models
{
    public interface Iiem_mst_bank
    {
        IEnumerable<iem_mst_bank> getbank();
        IEnumerable<iem_mst_bank> getbank(string filter_code, string filter_name);
        iem_mst_bank GetBankById(int ClassificationId);
        string InsertBank(iem_mst_bank Classifications);
        string DeleteBank(int ClassificationId);
        string UpdateBank(iem_mst_bank Classifications);
    }

    public interface Iiem_mst_courier
    {
        IEnumerable<iem_mst_courier> getcourier();
        IEnumerable<iem_mst_courier> getcourier(string filter_name);
        iem_mst_courier GetCourierById(int ClassificationId);
        string InsertCourier(iem_mst_courier Classifications);
        string DeleteCourier(int ClassificationId);
        string UpdateCourier(iem_mst_courier Classifications);
    }
    public interface Iiem_mst_currency
    {
        IEnumerable<iem_mst_currency> getcurrency();
        IEnumerable<iem_mst_currency> getcurrency(string filter_code, string filter_name);
        iem_mst_currency GetCurrencyById(int currencyId);
        string InsertCurrency(iem_mst_currency Currency);
        string DeleteCurrency(int currencyId);
        string UpdateCurrency(iem_mst_currency Currency);
    }
    public interface Iiem_mst_currencyrate
    {
        IEnumerable<iem_mst_currencyrate> getcurrencyRate();
        IEnumerable<iem_mst_currencyrate> getcurrencyRate(string currencyname, string periodfrom, string periodto);
        iem_mst_currencyrate GetCurrencyRateById(int currencyId);
        IEnumerable<Getcurrency> Getcurrency();
        string InsertCurrencyRate(iem_mst_currencyrate Currency);
        string DeleteCurrencyRate(int currencyId);
        string UpdateCurrencyRate(iem_mst_currencyrate Currency);
    }
    public interface Iiem_mst_region
    {
        IEnumerable<iem_mst_region> getregion();
        IEnumerable<iem_mst_region> getregion(string filter);
        iem_mst_region GetRegionById(int RegionId);
        string InsertRegion(iem_mst_region Region);
        string DeleteRegion(int RegionId);
        string UpdateRegion(iem_mst_region Region);
    }
    public interface Iiem_mst_uom
    {
        IEnumerable<iem_mst_uom> getuom();
        IEnumerable<iem_mst_uom> getuom(string filter_code, string filter_name);
        iem_mst_uom GetUomById(int UomId);
        string InsertUom(iem_mst_uom Uom);
        string DeleteUom(int UomId);
        string UpdateUom(iem_mst_uom Uom);
    }

    public interface Iiem_mst_bounce
    {
        IEnumerable<iem_mst_bouncereason> getbounce();
        IEnumerable<iem_mst_bouncereason> getbounce(string filter_code, string filter_name);
        iem_mst_bouncereason GetBounceById(int BounceId);
        string InsertBounce(iem_mst_bouncereason Bounce);
        void DeleteBounce(int BounceId, int updateperson);
        string UpdateBounce(iem_mst_bouncereason Bounce);
    }
    public interface Iiem_mst_Tax_rate
    {
        IEnumerable<Gettaxratetaxsubname> Gettaxsubname_id(int id);
        IEnumerable<iem_mst_tax_rate> gettaxrate();
        IEnumerable<Gettaxratetaxname> Gettaxname();
        IEnumerable<Gettaxratetaxsubname> Gettaxsubname();
        string Inserttaxrate(iem_mst_tax_rate taxcat);
        void Deletetaxrate(int taxId);
        iem_mst_tax_rate gettaxratebyid(int taxId);
        string Updattaxrate(iem_mst_tax_rate taxId);
        IEnumerable<iem_mst_tax_rate> gettaxrateid(string filter_name, string filter_name1);
    }
    public interface Iiem_mst_Tax_subtype
    {
        IEnumerable<iem_mst_tax_subtype> SubCategorydata(int id);
        IEnumerable<iem_mst_tax_subtype> ExpenseCategorydata(int id);
        IEnumerable<iem_mst_tax_subtype> NatureofExpensesdataother();
        IEnumerable<iem_mst_tax_subtype> gettaxsubtype();
        IEnumerable<iem_mst_tax_subtype> gettaxsubtypeid(string filter_name, string filter_name1);
        IEnumerable<Gettaxsub> Gettaxsubvl();
        IEnumerable<iem_mst_tax_subtype> GetGlNumber(string glnumber);
        string Inserttaxsub(iem_mst_tax_subtype taxcat);
        string Deletetaxsub(int taxId);
        string Updattaxsub(iem_mst_tax_subtype taxId);
        iem_mst_tax_subtype GettaxsubById(int taxId);
    }
    public interface Iiem_mst_Tax
    {
        IEnumerable<iem_mst_tax> gettax();
        IEnumerable<Gettax> Gettaxvl();
        IEnumerable<Gettax> Gettaxvl_id(int taxId);
        iem_mst_tax GettaxById(int taxId);
        IEnumerable<iem_mst_tax> gettaxid(string filter_name, string filter_name1);
        string Inserttax(iem_mst_tax taxcat);
        string Deletetax(int taxId);
        string Updattax(iem_mst_tax taxCat);
    }
    public interface Iiem_mst_expnature
    {
        IEnumerable<iem_mst_expnature> getexpnature();
        IEnumerable<iem_mst_expnature> getexpnature(string filter_name);
        iem_mst_expnature GetexpnatureById(int expnatureId);
        string Insertexpnature(iem_mst_expnature expnature);
        string Deleteexpnature(int expnatureId);
        string Updateexpnature(iem_mst_expnature expnature);
    }

    public interface Iiem_mst_cityclass
    {
        IEnumerable<iem_mst_cityclass> getcity();
        IEnumerable<iem_mst_cityclass> getcity(string filter_code, string filter_name);
        iem_mst_cityclass GetCityById(int CityId);
        string InsertCity(iem_mst_cityclass Classifications);
        string DeleteCity(int CityId, int updateperson);
        string UpdateCity(iem_mst_cityclass Classifications);
    }
    public interface Iiem_mst_tier
    {
        IEnumerable<iem_mst_tier> gettier();
        IEnumerable<iem_mst_tier> gettier(string filter_code, string filter_name);
        iem_mst_tier GetTierById(int TierId);
        string InsertTier(iem_mst_tier Tier);
        string DeleteTier(int TierId);
        string UpdateTier(iem_mst_tier Tier);
    }


    public interface Iiem_mst_country
    {
        IEnumerable<iem_mst_country> getcountry();
        IEnumerable<iem_mst_country> getcountry1(string country_code, string country_name, string currency_name);
        iem_mst_country GetCountryId(int CityId);
        string Insertcountry(iem_mst_country Classifications);
        string Deletecountry(int CityId);
        string UpdateCountry(iem_mst_country Classifications);

        IEnumerable<Getcurrency> GetCurrency();
        GetcurrencyById GetCurrencyById(int Curency);

    }
    public interface Iiem_mst_city
    {
        IEnumerable<iem_mst_city> getcity();
        IEnumerable<iem_mst_city> getcity_id(string city_code, string city_name, string city_pincode);
        iem_mst_city GetcityId(int CityId);
        string InsertCity(iem_mst_city City);
        void DeleteCity(int CityId);
        string UpdateCity(iem_mst_city City);

        IEnumerable<Getstate> Getstate();
        GetstateById GetstateById(int State);

        IEnumerable<Getregion> Getregion();
        GetregionById GetregionById(int Region);

        IEnumerable<Getcountry> Getcountry();
        GetcountryById GetcountryById(int Country);
        IEnumerable<GetregionById> GetregionBy_Id(int Region);
        IEnumerable<Getcityclass> Getcityclass();
        GetcityclassById GetcityclassById(int Cityclass);
        IEnumerable<GetregionById> GetcountryBy_Id(int Region);

        IEnumerable<Gettier> Gettier();
        GettierById GettierById(int tier);

    }
    public interface Iiem_mst_state
    {
        IEnumerable<iem_mst_state> getstate();
        IEnumerable<iem_mst_state> getstateid(string state_code, string state_name);
        iem_mst_state GetStateId(int StateId);
        string InsertState(iem_mst_state State);
        string DeleteState(int StateId);
        string UpdateState(iem_mst_state State);

        IEnumerable<Getregion> Getregion();
        GetregionById GetregionById(int Region);

        IEnumerable<Getcountry> Getcountry();
        GetcountryById GetcountryById(int Country);
    }
    public interface Iiem_mst_expensecategory
    {
        IEnumerable<iem_mst_expensecategory> getexpcategory();
        IEnumerable<iem_mst_expensecategory> getexpcategory(string expcat_code, string expcat_name, string getexpcategory);
        iem_mst_expensecategory GetexpcategoryById(int ExpId);
        string Insertexpcategory(iem_mst_expensecategory Expcate);
        string Deletexpcategory(int ExpCatId);
        string Updatexpcategory(iem_mst_expensecategory ExpCat);
        IEnumerable<iem_mst_expensecategory> SelectGLCode();
        IEnumerable<iem_mst_expensecategory> SelectGLSearch(string EmployeeName, string EmployeeCode);
        IEnumerable<GetGL> GetGL();
        GetGLById GetGLById(int expcatId);

        IEnumerable<Getexpnature> Getexpnature();
        GetexpnatureById GetexpnatureById(int Expcat);
    }
    public interface Iiem_mst_expensesubcategory
    {
        IEnumerable<iem_mst_expensesubcategory> getexpsubcategory();
        IEnumerable<iem_mst_expensesubcategory> getexpsubcategory1(string emp_code, string emp_name, string naturegid, string expcatgid);
        iem_mst_expensesubcategory GetexpsubcategoryById(int ExpsubId);
        string Insertexpsubcategory(iem_mst_expensesubcategory Expsubcate, string hsn);
        string Deletexpsubcategory(int ExpCatId);
        string Updatexpsubcategory(iem_mst_expensesubcategory ExpsubCat,string hsn);
        IEnumerable<Getexpcat> Getexpcat();
        GetexpcatById GetexpcatById(int ExpCatId);
        IEnumerable<GetexpcatById> GetexpcatBy_Id(int ExpCatId);
        IEnumerable<Getexpnature> Getexpnature();
        GetexpnatureById GetexpnatureById(int ExpcatId);
        IEnumerable<GetHsncode> GetHsncode();
        string Gethsndesc(string Exphsn);
        List<SelectListItem> GetAllHsn();
        List<SelectListItem> GetHsnforexpense(string id);
        string GethsndescEXPENSE(string id);
    }


    public interface Iiem_mst_delmat
    {
        IEnumerable<iem_mst_delmat> getdelmat();
        string checkFlowedit(iem_mst_delmat Delmatflow);
        void updateDelmatsetflow(iem_mst_delmat Delmatsetflow);
        iem_mst_delmat GetdelmatExceptionById(int DematId);
        string DeleteDelmatInformation(iem_mst_delmat Delmatinformation);
        string InsertDelmat(iem_mst_delmat Delmat, string[] Selecteddepartment);
        void DeleteDelmat(int delmatmatrixgid, int delmatsetflowgid);
        string UpdateDelmat(iem_mst_delmat Delmat, string[] Selecteddepartment);
        void InsertDelmatsetflow(iem_mst_delmat Delmatsetflow);
        string InsertDelmatMatrix(iem_mst_delmat Delmatmatrix);
        void UpdateDelmatMatrix(iem_mst_delmat Delmatmatrix);
        string InsertDelmatException(iem_mst_delmat Delmatexception);
        void UpdateDelmatException(iem_mst_delmat Updatedelmatexception);
        void DeleteDelmatException(int EceptionID);
        string checkduplicateFlow(iem_mst_delmat Delmatflow);
        string AddDelmatdepartment(iem_mst_delmat Delmatdetails, string[] Selecteddepartment);
        DataTable GetflowgidCountdel(int delmatgid);
        void updateDelmatsetflowedit(iem_mst_delmat Delmatsetflow);
        string delcount(int InDelamtGid);
        IEnumerable<GetDelmat> GetDelmat();
        GetDelmatById GetDelmatTypeById(int DelmatId);

        IEnumerable<GetDepartment> GetDepartment();
        GetDepartmentById GetDepartmentById(int DepatId);

        IEnumerable<GetSlab> GetSlab();
        string checkFlow(iem_mst_delmat Delmatflow);
        DelmatVisisbleById GetdelmatvisibleById(int DelmaytypeId);
        DelmatVisisbleById GetdelmaBranchActivityById(int DelmatId);
        DataTable GetDepartmentTable();

        IEnumerable<GetTitle> GetTitle();
        DataTable GetTitleById(int TitleId);
        DataTable GettileGidByname(string Titlename);
        DataTable GetDelmatGid(string Name);

        DataSet AutocompleteEmployee(string EmployeeName, string EmployeeCode);
        IEnumerable<GetEmployee> GetEmployee();
        IEnumerable<GetGrade> GetGrade();
        IEnumerable<GetDesignation> GetDesignation();
        IEnumerable<GetRole> GetRole();


        DataTable Getrows(string slabid);
        DataTable GetrowsOnlyByID(int slabranheId);
        DataTable InsertData(int TitleId, int ValueId);


        DataTable GetValueDetailsById(int EmpGid, int flag);
        DataTable GettitlevalueByName(string Titlevaluename, int flag);
        DataTable GettitleGidByName(string Titlevaluename, int flag);
        DataTable GetslabrangeGid(int SlabGid);
        DataTable GetSetFlowId();
        DataTable GetslabRangeByID(int slabrangegid);
        DataTable GetSlabById(int slabid);
        DataTable GetDelmatnameById(int delmatId);
        DataTable Getdelmattypename(int id);
        DataTable Getdelmattyprgid(int id);
        DataTable GetSlabGid(int slaid);
        DataTable Getslabname(int slabid);
        DataTable Getdelmatdeptgid(int delmat_gid);
        DataTable GetdelmatexceptionById(int exceptionid);


        DataTable GetflowgidCount(int delmatgid);
        DataTable SelectDelmatGidCount(int delmatgid);
        DataTable SelectAddApproval(int InDelamtGid, int flowgid);
        DataTable GetSlabRangeNameBySlabrangeId(int SlabrangeId, int delmatgid);
        DataTable getdelmatException(string delmatgid);



    }
    public interface Iiem_mst_role
    {
        IEnumerable<iem_mst_role> getrole();
        //IEnumerable<iem_mst_role> Getrolegroup();
        IEnumerable<Getrolegroup> Getrolegroupvl();
        //iem_mst_country Getrole(int rolegroupid);
        iem_mst_role Iem_Mst_Role(string id);
        // IEnumerable<iem_mst_role> getroledelete(int id);
        string getroledelete(int id);
        IEnumerable<iem_mst_role> searchrole(string rolecode, string rolename, string rolegroupname);
        iem_mst_role menu(int id = 0);
        string AddHolidayState(iem_mst_role sub, string[] SelectedState, string[] unselectedrole);
        List<menu> menuView(int id);
        string update(iem_mst_role obj, string[] SelectedState, string[] unselectedrole);
    }
    public interface Iiem_mst_advancetype
    {
        IEnumerable<iem_mst_advancetype> getadvancetypeGrid();
        IEnumerable<iem_mst_advancetype> getadvancetype(string advanceypename, string advancetyp_gl_no);
        iem_mst_advancetype GetadvancetypeById(int advancetypegid);
        string InsertAdvanceType(iem_mst_advancetype Classifications);
        string DeleteAdvanceType(int ClassificationId);
        string UpdateAdvanceType(iem_mst_advancetype Classifications);
        IEnumerable<GetGL> GetGL();
        DataTable getglno(int glgid);
        DataTable getglgid(int glno);
        DataTable getglnoByadvanctyid(int atypegid);
    }
    public interface Iiem_mst_branchtype
    {
        IEnumerable<iem_mst_branchtype> getbranchtype();
        IEnumerable<iem_mst_branchtype> getbranchtype(string filter_code);
        iem_mst_branchtype GetbranchtypeById(int ClassificationId);
        string Insertbranchtype(iem_mst_branchtype Classifications);
        string Deletebranchtype(int ClassificationId);
        string Updatebranchtype(iem_mst_branchtype Classifications);
    }
    public interface Iiem_mst_branchtier
    {
        IEnumerable<iem_mst_branchtier> getbranchtier();
        IEnumerable<iem_mst_branchtier> getbranchtier(string filter_name);
        iem_mst_branchtier GetbranchtierById(int ClassificationId);
        string Insertbranchtier(iem_mst_branchtier Classifications);
        string Deletebranchtier(int ClassificationId);
        string Updatebranchtier(iem_mst_branchtier Classifications);
    }
    public interface Iiem_mst_location
    {
        IEnumerable<iem_mst_location> getlocation();
        IEnumerable<iem_mst_location> getlocation(string locationcode, string locationname, string ciyname, string tiername);
        iem_mst_location GetlocationrById(int ClassificationId);
        string Insertlocation(iem_mst_location Classifications);
        string Deletelocation(int ClassificationId);
        string Updatelocation(iem_mst_location Classifications);
        IEnumerable<GetCity> GetCity();
        //GetstateById GetstateById(int State);
        IEnumerable<Gettier> Gettier();
        //Gettier GettierById()
    }
    public interface Iiem_mst_hotel
    {
        IEnumerable<iem_mst_hotel> GetHotel();
        IEnumerable<iem_mst_hotel> GetHotel(string HotelName);
        iem_mst_hotel GetHotelById(int HotelGid);
        string InsertHotel(iem_mst_hotel HotelModel);
        string DeleteHotel(int HotelGid);
        string UpdateHotel(iem_mst_hotel Updatehotelmodel);
    }
    public interface Iiem_mst_delegation
    {
        IEnumerable<iem_mst_delegation> GetDelegation();
        IEnumerable<iem_mst_delegation> GetDelegation(string delegateby, string delegateto, string periodfrom, string periodto, string delmatype);
        iem_mst_delegation GetDelegationById(int DelegationGid);
        string InsertDelegation(iem_mst_delegation DelegationModel);
        string DeleteDelegation(int DelegationGid);
        string UpdateDelegation(iem_mst_delegation UpdateDelegationmodel);
        IEnumerable<GetDelmattype> GetDelmattype();
        IEnumerable<iem_mst_delegation> EmployeeSearch();
        IEnumerable<iem_mst_delegation> EmployeeSearch(string EmpName, string Empcode);
    }
    public interface Iiem_mst_auditgroup
    {
        IEnumerable<iem_mst_auditgroup> GetAuditGroup();
        IEnumerable<iem_mst_auditgroup> GetAuditGroup(string Auditgrouname, string doctype, string docsubtype, string doccat);
        iem_mst_auditgroup GetAuditgroupById(int AuditgroupGid);
        string InsertAuditGroup(iem_mst_auditgroup Auditgroup);
        string DeleteAuditGroup(int AuditGroupGid);
        string UpdateAuditGroup(iem_mst_auditgroup UpdateAudditgroupmodel);
        IEnumerable<doctype> doctype();
        IEnumerable<docsubtype> docsubtype();
        IEnumerable<doccat> doccat();
    }
    public interface Iiem_mst_auditlist
    {
        IEnumerable<iem_mst_auditlist> GetAuditList();
        IEnumerable<iem_mst_auditlist> GetAuditList(string AuditListame, string auditgroupname, string auditsubname);
        iem_mst_auditlist GetAuditListById(int AuditListGid);
        string InsertAuditList(iem_mst_auditlist AuditList);
        string DeleteAuditList(int AuditGroupGid);
        string UpdateAuditGroup(iem_mst_auditlist UpdateAudditListmodel);
        IEnumerable<selctgroupname> selctgroupname();
        IEnumerable<selectsubgroupname> selectsubgroupname();
        DataTable getauditlistorder();
    }
    public interface Iiem_mst_finyear
    {
        IEnumerable<iem_mst_finayear> GetFinyear();
        IEnumerable<iem_mst_finayear> GetFinyear(string finyearcode, string periodfrom, string periodto);
        iem_mst_finayear GetFinyearById(int FinId);
        string InsertFinyear(iem_mst_finayear Finyear);
        string DeleteFinyear(int FinyearId);
        string UpdateFinyear(iem_mst_finayear UpdateFinyearmodel);
    }
    public interface Iiem_mast_auditsubgroup
    {
        IEnumerable<iem_mst_auditsubgroup> GetAuditsubgroup();
        IEnumerable<iem_mst_auditsubgroup> GetAuditsubgroup(string Auditsubgroupname, string auditgroupname);
        iem_mst_auditsubgroup GetAuditsubgroupById(int AuditsubgroupId);
        string InsertAuditsubgroup(iem_mst_auditsubgroup Auditsubgroup);
        string DeleteAuditsubgroup(int FinyearId);
        string UpdateAuditsubgroup(iem_mst_auditsubgroup UpdateAuditsubgroupmodel);
        IEnumerable<selctgroupname> selctgroupname();
        string getauditgroupname(int groupgid);
        string gotcount();
    }
    public interface Iiem_mst_accomodationtype
    {
        IEnumerable<iem_mst_accomodationtype> GetAccomodationtype();
        IEnumerable<iem_mst_accomodationtype> GetAccomodationtype(string Accomodationtype);
        iem_mst_accomodationtype GetAccomodationtypeById(int AccomodationtypeId);
        string Insertaccomodation(iem_mst_accomodationtype Accomodationtype);
        string DeleteAuditsubgroup(int AccomodationtypeId);
        string UpdateAccomodationtype(iem_mst_accomodationtype UpdateAccomodationtype);
    }
    public interface iem_mst_tgsttax
    {
        IEnumerable<Entity_taxgst> getgsttax(string action);
        IEnumerable<Entity_taxgst> getgsttaxbyHSN(string action,string HSNCode);
        IEnumerable<GetTaxsubtype> Gettaxsubtype();
        IEnumerable<GetHsncod> GetHsncode();
        IEnumerable<Getstatelist> GetStatecode();
        IEnumerable<Getgllist> GetGLdtl();
        IEnumerable<GetglCr> GetCLtl();
        IEnumerable<GetglRe> GetRLdtl();
        string Gethsndesc(Entity_taxgst Exphsn);
        string savegst(Entity_taxgst savegstdtl);
        Entity_taxgst Geteditdel(int taxgstid);
        string Updategsttax(Entity_taxgst updategst);
        string Deletegsttax(Entity_taxgst deltgst);
        string GetRolegroup();

        //pandiaraj
        IEnumerable<Entity_taxgst> uploadlist();
        List<Entity_taxgst> historylist();
        DataTable uploadGSTTAX(string ss, string filename);
        DataSet forexcel(int uploadgid, string type);
        string Updateuploadgsttax(Entity_taxgst updategst);
        IEnumerable<Entity_taxgst> GetAuditTrialList(int taxsubtypeid,int hsnid,int stateid);
        //pandiaraj
    }

    public interface Iiem_mst_CustomerDetail
    {
        IEnumerable<CustomerdetailModel> GetCustomerDetail(string filter);
        List<CustomerdetailModel> GetCustomerDetailById(string customer_code, string customer_name, string panno);
        CustomerdetailModel GetCustomerDetailId(int customerID);
        string InsertCustomerDetail(CustomerdetailModel CustomerdetailModel);
        string EditCustomerDetail(CustomerdetailModel CustomerdetailModel);
        void DeleteCustomerDetail(int customerID);
        IEnumerable<CustomerdetailModel> GetScoreByGrp(int id);
        string GetSPincodeID(int id);
        List<CustomerdetailModel> Getcitylist(int Pincodeid);
        //List<CustomerdetailModel> GetPincode();
        //List<CustomerdetailModel> GetAllState();
        //List<CustomerdetailModel> GetAllDistrict();
        DataTable GetCustomerDetailexcel();

        int CheckCustNameIsExists(string CustomerName, string PanNo);
        int CheckCustPanNoIsExists(string CustomerPanNo);
        void InsertCustomerHeader(CustomerDetailHeader supHeader);
        void InsertCustomerHeaderDetails(CustomerDetailHeader supHeader);
        string UpdateCustomerHeader(CustomerDetailHeader supHeader);
        Int64 GetCustomerHeaderGid(CustomerDetailHeader supHeader);
        CustomerDetailHeader GetcustomerdetailHeader(string id);
        void InsertCustContactDetails(CustomerContactDetails CustContactDetails);
        void DeleteCustContactDetails(int CustContactDetailsId);
        void UpdateCustContactDetails(CustomerContactDetails CustContactDetails);
        List<CustomerContactDetails> GetState(int CountryID);
        List<CustomerContactDetails> GetCity(int StateID, int CountryID);
        List<CustomerContactDetails> GetDistrictpincode(string pincode);
        List<CustomerContactDetails> Getcitylists(int Pincodeid);
        //void UpdateSupHeaderGidDirectors();


        List<EntityGstCustomer> getcustomer();
        List<EntityGstCustomer> GetState();
        EntityGstCustomer GetStateById(int StateId);
        string Insertcustomer(EntityGstCustomer Classifications);
        EntityGstCustomer getCustomerid(int PinId);
        string Updatecustomer(EntityGstCustomer Classifications);
        void DeleteCustomerMasterDetail(int customerID);
        string ApproveCustomerMasterDetail();
    }
	    public interface Iiem_common_delegation
    {
        IEnumerable<iem_common_delegation> GetDelegationList();
        IEnumerable<iem_common_delegation> GetDelegation(string delegateby, string delegateto, string periodfrom, string periodto, string delmatype);
        iem_common_delegation GetDelegationById(int DelegationGid);
        iem_common_delegation GetDelegationLoginDetails(int Logingid);
        string InsertCommonDelegationitem(iem_common_delegation DelegationModel);
        string DeleteDelegation(int DelegationGid);
        string UpdateDelegation(iem_common_delegation UpdateDelegationmodel);
        IEnumerable<GetDelmattype> GetDelmattype();
        IEnumerable<GetDepartment> GetDepartmenttype();
        IEnumerable<iem_common_delegation> EmployeeSearch();
        IEnumerable<iem_common_delegation> EmployeeSearch(string EmpName, string Empcode);
    }
}