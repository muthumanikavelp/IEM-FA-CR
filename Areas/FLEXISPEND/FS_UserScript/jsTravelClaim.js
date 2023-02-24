var objDialogyAddDebit, objDialogyAddPayment, objDialogyAddAttachment, objDialogyPPX, objDialogyAddPayment1;
var objDialogyInvoiceTravel;
var objDialogyGSTTravel;
var currencyid;
var Months = ["", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

UrlDet = UrlDet.replace("GetSupplierInvoiceMaker", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");
//------------- Shrinidhi 03.05.2016------------------------
var BenName = "";
///---------------------------
var SearchModel = function (id) {
    var self = this;

    self.ECFTravelArray = ko.observableArray();
    self.ECFPaymentArray = ko.observableArray();
    self.ECFEmployeeListArray = ko.observableArray();
    self.ECFAttachmentArray = ko.observableArray();
    self.ECFAuditrailArray = ko.observableArray();

    //GOPI
    self.ECFInvoiceArray = ko.observableArray();
    self.GSTInvoiceTaxArray = ko.observableArray();
    self.RCMInvoiceTaxArray = ko.observableArray();

    self.ProviderLocationArray = ko.observableArray();
    self.ReceiverLocationArray = ko.observableArray();

    self.GSTHSNCodeArray = ko.observableArray();
    self.GSTTaxTypeArray = ko.observableArray();
    self.GSTSubTaxTypeArray = ko.observableArray();
    self.TaxRateArray = ko.observableArray();
    self.GSTTotalTaxAmt = ko.observable(0.0);
    self.RCMTotalAmt = ko.observable(0.0);
    //GOPI

    self.ExpenseArray = ko.observableArray();
    self.CategoryArray = ko.observableArray();
    self.SubCategoryArray = ko.observableArray();
    self.BusinessSegmentArray = ko.observableArray();
    self.CostCenterArray = ko.observableArray();
    self.PayModeArray = ko.observableArray();
    self.AttachmentTypeArray = ko.observableArray();
    self.CurrencyArray = ko.observableArray();

    self.PlaceArray = ko.observableArray();
    self.TravelModeArray = ko.observableArray();
    self.TravelClassArray = ko.observableArray();

    self.PPXArray = ko.observableArray();
    self.SUSArray = ko.observableArray();

    self.EOWInexArray = ko.observableArray();
    self.EOWDespatchArray = ko.observableArray();
    self.EOWPaymentArray = ko.observableArray();
    self.PaymentDespatchArray = ko.observableArray();

    self.PayBankArray = ko.observableArray();
    self.PayModeCommonArray = ko.observableArray();
    self.DefaultPayBank = ko.observable("0");

    //Hsn codedropdown loading array /kavitha 06-03-18
    self.HsnCodeArray = ko.observableArray();

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
            url: UrlDet + "RunDedupEmployee",
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
        var downloadUrl = UrlDet + "DownloadDedupEmployeeData/" + EcfId;
        ko.utils.postJson(downloadUrl);
    };
    //END --Run Dedup

    //Gopi -- START Invoice Details
    self.ShortDescription = function (data) {
        if (data != null && data.length > 45) {
            data = data.substring(0, 45) + '...';
        } return data;
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

    self.LoadTaxType = function () {
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
                self.GSTTaxTypeArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.LoadTaxRate = function () {
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

    self.GSTChangeTaxType = function () {
        self.GSTLoadTaxSubType();

        $("#ddlGSTTaxType,#ddlGSTSubTaxType,#txtGSTTaxRate,#txtGSTTaxableAmount,#txtGSTTaxAmount").removeClass("valid").removeClass("required");
        $("#txtGSTTaxRate,#txtGSTTaxableAmount").addClass("required");
        $("#txtGSTTaxAmount").val("");
        $("#txtGSTTaxRate").val("0");
        // $("#txtGSTTaxRate").attr("disabled", true);

        var TaxId = $("#ddlGSTTaxType").val();
        var SubTaxgid = $("#ddlGSTSubTaxType").val();
        var SId = $("#hfSupplierId").val();
        if (TaxId == "0") {
            $("#ddlGSTTaxType,#ddlGSTSubTaxType,#txtGSTTaxAmount").addClass("required");
            return false;
        } else { $("#ddlGSTTaxType").addClass("valid"); $("#ddlGSTSubTaxType").addClass("required"); }
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

                $("#ddlGSTTaxType").removeClass("valid").removeClass("required");
                $("#ddlGSTTaxType").addClass("valid");
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);

                /*if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = response.Data2;*/
                if (Data1[0].Clear == 2) {
                    $("#txtGSTTaxRate").val(Data1[0].taxrate);
                    if (Data1[0].taxrate != "0" && Data1[0].taxrate != "") {
                        $("#txtGSTTaxRate").addClass("valid");
                    }
                }
                else if (Data1[0].Clear == 1) {
                    $("#txtGSTTaxRate").val(Data1[0].taxrate);
                    if (Data1[0].taxrate != "0" && Data1[0].taxrate != "") {
                        $("#txtGSTTaxRate").addClass("valid");
                    }
                    //$("#txtGSTTaxRate").removeAttr("disabled");
                    //jConfirm(Data1[0].Msg, "Confirm", function (callback) {
                    //    if (callback == true) {
                    //    } else {
                    //        $("#ddlGSTTaxType").val("0");
                    //        $("#ddlGSTTaxType").removeClass("valid").removeClass("required");
                    //        $("#ddlGSTTaxType").addClass("valid");
                    //        return false;
                    //    }
                    //});
                } else {
                    $("#ddlGSTTaxType").val("0");
                    $("#txtGSTTaxRate").val("0");
                    $("#ddlGSTTaxType").removeClass("valid").removeClass("required");
                    $("#ddlGSTTaxType").addClass("required");
                    jAlert(Data1[0].Msg, "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });

        var Rate = $.trim($("#txtGSTTaxRate").val()) == "" ? 0 : parseFloat($("#txtGSTTaxRate").val());
        var Amount = $.trim($("#txtGSTTaxableAmount").val()) == "" ? 0 : parseFloat($("#txtGSTTaxableAmount").val());
        var Tax = (Rate * Amount) / 100;
        $("#txtGSTTaxAmount").val(Tax.toFixed(2));
        $("#txtGSTTaxableAmount").removeClass("required").removeClass("valid");
        if (Amount > 0) {
            $("#txtGSTTaxableAmount").addClass("valid");
        } else {
            $("#txtGSTTaxableAmount").addClass("required");
        }

        if (Tax >= 0) {
            $("#txtGSTTaxAmount").addClass("valid");
        } else {
            $("#txtGSTTaxAmount").addClass("required");
        }
    };

    self.GSTChangeSubTaxType = function () {
        $("#ddlGSTSubTaxType").removeClass("valid").removeClass("required");
        if ($("#ddlGSTSubTaxType").val() == "0") {
            $("#ddlGSTSubTaxType").addClass("required");
        } else {
            $("#ddlGSTSubTaxType").addClass("valid");
        }

        var TaxId = $("#ddlGSTTaxType").val();
        var SubTaxgid = $("#ddlGSTSubTaxType").val();
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
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    $("#txtGSTTaxRate").val(Data1[0].taxrate).removeClass("required");
                    $("#txtGSTTaxRate").removeClass("valid");
                    if ($("#txtGSTTaxRate").val() == "" || parseFloat($("#txtGSTTaxRate").val()) == 0) {
                        $("#txtGSTTaxRate").addClass("required");
                    } else {
                        $("#txtGSTTaxRate").addClass("valid");
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });

        var Rate = $.trim($("#txtGSTTaxRate").val()) == "" ? 0 : parseFloat($("#txtGSTTaxRate").val());
        var Amount = $.trim($("#txtGSTTaxableAmount").val()) == "" ? 0 : parseFloat($("#txtGSTTaxableAmount").val());
        var Tax = (Rate * Amount) / 100;
        $("#txtGSTTaxAmount").val(Tax.toFixed(2));
        $("#txtGSTTaxableAmount").removeClass("required").removeClass("valid");
        if (Amount > 0) {
            $("#txtGSTTaxableAmount").addClass("valid");
        } else {
            $("#txtGSTTaxableAmount").addClass("required");
        }
        if (SubTaxgid == "0") {
            $("#txtGSTTaxRate").removeClass("required").removeClass("valid");
            $("#txtGSTTaxRate").addClass("required");
        }
        if (Tax >= 0) {
            $("#txtGSTTaxAmount").addClass("valid");
        } else {
            $("#txtGSTTaxAmount").addClass("required");
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

    self.LoadGSTINCategory = function () {
        var _Filter = {
            ViewType: "4",
            RefId: "0"
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

    self.AddInvoiceDetails = function () {
        $('#divInvoiceTravel').attr("style", "display:block;");
        objDialogyInvoiceTravel.dialog({ title: 'Add Invoice Details', width: '1100', height: '650' });
        objDialogyInvoiceTravel.dialog("open");

        $("#ddlgstReceiverLoc,#ddlgstProviderLoc,#hdfSVendorId,#hdfSVendorName").val("0");
        $("#txtInvoiceDate,#txtInvoiceNo,#txtServiceMonth,#txtInvoiceAmount,#txtWithoutTaxAmount,#txtInvoiceDescription,#txtVendorName,#txtVendorCode,#txtGSTINVendor,#txtGSTINFICCL").val("");

        $("input[name=rdogstChargedFlag][value=N]").prop("checked", true);
        $("#hdfEditViewGstChargedFlag").val("N");

        $("input[name=rdoVerify][value=1]").prop("checked", true);

        $("#txtGSTINVendor,#txtInvoiceDate,#txtInvoiceNo,#txtServiceMonth,#txtInvoiceAmount,#txtWithoutTaxAmount,#txtInvoiceDescription,#txtVendorName,#txtVendorCode,#ddlgstReceiverLoc,#ddlgstProviderLoc").removeClass("valid").removeClass("required").addClass("required");

        $("#tabGST").hide();
        $("#tabRCM").show();
        $('#Invoicetabs').tabs('select', 1);
        $("#hfInvId").val("0");
        $("#txtVendorName").attr("disabled", false);

        self.GSTInvoiceTaxArray("");
        self.FindTotalTax();
        self.RCMInvoiceTaxArray("");
        self.FindRCMTotal();
        self.ECFTravelArray("");
        self.ECFPaymentArray("");

        $(".viewForIvoice").css("display", "");
        return false;
    };

    self.EditInvoiceDetails = function (item) {
        $('#divInvoiceTravel').attr("style", "display:block;");
        objDialogyInvoiceTravel.dialog({ title: 'Invoice Details', width: '1100', height: '650' });
        objDialogyInvoiceTravel.dialog("open");

        $("#hfInvId").val(item.InvId);
        $("#ddlgstReceiverLoc,#ddlgstProviderLoc,#hdfSVendorId,#hdfSVendorName").val("0");
        $("#txtInvoiceDate,#txtInvoiceNo,#txtServiceMonth,#txtInvoiceAmount,#txtWithoutTaxAmount,#txtInvoiceDescription,#txtVendorName,#txtVendorCode,#txtGSTINState,#txtGSTINpan,#txtGSTINvertical,#txtGSTINFICCL").val("");
        $("input[name=rdogstChargedFlag][value=Y]").prop("checked", true);
        $("input[name=rdoVerify][value=1]").prop("checked", true);
        $("#txtGSTINState,#txtGSTINpan,#txtGSTINvertical,#txtInvoiceDate,#txtInvoiceNo,#txtServiceMonth,#txtInvoiceAmount,#txtWithoutTaxAmount,#txtInvoiceDescription,#txtVendorName,#txtVendorCode,#ddlgstReceiverLoc,#ddlgstProviderLoc").removeClass("valid").removeClass("required");

        $("#tabGST").hide();
        $('#Invoicetabs').tabs('select', 1);

        var InvId = $("#hfInvId").val();
        var EcfFilter = {
            InvId: InvId
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetInvoiceDetailsTravel",
            data: JSON.stringify(EcfFilter),
            contentType: "application/json;",
            async: false,
            success: function (response) {
                var Data1 = "", Data2 = "", Data3 = "", Data4 = "", Data5 = "", value = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);

                if (Data1.length > 0) {
                    $("#txtInvoiceDate").val(Data1[0].InvDate);
                    $("#txtInvoiceNo").val(Data1[0].InvNo);
                    $("#txtServiceMonth").val(Data1[0].servicemonth);
                    $("#txtInvoiceAmount").val(Data1[0].InvAmt);
                    $("#txtWithoutTaxAmount").val(Data1[0].WOTaxAmount);
                    $("#txtInvoiceDescription").val(Data1[0].InvDesc);

                    $("#hdfSVendorName").val(Data1[0].InvoiceSuppliergid);
                    if ($.trim(Data1[0].SupplierCode) != "") {
                        $("#txtVendorName").addClass("valid");
                        $("#txtVendorName").val(Data1[0].SupplierCode + ' - ' + Data1[0].SupplierName);
                    } else { $("#txtVendorName").addClass("required"); $("#txtVendorName").val(""); $("#txtVendorName").attr("disabled", false); }

                    $("input[name=rdoVerify][value=" + (Data1[0].isverified ? "1" : "0") + "]").prop("checked", true);
                    var _value = Data1[0].GstCharged == "Yes" ? "Y" : "N";
                    $("input[name=rdogstChargedFlag][value=" + _value + "]").prop("checked", true);
                    $("#hdfEditViewGstChargedFlag").val(_value);
                    $("#ddlgstProviderLoc").val(Data1[0].ProviderLocation);
                    //Ramya
                    $("#txtGSTINState").val((Data1[0].GstinVendor).substring(0, 2));
                    $("#txtGSTINpan").val((Data1[0].GstinVendor).substring(2, 12));
                    $("#txtGSTINvertical").val((Data1[0].GstinVendor).substring(12, 15));
                    //Ramya
                    $("#tabGST").show();//Add GSTIN split on 02-03-18
                    $("#ddlgstReceiverLoc").val(Data1[0].ReceiverLocation);
                    $("#txtGSTINFICCL").val(Data1[0].GstinFiccl);
                    $("#txtVendorName").attr("disabled", true);
                    $('#Invoicetabs').tabs('select', 0);
                    if ($("input[name=rdogstChargedFlag]:radio[checked=checked]").attr("value") == "Y") {
                        $("#tabGST").show();
                        $("#tabRCM").hide();
                    } else {
                        $("#tabGST").hide();
                        $("#tabRCM").show();
                        $("#txtVendorName").attr("disabled", false);
                        $('#Invoicetabs').tabs('select', 1);
                    }

                    value = $("#txtInvoiceDate").val();
                    if (value != "")
                        $("#txtInvoiceDate").addClass("valid");
                    else
                        $("#txtInvoiceDate").addClass("required");

                    value = $("#txtInvoiceNo").val();
                    if (value != "" && value != "0")
                        $("#txtInvoiceNo").addClass("valid");
                    else
                        $("#txtInvoiceNo").addClass("required");
                    //Ramya
                    //value = $("#txtGSTINVendor").val();
                    //if (value != "")
                    //    $("#txtGSTINVendor").addClass("valid");
                    //else
                    //    $("#txtGSTINVendor").addClass("required");

                    value = $("#txtGSTINState").val();
                    if (value != "")
                        $("#txtGSTINState").addClass("valid");
                    else
                        $("#txtGSTINState").addClass("required");

                    value = $("#txtGSTINpan").val();
                    if (value != "")
                        $("#txtGSTINpan").addClass("valid");
                    else
                        $("#txtGSTINpan").addClass("required");

                    value = $("#txtGSTINvertical").val();
                    if (value != "")
                        $("#txtGSTINvertical").addClass("valid");
                    else
                        $("#txtGSTINvertical").addClass("required");

                    value = $("#txtServiceMonth").val();
                    if (value != "")
                        $("#txtServiceMonth").addClass("valid");
                    else
                        $("#txtServiceMonth").addClass("required");

                    value = $("#txtInvoiceDescription").val();
                    if (value != "")
                        $("#txtInvoiceDescription").addClass("valid");
                    else
                        $("#txtInvoiceDescription").addClass("required");

                    value = $.trim($("#txtInvoiceAmount").val()) == "" ? 0 : parseFloat($("#txtInvoiceAmount").val());
                    if (value > 0)
                        $("#txtInvoiceAmount").addClass("valid");
                    else
                        $("#txtInvoiceAmount").addClass("required");

                    value = $.trim($("#txtWithoutTaxAmount").val()) == "" ? 0 : parseFloat($("#txtWithoutTaxAmount").val());
                    if (value > 0)
                        $("#txtWithoutTaxAmount").addClass("valid");
                    else
                        $("#txtWithoutTaxAmount").addClass("required");

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
                }

                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                self.GSTInvoiceTaxArray(Data2);
                self.FindTotalTax();

                if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null)
                    Data3 = JSON.parse(response.Data3);
                self.ECFTravelArray(Data3);

                if (response.Data4 != null && response.Data4 != "" && JSON.parse(response.Data4) != null)
                    Data4 = JSON.parse(response.Data4);
                self.ECFPaymentArray(Data4);

                if (response.Data6 != null && response.Data6 != "" && JSON.parse(response.Data6) != null)
                    Data6 = JSON.parse(response.Data6);
                self.RCMInvoiceTaxArray(Data6);
                self.FindRCMTotal();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });

        //Add GSTIN split on 02-03-18
        //if ($("#txtGSTINVendor").val() != "") {
        //    var gstin = $("#txtGSTINVendor").val().trim();
        //    //$("#txtGSTINState").val() + "" + $("#txtGSTINpan").val() + "" + $("#txtGSTINvertical").val()
        //    $("#txtGSTINState").val(gstin.substring(0, 2));
        //    $("#txtGSTINpan").val(gstin.substring(2, 12));
        //    $("#txtGSTINvertical").val(gstin.substring(gstin.length - 3, gstin.length));
        //}
        //Add GSTIN split on 02-03-18

        $(".viewForIvoice").css("display", "");
        return false;
    };

    self.ViewInvoiceDetails = function (item) {
        $('#divInvoiceTravel').attr("style", "display:block;");
        objDialogyInvoiceTravel.dialog({ title: 'Invoice Details', width: '1100', height: '650' });
        objDialogyInvoiceTravel.dialog("open");

        $("#hfInvId").val(item.InvId);
        $("#ddlgstReceiverLoc,#ddlgstProviderLoc,#hdfSVendorId,#hdfSVendorName").val("0");
        $("#txtInvoiceDate,#txtInvoiceNo,#txtServiceMonth,#txtInvoiceAmount,#txtWithoutTaxAmount,#txtInvoiceDescription,#txtVendorName,#txtVendorCode,#txtGSTINState,#txtGSTINpan,#txtGSTINvertical,#txtGSTINFICCL").val("");
        $("input[name=rdogstChargedFlag][value=Y]").prop("checked", true);
        $("input[name=rdoVerify][value=1]").prop("checked", true);
        $("#txtGSTINState,#txtGSTINpan,#txtGSTINvertical,#txtInvoiceDate,#txtInvoiceNo,#txtServiceMonth,#txtInvoiceAmount,#txtWithoutTaxAmount,#txtInvoiceDescription,#txtVendorName,#txtVendorCode,#ddlgstReceiverLoc,#ddlgstProviderLoc").removeClass("valid").removeClass("required");

        $("#tabGST").hide();
        $('#Invoicetabs').tabs('select', 1);

        var InvId = $("#hfInvId").val();
        var EcfFilter = {
            InvId: InvId
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetInvoiceDetailsTravel",
            data: JSON.stringify(EcfFilter),
            contentType: "application/json;",
            async: false,
            success: function (response) {
                var Data1 = "", Data2 = "", Data3 = "", Data4 = "", Data5 = "", Data6 = "", value = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);

                if (Data1.length > 0) {
                    $("#txtInvoiceDate").val(Data1[0].InvDate);
                    $("#txtInvoiceNo").val(Data1[0].InvNo);
                    $("#txtServiceMonth").val(Data1[0].servicemonth);
                    $("#txtInvoiceAmount").val(Data1[0].InvAmt);
                    $("#txtWithoutTaxAmount").val(Data1[0].WOTaxAmount);
                    $("#txtInvoiceDescription").val(Data1[0].InvDesc);

                    $("#hdfSVendorName").val(Data1[0].InvoiceSuppliergid);
                    if ($.trim(Data1[0].SupplierCode) != "") {
                        $("#txtVendorName").addClass("valid");
                        $("#txtVendorName").val(Data1[0].SupplierCode + ' - ' + Data1[0].SupplierName);
                    } else { $("#txtVendorName").addClass("required"); $("#txtVendorName").val(""); $("#txtVendorName").attr("disabled", false); }

                    $("input[name=rdoVerify][value=" + (Data1[0].isverified ? "1" : "0") + "]").prop("checked", true);
                    var _value = Data1[0].GstCharged == "Yes" ? "Y" : "N";
                    $("input[name=rdogstChargedFlag][value=" + _value + "]").prop("checked", true);
                    $("#hdfEditViewGstChargedFlag").val(_value);
                    $("#ddlgstProviderLoc").val(Data1[0].ProviderLocation); $("#txtGSTINVendor").val(Data1[0].GstinVendor);
                    $("#tabGST").show();//Add GSTIN split on 02-03-18
                    $("#ddlgstReceiverLoc").val(Data1[0].ReceiverLocation);
                    //$("#txtGSTINVendor").val(Data1[0].GstinVendor);
                    //Ramya
                    $("#txtGSTINState").val((Data1[0].GstinVendor).substring(0, 2));
                    $("#txtGSTINpan").val((Data1[0].GstinVendor).substring(2, 12));
                    $("#txtGSTINvertical").val((Data1[0].GstinVendor).substring(12, 15));
                    //Ramya
                    $("#txtGSTINFICCL").val(Data1[0].GstinFiccl);
                    if ($("input[name=rdogstChargedFlag]:radio[checked=checked]").attr("value") == "Y") {
                        $("#tabGST").show();
                        $("#tabRCM").hide();
                        $("#txtVendorName").attr("disabled", true);
                        $('#Invoicetabs').tabs('select', 0);
                    } else {
                        $("#tabGST").hide();
                        $("#tabRCM").show();
                        $("#txtVendorName").attr("disabled", false);
                        $('#Invoicetabs').tabs('select', 1);
                    }

                    value = $("#txtInvoiceDate").val();
                    if (value != "")
                        $("#txtInvoiceDate").addClass("valid");
                    else
                        $("#txtInvoiceDate").addClass("required");

                    value = $("#txtInvoiceNo").val();
                    if (value != "" && value != "0")
                        $("#txtInvoiceNo").addClass("valid");
                    else
                        $("#txtInvoiceNo").addClass("required");

                    //value = $("#txtGSTINVendor").val();
                    //if (value != "")
                    //    $("#txtGSTINVendor").addClass("valid");
                    //else
                    //    $("#txtGSTINVendor").addClass("required");

                    value = $("#txtGSTINState").val();
                    if (value != "")
                        $("#txtGSTINState").addClass("valid");
                    else
                        $("#txtGSTINState").addClass("required");

                    value = $("#txtGSTINpan").val();
                    if (value != "")
                        $("#txtGSTINpan").addClass("valid");
                    else
                        $("#txtGSTINpan").addClass("required");

                    value = $("#txtGSTINvertical").val();
                    if (value != "")
                        $("#txtGSTINvertical").addClass("valid");
                    else
                        $("#txtGSTINvertical").addClass("required");

                    value = $("#txtServiceMonth").val();
                    if (value != "")
                        $("#txtServiceMonth").addClass("valid");
                    else
                        $("#txtServiceMonth").addClass("required");

                    value = $("#txtInvoiceDescription").val();
                    if (value != "")
                        $("#txtInvoiceDescription").addClass("valid");
                    else
                        $("#txtInvoiceDescription").addClass("required");

                    value = $.trim($("#txtInvoiceAmount").val()) == "" ? 0 : parseFloat($("#txtInvoiceAmount").val());
                    if (value > 0)
                        $("#txtInvoiceAmount").addClass("valid");
                    else
                        $("#txtInvoiceAmount").addClass("required");

                    value = $.trim($("#txtWithoutTaxAmount").val()) == "" ? 0 : parseFloat($("#txtWithoutTaxAmount").val());
                    if (value > 0)
                        $("#txtWithoutTaxAmount").addClass("valid");
                    else
                        $("#txtWithoutTaxAmount").addClass("required");

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
                }

                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                self.GSTInvoiceTaxArray(Data2);
                self.FindTotalTax();

                if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null)
                    Data3 = JSON.parse(response.Data3);
                self.ECFTravelArray(Data3);

                if (response.Data4 != null && response.Data4 != "" && JSON.parse(response.Data4) != null)
                    Data4 = JSON.parse(response.Data4);
                self.ECFPaymentArray(Data4);

                if (response.Data6 != null && response.Data6 != "" && JSON.parse(response.Data6) != null)
                    Data6 = JSON.parse(response.Data6);
                self.RCMInvoiceTaxArray(Data6);
                self.FindRCMTotal();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });

        //Add GSTIN split on 02-03-18
        //if ($("#txtGSTINVendor").val() != "") {
        //    var gstin = $("#txtGSTINVendor").val().trim();
        //    //$("#txtGSTINState").val() + "" + $("#txtGSTINpan").val() + "" + $("#txtGSTINvertical").val()
        //    $("#txtGSTINState").val(gstin.substring(0, 2));
        //    $("#txtGSTINpan").val(gstin.substring(2, 12));
        //    $("#txtGSTINvertical").val(gstin.substring(gstin.length - 3, gstin.length));
        //}
        //Add GSTIN split on 02-03-18

        $(".viewForIvoice").css("display", "none");
        return false;
    };

    self.DeleteInvoiceDetails = function (item) {
        jConfirm("Are you sure? Want to delete Invoice!", "Confirm", function (callback) {
            if (callback == true) {
                var EcfId = $("#hfECFId").val();
                var InvId = item.InvId;
                var IsRemoved = "1";
                var InvoiceHeader = {
                    EcfId: EcfId,
                    InvId: InvId,
                    ProviderLocation: "0",
                    GstinVendor: "",
                    Suppliergid: item.Suppliergid,
                    InvNo: item.InvNo,
                    InvDate: "",
                    Amount: "0",
                    Desc: "",
                    WOTaxAmount: "0",
                    IsVerify: "0",
                    IsRemoved: IsRemoved,
                    GstCharged: "N",
                    ReceiverLocation: "0",
                    GstinFiccl: "",
                    ServiceMonth: ""
                };
                $.ajax({
                    type: "post",
                    url: UrlDet + "SetInvoiceHeaderDetailsEmp",
                    data: JSON.stringify(InvoiceHeader),
                    contentType: "application/json;",
                    success: function (response) {
                        var Data1 = "", Data2 = "";
                        if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                            Data1 = JSON.parse(response.Data1);
                            if (Data1[0].Clear == false)
                                jAlert(Data1[0].Msg, "Message");
                            else
                                $('#hfISDedup').val("0");
                            jAlert(Data1[0].Msg, "Message");
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

    self.SaveInvoiceDetails = function () {
        var GstCharged = "N", ProviderLocation = "0", GstinVendor = "", ReceiverLocation = "0", GstinFiccl = "", VendorId = "0";
        ProviderLocation = $("#ddlgstProviderLoc").val();
        //GstinVendor = $("#txtGSTINVendor").val();
        GstinVendor = $("#txtGSTINState").val() + $("#txtGSTINpan").val() + $("#txtGSTINvertical").val();
        GstCharged = $("input[name=rdogstChargedFlag]:radio[checked=checked]").attr("value");
        $("#hdfEditViewGstChargedFlag").val(GstCharged);
        ReceiverLocation = $("#ddlgstReceiverLoc").val();
        GstinFiccl = $("#txtGSTINFICCL").val();
        VendorId = $("#hdfSVendorName").val();

        var EcfId = $("#hfECFId").val();
        var InvId = $("#hfInvId").val();
        var InvDate = $("#txtInvoiceDate").val();
        var ServiceMonth = $("#txtServiceMonth").val();
        var InvNo = $("#txtInvoiceNo").val();
        var Desc = $("#txtInvoiceDescription").val();
        var Amount = $("#txtInvoiceAmount").val();
        var WOTaxAmount = $("#txtWithoutTaxAmount").val();
        var IsVerify = $("input[name=rdoVerify]:radio[checked=checked]").attr("value");
        var IsRemoved = "0";
        var month = new Date(Date.parse(ServiceMonth.split(' ')[0] + " 1, " + ServiceMonth.split(' ')[1])).getMonth() + 1;
        ServiceMonth = ("1/" + month + "/" + ServiceMonth.split(' ')[1]);
        // alert($("input[name=rdogstChargedFlag]:radio[checked=checked]").attr("value"));
        //radio button validation by kavitha on 05-07-2017
        if ($("input[name=rdogstChargedFlag]:radio[checked=checked]").attr("value") == "Y") {
            if ($.trim(VendorId) == "" || $.trim(VendorId) == "0") {
                jAlert("Ensure Supplier Code/Name!", "Message");
                return false;
            }
            if ($.trim(InvNo) == "" || $.trim(InvNo) == "0") {
                jAlert("Ensure Invoice No!", "Message");
                return false;
            }
            if ($.trim(Desc) == "") {
                jAlert("Ensure Invoice Description!", "Message");
                return false;
            }
        }

        if ($.trim(InvDate) == "") {
            jAlert("Ensure Invoice Date!", "Message");
            return false;
        }
        debugger;
        if ($.trim(ServiceMonth) == "" || ServiceMonth.includes("undefined")) {
            jAlert("Ensure Service Month!", "Message");
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

        //if ((parseInt(ProviderLocation) == 0 || $.trim(ProviderLocation) == "") && GstCharged == "Y") {
        if ((parseInt(ProviderLocation) == 0 || $.trim(ProviderLocation) == "")) {
            jAlert("Ensure Provider Location!", "Message");
            return false;
        }

        if ($.trim(GstinVendor) == "" && GstCharged == "Y") {
            jAlert("Ensure GSTIN Vendor!", "Message");
            return false;
        }

        //if ((parseInt(ReceiverLocation) == 0 || $.trim(ReceiverLocation) == "") && GstCharged == "Y") {
        if ((parseInt(ReceiverLocation) == 0 || $.trim(ReceiverLocation) == "")) {
            jAlert("Ensure Receiver Location!", "Message");
            return false;
        }

        if ($.trim(GstinFiccl) == "" && GstCharged == "Y") {
            jAlert("Ensure GSTIN FICCL!", "Message");
            return false;
        }
        if ($.trim(InvNo) == "" || $.trim(InvNo) == "0") {
            InvNo = 0;
        }

        var InvoiceHeader = {
            EcfId: EcfId,
            InvId: InvId,
            ProviderLocation: ProviderLocation,
            GstinVendor: GstinVendor,
            Suppliergid: VendorId,
            InvNo: InvNo,
            InvDate: InvDate,
            Amount: Amount,
            Desc: Desc,
            WOTaxAmount: WOTaxAmount,
            IsVerify: IsVerify,
            IsRemoved: IsRemoved,
            GstCharged: GstCharged,
            ReceiverLocation: ReceiverLocation,
            GstinFiccl: GstinFiccl,
            ServiceMonth: ServiceMonth
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetInvoiceHeaderDetailsEmp",
            data: JSON.stringify(InvoiceHeader),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "", Data3 = "", Data4 = "", Data5 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    debugger;
                    if (Data1[0].Clear == false)
                        jAlert(Data1[0].Msg, "Message");
                    else if (Data1[0].Reset == false) {
                        jAlert(Data1[0].Message, "Message");
                    }
                    else {
                        $('#hfISDedup').val("0");
                        jAlert(Data1[0].Msg, "Message");
                        $("#hfInvId").val(Data1[0].InvId);

                        if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                            Data2 = JSON.parse(response.Data2);
                        self.ECFInvoiceArray(Data2);

                        if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null)
                            Data3 = JSON.parse(response.Data3);
                        self.ECFPaymentArray(Data3);

                        if (response.Data4 != null && response.Data4 != "" && JSON.parse(response.Data4) != null)
                            Data4 = JSON.parse(response.Data4);
                        self.ECFTravelArray(Data4);

                        if (response.Data5 != null && response.Data5 != "" && JSON.parse(response.Data5) != null)
                            Data5 = JSON.parse(response.Data5);
                        self.GSTInvoiceTaxArray(Data5);
                        self.FindTotalTax();

                        if (response.Data6 != null && response.Data6 != "" && JSON.parse(response.Data6) != null)
                            Data6 = JSON.parse(response.Data6);
                        self.RCMInvoiceTaxArray(Data6);
                        self.FindRCMTotal();
                        if (GstCharged == "Y") {
                            $("#tabGST").show();//Add GSTIN split on 02-03-18
                            $("#tabRCM").hide();
                            $('#Invoicetabs').tabs('select', 0);
                        } else {
                            //  $("#tabGST").hide(); 
                            $("#tabRCM").show();
                            $("#tabGST").hide();//Add GSTIN split on 02-03-18
                            $('#Invoicetabs').tabs('select', 1);
                        }
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) { }
        });
    };

    self.AddGSTTax = function () {
        $('#GSTDialogTravel').attr("style", "display:block;");

        $("#hfGSTInvoiceTaxGid,#ddlGSTHSNCode").val("0");
        $("#chkGSTgsnapplicability").prop("checked", true);
        $("#txtGSTHSNDesc,#txtGSTTaxableAmount,#txtGSTTaxAmount").val("");
        $("#ddlGSTTaxType,#ddlGSTSubTaxType,#txtGSTTaxRate").val("0");

        $("#ddlGSTHSNCode,#txtGSTTaxableAmount,#txtGSTTaxAmount,#txtGSTTaxRate,#ddlGSTTaxType,#ddlGSTSubTaxType").removeClass("required").removeClass("valid");
        $("#ddlGSTHSNCode,#txtGSTTaxAmount,#txtGSTTaxRate,#ddlGSTTaxType,#ddlGSTSubTaxType").addClass("required");

        $("#txtGSTTaxableAmount").val($("#txtInvoiceAmount").val());
        var Amount = $.trim($("#txtGSTTaxableAmount").val()) == "" ? 0 : parseFloat($("#txtGSTTaxableAmount").val());
        if (Amount > 0) {
            $("#txtGSTTaxableAmount").addClass("valid");
        } else {
            $("#txtGSTTaxableAmount").addClass("required");
        }

        $("#btnGSTTaxSubmit").css("display", "");
        objDialogyGSTTravel.dialog({ title: 'Add GST', width: '460', height: '400' });
        objDialogyGSTTravel.dialog("open");
        return false;
    };

    self.ViewGSTTax = function (item) {
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
        //$("#txtGSTTaxRate").val(item.Rate);
        $("#tdOtherTax").css("display", "none");
        $("#tdGST").css('display', 'block');
        $("#txtGSTRate").val(item.Rate);
        $("#txtGSTTaxableAmount").val(item.TaxableAmt);
        $("#txtGSTTaxAmount").val(item.TaxAmt);
        $("#btnGSTTaxSubmit").css("display", "none");

        objDialogyGSTTravel.dialog({ title: 'View GST', width: '460', height: '400' });
        objDialogyGSTTravel.dialog("open");
    };
    self.EditGSTTax = function (item) {
        //$("#txtGSTTaxRate").attr("disabled", false);
        $("#ddlGSTHSNCode,#ddlGSTTaxType,#ddlGSTSubTaxType,#txtGSTRate,#txtGSTTaxAmount,#txtGSTTaxableAmount").removeClass("required").removeClass("valid");
        if (item.GsnApplicable == "Yes" || item.GsnApplicable == "yes") {
            $("#chkGSTgsnapplicability").prop('checked', true);
        } else {
            $("#chkGSTgsnapplicability").prop('checked', false);
        }

        $("#hfGSTInvoiceTaxGid").val(item.invoicetaxgid);



        //$("#ddlGSTHSNCode option:selected").text(item.HsnCode);
        $("#ddlGSTHSNCode option:contains(" + item.HsnCode + ")").attr('selected', 'selected');

        $("#txtGSTHSNDesc").val(item.HsnDescription);

        self.LoadGSTTaxType();
        $("#ddlGSTTaxType").val(item.Taxgid);
        self.GSTLoadTaxSubType();
        $("#ddlGSTSubTaxType").val(item.TaxsubtypegID);
        //$("#txtGSTTaxRate").val(item.Rate);
        $("#tdOtherTax").css("display", "none");
        $("#tdGST").css('display', 'block');
        $("#txtGSTRate").val(item.Rate);
        $("#txtGSTTaxableAmount").val(item.TaxableAmt);
        $("#txtGSTTaxAmount").val(item.TaxAmt);

        if ($("#ddlGSTHSNCode").val() == "0") { $("#ddlGSTHSNCode").addClass("required"); } else { $("#ddlGSTHSNCode").addClass("valid"); }

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

        if ($.trim($("#txtGSTTaxAmount").val()) == "" || parseFloat($("#txtGSTTaxAmount").val()) == 0) {
            $("#txtGSTTaxAmount").addClass("required");
        } else {
            $("#txtGSTTaxAmount").addClass("valid");
        }

        $("#btnGSTTaxSubmit").css("display", "");
        objDialogyGSTTravel.dialog({ title: 'GST Invoice Tax', width: '460', height: '400' });
        objDialogyGSTTravel.dialog("open");
    };

    self.DeleteGSTTax = function (item) {
        jConfirm("Are you sure? Want to delete Invoice GST Tax!", "Confirm", function (callback) {
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
                        self.FindTotalTax();
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

    self.SubmitGSTTax = function () {
        var GSNapplicable = "";

        if ($("#chkGSTgsnapplicability").prop("checked")) {
            GSNapplicable = "Y";
        } else {
            GSNapplicable = "N";
        }

        var HSNgid = $("#ddlGSTHSNCode option:selected").val();


        var InvoiceTaxgid = $("#hfGSTInvoiceTaxGid").val();
        var InvId = $("#hfInvId").val();
        var SupplierId = $("#hdfSVendorName").val();
        var TaxId = $("#ddlGSTTaxType").val();
        var SubTaxId = $("#ddlGSTSubTaxType").val();
        var TaxRate = $("#txtGSTRate").val();
        var TaxableAmt = $.trim($("#txtGSTTaxableAmount").val()) == "" ? 0 : parseFloat($("#txtGSTTaxableAmount").val());
        var TaxAmount = $.trim($("#txtGSTTaxAmount").val()) == "" ? 0 : parseFloat($("#txtGSTTaxAmount").val());

        var IsRemoved = "0";
        var INVAmount = $.trim($("#txtInvoiceAmount").val()) == "" ? 0 : parseFloat($("#txtInvoiceAmount").val());
        var Tax = (TaxRate * TaxableAmt) / 100;

        if ($.trim(HSNgid) == "" || parseFloat(HSNgid) == 0) {
            jAlert("Ensure HSN Code!", "Message");
            return false;
        }
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
                        objDialogyGSTTravel.dialog("close");
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

    self.SubmitInvoiceDetails = function () {

        var InvId = $("#hfInvId").val();

        var InputFilter = {
            InvId: InvId,
            IsTravel: "1"
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SubmitInvoiceEmp",
            data: JSON.stringify(InputFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == false) {
                        objDialogyInvoiceTravel.dialog("close");
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
    //END   -- Invoice Details

    this.LoadECFDetails = function () {
        var EcfId = $("#hdfEcfId").val();
        $("#hfECFId").val(EcfId);
        var EcfFilter = {
            EcfId: EcfId
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetEmployeeTravelClaim",
            data: JSON.stringify(EcfFilter),
            contentType: "application/json;",
            success: function (response) {
                $("#hfInvId").val("0");
                var Data1 = "", Data2 = "", Data3 = "", Data4 = "", Data5 = "", Data6 = "", Data7 = "", Data8 = "", Data9 = "", Data10 = "", Data11 = "", Data12 = "";
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
                    currencyid = Data2[0].CrnID;
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
                    $("#hfSupplierId").val(Data2[0].EmployeeId);

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

                /*if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null)
                    Data3 = JSON.parse(response.Data3);
                self.ECFTravelArray(Data3);

                if (response.Data4 != null && response.Data4 != "" && JSON.parse(response.Data4) != null)
                    Data4 = JSON.parse(response.Data4);
                self.ECFPaymentArray(Data4);
                */
                if (response.Data5 != null && response.Data5 != "" && JSON.parse(response.Data5) != null)
                    Data5 = JSON.parse(response.Data5);
                self.ECFEmployeeListArray(Data5);

                if (response.Data6 != null && response.Data6 != "" && JSON.parse(response.Data6) != null)
                    Data6 = JSON.parse(response.Data6);
                self.ECFAttachmentArray(Data6);

                if (response.Data7 != null && response.Data7 != "" && JSON.parse(response.Data7) != null)
                    Data7 = JSON.parse(response.Data7);
                self.ECFAuditrailArray(Data7);

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
                //------------- Shrinidhi 03.05.2016------------------------
                if (response.Data12 != null && response.Data12 != "" && JSON.parse(response.Data12) != null) {
                    Data12 = JSON.parse(response.Data12);
                    BenName = Data12;
                }
                ///---------------------------

                //Gopi
                var Data13 = "";
                if (response.Data13 != null && response.Data13 != "" && JSON.parse(response.Data13) != null)
                    Data13 = JSON.parse(response.Data13);
                self.ECFInvoiceArray(Data13);
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

    this.SetInvoiceDebitDetails = function () {

        var ecftravelgid = $("#hfDebitlineGId").val();
        var ecfId = $("#hfECFId").val();
        var InvId = $("#hfInvId").val();
        //alert(InvId);
        var expnaturegid = $("#ddlExpenses").val();
        var expCatId = $("#ddlCategory").val();
        var expSubcatId = $("#ddlSubCategory").val();
        var placefrom = $("#ddlPlaceFrom").val();
        var placeto = $("#ddlPlaceTo").val();
        var claimperiodfrm = $("#txtClaimFrom").val();
        var claimperiodto = $("#txtClaimTo").val();
        var trasportgid = $("#ddlTravelMode").val();
        var transportclassgid = $("#ddlTravelClass").val();
        var Distance = $("#txtTDistance").val();
        var Rate = $("#txtTRate").val();
        var FCCode = $("#ddlBU").val();
        var CCCode = $("#ddlCC").val();
        var OUCode = $("#hfOUCode").val();
        var ProductCode = $("#hfProductCode").val();
        var Amount = $("#txtDebitAmount").val();
        var Desc = $("#txtDebitDescription").val();
        var IsRemoved = "0";
        var Mode = expnaturegid.split('-')[1];
        //kavitha adding hsn gid into debitline table
        var hsngid = $("#ddlhsngid").val();
        expnaturegid = expnaturegid.split('-')[0];
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

        if (claimperiodfrm == "") {
            jAlert("Ensure Claim From!", "Error");
            return false;
        }
        if (claimperiodto == "") {
            jAlert("Ensure Claim To!", "Error");
            return false;
        }

        if (OUCode == "0") {
            jAlert("Ensure Location Code(OUCode)!", "Error");
            return false;
        }

        if (ProductCode == "0") {
            jAlert("Ensure Product Code!", "Error");
            return false;
        }

        if (placefrom == "0") {
            jAlert("Ensure Place From!", "Error");
            return false;
        }
        if (placeto == "0") {
            jAlert("Ensure Place To!", "Error");
            return false;
        }
        if (trasportgid == "0" && Mode == 'T') {
            jAlert("Ensure Transport Mode!", "Error");
            return false;
        }
        if (transportclassgid == "0" && Mode == 'T') {
            jAlert("Ensure Transport Class!", "Error");
            return false;
        }

        if ($("#txtClaimMonth").text() != $("#txtExpenseClaimMonth").val()) {
            jAlert("To Date Should be ECF Claim Month Date!", "Error");
            return false;
        }
        //if ($("input[name=rdorcmChargedFlag]:radio[checked=checked]").attr("value") == "Y") { 
        if (hsngid == "0") {
            jAlert("Ensure hsn details", "Error");
            return false;
        }
        //}


        var TravelDetails = {
            ecftravelgid: ecftravelgid,
            ecfId: ecfId,
            invId: InvId,
            expnaturegid: expnaturegid,
            expCatId: expCatId,
            expSubcatId: expSubcatId,
            placefrom: placefrom,
            placeto: placeto,
            claimperiodfrm: claimperiodfrm,
            claimperiodto: claimperiodto,
            trasportgid: trasportgid,
            transportclassgid: transportclassgid,
            Distance: Distance,
            Rate: Rate,
            FCCode: FCCode,
            CCCode: CCCode,
            OUCode: OUCode,
            ProductCode: ProductCode,
            Amount: Amount,
            Desc: Desc,
            IsRemoved: IsRemoved,
            hsngid: hsngid,
            rcm: rcm
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetTravelExpenseDetails",
            data: JSON.stringify(TravelDetails),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "", Data3 = "";
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
                self.ECFTravelArray(Data2);
                if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null)
                    Data3 = JSON.parse(response.Data3);

                self.GSTInvoiceTaxArray(Data3);
                self.FindTotalTax();

                if (response.Data4 != null && response.Data4 != "" && JSON.parse(response.Data4) != null)
                    Data4 = JSON.parse(response.Data4);
                self.RCMInvoiceTaxArray(Data4);
                self.FindRCMTotal();

                if (rcm == "N") {
                    $("#tabRCM").hide();
                } else {
                    $("#tabRCM").show();
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.DeleteInvoiceDebitDetails = function (DebitlineGId) {

        jConfirm("Are you sure? Want to delete Travel Details!", "Confirm", function (callback) {
            if (callback == true) {
                var ecftravelgid = DebitlineGId;
                var ecfId = $("#hfECFId").val();
                var InvId = $("#hfInvId").val();
                var expnaturegid = "0";
                var expCatId = "0";
                var expSubcatId = "0";
                var placefrom = "0";
                var placeto = "0";
                var claimperiodfrm = "";
                var claimperiodto = "";
                var trasportgid = "0";
                var transportclassgid = "0";
                var Distance = "0";
                var Rate = "0";
                var FCCode = "0";
                var CCCode = "0";
                var OUCode = "0";
                var ProductCode = "0";
                var Amount = "0";
                var Desc = "";
                var IsRemoved = "1";

                var TravelDetails = {
                    ecftravelgid: ecftravelgid,
                    ecfId: ecfId,
                    invId: InvId,
                    expnaturegid: expnaturegid,
                    expCatId: expCatId,
                    expSubcatId: expSubcatId,
                    placefrom: placefrom,
                    placeto: placeto,
                    claimperiodfrm: claimperiodfrm,
                    claimperiodto: claimperiodto,
                    trasportgid: trasportgid,
                    transportclassgid: transportclassgid,
                    Distance: Distance,
                    Rate: Rate,
                    FCCode: FCCode,
                    CCCode: CCCode,
                    OUCode: OUCode,
                    ProductCode: ProductCode,
                    Amount: Amount,
                    Desc: Desc,
                    IsRemoved: IsRemoved
                };
                $.ajax({
                    type: "post",
                    url: UrlDet + "SetTravelExpenseDetails",
                    data: JSON.stringify(TravelDetails),
                    contentType: "application/json;",
                    success: function (response) {
                        var Data1 = "", Data2 = "", Data3 = "";
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
                        self.ECFTravelArray(Data2);

                        if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null)
                            Data3 = JSON.parse(response.Data3);

                        self.GSTInvoiceTaxArray(Data3);
                        self.FindTotalTax();

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
        var Id = $("#txtarfrefglno").val();

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

        if (paymode == "PPX") {

            RefId = Id;
        }

        if (paymode == "0" || paymode == "-- Select One --") {
            jAlert("Ensure Payment Mode!", "Message");
            return false;
        }
        //if ($.trim(RefNo) == "") {
        //    if (paymode == "DD")
        //        jAlert("Ensure Payable At!", "Message");
        //    else
        //        jAlert("Ensure Account No!", "Message");
        //    return false;
        //}
        //if ($.trim(Beneficiary) == "") {
        //    jAlert("Ensure Beneficiary Name!", "Message");
        //    return false;
        //}

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
            IfscCode: IfscCode,
            Id: Id
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
                var PayBank = 0;
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
        if (flag == 10)
            objDialogySUS.dialog("close");
        if (flag == 4)
            objDialogyInvoiceTravel.dialog("close");
        if (flag == 5)
            objDialogyAddPayment1.dialog("close");
        if (flag == 13)
            objDialogyGSTTravel.dialog("close");
    };

    this.EditInvoiceDebitDetails = function (Index, flag) {
        debugger;
        var _tmpData = self.ECFTravelArray()[Index];

        var _value = _tmpData.rcm;
        if (_value == 'Yes') {
            //$("input[name=rdorcmChargedFlag][value=N]").prop("checked", true);
            $("input[name=rdorcmChargedFlag][value=Y]").attr("checked", true);
        }
        else {
            $("input[name=rdorcmChargedFlag][value=N]").prop("checked", true);
        }
        var GSTCharged = $("#hdfEditViewGstChargedFlag").val();
        if (GSTCharged == "Y") {
            //$("#rdorcmChargedYes").prop("disabled", true);
            $("input[name=rdorcmChargedFlag][value=N]").attr("disabled", true);
            $("input[name=rdorcmChargedFlag][value=Y]").attr("disabled", true);
        }
        else {
            $("input[name=rdorcmChargedFlag][value=N]").attr("disabled", false);
            $("input[name=rdorcmChargedFlag][value=Y]").attr("disabled", false);
        }
        $("#hfDebitlineGId").val(_tmpData.GId);
        //$("#hfInvId").val(_tmpData.InvId);
        $("#ddlExpenses").val(_tmpData.expnatureGid);
        self.LoadExpenseCategory();
        $("#ddlCategory").val(_tmpData.expcatgid);
        self.LoadExpenseSubCategory();
        $("#ddlSubCategory").val(_tmpData.subcatgid);
        self.LoadHsnArray();
        $("#txtClaimFrom").val(_tmpData.claimfrm);
        $("#txtClaimTo").val(_tmpData.claimto);
        $("#ddlBU").val(_tmpData.fccode);
        $("#ddlCC").val(_tmpData.CCCode);
        $("#txtProductCode").val(_tmpData.Product);
        $("#txtOUCode").val(_tmpData.ou);
        $("#hfProductCode").val(_tmpData.ProductCode);
        $("#hfOUCode").val(_tmpData.oucode);
        $("#txtDebitAmount").val(_tmpData.TravelAmt);
        $("#txtDebitDescription").val(_tmpData.Ddesc);

        $("#ddlPlaceFrom").val(_tmpData.PlaceFrm);
        $("#ddlPlaceTo").val(_tmpData.PlaceTo);
        $("#ddlhsngid").val(_tmpData.ecftravel_hsn_gid);
        var CatId = $("#ddlExpenses").val();
        if (CatId != "0")
            CatId = CatId.split("-")[1];
        if (CatId == "T") {
            $("#trTravel").slideDown();
        }
        else {
            $("#trTravel").slideUp();
        }
        $("#ddlTravelMode").val(_tmpData.trasportgid);
        self.LoadTravelClass();
        $("#ddlTravelClass").val(_tmpData.TranClassGid);
        $("#txtTRate").val(_tmpData.distance);
        $("#txtTDistance").val(_tmpData.rate);
        $("#txtTRate").attr("disabled", "disabled");
        $("#txtTDistance").attr("disabled", "disabled");

        var month = _tmpData.claimto.split("/")[1];
        $("#txtExpenseClaimMonth").val(Months[parseInt(month)] + " " + _tmpData.claimto.split("/")[2]);

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

        if ($("#txtClaimFrom").val() == "") {
            $("#txtClaimFrom").removeClass("valid");
            $("#txtClaimFrom").addClass("required");
        }
        else {
            $("#txtClaimFrom").addClass("valid");
            $("#txtClaimFrom").removeClass("required");
        }

        if ($("#txtClaimTo").val() == "") {
            $("#txtClaimTo").removeClass("valid");
            $("#txtClaimTo").addClass("required");
        }
        else {
            $("#txtClaimTo").addClass("valid");
            $("#txtClaimTo").removeClass("required");
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

        if ($("#ddlPlaceFrom").val() == "0") {
            $("#ddlPlaceFrom").removeClass("valid");
            $("#ddlPlaceFrom").addClass("required");
        }
        else {
            $("#ddlPlaceFrom").addClass("valid");
            $("#ddlPlaceFrom").removeClass("required");
        }
        if ($("#ddlPlaceTo").val() == "0") {
            $("#ddlPlaceTo").removeClass("valid");
            $("#ddlPlaceTo").addClass("required");
        }
        else {
            $("#ddlPlaceTo").addClass("valid");
            $("#ddlPlaceTo").removeClass("required");
        }
        if ($("#ddlTravelMode").val() == "0") {
            $("#ddlTravelMode").removeClass("valid");
            $("#ddlTravelMode").addClass("required");
        }
        else {
            $("#ddlTravelMode").addClass("valid");
            $("#ddlTravelMode").removeClass("required");
        }

        if ($("#ddlTravelClass").val() == "0") {
            $("#ddlTravelClass").removeClass("valid");
            $("#ddlTravelClass").addClass("required");
        }
        else {
            $("#ddlTravelClass").addClass("valid");
            $("#ddlTravelClass").removeClass("required");
        }

        if (flag == 0)
            $("#btnDebitLineSubmit").css("display", "none");
        else
            $("#btnDebitLineSubmit").css("display", "");
        objDialogyAddDebit.dialog({ title: 'Travel Details', width: '1000', height: '420' });
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
            $("#hfRefId").val(_tmpData.InvId);
            //$("#ddlPayMode1").val(_tmpData.PMode);
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

    this.FetchPPXPaymentRefNo = function () {
        var SupplierId = $("#hfSupplierId").val();
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

    this.SelectPPX = function (item) {
        $("#txtPayRefNo").val(item.ARFNo);
        $("#txtPaymentAmount").val(item.ARFException);
        $("#txtarfrefglno").val(item.ARFGlno);
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

    this.LoadNatureOfExpenses = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetNatureOfExpensesForTravel",
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
        if (CatId != "0")
            CatId = CatId.split("-")[0];
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

    this.LoadPayMode = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetPayModeEmployee",
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

    this.LoadPlace = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetPlace",
            data: "{}",
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);

                self.PlaceArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.LoadTravelMode = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetTravelMode",
            data: "{}",
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);

                self.TravelModeArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.LoadTravelClass = function () {
        var CatId = $("#ddlTravelMode").val();
        var CatFilter = {
            CatId: CatId
        };
        $.ajax({
            type: "post",
            url: UrlHelper + "GetTravelClass",
            data: JSON.stringify(CatFilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.TravelClassArray(Data1);
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
        var CatId = $("#ddlExpenses").val();
        if (CatId != "0")
            CatId = CatId.split("-")[1];
        if (CatId == "T") {
            $("#trTravel").slideDown();
        }
        else {
            $("#trTravel").slideUp();
        }
        $("#ddlTravelMode").val("0");
        self.LoadTravelClass();
        $("#ddlTravelClass").val("0");
        $("#txtTRate").val("");
        $("#txtTDistance").val("");
        $("#txtTRate").attr("disabled", "disabled");
        $("#txtTDistance").attr("disabled", "disabled");
        if ($("#ddlTravelMode").hasClass("valid")) {
            $("#ddlTravelMode").removeClass("valid");
            $("#ddlTravelMode").addClass("required");
        }
        if ($("#ddlTravelClass").hasClass("valid")) {
            $("#ddlTravelClass").removeClass("valid");
            $("#ddlTravelClass").addClass("required");
        }

        self.LoadExpenseCategory();
        self.LoadExpenseSubCategory();
    };

    this.ChangeExpencesCategory = function () {
        if ($("#ddlSubCategory").hasClass("valid")) {
            $("#ddlSubCategory").removeClass("valid");
            $("#ddlSubCategory").addClass("required");
        }
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
        var CatFilter = {
            CatId: CatId
        };
        $.ajax({
            type: "post",
            url: UrlHelper + "GetHsnArray",
            data: JSON.stringify(CatFilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);

                self.HsnCodeArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };



    this.ChangeTravelMode = function () {
        if ($("#ddlTravelClass").hasClass("valid")) {
            $("#ddlTravelClass").removeClass("valid");
            $("#ddlTravelClass").addClass("required");
        }
        self.LoadTravelClass();
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

    //Gopi
    self.LoadGSTINHSNCode();
    self.LoadTaxRate();
    self.LoadTaxType();


    self.LoadCurrencyType();
    self.LoadBusinessSegment();
    self.LoadCostCenter();
    self.LoadPayMode();
    self.LoadAttachmentType();
    self.LoadPlace();

    self.LoadNatureOfExpenses();
    self.LoadExpenseCategory();
    self.LoadExpenseSubCategory();
    self.LoadTravelMode();
    self.LoadTravelClass();

    self.loadSUSGrid();

    self.LoadPayModeCommon();
    self.LoadPayBank();

    //Gopi
    self.LoadGSTINCategory();

    self.LoadECFDetails();
    //kavitha
    self.LoadHsnArray();
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
    self.ECFInvoiceSummary = ko.observableArray();
    self.GSTInvoiceTaxHistory = ko.observableArray();
    self.ECFPaymentHistory = ko.observableArray();
    self.ECFTravelHistory = ko.observableArray();

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


                if (response.Data4 != null && response.Data4 != "" && JSON.parse(response.Data4) != null) {
                    Data4 = JSON.parse(response.Data4);
                }
                if (response.Data7 != null && response.Data7 != "" && JSON.parse(response.Data7) != null) {
                    Data7 = JSON.parse(response.Data7);
                }
                if (response.Data8 != null && response.Data8 != "" && JSON.parse(response.Data8) != null) {
                    Data8 = JSON.parse(response.Data8);
                }

                self.ECFInvoiceSummary(Data7);
                self.GSTInvoiceTaxHistory(Data8);
                self.ECFPaymentHistory(Data1);
                self.ECFTravelHistory(Data4);

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };



    self.LoadAuditHistory();

};

var mainViewModel = new SearchModel();
ko.applyBindings(mainViewModel, document.getElementById("travelclaim"));
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
        action: 'bulk'
    };

    $.ajax({
        type: "post",
        url: UrlDet + "CheckrcmExists",
        data: JSON.stringify(rcmflagparam),
        contentType: "application/json;",
        success: function (response) {
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });

    //Gopi
    objDialogyInvoiceTravel = $("[id$='divInvoiceTravel']");
    objDialogyInvoiceTravel.dialog({
        autoOpen: false,
        modal: true,
        width: 1100,
        height: 600,
        duration: 300
    });

    objDialogyAddDebit = $("[id$='ShowDialog1']");
    objDialogyAddDebit.dialog({
        autoOpen: false,
        modal: true,
        width: 1000,
        height: 420,
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

    //Gopi
    objDialogyGSTTravel = $("[id$='GSTDialogTravel']");
    objDialogyGSTTravel.dialog({
        autoOpen: false,
        modal: true,
        width: 460,
        height: 290,
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
            //alert(BenName);
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
            //$("input").prop('disabled', true);
        }
    });

    $("#ddlPlaceFrom").change(function () {
        var PlaceFrom = $("#ddlPlaceFrom").val();
        if (PlaceFrom != "0") {
            $("#ddlPlaceFrom").removeClass("required");
            $("#ddlPlaceFrom").addClass("valid");
        }
        else {
            $("#ddlPlaceFrom").removeClass("valid");
            $("#ddlPlaceFrom").addClass("required");
        }
    });

    $("#ddlPlaceTo").change(function () {
        var PlaceTo = $("#ddlPlaceTo").val();
        if (PlaceTo != "0") {
            $("#ddlPlaceTo").removeClass("required");
            $("#ddlPlaceTo").addClass("valid");
        }
        else {
            $("#ddlPlaceTo").removeClass("valid");
            $("#ddlPlaceTo").addClass("required");
        }
    });

    $("#ddlTravelMode").change(function () {
        var TravelMode = $("#ddlTravelMode").val();
        if (TravelMode != "0") {
            $("#ddlTravelMode").removeClass("required");
            $("#ddlTravelMode").addClass("valid");
        }
        else {
            $("#ddlTravelMode").removeClass("valid");
            $("#ddlTravelMode").addClass("required");
        }
    });

    $("#ddlTravelClass").change(function () {
        var TravelClass = $("#ddlTravelClass").val();
        if (TravelClass != "0") {
            $("#ddlTravelClass").removeClass("required");
            $("#ddlTravelClass").addClass("valid");
        }
        else {
            $("#ddlTravelClass").removeClass("valid");
            $("#ddlTravelClass").addClass("required");
        }
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

    //Gopi
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

    $(".fsDatepicker").keyup(function (e) {
        if (e.which == 8) {
            $(this).val("");
            $(this).removeClass("required").addClass("valid");
            $(this).addClass("required");
        }
    });

    $(".fsDatepicker").change(function (e) {
        var value = $(this).val();
        if (value != "")
            $(this).removeClass("required").addClass("valid");
        else
            $(this).removeClass("valid").addClass("required");
    });

    $("#txtServiceMonth").keyup(function (e) {
        if (e.which == 8) {
            $(this).val("");
            $(this).removeClass("required").addClass("valid");
            $(this).addClass("required");
        }
    });

    $("input[name=rdogstChargedFlag]:radio").change(function () {
        //$("#ddlgstProviderLoc").val("0");
        //$("#ddlgstReceiverLoc").val("0");
        //$("#txtGSTINVendor").val("");
        //$("#txtGSTINFICCL").val("");

        //$("#ddlgstProviderLoc,#ddlgstReceiverLoc,#txtGSTINVendor").removeClass("valid").removeClass("required");
        //$("#ddlgstProviderLoc,#ddlgstReceiverLoc,#txtGSTINVendor").addClass("required");
        if ($("input[name=rdogstChargedFlag]:radio[checked=checked]").attr("value") == "Y") {
            $("#tabGST").show(); $("#txtVendorName").attr("disabled", true);
            $("#tabRCM").hide(); //Add GSTIN split on 02-03-18
        }
        else {
            $("#tabGST").hide();
            $("#txtVendorName").attr("disabled", false);
            $("#tabRCM").show(); //Add GSTIN split on 02-03-18
        }
    });

    $("#txtInvoiceDate, #txtInvoiceDescription, #txtServiceMonth").focusout(function () {
        var value = $(this).val();
        if (value != "")
            $(this).removeClass("required").addClass("valid");
        else
            $(this).removeClass("valid").addClass("required");
    });

    $("#txtInvoiceNo").focusout(function () {
        var value = $(this).val();
        if (value != "" && value != "0")
            $(this).removeClass("required").addClass("valid");
        else
            $(this).removeClass("valid").addClass("required");
    });

    $("#txtWithoutTaxAmount,#txtInvoiceAmount").focusout(function () {
        var value = $.trim($(this).val()) == "" ? 0 : parseFloat($(this).val());
        if (value > 0)
            $(this).removeClass("required").addClass("valid");
        else
            $(this).removeClass("valid").addClass("required");
    });

    //search auto complete
    var tab = false;
    $("#txtVendorName").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfSVendorName").val("0");
        }
        var GstCharged = "N";
        GstCharged = $("input[name=rdogstChargedFlag]:radio[checked=checked]").attr("value");
        $("#hdfEditViewGstChargedFlag").val(GstCharged);
        $("#txtVendorName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteSupplierAll",
                    data: "{ 'txt' : '" + $("#txtVendorName").val() + "'}",
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
                $("#hdfSVendorName").val(i.item.val);
                $("#txtVendorName").val(i.item.label);
                if (GstCharged == "N") {
                    $("#txtVendorName").removeClass("required").removeClass("valid").addClass("valid");
                }
                tab = false;
            }
        });
    });

    $('#txtVendorName').bind('keydown', function (e) {
        if (e.keyCode === 9) {//Tab key
            tab = true;
        }
    }).bind('focusout', function (e) {
        if (tab) {
            tab = false;
            var hdfId = $("#hdfSVendorName").val();
            var _data = $("#txtVendorName").val();

            var GstCharged = "N";
            GstCharged = $("input[name=rdogstChargedFlag]:radio[checked=checked]").attr("value");
            $("#hdfEditViewGstChargedFlag").val(GstCharged);
            $("#txtVendorName").removeClass("required").removeClass("valid");
            if (hdfId.trim() != "" && hdfId.trim() != "0" && GstCharged == "N") {
                $("#txtVendorName").addClass("valid");
                return false;
            }
            //if (hdfId.trim() == "" || hdfId.trim() == "0") {
            if ($.trim($("#txtVendorName").val()) != "") {
                var Message = "";
                var Name = $.trim($("#txtVendorName").val());

                if (hdfId.trim() != "" && hdfId.trim() != "0") {
                    Name = Name.split('-')[1];
                    Name = $.trim(Name);
                    Message = "Are you want to map this GSTIN!";
                } else {
                    Message = "Are you want to create Adhoc!";
                }

                jConfirm(Message, "Confirm", function (callback) {
                    if (callback == true) {
                        var Name = $.trim($("#txtVendorName").val());
                        if (hdfId.trim() != "" && hdfId.trim() != "0") {
                            Name = Name.split('-')[1];
                            Name = $.trim(Name);
                        }
                        var _Filter = {
                            Name: Name,
                            GSTIN: $.trim($("#txtGSTINVendor").val()),
                            LocationId: $("#ddlgstProviderLoc").val(),
                            Suppliergid: hdfId,
                            IsGST: (GstCharged == "Y" || GstCharged == "y") ? "1" : "0"
                        };
                        $.ajax({
                            type: "post",
                            url: UrlDet + "SetAdhocVendor",
                            data: JSON.stringify(_Filter),
                            async: false,
                            contentType: "application/json;",
                            success: function (response) {
                                var Data1 = "";
                                $("#txtVendorName").removeClass("required").removeClass("valid");
                                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                                    Data1 = JSON.parse(response.Data1);
                                    if (Data1.length > 0) {
                                        if (Data1[0].Clear == true) {
                                            $("#hdfSVendorName").val(Data1[0].SupplierId);
                                            $("#txtVendorName").val(Data1[0].SupplierCode + ' - ' + Name);
                                            $("#txtVendorName").attr("disabled", GstCharged == "N" ? false : true);
                                            $("#txtVendorName").addClass("valid");
                                        } else {
                                            $("#hdfSVendorName").val("0");
                                            $("#txtVendorName").val(Name);
                                            $("#txtVendorName").addClass("required");
                                            $("#txtVendorName").attr("disabled", false);
                                            jAlert(Data1[0].Message, "Message");

                                        }
                                    }
                                } else {
                                    $("#hdfSVendorName").val("0");
                                    $("#txtVendorName").val("");
                                    $("#txtVendorName").addClass("required");
                                    $("#txtVendorName").attr("disabled", false);
                                    jAlert(Data1[0].Message, "Message");

                                }
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {

                            }
                        });
                        return false;
                    } else {
                        $("#txtVendorName").addClass("required");
                        return false;
                    }
                });
            } else {
                $("#hdfSVendorName").val("0");
                $("#txtVendorName").val("");
                $("#txtVendorName").addClass("required");
            }
            //} else {
            //    $("#txtVendorName").addClass("valid");
            //}
        }
        return false;
    });

    $("#ddlgstReceiverLoc").change(function () {
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

        //var _text = $('#ddlgstProviderLoc option:selected').attr('title');
        $("#txtGSTINVendor").val("");
        $("#txtGSTINVendor").removeClass("valid").removeClass("required").addClass("required");
        $("#txtVendorName").removeClass("valid").removeClass("required");
        $("#hdfSVendorName").val("0");
        $("#txtVendorName").val("");
        $("#txtVendorName").addClass("required");
        $("#txtVendorName").attr("disabled", false);
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

    $("#txtGSTINVendor").focusout(function () {
        $("#txtGSTINVendor").removeClass("valid").removeClass("required");
        if ($("#ddlgstProviderLoc").hasClass("valid") && $.trim($("#txtGSTINVendor").val()) != "") {
            $("#txtGSTINVendor").addClass("valid");
            var _Filter = {
                ViewType: "6",
                SubrefId: $("#ddlgstProviderLoc").val(),
                GSTIN: $.trim($("#txtGSTINVendor").val())
            };
            $.ajax({
                type: "post",
                url: UrlHelper + "GetGSTNVendorDetails",
                data: JSON.stringify(_Filter),
                async: false,
                contentType: "application/json;",
                success: function (response) {
                    var Data1 = "";
                    $("#txtVendorName").removeClass("required").removeClass("valid");
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                        Data1 = JSON.parse(response.Data1);
                        if (Data1.length > 0) {
                            $("#hdfSVendorName").val(Data1[0].Id);
                            $("#txtVendorName").val(Data1[0].Value + ' - ' + Data1[0].Value1);
                            $("#txtVendorName").attr("disabled", true);
                            $("#txtVendorName").addClass("valid");

                        }
                    } else {
                        $("#hdfSVendorName").val("0");
                        $("#txtVendorName").val("");
                        $("#txtVendorName").addClass("required");
                        $("#txtVendorName").attr("disabled", false);
                        $("#txtVendorName").focus();
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {

                }
            });
        } else {
            $("#txtGSTINVendor").addClass("required");
        }
    });

    //Pandiaraj 07-05-2018 focus out 
    $("#txtGSTINvertical").focusout(function () {
        if ($("#txtGSTINVendor").val().trim() != "" && $("#txtGSTINState").val().trim() != "" && $("#txtGSTINpan").val().trim() != "" && $("#txtGSTINvertical").val().trim() != "") {
            $("#txtGSTINVendor").removeClass("valid").removeClass("required");
            if ($("#ddlgstProviderLoc").hasClass("valid") && $.trim($("#txtGSTINVendor").val()) != "") {
                $("#txtGSTINVendor").addClass("valid");
                var _Filter = {
                    ViewType: "6",
                    SubrefId: $("#ddlgstProviderLoc").val(),
                    GSTIN: $.trim($("#txtGSTINVendor").val())
                };
                $.ajax({
                    type: "post",
                    url: UrlHelper + "GetGSTNVendorDetails",
                    data: JSON.stringify(_Filter),
                    async: false,
                    contentType: "application/json;",
                    success: function (response) {
                        var Data1 = "";
                        $("#txtVendorName").removeClass("required").removeClass("valid");
                        if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                            Data1 = JSON.parse(response.Data1);
                            if (Data1.length > 0) {
                                $("#hdfSVendorName").val(Data1[0].Id);
                                $("#txtVendorName").val(Data1[0].Value + ' - ' + Data1[0].Value1);
                                $("#txtVendorName").attr("disabled", true);
                                $("#txtVendorName").addClass("valid");

                            }
                        } else {
                            $("#hdfSVendorName").val("0");
                            $("#txtVendorName").val("");
                            $("#txtVendorName").addClass("required");
                            $("#txtVendorName").attr("disabled", false);
                            $("#txtVendorName").focus();
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {

                    }
                });
            } else {
                $("#txtGSTINVendor").addClass("required");
            }
        }

        else {
            jAlert("Fill the GSTIN Details", "Message");
        }
    });
    //panidaraj
    $("#drpaudit").change(function () {
        //alert('Changed');
        if ($('#drpaudit').val() == 1) {
            $("#InvoiceHeader").css("display", "block");
            $("#showpaymentdialog").css("display", "none");
            $("#GstTaxHeader").css("display", "none");
            $("#TravelHistory").css("display", "none");

        }
        else if ($('#drpaudit').val() == 2) {
            $("#InvoiceHeader").css("display", "none");
            $("#showpaymentdialog").css("display", "none");
            $("#GstTaxHeader").css("display", "block");
            $("#TravelHistory").css("display", "none");
        }

        else if ($('#drpaudit').val() == 3) {
            $("#InvoiceHeader").css("display", "none");
            $("#showpaymentdialog").css("display", "none");
            $("#GstTaxHeader").css("display", "none");
            $("#TravelHistory").css("display", "block");
        }
        else if ($('#drpaudit').val() == 4) {
            $("#InvoiceHeader").css("display", "none");
            $("#showpaymentdialog").css("display", "block");
            $("#GstTaxHeader").css("display", "none");
            $("#TravelHistory").css("display", "none");
        }

        else {
            $("#InvoiceHeader").css("display", "none");
            $("#showpaymentdialog").css("display", "none");
            $("#GstTaxHeader").css("display", "none");
            $("#TravelHistory").css("display", "none");
        }

    });


});

function isEvent(evt) {
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

    if ($("#txtClaimFrom").hasClass("valid")) {
        $("#txtClaimFrom").removeClass("valid");
        $("#txtClaimFrom").addClass("required");
    }
    if ($("#txtClaimTo").hasClass("valid")) {
        $("#txtClaimTo").removeClass("valid");
        $("#txtClaimTo").addClass("required");
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

    if ($("#ddlPlaceFrom").hasClass("valid")) {
        $("#ddlPlaceFrom").removeClass("valid");
        $("#ddlPlaceFrom").addClass("required");
    }

    if ($("#ddlPlaceTo").hasClass("valid")) {
        $("#ddlPlaceTo").removeClass("valid");
        $("#ddlPlaceTo").addClass("required");
    }

    if ($("#ddlTravelMode").hasClass("valid")) {
        $("#ddlTravelMode").removeClass("valid");
        $("#ddlTravelMode").addClass("required");
    }

    if ($("#ddlTravelClass").hasClass("valid")) {
        $("#ddlTravelClass").removeClass("valid");
        $("#ddlTravelClass").addClass("required");
    }

    objDialogyAddDebit.dialog({ title: 'Travel Details', width: '1000', height: '420' });
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
    $("#txtClaimFrom").val("");
    $("#txtClaimTo").val("");
    $("#txtExpenseClaimMonth").val("");

    $("#ddlPlaceFrom").val("0");
    $("#ddlPlaceTo").val("0");
    $("#ddlTravelMode").val("0");
    $("#ddlTravelClass").val("0");
    $("#txtTRate").val("");
    $("#txtTDistance").val("");
    $("#trTravel").css("display", "none");
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

//Add GSTIN split on 02-03-18

$("#ddlgstProviderLoc").change(function () {

    var textprovider = $("#ddlgstProviderLoc option:selected").text().trim();
    var stategst = textprovider.substring(textprovider.length - 2, textprovider.length);
    $("#txtGSTINState").val("");
    $("#txtGSTINState").val(stategst);
    $("#txtGSTINVendor").val("");
    $("#txtGSTINVendor").val(stategst.trim() + "" + $("#txtGSTINvertical").val() + "" + $("#gstvertical").val());

    value = $("#txtGSTINVendor").val();
    if (value != "") {
        $("#txtGSTINVendor").addClass("valid");
        $("#txtGSTINVendor").removeClass("required");
    }
    else {
        $("#txtGSTINVendor").addClass("required");
        $("#txtGSTINVendor").removeClass("valid");
    }

});

$('#txtGSTINpan').keyup(function () {

    value = $("#txtGSTINpan").val();
    if (value != "") {
        $("#txtGSTINpan").addClass("valid");
        $("#txtGSTINpan").removeClass("required");
    }
    else {
        $("#txtGSTINpan").addClass("required");
        $("#txtGSTINpan").removeClass("valid");
    }


    var text = $(this).val();
    $(this).val(text.toUpperCase());
    $("#txtGSTINVendor").val("");
    $("#txtGSTINVendor").val($("#txtGSTINState").val() + "" + $("#txtGSTINpan").val() + "" + $("#txtGSTINvertical").val());

    value = $("#txtGSTINVendor").val();
    if (value != "") {
        $("#txtGSTINVendor").addClass("valid");
        $("#txtGSTINVendor").removeClass("required");
    }
    else {
        $("#txtGSTINVendor").addClass("required");
        $("#txtGSTINVendor").removeClass("valid");
    }
});

$('#txtGSTINvertical').keyup(function () {

    value = $("#txtGSTINvertical").val();
    if (value != "") {
        $("#txtGSTINvertical").addClass("valid");
        $("#txtGSTINvertical").removeClass("required");
    }
    else {
        $("#txtGSTINvertical").addClass("required");
        $("#txtGSTINvertical").removeClass("valid");
    }

    var text = $(this).val();
    $(this).val(text.toUpperCase());
    $("#txtGSTINVendor").val("");
    $("#txtGSTINVendor").val($("#txtGSTINState").val() + "" + $("#txtGSTINpan").val() + "" + $("#txtGSTINvertical").val());

    value = $("#txtGSTINVendor").val();
    if (value != "") {
        $("#txtGSTINVendor").addClass("valid");
        $("#txtGSTINVendor").removeClass("required");
    }
    else {
        $("#txtGSTINVendor").addClass("required");
        $("#txtGSTINVendor").removeClass("valid");
    }

});
//Add GSTIN split on 02-03-18 
