var objDialogyAddDebit, objDialogyAddPayment, objDialogyAddAttachment, objDialogyGridDebitLine, objDialogyWHTax, objDialogyAddPayment1, objDialogyBenificiary;
var objDialogyGST;
UrlDet = UrlDet.replace("GetSupplierInvoiceMaker", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");

var Months = ["", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
var SearchModel = function (id) {
    var self = this;


    self.ECFExpenseArray = ko.observableArray();
    self.ECFPaymentArray = ko.observableArray();
    self.ECFAttachmentArray = ko.observableArray();
    self.ECFAuditrailArray = ko.observableArray();
    self.GSTHSNCodeArray = ko.observableArray();
    self.GSTTaxTypeArray = ko.observableArray();
    self.GSTSubTaxTypeArray = ko.observableArray();
    self.InvoiceExpenseArray = ko.observableArray();
    self.GSTInvoiceTaxArray = ko.observableArray();
    self.RCMInvoiceTaxArray = ko.observableArray();
    //self.TaxTypeArray = ko.observableArray();
    self.ExpenseArray = ko.observableArray();
    self.CategoryArray = ko.observableArray();
    self.SubCategoryArray = ko.observableArray();
    self.BusinessSegmentArray = ko.observableArray();
    self.CostCenterArray = ko.observableArray();
    self.PayModeArray = ko.observableArray();
    self.AttachmentTypeArray = ko.observableArray();
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

    self.BenificiaryArray = ko.observableArray();
    //Hsn codedropdown loading array //ramya
    self.HsnCodeArray = ko.observableArray();
    self.DatatableCall = function () {
        if ($("#gvPouchInward > tbody > tr").length == self.BenificiaryArray().length) {
            $("#gvPouchInward").DataTable({
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

    self.ShortDescription = function (data) {
        if (data != null && data.length > 45) {
            data = data.substring(0, 45) + '...';
        } return data;
    };

    this.RunDedup = function () {
        var _tmpData = {
            EcfId: $("#hfECFId").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "RunDedupDSA",
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
        var downloadUrl = UrlDet + "DownloadDedupDSAData/" + EcfId;
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
            url: UrlDet + "GetDSASupplierInvoice",
            data: JSON.stringify(EcfFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "", Data3 = "", Data4 = "", Data5 = "", Data6 = "", Data7 = "", Data8 = "", Data9 = "", Data10 = "", Data11 = "";
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
                self.InvoiceHoldingTaxArray(Data7);

                if (response.Data8 != null && response.Data8 != "" && JSON.parse(response.Data8) != null)
                    Data8 = JSON.parse(response.Data8);
                self.EOWInexArray(Data8);

                if (response.Data9 != null && response.Data9 != "" && JSON.parse(response.Data9) != null)
                    Data9 = JSON.parse(response.Data9);
                self.EOWDespatchArray(Data9);

                if (response.Data10 != null && response.Data10 != "" && JSON.parse(response.Data10) != null)
                    Data10 = JSON.parse(response.Data10);
                self.EOWPaymentArray(Data10);

                if (response.Data11 != null && response.Data11 != "" && JSON.parse(response.Data11) != null)
                    Data11 = JSON.parse(response.Data11);
                self.PaymentDespatchArray(Data11);
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
                    else
                        jAlert(Data1[0].Msg, "Message");
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

    this.ViewInvoiceDetails = function (InvId) {
        $("#hfInvId").val(InvId);

        var InputFilter = {
            InvId: InvId
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetDSAInvoiceDetails",
            data: JSON.stringify(InputFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.InvoiceExpenseArray(Data1);

                try {
                    debugger;
                    if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                        Data2 = JSON.parse(response.Data2);
                        self.GSTInvoiceTaxArray(Data2);
                        self.RCMInvoiceTaxArray("");
                        $('#itabs-10').attr("style", "display:block;");
                        $('#itabs-1').attr("style", "display:none;");
                    }
                    else {
                        $('#itabs-10').attr("style", "display:none;");
                        self.GSTInvoiceTaxArray("");
                    }
                }
                catch (e) {
                    $('#itabs-10').attr("style", "display:none;");
                    self.GSTInvoiceTaxArray("");
                }
                try {
                    debugger;
                    if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null) {
                        Data3 = JSON.parse(response.Data3);
                        self.RCMInvoiceTaxArray(Data3);
                        self.GSTInvoiceTaxArray("");
                        $('#itabs-10').attr("style", "display:none;");
                        $('#itabs-1').attr("style", "display:block;");
                    }
                    else {
                        $('#itabs-1').attr("style", "display:none;");
                        self.RCMInvoiceTaxArray("");
                    }
                }
                catch (e) {
                    $('#itabs-1').attr("style", "display:none;");
                    self.RCMInvoiceTaxArray("");
                }
                $('#ShowDialog4').attr("style", "display:block;");
                objDialogyGridDebitLine.dialog({ title: 'View Expense Details', width: '1000', height: '400' });
                objDialogyGridDebitLine.dialog("open");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
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
        var hsngid = $("#ddlhsngid").val();

        var rcm = $("input[name=rdorcmChargedFlag]:radio[checked=checked]").attr("value");
        if (expnaturegid == "0") {
            jAlert("Ensure Expense Nature!", "Error");
            return false;
        }

        if (expCatId == "0") {
            jAlert("Ensure Expense Category!", "Error");
            return false;
        }

        if (expSubcatId == "0") {
            jAlert("Ensure Expense SubCategory!", "Error");
            return false;
        }

        if (OUCode == "0") {
            jAlert("Ensure Location Code(OUCode)!", "Error");
            return false;
        }

        if (ProductCode == "0") {
            jAlert("Ensure Ensure Product Code!", "Error");
            return false;
        }

        if (Amount == "0") {
            jAlert("Ensure Amount Greater than Zero!", "Error");
            return false;
        }

        if (hsngid == "0") {
            jAlert("Ensure hsn details", "Error");
            return false;
        }

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
            rcm: rcm
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetDebitLineDetails",
            data: JSON.stringify(InvoiceDebitLine),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == false)
                        jAlert(Data1[0].Msg, "Error");
                    else {
                        objDialogyAddDebit.dialog("close");
                        jAlert(Data1[0].Msg, "Message");
                    }
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                self.InvoiceExpenseArray(Data2);

                if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null) {
                    Data3 = JSON.parse(response.Data3);
                    self.GSTInvoiceTaxArray(Data3);
                }

                if (response.Data4 != null && response.Data4 != "" && JSON.parse(response.Data4) != null) {
                    Data4 = JSON.parse(response.Data4);
                    self.RCMInvoiceTaxArray(Data4);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.DeleteInvoiceDebitDetails = function (DebitlineGId) {

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
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == false)
                        jAlert(Data1[0].Msg, "Error");
                    else {
                        objDialogyAddDebit.dialog("close");
                        jAlert(Data1[0].Msg, "Message");
                    }
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                self.InvoiceExpenseArray(Data2);

                if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null)
                    Data3 = JSON.parse(response.Data3);
                self.GSTInvoiceTaxArray(Data3);

                if (response.Data4 != null && response.Data4 != "" && JSON.parse(response.Data4) != null)
                    Data4 = JSON.parse(response.Data4);
                self.RCMInvoiceTaxArray(Data4);
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

        if (paymode == "0") {
            jAlert("Ensure Payment Mode!", "Message");
            return false;
        }
        if ($.trim(RefNo) == "") {
            if (paymode == "DD")
                jAlert("Ensure Payable At!", "Message");
            else
                jAlert("Ensure Account No!", "Message");
            return false;
        }
        if ($.trim(Beneficiary) == "" && paymode != "PPX" && paymode != "SUS") {
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

    this.CloseDetails = function (flag) {
        if (flag == 1)
            objDialogyAddDebit.dialog("close");
        if (flag == 2)
            objDialogyAddPayment.dialog("close");
        if (flag == 3)
            objDialogyAddAttachment.dialog("close");
        if (flag == 4)
            objDialogyGridDebitLine.dialog("close");
        if (flag == 5)
            objDialogyAddPayment1.dialog("close");
        if (flag == 7)
            objDialogyWHTax.dialog("close");
        if (flag == 13)
            objDialogyGST.dialog("close");
    };

    this.EditInvoiceDebitDetails = function (Index, flag) {
        debugger;
        var _tmpData = self.InvoiceExpenseArray()[Index];

        $("#hfDebitlineGId").val(_tmpData.GId);
        $("#hfInvId").val(_tmpData.invid);
        $("#ddlExpenses").val(_tmpData.expnaturegid);
        self.LoadExpenseCategory();
        $("#ddlCategory").val(_tmpData.catgid);
        self.LoadExpenseSubCategory();
        $("#ddlSubCategory").val(_tmpData.subcatgid);
        $("#ddlBU").val(_tmpData.fccode);
        $("#ddlCC").val(_tmpData.cccode);
        $("#txtProductCode").val(_tmpData.Product);
        $("#txtOUCode").val(_tmpData.location);
        $("#hfProductCode").val(_tmpData.ProdCode);
        $("#hfOUCode").val(_tmpData.OUCode);
        $("#txtDebitAmount").val(_tmpData.Amt);
        $("#txtDebitDescription").val(_tmpData.ddesc);
        self.LoadHsnArray();
        $("#ddlhsngid").val(_tmpData.hsngid); //Ramya
        var rcm = _tmpData.rcm;
        $("#hfSupplierId").val(_tmpData.SupplierId);
        //Ramya Aug

        //var _value = rcm;
        //$("input[name=rdorcmChargedFlag][value=" + _value + "]").prop("checked", true);
        $("#hdfEditViewGstChargedFlag").val(_tmpData.GSTCharged);

        var GSTCharged = $("#hdfEditViewGstChargedFlag").val();
        if (GSTCharged == "Y") {
            $("input[name=rdorcmChargedFlag][value=N]").attr("disabled", true);
            $("input[name=rdorcmChargedFlag][value=Y]").attr("disabled", true);
            $("input[name=rdorcmChargedFlag][value=N]").attr("checked", true);
        }
        else {
            if (rcm == 'Yes') {
                $("input[name=rdorcmChargedFlag][value=Y]").attr("checked", true);
            }
            else {
                $("input[name=rdorcmChargedFlag][value=N]").attr("checked", true);
            }
            $("input[name=rdorcmChargedFlag][value=N]").attr("disabled", false);
            $("input[name=rdorcmChargedFlag][value=Y]").attr("disabled", false);
        }
       

        if ($("#ddlExpenses").val() == "0") {
            $("#ddlExpenses").removeClass("valid");
            $("#ddlExpenses").addClass("required");
        }
        else {
            $("#ddlExpenses").addClass("valid");
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

        if ($("#ddlhsngid").val() == "0") {
            $("#ddlhsngid").removeClass("valid");
            $("#ddlhsngid").addClass("required");
        }
        else {
            $("#ddlhsngid").addClass("valid");
            $("#ddlhsngid").removeClass("required");
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
        $("#ddlhsngid").val(_tmpData.hsngid);
        if (flag == 0)
            $("#btnDebitLineSubmit").css("display", "none");
        else
            $("#btnDebitLineSubmit").css("display", "");
        objDialogyAddDebit.dialog({ title: 'Expense Details', width: '800', height: '400' });
        objDialogyAddDebit.dialog("open");
    };

    this.EditInvoiceCreditDetails = function (Index, flag) {
        var _tmpData = self.ECFPaymentArray()[Index];
        $("#hfIFSCCode").val(_tmpData.IfscCode);
        $("#hfSupplierId").val(_tmpData.supplierGId);
        if (_tmpData.PMode == "DD") {
            $("#txtPayRefNo1").removeAttr("disabled");
        }
        if (_tmpData.PMode == "EFT" || _tmpData.PMode == "CHQ" || _tmpData.PMode == "ERA" || _tmpData.PMode == "DD" || _tmpData.PMode == "RRP") {
            $("#hfCreditlineGId").val(_tmpData.CrditLineGID);
            $("#hfInvId").val(_tmpData.InvId);
            $("#hfRefId").val(_tmpData.InvId);
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
        self.LoadExpenseCategory();
        self.LoadExpenseSubCategory();
    };

    this.ChangeExpencesCategory = function () {
        self.LoadExpenseSubCategory();
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
        debugger;
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
            //var ProviderLocation = $("#ddlgstProviderLoc").val();
            var ProviderLocation = 0;
            var GstinVendor = $("#txtGSTINVendor").val();

            //if ((parseInt(ProviderLocation) == 0 || $.trim(ProviderLocation) == "")) {
            //    jAlert("Ensure Provider Location!", "Message");
            //    return false;
            //}

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
        //var potype = $("#hfISPO").val();
        var potype = "";
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
                    if (parseInt($("#ddlhsngid").val()) == 0 || $("#ddlhsngid").val() == "" || $("#ddlhsngid").val() == null) {
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
        $("#hfSupplierId").val(item.SupplierId);
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
        objDialogyGST.dialog({
            title: 'GST Invoice Tax', width: '460', height: '400', close: function (event, ui) {
                //objDialogyInvoice.find("form").remove();
                objDialogyGST.dialog('destroy');


            }
        });
        objDialogyGST.dialog("open");
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

    //With Holding Tax Start
    this.SetWithHoldTaxDetails = function () {

        var withholdtaxgid = $("#hfWHTaxgid").val();
        var Invoicegid = $("#hfECFId").val();
        var TaxId = $("#ddlWHTaxType").val();
        var taxsubtypegid = $("#ddlWHTaxSubType").val();
        var TaxRate = $("#txtWHTaxRate").val();
        var TaxableAmt = $.trim($("#txtWHTaxableAmount").val()) == "" ? 0 : parseFloat($("#txtWHTaxableAmount").val());
        var TaxAmount = $.trim($("#txtWHTaxAmount").val()) == "" ? 0 : parseFloat($("#txtWHTaxAmount").val());
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
        if (withholdtaxgid != "0") {
            if ($.trim(TaxableAmt) == "" || parseFloat(TaxableAmt) == 0) {
                jAlert("Ensure Taxable Amount!", "Message");
                return false;
            }
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
            url: UrlDet + "SetWithHoldingTaxDSA",
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

        $("#txtWHTaxableAmount").removeAttr("disabled");
        $("#txtWHNetAmount").removeAttr("disabled");

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
                    url: UrlDet + "SetWithHoldingTaxDSA",
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
        if ($("#ddlWHTaxSubType").hasClass("valid")) {
            $("#ddlWHTaxSubType").removeClass("valid");
            $("#ddlWHTaxSubType").addClass("required");
        }
        self.LoadWHTaxSubType();

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
        //$("#txtWHTaxRate").removeAttr("disabled");
        //var TaxId = $("#ddlWHTaxType").val();
        //var SId = $("#hfSupplierId").val();
        //if (TaxId == "0")
        //    return false;
        //var InputFilter = {
        //    TaxId: TaxId,
        //    SId: SId
        //};
        //$.ajax({
        //    type: "post",
        //    url: UrlDet + "GetTaxRate",
        //    data: JSON.stringify(InputFilter),
        //    async: false,
        //    contentType: "application/json;",
        //    success: function (response) {
        //        var Data1 = "";
        //        if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
        //            Data1 = JSON.parse(response.Data1);
        //        if (Data1[0].Clear == 2) {
        //            $("#txtWHTaxRate").val(Data1[0].taxrate);
        //            //$("#txtTaxRate").attr("disabled", "disabled");
        //            if ($("#txtWHTaxRate").hasClass("required")) {
        //                $("#txtWHTaxRate").removeClass("required");
        //                $("#txtWHTaxRate").addClass("valid");
        //            }
        //        }
        //        else if (Data1[0].Clear == 1) {
        //            $("#txtWHTaxRate").removeAttr("disabled");
        //            jConfirm(Data1[0].Msg, "Confirm", function (callback) {
        //                if (callback == true) {
        //                } else {
        //                    $("#ddlWHTaxType").val("0");
        //                    if ($("#ddlWHTaxType").hasClass("valid")) {
        //                        $("#ddlWHTaxType").removeClass("valid");
        //                        $("#ddlWHTaxType").addClass("required");
        //                    }
        //                    return false;
        //                }
        //            });
        //        }
        //        else {
        //            $("#ddlWHTaxType").val("0");
        //            $("#txtWHTaxRate").val("0");
        //            jAlert(Data1[0].Msg, "Message");
        //        }
        //    },
        //    error: function (XMLHttpRequest, textStatus, errorThrown) {
        //        //alert(errorThrown);
        //    }
        //});

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
                var Data1 = "";
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
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.TaxRateArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.DownloadFormat = function () {
        window.location = UrlDef + "/Template/DSAWithHoldingTax.xls";


    };

    this.UploadDebitLine = function () {

        var ecfid = $("#hfECFId").val();

        var InputFilter = {
            ecfid: ecfid
        };

        $.ajax({
            type: "post",
            url: UrlDet + "UploadDSAWithHoldingTaxExcel",
            data: JSON.stringify(InputFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "", Data3 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == false) {
                        $("#fuDebitLine").replaceWith($("#fuDebitLine").val('').clone(true));
                        jAlert(Data1[0].Msg, "Message");
                    }
                    else {
                        $("#fuDebitLine").replaceWith($("#fuDebitLine").val('').clone(true));
                        jAlert(Data1[0].Msg, "Message");

                        if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                            Data2 = JSON.parse(response.Data2);
                        self.ECFPaymentArray(Data2);
                        if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null)
                            Data3 = JSON.parse(response.Data3);
                        self.InvoiceHoldingTaxArray(Data3);
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });

    };

    this.DownloadFormat1 = function () {
        window.location = UrlDef + "/Template/DSADebitline.xlsx";

    };

    this.UploadDebitLine1 = function () {

        var ECFId = $("#hfECFId").val();
        var _sheetName = $("#ddlDSASheet").val();

        if (_sheetName == "0") {
            jAlert("Ensure Sheet Name.", "Message");
            return false;
        }

        var InputFilter = {
            ECFId: ECFId,
            SheetName: _sheetName
        };

        $.ajax({
            type: "post",
            url: UrlDet + "UploadDSAdebitlineExcel",
            data: JSON.stringify(InputFilter),
            contentType: "application/json;",
            success: function (response) {
                $("#ddlDSASheet").html("");
                $("#ddlDSASheet").append("<option value='0' >--Select--</option>");
                var Data1 = "", Data2 = "", Data3 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == "false" || Data1[0].Clear == false || Data1[0].Clear == "0") {
                        $("#fuDebitLine1").replaceWith($("#fuDebitLine1").val('').clone(true));
                        jAlert(Data1[0].Message, "Message");
                    }
                    else {
                        jAlert(Data1[0].Message, "Message");
                        $("#fuDebitLine1").replaceWith($("#fuDebitLine1").val('').clone(true));
                        if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                            Data2 = JSON.parse(response.Data2);
                        self.ECFExpenseArray(Data2);
                        if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null)
                            Data3 = JSON.parse(response.Data3);
                        self.ECFPaymentArray(Data3);
                    }
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
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
    this.ResetDSA = function () {

        jConfirm("Are you sure? Want to Clear All data!", "Confirm", function (callback) {
            if (callback == true) {
                var ECFId = $("#hfECFId").val();

                var InputFilter = {
                    ECFId: ECFId
                };

                $.ajax({
                    type: "post",
                    url: UrlDet + "SetDSAReset",
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
                                self.ECFExpenseArray("");
                                self.ECFPaymentArray("");
                                self.InvoiceHoldingTaxArray("");
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
        //var INVAmount = $.trim($("#txtInvoiceAmount").val()) == "" ? 0 : parseFloat($("#txtInvoiceAmount").val());
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

        //if (INVAmount < (TaxableAmt + TaxAmount) && parseFloat(TaxRate) != 100) {
        //    jAlert("Taxable Amount + Tax Amount should not exceeded Invoice Amount!", "Message");
        //    return false;
        //}

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
    self.LoadTaxRate();
    self.LoadWHTaxType();
    self.LoadWHTaxSubType();
    //With Holding Tax End 
    self.LoadGSTTaxType();
    self.LoadCurrencyType();

    self.LoadBusinessSegment();
    self.LoadCostCenter();
    self.LoadPayMode();
    self.LoadAttachmentType();

    self.LoadNatureOfExpenses();
    self.LoadExpenseCategory();
    self.LoadExpenseSubCategory();

    self.LoadECFDetails();

    self.LoadPayModeCommon();
    self.LoadPayBank();
    
    self.ECFAuditHistory = ko.observableArray();
    self.LoadHsnArray();
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

                if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null) {
                    Data3 = JSON.parse(response.Data3);
                }
                if (response.Data7 != null && response.Data7 != "" && JSON.parse(response.Data7) != null) {
                    Data7 = JSON.parse(response.Data7);
                }



                self.ECFPaymentHistory(Data1);
                self.InvoiceHoldingTaxHistory(Data2);
                self.InvoiceDebitHistory(Data3);
                self.ECFInvoiceHeader(Data7);

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };



    self.LoadAuditHistory();




};

var mainViewModel = new SearchModel();
ko.applyBindings(mainViewModel, document.getElementById("nontravelclaim"));

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
        height: 400,
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

    objDialogyGridDebitLine = $("[id$='ShowDialog4']");
    objDialogyGridDebitLine.dialog({
        autoOpen: false,
        modal: true,
        width: 1000,
        height: 400,
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

    objDialogyGST = $("[id$='GSTDialog']");
    objDialogyGST.dialog({
        autoOpen: false,
        modal: true,
        width: 460,
        height: 290,
        duration: 300,
        close: function (event, ui) {
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

    $("#txtClaimFrom").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        maxDate: 'd',
        onSelect: function (selected) {
            $("#txtClaimFrom").addClass('valid');
            $("#txtClaimTo").datepicker("option", "minDate", selected)
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
            //kavitha
            $("#txtPayRefNo1").removeAttr("disabled");
            $("#lblPayableAt").text("Payable At");
            //$("input").prop('disabled', false);
        }
        else {
            $("#txtPayRefNo1").attr("disabled");
            $("#lblPayableAt").text("Account No");
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

    $("#attUploader").on('change', function (e) {
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

    $("#fuDebitLine").on('change', function (e) {
        var iframe = $('<iframe name="postiframeDebit" id="postiframeDebit" style="display: none"></iframe>');
        $("body").append(iframe);
        var form = $('#frmDebitUploader');
        form.attr("action", UrlDet + "UploadDSAWithHoldingTax");
        form.attr("method", "post");
        form.attr("encoding", "multipart/form-data");
        form.attr("enctype", "multipart/form-data");
        form.attr("target", "postiframeDebit");
        form.attr("file", $('#fuDebitLine').val());
        form.submit();

        return false;
    });

    $("frmDebitUploader1").bind('ajax:complete', function () {
        alert("Test");
    });

    $("#fuDebitLine1").on('change', function (e) {
        var iframe = $('<iframe name="postiframeDebit1" id="postiframeDebit1" style="display: none"></iframe>');
        $("body").append(iframe);
        var form = $('#frmDebitUploader1');
        form.attr("action", UrlDet + "UploadDSADebitline");
        form.attr("method", "post");
        form.attr("encoding", "multipart/form-data");
        form.attr("enctype", "multipart/form-data");
        form.attr("target", "postiframeDebit1");
        form.attr("file", $('#fuDebitLine1').val());
        form.submit();

        setTimeout(function () {
            $.ajax({
                type: "post",
                url: UrlDet + "FillDSADebitLineSheets",
                data: "{}",
                contentType: "application/json;",
                success: function (response) {
                    var Data1 = "", tmpStr = "";
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                        Data1 = JSON.parse(response.Data1);

                    $("#ddlDSASheet").html("");
                    if (Data1.length > 0) {
                        $("#ddlDSASheet").append("<option value='0' >--Select--</option>");
                        for (var i = 0; i < Data1.length; i++) {
                            tmpStr = Data1[i].TABLE_NAME.replace("$", "");
                            tmpStr = tmpStr.replace("'", "")
                            $("#ddlDSASheet").append("<option value=" + Data1[i].TABLE_NAME + " >" + tmpStr.replace("'", "") + "</option>");
                        }
                    }
                    else {
                        $("#ddlDSASheet").append("<option value='0' >--Select--</option>");
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //alert(errorThrown);
                }
            });
        }, 7000);

        //Fill the dropdown content

        return false;
    });



    $("#drpaudit").change(function () {
        //alert('Changed');
        if ($('#drpaudit').val() == 1) {
            $("#showpaymentdialog").css("display", "block");
            $("#showAuditworktray").css("display", "none");
            $("#showDebitline").css("display", "none");
            $("#InvoiceHeader").css("display", "none");
            $("#showExpensehistory").css("display", "none");

        }
        else if ($('#drpaudit').val() == 2) {
            $("#showAuditworktray").css("display", "block");
            $("#showpaymentdialog").css("display", "none");
            $("#showDebitline").css("display", "none");
            $("#InvoiceHeader").css("display", "none");
            $("#showExpensehistory").css("display", "none");

        }

        else if ($('#drpaudit').val() == 3) {
            $("#showDebitline").css("display", "block");
            $("#showAuditworktray").css("display", "none");
            $("#showpaymentdialog").css("display", "none");
            $("#InvoiceHeader").css("display", "none");
            $("#showExpensehistory").css("display", "none");

        }
        else if ($('#drpaudit').val() == 4) {
            $("#InvoiceHeader").css("display", "block");
            $("#showDebitline").css("display", "none");
            $("#showAuditworktray").css("display", "none");
            $("#showpaymentdialog").css("display", "none");
            $("#showExpensehistory").css("display", "none");

        }
        else if ($('#drpaudit').val() == 5) {
            $("#InvoiceHeader").css("display", "none");
            $("#showDebitline").css("display", "none");
            $("#showAuditworktray").css("display", "none");
            $("#showpaymentdialog").css("display", "none");
            $("#showExpensehistory").css("display", "block");

        }

        else {
            $("#InvoiceHeader").css("display", "none");
            $("#showAuditworktray").css("display", "none");
            $("#showpaymentdialog").css("display", "none");
            $("#showDebitline").css("display", "none");
            $("#showExpensehistory").css("display", "none");
        }

    });



});

function isEvent(evt) {
    return false;
}

function AddDebitAmount() {
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

    objDialogyAddDebit.dialog({ title: 'Expense Details', width: '800', height: '400' });
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
    $("#txtExpenseClaimMonth").val("");
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

    $("#txtWHTaxableAmount").removeClass("required");
    $("#txtWHTaxableAmount").attr("disabled", "disabled");
    $("#txtWHNetAmount").attr("disabled", "disabled");

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


