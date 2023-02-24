using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Web.Mvc;

namespace IEM.Areas.MASTERS.Models
{
    public interface Gstpincode
    {
        IEnumerable<EntityGSTPincode> getPincode();
        IEnumerable<EntityGSTPincode> getPincode_list(string Pincode_code);
        string Insertpincode(EntityGSTPincode Classifications);
        string Updatepincode(EntityGSTPincode Classifications);
        string Deletepincode(int PinId);

        IEnumerable<EntityGSTPincode> GetDistrict();
        EntityGSTPincode GetDistrictById(int DistricrtId);        
        EntityGSTPincode getPincodeId(int PinId);
         

        IEnumerable<EntityGSTPincode> GetDistrictByStateId(int stateId);

        IEnumerable<EntityGSTPincode> GetState();
        EntityGSTPincode GetStateById(int StateId);
        //EntityGSTPincode getStateId(int SId); 

        //iem_mst_pincode GetCountryId(int CityId);
       

        //IEnumerable<Getcurrency> GetCurrency();
        //GetcurrencyById GetCurrencyById(int Curency);       
    }
    public interface GstVendor
    {
        IEnumerable<EntityGstvendor> getvendor();
        IEnumerable<EntityGstvendor> getmaker();        
        IEnumerable<EntityGstvendor> GetState();
        EntityGstvendor GetStateById(int StateId);
        string Insertvendor(EntityGstvendor Classifications);
        EntityGstvendor getVendorid(int PinId);

        string Updatevendor(EntityGstvendor Classifications);
        string DeleteVendor(int PinId);
        
    }
    public interface GstHSNCode
    {
        IEnumerable<EntityGstHsn> gethsncode();
        IEnumerable<EntityGstHsn> getHsncode_list(string Hsncode);
        string Inserthsncode(EntityGstHsn Classifications);       
        string Updatehsncode(EntityGstHsn Classifications);       
        string Deletehsncode(int PinId);

        EntityGstHsn gethsncodeid(int PinId);
        
    }
    public interface GSTFicc
    { 
        IEnumerable<EntityGstFicc> getFicc();
        String insertbranch(EntityGstFicc Classifications);
        String updatebranch(EntityGstFicc Classifications); 
      
    }

    public interface GSTFIccBranch
    {
         
        IEnumerable<EntityGSTFiccBranch> getPincode();
        IEnumerable<EntityGSTFiccBranch> SlectBranchType();
        IEnumerable<EntityGSTFiccBranch> SelectCity();
        IEnumerable<EntityGSTFiccBranch> SelectLoction();
        IEnumerable<EntityGSTFiccBranch> SelectEmployee();
        IEnumerable<EntityGSTFiccBranch> SelectEmployeeSearch(string EmployeeName, string EmployeeCode);
        IEnumerable<EntityGSTFiccBranch> SelectBranch();
        IEnumerable<EntityGSTFiccBranch> SelectBranchEdit(int id);
        string InsertBranch(EntityGSTFiccBranch bran);
        string UpdateBranch(EntityGSTFiccBranch bran);
       // string EditBranch(EntityGSTFiccBranch bran);
        string DeleteBranch(int id);
        IEnumerable<EntityGSTFiccBranch> Search(string BranchCode = null, string BranchName = null, string BranchType = null, string City = null, string Branch = null, string ActiveStatus = null);
        DataTable BranchLoad();

        IEnumerable<EntityGSTFiccBranch> GetDistrictByStateId(string stateId);
        IEnumerable<EntityGSTFiccBranch> getstate();
        IEnumerable<EntityGSTFiccBranch> getdistrict();
    }

}
