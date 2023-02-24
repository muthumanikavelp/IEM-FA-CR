using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using IEM.Areas.FLEXIBUY.Models;

namespace IEM.Areas.FLEXIBUY.Models
{
    public interface PrintRepository
    {
        IEnumerable<CbfPrintEntity> GetCBFSummary(string CBFNumber = null);
        CbfPrintEntity GetCBFData(string CBFNumber = null);
        IEnumerable<CbfPrintEntity> GetCBFDetailsList(string CBFNumber = null);
        IEnumerable<ApprovalHistoryForPrint> GetApproverList(string CBFNumber = null, string ActionFor = null);

        IEnumerable<POPrintEntity> GetPOSummary(string PONumber = null);
        IEnumerable<WOPrintEntity> GetWOSummary(string WONumber = null);
       
        POPrintEntity GetPOData(string PONumber = null);  
        IEnumerable<POPrintEntity> GetPODetailsList(string PONumber = null);

        WOPrintEntity GetWOData(string WONumber = null);
        IEnumerable<WOPrintEntity> GetWODetailsList(string WONumber = null);
        int GetBranchIsSingle(int pogid = 0);

        IEnumerable<ShipmentDetails> GetShipmentDetails(int refgid = 0);
        DataTable GetTC();
        IEnumerable<ShipmentDetails> GetShipmentDetailsWO(int refgid = 0);

        DataSet GetDocSummary();
        DataSet GetQueryScreenData();
        DataSet GetVendorSelectionData();
        DataSet VendorSelectionOpexData();
        DataSet VendorSelectionOpexDetailsSummary(string docgid = null,string doctype = null);
        DataSet  GetCCAutoComplete(string searchtext = null);
        DataSet GetOBFDetailsById(string docgid = null);
        int UpdateOBFDetails(OBFDetails objOBFDetails);
        DataSet GetOBFDetailsAll(int docgid = 0);
        int SubmitOBFDetails(OBFDetails objOBFDetails);
        DataSet VendorSelectionOpexDetailsSummary1(string docgid = null);
        DataSet VendorSelectionOpexDetailsSummaryOld(string docgid = null);

        DataSet GetHeaderDetailsPAR(string docgid = null);
        DataSet GenerateOBFDetails(OBFDetails objOBFDetails);
        DataSet VendorSelectionOpexDetailsSummaryOldPAR(string docgid = null);
        DataSet GetOBFDetailsByIdPAR(string docgid = null);
        int UpdateOBFDetailsPAR(OBFDetails objOBFDetails); 
        DataSet GetOBFDetailsAllPAR(int docgid = 0); 
        int SubmitOBFDetailsPAR(OBFDetails objOBFDetails);
        int CheckProductExists(int obfgid = 0);
        DataSet GetPORptData(string cbfnumber = null, string ponumber = null, string podate = null, string poenddate = null);
        DataSet GetPORptDataFull(string cbfnumber = null, string ponumber = null, string podate = null, string poenddate = null);

        DataSet PendingForConfirmationData();
    } 
}