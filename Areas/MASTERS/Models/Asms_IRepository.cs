using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEM.Areas.MASTERS.Models
{
    public interface Asms_IRepository
    {
        //OrgType
        IEnumerable<OrgTypeModel> GetOrgType(string filter);
        OrgTypeModel GetOrgTypeById(int OrgTypeId);
        string InsertOrgType(OrgTypeModel OrgTypess);
        OrgTypeModel DeleteOrgType(int OrgTypesId);
        string UpdateOrgType(OrgTypeModel OrgTypes);

        //ServiceType
        IEnumerable<ServiceTypeModel> GetServiceType(string filter);
        ServiceTypeModel GetServiceTypeById(int ServiceType);
        string InsertServiceType(ServiceTypeModel ServiceType);
        string EditServiceType(ServiceTypeModel ServiceType);
        ServiceTypeModel DeleteServiceType(int ServiceType);
        IEnumerable<SErateModel> GetSEratedrop();

        //SupplierCategory
        IEnumerable<CategoryModel> GetCategory(string filter);
        CategoryModel GetCategoryById(int Category);
        string InsertCategory(CategoryModel Category);
        string EditCategory(CategoryModel Category);
        CategoryModel DeleteCategory(int Category);
       

        // Document Group   
        IEnumerable<DocGrpModel> GetDocGrp(string filter);
        DocGrpModel GetDocGrpById(int DocGrp);
        string InsertDocGrp(DocGrpModel DocGrp);
        string EditDocGrp(DocGrpModel DocGrp);
        string DeleteDocGrp(int DocGrp);

        //Document Frequency
        IEnumerable<DocFrqModel> GetDocFrq(string filter);
        DocFrqModel GetDocFrqById(int DocFrq);
        string InsertDocFrq(DocFrqModel DocFrq);
        string EditDocFrq(DocFrqModel DocFrq);
        string DeleteDocFrq(int DocFrq);

        //Document Name
        IEnumerable<DocNameModel> GetDocName(string filter);        
        DocNameModel GetDocNameById(int DocName);
        string InsertDocName(DocNameModel DocName);
        string EditDocName(DocNameModel DocName);
        DocNameModel DeleteDocName(int DocName);
        string GetDocGrpID(int DocFrqID);
        string GetDocFrqID(int DocFrqID);
        IEnumerable<DocFrqModel> GetDocFrq();
        IEnumerable<DocFrqModel> GetDocFrqedit();
        IEnumerable<DocGrpModel> GetDocGrp();
        IEnumerable<FrqModel> GetFrq();

        //Supplier Evaluation Category     
        IEnumerable<SECategoryModel> GetSECategory(string filter);
        IEnumerable<SECategoryModel> GetSECategoryWithQues();
        IEnumerable<SECategoryModel> GetSECategory();
        SECategoryModel GetSECategoryById(int SECategory);
       string InsertSECategory(SECategoryModel SECategory);
       string EditSECategory(SECategoryModel SECategory);
       string DeleteSECategory(int SECategory);

       //Supplier Evaluation Sub Category OR Supplier Evaluation Questions
        IEnumerable<SESubCategoryModel> GetSESubCategory(string filter);
        SESubCategoryModel GetSESubCategoryById(int SESubCategoryModel);
        IEnumerable<SErateModel> GetSErate();
        string InsertSESubCategory(SESubCategoryModel SESubCategoryModel);
        string EditSESubCategory(SESubCategoryModel SESubCategoryModel);
        SESubCategoryModel DeleteSESubCategory(int SESubCategoryModel);
        IEnumerable<SESubCategoryModel> GetScoreByGrp(int id);
        string GetSErateById(int id);
        string GetSECategoryId(int SECategoryId);
        string GetCategoryname(int SECategoryId);      

    }

}