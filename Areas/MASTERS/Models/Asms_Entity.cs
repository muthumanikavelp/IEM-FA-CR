using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IEM.Areas.MASTERS.Models
{
    public class Asms_Entity
    {

    }

    public class OrgTypeModel
    {
        public Int32 sNo { get; set; }
        public Int32 typeID { get; set; }
        public string typeName { get; set; }
        public List<OrgTypeModel> orgTypeModelList { get; set; }
    }

    public class ServiceTypeModel
    {
        public Int32 sNo { get; set; }    
        public Int32 serviceID { get; set; }
        public string serviceName { get; set; }
        public List<ServiceTypeModel> serviceTypeModelList { get; set; }
    }

    public class CategoryModel
    {
        public Int32 sNo { get; set; }    
        public Int32 categoryID { get; set; }
        public string categoryName { get; set; }
        public List<CategoryModel> categoryModelList { get; set; }
    }
   
    public class DocGrpModel
    {
        public Int32 sNo { get; set; }    
        public Int32 docGrpID { get; set; }
        public string docGrpName { get; set; }
        public List<DocGrpModel> docGrpList { get; set; }
    }

    public class DocFrqModel
    {
        public Int32 sNo { get; set; }    
        public Int32 docFrqID { get; set; }
        public string docFrqName { get; set; }
        public Int32 docFrqMonth { get; set; }
        public List<DocFrqModel> docFrqList { get; set; }
    }
    public class FrqModel
    {        
        public Int32 frqID { get; set; }
        public string frqName { get; set; }
        public List<FrqModel> frqList { get; set; }
    }

    public class DocNameModel
    {
        public Int32 sNo { get; set; }     
        public string docFrqID { get; set; }
        public string docFrqName { get; set; }
        public Int32 frqID { get; set; }
        public string frqName { get; set; }
        public SelectList FrqModel { get; set; }
        public SelectList docFrqModel { get; set; }
        public Int32 docGrpID { get; set; }
        public string docGrpName { get; set; }
        public SelectList docGrpModel { get; set; }
        public Int32 docNameID { get; set; }
        public string docNameName { get; set; }
        public List<DocFrqModel> docFrqList { get; set; }
        public List<DocGrpModel> docGrpList { get; set; }
        public List<DocNameModel> docNameList { get; set; }
        public List<FrqModel> frqList { get; set; }
        public string manOpt { get; set; }
        public Int32 manDays { get; set; }
    }

    public class AgreementModel
    {
        public Int32 sNo { get; set; }    
        public Int32 agreementID { get; set; }
        public string agreementName { get; set; }
        public List<AgreementModel> agreementModelList { get; set; }
    }

    public class SECategoryModel
    {
        public Int32 sNo { get; set; }    
        public Int32 seCategoryID { get; set; }
        public string seCategoryName { get; set; }
        public SelectList seCategorylist { get; set; }
        public List<SECategoryModel> seCategoryModelList { get; set; }
    }

    public class SErateModel
    {
        public Int32 sNo { get; set; }    
        public Int32 seRateID { get; set; }
        public string seRateName { get; set; }
        public SelectList seRate { get; set; }
    }

    public class SESubCategoryModel
    {
        public Int32 sNo { get; set; }  
        public SelectListItem seScoreModel { get; set; }
        public SelectList seRateMod { get; set; }
        public Int32 seRateID { get; set; }
        public string seRateName { get; set; }
        public Int32 seCategoryID { get; set; }
        public string seCategoryName { get; set; }
        public string seRatescore { get; set; }
        public SelectList seCategoryMod { get; set; }
        public Int32 seSubCategoryID { get; set; }
        public string seSubCategoryName { get; set; }
        public List<SESubCategoryModel> seSubCategoryList { get; set; }
        public List<SECategoryModel> seCategoryList { get; set; }
    }

}