using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEM.Areas.IFAMS.Models
{
    public class ReportEntity_M
    { }
    public class AssetReportModel
    {
        public string years { get; set; }
        public dynamic months { get; set; }
        public string jan { get; set; }
        public string feb { get; set; }
        public string mar { get; set; }
        public string apr { get; set; }
        public string may { get; set; }
        public string june { get; set; }
        public string jul { get; set; }
        public string aug { get; set; }
        public string sep { get; set; }
        public string oct { get; set; }
        public string nov { get; set; }
        public string dec { get; set; }
        public string[] assetdetDetidarray { get; set; }
        public string FAGroupid { get; set; }
        public string assetidqty { get; set; }
        public string Year { get; set; }
        public int countYear { get; set; }
        //soaheader table       
        public Int32 soaGid { get; set; }
        public string soaProcessdate { get; set; }
        public string soaSalenumber { get; set; }
        public decimal soaSalevalue { get; set; }
        public string soaSaledate { get; set; }
        public Int32 soaLocgid { get; set; }
        public Int32 soaJobcode { get; set; }
        public string soaUploadfilename { get; set; }
        public DateTime soaUploasdate { get; set; }
        public string soaMakerid { get; set; }
        public DateTime soaMakerdate { get; set; }
        public string soaCheckerid { get; set; }
        public DateTime soaCheckerdate { get; set; }
        public String soaisremoved { get; set; }
        public Int32 soaNorecords { get; set; }
        public string soaStatus { get; set; }
        public string saleSheetname { get; set; }
        public Int32 saleSheetid { get; set; }
        public string soaVendorname { get; set; }
        public string soavendoradd { get; set; }
        public string soaVatamt { get; set; }
        public string soaVatper { get; set; }
        public string soaProportion { get; set; }
        public Int32 soaTnorecords { get; set; }
        public decimal soaWtaxamt { get; set; }
        public string soacaptilisationdat { get; set; }
        public string soainvno { get; set; }
        


        //Assetdetails
        public string assetdetDetid { get; set; }
        public string assetdetCode { get; set; }
        public string assetdetDescription { get; set; }
        public string assetdetLocationcode { get; set; }
        public decimal assetdetAssetvalue { get; set; }
        public int assetdetSaleid { get; set; }
        public int assetdetgid { get; set; }
        public decimal assetdettrfval { get; set; }
        public string assetdetgroupid { get; set; }
        public string assetdettrndate { get; set; }
        public string assetdettaken { get; set; }
        public string branchlaunchdat { get; set; }
        public string assetleasedat { get; set; }
        public string assetleaseenddat { get; set; }
        public string assetcapdate { get; set; }
        public string assetdetqty { get; set; }
        public string assettrffrm { get; set; }
        public decimal assettrfvalue { get; set; }
        public string assetdetstaus { get; set; }
        public string assetdettrfdat { get; set; }
        public string assetdetsaledat { get; set; }
        public string department { get; set; }
        public string assetdetspoke { get; set; }
        public string bscc { get; set; }
        public string upbscc { get; set; }
        public string ponumb { get; set; }
        public string cbfnumb { get; set; }
        public string vendornam { get; set; }
        public string Naration { get; set; }
        public string history { get; set; }
        public string assetdetponum { get; set; }
        public string assetdetstatus1 { get; set; }
        public string BRCode {get; set;}
        public string BRName {get; set;}
        public string BRAddress {get; set;}
        public string BRActive {get; set;}
        public string BRSdate { get; set; }
        public string barCode { get; set; }
        public string assetSerialNo { get; set; }


       

        //Soadetail
        public decimal soadetSalevalue { get; set; }
        public decimal soadetVatvalue { get; set; }
        public decimal soadetWdvalue { get; set; }
        public decimal soadetProfitloss { get; set; }
        public decimal soadetIvvalue { get; set; }
        public Int32 soadetsoaheadgid { get; set; }
        public Int32 soadetgid { get; set; }
        public Int32 soadetassetdetgid { get; set; }
        public decimal saleamt { get; set; }
        public string soadetinwno { get; set; }
        public decimal soadetrectamt { get; set; }

        //soapayment
        public Int32 soapayGid { get; set; }
        public Int32 soapaySoaheadergid { get; set; }
        public string soapaySaleno { get; set; }
        public string soapayChqno { get; set; }
        public string soapayAmount { get; set; }
        public string soapayRealizationdate { get; set; }
        public string soapayIsremoved { get; set; }
        public string soapaychqdat { get; set; }
        public string soapaymode { get; set; }
        public string soapayGL { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }

        //transfer
        public string toaProcessdate { get; set; }
        public string toaTrannumber { get; set; }
        public decimal toaTranvalue { get; set; }
        public string toaTrandate { get; set; }
        public Int32 toaLocgid { get; set; }
        public string toaJobcode { get; set; }
        public string toaUploadfilename { get; set; }
        public string toaUploasdate { get; set; }
        public string toaMakerid { get; set; }
        public DateTime toaMakerdate { get; set; }
        public string toaCheckerid { get; set; }
        public DateTime toaCheckerdate { get; set; }
        public String toaisremoved { get; set; }
        public Int32 toaNorecords { get; set; }
        public string toaStatus { get; set; }
        public string toaVendorname { get; set; }
        public string toavendoradd { get; set; }
        public string toaVatamt { get; set; }
        public string toaVatper { get; set; }
        public string toaProportion { get; set; }
        public Int32 toaTnorecords { get; set; }
        public decimal toaWtaxamt { get; set; }
        public string toacaptilisationdat { get; set; }
        public string toatfrdate { get; set; }
        public string toatfrvaluedate { get; set; }

        //transfer detail
        public decimal toadetTranvalue { get; set; }
        public decimal toadetVatvalue { get; set; }
        public decimal toadetWdvalue { get; set; }
        public decimal toadetProfitloss { get; set; }
        public decimal toadetIvvalue { get; set; }
        public Int32 toadettoaheadgid { get; set; }
        public Int32 toadetgid { get; set; }
        public Int32 toadetassetdetgid { get; set; }
        public string toadetinwno { get; set; }
        public decimal toadetrectamt { get; set; }
        public string toadetnewassetdetid { get; set; }


        //Writeoff
        public string woaProcessdate { get; set; }
        public string woaWOnumber { get; set; }
        public decimal woaWOvalue { get; set; }
        public string woaWOdate { get; set; }
        public Int32 woaLocgid { get; set; }
        public string woaJobcode { get; set; }
        public string woaUploadfilename { get; set; }
        public string woaUploasdate { get; set; }
        public string woaMakerid { get; set; }
        public DateTime woaMakerdate { get; set; }
        public string woaCheckerid { get; set; }
        public DateTime woaCheckerdate { get; set; }
        public String woaisremoved { get; set; }
        public Int32 woaNorecords { get; set; }
        public string woaStatus { get; set; }
        public string woaVendorname { get; set; }
        public string woavendoradd { get; set; }
        public string woaVatamt { get; set; }
        public string woaVatper { get; set; }
        public string woaProportion { get; set; }
        public Int32 woaTnorecords { get; set; }
        public decimal woaaWtaxamt { get; set; }
        public string woacaptilisationdat { get; set; }
        public string woatfrdate { get; set; }
        public string woatfrvaluedate { get; set; }

        //Writeoff detail
        public decimal woadetWOvalue { get; set; }
        public decimal woadetVatvalue { get; set; }
        public decimal woadetWdvalue { get; set; }
        public decimal woadetProfitloss { get; set; }
        public decimal woadetIvvalue { get; set; }
        public Int32 woadettoaheadgid { get; set; }
        public Int32 woadetgid { get; set; }
        public Int32 woadetassetdetgid { get; set; }
        public string woadetinwno { get; set; }
        public decimal woadetrectamt { get; set; }
        public string woadetnewassetdetid { get; set; }
        public decimal woadeptilldate { get; set; }

        //AssetCodeChange
        public string accnewassetdetid { get; set; }
        public string accnewassetcode { get; set; }
        public string accupdateby { get; set; }
        public string accupdatedate { get; set; }
        public string Reason { get; set; }
        public string accoldassetcode { get; set; }


        //goaheader

        public int goahgid { get; set; }
        public string goagroupid { get; set; }
        public string goastatus { get; set; }
        public string goaupdateby { get; set; }
        public string goaupdatedate { get; set; }
        public string goaphysicalID { get; set; }

        //depreciation
        public string deptype { get; set; }
        public string deprate { get; set; }
        public string depgl { get; set; }
        public string deppvgl { get; set; }
        public string depjan { get; set; }
        public string depfeb { get; set; }
        public string depmar { get; set; }
        public string depapr { get; set; }
        public string depmay { get; set; }
        public string depjun { get; set; }
        public string depjul { get; set; }
        public string depaug { get; set; }
        public string depsep { get; set; }
        public string depoct { get; set; }
        public string depnov { get; set; }
        public string depdec { get; set; }
        public string deptot { get; set; }

        public string wdv { get; set; }

        //FAReport
        public string fainwrdfdate { get; set; }
        public string fainwrdtdate { get; set; }
        public string fadepfdate { get; set; }
        public string fadeptdate { get; set; }
        public string fahistorydate { get; set; }
        public string fagrpid { get; set; }
        public string Command { get; set; }

        public string CLASSIFICATION { get; set; }
        public string OFFICE { get; set; }
        public string Branch_Status { get; set; }
        public string branch_branchtype_name { get; set; }
        public string VERIFIABLE_STATUS { get; set; }
        public string ASSET_STATUS { get; set; }
        public string SALE_VALUE { get; set; }
        public string VAT_VALUE { get; set; }
        public string TOTAL_SALE_VALUE { get; set; }
        public string NET_LOSS { get; set; }
        public string ACCUMULATED_DEP { get; set; }
        public string CUMMULATIVE_DEP { get; set; }
        public string inwarddate { get; set; }
        public string acfnumber { get; set; }
        public string assetdetglcode { get; set; }

        public List<AssetReportModel> ReportModel { get; set; }
        public List<AssetReportModel> ReportModel1 { get; set; }
        public List<AssetReportModel> ReportModel2 { get; set; }

        //
        public string Particulars { get; set; }
        public string OracleGL_g { get; set; }
        public string Opening_g { get; set; }
        public string Additions_g { get; set; }
        public string Deletions_g { get; set; }
        public string Impairment_g { get; set; }
        public string Closing_g { get; set; }
        public string OracleGL_d { get; set; }
        public string Opening_d { get; set; }
        public string Additions_d { get; set; }
        public string Deletions_d { get; set; }
        public string Impairment_d { get; set; }
        public string Closing_d { get; set; }
        public string Opening_n { get; set; }
        public string Closing_n { get; set; }
        public string tb_g { get; set; }
        public string tb_d { get; set; }
        public string diff_g { get; set; }
        public string diff_d { get; set; }


        //asset clearing 

        public string ECF_No { get; set; }
        public string Invoice_No { get; set; }
        public string Invoice_Amount { get; set; }
        public string Asset_Category { get; set; }
        public string Asset_SubCategory_Code { get; set; }
        public string Asset_Description { get; set; }
        public string GL_Code { get; set; }
        public string Qunatity { get; set; }
        public string Rate { get; set; }
        public string Amount { get; set; }
        public string Transaction_Type { get; set; }
        public string Shipment_Type { get; set; }
        public string Branch { get; set; }
        public string Satus { get; set; }
        public decimal cgst_amount { get; set; }
        public decimal sgst_amount { get; set; }
        public decimal igst_amount { get; set; }
        public decimal cgcess_amount { get; set; }
        public decimal sgcess_amount { get; set; }
        public decimal igcess_amount { get; set; }
        public decimal ccess_amount { get; set; }
        public decimal taxable_amount { get; set; }
    }
}