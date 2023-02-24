var objDialogyAdd, objDialogyAddDebit, objDialogyAddPayment, objDialogyAddAttachment, objDialogyInvoice, objDialogyPPX,
    objDialogyCreditNote, objDialogyWHTax, objDialogyAmort, objDialogyPoMapping, objDialogyAddPayment1, objDialogyBenificiary;
var currencyid;
var objDialogyGST;

UrlDet = UrlDet.replace("GetSupplierInvoiceMaker", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");


var SearchModel = function (id) {
    var self = this;

    self.GSTInvoiceTaxArray = ko.observableArray();
    self.RCMInvoiceTaxArray = ko.observableArray();
    self.GSTTaxTypeArray = ko.observableArray();
    self.GSTSubTaxTypeArray = ko.observableArray();
    self.GSTHSNCodeArray = ko.observableArray();
    self.GSTTotalTaxAmt = ko.observable(0.0);
    self.RCMTotalAmt = ko.observable(0.0);
    self.ECFHeaderArray = ko.observableArray();
    self.ECFInvoiceArray = ko.observableArray();
    self.ECFAttachmentArray = ko.observableArray();
    self.ECFAuditrailArray = ko.observableArray();
    self.ECFDeviationArray = ko.observableArray();

    self.InvoiceTaxArray = ko.observableArray();
    self.InvoiceDebitArray = ko.observableArray();
    self.InvoicePaymentArray = ko.observableArray();
    self.InvoicePOArray = ko.observableArray();
    self.AmortDebitArray = ko.observableArray();
    self.InvoiceAttachmentArray = ko.observableArray();
    self.InvoiceHoldingTaxArray = ko.observableArray();

    self.CurrencyArray = ko.observableArray();
    self.InvoiceTypeArray = ko.observableArray();
    self.TaxTypeArray = ko.observableArray();
    self.ExpenseArray = ko.observableArray();
    self.CategoryArray = ko.observableArray();
    self.SubCategoryArray = ko.observableArray();
    self.BusinessSegmentArray = ko.observableArray();
    self.CostCenterArray = ko.observableArray();
    self.PayModeArray = ko.observableArray();

    self.AttachmentTypeArray = ko.observableArray();
    self.WHTaxTypeArray = ko.observableArray();
    self.WHTaxSubTypeArray = ko.observableArray();
    self.AmortGlArray = ko.observableArray();
    self.AmortCycleArray = ko.observableArray();
    self.TaxRateArray = ko.observableArray();
    self.SubTaxTypeArray = ko.observableArray();

    self.BenificiaryArray = ko.observableArray();

    self.PPXArray = ko.observableArray();
    self.CreditNoteArray = ko.observableArray();
    self.SUSArray = ko.observableArray();
    self.ScheduleArray = ko.observableArray();
    self.POMappingArray = ko.observableArray();

    self.EOWInexArray = ko.observableArray();
    self.EOWDespatchArray = ko.observableArray();
    self.EOWPaymentArray = ko.observableArray();
    self.PaymentDespatchArray = ko.observableArray();

    self.PayBankArray = ko.observableArray();
    self.PayModeCommonArray = ko.observableArray();
    self.DefaultPayBank = ko.observable("0");

    //Hsn codedropdown loading array /kavitha 06-03-18
    self.HsnCodeArray = ko.observableArray();

    self.ProviderLocationArray = ko.observableArray();
    self.ReceiverLocationArray = ko.observableArray();

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

    self.ShortDescription = function (data) {
        if (data != null && data.length > 45) {
            data = data.substring(0, 45) + '...';
        } return data;
    };

    //START --Run Dedup
    self.DedupDetailsArray = ko.observableArray();

    this.RunDedup = function () {
        var _tmpData = {
            EcfId: $("#hfECFId").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "RunDedup",
            data: JSON.stringify(_tmpData),
            contentType: "application/json;",
            success: function (response) {
                self.DedupDetailsArray.removeAll();

                var Data1 = "", Data2 = "";
                if (response != null && response != "") {
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                        Data1 = JSON.parse(response.Data1);
                        $('#hfISDedup').val("1");
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
        var downloadUrl = UrlDet + "DownloadDedupData/" + EcfId;
        ko.utils.postJson(downloadUrl);
    };
    //END --Run Dedup

    this.LoadECFDetails = function () {
        debugger;
        var EcfId = $("#hdfEcfId").val();
        $("#hfECFId").val(EcfId);
        var EcfFilter = {
            EcfId: EcfId
        };

        $.ajax({
            type: "post",
            url: UrlDet + "GetSupplierInvoiceMaker",
            data: JSON.stringify(EcfFilter),
            contentType: "application/json;",
            success: function (response) {

                //alert('Successs');
                var Data1 = "", Data2 = "", Data3 = "", Data4 = "", Data5 = "", Data6 = "", Data7 = "", Data8 = "", Data9 = "", Data10 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    //if (Data1[0].Message != "")
                    // jAlert(Data1[0].Message, "Notification");
                }

                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                if (Data2.length > 0) {

                    $("#lblRaiserMode").text(Data2[0].RaiserMode);
                    $("#lblRaiserCode").text(Data2[0].RaiserId);
                    $("#lblRaiserName").text(Data2[0].RaiserName);
                    $("#lblRaiserGrade").text(Data2[0].RraiserGrade);


                    $("#txtECFDate").text(Data2[0].EcfDate);
                    $("#ddlInvoiceType").text(Data2[0].InvoiceTypeName);
                    $("#txtSupplierCode").text(Data2[0].SCode);
                    $("#hfSupplierId").val(Data2[0].SupplierId);
                    $("#txtSupplierName").text(Data2[0].SName);                    
                    //self.LoadCurrencyType();
                    debugger;
                    //$("#ddlCurrency selectedOptions:selected").val(Data2[0].CrnID);
                    //$("#ddlCurrency  selectedOptions:" + Data2[0].CrnID); //+ "]").attr("selected", "selected");
                    currencyid = Data2[0].CrnID;
                    $("#ddlCurrency").val(Data2[0].CrnID);
                    $("#txtExchangeRate").val(Data2[0].ExchangeRate);
                    $("#txtCurrencyAmount").val(Data2[0].CurrencyAmount);
                    $("#txtECFAmount").text(Data2[0].EcfAmount);
                    $("#txtECFNo").text(Data2[0].ecfno);
                    if (Data2[0].InvoiceTypeName == "PO" || Data2[0].InvoiceTypeName == "WO")
                        $("#tabPO").css("display", "");
                    else
                        $("#tabPO").css("display", "none");
                    $("#txtReducedAmount").val(Data2[0].ReducedAmt);
                    $("#txtProcessedAmount").val(Data2[0].ProcessedAmt);
                    $("#txtECFDescription").val(Data2[0].EDesc);
                    //$("input[name=rdoGAmort][value=" + Data2[0].AmortFlag + "]").prop("checked", true);
                    //var PayNet = Data2[0].PaymentNetting ? "1" : "0";
                    //$("input[name=rdoGPaymentNetting][value=" + PayNet + "]").prop("checked", true);
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
                    if (Data2[0].AmortFlag == "Y") {

                        $("#rdoAmortYes").prop("checked", true)
                    }
                    else {
                        // alert('no')
                        $("#rdoAmortNo").prop("checked", true)
                        //  $("input[name=rdoGPaymentNetting][value=" + Data2[0].PaymentNetting + "]").prop("checked", false);
                    }

                    $("#hfISPO").val(Data2[0].InvoiceTypeName);
                    if ($("#hfISPO").val() == "PO") {
                        $("#divForNonPO").css("display", "none");
                        $(".divForNonPO1").css("display", "none");
                    }

                    //$("#txtAmortFrom").val(Data2[0].AmortFrom);
                    //$("#txtAmortTo").val(Data2[0].AmortTo);
                    //$("#txtAmortDescription").val(Data2[0].AmortDesc);
                    //if ($.trim(Data2[0].AmortFrom).length > 0)
                    //    $("#txtAmortFrom").addClass("valid");
                    //if ($.trim(Data2[0].AmortTo).length > 0)
                    //    $("#txtAmortTo").addClass("valid");
                    //if ($.trim(Data2[0].AmortDesc).length > 0)
                    //    $("#txtAmortDescription").addClass("valid");

                    //if ($("input[name=rdoGAmort]:radio[checked=checked]").attr("value") == "Y")
                    //    $(".trAmort").css("display", "");
                    //else
                    //    $(".trAmort").css("display", "none");

                    $("#hfBU").val(Data2[0].fc);
                    $("#hfCC").val(Data2[0].cc);
                    $("#hfEmpProductCode").val(Data2[0].productcode);
                    $("#hfEmpOUCode").val(Data2[0].oucode);
                    $("#hfProductCodeText").val(Data2[0].Product);
                    $("#hfOUCodeText").val(Data2[0].location);

                    $("#txtSPANNo").val(Data2[0].PANNo);
                    $("#txtSNSDL").val(Data2[0].NSDL);
                    $("#txtSupplierAddress").val(Data2[0].SAddress);
                    $("#txtSTINNo").val(Data2[0].TINNo);

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
                self.ECFHeaderArray(Data2);

                if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null)
                    Data3 = JSON.parse(response.Data3);
                self.ECFInvoiceArray(Data3);

                if (response.Data4 != null && response.Data4 != "" && JSON.parse(response.Data4) != null)
                    Data4 = JSON.parse(response.Data4);
                self.ECFAttachmentArray(Data4);

                if (response.Data5 != null && response.Data5 != "" && JSON.parse(response.Data5) != null)
                    Data5 = JSON.parse(response.Data5);
                self.ECFAuditrailArray(Data5);

                if (response.Data6 != null && response.Data6 != "" && JSON.parse(response.Data6) != null)
                    Data6 = JSON.parse(response.Data6);
                self.ECFDeviationArray(Data6);

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

                self.LoadGSTINCategory();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
                // alert('Fail');
            }
        });
    };

    this.SetECFDetails = function () {
        var EcfId = $("#hfECFId").val();
        var ReducedAmt = $("#txtReducedAmount").val();
        var ProcessedAmt = $("#txtProcessedAmount").val();
        var PaymentNett = $("input[name=rdoGPaymentNetting]:radio[checked=checked]").attr("value");
        var Amort = $("input[name=rdoGAmortECF]:radio[checked=checked]").attr("value");
        //var AmortFrom = $("#txtAmortFrom").val();
        //var AmortTo = $("#txtAmortTo").val();
        //var AmortDesc = $("#txtAmortDescription").val();
        debugger;
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

        //if ($.trim(AmortFrom) == "" && Amort == 'Y') {
        //    jAlert("Ensure Amort From!", "Message");
        //    return false;
        //}
        //if ($.trim(AmortTo) == "" && Amort == 'Y') {
        //    jAlert("Ensure Amort To!", "Message");
        //    return false;
        //}
        //if ($.trim(AmortDesc) == "" && Amort == 'Y') {
        //    jAlert("Ensure Amort Description!", "Message");
        //    return false;
        //}

        var ECFHeader = {
            EcfId: EcfId,
            ReducedAmt: ReducedAmt,
            ProcessedAmt: ProcessedAmt,
            PaymentNett: PaymentNett,
            Amort: Amort,
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
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == false)
                        jAlert(Data1[0].Msg, "Message");
                    else
                        jAlert(Data1[0].Msg, "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.SetInvoiceHeaderDetails = function () {
        var GstCharged = "N", ProviderLocation = "0", GstinVendor = "", ReceiverLocation = "0", GstinFiccl = "";

                GstCharged = $("input[name=rdogstChargedFlag]:radio[checked=checked]").attr("value");
                $("#hdfEditViewGstChargedFlag").val(GstCharged);
                    ProviderLocation = $("#ddlgstProviderLoc").val();
                    GstinVendor = $("#txtGSTINVendor").val();
                    ReceiverLocation = $("#ddlgstReceiverLoc").val();
                    GstinFiccl = $("#txtGSTINFICCL").val();
         
                var rententionRate = "0", rententionAmount = "0", ReleaseOn = "";
                var EcfId = $("#hfECFId").val();
                var InvId = $("#hfInvId").val();
                var InvDate = $("#txtInvoiceDate").val();
                var ServiceMonth = $("#txtServiceMonth").val();
                var InvNo = $("#txtInvoiceNo").val();
                var Desc = $("#txtInvoiceDescription").val();
                var Amount = $("#txtInvoiceAmount").val();
                var WOTaxAmount = $("#txtWithoutTaxAmount").val();
                var ProvisionFlag = $("input[name=rdoProvisionFlag]:radio[checked=checked]").attr("value");
                var RetentionFlag = $("input[name=rdoRetentionFlag]:radio[checked=checked]").attr("value");
                var Amort = $("input[name=rdoGAmort]:radio[checked=checked]").attr("value");
                var IsVerify = $("input[name=rdoVerify]:radio[checked=checked]").attr("value");
                if (RetentionFlag == "Y") {
                    rententionRate = $("#txtRetentionPercentage").val();
                    rententionAmount = $("#txtRetentionAmount").val();
                    ReleaseOn = $("#txtReleaseDate").val();
                }
                var IsRemoved = "0";

        var month = new Date(Date.parse(ServiceMonth.split(' ')[0] + " 1, " + ServiceMonth.split(' ')[1])).getMonth() + 1;
        ServiceMonth = ("1/" + month + "/" + ServiceMonth.split(' ')[1]);

        if ($.trim(InvDate) == "") {
            jAlert("Ensure Invoice Date!", "Message");
            return false;
        }

        if ($.trim(InvNo) == "") {
            jAlert("Ensure Invoice No!", "Message");
            return false;
        }

        if ($.trim(Desc) == "") {
            jAlert("Ensure Invoice Description!", "Message");
            return false;
        }

        if ($.trim(Amount) == "" || parseFloat(Amount) == 0) {
            jAlert("Ensure Invoice Amount!", "Message");
            return false;
        }

        if ($.trim(WOTaxAmount) == "") {
            jAlert("Ensure Without Tax Amount!", "Message");
            return false;
        }

        if (parseFloat(WOTaxAmount) > parseFloat(Amount)) {
            jAlert("Without Tax Amount should be less than or equal to invoice Amount!", "Message");
            return false;
        }

        if ((parseFloat(rententionRate) == 0 || $.trim(rententionRate) == "") && RetentionFlag == "Y") {
            jAlert("Ensure Retention Percentage!", "Message");
            return false;
        }
        if ((parseFloat(rententionAmount) == 0 || $.trim(rententionAmount) == "") && RetentionFlag == "Y") {
            jAlert("Ensure Retention Amount!", "Message");
            return false;
        }

        if ($.trim(ReleaseOn) == "" && RetentionFlag == "Y") {
            jAlert("Ensure Release Date!", "Message");
            return false;
        }

       // if ((parseInt(ProviderLocation) == 0 || $.trim(ProviderLocation) == "") && GstCharged == "Y") {
            if ((parseInt(ProviderLocation) == 0 || $.trim(ProviderLocation) == "")) {
            jAlert("Ensure Provider Location!", "Message");
            return false;
        }

        // if ((parseInt(ReceiverLocation) == 0 || $.trim(ReceiverLocation) == "") && GstCharged == "Y") {
            if ((parseInt(ReceiverLocation) == 0 || $.trim(ReceiverLocation) == "")) {
            jAlert("Ensure Receiver Location!", "Message");
            return false;
        }

        if ($.trim(GstinVendor) == "" && GstCharged == "Y") {
            jAlert("Ensure GSTIN Vendor!", "Message");
            return false;
        }

        if ($.trim(GstinFiccl) == "" && GstCharged == "Y") {
            jAlert("Ensure GSTIN FICCL!", "Message");
            return false;
        }


        var InvoiceHeader = {
            EcfId: EcfId,
            InvId: InvId,
            InvDate: InvDate,
            InvNo: InvNo,
            Desc: Desc,
            Amount: Amount,
            WOTaxAmount: WOTaxAmount,
            ServiceMonth: ServiceMonth,
            ProvisionFlag: ProvisionFlag,
            RetentionFlag: RetentionFlag,
            Amort: Amort,
            rententionRate: rententionRate,
            rententionAmount: rententionAmount,
            ReleaseOn: ReleaseOn,
            IsVerify: IsVerify,
            IsRemoved: IsRemoved,
            GstCharged: GstCharged,
            ProviderLocation: ProviderLocation,
            GstinVendor: GstinVendor,
            ReceiverLocation: ReceiverLocation,
            GstinFiccl: GstinFiccl,
            Suppliergid: "0"
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetInvoiceHeaderDetails",
            data: JSON.stringify(InvoiceHeader),
            contentType: "application/json;",
            success: function (response) {
                        var Data1 = "", Data2 = "", Data3 = "", Data4 = "", Data5 = "",Data6="";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == false)
                        jAlert(Data1[0].Msg, "Message");
                    else {
                        $('#hfISDedup').val("0");
                        jAlert(Data1[0].Msg, "Message");
                        $("#hfInvId").val(Data1[0].InvId);
                        if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                            Data2 = JSON.parse(response.Data2);
                        self.ECFInvoiceArray(Data2);

                        if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null)
                            Data3 = JSON.parse(response.Data3);
                        self.InvoicePaymentArray(Data3);


                        if (response.Data4 != null && response.Data4 != "" && JSON.parse(response.Data4) != null) {
                            Data4 = JSON.parse(response.Data4);
                            self.GSTInvoiceTaxArray(Data4);
                        }
                                self.GSTTotalTaxAmt();
                           
                                if (response.Data5 != null && response.Data5 != "" && JSON.parse(response.Data5) != null)
                                    Data5 = JSON.parse(response.Data5);
                                self.InvoiceDebitArray(Data5);

                                //RCM
                                if (response.Data6 != null && response.Data6 != "" && JSON.parse(response.Data6) != null)
                                {
                                    Data6 = JSON.parse(response.Data6);
                                    self.RCMInvoiceTaxArray(Data6);
                                    self.RCMTotalTaxAmt();
                                }
                                
                                
                                if ($("input[name=rdogstChargedFlag]:radio[checked=checked]").attr("value") == "Y") {     
                                    $('#Invoicetabs').tabs('select', 1);
                                    $("#tabGST").show();
                                    $("#tabRCM").hide();
                                }
                                else {  
                                    $("#tabGST").hide();
                                    $("#tabRCM").show();
                                    $('#Invoicetabs').tabs('select', 0);
                                }
                            }

                }

                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        //alert(errorThrown);
                    }
                });
        //    }
        //    else
        //        return false;
        //});
    }

    this.DeleteInvoiceHeaderDetails = function (InvId) {

        jConfirm("Are you sure? Want to delete Invoice!", "Confirm", function (callback) {
            if (callback == true) {
                var EcfId = $("#hfECFId").val();
                var InvDate = "";
                var InvNo = "";
                var Desc = "";
                var ProvisionFlag = "";
                var RetentionFlag = "";
                var rententionRate = "0";
                var rententionAmount = "0"
                var ReleaseOn = ""
                var IsRemoved = "1";
                var WOTaxAmount = "0";

                var InvoiceHeader = {
                    EcfId: EcfId,
                    InvId: InvId,
                    InvDate: InvDate,
                    InvNo: InvNo,
                    Desc: Desc,
                    Amount: "0",
                    WOTaxAmount: WOTaxAmount,
                    ProvisionFlag: ProvisionFlag,
                    RetentionFlag: RetentionFlag,
                    rententionRate: rententionRate,
                    rententionAmount: rententionAmount,
                    ReleaseOn: ReleaseOn,
                    IsRemoved: IsRemoved,
                    GstCharged: "N",
                    ProviderLocation: "0",
                    GstinVendor: "",
                    ReceiverLocation: "0",
                    GstinFiccl: "",
                    Suppliergid: "0"
                };
                $.ajax({
                    type: "post",
                    url: UrlDet + "SetInvoiceHeaderDetails",
                    data: JSON.stringify(InvoiceHeader),
                    contentType: "application/json;",
                    success: function (response) {
                        var Data1 = "", Data2 = "";
                        if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                            Data1 = JSON.parse(response.Data1);
                            if (Data1[0].Clear == false)
                                jAlert(Data1[0].Msg, "Message");
                            else
                                jAlert(Data1[0].Msg, "Message");
                            $('#hfISDedup').val("0");
                        }
                        if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                            Data2 = JSON.parse(response.Data2);
                        self.ECFInvoiceArray(Data2);
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

    this.SubmitInvoiceDetails = function () {

        var InvId = $("#hfInvId").val();

        var InputFilter = {
            InvId: InvId
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SubmitInvoice",
            data: JSON.stringify(InputFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                //
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == false) {
                        objDialogyInvoice.dialog("close");
                        jAlert("Invoice Submitted Sucessfully", "Message");
                    }
                    else {
                        jAlert(Data1[0].Msg, "Message");
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.AddInvoiceDetails = function () {

        $('#divInvoice').attr("style", "display:block;");
        objDialogyInvoice.dialog({
            title: 'Add Invoice Details', width: '1100', height: '600', close: function (event, ui) {
                //objDialogyInvoice.find("form").remove();
                objDialogyInvoice.dialog('destroy');


            }
        });
        objDialogyInvoice.dialog("open");

        $("#hfInvId").val("0");

        $("#txtInvoiceDate").val("");
        $("#txtInvoiceNo").val("");
        $("#txtServiceMonth").val("");
        $("#txtInvoiceAmount").val("");
        $("#txtWithoutTaxAmount").val("");
        $("#txtInvoiceDescription").val("");
        $("#txtRetentionPercentage").val("");
        $("#txtRetentionAmount").val("");
        $("#txtReleaseDate").val("");

        $("input[name=rdoRetentionFlag][value=N]").prop("checked", true);
        $("input[name=rdoProvisionFlag][value=N]").prop("checked", true);
        $("input[name=rdoGAmort][value=N").prop("checked", true);
        $("input[name=rdoVerify][value=1]").prop("checked", true);

        $(".divRetention").slideUp();
        $(".trAmort").css("display", "none");
        $("#btnAmort").html("<span style=' color:white; font-size:12px; margin-right: 5px;' class='glyphicon glyphicon-plus'></span>Amort");

        self.InvoiceTaxArray("");
        self.GSTInvoiceTaxArray("");
        self.RCMInvoiceTaxArray("");
        self.InvoiceDebitArray("");
        self.InvoicePaymentArray("");
        self.InvoicePOArray("");
        self.InvoicePOArray("");
        self.InvoiceHoldingTaxArray("");
        self.InvoiceAttachmentArray("");
        $("#tabTax").hide();
        if ($("#hfISPO").val() == "PO") {
            $("#divForNonPO").css("display", "none");
            $("#tabTax").show();
        }
        $("#tabPO").css("display", "none");

        $("#txtInvoiceDate").removeClass("valid").addClass("required");
        $("#txtInvoiceNo").removeClass("valid").addClass("required");
        $("#txtServiceMonth").removeClass("valid").addClass("required");
        $("#txtInvoiceAmount").removeClass("valid").addClass("required");
        $("#txtWithoutTaxAmount").removeClass("valid").addClass("required");
        $("#txtInvoiceDescription").removeClass("valid").addClass("required");

        $("input[name=rdogstChargedFlag][value=N]").prop("checked", true);
        $("#ddlgstProviderLoc").val("0");
        $("#ddlgstReceiverLoc").val("0");
        $("#txtGSTINVendor").val("");
        $("#txtGSTINFICCL").val("");

        $("#ddlgstProviderLoc,#ddlgstReceiverLoc").removeClass("valid").removeClass("required");
        $("#ddlgstProviderLoc,#ddlgstReceiverLoc").addClass("required");
         
        $("#tabGST").hide();
        $('#Invoicetabs').tabs('select', 0);
        self.FindTotalTax();
        self.FindRCMTotal();
        //ByGOPI
        $(".viewForIvoice").css("display", "");
        return false;
    };

    this.SetInvoiceTaxDetails = function () {
        var InvoiceTaxgid = $("#hfInvoiceTaxgid").val();
        var InvId = $("#hfInvId").val();
        var SupplierId = $("#hfSupplierId").val();
        var TaxId = $("#ddlTaxType").val();
        var SubTaxId = $("#ddlSubTaxType").val();
        var TaxRate = $("#txtTaxRate").val();
        var TaxableAmt = $.trim($("#txtTaxableAmount").val()) == "" ? 0 : parseFloat($("#txtTaxableAmount").val());
        var TaxAmount = $.trim($("#txtTaxAmount").val()) == "" ? 0 : parseFloat($("#txtTaxAmount").val());
        var IsRemoved = "0";
        var INVAmount = $.trim($("#txtInvoiceAmount").val()) == "" ? 0 : parseFloat($("#txtInvoiceAmount").val());
        var Tax = (TaxRate * TaxableAmt) / 100;

        if (TaxId == "0") {
            jAlert("Ensure Tax Type!", "Message");
            return false;
        }

        if (SubTaxId == "0") {
            jAlert("Ensure Tax SubType!", "Message");
            return false;
        }

        if ($.trim(TaxRate) == "" || parseFloat(TaxRate) == 0) {
            jAlert("Ensure Tax Rate!", "Message");
            return false;
        }

        if ($.trim(TaxableAmt) == "" || parseFloat(TaxableAmt) == 0) {
            jAlert("Ensure Taxable Amount!", "Message");
            return false;
        }

        if (TaxAmount == 0) {
            jAlert("Ensure Tax Amount!", "Message");
            return false;
        }

        if (TaxAmount > (Tax + 5) || TaxAmount < (Tax - 5)) {
            jAlert("You can only plus or minus 5 into Tax Amount!", "Message");
            return false;
        }

        if (INVAmount < (TaxableAmt + TaxAmount) && parseFloat(TaxRate) != 100) {
            jAlert("Taxable Amount + Tax Amount should not exceeded Invoice Amount!", "Message");
            return false;
        }

        var InvoiceTax = {
            InvoiceTaxgid: InvoiceTaxgid,
            InvId: InvId,
            SupplierId: SupplierId,
            TaxId: TaxId,
            SubTaxId: SubTaxId,
            TaxRate: TaxRate,
            TaxableAmt: TaxableAmt,
            TaxAmount: TaxAmount,
            IsRemoved: IsRemoved
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetInvoiceTaxDetails",
            data: JSON.stringify(InvoiceTax),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "", Data3 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == false)
                        jAlert(Data1[0].Msg, "Message");
                    else {
                        objDialogyAdd.dialog("close");
                        jAlert(Data1[0].Msg, "Message");
                    }
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                self.InvoiceTaxArray(Data2);

                if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null)
                    Data3 = JSON.parse(response.Data3);
                self.InvoiceDebitArray(Data3);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.DeleteInvoiceTaxDetails = function (InvoiceTaxgid) {

        jConfirm("Are you sure? Want to delete Invoice Tax!", "Confirm", function (callback) {
            if (callback == true) {
                var InvId = $("#hfInvId").val();
                var SupplierId = "0";
                var TaxId = "0";
                var TaxRate = "0";
                var TaxableAmt = "0";
                var TaxAmount = "0";
                var IsRemoved = "1";
                var InvoiceTax = {
                    InvoiceTaxgid: InvoiceTaxgid,
                    InvId: InvId,
                    SupplierId: SupplierId,
                    TaxId: TaxId,
                    TaxRate: TaxRate,
                    TaxableAmt: TaxableAmt,
                    TaxAmount: TaxAmount,
                    IsRemoved: IsRemoved
                };
                $.ajax({
                    type: "post",
                    url: UrlDet + "SetInvoiceTaxDetails",
                    data: JSON.stringify(InvoiceTax),
                    contentType: "application/json;",
                    success: function (response) {
                        var Data1 = "", Data2 = "", Data3 = "";
                        if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                            Data1 = JSON.parse(response.Data1);
                            if (Data1[0].Clear == false)
                                jAlert(Data1[0].Msg, "Message");
                            else {
                                objDialogyAdd.dialog("close");
                                jAlert(Data1[0].Msg, "Message");
                            }
                        }
                        if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                            Data2 = JSON.parse(response.Data2);
                        self.InvoiceTaxArray(Data2);

                        if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null)
                            Data3 = JSON.parse(response.Data3);
                        self.InvoiceDebitArray(Data3);
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

    this.SetInvoiceDebitDetails = function () {

        var DebitlineGId = $("#hfDebitlineGId").val();
        var ECFId = $("#hfECFId").val();
        var invid = $("#hfInvId").val();
        var expnaturegid = $("#ddlExpenses").val();
        var expCatId = $("#ddlCategory").val();
        var expSubcatId = $("#ddlSubCategory").val();
        var CCCode = $("#ddlCC").val();;
        var FCCode = $("#ddlBU").val();;
        var OUCode = $("#hfOUCode").val();;
        var ProductCode = $("#hfProductCode").val();;
        var Amount = $("#txtDebitAmount").val();
        var Desc = $("#txtDebitDescription").val();
        var IsRemoved = "0";
        //kavitha adding hsn gid into debitline table
        var hsngid = $("#ddlhsngid").val();
 
        var rcm = $("input[name=rdorcmChargedFlag]:radio[checked=checked]").attr("value");
        if (expnaturegid == "0" && $("#hfISPO").val() != "PO") {
            jAlert("Ensure Expense Nature!", "Message");
            return false;
        }

        if (expCatId == "0") {
            jAlert("Ensure Expense Category!", "Message");
            return false;
        }

        if (expSubcatId == "0") {
            jAlert("Ensure Expense SubCategory!", "Message");
            return false;
        }
        if (OUCode == "0") {
            jAlert("Ensure Location Code(OUCode)!", "Message");
            return false;
        }

        if (ProductCode == "0") {
            jAlert("Ensure Ensure Product Code!", "Message");
            return false;
        }
        //if ($("input[name=rdogstChargedFlag]:radio[checked=checked]").attr("value") == "Y") {
            if (hsngid == "0") {
                jAlert("Ensure hsn details", "Error");
                return false;
            }
        //}

        var InvoiceDebitLine = {
            DebitlineGId: DebitlineGId,
            ECFId: ECFId,
            invid: invid,
            expnaturegid: expnaturegid,
            expCatId: expCatId,
            expSubcatId: expSubcatId,
            Desc: Desc,
            CCCode: CCCode,
            FCCode: FCCode,
            OUCode: OUCode,
            ProductCode: ProductCode,
            Amount: Amount,
            IsRemoved: IsRemoved,
            hsngid: hsngid,
            rcm:rcm
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetDebitLineDetails",
            data: JSON.stringify(InvoiceDebitLine),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "", Data3 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == false)
                        jAlert(Data1[0].Msg, "Message");
                    else {
                        objDialogyAddDebit.dialog("close");
                        jAlert(Data1[0].Msg, "Message");
                    }
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                self.InvoiceDebitArray(Data2);
                                
                if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null) {
                    Data3 = JSON.parse(response.Data3);
                    self.GSTInvoiceTaxArray(Data3);
                }
                self.GSTTotalTaxAmt();

                if (response.Data4 != null && response.Data4 != "" && JSON.parse(response.Data4) != null) {
                    Data4 = JSON.parse(response.Data4);
                    self.RCMInvoiceTaxArray(Data4);
                }
                self.RCMTotalAmt();

                //if (rcm == "N") {
                //    $("#tabRCM").hide();
                //} else {
                //    $("#tabRCM").show();
                //}
                if ($("input[name=rdogstChargedFlag]:radio[checked=checked]").attr("value") == "Y") {
                    $('#Invoicetabs').tabs('select', 1);
                    $("#tabGST").show();
                    $("#tabRCM").hide();
                }
                else {
                    $("#tabGST").hide();
                    $("#tabRCM").show();
                    $('#Invoicetabs').tabs('select', 0);
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.DeleteInvoiceDebitDetails = function (DebitlineGId) {

        jConfirm("Are you sure? Want to delete Debit Line!", "Confirm", function (callback) {
            if (callback == true) {
                var ECFId = $("#hfECFId").val();
                var invid = $("#hfInvId").val();
                var expnaturegid = "0";
                var expCatId = "0";
                var expSubcatId = "0";
                var CCCode = "0";
                var FCCode = "0";
                var OUCode = "0";
                var ProductCode = "0";
                var Amount = "0";
                var Desc = "";
                var IsRemoved = "1";

                var InvoiceDebitLine = {
                    DebitlineGId: DebitlineGId,
                    ECFId: ECFId,
                    invid: invid,
                    expnaturegid: expnaturegid,
                    expCatId: expCatId,
                    expSubcatId: expSubcatId,
                    Desc: Desc,
                    CCCode: CCCode,
                    FCCode: FCCode,
                    OUCode: OUCode,
                    ProductCode: ProductCode,
                    Amount: Amount,
                    IsRemoved: IsRemoved
                };
                $.ajax({
                    type: "post",
                    url: UrlDet + "SetDebitLineDetails",
                    data: JSON.stringify(InvoiceDebitLine),
                    contentType: "application/json;",
                    success: function (response) {
                        var Data1 = "", Data2 = "", Data3 = "", Data4 = "";
                        if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                            Data1 = JSON.parse(response.Data1);
                            if (Data1[0].Clear == false)
                                jAlert(Data1[0].Msg, "Message");
                            else {
                                objDialogyAddDebit.dialog("close");
                                jAlert(Data1[0].Msg, "Message");
                            }
                        }
                        if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                            Data2 = JSON.parse(response.Data2);
                        self.InvoiceDebitArray(Data2);
                        if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null)
                            Data3 = JSON.parse(response.Data3);

                        self.GSTInvoiceTaxArray(Data3);
                        self.FindTotalTax();

                        //RCM
                        if (response.Data4 != null && response.Data4 != "" && JSON.parse(response.Data4) != null)
                            Data4 = JSON.parse(response.Data4);

                        self.RCMInvoiceTaxArray(Data4);
                        self.RCMTotalAmt()
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

    this.SetInvoiceCreditDetails = function (flag) {
        var paymode = "", RefNo = "", Beneficiary = "", Amount = "", Desc = "";
        var PayBank = 0;
        var CreditlineGId = $("#hfCreditlineGId").val();
        var Ecfid = $("#hfECFId").val();
        var InvId = $("#hfInvId").val();
        var RefId = $("#hfRefId").val();
        var IfscCode = $("#hfIFSCCode").val();

        if (flag == "1") {
            paymode = $("#ddlPayMode1 option:selected").text();
            PayBank = $("#ddlPayBank").val();
            RefNo = $("#txtPayRefNo1").val();
            Beneficiary = $("#txtPayBeneficiary1").val();
            //  alert(Beneficiary);
            Amount = $("#txtPaymentAmount1").val();
            Desc = $("#txtPayDescription1").val();
        }
        else {
            paymode = $("#ddlPayMode option:selected").text();
            PayBank = "0";
            RefNo = $("#txtPayRefNo").val();
            Beneficiary = $("#txtPayBeneficiary").val();
            //     alert(Beneficiary);
            Amount = $("#txtPaymentAmount").val();
            Desc = $("#txtPayDescription").val();
        }
        var IsRemoved = "0";

        if (paymode == "0") {
            jAlert("Ensure Payment Mode!", "Message");
            return false;
        }
        if (paymode == "EFT" || paymode == "RRP" || paymode == "ERA") {
            if ($.trim(RefNo) == "") {
                // alert(RefNo);
                jAlert("Ensure Account No!", "Message");
                return false;
            }
        }

        if (paymode == "DD") {
            if ($.trim(RefNo) == "") {
                // alert(RefNo);
                jAlert("Ensure Payable At!", "Message");
                return false;
            }
        }

        if (paymode == "-- Select One --") {
            jAlert("Ensure Payment Mode!", "Message");
            return false;



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
        if (PayBank == "0" && flag == "1") {
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
            url: UrlDet + "SetCreditLineDetails",
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
                        jAlert(Data1[0].Msg, "Message");
                    }
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                self.InvoicePaymentArray(Data2);

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    //edit by anand
    this.DeleteInvoiceCreditDetails = function (Index) {
        var _tmpData = self.InvoicePaymentArray()[Index];
        jConfirm("Are you sure? Want to delete Payment!", "Confirm", function (callback) {
            if (callback == true) {
                var Ecfid = $("#hfECFId").val();
                var InvId = _tmpData.InvId;
                var RefId = _tmpData.InvId;
                var paymode = _tmpData.PMode;
                var Beneficiary = _tmpData.Beneficiary;
                var Amount = _tmpData.Amt;
                var Desc = _tmpData.Desc;
                var IsRemoved = "1";
                var PayBank = 0;
                var IfscCode = "";
                var RefNo = _tmpData.RefNo;

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
                //console.log(InvoiceCreditLine)
                $.ajax({
                    type: "post",
                    url: UrlDet + "SetCreditLineDetails",
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
                                jAlert(Data1[0].Msg, "Message");
                            }
                        }
                        if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                            Data2 = JSON.parse(response.Data2);
                        self.InvoicePaymentArray(Data2);
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

    this.SetWithHoldTaxDetails = function () {

        var withholdtaxgid = $("#hfWHTaxgid").val();
        var Invoicegid = $("#hfInvId").val();
        var TaxId = $("#ddlWHTaxType").val();
        var taxsubtypegid = $("#ddlWHTaxSubType").val();
        var TaxRate = $("#txtWHTaxRate").val();
        var TaxableAmt = $("#txtWHTaxableAmount").val();
        var TaxAmount = $("#txtWHTaxAmount").val();
        var NetAmount = $.trim($("#txtWHNetAmount").val()) == "" ? 0 : parseFloat($("#txtWHNetAmount").val());
        var IsRemoved = "0";

        if (TaxId == "0") {
            jAlert("Ensure Tax Type!", "Message");
            return false;
        }
        if (taxsubtypegid == "0") {
            jAlert("Ensure Tax Subtype!", "Message");
            return false;
        }
        if ($.trim(TaxRate) == "" || parseFloat(TaxRate) == 0) {
            jAlert("Ensure Tax Rate!", "Message");
            return false;
        }
        if ($.trim(TaxableAmt) == "" || parseFloat(TaxableAmt) == 0) {
            jAlert("Ensure Taxable Amount!", "Message");
            return false;
        }

        var WHTax = {
            withholdtaxgid: withholdtaxgid,
            Invoicegid: Invoicegid,
            TaxId: TaxId,
            taxsubtypegid: taxsubtypegid,
            TaxRate: TaxRate,
            TaxableAmt: TaxableAmt,
            TaxAmount: TaxAmount,
            NetAmount: NetAmount,
            IsRemoved: IsRemoved
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetWithHoldingTaxDetails",
            data: JSON.stringify(WHTax),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "", Data3 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == false)
                        jAlert(Data1[0].Msg, "Message");
                    else {
                        objDialogyWHTax.dialog("close");
                        jAlert(Data1[0].Msg, "Message");
                    }
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                self.InvoiceHoldingTaxArray(Data2);
                if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null)
                    Data3 = JSON.parse(response.Data3);
                self.InvoicePaymentArray(Data3);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.DeleteWithHoldTaxDetails = function (withholdtaxgid) {

        jConfirm("Are you sure? Want to delete With Holding Tax!", "Confirm", function (callback) {
            // 

            if (callback == true) {

                //  alert();
                //  
                var Invoicegid = $("#hfInvId").val();
                var TaxId = "0";
                var taxsubtypegid = "0";
                var TaxRate = "0";
                var TaxableAmt = "0";
                var TaxAmount = "0";
                var NetAmount = "0";
                var IsRemoved = "1";

                var WHTax = {
                    withholdtaxgid: withholdtaxgid,
                    Invoicegid: Invoicegid,
                    TaxId: TaxId,
                    taxsubtypegid: taxsubtypegid,
                    TaxRate: TaxRate,
                    TaxableAmt: TaxableAmt,
                    TaxAmount: TaxAmount,
                    NetAmount: NetAmount,
                    IsRemoved: IsRemoved
                };

                $.ajax({
                    type: "post",
                    url: UrlDet + "SetWithHoldingTaxDetails",
                    data: JSON.stringify(WHTax),
                    contentType: "application/json;",
                    success: function (response) {
                        var Data1 = "", Data2 = "";
                        if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                            Data1 = JSON.parse(response.Data1);
                            if (Data1[0].Clear == false)
                                jAlert(Data1[0].Msg, "Message");
                            else {
                                objDialogyAddPayment.dialog("close");
                                jAlert(Data1[0].Msg, "Message");
                            }
                        }
                        if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                            Data2 = JSON.parse(response.Data2);
                        self.InvoiceHoldingTaxArray(Data2);
                        if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null)
                            Data3 = JSON.parse(response.Data3);
                        self.InvoicePaymentArray(Data3);
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

    this.SetAttachmentDetails = function () {

        var flag = $("#hfAttachFlag").val();

        var EcfId = $("#hfECFId").val();
        var Invoicegid = $("#hfInvId").val();
        var AttachmentId = $("#hfAttachmetId").val();
        var AttachmentName = "";
        var AttachmentType = $("#ddlAttachmentType").val();
        var Desc = $("#txtAttachDescription").val();
        var IsRemoved = "0";
        if (flag == "0")
            Invoicegid = "0";
        var Attachment = {
            EcfId: EcfId,
            Invoicegid: Invoicegid,
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
                var Data1 = "", Data2 = "", Data3 = "";
                // 
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == false)
                        jAlert(Data1[0].Msg, "Message");
                    else {
                        objDialogyAddAttachment.dialog("close");
                        jAlert(Data1[0].Msg, "Message");
                    }
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null)
                    Data3 = JSON.parse(response.Data3);
                if (Data1[0].Clear == true) {
                    self.ECFAttachmentArray(Data2);
                    self.InvoiceAttachmentArray(Data3);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.DeleteAttachmentDetails = function (AttachmentId) {

        jConfirm("Are you sure? Want to delete Attachment!", "Confirm", function (callback) {
            if (callback == true) {
                var EcfId = $("#hfECFId").val();
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
                        // 
                        if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                            Data1 = JSON.parse(response.Data1);
                            if (Data1[0].Clear == false)
                                jAlert(Data1[0].Msg, "Message");
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
	var FileName = FileName.replace("&","-");
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

    this.CloseDetails = function (flag) {
        if (flag == 0)
            objDialogyAdd.dialog("close");
        if (flag == 1)
            objDialogyAddDebit.dialog("close");
        if (flag == 2)
            objDialogyInvoice.dialog("close");
        if (flag == 3)
            objDialogyAddPayment.dialog("close");
        if (flag == 4)
            objDialogyAddAttachment.dialog("close");
        if (flag == 5)
            objDialogyPPX.dialog("close");
        if (flag == 6)
            objDialogyCreditNote.dialog("close");
        if (flag == 7)
            objDialogyWHTax.dialog("close");
        if (flag == 8)
            objDialogyAmort.dialog("close");
        if (flag == 9)
            objDialogyPoMapping.dialog("close");
        if (flag == 10)
            objDialogySUS.dialog("close");
        if (flag == 11)
            objDialogyAddPayment1.dialog("close");
        if (flag == 12)
            objDialogyBenificiary.dialog("close");
        if (flag == 13)
            objDialogyGST.dialog("close");

    };

    this.EditInvoiceTaxDetails = function (invoicetaxgid, flag) {
        // 
        $("#hfInvoiceTaxgid").val(invoicetaxgid);
        $("#ddlTaxType").val($("#hfTaxId" + invoicetaxgid).val());
        self.LoadTaxSubType();
        $("#ddlSubTaxType").val($("#hfSubTaxId" + invoicetaxgid).val());
        $("#txtTaxRate").val($("#lblRate" + invoicetaxgid).text());
        $("#txtTaxableAmount").val($("#lblTaxableAmt" + invoicetaxgid).text());
        $("#txtTaxAmount").val($("#lblTaxAmt" + invoicetaxgid).text());

        if ($("#ddlTaxType").val() == "0") {
            $("#ddlTaxType").removeClass("valid");
            $("#ddlTaxType").addClass("required");
        }
        else {
            $("#ddlTaxType").addClass("valid");
            $("#ddlTaxType").removeClass("required");
        }
        if ($("#ddlSubTaxType").val() == "0") {
            $("#ddlSubTaxType").removeClass("valid");
            $("#ddlSubTaxType").addClass("required");
        }
        else {
            $("#ddlSubTaxType").addClass("valid");
            $("#ddlSubTaxType").removeClass("required");
        }
        if ($.trim($("#txtTaxRate").val()) == "" || parseFloat($("#txtTaxRate").val()) == 0) {
            $("#txtTaxRate").removeClass("valid");
            $("#txtTaxRate").addClass("required");
        }
        else {
            $("#txtTaxRate").addClass("valid");
            $("#txtTaxRate").removeClass("required");
        }
        if ($.trim($("#txtTaxableAmount").val()) == "" || parseFloat($("#txtTaxableAmount").val()) == 0) {
            $("#txtTaxableAmount").removeClass("valid");
            $("#txtTaxableAmount").addClass("required");
        }
        else {
            $("#txtTaxableAmount").addClass("valid");
            $("#txtTaxableAmount").removeClass("required");
        }

        if (flag == 0)
            $("#btnTaxSubmit").css("display", "none");
        else
            $("#btnTaxSubmit").css("display", "");
        objDialogyAdd.dialog({ title: 'Invoice Tax', width: '460', height: '290',close: function(event, ui) 
        {
            //objDialogyInvoice.find("form").remove();
            objDialogyAdd.dialog('destroy');
              
             
        }  });
        objDialogyAdd.dialog("open");
    };

    this.EditInvoiceDebitDetails = function (Index, flag) {
        var _tmpData = self.InvoiceDebitArray()[Index];

        var _value = _tmpData.rcm;
        $("input[name=rdorcmChargedFlag][value=N]").prop("checked", true);

        var GSTCharged = $("#hdfEditViewGstChargedFlag").val();
        if (GSTCharged == "Y") {
            $("input[name=rdorcmChargedFlag][value=N]").attr("disabled", true);
            $("input[name=rdorcmChargedFlag][value=Y]").attr("disabled", true);
        }
        else {
            if (_value == 'Yes') {
                $("input[name=rdorcmChargedFlag][value=Y]").attr("checked", true);
            }
            $("input[name=rdorcmChargedFlag][value=N]").attr("disabled", false);
            $("input[name=rdorcmChargedFlag][value=Y]").attr("disabled", false);
        }
        $("#hfDebitlineGId").val(_tmpData.GId);
        $("#hfInvId").val(_tmpData.invid);
        $("#ddlExpenses").val(_tmpData.expnaturegid);

        if ($("#hfISPO").val() != "PO") {
            self.LoadExpenseCategory();
            $("#ddlCategory").val(_tmpData.catgid);
            self.LoadExpenseSubCategory();
            $("#ddlSubCategory").val(_tmpData.subcatgid);
            self.LoadHsnArray();
            $("#tdHide").css("display", "");
            $("#tdVisible").attr("colspan", "1");
            $("#tdVisible").css("width", "33%");
        }
        else {
            self.LoadAssetCategory();
            $("#ddlCategory").val(_tmpData.catgid);
            self.LoadAssetSubCategory();
            $("#ddlSubCategory").val(_tmpData.subcatgid);
            self.LoadHsnArray();
            $("#tdHide").css("display", "none");
            $("#tdVisible").attr("colspan", "2");
            $("#tdVisible").css("width", "66%");
        }

        //self.LoadExpenseCategory();
        //$("#ddlCategory").val(_tmpData.catgid);
        //self.LoadExpenseSubCategory();
        //$("#ddlSubCategory").val(_tmpData.subcatgid);
        $("#ddlBU").val(_tmpData.fccode);
        $("#ddlCC").val(_tmpData.cccode);
        $("#txtProductCode").val(_tmpData.Product);
        $("#txtOUCode").val(_tmpData.location);
        $("#hfProductCode").val(_tmpData.ProdCode);
        $("#hfOUCode").val(_tmpData.OUCode);
        $("#txtDebitAmount").val(_tmpData.Amt);
        $("#txtDebitDescription").val(_tmpData.ddesc);
        $("#ddlhsngid").val(_tmpData.hsngid);

        if ($("#ddlExpenses").val() == "0") {
            $("#ddlExpenses").removeClass("valid");
            $("#ddlExpenses").addClass("required");
        }
        else {
            $("#ddlExpenses").addClass("valid");
            $("#ddlExpenses").removeClass("required");
        }
        if ($("#hfISPO").val() == "PO") {
            $("#ddlExpenses").removeClass("valid");
            $("#ddlExpenses").removeClass("required");
        }
        if ($("#ddlCategory").val() == "0") {
            $("#ddlCategory").removeClass("valid");
            $("#ddlCategory").addClass("required");
        }
        else {
            $("#ddlCategory").addClass("valid");
            $("#ddlCategory").removeClass("required");
        }
        if ($("#ddlSubCategory").val() == "0") {
            $("#ddlSubCategory").removeClass("valid");
            $("#ddlSubCategory").addClass("required");
        }
        else {
            $("#ddlSubCategory").addClass("valid");
            $("#ddlSubCategory").removeClass("required");
        }
        if ($("#txtProductCode").val() == "" || $("#hfProductCode").val() == "0") {
            $("#txtProductCode").removeClass("valid");
            $("#txtProductCode").addClass("required");
        }
        else {
            $("#txtProductCode").addClass("valid");
            $("#txtProductCode").removeClass("required");
        }
        if ($("#txtOUCode").val() == "" || $("#hfOUCode").val() == "0") {
            $("#txtOUCode").removeClass("valid");
            $("#txtOUCode").addClass("required");
        }
        else {
            $("#txtOUCode").addClass("valid");
            $("#txtOUCode").removeClass("required");
        }
        if ($("#txtDebitAmount").val() == "" || parseFloat($("#txtDebitAmount").val()) == 0) {
            $("#txtDebitAmount").removeClass("valid");
            $("#txtDebitAmount").addClass("required");
        }
        else {
            $("#txtDebitAmount").addClass("valid");
            $("#txtDebitAmount").removeClass("required");
        }

        if (flag == 0)
            $("#btnDebitLineSubmit").css("display", "none");
        else
            $("#btnDebitLineSubmit").css("display", "");
        objDialogyAddDebit.dialog({
            title: 'Debit Line', width: '800', height: '400', close: function (event, ui) {
                //objDialogyInvoice.find("form").remove();
                objDialogyAddDebit.dialog('destroy');


            }
        });
        objDialogyAddDebit.dialog("open");
    };

    this.EditInvoiceCreditDetails = function (Index, flag) {
        var _tmpData = self.InvoicePaymentArray()[Index];
        $("#hfIFSCCode").val(_tmpData.IfscCode);

        //kavitha

        if (_tmpData.PMode == "DD") {
            $("#txtPayRefNo1").removeAttr("disabled");
        }

        if (_tmpData.PMode == "EFT" || _tmpData.PMode == "CHQ" || _tmpData.PMode == "ERA" || _tmpData.PMode == "DD" || _tmpData.PMode == "RRP") {
            $("#hfCreditlineGId").val(_tmpData.CrditLineGID);
            $("#hfInvId").val(_tmpData.InvId);
            $("#hfRefId").val(_tmpData.InvId)
            $("#ddlPayMode1").val(_tmpData.PMode);
            $("#ddlPayBank").val(_tmpData.Bankid);
            $("#txtPayRefNo1").val(_tmpData.RefNo);
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
            objDialogyAddPayment1.dialog({
                title: 'Payment Details', width: '560', height: '380', close: function (event, ui) {
                    //objDialogyInvoice.find("form").remove();
                    objDialogyAddPayment1.dialog('destroy');


                }
            });
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



            if (flag == 0) {
                $("#btnCreditLineSubmit").css("display", "none");
            }
            else
                $("#btnCreditLineSubmit").css("display", "");
            objDialogyAddPayment.dialog({ title: 'Payment Details', width: '560', height: '320',close: function(event, ui) 
            {
                //objDialogyInvoice.find("form").remove();
                objDialogyAddPayment.dialog('destroy');
              
             
            }  });
            objDialogyAddPayment.dialog("open");
        }
    };

    this.EditWithHoldTaxDetails = function (Index, flag) {
        var _tmpData = self.InvoiceHoldingTaxArray()[Index];

        $("#hfWHTaxgid").val(_tmpData.withholdtaxgid);
        $("#hfInvId").val(_tmpData.invoicegid);
        $("#ddlWHTaxType").val(_tmpData.TaxId);
        self.LoadWHTaxSubType();
        $("#ddlWHTaxSubType").val(_tmpData.subtypegid);
        $("#txtWHTaxRate").val(_tmpData.Rate);
        $("#txtWHTaxableAmount").val(_tmpData.TaxableAmt);
        $("#txtWHTaxAmount").val(_tmpData.TaxAmount);
        $("#txtWHNetAmount").val(_tmpData.NetAmt);

        $("#ddlWHTaxType").attr("disabled", "disabled");

        if ($("#ddlWHTaxType").val() == "0") {
            $("#ddlWHTaxType").removeClass("valid");
            $("#ddlWHTaxType").addClass("required");
        }
        else {
            $("#ddlWHTaxType").addClass("valid");
            $("#ddlWHTaxType").removeClass("required");
        }
        if ($("#ddlWHTaxSubType").val() == "0") {
            $("#ddlWHTaxSubType").removeClass("valid");
            $("#ddlWHTaxSubType").addClass("required");
        }
        else {
            $("#ddlWHTaxSubType").addClass("valid");
            $("#ddlWHTaxSubType").removeClass("required");
        }
        if ($("#txtWHTaxRate").val() == "" || parseFloat($("#txtWHTaxRate").val()) == 0) {
            $("#txtWHTaxRate").removeClass("valid");
            $("#txtWHTaxRate").addClass("required");
        }
        else {
            $("#txtWHTaxRate").addClass("valid");
            $("#txtWHTaxRate").removeClass("required");
        }
        if ($("#txtWHTaxableAmount").val() == "" || parseFloat($("#txtWHTaxableAmount").val()) == 0) {
            $("#txtWHTaxableAmount").removeClass("valid");
            $("#txtWHTaxableAmount").addClass("required");
        }
        else {
            $("#txtWHTaxableAmount").addClass("valid");
            $("#txtWHTaxableAmount").removeClass("required");
        }

        if (flag == 0)
            $("#btnWHTaxSubmit").css("display", "none");
        else
            $("#btnWHTaxSubmit").css("display", "");
        objDialogyWHTax.dialog({ title: 'With Holding Tax', width: '460', height: '320',close: function(event, ui) 
        {
            //objDialogyInvoice.find("form").remove();
            objDialogyWHTax.dialog('destroy');
              
             
        }  });
        objDialogyWHTax.dialog("open");
    };

    this.ViewInvoice = function (InvId, flag) {

        //$("#divInvoice").slideDown();
        $('#divInvoice').attr("style", "display:block;");
        objDialogyInvoice.dialog({
            title: 'Invoice Details', width: '1100', height: '600', close: function (event, ui) {
                //objDialogyInvoice.find("form").remove();
                objDialogyInvoice.dialog('destroy');                
            }
        });
        objDialogyInvoice.dialog("open");

        $("#txtInvoiceDate").removeClass("required").addClass("valid");
        $("#txtInvoiceNo").removeClass("required").addClass("valid");
        $("#txtServiceMonth").removeClass("required").addClass("valid");
        $("#txtInvoiceAmount").removeClass("required").addClass("valid");
        $("#txtWithoutTaxAmount").removeClass("required").addClass("valid");
        $("#txtInvoiceDescription").removeClass("required").addClass("valid");

        $("#hfInvId").val(InvId);
        var EcfFilter = {
            InvId: InvId
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetInvoiceDetails",
            data: JSON.stringify(EcfFilter),
            contentType: "application/json;",
            async: false,
            success: function (response) {
                var Data1 = "", Data2 = "", Data3 = "", Data4 = "", Data5 = "", Data6 = "", Data7 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                if (Data1.length > 0) {
                    $("#txtInvoiceDate").val(Data1[0].InvDate);
                    $("#txtInvoiceNo").val(Data1[0].InvNo);
                    $("#txtServiceMonth").val(Data1[0].servicemonth);
                    $("#txtInvoiceAmount").val(Data1[0].InvAmt);
                    $("#txtWithoutTaxAmount").val(Data1[0].WOTaxAmount);
                    $("#txtInvoiceDescription").val(Data1[0].InvDesc);
                    $("#txtRetentionPercentage").val(Data1[0].Retentionpercent);
                    $("#txtRetentionAmount").val(Data1[0].RetentionAmt);
                    $("#txtReleaseDate").val(Data1[0].RetentionReleaseOn);
                    $("input[name=rdoRetentionFlag][value=" + Data1[0].InvRetention + "]").prop("checked", true);
                    $("input[name=rdoProvisionFlag][value=" + Data1[0].InvProvision + "]").prop("checked", true);
                    $("input[name=rdoGAmort][value=" + Data1[0].amort + "]").prop("checked", true);
                    $("input[name=rdoVerify][value=" + (Data1[0].isverified ? "1" : "0") + "]").prop("checked", true);

                    if ($("input[name=rdoRetentionFlag]:radio[checked=checked]").attr("value") == "Y")
                        $(".divRetention").slideDown();
                    else
                        $(".divRetention").slideUp();

                    if ($("input[name=rdoGAmort]:radio[checked=checked]").attr("value") == "Y")
                        $(".trAmort").css("display", "");
                    else
                        $(".trAmort").css("display", "none");

                    if ($.trim(Data1[0].Retentionpercent).length > 0)
                        $("#txtRetentionPercentage").addClass("valid");
                    if ($.trim(Data1[0].RetentionAmt).length > 0)
                        $("#txtRetentionAmount").addClass("valid");
                    if ($.trim(Data1[0].RetentionReleaseOn).length > 0)
                        $("#txtReleaseDate").addClass("valid");
                    $("#hfAmortId").val(Data1[0].amortid);
                    if (Data1[0].amortid == "0") {
                        $("#btnAmort").html("<span style=' color:white; font-size:12px; margin-right: 5px;' class='glyphicon glyphicon-plus'></span>Amort");
                    }
                    else {
                        $("#btnAmort").html("<span style=' color:white; font-size:12px; margin-right: 5px;' class='glyphicon glyphicon-edit'></span>Edit Amort");
                    }

                    var _value = Data1[0].GstCharged == "Yes" ? "Y" : "N";
                    $("input[name=rdogstChargedFlag][value=" + _value + "]").prop("checked", true);
                    $("#hdfEditViewGstChargedFlag").val(_value);
                    $("#ddlgstProviderLoc").val(Data1[0].ProviderLocation);
                    $("#ddlgstReceiverLoc").val(Data1[0].ReceiverLocation);
                    $("#txtGSTINVendor").val(Data1[0].GstinVendor);
                    $("#txtGSTINFICCL").val(Data1[0].GstinFiccl);

                    $("#ddlgstProviderLoc,#ddlgstReceiverLoc").removeClass("valid").removeClass("required");

                    if (parseInt($("#ddlgstProviderLoc").val()) == 0 || $("#ddlgstProviderLoc").val() == "") {
                        $("#ddlgstProviderLoc").addClass("required");
                    } else {
                        $("#ddlgstProviderLoc").addClass("valid");
                    }
                    if (parseInt($("#ddlgstReceiverLoc").val()) == 0 || $("#ddlgstReceiverLoc").val() == "") {
                        $("#ddlgstReceiverLoc").addClass("required");
                    } else {
                        $("#ddlgstReceiverLoc").addClass("valid");
                    } 
                    if ($("input[name=rdogstChargedFlag]:radio[checked=checked]").attr("value") == "Y") {                       
                        $("#tabGST").show();
                        $("#tabRCM").hide();
                        $('#Invoicetabs').tabs('select', 1);
                    } else { 
                        $("#tabGST").hide();
                        $("#tabRCM").show();
                        $('#Invoicetabs').tabs('select', 0);
                    }
                }

                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    self.InvoiceTaxArray(Data2);
                    self.GSTInvoiceTaxArray(Data2);
                    self.FindTotalTax();
                }

                if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null)
                    Data3 = JSON.parse(response.Data3);
                self.InvoiceDebitArray(Data3);

                if (response.Data4 != null && response.Data4 != "" && JSON.parse(response.Data4) != null)
                    Data4 = JSON.parse(response.Data4);
                self.InvoicePaymentArray(Data4);
                $("#tabTax").hide();
                if (response.Data5 != null && response.Data5 != "" && JSON.parse(response.Data5) != null) {
                    Data5 = JSON.parse(response.Data5);
                    self.InvoicePOArray(Data5);
                    $("#tabTax").show();
                }

                if (response.Data6 != null && response.Data6 != "" && JSON.parse(response.Data6) != null)
                    Data6 = JSON.parse(response.Data6);
                self.InvoiceHoldingTaxArray(Data6);

                if (response.Data7 != null && response.Data7 != "" && JSON.parse(response.Data7) != null)
                    Data7 = JSON.parse(response.Data7);
                self.InvoiceAttachmentArray(Data7);

                //RCM
                if (response.Data8 != null && response.Data8 != "" && JSON.parse(response.Data8) != null) {
                    Data8 = JSON.parse(response.Data8);
                    self.RCMInvoiceTaxArray(Data8);
                    self.FindRCMTotal();
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
        if (flag == 0)
            $(".viewForIvoice").css("display", "none");
        else
            $(".viewForIvoice").css("display", "");
        $("#tabTax").hide();
        if ($("#hfISPO").val() == "PO") {
            $("#tabTax").show();
            $("#divForNonPO").css("display", "none");
            $("#deleteDiv").attr("style", "display:none;");
        }

        return false;
    };

    this.ViewAmortSchedule = function () {

        var InvId = $("#hfInvId").val();
        var startdate = $("#txtPAmortDateFrom").val();
        var enddate = $("#txtPAmortDateTo").val();
        var frequencygid = $("#ddlPAmortCycle").val();
        var split = $("#txtPAmortSplit").val();
        var amount = $("#txtPAmortAmount").val();
        var glno = $("#ddlPAmortGl").val();
        var Desc = $("#txtPAmortDesc").val();

        if ($.trim(startdate) == "") {
            jAlert("Ensure Amort From!", "Message");
            return false;
        }

        if ($.trim(enddate) == "") {
            jAlert("Ensure Amort To!", "Message");
            return false;
        }

        if (frequencygid == "") {
            jAlert("Ensure Amort Cycle!", "Message");
            return false;
        }

        if ($.trim(amount) == "" || parseFloat($.trim(amount)) == 0) {
            jAlert("Ensure Amort Amount!", "Message");
            return false;
        }

        if (parseFloat($.trim(split)) == 0) {
            jAlert("Increase Date Range!", "Message");
            return false;
        }

        var AmortSchedule = {
            amortgid: "0",
            InvId: InvId,
            supplierId: "0",
            amount: amount,
            glno: glno,
            startdate: startdate,
            enddate: enddate,
            frequencygid: frequencygid,
            split: split,
            active: "Y",
            isremoved: "0"
        };

        $.ajax({
            type: "post",
            url: UrlDet + "GetAmortSchedule",
            data: JSON.stringify(AmortSchedule),
            contentType: "application/json;",
            async: false,
            success: function (response) {
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == false)
                        jAlert(Data1[0].Msg, "Message");
                }

                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                self.ScheduleArray(Data2);

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.SetAmortSchedule = function () {

        var amortgid = $("#hfAmortId").val();
        var EcfId = $("#hfECFId").val();
        var InvId = $("#hfInvId").val();
        var supplierId = $("#hfSupplierId").val();
        var startdate = $("#txtPAmortDateFrom").val();
        var enddate = $("#txtPAmortDateTo").val();
        var frequencygid = $("#ddlPAmortCycle").val();
        var split = $("#txtPAmortSplit").val();
        var amount = $("#txtPAmortAmount").val();
        var glno = $("#ddlPAmortGl  option:selected").text();
        var Desc = $("#txtPAmortDesc").val();
        var Percentage = "";
        var TotalPercentage = 0;
        for (var Index = 0; Index < self.AmortDebitArray().length ; Index++) {
            var _tmpData = self.AmortDebitArray()[Index];
            Percentage = Percentage + (Percentage == "" ? (_tmpData.GId + "/" + $("#txtDebitlinePercentage" + _tmpData.GId).val()) : ("~" + _tmpData.GId + "/" + $("#txtDebitlinePercentage" + _tmpData.GId).val()));
            TotalPercentage = TotalPercentage + parseFloat($("#txtDebitlinePercentage" + _tmpData.GId).val());
        }
        if ($.trim(startdate) == "") {
            jAlert("Ensure Amort From!", "Message");
            return false;
        }

        if ($.trim(enddate) == "") {
            jAlert("Ensure Amort To!", "Message");
            return false;
        }

        if (frequencygid == "") {
            jAlert("Ensure Amort Cycle!", "Message");
            return false;
        }

        if ($.trim(amount) == "" || parseFloat($.trim(amount)) == 0) {
            jAlert("Ensure Amort Amount!", "Message");
            return false;
        }

        if (parseFloat($.trim(split)) == 0) {
            jAlert("Increase Date Range!", "Message");
            return false;
        }

        if ($("#ddlPAmortGl").val() == "0") {
            jAlert("Ensure Amort GL!", "Message");
            return false;
        }

        if (Math.round(TotalPercentage) > 100) {

            jAlert("Total Debitline  Percentage should not exceeded to 100!", "Message");
            return false;
        }

        if (Math.round(TotalPercentage) < 100) {
            jAlert("Total Debitline Percentage Should be Matched With 100!", "Message");
            return false;
        }

        var AmortSchedule = {
            amortgid: amortgid,
            EcfId: EcfId,
            InvId: InvId,
            supplierId: supplierId,
            amount: amount,
            glno: glno,
            startdate: startdate,
            enddate: enddate,
            frequencygid: frequencygid,
            split: split,
            active: "Y",
            isremoved: "0",
            Percentage: Percentage
        };

        $.ajax({
            type: "post",
            url: UrlDet + "SetAmortScheduleDetails",
            data: JSON.stringify(AmortSchedule),
            contentType: "application/json;",
            async: false,
            success: function (response) {
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == false)
                        jAlert(Data1[0].Msg, "Message");
                    else {
                        $("#hfAmortId").val(Data1[0].amortid)
                        $("#btnAmort").html("<span style=' color:white; font-size:12px; margin-right: 5px;' class='glyphicon glyphicon-edit'></span>Edit Amort");
                        objDialogyAmort.dialog("close");
                        jAlert(Data1[0].Msg, "Message");
                    }
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.ViewAmortDetails = function () {

        //var AmortFrom = $("#txtAmortFrom").val();
        //var AmortTo = $("#txtAmortTo").val();
        //var AmortDesc = $("#txtAmortDescription").val();

        //if ($.trim(AmortFrom) == "") {
        //    jAlert("Ensure Amort From!", "Message");
        //    return false;
        //}
        //if ($.trim(AmortTo) == "") {
        //    jAlert("Ensure Amort To!", "Message");
        //    return false;
        //}
        //if ($.trim(AmortDesc) == "") {
        //    jAlert("Ensure Amort Description!", "Message");
        //    return false;
        //}

        var InvId = $("#hfInvId").val();
        var AmortId = $("#hfAmortId").val();
        var InputFilter = {
            InvId: InvId,
            AmortId: AmortId
        };

        $.ajax({
            type: "post",
            url: UrlDet + "SetECFAmort",
            data: JSON.stringify(InputFilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "", Data3 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                if (Data1.length > 0) {

                    $("#txtPAmortDateFrom").val(Data1[0].startdate);
                    $("#txtPAmortDateTo").val(Data1[0].enddate);
                    $("#txtPAmortAmount").val(Data1[0].amortamount);
                    $("#ddlPAmortCycle").val(Data1[0].frequencygid);
                    $("#txtPAmortSplit").val(Data1[0].split);
                    $("#ddlPAmortGl").val(Data1[0].glno + '~' + Data1[0].gldesc);
                    $("#txtPAmortDesc").val(Data1[0].gldesc);

                }
                else {
                    //$("#txtPAmortDateFrom").val($("#txtAmortFrom").val());
                    //$("#txtPAmortDateTo").val($("#txtAmortTo").val());
                    $("#txtPAmortAmount").val($("#txtInvoiceAmount").val());
                    //$("#txtPAmortDesc").val($("#txtAmortDescription").val());
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                //self.ScheduleArray.removeAll();
                self.ScheduleArray(Data2);
                if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null)
                    Data3 = JSON.parse(response.Data3);
                self.AmortDebitArray.removeAll();
                self.AmortDebitArray(Data3);

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });

        //$("#txtPAmortDateTo").datepicker("option", "minDate", $("#txtAmortFrom").val());
        //$("#txtPAmortDateFrom").datepicker("option", "maxDate", $("#txtAmortTo").val());

        if ($("#txtPAmortDateFrom").val() == "") {
            $("#txtPAmortDateFrom").removeClass("valid");
            $("#txtPAmortDateFrom").addClass("required");
        }
        else {
            $("#txtPAmortDateFrom").addClass("valid");
            $("#txtPAmortDateFrom").removeClass("required");
        }

        if ($("#txtPAmortDateTo").val() == "") {
            $("#txtPAmortDateTo").removeClass("valid");
            $("#txtPAmortDateTo").addClass("required");
        }
        else {
            $("#txtPAmortDateTo").addClass("valid");
            $("#txtPAmortDateTo").removeClass("required");
        }
        if ($("#ddlPAmortCycle").val() == "") {
            $("#ddlPAmortCycle").removeClass("valid");
            $("#ddlPAmortCycle").addClass("required");
        }
        else {
            $("#ddlPAmortCycle").addClass("valid");
            $("#ddlPAmortCycle").removeClass("required");
        }
        if ($("#txtPAmortAmount").val() == "" || parseFloat($("#txtPAmortAmount").val()) == 0) {
            $("#txtPAmortAmount").removeClass("valid");
            $("#txtPAmortAmount").addClass("required");
        }
        else {
            $("#txtPAmortAmount").addClass("valid");
            $("#txtPAmortAmount").removeClass("required");
        }
        if ($("#ddlPAmortGl").val() == "0") {
            $("#ddlPAmortGl").removeClass("valid");
            $("#ddlPAmortGl").addClass("required");
        }
        else {
            $("#ddlPAmortGl").addClass("valid");
            $("#ddlPAmortGl").removeClass("required");
        }
        //if ($("#txtPAmortDesc").val() == "") {
        //    $("#txtPAmortDesc").removeClass("valid");
        //    $("#txtPAmortDesc").addClass("required");
        //}
        //else {
        //    $("#txtPAmortDesc").addClass("valid");
        //    $("#txtPAmortDesc").removeClass("required");
        //}

        $('#AmortDialog').attr("style", "display:block;");
        objDialogyAmort.dialog({ title: 'Amort Schedule', width: '900', height: '550',close: function(event, ui) 
        {
            //objDialogyInvoice.find("form").remove();
            objDialogyAmort.dialog('destroy');
              
             
        }  });
        objDialogyAmort.dialog("open");
    };

    this.ViewPOMapping = function (POHeader, InvoicePOId) {

        var INVId = $("#hfInvId").val();

        var InputFilter = {
            POHeader: POHeader
        };
        var InputFilter1 = {
            InvoicePOId: InvoicePOId,
            INVId: INVId
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetPOMappingHeader",
            data: JSON.stringify(InputFilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                if (Data1.length > 0) {

                    $("#txtPODate").val(Data1[0].PODate);
                    $("#txtPONo").val(Data1[0].pono);
                    $("#txtPOStatus").val(Data1[0].POStatus);
                    $("#txtPOApproved").val(Data1[0].ApprovedlStatus);
                    $("#txtPOAmount").val(Data1[0].POAmount);
                    $("#txtReceivedQtyAmount").val(Data1[0].ReceivedQtyAmount);
                    $("#txtInvoicedQtyAmount").val(Data1[0].InvoicedQtyAmount);
                    $("#txtCurrentQtyAmount").val(Data1[0].CurrentQtyAmount);
                    $("#txtBalanceAmount").val(Data1[0].BalanceAmount);

                    $.ajax({
                        type: "post",
                        url: UrlDet + "GetPOMappingDetails",
                        data: JSON.stringify(InputFilter1),
                        async: false,
                        contentType: "application/json;",
                        success: function (response) {
                            var Data2 = "";
                            if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                                Data2 = JSON.parse(response.Data1);
                            self.POMappingArray(Data2);
                            $('#POMappingDialog').attr("style", "display:block;");
                            objDialogyPoMapping.dialog({ title: 'View PO Mapping', width: '950', height: '500',close: function(event, ui) 
                            {
                                //objDialogyInvoice.find("form").remove();
                                objDialogyPoMapping.dialog('destroy');
              
             
                            }  });
                            objDialogyPoMapping.dialog("open");
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {

                        }
                    });
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {

            }
        });
    };

    this.LoadInvoiceType = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetInvoiceType",
            data: "{}",
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.InvoiceTypeArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.LoadCurrencyType = function () {
        debugger;
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
                //$("#ddlCurrency selectedOptions:selected").val("4");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.LoadNatureOfExpenses = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetNatureOfExpenses",
            data: "{}",
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.ExpenseArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.LoadExpenseCategory = function () {
        var CatId = $("#ddlExpenses").val();
        var CatFilter = {
            CatId: CatId
        };
        $.ajax({
            type: "post",
            url: UrlHelper + "GetExpenseCategory",
            data: JSON.stringify(CatFilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.CategoryArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {

            }
        });
    };

    self.LoadGSTINCategory = function () {
        var RefId = $("#hfSupplierId").val(); //var RefId = "5838";
        var _Filter = {
            ViewType: "1",
            RefId: RefId
        };
        $.ajax({
            type: "post",
            url: UrlHelper + "GetGSTINArray",
            data: JSON.stringify(_Filter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.ProviderLocationArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {

            }
        });

        var _tmpFilter = {
            ViewType: "5",
            RefId: "0"
        };
        $.ajax({
            type: "post",
            url: UrlHelper + "GetGSTINArray",
            data: JSON.stringify(_tmpFilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.ReceiverLocationArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {

            }
        });
    };

    self.LoadGSTINHSNCode = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "LoadGSTINHSNCode",
            data: '{}',
            contentType: "application/json;",
            success: function (response) {
                self.GSTHSNCodeArray(response);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.LoadExpenseSubCategory = function () {
        var CatId = $("#ddlCategory").val();
        var CatFilter = {
            CatId: CatId
        };
        $.ajax({
            type: "post",
            url: UrlHelper + "GetExpenseSubCategory",
            data: JSON.stringify(CatFilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.SubCategoryArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.LoadTaxType = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetTaxType",
            data: "{}",
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.TaxTypeArray(Data1);
                //self.GSTTaxTypeArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.LoadGSTTaxType = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetGSTTaxType",
            data: "{}",
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                //self.TaxTypeArray(Data1);
                self.GSTTaxTypeArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.LoadBusinessSegment = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetBusinessSegment",
            data: "{}",
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.BusinessSegmentArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.LoadCostCenter = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetCostCenter",
            data: "{}",
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.CostCenterArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.LoadPayMode = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetPayMode",
            data: "{}",
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.PayModeArray(Data1);
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

    this.ChangeNatureOfExpences = function () {
        if ($("#ddlCategory").hasClass("valid")) {
            $("#ddlCategory").removeClass("valid");
            $("#ddlCategory").addClass("required");
        }
        if ($("#ddlSubCategory").hasClass("valid")) {
            $("#ddlSubCategory").removeClass("valid");
            $("#ddlSubCategory").addClass("required");
        }
        if ($("#hfISPO").val() != "PO") {
            self.LoadExpenseCategory();
            self.LoadExpenseSubCategory();
        }
        else {
            self.LoadAssetCategory();
            self.LoadAssetSubCategory();
        }
        //self.LoadExpenseCategory();
        //self.LoadExpenseSubCategory();
    };

    this.ChangeExpencesCategory = function () {
        if ($("#ddlSubCategory").hasClass("valid")) {
            $("#ddlSubCategory").removeClass("valid");
            $("#ddlSubCategory").addClass("required");
        }
        if ($("#hfISPO").val() != "PO") {
            self.LoadExpenseSubCategory();
        }
        else {
            self.LoadAssetSubCategory();
        }
        //self.LoadExpenseSubCategory();
    };


    //change of expense cat to load hsn kavitha
    this.ChangeExpencesubCategory = function () {
        if ($("#ddlhsngid").hasClass("valid")) {
            $("#ddlhsngid").removeClass("valid");
            $("#ddlhsngid").addClass("required");
        }
        self.LoadHsnArray();
    };


    this.ChangeHsnCode = function () {
        //hsn+provider location state 
        var hsngid = $("#ddlhsngid").val();
        if (hsngid == "0") {
            jAlert("Ensure hsn details", "Error");
            return false;
        }
        $("#rdorcmChargedNo").prop("checked", true);
        var GSTCharged = $("#hdfEditViewGstChargedFlag").val();
        if (GSTCharged == "N") {
            var DebitlineGId = $("#hfDebitlineGId").val();
            var ECFId = $("#hfECFId").val();
            var invid = $("#hfInvId").val();
            var ProviderLocation = $("#ddlgstProviderLoc").val();
            var GstinVendor = $("#txtGSTINVendor").val();

            if ((parseInt(ProviderLocation) == 0 || $.trim(ProviderLocation) == "")) {
                jAlert("Ensure Provider Location!", "Message");
                return false;
            }

            var rcmflagparam = {
                DebitlineGId: DebitlineGId,
                ECFId: ECFId,
                invid: invid,
                hsngid: hsngid,
                ProviderLocation: ProviderLocation,
                action: 'single'

            };

            $.ajax({
                type: "post",
                url: UrlDet + "CheckrcmExists",
                data: JSON.stringify(rcmflagparam),
                contentType: "application/json;",
                success: function (response) {
                    var Data1 = "";
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                        Data1 = JSON.parse(response.Data1);
                        if (Data1[0].STATUS == 'EXISTS') {
                            $("#rdorcmChargedYes").prop("checked", true);
                            $("input[name=rdorcmChargedFlag][value=N]").attr("disabled", false);
                            $("input[name=rdorcmChargedFlag][value=Y]").attr("disabled", false);
                        }
                        else {
                            $("#rdorcmChargedNo").prop("checked", true);
                            $("input[name=rdorcmChargedFlag][value=N]").attr("disabled", true);
                            $("input[name=rdorcmChargedFlag][value=Y]").attr("disabled", true);
                        }
                    }


                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            });

        }


    };
    this.LoadHsnArray = function () {

        var CatId = $("#ddlSubCategory").val();
        var potype = $("#hfISPO").val();
        var CatFilter = {
            CatId: CatId,
            potype: potype
        };
        $.ajax({
            type: "post",
            url: UrlHelper + "GetHsnArray",
            data: JSON.stringify(CatFilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    self.HsnCodeArray(Data1);
                    if (parseInt($("#ddlhsngid").val()) == 0 || $("#ddlhsngid").val() == "" || $("#ddlhsngid").val() ==null) {
                        $("#ddlhsngid").addClass("required");
                        $("#ddlhsngid").removeClass("valid");
                    } else {
                        $("#ddlhsngid").addClass("valid");
                        $("#ddlhsngid").removeClass("required");
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.ChangeTaxType = function () {
        self.LoadTaxSubType();

        if ($("#txtTaxRate").hasClass("valid")) {
            $("#txtTaxRate").removeClass("valid");
            $("#txtTaxRate").addClass("required");
        }
        if ($("#txtTaxableAmount").hasClass("valid")) {
            $("#txtTaxableAmount").removeClass("valid");
            $("#txtTaxableAmount").addClass("required");
        }

        //$("#txtTaxableAmount").val("")
        $("#txtTaxAmount").val("")
        $("#txtTaxRate").val("0")
        $("#txtTaxRate").removeAttr("disabled");
        var TaxId = $("#ddlTaxType").val();
        var SubTaxgid = $("#ddlSubTaxType").val();
        var SId = $("#hfSupplierId").val();
        if (TaxId == "0")
            return false;
        var InputFilter = {
            TaxId: TaxId,
            SubTaxgid: SubTaxgid,
            SId: SId,
            View: 0
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetTaxRate",
            data: JSON.stringify(InputFilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                //alert("1");
                var Data1 = "", Data2 = "", Data3 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = response.Data2;

                $("#ddlWHTaxSubType").val(Data2);
                if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null)
                    Data3 = response.Data3;

                $("#ddlWHTaxType").val(Data3);
                if (Data1[0].Clear == 2) {
                    $("#txtTaxRate").val(Data1[0].taxrate);
                    //$("#txtTaxRate").attr("disabled", "disabled");
                    if ($("#txtTaxRate").hasClass("required")) {
                        $("#txtTaxRate").removeClass("required");
                        $("#txtTaxRate").addClass("valid");
                    }
                }
                else if (Data1[0].Clear == 1) {
                    $("#txtTaxRate").removeAttr("disabled");
                    jConfirm(Data1[0].Msg, "Confirm", function (callback) {
                        if (callback == true) {

                        } else {
                            $("#ddlTaxType").val("0");
                            if ($("#ddlTaxType").hasClass("valid")) {
                                $("#ddlTaxType").removeClass("valid");
                                $("#ddlTaxType").addClass("required");
                            }
                            return false;
                        }
                    });
                }
                else {
                    $("#ddlTaxType").val("0");
                    $("#txtTaxRate").val("0");
                    jAlert(Data1[0].Msg, "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });

        var Rate = $.trim($("#txtTaxRate").val()) == "" ? 0 : parseFloat($("#txtTaxRate").val());
        var Amount = $.trim($("#txtTaxableAmount").val()) == "" ? 0 : parseFloat($("#txtTaxableAmount").val());
        var Tax = (Rate * Amount) / 100;
        $("#txtTaxAmount").val(Tax.toFixed(2))
        if (Amount > 0) {
            $("#txtTaxableAmount").removeClass("required");
            $("#txtTaxableAmount").addClass("valid");
        }
        else {
            $("#txtTaxableAmount").removeClass("valid");
            $("#txtTaxableAmount").addClass("required");
        }
    };

    this.ChangeSubTaxType = function () {

        if ($("#ddlSubTaxType").val() == "0") {
            $("#ddlSubTaxType").removeClass("valid");
            $("#ddlSubTaxType").addClass("required");
        }
        else {
            $("#ddlSubTaxType").addClass("valid");
            $("#ddlSubTaxType").removeClass("required");
        }

        var TaxId = $("#ddlTaxType").val();
        var SubTaxgid = $("#ddlSubTaxType").val();
        var SId = $("#hfSupplierId").val();
        if (TaxId == "0")
            return false;
        var InputFilter = {
            TaxId: TaxId,
            SubTaxgid: SubTaxgid,
            SId: SId,
            View: 0
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetTaxRate",
            data: JSON.stringify(InputFilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "", Data3 = ""; //alert("2");
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                //if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                //    Data2 = response.Data2;

                //$("#ddlWHTaxSubType").val(Data2);
                //if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null)
                //    Data3 = response.Data3;

                //$("#ddlWHTaxType").val(Data3);
                $("#txtTaxRate").val(Data1[0].taxrate);
                if ($("#txtTaxRate").val() == "" || parseFloat($("#txtTaxRate").val()) == 0) {
                    $("#txtTaxRate").removeClass("valid");
                    $("#txtTaxRate").addClass("required");
                }
                else {
                    $("#txtTaxRate").addClass("valid");
                    $("#txtTaxRate").removeClass("required");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });

        var Rate = $.trim($("#txtTaxRate").val()) == "" ? 0 : parseFloat($("#txtTaxRate").val());
        var Amount = $.trim($("#txtTaxableAmount").val()) == "" ? 0 : parseFloat($("#txtTaxableAmount").val());
        var Tax = (Rate * Amount) / 100;
        $("#txtTaxAmount").val(Tax.toFixed(2));
        if (Amount > 0) {
            $("#txtTaxableAmount").removeClass("required");
            $("#txtTaxableAmount").addClass("valid");
        }
        else {
            $("#txtTaxableAmount").removeClass("valid");
            $("#txtTaxableAmount").addClass("required");
        }

    };

    this.ChangeWHSubTaxType = function () {

        var TaxId = $("#ddlWHTaxType").val();
        var SubTaxgid = $("#ddlWHTaxSubType").val();
        var SId = $("#hfSupplierId").val();
        if (TaxId == "0")
            return false;
        if (TaxId == "1")
        var InputFilter = {
            TaxId: TaxId,
            SubTaxgid: SubTaxgid,
            SId: SId,
            View: 1
        };
        else
            var InputFilter = {
                TaxId: TaxId,
                SubTaxgid: SubTaxgid,
                SId: SId,
                View: 0
            };
        $.ajax({
            type: "post",
            url: UrlDet + "GetTaxRate",
            data: JSON.stringify(InputFilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "", Data3 = ""; //alert("3");
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);


                $("#txtWHTaxRate").val(Data1[0].taxrate);
                if ($("#txtWHTaxRate").val() == "" || parseFloat($("#txtWHTaxRate").val()) == 0) {
                    $("#txtWHTaxRate").removeClass("valid");
                    $("#txtWHTaxRate").addClass("required");
                }
                else {
                    $("#txtWHTaxRate").addClass("valid");
                    $("#txtWHTaxRate").removeClass("required");
                }
                if (Data1[0].NSDL == "Y")
                    $("#lblNSDL").css("display", "");
                else
                    $("#lblNSDL").css("display", "none");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });

        var Rate = $.trim($("#txtWHTaxRate").val()) == "" ? 0 : parseFloat($("#txtWHTaxRate").val());
        var Amount = $.trim($("#txtWHTaxableAmount").val()) == "" ? 0 : parseFloat($("#txtWHTaxableAmount").val());
        var Tax = (Rate * Amount) / 100;
        $("#txtWHTaxAmount").val(Tax)
        $("#txtWHNetAmount").val(Amount - Tax);
        if (Amount > 0) {
            $("#txtWHTaxableAmount").removeClass("required");
            $("#txtWHTaxableAmount").addClass("valid");
        }
        else {
            $("#txtWHTaxableAmount").removeClass("valid");
            $("#txtWHTaxableAmount").addClass("required");
        }
    };

    this.FetchPPXPaymentRefNo = function () {
        var SupplierId = $("#hfSupplierId").val();
        var type = $("#ddlPayMode option:selected").text();
        var ViewType = "0";
        if ($.trim(type) == "PPX") {
            ViewType = "0"
        }
        else if ($.trim(type) == "CRN") {
            ViewType = "1"
        }
        else if ($.trim(type) == "SUS") {
            ViewType = "2"
            //return false;
        }
        else if ($.trim(type) == "EFT") {
            ViewType = "3"
            return false;
        }
        else {
            return false;
        }
        var InputFilter = {
            SupplierId: SupplierId,
            ViewType: ViewType
        };
        $.ajax({
            type: "post",
            url: UrlDet + "FetchARFDetails",
            data: JSON.stringify(InputFilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                if (ViewType == "0") {
                    self.PPXArray(Data2);
                }
                else if (ViewType == "1") {
                    self.CreditNoteArray(Data2);
                }
                else if (ViewType == "2") {

                    self.loadSUSGrid();

                    self.SUSArray(Data2);
                } else if (ViewType == "3") {
                    //self.PPXArray(Data2);
                }
                //self.PPXArray(Data2);
                if ($.trim(Data1[0].Message) == "") {
                    if (ViewType == "0") {
                        $('#PPXDialog').attr("style", "display:block;");
                        objDialogyPPX.dialog({ title: 'Supplier PPX Details', width: '900', height: '400',close: function(event, ui) 
                        {
                            //objDialogyInvoice.find("form").remove();
                            objDialogyPPX.dialog('destroy');
              
             
                        }  });
                        objDialogyPPX.dialog("open");
                    }
                    else if (ViewType == "1") {
                        $('#CreditNoteDialog').attr("style", "display:block;");
                        objDialogyCreditNote.dialog({ title: 'Supplier Credit Note Details', width: '900', height: '400',close: function(event, ui) 
                        {
                            //objDialogyInvoice.find("form").remove();
                            objDialogyCreditNote.dialog('destroy');
              
             
                        }  });
                        objDialogyCreditNote.dialog("open");
                    }
                    else if (ViewType == "2") {
                        $("#txtSrcGLNo").val("");

                        $('#SUSDialog').attr("style", "display:block;");
                        objDialogySUS.dialog({ title: 'Supplier SUS Details', width: '900', height: '550',close: function(event, ui) 
                        {
                            //objDialogyInvoice.find("form").remove();
                            objDialogySUS.dialog('destroy');
              
             
                        }  });
                        objDialogySUS.dialog("open");
                    } else if (ViewType == "3") {
                    }
                }
                else {
                    jAlert(Data1[0].Message, "Message");
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.FetchBenificiary = function () {

        var SupplierId = $("#hfSupplierId").val();
        var PayMode = $("#ddlPayMode1").val();
        if (PayMode == "0")
            return false;
        var InputFilter = {
            PayMode: PayMode,
            SupplierId: SupplierId
        };
        $.ajax({
            type: "post",
            url: UrlDet + "FetchBenificiaryDetails",
            data: JSON.stringify(InputFilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                // alert();
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.BenificiaryArray(Data1);
                $('#BenificiaryDialog').attr("style", "display:block;");
                objDialogyBenificiary.dialog({ title: 'Benificiary Details', width: '700', height: '400',close: function(event, ui) 
                {
                    //objDialogyInvoice.find("form").remove();
                    objDialogyBenificiary.dialog('destroy');
              
             
                }  });
                objDialogyBenificiary.dialog("open");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.SrcGLNo = function () {
        var _GLNo = $("#txtSrcGLNo").val();
        var ViewType = "3";

        if ($.trim(_GLNo) == "") {
            jAlert("Ensure GL Number/Name!", "Message");
            return false;
        }

        var InputFilter = {
            GLDet: _GLNo,
            ViewType: ViewType
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SearchGLDetails",
            data: JSON.stringify(InputFilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);

                if (ViewType == "3") {
                    self.loadSUSGrid();
                    self.SUSArray(Data2);
                }
                if ($.trim(Data1[0].Message) != "") {
                    jAlert(Data1[0].Message, "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.SUSDatatableCall = function () {
        if ($("#gvSUSDet > tbody > tr").length == self.SUSArray().length) {
            $("#gvSUSDet").DataTable({
                "autoWidth": false,
                "destroy": true,
                "aoColumnDefs": [{
                    "aTargets": ["nosort"],
                    "bSortable": false
                }]
            });
        }
    };

    self.loadSUSGrid = function () {
        $("#gvSUSDet").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.SUSArray.removeAll();
    }

    this.SelectPPX = function (item) {
        $("#txtPayRefNo").val(item.ARFNo);
        $("#txtPaymentAmount").val(item.ARFException);
        $("#hfRefId").val(item.ecfarfgid)
        if ($("#txtPayRefNo").hasClass("required")) {
            $("#txtPayRefNo").removeClass("required");
            $("#txtPayRefNo").addClass("valid");
        }
        if ($("#txtPaymentAmount").hasClass("required")) {
            $("#txtPaymentAmount").removeClass("required");
            $("#txtPaymentAmount").addClass("valid");
        }
        objDialogyPPX.dialog("close");
    };

    this.SelectBenificiary = function (item) {
        $("#txtPayRefNo1").val(item.AccountNo);
        $("#txtPayBeneficiary1").val(item.BeneficiaryName);
        $("#hfIFSCCode").val(item.IFSCCode);

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
        objDialogyBenificiary.dialog("close");
    };

    this.SelectCreditNote = function (item) {
        $("#txtPayRefNo").val(item.creditnoteno);
        $("#txtPaymentAmount").val(item.creditnoteamt);
        $("#hfRefId").val(item.creditnotegid)
        if ($("#txtPayRefNo").hasClass("required")) {
            $("#txtPayRefNo").removeClass("required");
            $("#txtPayRefNo").addClass("valid");
        }
        if ($("#txtPaymentAmount").hasClass("required")) {
            $("#txtPaymentAmount").removeClass("required");
            $("#txtPaymentAmount").addClass("valid");
        }
        objDialogyCreditNote.dialog("close");
    };

    this.SelectSUS = function (item) {
        $("#txtPayRefNo").val(item.GLNo);
        $("#hfRefId").val(item.GLNo);
        if ($("#txtPayRefNo").hasClass("required")) {
            $("#txtPayRefNo").removeClass("required");
            $("#txtPayRefNo").addClass("valid");
        }
        //if ($("#txtPaymentAmount").hasClass("required")) {
        //    $("#txtPaymentAmount").removeClass("required");
        //    $("#txtPaymentAmount").addClass("valid");
        //}
        objDialogySUS.dialog("close");
    };

    this.LoadWHTaxType = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetWHTaxType",
            data: "{}",
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.WHTaxTypeArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.LoadWHTaxSubType = function () {
        var CatId = $("#ddlWHTaxType").val();
        var CatFilter = {
            CatId: CatId
        };
        $.ajax({
            type: "post",
            url: UrlHelper + "GetWHTaxSubType",
            data: JSON.stringify(CatFilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);

                self.WHTaxSubTypeArray(Data1);

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {

            }
        });
    };

    this.LoadTaxSubType = function () {
        var CatId = $("#ddlTaxType").val();
        var CatFilter = {
            CatId: CatId
        };
        $.ajax({
            type: "post",
            url: UrlHelper + "GetWHTaxSubType",
            data: JSON.stringify(CatFilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.SubTaxTypeArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {

            }
        });
    };

    this.ChangeWHTaxType = function () {
        if ($("#ddlWHTaxSubType").hasClass("valid")) {
            $("#ddlWHTaxSubType").removeClass("valid");
            $("#ddlWHTaxSubType").addClass("required");
        }
        self.LoadWHTaxSubType();

        if ($("#txtWHTaxRate").hasClass("valid")) {
            $("#txtWHTaxRate").removeClass("valid");
            $("#txtWHTaxRate").addClass("required");
        }
        //if ($("#txtTaxableAmount").hasClass("valid")) {
        //    $("#txtTaxableAmount").removeClass("valid");
        //    $("#txtTaxableAmount").addClass("required");
        //}

        //$("#txtWHTaxableAmount").val("");
        $("#txtWHTaxAmount").val("");
        $("#txtWHNetAmount").val("");
        $("#txtWHTaxRate").val("0");
        //$("#txtWHTaxRate").removeAttr("disabled");
        var TaxId = $("#ddlWHTaxType").val();
        var SubTaxgid = $("#ddlWHTaxSubType").val();
        var SId = $("#hfSupplierId").val();
        if (TaxId == "0")
            return false;
        var InputFilter = {
            TaxId: TaxId,
            SubTaxgid: SubTaxgid,
            SId: SId,
            View: 1
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetTaxRate",
            data: JSON.stringify(InputFilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "", Data3 = ""; //alert("4");
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);

                if (Data1[0].Clear == 2) {
                    $("#txtWHTaxRate").val(Data1[0].taxrate);
                    //$("#txtTaxRate").attr("disabled", "disabled");
                    if ($("#txtWHTaxRate").hasClass("required")) {
                        $("#txtWHTaxRate").removeClass("required");
                        $("#txtWHTaxRate").addClass("valid");

                        if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null)
                            Data3 = response.Data3;
                        //  alert(Data3)
                        $("#ddlWHTaxSubType").val(Data3);
                        //if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                        //    Data2 = response.Data2;
                        //alert(Data2)
                        //$("#txtWHTaxRate").val(Data2);
                    }
                }
                else if (Data1[0].Clear == 1) {
                    //$("#txtWHTaxRate").removeAttr("disabled");
                    jConfirm(Data1[0].Msg, "Confirm", function (callback) {
                        if (callback == true) {
                        } else {
                            $("#ddlWHTaxType").val("0");
                            if ($("#ddlWHTaxType").hasClass("valid")) {
                                $("#ddlWHTaxType").removeClass("valid");
                                $("#ddlWHTaxType").addClass("required");
                            }
                            return false;
                        }
                    });
                }
                else {
                    $("#ddlWHTaxType").val("0");
                    $("#txtWHTaxRate").val("0");
                    jAlert(Data1[0].Msg, "Message");
                }
                if (Data1[0].NSDL == "Y")
                    $("#lblNSDL").css("display", "");
                else
                    $("#lblNSDL").css("display", "none");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });

        var Rate = $.trim($("#txtWHTaxRate").val()) == "" ? 0 : parseFloat($("#txtWHTaxRate").val());
        var Amount = $.trim($("#txtWHTaxableAmount").val()) == "" ? 0 : parseFloat($("#txtWHTaxableAmount").val());
        var Tax = (Rate * Amount) / 100;
        $("#txtWHTaxAmount").val(Tax)
        $("#txtWHNetAmount").val(Amount - Tax);
        if (Amount > 0) {
            $("#txtWHTaxableAmount").removeClass("required");
            $("#txtWHTaxableAmount").addClass("valid");
        }
        else {
            $("#txtWHTaxableAmount").removeClass("valid");
            $("#txtWHTaxableAmount").addClass("required");
        }
    };

    this.LoadAmortCycle = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "LoadAmortCycle",
            data: "{}",
            async: false,
            contentType: "application/json;",
            success: function (response) {
                //var Data1 = "";
                //if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                //    Data1 = JSON.parse(response.Data1);
                self.AmortCycleArray(response);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.LoadAmortGl = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetAmortGl",
            data: "{}",
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.AmortGlArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.LoadTaxRate = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetTaxRate",
            data: "{}",
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "", Data3 = "";// alert("6");
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.TaxRateArray(Data1);
                //if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                //    Data2 = response.Data2;

                //$("#ddlWHTaxSubType").val(Data2);
                //if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null)
                //    Data3 = response.Data3;

                //$("#ddlWHTaxType").val(Data3);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.DownloadFormat = function () {
        window.location = UrlDef + "/Template/SupplierDebitline.xls";
    };

    this.UploadDebitLine = function () {

        var InvId = $("#hfInvId").val();

        var InputFilter = {
            InvId: InvId
        };

        $.ajax({
            type: "post",
            url: UrlDet + "DebitLineExcelSheets",
            data: JSON.stringify(InputFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "", Data3 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == "2") {
                        $("#fuDebitLine").replaceWith($("#fuDebitLine").val('').clone(true));
                        jConfirm(Data1[0].Msg + " Do you want to download error list!", "Confirm", function (callback) {
                            if (callback == true) {
                                ko.utils.postJson(UrlDet + "DownloadErrorExcel");
                            } else {
                                return false;
                            }
                        });
                    }
                    else if (Data1[0].Clear == "0") {
                        $("#fuDebitLine").replaceWith($("#fuDebitLine").val('').clone(true));
                        jAlert(Data1[0].Msg, "Message");
                    }
                    else {
                        jAlert(Data1[0].Msg, "Message");
                        $("#fuDebitLine").replaceWith($("#fuDebitLine").val('').clone(true));
                        if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                            Data2 = JSON.parse(response.Data2);
                        self.InvoiceDebitArray(Data2);

                        if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null)

                            Data3 = JSON.parse(response.Data3);
   
                        self.GSTInvoiceTaxArray(Data3);
                        self.FindTotalTax();
                    }
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });

    };

    this.DownloadDebitLine = function () {

        var InvId = $("#hfInvId").val();

        var InputFilter = {
            InvId: InvId
        };

        $.ajax({
            type: "post",
            url: UrlDet + "DebitlineExcel",
            data: JSON.stringify(InputFilter),
            contentType: "application/json;",
            success: function (response) {
                ko.utils.postJson(UrlDet + "DownloadExcel");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });

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
            url: UrlHelper + "GetPayModeCommon",
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

    this.LoadAssetCategory = function () {
        var CatId = $("#ddlExpenses").val();
        var CatFilter = {
        };
        $.ajax({
            type: "post",
            url: UrlHelper + "GetAssetCategory",
            data: JSON.stringify(CatFilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.CategoryArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {

            }
        });
    };

    this.LoadAssetSubCategory = function () {
        var CatId = $("#ddlCategory").val();
        var CatFilter = {
            CatId: CatId
        };
        $.ajax({
            type: "post",
            url: UrlHelper + "GetAssetSubCategory",
            data: JSON.stringify(CatFilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.SubCategoryArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.AddDebitline = function () {
        var GSTCharged = $("#hdfEditViewGstChargedFlag").val();
        if (GSTCharged == "Y") {
            //$("#rdorcmChargedYes").prop("disabled", true);
            $("input[name=rdorcmChargedFlag][value=N]").attr("disabled", true);
            $("input[name=rdorcmChargedFlag][value=Y]").attr("disabled", true);
            //$("#rdorcmChargedNo").prop("disabled", true);
            $("#rdorcmChargedNo").prop("checked", true);
        }
        else {
            $("input[name=rdorcmChargedFlag][value=N]").attr("disabled", false);
            $("input[name=rdorcmChargedFlag][value=Y]").attr("disabled", false);
            $("#rdorcmChargedNo").prop("checked", true);
        }
        $('#ShowDialog1').attr("style", "display:block;");
        self.LoadNatureOfExpenses();

        if ($("#hfISPO").val() != "PO") {
            self.LoadExpenseCategory();
            self.LoadExpenseSubCategory();
            $("#ddlExpenses").removeClass("valid");
            $("#ddlExpenses").addClass("required");
            $("#tdHide").css("display", "");
            $("#tdVisible").attr("colspan", "1");
            $("#tdVisible").css("width", "33%");
        }
        else {
            self.LoadAssetCategory();
            self.LoadAssetSubCategory();
            $("#ddlExpenses").removeClass("valid");
            $("#ddlExpenses").removeClass("required");
            $("#tdHide").css("display", "none");
            $("#tdVisible").css("width", "66%");
            $("#tdVisible").attr("colspan", "2");
        }


        if ($("#ddlCategory").hasClass("valid")) {
            $("#ddlCategory").removeClass("valid");
            $("#ddlCategory").addClass("required");
        }
        if ($("#ddlSubCategory").hasClass("valid")) {
            $("#ddlSubCategory").removeClass("valid");
            $("#ddlSubCategory").addClass("required");
        }
        if ($("#hfProductCodeText").val() != "" && $("#hfEmpProductCode").val() != "0") {
            $("#txtProductCode").removeClass("required");
            $("#txtProductCode").addClass("valid");
        }
        else {
            $("#txtProductCode").removeClass("valid");
            $("#txtProductCode").addClass("required");
        }
        if ($("#hfOUCodeText").val() != "" && $("#hfEmpOUCode").val() != "0") {
            $("#txtOUCode").removeClass("required");
            $("#txtOUCode").addClass("valid");
        }
        else {
            $("#txtOUCode").removeClass("valid");
            $("#txtOUCode").addClass("required");
        }
        if ($("#txtDebitAmount").hasClass("valid")) {
            $("#txtDebitAmount").removeClass("valid");
            $("#txtDebitAmount").addClass("required");
        }

        objDialogyAddDebit.dialog({ title: 'Debit Line', width: '800', height: '400', close: function(event, ui) 
         {
             //objDialogyInvoice.find("form").remove();
            objDialogyAddDebit.dialog('destroy');
              
             
        }  });
        $("#hfDebitlineGId").val("0");
        $("#ddlExpenses").val("0");
        $("#ddlCategory").val("0");
        $("#ddlSubCategory").val("0");
        $("#ddlBU").val($("#hfBU").val());
        $("#ddlCC").val($("#hfCC").val());
        $("#txtProductCode").val($("#hfProductCodeText").val());
        $("#txtOUCode").val($("#hfOUCodeText").val());
        $("#hfProductCode").val($("#hfEmpProductCode").val());
        $("#hfOUCode").val($("#hfEmpOUCode").val());
        $("#txtDebitAmount").val("");
        $("#txtDebitDescription").val("");
        $("#btnDebitLineSubmit").css("display", "");
        objDialogyAddDebit.dialog("open");
        return false;
    }

    this.ResetDebitLine = function () {

        jConfirm("Are you sure? Want to Clear All data!", "Confirm", function (callback) {
            if (callback == true) {
                var INVId = $("#hfInvId").val();

                var InputFilter = {
                    INVId: INVId
                };

                $.ajax({
                    type: "post",
                    url: UrlDet + "SetDebitlineReset",
                    data: JSON.stringify(InputFilter),
                    contentType: "application/json;",
                    success: function (response) {
                        var Data1 = "";
                        if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                            Data1 = JSON.parse(response.Data1);
                            if (Data1[0].Clear == false) {
                                jAlert(Data1[0].Msg, "Message");
                            }
                            else {
                                jAlert(Data1[0].Msg, "Message");
                                self.InvoiceDebitArray("");
                            }
                        }

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

    //<!-- GST Invoice Tax Details -->
    self.GSTLoadTaxSubType = function () {
        var CatId = $("#ddlGSTTaxType").val();
        var CatFilter = {
            CatId: CatId
        };
        $.ajax({
            type: "post",
            url: UrlHelper + "GetWHTaxSubType",
            data: JSON.stringify(CatFilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.GSTSubTaxTypeArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {

            }
        });
    };

    

    self.HSNCodeSelectChanged = function (obj, event) {
        if (event.originalEvent) {
            $("#ddlGSTHSNCode").removeClass("valid").removeClass("required");
            var value = $("#ddlGSTHSNCode").val();
            if (value != null && value != "0" && value != "") {
                $("#ddlGSTHSNCode").addClass("valid");
            } else { $("#ddlGSTHSNCode").addClass("required"); }

            var _text = $('#ddlGSTHSNCode option:selected').attr('title');
            $("#txtGSTHSNDesc").val(_text);
        } else {
        }
    }

    self.ViewGSTInvoiceTaxDetails = function (item) {
        //$("#txtGSTTaxRate").attr("disabled", true);
        $("#ddlGSTHSNCode,#ddlGSTTaxType,#ddlGSTSubTaxType,#txtGSTRate,#txtGSTTaxAmount,#txtGSTTaxableAmount").removeClass("required").removeClass("valid");
        if (item.GsnApplicable == "Yes" || item.GsnApplicable == "yes") {
            $("#chkGSTgsnapplicability").prop('checked', true);
        } else {
            $("#chkGSTgsnapplicability").prop('checked', false);
        }

        $("#hfGSTInvoiceTaxGid").val(item.invoicetaxgid);
        $("#ddlGSTHSNCode option:contains(" + item.HsnCode + ")").attr('selected', 'selected');
        $("#txtGSTHSNDesc").val(item.HsnDescription);
        self.LoadGSTTaxType();
        $("#ddlGSTTaxType").val(item.Taxgid);
        self.GSTLoadTaxSubType();
        $("#ddlGSTSubTaxType").val(item.TaxsubtypegID);
        $("#tdOtherTax").css("display", "none");
        $("#tdGST").css('display', 'block');
        $("#txtGSTRate").val(item.Rate);
        $("#txtGSTTaxableAmount").val(item.TaxableAmt);
        $("#txtGSTTaxAmount").val(item.TaxAmt);
        $("#btnGSTTaxSubmit").css("display", "none");

        objDialogyGST.dialog({ title: 'GST Invoice Tax', width: '460', height: '400',close: function(event, ui) 
        {
            //objDialogyInvoice.find("form").remove();
            objDialogyGST.dialog('destroy');
              
             
        }  });
        objDialogyGST.dialog("open");
    };

    self.EditGSTInvoiceTaxDetails = function (item) {
        debugger;
        //$("#txtGSTTaxRate").attr("disabled", false);
        $("#ddlGSTHSNCode,#ddlGSTTaxType,#ddlGSTSubTaxType,#txtGSTRate,#txtGSTTaxAmount,#txtGSTTaxableAmount").removeClass("required").removeClass("valid");
        if (item.GsnApplicable == "Yes" || item.GsnApplicable == "yes") {
            $("#chkGSTgsnapplicability").prop('checked', true);
        } else {
            $("#chkGSTgsnapplicability").prop('checked', false);
        }

        $("#hfGSTInvoiceTaxGid").val(item.invoicetaxgid);
        //$("#ddlGSTHSNCode option:contains(" + item.HsnCode + ")").attr('selected', 'selected');
        $("#ddlGSTHSNCode option:selected").text(item.HsnCode);
        $("#txtGSTHSNDesc").val(item.HsnDescription);
        self.LoadGSTTaxType();
        $("#ddlGSTTaxType").val(item.Taxgid);
        self.GSTLoadTaxSubType();
        $("#ddlGSTSubTaxType").val(item.TaxsubtypegID);
        $("#tdOtherTax").css("display", "none");
        $("#tdGST").css('display', 'block');
        $("#txtGSTRate").val(item.Rate);
        $("#txtGSTTaxableAmount").val(item.TaxableAmt);
        $("#txtGSTTaxAmount").val(item.TaxAmt);

        if ($("#ddlGSTTaxType").val() == "0") {
            $("#ddlGSTTaxType").addClass("required");
        } else {
            $("#ddlGSTTaxType").addClass("valid");
        }
        if ($("#ddlGSTSubTaxType").val() == "0") {
            $("#ddlGSTSubTaxType").addClass("required");
        }
        else {
            $("#ddlGSTSubTaxType").addClass("valid");
        }
        if ($.trim($("#txtGSTRate").val()) == "" || parseFloat($("#txtGSTRate").val()) == 0) {
            $("#txtGSTRate").addClass("required");
        } else {
            $("#txtGSTRate").addClass("valid");
        }
        if ($.trim($("#txtGSTTaxableAmount").val()) == "" || parseFloat($("#txtGSTTaxableAmount").val()) == 0) {
            $("#txtGSTTaxableAmount").addClass("required");
        } else {
            $("#txtGSTTaxableAmount").addClass("valid");
        }

        $("#btnGSTTaxSubmit").css("display", "");
        objDialogyGST.dialog({ title: 'GST Invoice Tax', width: '460', height: '400',close: function(event, ui) 
        {
            //objDialogyInvoice.find("form").remove();
            objDialogyGST.dialog('destroy');
              
             
        }  });
        objDialogyGST.dialog("open");
    };

    self.DeleteGSTInvoiceTaxDetails = function (item) {
        jConfirm("Are you sure? Want to delete Invoice Tax!", "Confirm", function (callback) {
            if (callback == true) {
                var InvId = $("#hfInvId").val();
                var InvoiceTaxgid = item.invoicetaxgid;

                var InvoiceTax = {
                    InvoiceTaxgid: InvoiceTaxgid,
                    InvId: InvId,
                    SupplierId: "0",
                    GSNapplicable: "N",
                    HSNgid: "0",
                    TaxId: "0",
                    TaxRate: "0",
                    TaxableAmt: "0",
                    TaxAmount: "0",
                    IsRemoved: "1"
                };
                $.ajax({
                    type: "post",
                    url: UrlDet + "SetGSTInvoiceTaxDetails",
                    data: JSON.stringify(InvoiceTax),
                    contentType: "application/json;",
                    success: function (response) {
                        var Data1 = "", Data2 = "", Data3 = "";
                        if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                            Data1 = JSON.parse(response.Data1);
                            if (Data1[0].Clear == false)
                                jAlert(Data1[0].Msg, "Message");
                            else {
                                jAlert(Data1[0].Msg, "Message");
                            }
                        }
                        if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                            Data2 = JSON.parse(response.Data2);
                        self.GSTInvoiceTaxArray(Data2);

                        /*if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null)
                            Data3 = JSON.parse(response.Data3);
                        self.InvoiceDebitArray(Data3);*/
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

    self.SetGSTInvoiceTaxDetails = function () {
        var GSNapplicable = "";

        if ($("#chkGSTgsnapplicability").prop("checked")) {
            GSNapplicable = "Y";
        } else {
            GSNapplicable = "N";
        }
        debugger;
        var HSNgid = $("#ddlGSTHSNCode option:selected").text();
        //var HSNgid = $("#ddlGSTHSNCode").val();
        
        var InvoiceTaxgid = $("#hfGSTInvoiceTaxGid").val();
        var InvId = $("#hfInvId").val();
        var SupplierId = $("#hfSupplierId").val();
        var TaxId = $("#ddlGSTTaxType").val();
        var SubTaxId = $("#ddlGSTSubTaxType").val();
        var TaxRate = $("#txtGSTRate").val();
        var TaxableAmt = $.trim($("#txtGSTTaxableAmount").val()) == "" ? 0 : parseFloat($("#txtGSTTaxableAmount").val());
        var TaxAmount = $.trim($("#txtGSTTaxAmount").val()) == "" ? 0 : parseFloat($("#txtGSTTaxAmount").val());
        var IsRemoved = "0";
        var INVAmount = $.trim($("#txtInvoiceAmount").val()) == "" ? 0 : parseFloat($("#txtInvoiceAmount").val());
        var Tax = (TaxRate * TaxableAmt) / 100;

        if (TaxId == "0") {
            jAlert("Ensure Tax Type!", "Message");
            return false;
        }

        if (SubTaxId == "0") {
            jAlert("Ensure Tax SubType!", "Message");
            return false;
        }

        if ($.trim(TaxRate) == "" || parseFloat(TaxRate) == 0) {
            jAlert("Ensure Tax Rate!", "Message");
            return false;
        }

        if ($.trim(TaxableAmt) == "" || parseFloat(TaxableAmt) == 0) {
            jAlert("Ensure Taxable Amount!", "Message");
            return false;
        }

        if (TaxAmount == 0) {
            jAlert("Ensure Tax Amount!", "Message");
            return false;
        }

        if (TaxAmount > (Tax + 5) || TaxAmount < (Tax - 5)) {
            jAlert("You can only plus or minus 5 into Tax Amount!", "Message");
            return false;
        }

        if (INVAmount < (TaxableAmt + TaxAmount) && parseFloat(TaxRate) != 100) {
            jAlert("Taxable Amount + Tax Amount should not exceeded Invoice Amount!", "Message");
            return false;
        }

        var InvoiceTax = {
            InvoiceTaxgid: InvoiceTaxgid,
            InvId: InvId,
            SupplierId: SupplierId,
            GSNapplicable: GSNapplicable,
            HSNgid: HSNgid,
            TaxId: TaxId,
            SubTaxId: SubTaxId,
            TaxRate: TaxRate,
            TaxableAmt: TaxableAmt,
            TaxAmount: TaxAmount,
            IsRemoved: IsRemoved
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetGSTInvoiceTaxDetails",
            data: JSON.stringify(InvoiceTax),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "", Data3 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == false)
                        jAlert(Data1[0].Msg, "Message");
                    else {
                        objDialogyGST.dialog("close");
                        jAlert(Data1[0].Msg, "Message");
                    }
                }

                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                self.GSTInvoiceTaxArray(Data2);

                /*if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null)
                    Data3 = JSON.parse(response.Data3);
                self.InvoiceDebitArray(Data3);*/
                self.FindTotalTax();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.FindTotalTax = function () { 
        var total = 0.0;
        var items = ko.utils.arrayForEach(self.GSTInvoiceTaxArray(), function (item) {
            if (item.TaxAmt != null && item.TaxAmt != "") {
                total += parseFloat(ko.utils.unwrapObservable(item.TaxAmt));
            }
        });
        self.GSTTotalTaxAmt(total.toFixed(2));
    };
    self.FindRCMTotal = function () {
        var total = 0.0;
        var items = ko.utils.arrayForEach(self.RCMInvoiceTaxArray(), function (item) {
            if (item.debitrcm_amount != null && item.debitrcm_amount != "") {
                total += parseFloat(ko.utils.unwrapObservable(item.debitrcm_amount));
            }
        });
        self.RCMTotalAmt(total.toFixed(2));
    };
    //<!-- GST Invoice Tax Details -->

    self.LoadGSTINHSNCode();
    self.LoadInvoiceType();
    self.LoadCurrencyType();
    self.LoadECFDetails();

    self.LoadTaxType();
    self.LoadGSTTaxType();
    self.LoadBusinessSegment();
    self.LoadCostCenter();
    self.LoadPayMode();
    self.LoadAttachmentType();
    self.LoadAmortCycle();
    self.LoadAmortGl();
    self.LoadTaxRate();

    self.LoadNatureOfExpenses();
    self.LoadExpenseCategory();
    self.LoadExpenseSubCategory();

    self.LoadWHTaxType();
    self.LoadWHTaxSubType();
    self.LoadTaxSubType();

    self.loadSUSGrid();

    self.LoadPayModeCommon();
    self.LoadPayBank();
//kavitha
    self.LoadHsnArray();
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
    self.InvoiceHoldingTaxHistory = ko.observableArray();
    self.InvoiceDebitHistory = ko.observableArray();
    self.ECFInvoiceHeader = ko.observableArray();
    self.GSTInvoiceTaxHistory = ko.observableArray();
    self.AmortHistory = ko.observableArray();

    this.LoadAuditHistory = function () {
        debugger;
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
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                }

                if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null) {
                    Data3 = JSON.parse(response.Data3);
                }
                if (response.Data7 != null && response.Data7 != "" && JSON.parse(response.Data7) != null) {
                    Data7 = JSON.parse(response.Data7);
                }
                if (response.Data8 != null && response.Data8 != "" && JSON.parse(response.Data8) != null) {
                    Data8 = JSON.parse(response.Data8);
                }
                if (response.Data9 != null && response.Data9 != "" && JSON.parse(response.Data9) != null) {
                    Data9 = JSON.parse(response.Data9);
                }

                self.ECFPaymentHistory(Data1);
                self.InvoiceHoldingTaxHistory(Data2);
                self.InvoiceDebitHistory(Data3);
                self.ECFInvoiceHeader(Data7);
                self.GSTInvoiceTaxHistory(Data8);
                self.AmortHistory(Data9);
                

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

     

    self.drpaudit = function () { 
    //$("#drpaudit").change(function () {
        //alert('Changed');
        self.LoadAuditHistory();
        debugger;
        if ($('#drpaudit').val() == 1) {
            $("#showpaymentdialog").css("display", "block");
            $("#showAuditworktray").css("display", "none");
            $("#showDebitline").css("display", "none");
            $("#InvoiceHeader").css("display", "none");
            $("#GstTaxHeader").css("display", "none");
            $("#AmortHeader").css("display", "none");
        }
        else if ($('#drpaudit').val() == 2) {
            $("#showAuditworktray").css("display", "block");
            $("#showpaymentdialog").css("display", "none");
            $("#showDebitline").css("display", "none");
            $("#InvoiceHeader").css("display", "none");
            $("#GstTaxHeader").css("display", "none");
            $("#AmortHeader").css("display", "none");
        }

        else if ($('#drpaudit').val() == 3) {
            $("#showDebitline").css("display", "block");
            $("#showAuditworktray").css("display", "none");
            $("#showpaymentdialog").css("display", "none");
            $("#InvoiceHeader").css("display", "none");
            $("#GstTaxHeader").css("display", "none");
            $("#AmortHeader").css("display", "none");
        }
        else if ($('#drpaudit').val() == 4) {
            $("#InvoiceHeader").css("display", "block");
            $("#showDebitline").css("display", "none");
            $("#showAuditworktray").css("display", "none");
            $("#showpaymentdialog").css("display", "none");
            $("#GstTaxHeader").css("display", "none");
            $("#AmortHeader").css("display", "none");
        }
        else if ($('#drpaudit').val() == 5) {
            $("#InvoiceHeader").css("display", "none");
            $("#showDebitline").css("display", "none");
            $("#showAuditworktray").css("display", "none");
            $("#showpaymentdialog").css("display", "none");
            $("#GstTaxHeader").css("display", "block");
            $("#AmortHeader").css("display", "none");
        }
        else if ($('#drpaudit').val() == 6) {
            $("#InvoiceHeader").css("display", "none");
            $("#showDebitline").css("display", "none");
            $("#showAuditworktray").css("display", "none");
            $("#showpaymentdialog").css("display", "none");
            $("#GstTaxHeader").css("display", "none");
            $("#AmortHeader").css("display", "block");
        }
        else {
            $("#InvoiceHeader").css("display", "none");
            $("#showAuditworktray").css("display", "none");
            $("#showpaymentdialog").css("display", "none");
            $("#showDebitline").css("display", "none");
            $("#GstTaxHeader").css("display", "none");
            $("#AmortHeader").css("display", "none");
        }

        

    }

   // self.LoadAuditHistory();
};

var mainViewModel = new SearchModel();
ko.applyBindings(mainViewModel, document.getElementById("secSupplierInvoice"));
debugger;
$("#ddlCurrency").val(currencyid);
$(document).ready(function () {
    var DebitlineGId = $("#hfDebitlineGId").val();
    var ECFId = $("#hfECFId").val();
    var invid = $("#hfInvId").val();
    var ProviderLocation = "0";
   ProviderLocation = $("#ddlgstProviderLoc").val();
    var GstinVendor = $("#txtGSTINVendor").val();
    var hsngid = $("#ddlhsngid").val();
 

    var rcmflagparam = {
        DebitlineGId: DebitlineGId,
        ECFId: ECFId,
        invid: invid,
        hsngid: hsngid,
        ProviderLocation: ProviderLocation,
        action:'bulk'
    };

    $.ajax({
        type: "post",
        url: UrlDet + "CheckrcmExists",
        data: JSON.stringify(rcmflagparam),
        contentType: "application/json;",
        success: function (response) {
            //var Data1 = "";
            ////if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
            //    Data1 = JSON.parse(response.Data1);
                //if (Data1[0].STATUS == 'EXISTS')
                    //  jAlert(Data1[0].Msg, "Message");
                    //$("#rdorcmChargedYes").prop("checked", true);
               // else {
                    // objDialogyAddDebit.dialog("close");
                    //jAlert(Data1[0].Msg, "Message");
                  //  $("#rdorcmChargedNo").prop("checked", true);
               // }
           // }


        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });

    objDialogyAdd = $("[id$='ShowDialog']");
    objDialogyAdd.dialog({
        autoOpen: false,
        modal: true,
        width: 460,
        height: 290,
        duration: 300,
        close: function(event, ui) 
        {
            //objDialogyInvoice.find("form").remove();
            objDialogyAdd.dialog('destroy');
              
             
        } 

    });

    objDialogyAddDebit = $("[id$='ShowDialog1']");
    objDialogyAddDebit.dialog({
        autoOpen: false,
        modal: true,
        width: 800,
        height: 400,
        duration: 300,
        close: function (event, ui) {
            //objDialogyInvoice.find("form").remove();
            objDialogyAddDebit.dialog('destroy');


        }

    });

    objDialogyAddPayment = $("[id$='ShowDialog2']");
    objDialogyAddPayment.dialog({
        autoOpen: false,
        modal: true,
        width: 560,
        height: 380,
        duration: 300,
        close: function(event, ui) 
        {
            //objDialogyInvoice.find("form").remove();
            objDialogyAddPayment.dialog('destroy');
              
             
        } 
    });

    objDialogyAddAttachment = $("[id$='ShowDialog3']");
    objDialogyAddAttachment.dialog({
        autoOpen: false,
        modal: true,
        width: 560,
        height: 300,
        duration: 300,
        close: function(event, ui) 
        {
            //objDialogyInvoice.find("form").remove();
            objDialogyAddAttachment.dialog('destroy');
              
             
        } 
    });

    objDialogyInvoice = $("[id$='divInvoice']");
    objDialogyInvoice.dialog({
        autoOpen: false,
        modal: true,
        width: 1100,
        height: 600,
        duration: 300, 
         close: function(event, ui) 
         {
             //objDialogyInvoice.find("form").remove();
             objDialogyInvoice.dialog('destroy');
              
             
        } 
    });

    objDialogyPPX = $("[id$='PPXDialog']");
    objDialogyPPX.dialog({
        autoOpen: false,
        modal: true,
        width: 900,
        height: 400,
        duration: 300,
        close: function(event, ui) 
        {
            //objDialogyInvoice.find("form").remove();
            objDialogyPPX.dialog('destroy');
              
             
        } 
    });

    objDialogyBenificiary = $("[id$='BenificiaryDialog']");
    objDialogyBenificiary.dialog({
        autoOpen: false,
        modal: true,
        width: 700,
        height: 400,
        duration: 300,
        close: function(event, ui) 
        {
            //objDialogyInvoice.find("form").remove();
            objDialogyBenificiary.dialog('destroy');
              
             
        } 
    });

    objDialogyCreditNote = $("[id$='CreditNoteDialog']");
    objDialogyCreditNote.dialog({
        autoOpen: false,
        modal: true,
        width: 900,
        height: 400,
        duration: 300,
        close: function(event, ui) 
        {
            //objDialogyInvoice.find("form").remove();
            objDialogyCreditNote.dialog('destroy');
              
             
        } 
    });

    objDialogySUS = $("[id$='SUSDialog']");
    objDialogySUS.dialog({
        autoOpen: false,
        modal: true,
        width: 600,
        height: 550,
        close: function(event, ui) 
        {
            //objDialogyInvoice.find("form").remove();
            objDialogySUS.dialog('destroy');
              
             
        } 
    });

    objDialogyWHTax = $("[id$='WithholdTaxDialog']");
    objDialogyWHTax.dialog({
        autoOpen: false,
        modal: true,
        width: 460,
        height: 320,
        duration: 300,
        close: function(event, ui) 
        {
            //objDialogyInvoice.find("form").remove();
            objDialogyWHTax.dialog('destroy');
              
             
        } 
    });

    objDialogyAmort = $("[id$='AmortDialog']");
    objDialogyAmort.dialog({
        autoOpen: false,
        modal: true,
        width: 900,
        height: 600,
        duration: 300,
        close: function(event, ui) 
        {
            //objDialogyInvoice.find("form").remove();
            objDialogyAmort.dialog('destroy');
              
             
        } 
    });

    objDialogyPoMapping = $("[id$='POMappingDialog']");
    objDialogyPoMapping.dialog({
        autoOpen: false,
        modal: true,
        width: 950,
        height: 500,
        duration: 300,
        close: function(event, ui) 
        {
            //objDialogyInvoice.find("form").remove();
            objDialogyPoMapping.dialog('destroy');
              
             
        } 

    });

    objDialogyAddPayment1 = $("[id$='ShowDialogPayBank']");
    objDialogyAddPayment1.dialog({
        autoOpen: false,
        modal: true,
        width: 560,
        height: 380,
        duration: 300,
        close: function (event, ui) {
            //objDialogyInvoice.find("form").remove();
            objDialogyAddPayment1.dialog('destroy');


        }
    });

    objDialogyGST = $("[id$='GSTDialog']");
    objDialogyGST.dialog({
        autoOpen: false,
        modal: true,
        width: 460,
        height: 290,
        duration: 300,
        close: function(event, ui) 
        {
            //objDialogyInvoice.find("form").remove();
            objDialogyGST.dialog('destroy');
              
             
        } 
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

    $('#txtServiceMonth').datepicker({
        changeMonth: true,
        changeYear: true,
        showButtonPanel: true,
        maxDate: '-id',
        dateFormat: 'MM yy'
    }).focus(function () {
        var thisCalendar = $(this);
        $('.ui-datepicker-header').css("width", "240px");
        $('.ui-datepicker-calendar').detach();
        $('.ui-datepicker-close').click(function () {
            var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
            var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
            thisCalendar.datepicker('setDate', new Date(year, month, 1));
            $("#txtServiceMonth").removeClass('required');
            $("#txtServiceMonth").addClass('valid');
        });
    });

    $("#txtAmortFrom").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        numberOfMonths: 1,
        onSelect: function (selected) {
            $("#txtAmortFrom").addClass('valid');
            $("#txtAmortTo").datepicker("option", "minDate", selected)
        }
    });

    $("#txtAmortTo").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        numberOfMonths: 1,
        onSelect: function (selected) {
            $("#txtAmortTo").addClass('valid');
            $("#txtAmortFrom").datepicker("option", "maxDate", selected)
        }
    });

    $("#txtPAmortDateFrom").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        numberOfMonths: 1,
        onSelect: function (selected) {
            $("#txtPAmortDateFrom").addClass('valid');
            $("#txtPAmortDateTo").datepicker("option", "minDate", selected)
            GetAmortSplit();
        }
    });

    $("#txtPAmortDateTo").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        numberOfMonths: 1,
        onSelect: function (selected) {
            $("#txtPAmortDateTo").addClass('valid');
            $("#txtPAmortDateFrom").datepicker("option", "maxDate", selected)
            GetAmortSplit();
        }
    });

    $("#txtReleaseDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        minDate: 'd',
        onSelect: function (selected) {
            $("#txtReleaseDate").removeClass('required');
            $("#txtReleaseDate").addClass('valid');
        }
    });

    $("#txtAmortDescription").keyup(function () {
        if ($.trim($("#txtAmortDescription").val()).length > 0) {
            $("#txtAmortDescription").removeClass('required');
            $("#txtAmortDescription").addClass('valid');
        }
        else {
            $("#txtAmortDescription").addClass('required');
            $("#txtAmortDescription").removeClass('valid');
        }
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

    $("input[name=rdoGAmort]:radio").change(function () {
        $("#txtAmortFrom").removeClass("valid");
        $("#txtAmortFrom").addClass("required");
        $("#txtAmortTo").removeClass("valid");
        $("#txtAmortTo").addClass("required");
        $("#txtAmortDescription").removeClass("valid");
        $("#txtAmortDescription").addClass("required");

        $("#txtAmortFrom").val("");
        $("#txtAmortTo").val("");
        $("#txtAmortDescription").val("");

        if ($("input[name=rdoGAmort]:radio[checked=checked]").attr("value") == "Y")
            $(".trAmort").css("display", "");
        else
            $(".trAmort").css("display", "none");
    });

    $("input[name=rdoRetentionFlag]:radio").change(function () {

        $("#txtRetentionPercentage").removeClass("valid");
        $("#txtRetentionPercentage").addClass("required");
        $("#txtRetentionAmount").removeClass("valid");
        $("#txtRetentionAmount").addClass("required");
        $("#txtReleaseDate").removeClass("valid");
        $("#txtReleaseDate").addClass("required");

        $("#txtRetentionPercentage").val("");
        $("#txtRetentionAmount").val("");
        $("#txtReleaseDate").val("");

        if ($("input[name=rdoRetentionFlag]:radio[checked=checked]").attr("value") == "Y")
            $(".divRetention").slideDown();
        else
            $(".divRetention").slideUp();
    });

   /* $("input[name=rdogstChargedFlag]:radio").change(function () {
      //  alert($("input[name=rdogstChargedFlag]:radio[checked=checked]").attr("value"));
        $("#ddlgstProviderLoc").val("0");
        $("#ddlgstReceiverLoc").val("0");
        $("#txtGSTINVendor").val("");
        $("#txtGSTINFICCL").val("");

        $("#ddlgstProviderLoc,#ddlgstReceiverLoc").removeClass("valid").removeClass("required");
        $("#ddlgstProviderLoc,#ddlgstReceiverLoc").addClass("required");

        if ($("input[name=rdogstChargedFlag]:radio[checked=checked]").attr("value") == "Y") {
            $(".divGSTChargedShow").show();
            $(".divGSTChargedShowrcm").show()
        }

        else {
            $(".divGSTChargedShow").hide();
            $(".divGSTChargedShowrcm").show();
        }
    });*/

    $("#ddlgstReceiverLoc").change(function () {
        //var value = $(this).val();
        //var s = $("#ddlgstReceiverLoc").prop('selectedIndex');

        $("#ddlgstReceiverLoc").removeClass("valid").removeClass("required");
        var value = $(this).val();
        if (value != null && value != "0" && value != "") {
            $("#ddlgstReceiverLoc").addClass("valid");
        } else { $("#ddlgstReceiverLoc").addClass("required"); }

        var _text = $('#ddlgstReceiverLoc option:selected').attr('title');
        $("#txtGSTINFICCL").val(_text);
    });

    $("#ddlgstProviderLoc").change(function () {
        $("#ddlgstProviderLoc").removeClass("valid").removeClass("required");
        var value = $(this).val();
        if (value != null && value != "0" && value != "") {
            $("#ddlgstProviderLoc").addClass("valid");
        } else { $("#ddlgstProviderLoc").addClass("required"); }

        var _text = $('#ddlgstProviderLoc option:selected').attr('title');
        $("#txtGSTINVendor").val(_text);
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

    $("#ddlWHTaxType").change(function () {
        var TaxType = $("#ddlWHTaxType").val();
        if (TaxType != "0") {
            $("#ddlWHTaxType").removeClass("required");
            $("#ddlWHTaxType").addClass("valid");
        }
        else {
            $("#ddlWHTaxType").removeClass("valid");
            $("#ddlWHTaxType").addClass("required");
        }
    });

    $("#ddlWHTaxSubType").change(function () {
        var TaxType = $("#ddlWHTaxSubType").val();
        if (TaxType != "0") {
            $("#ddlWHTaxSubType").removeClass("required");
            $("#ddlWHTaxSubType").addClass("valid");
        }
        else {
            $("#ddlWHTaxSubType").removeClass("valid");
            $("#ddlWHTaxSubType").addClass("required");
        }
    });

    $("#ddlCurrency").change(function () {
        debugger;
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
            debugger;
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

    $("#ddlTaxType").change(function () {
        var TaxType = $("#ddlTaxType").val();
        if (TaxType != "0") {
            $("#ddlTaxType").removeClass("required");
            $("#ddlTaxType").addClass("valid");
        }
        else {
            $("#ddlTaxType").removeClass("valid");
            $("#ddlTaxType").addClass("required");
        }
    });

    $("#ddlExpenses").change(function () {
        var Expenses = $("#ddlExpenses").val();
        if (Expenses != "0") {
            $("#ddlExpenses").removeClass("required");
            $("#ddlExpenses").addClass("valid");
        }
        else {
            $("#ddlExpenses").removeClass("valid");
            $("#ddlExpenses").addClass("required");
        }
    });

    $("#ddlCategory").change(function () {
        var Category = $("#ddlCategory").val();
        if (Category != "0") {
            $("#ddlCategory").removeClass("required");
            $("#ddlCategory").addClass("valid");
        }
        else {
            $("#ddlCategory").removeClass("valid");
            $("#ddlCategory").addClass("required");
        }
    });


    $("#ddlSubCategory").change(function () {
        var SubCategory = $("#ddlSubCategory").val();
        if (SubCategory != "0") {
            $("#ddlSubCategory").removeClass("required");
            $("#ddlSubCategory").addClass("valid");
        }
        else {
            $("#ddlSubCategory").removeClass("valid");
            $("#ddlSubCategory").addClass("required");
        }
    });

    $("#ddlPayMode").change(function () {
        var PayMode = $("#ddlPayMode").val();
        if (PayMode != "0") {
            $("#ddlPayMode").removeClass("required");
            $("#ddlPayMode").addClass("valid");
        }
        else {
            $("#ddlPayMode").removeClass("valid");
            $("#ddlPayMode").addClass("required");
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
        if (PayMode == "-- Select One --") {
            jAlert("Ensure Payment Mode!", "Message");
            return false;
        }

        if (PayMode == "DD") {
            $("#lblPayableAt").text("Payable At");
            //kavitha
            $("#txtPayRefNo1").removeAttr("disabled");
            //Code Added by sabari on 20170108
            //$("input").prop('disabled', false);
        }
        else {
            $("#lblPayableAt").text("Account No");
            $("#txtPayRefNo1").attr("disabled");
            //Code Added by sabari on 20170108
            //$("input").prop('disabled', true);
        }
        $("#txtPayRefNo1").val("");
        $("#txtPayBeneficiary1").val("");
        $("#hfIFSCCode").val("");

        $("#txtPayRefNo1").removeClass("valid").addClass("required");
        $("#txtPayBeneficiary1").removeClass("valid").addClass("required");
    });

    $("#txtDebitAmount").keyup(function () {
        var Amount = $.trim($("#txtDebitAmount").val()) == "" ? 0 : parseFloat($("#txtDebitAmount").val());
        if (Amount > 0) {
            $("#txtDebitAmount").removeClass("required");
            $("#txtDebitAmount").addClass("valid");
        }
        else {
            $("#txtDebitAmount").removeClass("valid");
            $("#txtDebitAmount").addClass("required");
        }
    });

    $("#txtPaymentAmount").keyup(function () {
        var Amount = $.trim($("#txtPaymentAmount").val()) == "" ? 0 : parseFloat($("#txtPaymentAmount").val());
        if (Amount > 0) {
            $("#txtPaymentAmount").removeClass("required");
            $("#txtPaymentAmount").addClass("valid");
        }
        else {
            $("#txtPaymentAmount").removeClass("valid");
            $("#txtPaymentAmount").addClass("required");
        }
    });

    $("#txtPayDescription").keyup(function () {
        var Amount = $.trim($("#txtPayDescription").val());
        if (Amount != "") {
            $("#txtPayDescription").removeClass("required");
            $("#txtPayDescription").addClass("valid");
        }
        else {
            $("#txtPayDescription").removeClass("valid");
            $("#txtPayDescription").addClass("required");
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

    $("#txtTaxRate").keyup(function () {
        var Rate = $.trim($("#txtTaxRate").val()) == "" ? 0 : parseFloat($("#txtTaxRate").val());
        var Amount = $.trim($("#txtTaxableAmount").val()) == "" ? 0 : parseFloat($("#txtTaxableAmount").val());
        var Tax = (Rate * Amount) / 100;
        $("#txtTaxAmount").val(Tax.toFixed(2))
        if (Rate > 0) {
            $("#txtTaxRate").removeClass("required");
            $("#txtTaxRate").addClass("valid");
        }
        else {
            $("#txtTaxRate").removeClass("valid");
            $("#txtTaxRate").addClass("required");
        }
    });

    $("#txtTaxRate").change(function () {
        var Rate = $.trim($("#txtTaxRate").val()) == "" ? 0 : parseFloat($("#txtTaxRate").val());
        var Amount = $.trim($("#txtTaxableAmount").val()) == "" ? 0 : parseFloat($("#txtTaxableAmount").val());
        var Tax = (Rate * Amount) / 100;
        $("#txtTaxAmount").val(Tax.toFixed(2))
        if (Rate > 0) {
            $("#txtTaxRate").removeClass("required");
            $("#txtTaxRate").addClass("valid");
        }
        else {
            $("#txtTaxRate").removeClass("valid");
            $("#txtTaxRate").addClass("required");
        }
    });

    $("#txtTaxableAmount").keyup(function () {
        var Rate = $.trim($("#txtTaxRate").val()) == "" ? 0 : parseFloat($("#txtTaxRate").val());
        var Amount = $.trim($("#txtTaxableAmount").val()) == "" ? 0 : parseFloat($("#txtTaxableAmount").val());
        var Tax = (Rate * Amount) / 100;
        $("#txtTaxAmount").val(Tax.toFixed(2))
        if (Amount > 0) {
            $("#txtTaxableAmount").removeClass("required");
            $("#txtTaxableAmount").addClass("valid");
        }
        else {
            $("#txtTaxableAmount").removeClass("valid");
            $("#txtTaxableAmount").addClass("required");
        }
    });

    $("#txtRetentionPercentage").keyup(function () {
        var Rate = $.trim($("#txtRetentionPercentage").val()) == "" ? 0 : parseFloat($("#txtRetentionPercentage").val());
        var Amount = $.trim($("#txtInvoiceAmount").val()) == "" ? 0 : parseFloat($("#txtInvoiceAmount").val());
        var Tax = (Rate * Amount) / 100;
        $("#txtRetentionAmount").val(Tax);
        if (Rate > 0) {
            $("#txtRetentionPercentage").removeClass("required");
            $("#txtRetentionPercentage").addClass("valid");
        }
        else {
            $("#txtRetentionPercentage").removeClass("valid");
            $("#txtRetentionPercentage").addClass("required");
        }
        if (Tax > 0) {
            $("#txtRetentionAmount").removeClass("required");
            $("#txtRetentionAmount").addClass("valid");
        }
        else {
            $("#txtRetentionAmount").removeClass("valid");
            $("#txtRetentionAmount").addClass("required");
        }
    });

    $("#txtRetentionAmount").keyup(function () {
        var Rate = $.trim($("#txtRetentionAmount").val()) == "" ? 0 : parseFloat($("#txtRetentionAmount").val());
        var Amount = $.trim($("#txtInvoiceAmount").val()) == "" ? 0 : parseFloat($("#txtInvoiceAmount").val());
        var Tax = (Rate * 100) / Amount;
        $("#txtRetentionPercentage").val(Tax.toFixed(2));
        if (Rate > 0) {
            $("#txtRetentionAmount").removeClass("required");
            $("#txtRetentionAmount").addClass("valid");
        }
        else {
            $("#txtRetentionAmount").removeClass("valid");
            $("#txtRetentionAmount").addClass("required");
        }
        if (Tax > 0) {
            $("#txtRetentionPercentage").removeClass("required");
            $("#txtRetentionPercentage").addClass("valid");
        }
        else {
            $("#txtRetentionPercentage").removeClass("valid");
            $("#txtRetentionPercentage").addClass("required");
        }
    });

    $("#txtProductCode").keyup(function (e) {
        if (e.which != 13 && e.which != 9) {
            $("#hfProductCode").val("0");
            $("#txtProductCode").removeClass("valid");
            $("#txtProductCode").addClass("required");
        }



        $("#txtProductCode").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteProductCode",
                    data: "{ 'txt' : '" + $("#txtProductCode").val() + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                label: item.split('~')[1],
                                val: item.split('~')[0]
                            }
                        }))
                    },
                    error: function (response) {
                        //alert(response.responseText);
                    },
                    failure: function (response) {
                        //alert(response.responseText);
                    }
                });
            },
            minLength: 1,
            select: function (e, i) {
                $("#hfProductCode").val(i.item.val);
                $("#txtProductCode").val(i.item.label);
                $("#txtProductCode").addClass("valid");
                $("#txtProductCode").removeClass("required");
            }
        });
    });

    $("#txtOUCode").keyup(function (e) {
        if (e.which != 13 && e.which != 9) {
            $("#hfOUCode").val("0");
            $("#txtOUCode").removeClass("valid");
            $("#txtOUCode").addClass("required");
        }

        var FCCode = $("#ddlBU").val();

        $("#txtOUCode").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteLocationCode",
                    data: "{ 'txt' : '" + $("#txtOUCode").val() + "','Code' : '" + FCCode + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                label: item.split('~')[1],
                                val: item.split('~')[0]
                            }
                        }))
                    },
                    error: function (response) {
                        //alert(response.responseText);
                    },
                    failure: function (response) {
                        //alert(response.responseText);
                    }
                });
            },
            minLength: 1,
            select: function (e, i) {
                $("#hfOUCode").val(i.item.val);
                $("#txtOUCode").val(i.item.label);
                $("#txtOUCode").addClass("valid");
                $("#txtOUCode").removeClass("required");
            }
        });
    });

    $("#ddlBU").change(function () {
        $("#txtOUCode").val("");
        $("#hfOUCode").val("0");
        $("#txtOUCode").removeClass("valid");
        $("#txtOUCode").addClass("required");
    });

    $(".attUploader").on('change', function (e) {
        var Attach_list = Attachment_list();
        var Attachment_fomat = Attached_fileformat();  //Pandiaraj 18-11-2019 
        var iframe = $('<iframe name="postiframe" id="postiframe" style="display: none"></iframe>');
        $("body").append(iframe);
        var form = $('#uploaderForm');
        //form.attr("action", UrlDet + "UploadAttachment");
        form.attr("action", UrlDet + "UploadAttachment/?attach=" + Attach_list + '&attaching_format=' + Attachment_fomat);  //Pandiaraj 18-11-2019 
        form.attr("method", "post");
        form.attr("encoding", "multipart/form-data");
        form.attr("enctype", "multipart/form-data");
        form.attr("target", "postiframe");
        form.attr("file", $('#attUploader').val());
        form.submit();
        return false;
    });

    $("#txtWHTaxRate").change(function () {
        var Rate = $.trim($("#txtWHTaxRate").val()) == "" ? 0 : parseFloat($("#txtWHTaxRate").val());
        var Amount = $.trim($("#txtWHTaxableAmount").val()) == "" ? 0 : parseFloat($("#txtWHTaxableAmount").val());
        var Tax = (Rate * Amount) / 100;
        $("#txtWHTaxAmount").val(Tax);
        $("#txtWHNetAmount").val(Amount - Tax);
        if (Rate > 0) {
            $("#txtWHTaxRate").removeClass("required");
            $("#txtWHTaxRate").addClass("valid");
        }
        else {
            $("#txtWHTaxRate").removeClass("valid");
            $("#txtWHTaxRate").addClass("required");
        }
    });

    $("#txtWHTaxableAmount").keyup(function () {
        var Rate = $.trim($("#txtWHTaxRate").val()) == "" ? 0 : parseFloat($("#txtWHTaxRate").val());
        var Amount = $.trim($("#txtWHTaxableAmount").val()) == "" ? 0 : parseFloat($("#txtWHTaxableAmount").val());
        var Tax = (Rate * Amount) / 100;
        $("#txtWHTaxAmount").val(Tax)
        $("#txtWHNetAmount").val(Amount - Tax);
        if (Amount > 0) {
            $("#txtWHTaxableAmount").removeClass("required");
            $("#txtWHTaxableAmount").addClass("valid");
        }
        else {
            $("#txtWHTaxableAmount").removeClass("valid");
            $("#txtWHTaxableAmount").addClass("required");
        }
    });

    $("#txtPAmortAmount").keyup(function () {
        var Amount = $.trim($("#txtPAmortAmount").val()) == "" ? 0 : parseFloat($("#txtPAmortAmount").val());
        if (Amount > 0) {
            $("#txtPAmortAmount").removeClass("required");
            $("#txtPAmortAmount").addClass("valid");
        }
        else {
            $("#txtPAmortAmount").removeClass("valid");
            $("#txtPAmortAmount").addClass("required");
        }
    });

    $("#ddlPAmortCycle").change(function () {
        var Cycle = $("#ddlPAmortCycle").val();
        if (Cycle != "") {
            $("#ddlPAmortCycle").removeClass("required");
            $("#ddlPAmortCycle").addClass("valid");
        }
        else {
            $("#ddlPAmortCycle").removeClass("valid");
            $("#ddlPAmortCycle").addClass("required");
        }
        GetAmortSplit();
    });

    $("#ddlPAmortGl").change(function () {
        var gl = $("#ddlPAmortGl").val();
        if (gl != "0") {
            $("#txtPAmortDesc").val(gl.split('~')[1]);
            $("#ddlPAmortGl").removeClass("required");
            $("#ddlPAmortGl").addClass("valid");
        }
        else {
            $("#txtPAmortDesc").val("");
            $("#ddlPAmortGl").removeClass("valid");
            $("#ddlPAmortGl").addClass("required");
        }
    });

    $("#txtPAmortDesc").keyup(function () {
        var Desc = $.trim($("#txtPAmortDesc").val());
        if (Desc != "") {
            $("#txtPAmortDesc").removeClass("required");
            $("#txtPAmortDesc").addClass("valid");
        }
        else {
            $("#txtPAmortDesc").removeClass("valid");
            $("#txtPAmortDesc").addClass("required");
        }
    });

    $("#fuDebitLine").on('change', function (e) {
        var iframe = $('<iframe name="postiframeDebit" id="postiframeDebit" style="display: none"></iframe>');
        $("body").append(iframe);
        var form = $('#frmDebitUploader');
        form.attr("action", UrlDet + "UploadDebitLine");
        form.attr("method", "post");
        form.attr("encoding", "multipart/form-data");
        form.attr("enctype", "multipart/form-data");
        form.attr("target", "postiframeDebit");
        form.attr("file", $('#fuDebitLine').val());
        form.submit();

        return false;
    });

    $("#txtInvoiceNo, #txtInvoiceDate, #txtInvoiceDescription, #txtWithoutTaxAmount").change(function () {
        var value = $(this).val();
        if (value != "")
            $(this).removeClass("required").addClass("valid");
        else
            $(this).removeClass("valid").addClass("required");
    });

    $("#txtInvoiceAmount").change(function () {
        var value = $.trim($(this).val()) == "" ? 0 : parseFloat($(this).val());
        if (value > 0)
            $(this).removeClass("required").addClass("valid");
        else
            $(this).removeClass("valid").addClass("required");
    });

    $("#txtGSTTaxRate").change(function () {
        var Rate = $.trim($("#txtGSTTaxRate").val()) == "" ? 0 : parseFloat($("#txtGSTTaxRate").val());
        var Amount = $.trim($("#txtGSTTaxableAmount").val()) == "" ? 0 : parseFloat($("#txtGSTTaxableAmount").val());
        var Tax = (Rate * Amount) / 100;
        $("#txtGSTTaxRate").removeClass("required").removeClass("valid");
        $("#txtGSTTaxAmount").val(Tax.toFixed(2))
        if (Rate > 0) {
            $("#txtGSTTaxRate").addClass("valid");
        } else {
            $("#txtGSTTaxRate").addClass("required");
        }
    });

    $("#txtGSTTaxableAmount").keyup(function () {
        var Rate = $.trim($("#txtGSTRate").val()) == "" ? 0 : parseFloat($("#txtGSTRate").val());
        var Amount = $.trim($("#txtGSTTaxableAmount").val()) == "" ? 0 : parseFloat($("#txtGSTTaxableAmount").val());
        var Tax = (Rate * Amount) / 100;
        $("#txtGSTTaxAmount").val(Tax.toFixed(2));

        $("#txtGSTTaxableAmount").removeClass("required").removeClass("valid");
        if (Amount > 0) {
            $("#txtGSTTaxableAmount").addClass("valid");
        } else {
            $("#txtGSTTaxableAmount").addClass("required");
        }
    });

   
});

function isEvent(evt) {
    return false;
}

function AddWHTaxAmount() {
    $('#WithholdTaxDialog').attr("style", "display:block;");

    if ($("#ddlWHTaxType").hasClass("valid")) {
        $("#ddlWHTaxType").removeClass("valid");
        $("#ddlWHTaxType").addClass("required");
    }
    if ($("#ddlWHTaxSubType").hasClass("valid")) {
        $("#ddlWHTaxSubType").removeClass("valid");
        $("#ddlWHTaxSubType").addClass("required");
    }
    if ($("#txtWHTaxRate").hasClass("valid")) {
        $("#txtWHTaxRate").removeClass("valid");
        $("#txtWHTaxRate").addClass("required");
    }

    $("#ddlWHTaxType").removeAttr("disabled");

    //$("#txtWHTaxableAmount").val($(".ChqAmount0").text());

    var Amount = $.trim($("#txtWHTaxableAmount").val()) == "" ? 0 : parseFloat($("#txtWHTaxableAmount").val());
    if (Amount > 0) {
        $("#txtWHTaxableAmount").removeClass("required");
        $("#txtWHTaxableAmount").addClass("valid");
    }
    else {
        $("#txtWHTaxableAmount").removeClass("valid");
        $("#txtWHTaxableAmount").addClass("required");
    }

    objDialogyWHTax.dialog({ title: 'With Holding Tax', width: '460', height: '320', 
        close: function(event, ui) 
    {
        //objDialogyInvoice.find("form").remove();
        objDialogyWHTax.dialog('destroy');
              
             
    } });
    $("#hfWHTaxgid").val("0");
    $("#ddlWHTaxType").val("0");
    $("#ddlWHTaxSubType").val("0");
    $("#txtWHTaxRate").val("0");
    $("#txtWHTaxableAmount").val("");
    $("#txtWHTaxAmount").val("");
    $("#txtWHNetAmount").val("");
    $("#btnWHTaxSubmit").css("display", "");
    objDialogyWHTax.dialog("open");
    return false;
}

function AddTaxAmount() {
    $('#ShowDialog').attr("style", "display:block;");

    if ($("#ddlTaxType").hasClass("valid")) {
        $("#ddlTaxType").removeClass("valid");
        $("#ddlTaxType").addClass("required");
    }
    if ($("#ddlSubTaxType").hasClass("valid")) {
        $("#ddlSubTaxType").removeClass("valid");
        $("#ddlSubTaxType").addClass("required");
    }
    if ($("#txtTaxRate").hasClass("valid")) {
        $("#txtTaxRate").removeClass("valid");
        $("#txtTaxRate").addClass("required");
    }
    $("#txtTaxableAmount").val($("#txtWithoutTaxAmount").val());
    var Amount = $.trim($("#txtTaxableAmount").val()) == "" ? 0 : parseFloat($("#txtTaxableAmount").val());
    if (Amount > 0) {
        $("#txtTaxableAmount").removeClass("required");
        $("#txtTaxableAmount").addClass("valid");
    }
    else {
        $("#txtTaxableAmount").removeClass("valid");
        $("#txtTaxableAmount").addClass("required");
    }

    objDialogyAdd.dialog({ title: 'Invoice Tax', width: '460', height: '290',close: function(event, ui) 
    {
        //objDialogyInvoice.find("form").remove();
        objDialogyAdd.dialog('destroy');
              
             
    }  });
    $("#hfInvoiceTaxgid").val("0");
    $("#ddlTaxType").val("0");
    $("#ddlSubTaxType").val("0");
    $("#txtTaxRate").val("0");
    $("#txtTaxAmount").val("");
    $("#btnTaxSubmit").css("display", "");
    objDialogyAdd.dialog("open");
    return false;
}

function AddGSTTaxAmount() {
    $('#GSTDialog').attr("style", "display:block;");

    $("#hfGSTInvoiceTaxGid,#ddlGSTHSNCode").val("0");
    $("#chkGSTgsnapplicability").prop("checked", true);
    $("#txtGSTHSNDesc,#txtGSTTaxableAmount,#txtGSTTaxAmount").val("");
    $("#ddlGSTTaxType,#ddlGSTSubTaxType,#txtGSTRate").val("0");

    $("#ddlGSTHSNCode,#txtGSTTaxableAmount,#txtGSTTaxAmount,#txtGSTRate,#ddlGSTTaxType,#ddlGSTSubTaxType").removeClass("required").removeClass("valid");
    $("#ddlGSTHSNCode,#txtGSTTaxAmount,#txtGSTRate,#ddlGSTTaxType,#ddlGSTSubTaxType").addClass("required");

    $("#txtGSTTaxableAmount").val($("#txtInvoiceAmount").val());
    var Amount = $.trim($("#txtGSTTaxableAmount").val()) == "" ? 0 : parseFloat($("#txtGSTTaxableAmount").val());
    if (Amount > 0) {
        $("#txtGSTTaxableAmount").addClass("valid");
    } else {
        $("#txtGSTTaxableAmount").addClass("required");
    }

    $("#btnGSTTaxSubmit").css("display", "");
    objDialogyGST.dialog({ title: 'GST Invoice Tax', width: '460', height: '400',close: function(event, ui) 
    {
        //objDialogyInvoice.find("form").remove();
        objDialogyGST.dialog('destroy');
              
             
    }  });
    objDialogyGST.dialog("open");
    return false;
}

function AddDebitAmount() {
    //Ramya Aug
    var GSTCharged = $("#hdfEditViewGstChargedFlag").val();
    if (GSTCharged == "Y") {
        //$("#rdorcmChargedYes").prop("disabled", true);
        $("input[name=rdorcmChargedFlag][value=N]").attr("disabled", true);
        $("input[name=rdorcmChargedFlag][value=Y]").attr("disabled", true);
        //$("#rdorcmChargedNo").prop("disabled", true);
        $("#rdorcmChargedNo").prop("checked", true);
    }
    else {
        $("input[name=rdorcmChargedFlag][value=N]").attr("disabled", false);
        $("input[name=rdorcmChargedFlag][value=Y]").attr("disabled", false);
        $("#rdorcmChargedNo").prop("checked", true);
    }
    $('#ShowDialog1').attr("style", "display:block;");

    if ($("#ddlExpenses").hasClass("valid")) {
        $("#ddlExpenses").removeClass("valid");
        $("#ddlExpenses").addClass("required");
    }
    if ($("#ddlCategory").hasClass("valid")) {
        $("#ddlCategory").removeClass("valid");
        $("#ddlCategory").addClass("required");
    }
    if ($("#ddlSubCategory").hasClass("valid")) {
        $("#ddlSubCategory").removeClass("valid");
        $("#ddlSubCategory").addClass("required");
    }


    if ($("#ddlhsngid").hasClass("valid")) {
        $("#ddlhsngid").removeClass("valid");
        $("#ddlhsngid").addClass("required");
    }


    if ($("#hfProductCodeText").val() != "" && $("#hfEmpProductCode").val() != "0") {
        $("#txtProductCode").removeClass("required");
        $("#txtProductCode").addClass("valid");
    }
    else {
        $("#txtProductCode").removeClass("valid");
        $("#txtProductCode").addClass("required");
    }
    if ($("#hfOUCodeText").val() != "" && $("#hfEmpOUCode").val() != "0") {
        $("#txtOUCode").removeClass("required");
        $("#txtOUCode").addClass("valid");
    }
    else {
        $("#txtOUCode").removeClass("valid");
        $("#txtOUCode").addClass("required");
    }
    if ($("#txtDebitAmount").hasClass("valid")) {
        $("#txtDebitAmount").removeClass("valid");
        $("#txtDebitAmount").addClass("required");
    }

    objDialogyAddDebit.dialog({
        title: 'Debit Line', width: '800', height: '400', close: function (event, ui) {
            //objDialogyInvoice.find("form").remove();
            objDialogyAddDebit.dialog('destroy');


        }
    });
    $("#hfDebitlineGId").val("0");
    $("#ddlExpenses").val("0");
    $("#ddlCategory").val("0");
    $("#ddlSubCategory").val("0");
    $("#ddlBU").val($("#hfBU").val());
    $("#ddlCC").val($("#hfCC").val());
    $("#txtProductCode").val($("#hfProductCodeText").val());
    $("#txtOUCode").val($("#hfOUCodeText").val());
    $("#hfProductCode").val($("#hfEmpProductCode").val());
    $("#hfOUCode").val($("#hfEmpOUCode").val());
    $("#txtDebitAmount").val("");
    $("#txtDebitDescription").val("");
    $("#btnDebitLineSubmit").css("display", "");

    $("#ddlhsngid").val("0");
    $("#ddlhsngid").empty();
    $("#ddlCategory").empty();
    $("#ddlSubCategory").empty();

    objDialogyAddDebit.dialog("open");
    return false;
}

function AddPaymentAmount() {
    $('#ShowDialog2').attr("style", "display:block;");

    if ($("#ddlPayMode").hasClass("valid")) {
        $("#ddlPayMode").removeClass("valid");
        $("#ddlPayMode").addClass("required");
    }
    if ($("#txtPayRefNo").hasClass("valid")) {
        $("#txtPayRefNo").removeClass("valid");
        $("#txtPayRefNo").addClass("required");
    }
    if ($("#txtPaymentAmount").hasClass("valid")) {
        $("#txtPaymentAmount").removeClass("valid");
        $("#txtPaymentAmount").addClass("required");
    }
    if ($("#txtPayDescription").hasClass("valid")) {
        $("#txtPayDescription").removeClass("valid");
        $("#txtPayDescription").addClass("required");
    }

    $("#ddlPayMode").removeAttr("disabled");
    $("#txtPayRefNo").attr("disabled");


    objDialogyAddPayment.dialog({ title: 'Payment Details', width: '560', height: '320',close: function(event, ui) 
    {
        //objDialogyInvoice.find("form").remove();
        objDialogyAddPayment.dialog('destroy');
              
             
    }  });
    $("#hfCreditlineGId").val("0");
    $("#hfRefId").val("0");
    $("#ddlPayMode,#ddlPayBank").val("0");
    $("#txtPayRefNo").val("");
    $("#txtPayBeneficiary").val($("#txtSupplierName").text());
    $("#txtPaymentAmount").val("");
    $("#txtPayDescription").val("");
    $("#hfIFSCCode").val("");

    objDialogyAddPayment.dialog("open");
    return false;
}

function AddAttachment(flag) {
    $('#ShowDialog3').attr("style", "display:block;");

    if ($("#ddlAttachmentType").hasClass("valid")) {
        $("#ddlAttachmentType").removeClass("valid");
        $("#ddlAttachmentType").addClass("required");
    }
    if ($("#txtAttachDescription").hasClass("valid")) {
        $("#txtAttachDescription").removeClass("valid");
        $("#txtAttachDescription").addClass("required");
    }

    objDialogyAddAttachment.dialog({ title: 'Attachment Details', width: '560', height: '300',close: function(event, ui) 
    {
        //objDialogyInvoice.find("form").remove();
        objDialogyAddAttachment.dialog('destroy');
              
             
    }  });
    $("#attUploader").replaceWith($("#attUploader").val('').clone(true));
    $("#ddlAttachmentType").val("0");
    $("#txtAttachDescription").val("");
    $("#hfAttachFlag").val(flag);
    objDialogyAddAttachment.dialog("open");
    return false;
}

function ScheduleAmort() {

    var AmortFrom = $("#txtAmortFrom").val();
    var AmortTo = $("#txtAmortTo").val();
    var AmortDesc = $("#txtAmortDescription").val();

    if ($.trim(AmortFrom) == "") {
        jAlert("Ensure Amort From!", "Message");
        return false;
    }
    if ($.trim(AmortTo) == "") {
        jAlert("Ensure Amort To!", "Message");
        return false;
    }
    if ($.trim(AmortDesc) == "") {
        jAlert("Ensure Amort Description!", "Message");
        return false;
    }
    $("#txtPAmortDateFrom").val($("#txtAmortFrom").val());
    $("#txtPAmortDateTo").val($("#txtAmortTo").val());
    $("#txtPAmortAmount").val($("#txtECFAmount").text());
    $("#txtPAmortDesc").val($("#txtAmortDescription").val());
    //$("#txtPAmortDateTo").datepicker("option", "minDate", $("#txtAmortFrom").val());
    //$("#txtPAmortDateFrom").datepicker("option", "maxDate", $("#txtAmortTo").val());

    if ($("#txtPAmortDateFrom").val() == "") {
        $("#txtPAmortDateFrom").removeClass("valid");
        $("#txtPAmortDateFrom").addClass("required");
    }
    else {
        $("#txtPAmortDateFrom").addClass("valid");
        $("#txtPAmortDateFrom").removeClass("required");
    }

    if ($("#txtPAmortDateTo").val() == "") {
        $("#txtPAmortDateTo").removeClass("valid");
        $("#txtPAmortDateTo").addClass("required");
    }
    else {
        $("#txtPAmortDateTo").addClass("valid");
        $("#txtPAmortDateTo").removeClass("required");
    }
    if ($("#ddlPAmortCycle").val() == "") {
        $("#ddlPAmortCycle").removeClass("valid");
        $("#ddlPAmortCycle").addClass("required");
    }
    else {
        $("#ddlPAmortCycle").addClass("valid");
        $("#ddlPAmortCycle").removeClass("required");
    }
    if ($("#txtPAmortAmount").val() == "" || parseFloat($("#txtPAmortAmount").val()) == 0) {
        $("#txtPAmortAmount").removeClass("valid");
        $("#txtPAmortAmount").addClass("required");
    }
    else {
        $("#txtPAmortAmount").addClass("valid");
        $("#txtPAmortAmount").removeClass("required");
    }
    if ($("#ddlPAmortGl").val() == "0") {
        $("#ddlPAmortGl").removeClass("valid");
        $("#ddlPAmortGl").addClass("required");
    }
    else {
        $("#ddlPAmortGl").addClass("valid");
        $("#ddlPAmortGl").removeClass("required");
    }
    if ($("#txtPAmortDesc").val() == "") {
        $("#txtPAmortDesc").removeClass("valid");
        $("#txtPAmortDesc").addClass("required");
    }
    else {
        $("#txtPAmortDesc").addClass("valid");
        $("#txtPAmortDesc").removeClass("required");
    }

    $('#AmortDialog').attr("style", "display:block;");

    objDialogyAmort.dialog({ title: 'Amort Schedule', width: '900', height: '550',close: function(event, ui) 
    {
        //objDialogyInvoice.find("form").remove();
        objDialogyAmort.dialog('destroy');
              
             
    }  });
    objDialogyAmort.dialog("open");
    return false;
}

function GetAmortSplit() {

    var AmortFrom = $("#txtPAmortDateFrom").val();
    var AmortTo = $("#txtPAmortDateTo").val();
    var Cycle = $("#ddlPAmortCycle").val();
    if ($.trim(AmortFrom) == "" || $.trim(AmortTo) == "" || $.trim(Cycle) == "") {
        return false;
    }

    var AmortSchedule = {
        amortgid: "0",
        ecfid: "0",
        supplierId: "0",
        amount: "0",
        glno: "",
        startdate: AmortFrom,
        enddate: AmortTo,
        frequencygid: Cycle,
        split: '0',
        active: "Y",
        isremoved: "0"
    };

    $.ajax({
        type: "post",
        url: UrlDet + "GetAmortSplit",
        data: JSON.stringify(AmortSchedule),
        contentType: "application/json;",
        success: function (response) {
            var Data1 = "";
            if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                Data1 = JSON.parse(response.Data1);
                $("#txtPAmortSplit").val(Data1[0].Split)
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //alert(errorThrown);
        }
    });

}
