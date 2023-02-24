using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEM.Areas.Dashboard.Models
{
    public class ASMSPDFModel
    {
        public string ASMSType { get; set; }
        public string Supplier { get; set; }
        public string Raiser { get; set; }
        public string ContractFrom { get; set; }
        public string ContractTo { get; set; }
        public string RaiserComments { get; set; }
        public string PreviousApprover { get; set; }
        public string PreviousApproverRemarks { get; set; }

        //ACTIVATION
        public string ActivationFrom { get; set; }
        public string ActivationTo { get; set; }
        public string ActivateReason { get; set; }
        //DEACTIVATION
        public string DeActivateReason { get; set; }
        //ONBOARDING & RENEWAL
        public string ApproxContractValue { get; set; }
        public string GSTINProvided { get; set; }
        //RENEWAL
        public string PreviousContractFrom { get; set; }
        public string PreviousContractTo { get; set; }
    }
}

/*
                            if (self.VendorTitle() == "ACTIVATION") {
                                $("#txtASMSActivationFrom").val(Data1[0].ActivationFrom);
                                $("#txtASMSActivationTo").val(Data1[0].ActivationTo);
                                $("#txtASMSActivationReason").val(Data1[0].ActivateReason);
                            }
                            if (self.VendorTitle() == "DEACTIVATION") {
                                $("#txtASMSDeActivationReason").val(Data1[0].DeActivateReason);
                            }
                            if (self.VendorTitle() == "ONBOARDING") {
                                $("#txtASMSApproxContractValue").val(Data1[0].ApproxContractValue == null ? "" : Data1[0].ApproxContractValue);
                                $("#txtASMSGSTINProvided").val(Data1[0].GSTINProvided);
                            }
                            if (self.VendorTitle() == "RENEWAL") {
                                $("#txtASMSApproxContractValue").val(Data1[0].ApproxContractValue == null ? "" : Data1[0].ApproxContractValue);
                                $("#txtASMSGSTINProvided").val(Data1[0].GSTINProvided);
                                $("#txtASMSPrevContractStrDate").val(Data1[0].PreviousContractFrom);
                                $("#txtASMSPrevContractEndDate").val(Data1[0].PreviousContractTo);
                            }
*/