var objDialogyAddDebit, objDialogyAddPayment, objDialogyAddAttachment, objDialogyPPX, objDialogyExpenseDetails, objDialogyAddPayment1;

UrlDet = UrlDet.replace("GetSupplierInvoiceMaker", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");
//------------- Shrinidhi 03.05.2016------------------------
var BenName = "";
///---------------------------
var SearchModel = function (id) {
    var self = this;

    self.ECFExpenseArray = ko.observableArray();
    self.ECFPaymentArray = ko.observableArray();
    self.ECFAttachmentArray = ko.observableArray();
    self.ECFAuditrailArray = ko.observableArray();

    self.CurrencyArray = ko.observableArray();
    self.AttachmentTypeArray = ko.observableArray();

    self.EOWInexArray = ko.observableArray();
    self.EOWDespatchArray = ko.observableArray();
    self.EOWPaymentArray = ko.observableArray();
    self.PaymentDespatchArray = ko.observableArray();

    self.ECFTravelExpenseArray = ko.observableArray();

    self.PayBankArray = ko.observableArray();
    self.PayModeCommonArray = ko.observableArray();
    self.PayModeLCEditArray = ko.observableArray();
    self.DefaultPayBank = ko.observable("0");

    //START --Run Dedup
    self.DedupDetailsArray = ko.observableArray();

    self.formatNumber = function (num) {
        //return Number(num).toFixed(2);
        var currentval = Number(num).toFixed(2);

        var testDecimal;
        currentval.match(/\./g) === null ? testDecimal = 0 : testDecimal = currentval.match(/\./g);
        if (testDecimal.length > 1) {
            currentval = currentval.slice(0, -1);
        }

        var components = currentval.toString().split(".");
        if (components.length === 1)
            components[0] = currentval;
        components[0] = components[0].replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        if (components.length === 2)
            components[1] = components[1].replace(/\D/g, "");
        return components.join(".");
    };

    this.RunDedup = function () {
        var _tmpData = {
            EcfId: $("#hfECFId").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "RunDedupLC",
            data: JSON.stringify(_tmpData),
            contentType: "application/json;",
            success: function (response) {
                self.DedupDetailsArray.removeAll();
                $('#hfISDedup').val("1");
                var Data1 = "", Data2 = "";
                if (response != null && response != "") {
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                        Data1 = JSON.parse(response.Data1);

                        if (Data1[0].Message != "") {
                            jAlert(Data1[0].Message, "Message");
                        }
                    }
                    if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                        Data2 = JSON.parse(response.Data2);
                        self.DedupDetailsArray(Data2);
                    }
                }
                else {
                    jAlert("unable to process your request.. please try again.", "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.DedupDownload = function () {
        var EcfId = parseInt($("#hfECFId").val());
        var downloadUrl = UrlDet + "DownloadDedupLCData/" + EcfId;
        ko.utils.postJson(downloadUrl);
    };
    //END --Run Dedup

    this.LoadECFDetails = function () {
      
        var EcfId = $("#hdfEcfId").val();
        $("#hfECFId").val(EcfId);
        var EcfFilter = {
            EcfId: EcfId
        };

        $.ajax({
            type: "post",
            url: UrlDet + "GetEmployeeLocalConveyance",
            data: JSON.stringify(EcfFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "", Data3 = "", Data4 = "", Data5 = "", Data6 = "", Data7 = "", Data8 = "", Data9 = "", Data10 = "",Data12 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                if (Data2.length > 0) {

                    $("#txtECFNo").text(Data2[0].ECFNo);
                    $("#txtECFDate").text(Data2[0].EcfDate);
                    $("#txtECFAmount").text(Data2[0].EcfAmount);
                    $("#lblRaiserMode").text(Data2[0].RaiserMode);
                    $("#lblRaiserCode").text(Data2[0].RaiserId);
                    $("#lblRaiserName").text(Data2[0].RaiserName);
                    $("#txtClaimMonth").text(Data2[0].ClaimMonth);
                    $("#txtExchangeRate").val(Data2[0].ExchangeRate);
                    $("#ddlCurrency").val(Data2[0].CrnID);
                    $("#txtCurrencyAmount").val(Data2[0].CurrencyAmount);
                    $("#txtReducedAmount").val(Data2[0].ReducedAmt);
                    $("#txtProcessedAmount").val(Data2[0].ProcessedAmt);
                    //alert($("rdoGPaymentNetting").val(Data2[0].PaymentNetting))
                    //$("rdoGPaymentNetting").val(Data2[0].PaymentNetting);
                    // var PayNet = Data2[0].PaymentNetting ;
                    //alert(Data2[0].PaymentNetting)
                  
                    if (Data2[0].PaymentNetting == "Y") {
                       // alert('yes')
                        $("#rdoPaymentNettingYes").prop("checked", true)
                       // $("input[name=rdoGPaymentNetting][value=" + Data2[0].PaymentNetting + "]").prop("checked", true);
                    }
                    else {
                       // alert('no')
                        $("#rdoPaymentNettingNo").prop("checked", true)
                      //  $("input[name=rdoGPaymentNetting][value=" + Data2[0].PaymentNetting + "]").prop("checked", false);
                    }
                   
                    $("#txtECFDescription").val(Data2[0].ECFDesc);

                    $("#hfBU").val(Data2[0].fc);
                    $("#hfCC").val(Data2[0].cc);
                    $("#hfEmpProductCode").val(Data2[0].productcode);
                    $("#hfEmpOUCode").val(Data2[0].oucode);
                    $("#hfProductCodeText").val(Data2[0].Product);
                    $("#hfOUCodeText").val(Data2[0].location);
                    $("#hfInvId").val(Data2[0].InvId);

                    if (Data2[0].phyInw == "N") {
                        $("#txtPyhsicalVerify").val("NOT OK");
                        $("#txtPyhsicalVerify").css("color", "red");
                    }
                    else
                        $("#txtPyhsicalVerify").val("OK");
                    if (Data2[0].IsUrgent == "N")
                        $("#txtUrgentTagging").val("No");
                    else {
                        $("#txtUrgentTagging").val("Yes");
                        $("#txtUrgentTagging").css("color", "red");
                        $("#lblUrgent").css("display", "block");
                    }
                }

                if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null)
                    Data3 = JSON.parse(response.Data3);
                self.ECFExpenseArray(Data3);

                if (response.Data4 != null && response.Data4 != "" && JSON.parse(response.Data4) != null)
                    Data4 = JSON.parse(response.Data4);
                self.ECFPaymentArray(Data4);

                if (response.Data5 != null && response.Data5 != "" && JSON.parse(response.Data5) != null)
                    Data5 = JSON.parse(response.Data5);
                self.ECFAttachmentArray(Data5);

                if (response.Data6 != null && response.Data6 != "" && JSON.parse(response.Data6) != null)
                    Data6 = JSON.parse(response.Data6);
                self.ECFAuditrailArray(Data6);

                if (response.Data7 != null && response.Data7 != "" && JSON.parse(response.Data7) != null)
                    Data7 = JSON.parse(response.Data7);
                self.EOWInexArray(Data7);

                if (response.Data8 != null && response.Data8 != "" && JSON.parse(response.Data8) != null)
                    Data8 = JSON.parse(response.Data8);
                self.EOWDespatchArray(Data8);

                if (response.Data9 != null && response.Data9 != "" && JSON.parse(response.Data9) != null)
                    Data9 = JSON.parse(response.Data9);
                self.EOWPaymentArray(Data9);

                if (response.Data10 != null && response.Data10 != "" && JSON.parse(response.Data10) != null)
                    Data10 = JSON.parse(response.Data10);
                self.PaymentDespatchArray(Data10);

                //------------- Shrinidhi 03.05.2016------------------------
                if (response.Data12 != null && response.Data12 != "" && JSON.parse(response.Data12) != null) {
                    Data12 = JSON.parse(response.Data12);
                    BenName = Data12;
                }
                ///---------------------------
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.SetECFDetails = function () {
        var EcfId = $("#hfECFId").val();
        var ReducedAmt = $("#txtReducedAmount").val();
        var ProcessedAmt = $("#txtProcessedAmount").val();
        var PaymentNett = $("input[name=rdoGPaymentNetting]:radio[checked=checked]").attr("value");
        var Amort = "N";
        var AmortFrom = "";
        var AmortTo = "";
        var AmortDesc = "";
        var currencygid = $("#ddlCurrency").val();
        var curencycode = $("#ddlCurrency option:selected").text();
        var currencyrate = $("#txtExchangeRate").val();
        var currencyamt = $("#txtCurrencyAmount").val();
        var ecfamt = $("#txtECFAmount").text();
        var ecfdesc = $("#txtECFDescription").val();

        if ($.trim(currencygid) == "0") {
            jAlert("Ensure Currency!", "Message");
            return false;
        }

        if ($.trim(currencyrate) == "" || parseFloat(currencyrate) == 0) {
            jAlert("Ensure Currency Rate!", "Message");
            return false;
        }

        var ECFHeader = {
            EcfId: EcfId,
            ReducedAmt: ReducedAmt,
            ProcessedAmt: ProcessedAmt,
            PaymentNett: PaymentNett,
            Amort: Amort,
            AmortFrom: AmortFrom,
            AmortTo: AmortTo,
            AmortDesc: AmortDesc,
            currencygid: currencygid,
            curencycode: curencycode,
            currencyrate: currencyrate,
            currencyamt: currencyamt,
            ecfamt: ecfamt,
            ecfdesc: ecfdesc
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetECFHeaderDetails",
            data: JSON.stringify(ECFHeader),
            contentType: "application/json;",
            success: function (response) {
               // alert('response')
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == false)
                        jAlert(Data1[0].Msg, "Message");
                    else {
                        jAlert(Data1[0].Msg, "Message");
                        self.LoadECFDetails();
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.SetInvoiceCreditDetails = function (flag) {
        var paymode = "", RefNo = "", Beneficiary = "", Amount = "", Desc = "";
        var PayBank = 0;
        var CreditlineGId = $("#hfCreditlineGId").val();
        var Ecfid = $("#hfECFId").val();
        var InvId = $("#hfInvId").val();
        var RefId = $("#hfRefId").val();
        var IfscCode = $("#hfIFSCCode").val();
        paymode = $("#ddlPayMode1 option:selected").text();
        if (flag == "1" && paymode != "RRP") {
            PayBank = $("#ddlPayBank").val();
            RefNo = $("#txtPayRefNo1").val();
            Beneficiary = $("#txtPayBeneficiary1").val();
            Amount = $("#txtPaymentAmount1").val();
            Desc = $("#txtPayDescription1").val();
        }
        else { 
            PayBank = "0";
            RefNo = $("#txtPayRefNo1").val();
            Beneficiary = "";
            Amount = $("#txtPaymentAmount1").val();
            Desc = $("#txtPayDescription1").val();
        }
        var IsRemoved = "0";

        if (paymode == "0" || paymode == "-- Select One --") {
            jAlert("Ensure Payment Mode!", "Message");
            return false;
        }

        if (paymode == "EFT" || paymode == "RRP" || paymode == "ERA") {
            if ($.trim(RefNo) == "") {
                //  alert(RefNo);
                jAlert("Ensure Account No!", "Message");
                return false;
            }
        }

        if (paymode == "PPX") {
            if ($.trim(RefNo) == "") {
                jAlert("Ensure Reference Number!", "Message");
                return false;
            }
        }
        if ($.trim(Beneficiary) == "" && paymode != "PPX" && paymode != "SUS" && paymode != "RRP") {
            jAlert("Ensure Beneficiary Name!", "Message");
            return false;
        }
        if ($.trim(Amount) == "" || parseFloat(Amount) == 0) {
            jAlert("Ensure Payment Amount!", "Message");
            return false;
        }
        if (PayBank == "0" && flag == "1" && paymode != "RRP") {
            jAlert("Ensure Pay Bank!", "Message");
            return false;
        }
        if ($.trim(Desc) == "") {
            jAlert("Ensure Description!", "Message");
            return false;
        }

        var InvoiceCreditLine = {
            CreditlineGId: CreditlineGId,
            Ecfid: Ecfid,
            InvId: InvId,
            RefId: RefId,
            paymode: paymode,
            RefNo: RefNo,
            Beneficiary: Beneficiary,
            Amount: Amount,
            BankId: PayBank,
            Desc: Desc,
            IsRemoved: IsRemoved,
            IfscCode: IfscCode
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetEditCreditLineDetails",
            data: JSON.stringify(InvoiceCreditLine),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == false)
                        jAlert(Data1[0].Msg, "Message");
                    else {
                        objDialogyAddPayment.dialog("close");
                        objDialogyAddPayment1.dialog("close");
                        self.LoadECFDetails();
                        jAlert(Data1[0].Msg, "Message");
                    }
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                //self.ECFPaymentArray(Data2);

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    //edit by anand
    this.DeleteInvoiceCreditDetails = function (Index) {
        var _tmpData = self.ECFPaymentArray()[Index];
        jConfirm("Are you sure? Want to delete Payment!", "Confirm", function (callback) {
            if (callback == true) {
                var Ecfid = $("#hfECFId").val();
                var InvId = $("#hfInvId").val();
                var RefId = $("#hfInvId").val();
                var paymode = _tmpData.PMode;
                var RefNo = _tmpData.RefNo;
                var Beneficiary = _tmpData.Beneficiary;
                var Amount = _tmpData.Amt;
                var Desc = _tmpData.Desc;
                var IsRemoved = "1";
                var PayBank = _tmpData.Bankid;
                var IfscCode = _tmpData.IfscCode;

                var InvoiceCreditLine = {
                    CreditlineGId: _tmpData.CrditLineGID,
                    Ecfid: Ecfid,
                    InvId: InvId,
                    RefId: RefId,
                    paymode: paymode,
                    BankId: PayBank,
                    RefNo: RefNo,
                    Beneficiary: Beneficiary,
                    Amount: Amount,
                    Desc: Desc,
                    IsRemoved: IsRemoved,
                    IfscCode: IfscCode
                };
                $.ajax({
                    type: "post",
                    url: UrlDet + "SetEditCreditLineDetails", // Ramya Modified - 27 jul 18
                    data: JSON.stringify(InvoiceCreditLine),
                    contentType: "application/json;",
                    success: function (response) {
                        var Data1 = "", Data2 = "";
                        if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                            Data1 = JSON.parse(response.Data1);
                            if (Data1[0].Clear == false)
                                jAlert(Data1[0].Msg, "Message");
                            else {
                                objDialogyAddPayment.dialog("close");
                                objDialogyAddPayment1.dialog("close");
                                self.LoadECFDetails();
                                jAlert(Data1[0].Msg, "Message");
                            }
                        }
                        if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                            Data2 = JSON.parse(response.Data2);
                        //self.ECFPaymentArray(Data2);
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        //alert(errorThrown);
                    }
                });
            } else {
                return false;
            }
        });


    };

    this.EditInvoiceCreditDetails = function (Index, flag) {
        var _tmpData = self.ECFPaymentArray()[Index];
        $("#hfIFSCCode").val(_tmpData.IfscCode);
        if (_tmpData.PMode == "DD") {
            $("#txtPayRefNo1").removeAttr("disabled");
        }

        if (_tmpData.PMode == "EFT" || _tmpData.PMode == "CHQ" || _tmpData.PMode == "ERA" || _tmpData.PMode == "DD" || _tmpData.PMode == "RRP") {
            $("#hfCreditlineGId").val(_tmpData.CrditLineGID);
            $("#hfInvId").val(_tmpData.InvId);
            $("#hfRefId").val(_tmpData.InvId);
            $("#ddlPayMode1").val(_tmpData.PMode);
            $("#ddlPayBank").val(_tmpData.Bankid);
            //$("#txtPayRefNo1").val(_tmpData.RefNo);
            $("#txtPayRefNo1").val("211100037");
            $("#txtPayBeneficiary1").val(_tmpData.Beneficiary);
            $("#txtPaymentAmount1").val(_tmpData.Amt);
            $("#txtPayDescription1").val(_tmpData.Desc);

            if ($("#ddlPayBank").val() == "0" || $("#ddlPayBank").val() == "" || $("#ddlPayBank").val() == null) {
                $("#ddlPayBank").removeClass("valid");
                $("#ddlPayBank").addClass("required");
            }
            else {
                $("#ddlPayBank").addClass("valid");
                $("#ddlPayBank").removeClass("required");
            }

            if ($("#ddlPayMode1").val() == "0") {
                $("#ddlPayMode1").removeClass("valid");
                $("#ddlPayMode1").addClass("required");
            }
            else {
                $("#ddlPayMode1").addClass("valid");
                $("#ddlPayMode1").removeClass("required");
            }
            if ($("#txtPayRefNo1").val() == "") {
                $("#txtPayRefNo1").removeClass("valid");
                $("#txtPayRefNo1").addClass("required");
            }
            else {
                $("#txtPayRefNo1").addClass("valid");
                $("#txtPayRefNo1").removeClass("required");
            }
            if ($("#txtPayBeneficiary1").val() == "") {
                $("#txtPayBeneficiary1").removeClass("valid");
                $("#txtPayBeneficiary1").addClass("required");
            }
            else {
                $("#txtPayBeneficiary1").addClass("valid");
                $("#txtPayBeneficiary1").removeClass("required");
            }
            if ($("#txtPayDescription1").val() == "") {
                $("#txtPayDescription1").removeClass("valid");
                $("#txtPayDescription1").addClass("required");
            }
            else {
                $("#txtPayDescription1").addClass("valid");
                $("#txtPayDescription1").removeClass("required");
            }
            if ($("#txtPaymentAmount1").val() == "" || parseFloat($("#txtPaymentAmount1").val()) == 0) {
                $("#txtPaymentAmount1").removeClass("valid");
                $("#txtPaymentAmount1").addClass("required");
            }
            else {
                $("#txtPaymentAmount1").addClass("valid");
                $("#txtPaymentAmount1").removeClass("required");
            }

            if (_tmpData.PMode == "DD") {
                $("#lblPayableAt").text("Payable At");
            }
            else {
                $("#lblPayableAt").text("Account No");
            }

            if (flag == 0) {
                $("#btnCreditLineSubmit1").css("display", "none");
            }
            else
                $("#btnCreditLineSubmit1").css("display", "");
            objDialogyAddPayment1.dialog({ title: 'Payment Details', width: '560', height: '380' });
            objDialogyAddPayment1.dialog("open");
        }
        else {
            $("#hfCreditlineGId").val(_tmpData.CrditLineGID);
            $("#hfInvId").val(_tmpData.InvId);
            $("#hfRefId").val(_tmpData.InvId)
            $("#ddlPayMode").val(_tmpData.PMode);
            $("#txtPayRefNo").val(_tmpData.RefNo);
            $("#txtPayBeneficiary").val(_tmpData.Beneficiary);
            $("#txtPaymentAmount").val(_tmpData.Amt);
            $("#txtPayDescription").val(_tmpData.Desc);
            $("#ddlPayMode").attr("disabled", "disabled");


            if ($("#ddlPayMode").val() == "0") {
                $("#ddlPayMode").removeClass("valid");
                $("#ddlPayMode").addClass("required");
            }
            else {
                $("#ddlPayMode").addClass("valid");
                $("#ddlPayMode").removeClass("required");
            }
            if ($("#txtPayRefNo").val() == "") {
                $("#txtPayRefNo").removeClass("valid");
                $("#txtPayRefNo").addClass("required");
            }
            else {
                $("#txtPayRefNo").addClass("valid");
                $("#txtPayRefNo").removeClass("required");
            }
            if ($("#txtPayDescription").val() == "") {
                $("#txtPayDescription").removeClass("valid");
                $("#txtPayDescription").addClass("required");
            }
            else {
                $("#txtPayDescription").addClass("valid");
                $("#txtPayDescription").removeClass("required");
            }
            if ($("#txtPaymentAmount").val() == "" || parseFloat($("#txtPaymentAmount").val()) == 0) {
                $("#txtPaymentAmount").removeClass("valid");
                $("#txtPaymentAmount").addClass("required");
            }
            else {
                $("#txtPaymentAmount").addClass("valid");
                $("#txtPaymentAmount").removeClass("required");
            }

            if (_tmpData.PMode == "CRD") {
                $("#txtPayBeneficiary").removeClass("valid");
                $("#txtPayBeneficiary").addClass("required");
                $("#txtPayBeneficiary").removeAttr("disabled");
                $("#txtPayRefNo").removeAttr("disabled");

                if ($("#txtPayBeneficiary").val() == "") {
                    $("#txtPayBeneficiary").removeClass("valid");
                    $("#txtPayBeneficiary").addClass("required");
                }
                else {
                    $("#txtPayBeneficiary").addClass("valid");
                    $("#txtPayBeneficiary").removeClass("required");
                }
            }
            else {
                $("#txtPayBeneficiary").removeClass("required");
                $("#txtPayBeneficiary").removeClass("valid");
                $("#txtPayBeneficiary").attr("disabled", "disabled");
                $("#txtPayRefNo").attr("disabled", "disabled");
            }

            if (flag == 0) {
                $("#btnCreditLineSubmit").css("display", "none");
            }
            else
                $("#btnCreditLineSubmit").css("display", "");
            objDialogyAddPayment.dialog({ title: 'Payment Details', width: '560', height: '320' });
            objDialogyAddPayment.dialog("open");
        }
    };

    this.SetAttachmentDetails = function () {

        var EcfId = $("#hdfEcfId").val();
        var AttachmentId = $("#hfAttachmetId").val();
        var AttachmentName = "";
        var AttachmentType = $("#ddlAttachmentType").val();
        var Desc = $("#txtAttachDescription").val();
        var IsRemoved = "0";
        var Attachment = {
            EcfId: EcfId,
            AttachmentId: AttachmentId,
            AttachmentName: AttachmentName,
            AttachmentType: AttachmentType,
            Desc: Desc,
            IsRemoved: IsRemoved
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetAttachmentDetails",
            data: JSON.stringify(Attachment),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = ""; 
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == false)
                        jAlert(Data1[0].Msg, "Error");
                    else {
                        objDialogyAddAttachment.dialog("close");
                        jAlert(Data1[0].Msg, "Message");
                    }
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                if (Data1[0].Clear == true)
                    self.ECFAttachmentArray(Data2);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.DeleteAttachmentDetails = function (AttachmentId) {

        jConfirm("Are you sure? Want to delete Attachment!", "Confirm", function (callback) {
            if (callback == true) {
                var EcfId = $("#hdfEcfId").val();
                var AttachmentName = "";
                var AttachmentType = "";
                var Desc = "";
                var IsRemoved = "1";
                var Attachment = {
                    EcfId: EcfId,
                    AttachmentId: AttachmentId,
                    AttachmentName: AttachmentName,
                    AttachmentType: AttachmentType,
                    Desc: Desc,
                    IsRemoved: IsRemoved
                };
                $.ajax({
                    type: "post",
                    url: UrlDet + "SetAttachmentDetails",
                    data: JSON.stringify(Attachment),
                    contentType: "application/json;",
                    success: function (response) {
                        var Data1 = "", Data2 = ""; 
                        if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                            Data1 = JSON.parse(response.Data1);
                            if (Data1[0].Clear == false)
                                jAlert(Data1[0].Msg, "Error");
                            else {
                                objDialogyAddAttachment.dialog("close");
                                jAlert(Data1[0].Msg, "Message");
                            }
                        }
                        if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                            Data2 = JSON.parse(response.Data2);
                        if (Data1[0].Clear == true)
                            self.ECFAttachmentArray(Data2);
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        //alert(errorThrown);
                    }
                });
            } else {
                return false;
            }
        });
    };

    this.AttachmentDownLoad = function (Id, FileName) {
        //var flieDownload = {
        //    Id: Id,
        //    FileName: FileName
        //};
        //$.ajax({
        //    type: "post",
        //    url: UrlDet + "Download",
        //    data: JSON.stringify(flieDownload),
        //    contentType: "application/json;",
        //    success: function (response) {

        //    },
        //    error: function (XMLHttpRequest, textStatus, errorThrown) {
        //        //alert(errorThrown);
        //    }
        //});
        var iframe = $('<iframe name="postiframe" id="postiframe" style="display: none"></iframe>');
        $("body").append(iframe);
        var form = $('#uploaderForm');
        var FormattedFileName = FileName.replace("&", "-");
        form.attr("action", UrlDet + "Download?id=" + Id + "&FileName=" + FormattedFileName);
        //form.attr("action", UrlDet + "Download?id=" + Id + "&FileName=" + FileName);
        form.attr("method", "post");
        form.attr("encoding", "multipart/form-data");
        form.attr("enctype", "multipart/form-data");
        form.attr("target", "postiframe");
        form.attr("file", $('#attUploader').val());
        form.submit();
    };

    this.LoadCurrencyType = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetCurrencyType",
            data: "{}",
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.CurrencyArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.LoadAttachmentType = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetAttachmentType",
            data: "{}",
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1); 
                self.AttachmentTypeArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.ViewExpenseDetails = function (EmpId, ExpId) {
        var EcfId = $("#hfECFId").val();
        var InputFilter = {
            EcfId: EcfId,
            EmpId: EmpId,
            ExpId: ExpId
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetLocalConveyanceDetails",
            data: JSON.stringify(InputFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.ECFTravelExpenseArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
        $('#ShowDialog4').attr("style", "display:block;");
        objDialogyExpenseDetails.dialog({ title: 'View Expense Details', width: '900', height: '500' });
        objDialogyExpenseDetails.dialog("open");
    };

    this.CloseDetails = function (flag) {
        if (flag == 0)
            objDialogyExpenseDetails.dialog("close");
        if (flag == 1)
            objDialogyAddPayment1.dialog("close");
    };

    this.LoadPayBank = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetPayBank",
            data: "{}",
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.PayBankArray(Data1);

                if (response.Data2 != null && response.Data2 != "") {
                    self.DefaultPayBank(response.Data2)
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.LoadPayModeCommon = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetPayModeCommon1",
            data: "{}",
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.PayModeCommonArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    //Ramya - Edit LC PayMode
    this.LoadEditLCPayMode = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetEditLCPayMode",
            data: "{}",
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.PayModeLCEditArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    };

    self.LoadCurrencyType();
    self.LoadAttachmentType();

    self.LoadECFDetails();

    self.LoadPayModeCommon();
    self.LoadPayBank();
    self.LoadEditLCPayMode();
    //bharathidhasan kumar

    self.ECFAuditHistory = ko.observableArray();

    self.DatatableCall = function () {

        if ($("#tblaudithistory > tbody > tr").length == self.ECFAuditHistory().length) {
            $("#tblaudithistory").DataTable({
                "autoWidth": false,
                "destroy": true,
                "aoColumnDefs": [{
                    "aTargets": ["nosort"],
                    "bSortable": false
                },
                {
                    "aTargets": ["colDate"],
                    "bSortable": true,
                    "sType": "date-uk"
                }]
            });
        }
    };
    self.ECFPaymentHistory = ko.observableArray();
    self.ExpenseDetailhistory = ko.observableArray();
     

    this.LoadAuditHistory = function () {

        var EcfId = $("#hdfEcfId").val();
        $("#hfECFId").val(EcfId);
        var EcfFilter = {
            EcfId: EcfId
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetAuditForTraveClaim",
            data: JSON.stringify(EcfFilter),
            contentType: "application/json;",
            success: function (response) {
                //$("#hfInvId").val("0");
                var Data1 = "", Data2 = "", Data3 = "", Data4 = "", Data5 = "", Data6 = "", Data7 = "", Data8 = "", Data9 = "", Data10 = "", Data11 = "", Data12 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                }
                if (response.Data10!= null && response.Data10 != "" && JSON.parse(response.Data10) != null) {
                    Data10 = JSON.parse(response.Data10);
                }
  

                self.ECFPaymentHistory(Data1);
                self.ExpenseDetailhistory(Data10);
                 

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };
    self.LoadAuditHistory();


};

var mainViewModel = new SearchModel();
ko.applyBindings(mainViewModel, document.getElementById("localConveyance"));

$(document).ready(function () {
    objDialogyAddDebit = $("[id$='ShowDialog1']");
    objDialogyAddDebit.dialog({
        autoOpen: false,
        modal: true,
        width: 800,
        height: 450,
        duration: 300

    });

    objDialogyAddPayment = $("[id$='ShowDialog2']");
    objDialogyAddPayment.dialog({
        autoOpen: false,
        modal: true,
        width: 560,
        height: 330,
        duration: 300
    });

    objDialogyAddAttachment = $("[id$='ShowDialog3']");
    objDialogyAddAttachment.dialog({
        autoOpen: false,
        modal: true,
        width: 560,
        height: 300,
        duration: 300
    });

    objDialogyPPX = $("[id$='PPXDialog']");
    objDialogyPPX.dialog({
        autoOpen: false,
        modal: true,
        width: 900,
        height: 400,
        duration: 300
    });

    objDialogyExpenseDetails = $("[id$='ShowDialog4']");
    objDialogyExpenseDetails.dialog({
        autoOpen: false,
        modal: true,
        width: 900,
        height: 400,
        duration: 300
    });

    objDialogyAddPayment1 = $("[id$='ShowDialogPayBank']");
    objDialogyAddPayment1.dialog({
        autoOpen: false,
        modal: true,
        width: 560,
        height: 380,
        duration: 300
    });

    var objDate = new Date();
    var Presentyear = objDate.getFullYear();
    $(".fsDatepicker").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
    });

    jQuery.extend(jQuery.fn.dataTableExt.oSort, {
        "date-uk-pre": function (a) {
            a = a.split(">")[1].split("<")[0];
            var ukDatea = a.split("/");
            return (ukDatea[2] + ukDatea[1] + ukDatea[0]) * 1;
        },
        "date-uk-asc": function (a, b) {
            return ((a < b) ? -1 : ((a > b) ? 1 : 0));
        },
        "date-uk-desc": function (a, b) {
            return ((a < b) ? 1 : ((a > b) ? -1 : 0));
        }
    });

    $("#txtReducedAmount").keyup(function () {
        var ReducedAmount = $.trim($("#txtReducedAmount").val()) == "" ? 0 : parseFloat($("#txtReducedAmount").val());
        var Amount = $.trim($("#txtECFAmount").text()) == "" ? 0 : parseFloat($("#txtECFAmount").text());
        var ProcessedAmount = Amount - ReducedAmount;
        if (ProcessedAmount < 0) {
            $("#txtReducedAmount").val("0");
            $("#txtProcessedAmount").val($("#txtECFAmount").text());
            jAlert("Reduced Amount should not greater than ECF Amount", "Message");
        }
        else
            $("#txtProcessedAmount").val(ProcessedAmount);
    });

    $("#txtExchangeRate").keyup(function () {
        var Rate = $.trim($("#txtExchangeRate").val()) == "" ? 0 : parseFloat($("#txtExchangeRate").val());
        var Amount = $.trim($("#txtCurrencyAmount").val()) == "" ? 0 : parseFloat($("#txtCurrencyAmount").val());
        var ECFAmount = Rate * Amount;
        $("#txtECFAmount").text(ECFAmount)
        if (Rate <= 0) {
            $("#txtExchangeRate").addClass("required");
            $("#txtExchangeRate").removeClass("valid");
        }
        else {
            $("#txtExchangeRate").removeClass("required");
            $("#txtExchangeRate").addClass("valid");
        }

    });

    $("#ddlCurrency").change(function () {
        var Currency = $("#ddlCurrency").val();
        if (Currency != "0") {
            $("#ddlCurrency").removeClass("required");
            $("#ddlTaxType").addClass("valid");

            var RateFilter = {
                Currency: Currency
            };
            $.ajax({
                type: "post",
                url: UrlDet + "GetCurrencyRate",
                async: false,
                data: JSON.stringify(RateFilter),
                contentType: "application/json;",
                success: function (response) {
                    var Data1 = "";
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                        Data1 = JSON.parse(response.Data1);
                    if (Data1.length > 0) {
                        $("#txtExchangeRate").val(Data1[0].CurrencyRate);
                    }
                    else
                        $("#txtExchangeRate").val("0");
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //alert(errorThrown);
                }
            });
        }
        else {
            $("#txtExchangeRate").val("0");
            $("#ddlCurrency").removeClass("valid");
            $("#ddlCurrency").addClass("required");
        }
        var Rate = $.trim($("#txtExchangeRate").val()) == "" ? 0 : parseFloat($("#txtExchangeRate").val());
        var Amount = $.trim($("#txtCurrencyAmount").val()) == "" ? 0 : parseFloat($("#txtCurrencyAmount").val());
        var ECFAmount = Rate * Amount;
        $("#txtECFAmount").text(ECFAmount);
        if (Rate <= 0) {
            $("#txtExchangeRate").addClass("required");
            $("#txtExchangeRate").removeClass("valid");
        }
        else {
            $("#txtExchangeRate").removeClass("required");
            $("#txtExchangeRate").addClass("valid");
        }
    });

    //Modified
    $("#ddlPayMode").change(function () {
        var PayMode = $("#ddlPayMode").val();
        if (PayMode != "0") {
            $("#ddlPayMode").removeClass("required");
            $("#ddlPayMode").addClass("valid");
            if (PayMode == "CRD") {
                $("#txtPayBeneficiary").removeClass("valid");
                $("#txtPayBeneficiary").addClass("required");

                $("#txtPayBeneficiary").removeAttr("disabled");
                $("#txtPayRefNo").removeAttr("disabled");
            }
            else {
                $("#txtPayBeneficiary").removeClass("required");
                $("#txtPayBeneficiary").removeClass("valid");

                $("#txtPayBeneficiary").attr("disabled", "disabled");
                $("#txtPayRefNo").attr("disabled", "disabled");
            }

        }
        else {
            $("#ddlPayMode").removeClass("valid");
            $("#ddlPayMode").addClass("required");
        }
        $("#txtPayRefNo").val("");
        $("#txtPayBeneficiary").val("");
        $("#txtPaymentAmount").val("");
        $("#txtPayDescription").val("");

        $("#txtPaymentAmount").removeClass("valid");
        $("#txtPaymentAmount").addClass("required");
        $("#txtPayDescription").removeClass("valid");
        $("#txtPayDescription").addClass("required");
        
        if (PayMode == "RRP") {
            $("#txtPayRefNo").val("211100037");

            //----------------- Shrinidhi 03.05.2016-----------------------
            $("#txtPayBeneficiary").val(BenName[0].Beneficiary);
            //-------------------------------------------------------------

            $("#txtPayRefNo").removeClass("required").addClass("valid");
        }
        else
            $("#txtPayRefNo").removeClass("valid").addClass("required");

    });

    $("#txtPayBeneficiary").keyup(function () {
        var Amount = $.trim($("#txtPayBeneficiary").val());
        if (Amount != "") {
            $("#txtPayBeneficiary").removeClass("required");
            $("#txtPayBeneficiary").addClass("valid");
        }
        else {
            $("#txtPayBeneficiary").removeClass("valid");
            $("#txtPayBeneficiary").addClass("required");
        }
    });

    $("#txtPayRefNo").keyup(function () {
        var Amount = $.trim($("#txtPayRefNo").val());
        if (Amount != "") {
            $("#txtPayRefNo").removeClass("required");
            $("#txtPayRefNo").addClass("valid");
        }
        else {
            $("#txtPayRefNo").removeClass("valid");
            $("#txtPayRefNo").addClass("required");
        }
    });

    $("#ddlPayBank").change(function () {
        var PayMode = $("#ddlPayBank").val();
        if (PayMode != "0") {
            $("#ddlPayBank").removeClass("required");
            $("#ddlPayBank").addClass("valid");
        }
        else {
            $("#ddlPayBank").removeClass("valid");
            $("#ddlPayBank").addClass("required");
        }
    });

    $("#txtPayRefNo1").change(function () {
        var PayMode = $("#txtPayRefNo1").val();
        if (PayMode != "") {
            $("#txtPayRefNo1").removeClass("required");
            $("#txtPayRefNo1").addClass("valid");
        }
        else {
            $("#txtPayRefNo1").removeClass("valid");
            $("#txtPayRefNo1").addClass("required");
        }
    });

    $("#txtPayBeneficiary1").change(function () {
        var PayMode = $("#txtPayBeneficiary1").val();
        if (PayMode != "") {
            $("#txtPayBeneficiary1").removeClass("required");
            $("#txtPayBeneficiary1").addClass("valid");
        }
        else {
            $("#txtPayBeneficiary1").removeClass("valid");
            $("#txtPayBeneficiary1").addClass("required");
        }
    });

    $("#ddlPayMode1").change(function () {
        var PayMode = $("#ddlPayMode1").val();
        if (PayMode != "0") {
            $("#ddlPayMode1").removeClass("required");
            $("#ddlPayMode1").addClass("valid");
        }
        else {
            $("#ddlPayMode1").removeClass("valid");
            $("#ddlPayMode1").addClass("required");
        }
        if (PayMode == "DD") {
            $("#txtPayRefNo1").removeAttr("disabled");
            $("#lblPayableAt").text("Payable At");
           // $("input").prop('disabled', false);
        }
        else {
            $("#txtPayRefNo1").attr("disabled");
            $("#lblPayableAt").text("Account No");
            //$("input").prop('disabled', true);
        }
    });


    $("#ddlAttachmentType").change(function () {
        var AttachmentType = $("#ddlAttachmentType").val();
        if (AttachmentType != "0") {
            $("#ddlAttachmentType").removeClass("required");
            $("#ddlAttachmentType").addClass("valid");
        }
        else {
            $("#ddlAttachmentType").removeClass("valid");
            $("#ddlAttachmentType").addClass("required");
        }
    });

    $("#txtAttachDescription").keyup(function () {
        var Desc = $.trim($("#txtAttachDescription").val());
        if (Desc != "") {
            $("#txtAttachDescription").removeClass("required");
            $("#txtAttachDescription").addClass("valid");
        }
        else {
            $("#txtAttachDescription").removeClass("valid");
            $("#txtAttachDescription").addClass("required");
        }
    });

    $(".attUploader").on('change', function (e) {
        var Attach_list = Attachment_list();
        var Attachment_fomat = Attached_fileformat();  //Pandiaraj 18-11-2019 
        var iframe = $('<iframe name="postiframe" id="postiframe" style="display: none"></iframe>');
        $("body").append(iframe);
        var form = $('#uploaderForm');
        // form.attr("action", UrlDet + "UploadAttachment");
        form.attr("action", UrlDet + "UploadAttachment/?attach=" + Attach_list + '&attaching_format=' + Attachment_fomat);  //Pandiaraj 18-11-2019 
        form.attr("method", "post");
        form.attr("encoding", "multipart/form-data");
        form.attr("enctype", "multipart/form-data");
        form.attr("target", "postiframe");
        form.attr("file", $('#attUploader').val());
        form.submit();
        return false;
    });


    $("#drpaudit").change(function () {
        //alert('Changed');
        if ($('#drpaudit').val() == 1) {
            $("#showpaymentdialog").css("display", "block");
            $("#showExpensehistory").css("display", "none");
           

        }
        else if ($('#drpaudit').val() == 2) {
            $("#showpaymentdialog").css("display", "none");
            $("#showExpensehistory").css("display", "block");
        }
 

        else {
            $("#showpaymentdialog").css("display", "none");
            $("#showExpensehistory").css("display", "none");
        }

    });


});

function isEvent(evt) {
    return false;
}

function AddAttachment() {
    $('#ShowDialog3').attr("style", "display:block;");

    if ($("#ddlAttachmentType").hasClass("valid")) {
        $("#ddlAttachmentType").removeClass("valid");
        $("#ddlAttachmentType").addClass("required");
    }
    if ($("#txtAttachDescription").hasClass("valid")) {
        $("#txtAttachDescription").removeClass("valid");
        $("#txtAttachDescription").addClass("required");
    }

    objDialogyAddAttachment.dialog({ title: 'Attachment Details', width: '560', height: '300' });
    $("#attUploader").replaceWith($("#attUploader").val('').clone(true));
    $("#ddlAttachmentType").val("0");
    $("#txtAttachDescription").val("");
    objDialogyAddAttachment.dialog("open");
    return false;
}


