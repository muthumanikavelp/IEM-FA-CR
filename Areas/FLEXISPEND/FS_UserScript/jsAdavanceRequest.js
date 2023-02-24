var objDialogyAddDebit, objDialogyAddPayment, objDialogyAddAttachment, objDialogyWHTax, objDialogyAddPayment1, objDialogySUS, objDialogyBenificiary;
var Months = ["", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
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

    self.ExpenseArray = ko.observableArray();
    self.CategoryArray = ko.observableArray();
    self.SubCategoryArray = ko.observableArray();
    self.BusinessSegmentArray = ko.observableArray();
    self.CostCenterArray = ko.observableArray();
    self.PayModeArray = ko.observableArray();
    self.AttachmentTypeArray = ko.observableArray();
    self.AdvanceTypeArray = ko.observableArray();
    self.CurrencyArray = ko.observableArray();

    self.InvoiceHoldingTaxArray = ko.observableArray();
    self.WHTaxTypeArray = ko.observableArray();
    self.WHTaxSubTypeArray = ko.observableArray();
    self.TaxRateArray = ko.observableArray();

    self.EOWInexArray = ko.observableArray();
    self.EOWDespatchArray = ko.observableArray();
    self.EOWPaymentArray = ko.observableArray();
    self.PaymentDespatchArray = ko.observableArray();

    self.PayBankArray = ko.observableArray();
    self.PayModeCommonArray = ko.observableArray();
    self.DefaultPayBank = ko.observable("0");

    self.SUSArray = ko.observableArray();

    self.BenificiaryArray = ko.observableArray();

    //START --Run Dedup
    self.DedupDetailsArray = ko.observableArray();

    this.RunDedup = function () {
        var _tmpData = {
            EcfId: $("#hfECFId").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "RunDedupARF",
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

    this.DedupDownload = function () {
        var EcfId = parseInt($("#hfECFId").val());
        var downloadUrl = UrlDet + "DownloadDedupARFData/" + EcfId;
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
            url: UrlDet + "GetAdvanceRequest",
            data: JSON.stringify(EcfFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "", Data3 = "", Data4 = "", Data5 = "", Data6 = "", Data7 = "", Data8 = "", Data9 = "", Data10 = "", Data11 = "", Data12 = "";
                
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                if (Data2.length > 0) {
                    
                    $("#txtECFNumber").text(Data2[0].ARFNo);
                    $("#txtECFDate").text(Data2[0].ARFDate);
                    $("#txtECFAmount").text(Data2[0].ARFAmount);
                    $("#lblRaiserMode").text(Data2[0].RaiserMode);
                    $("#lblRaiserCode").text(Data2[0].raiserId);
                    $("#lblRaiserName").text(Data2[0].raiserName);
                    $("#txtARFType").text(Data2[0].ARFTtype);
                    $("#txtSECode").text(Data2[0].code);
                    $("#txtSEName").text(Data2[0].Name);
                    $("#txtClaimMonth").text(Data2[0].ClaimMonth);

                    $("#txtExchangeRate").val(Data2[0].ExchangeRate);
                    $("#ddlCurrency").val(Data2[0].CrnID);
                    $("#txtCurrencyAmount").val(Data2[0].CurrencyAmount);
                    $("#txtReducedAmount").val(Data2[0].ReducedAmt);
                    $("#txtProcessedAmount").val(Data2[0].ProcessedAmt);
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
                    $("#txtECFDescription").val(Data2[0].EDesc);

                    $("#hfBU").val(Data2[0].fc);
                    $("#hfCC").val(Data2[0].cc);
                    $("#hfEmpProductCode").val(Data2[0].productcode);
                    $("#hfEmpOUCode").val(Data2[0].oucode);
                    $("#hfProductCodeText").val(Data2[0].Product);
                    $("#hfOUCodeText").val(Data2[0].location);
                    $("#hfInvId").val(Data2[0].Invid);

                    $("#hfSupplierId").val(Data2[0].SupplierGId);

                    if (Data2[0].docsubtypegid == "6") {
                        self.LoadPayMode("1");
                        $("#lblSECode").text("Employee Code");
                        $("#lblSEName").text("Employee Name");
                        $("#changableARF").css("display", "none");
                        $("#disablePaymode").css("display", "none");
                    }
                    else {
                        self.LoadPayMode("0");
                        $("#lblSECode").text("Supplier Code");
                        $("#lblSEName").text("Supplier Name");
                        $("#changableARF").css("display", "");
                        $("#showSupplierARF").css("display", "");
                    }

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
                ////------------- Shrinidhi 03.05.2016------------------------
                //BenName = Data4;
                /////---------------------------
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
                self.InvoiceHoldingTaxArray(Data10);

                if (response.Data11 != null && response.Data11 != "" && JSON.parse(response.Data11) != null)
                    Data11 = JSON.parse(response.Data11);
                self.PaymentDespatchArray(Data11);
                
                ////------------- Shrinidhi 03.05.2016------------------------
                //BenName = Data4;
                if (response.Data12 != null && response.Data12 != "" && JSON.parse(response.Data12) != null) {
                    Data12 = JSON.parse(response.Data12);
                    BenName = Data12;
                }
                /////---------------------------            
               
               
               
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
    };

    this.DeleteAttachmentDetails = function (AttachmentId) {

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
        form.attr("method", "post");
        form.attr("encoding", "multipart/form-data");
        form.attr("enctype", "multipart/form-data");
        form.attr("target", "postiframe");
        form.attr("file", $('#attUploader').val());
        form.submit();
    };

    this.SetInvoiceDebitDetails = function () {

        var ecfartgid = $("#hfDebitlineGId").val();
        var ECFId = $("#hfECFId").val();
        var advancetypegid = $("#ddlAdvanceType").val();
        var promoinvoice = $("#txtPromoInvoice").val();
        var pono = $("#txtPONo").val();
        var cbfno = $("#txtCBFNo").val();
        var CCCode = $("#ddlCC").val();
        var FCCode = $("#ddlBU").val();
        var OUCode = $("#hfOUCode").val();;
        var ProductCode = $("#hfProductCode").val();;
        var Amount = $("#txtDebitAmount").val();;
        var liQdate = $("#txtLiquidationDate").val();;
        var desc = $("#txtDebitDescription").val();
        var IsRemoved = "0";

        if (advancetypegid == "0") {
            jAlert("Ensure Advance Type!", "Message");
            return false;
        }

        if ($.trim(liQdate) == "") {
            jAlert("Ensure Liquidation Date!", "Message");
            return false;
        }

        if (OUCode == "0") {
            jAlert("Ensure Location Code(OUCode)!", "Message");
            return false;
        }

        if (ProductCode == "0") {
            jAlert("Ensure Product Code!", "Message");
            return false;
        }

        if ($.trim(Amount) == "" || parseFloat(Amount) == 0) {
            jAlert("Ensure Amount!", "Message");
            return false;
        }

        if ($.trim(desc) == "") {
            jAlert("Ensure Liquidation Date!", "Message");
            return false;
        }

        var ARFDebit = {
            ecfartgid: ecfartgid,
            ECFId: ECFId,
            advancetypegid: advancetypegid,
            promoinvoice: promoinvoice,
            pono: pono,
            cbfno: cbfno,
            CCCode: CCCode,
            FCCode: FCCode,
            OUCode: OUCode,
            ProductCode: ProductCode,
            Amount: Amount,
            liQdate: liQdate,
            desc: desc,
            IsRemoved: IsRemoved
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetARFDebit",
            data: JSON.stringify(ARFDebit),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == false)
                        jAlert(Data1[0].Msg, "Message");
                    else {
                        $('#hfISDedup').val("0");
                        objDialogyAddDebit.dialog("close");
                        jAlert(Data1[0].Msg, "Message");
                    }
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                self.ECFExpenseArray(Data2);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.DeleteInvoiceDebitDetails = function (ecfartgid) {

        var ECFId = $("#hfECFId").val();
        var advancetypegid = "0";
        var promoinvoice = "";
        var pono = "";
        var cbfno = "";
        var CCCode = "";
        var FCCode = "";
        var OUCode = "";
        var ProductCode = "";
        var Amount = "0";
        var liQdate = "";
        var desc = "";
        var IsRemoved = "1";        

        var ARFDebit = {
            ecfartgid: ecfartgid,
            ECFId: ECFId,
            advancetypegid: advancetypegid,
            promoinvoice: promoinvoice,
            pono: pono,
            cbfno: cbfno,
            CCCode: CCCode,
            FCCode: FCCode,
            OUCode: OUCode,
            ProductCode: ProductCode,
            Amount: Amount,
            liQdate: liQdate,
            desc: desc,
            IsRemoved: IsRemoved
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetARFDebit",
            data: JSON.stringify(ARFDebit),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == false)
                        jAlert(Data1[0].Msg, "Message");
                    else {
                        $('#hfISDedup').val("0");
                        objDialogyAddDebit.dialog("close");
                        jAlert(Data1[0].Msg, "Message");
                    }
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                self.ECFExpenseArray(Data2);
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
       

        if (flag == "1") {
            paymode = $("#ddlPayMode1 option:selected").text();
            PayBank = $("#ddlPayBank").val();
            RefNo = $("#txtPayRefNo1").val();
            Beneficiary = $("#txtPayBeneficiary1").val();
            Amount = $("#txtPaymentAmount1").val();
            Desc = $("#txtPayDescription1").val();
        }
        else {
            paymode = $("#ddlPayMode option:selected").text();
            PayBank = "0";
            RefNo = $("#txtPayRefNo").val();
            Beneficiary = $("#txtPayBeneficiary").val();
            Amount = $("#txtPaymentAmount").val();
            Desc = $("#txtPayDescription").val();
        }
        var IsRemoved = "0";

        if (paymode == "0" || paymode == "-- Select One --") {
            jAlert("Ensure Payment Mode!", "Message");
            return false;
        }
        if (paymode != "CHQ") {

            if ($.trim(RefNo) == "") {
                if (paymode == "DD")
                    jAlert("Ensure Payable At!", "Message");
                else
                    jAlert("Ensure Account No!", "Message");
                return false;
            }
        }
        //if ($.trim(Beneficiary) == "") {
        //    jAlert("Ensure Beneficiary Names!", "Message");
        //    return false;
        //}

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
            return false;s
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
            IfscCode: IfscCode,
            
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
                self.ECFPaymentArray(Data2);

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
                        self.ECFPaymentArray(Data2);
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

    this.CloseDetails = function (flag) {
        if (flag == 1)
            objDialogyAddDebit.dialog("close");
        if (flag == 2)
            objDialogyAddPayment.dialog("close");
        if (flag == 3)
            objDialogyAddAttachment.dialog("close");
        if (flag == 4)
            objDialogyWHTax.dialog("close");
        if (flag == 5)
            objDialogyAddPayment1.dialog("close");
        if (flag == 10)
            objDialogySUS.dialog("close");
    };

    this.EditInvoiceDebitDetails = function (Index, flag) {
        var _tmpData = self.ECFExpenseArray()[Index];

        $("#hfDebitlineGId").val(_tmpData.ecfarfgid);
        $("#ddlAdvanceType").val(_tmpData.advancetypegid);
        $("#txtPromoInvoice").val(_tmpData.PromoInvoice);
        $("#txtPONo").val(_tmpData.pono);
        $("#txtCBFNo").val(_tmpData.CBFNo);
        $("#txtLiquidationDate").val(_tmpData.LiqDate);
        $("#ddlBU").val(_tmpData.fc);
        $("#ddlCC").val(_tmpData.cc);
        $("#txtProductCode").val(_tmpData.product);
        $("#txtOUCode").val(_tmpData.location);
        $("#hfProductCode").val(_tmpData.productcode);
        $("#hfOUCode").val(_tmpData.oucode);
        $("#txtDebitAmount").val(_tmpData.Amount);
        $("#txtDebitDescription").val(_tmpData.ecfarfDesc);

        if ($("#ddlAdvanceType").val() == "0") {
            $("#ddlAdvanceType").removeClass("valid");
            $("#ddlAdvanceType").addClass("required");
        }
        else {
            $("#ddlAdvanceType").addClass("valid");
            $("#ddlAdvanceType").removeClass("required");
        }
        if ($("#txtLiquidationDate").val() == "") {
            $("#txtLiquidationDate").removeClass("valid");
            $("#txtLiquidationDate").addClass("required");
        }
        else {
            $("#txtLiquidationDate").addClass("valid");
            $("#txtLiquidationDate").removeClass("required");
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

        if ($("#txtDebitDescription").val() == "") {
            $("#txtDebitDescription").removeClass("valid");
            $("#txtDebitDescription").addClass("required");
        }
        else {
            $("#txtDebitDescription").addClass("valid");
            $("#txtDebitDescription").removeClass("required");
        }

        if (flag == 0)
            $("#btnDebitLineSubmit").css("display", "none");
        else
            $("#btnDebitLineSubmit").css("display", "");
        objDialogyAddDebit.dialog({ title: 'Advance Entry', width: '800', height: '450' });
        objDialogyAddDebit.dialog("open");
    };

    this.EditInvoiceCreditDetails = function (Index, flag) {
        var _tmpData = self.ECFPaymentArray()[Index];
        $("#hfIFSCCode").val(_tmpData.IfscCode);
        if (_tmpData.PMode == "DD") {
            $("#txtPayRefNo1").removeAttr("disabled");
        }
        if (_tmpData.PMode == "EFT" || _tmpData.PMode == "CHQ" || _tmpData.PMode == "ERA" || _tmpData.PMode == "DD") {
            $("#hfCreditlineGId").val(_tmpData.CrditLineGID);
            //$("#hfInvId").val(_tmpData.InvId);
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

            if (flag == 0) {
                $("#btnCreditLineSubmit").css("display", "none");
            }
            else
                $("#btnCreditLineSubmit").css("display", "");
            objDialogyAddPayment.dialog({ title: 'Payment Details', width: '560', height: '320' });
            objDialogyAddPayment.dialog("open");
        }
    };

    this.LoadAdvanceType = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetAdvanceType",
            data: "{}",
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.AdvanceTypeArray(Data1);
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

    this.LoadPayMode = function (flag) {
        var MUrl = "GetARFSupplierPayMode";
        if (flag = "1")
            MUrl = "GetARFEmployeePayMode";
        $.ajax({
            type: "post",
            url: UrlHelper + MUrl,
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
        self.LoadExpenseCategory();
        self.LoadExpenseSubCategory();
    };

    this.ChangeExpencesCategory = function () {
        self.LoadExpenseSubCategory();
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

    self.LoadCurrencyType();

    //With Holding Tax Start
    this.SetWithHoldTaxDetails = function () {

        var withholdtaxgid = $("#hfWHTaxgid").val();
        var Invoicegid = $("#hfECFId").val();
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
            url: UrlDet + "SetWithHoldingTaxARF",
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
                self.ECFPaymentArray(Data3);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.EditWithHoldTaxDetails = function (Index, flag) {
        var _tmpData = self.InvoiceHoldingTaxArray()[Index];

        $("#hfWHTaxgid").val(_tmpData.withholdtaxgid);
        //$("#hfInvId").val(_tmpData.invoicegid);
        $("#ddlWHTaxType").val(_tmpData.TaxId);
        self.LoadWHTaxSubType();
        $("#ddlWHTaxSubType").val(_tmpData.subtypegid);
        $("#txtWHTaxRate").val(_tmpData.Rate);
        $("#txtWHTaxableAmount").val(_tmpData.TaxableAmt);
        $("#txtWHTaxAmount").val(_tmpData.TaxAmount);
        $("#txtWHNetAmount").val(_tmpData.NetAmt);

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
        objDialogyWHTax.dialog({ title: 'With Holding Tax', width: '460', height: '290' });
        objDialogyWHTax.dialog("open");
    };

    this.DeleteWithHoldTaxDetails = function (withholdtaxgid) {

        jConfirm("Are you sure? Want to delete With Holding Tax!", "Confirm", function (callback) {
            if (callback == true) {
                var Invoicegid = $("#hfECFId").val();
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
                    url: UrlDet + "SetWithHoldingTaxARF",
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
                        self.ECFPaymentArray(Data3);
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
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.WHTaxSubTypeArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {

            }
        });
    };

    this.ChangeWHTaxType = function () {

        //SHRINIDHI ----26072016
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

       // $("#txtWHTaxableAmount").val("");
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
                var Data1 = "", Data2 = "", Data3 = "";// alert("4");
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);

                if (Data1[0].Clear == 2) {
                  //  alert(Data1[0].taxrate)
                    $("#txtWHTaxRate").val(Data1[0].taxrate);
                    $("#txtWHTaxRate").removeAttr("disabled");
                    if ($("#txtWHTaxRate").hasClass("required")) {
                        $("#txtWHTaxRate").removeClass("required");
                        $("#txtWHTaxRate").addClass("valid");

                        if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null)
                            Data3 = response.Data3;
                        // alert(Data3)
                        $("#ddlWHTaxSubType").val(Data3);
                        if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                            Data2 = response.Data2;
                     //  alert(Data2)
                      //  $("#txtWHTaxRate").val(Data2);
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

    this.ChangeWHTaxSubType = function () {

        if ($("#txtWHTaxRate").hasClass("valid")) {
            $("#txtWHTaxRate").removeClass("valid");
            $("#txtWHTaxRate").addClass("required");
        }
        if ($("#txtTaxableAmount").hasClass("valid")) {
            $("#txtTaxableAmount").removeClass("valid");
            $("#txtTaxableAmount").addClass("required");
        }

        $("#txtWHTaxableAmount").val("");
        $("#txtWHTaxAmount").val("");
        $("#txtWHNetAmount").val("");
        $("#txtWHTaxRate").val("0");

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
                var Data1 = "" ;// alert("15");
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                $("#txtWHTaxRate").val(Data1[0].taxrate);

               
                if (parseFloat($("#txtWHTaxRate").val()) == 0) {
                    $("#txtWHTaxRate").removeClass("valid");
                    $("#txtWHTaxRate").addClass("required");
                }
                else {
                    $("#txtWHTaxRate").removeClass("required");
                    $("#txtWHTaxRate").addClass("valid");
                }

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
                var Data1 = "", Data2 = "", Data3 = ""; //alert("16");
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.TaxRateArray(Data1);
                
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

    this.FetchPPXPaymentRefNo = function () {
        var SupplierId = $("#hfSupplierId").val();
        SupplierId = "1";
        var type = $("#ddlPayMode option:selected").text();
        var ViewType = "0";
        if ($.trim(type) == "PPX") {
            ViewType = "4"
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
                if (ViewType == "4") {
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
                if ($.trim(Data1[0].Message) == "") {
                    if (ViewType == "4") {
                        $('#PPXDialog').attr("style", "display:block;");
                        objDialogyPPX.dialog({ title: 'Supplier PPX Details', width: '900', height: '400' });
                        objDialogyPPX.dialog("open");
                    }
                    else if (ViewType == "1") {
                        $('#CreditNoteDialog').attr("style", "display:block;");
                        objDialogyCreditNote.dialog({ title: 'Supplier Credit Note Details', width: '900', height: '400' });
                        objDialogyCreditNote.dialog("open");
                    }
                    else if (ViewType == "2") {
                        $("#txtSrcGLNo").val("");

                        $('#SUSDialog').attr("style", "display:block;");
                        objDialogySUS.dialog({ title: 'Employee SUS Details', width: '900', height: '550' });
                        objDialogySUS.dialog("open");
                    }
                    else if (ViewType == "3") {
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
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.BenificiaryArray(Data1);
                $('#BenificiaryDialog').attr("style", "display:block;");
                objDialogyBenificiary.dialog({ title: 'Benificiary Details', width: '700', height: '400' });
                objDialogyBenificiary.dialog("open");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
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

    self.LoadTaxRate();
    self.LoadWHTaxType();
    self.LoadWHTaxSubType();
    //With Holding Tax End

    self.LoadBusinessSegment();
    self.LoadCostCenter();
    //self.LoadPayMode();
    self.LoadAttachmentType();
    self.LoadAdvanceType();

    self.LoadECFDetails();
    //self.LoadNatureOfExpenses();
    //self.LoadExpenseCategory();
    //self.LoadExpenseSubCategory();

    self.loadSUSGrid();

    self.LoadPayModeCommon();
    self.LoadPayBank();

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
    self.InvoiceHoldingTaxHistory = ko.observableArray();
    self.Advance_History = ko.observableArray();
     
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
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                }

                if (response.Data5 != null && response.Data5 != "" && JSON.parse(response.Data5) != null) {
                    Data5 = JSON.parse(response.Data5);
                }
             

                self.ECFPaymentHistory(Data1);
                self.InvoiceHoldingTaxHistory(Data2);
                self.Advance_History(Data5);     

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };
    self.LoadAuditHistory();
};

var mainViewModel = new SearchModel();
ko.applyBindings(mainViewModel, document.getElementById("divArfDetails"));

$(document).ready(function () {

    objDialogyBenificiary = $("[id$='BenificiaryDialog']");
    objDialogyBenificiary.dialog({
        autoOpen: false,
        modal: true,
        width: 700,
        height: 400,
        duration: 300
    });

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

    objDialogyWHTax = $("[id$='WithholdTaxDialog']");
    objDialogyWHTax.dialog({
        autoOpen: false,
        modal: true,
        width: 460,
        height: 300,
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

    objDialogySUS = $("[id$='SUSDialog']");
    objDialogySUS.dialog({
        autoOpen: false,
        modal: true,
        width: 600,
        height: 550
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

    $("#txtLiquidationDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        minDate: 'd',
        onSelect: function (selected) {
            $("#txtLiquidationDate").addClass('valid');
        }
    });

    $("#txtClaimTo").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        maxDate: 'd',
        onSelect: function (selected) {
            var month = selected.split("/")[1];
            $("#txtExpenseClaimMonth").val(Months[parseInt(month)] + " " + selected.split("/")[2]);
            $("#txtClaimTo").addClass('valid');
            $("#txtClaimFrom").datepicker("option", "maxDate", selected)
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

    $("#ddlAdvanceType").change(function () {
        var AdvanceType = $("#ddlAdvanceType").val();
        if (AdvanceType != "0") {
            $("#ddlAdvanceType").removeClass("required");
            $("#ddlAdvanceType").addClass("valid");
        }
        else {
            $("#ddlAdvanceType").removeClass("valid");
            $("#ddlAdvanceType").addClass("required");
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

    //$("#ddlPayMode").change(function () {
    //    var PayMode = $("#ddlPayMode").val();
    //    if (PayMode != "0") {
    //        $("#ddlPayMode").removeClass("required");
    //        $("#ddlPayMode").addClass("valid");
    //    }
    //    else {
    //        $("#ddlPayMode").removeClass("valid");
    //        $("#ddlPayMode").addClass("required");
    //    }
    //});

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
        else if (PayMode == "SUS") {
           $("#txtPayBeneficiary").val(BenName[0].Beneficiary);
        }
        else
            $("#txtPayRefNo").removeClass("valid").addClass("required");
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
        $("#txtPayRefNo1").attr("disabled");
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
            //$("input").prop('disabled', false);
        }
        else {
            $("#txtPayRefNo1").attr("disabled");
            $("#lblPayableAt").text("Account No");
           // $("input").prop('disabled', true);
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

    $("#txtDebitDescription").keyup(function () {
        var Desc = $.trim($("#txtDebitDescription").val());
        if (Desc != "") {
            $("#txtDebitDescription").removeClass("required");
            $("#txtDebitDescription").addClass("valid");
        }
        else {
            $("#txtDebitDescription").removeClass("valid");
            $("#txtDebitDescription").addClass("required");
        }
    });

    $("#txtProductCode").keyup(function (e) {
        if (e.which != 13) {
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
        if (e.which != 13) {
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

    //With Holding Tax Start
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

    $("#txtWHTaxRate").change(function () {
        var Rate = $.trim($("#txtWHTaxRate").val()) == "" ? 0 : parseFloat($("#txtWHTaxRate").val());
        var Amount = $.trim($("#txtWHTaxableAmount").val()) == "" ? 0 : parseFloat($("#txtWHTaxableAmount").val());
        var Tax = ((Rate * Amount) / 100).toFixed(0);
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
        var Tax = ((Rate * Amount) / 100).toFixed(0);
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
    //With Holding Tax End

    $("#drpaudit").change(function () {
        //alert('Changed');
        if ($('#drpaudit').val() == 1) {
            $("#showpaymentdialog").css("display", "block");
            $("#showAuditworktray").css("display", "none");
            $("#AdvanceHistory").css("display", "none");
             
        }
        else if ($('#drpaudit').val() == 2) {
            $("#showpaymentdialog").css("display", "none");
            $("#showAuditworktray").css("display", "block");
            $("#AdvanceHistory").css("display", "none");
        }

        else if ($('#drpaudit').val() == 3) {
            $("#showpaymentdialog").css("display", "none");
            $("#showAuditworktray").css("display", "none");
            $("#AdvanceHistory").css("display", "block");
        }
        
        else {
            $("#showpaymentdialog").css("display", "none");
            $("#showAuditworktray").css("display", "none");
            $("#AdvanceHistory").css("display", "none");
        }

    });




});

function isEvent(evt) {
    return false;
}

function AddDebitAmount() {
    $('#ShowDialog1').attr("style", "display:block;");

    if ($("#ddlAdvanceType").hasClass("valid")) {
        $("#ddlAdvanceType").removeClass("valid");
        $("#ddlAdvanceType").addClass("required");
    }
    if ($("#txtLiquidationDate").hasClass("valid")) {
        $("#txtLiquidationDate").removeClass("valid");
        $("#txtLiquidationDate").addClass("required");
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

    if ($("#txtDebitDescription").hasClass("valid")) {
        $("#txtDebitDescription").removeClass("valid");
        $("#txtDebitDescription").addClass("required");
    }

    objDialogyAddDebit.dialog({ title: 'Advance Entry', width: '800', height: '450' });
    $("#hfDebitlineGId").val("0");
    $("#ddlAdvanceType").val("0");
    $("#ddlBU").val($("#hfBU").val());
    $("#ddlCC").val($("#hfCC").val());
    $("#txtProductCode").val($("#hfProductCodeText").val());
    $("#txtOUCode").val($("#hfOUCodeText").val());
    $("#hfProductCode").val($("#hfEmpProductCode").val());
    $("#hfOUCode").val($("#hfEmpOUCode").val());
    $("#txtDebitAmount").val("");
    $("#txtDebitDescription").val("");
    $("#txtLiquidationDate").val("");
    $("#txtPromoInvoice").val("");
    $("#txtPONo").val("");
    $("#txtCBFNo").val("");
    $("#btnDebitLineSubmit").css("display", "");
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

    objDialogyAddPayment.dialog({ title: 'Payment Details', width: '560', height: '330' });
    $("#hfCreditlineGId").val("0");
    $("#ddlPayMode").val("0");
    $("#txtPayRefNo").val("");
    $("#txtPayBeneficiary").val($("#txtSupplierName").val());
    $("#txtPaymentAmount").val("");
    $("#txtPayDescription").val("");
    $("#hfIFSCCode").val("");

    objDialogyAddPayment.dialog("open");
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
    if ($("#txtWHTaxableAmount").hasClass("valid")) {
        $("#txtWHTaxableAmount").removeClass("valid");
        $("#txtWHTaxableAmount").addClass("required");
    }

    objDialogyWHTax.dialog({ title: 'With Holding Tax', width: '460', height: '290' });
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


