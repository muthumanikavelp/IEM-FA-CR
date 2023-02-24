var objDialogyTemplate, objDialogyAddTemplate, objDialogyShipment, objDialogyAddShipment, objDialogyPODetails;

UrlDet = UrlDet.replace("CBFSelection", "");
UrlHelper = UrlHelper.replace("GetProjectOwner", "");

var SearchModel = function (id) {

    var self = this;

    self.PODetailsArray = ko.observableArray();
    self.POShipmentArray = ko.observableArray();
    self.POAuditTrailArray = ko.observableArray();

    self.ProjectOwnerArray = ko.observableArray();
    self.POTemplateArray = ko.observableArray();
    self.ShipmentTypeArray = ko.observableArray();
    self.ProductNameArray = ko.observableArray();
    self.UOMArray = ko.observableArray();
    self.POAttachment = ko.observableArray();

    this.GetPO = function () {
        var POHeaderGId = $("#hfPO").val();
        if (POHeaderGId != "0") {
            var inputFilter = {
                POHeaderGId: POHeaderGId
            };
            $.ajax({
                type: "post",
                url: UrlDet + "GetPO",
                data: JSON.stringify(inputFilter),
                contentType: "application/json;",
                success: function (response) {
                    var Data1 = "", Data2 = "", Data3 = "", Data4 = "";
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                        Data1 = JSON.parse(response.Data1);

                    }
                    if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                        Data2 = JSON.parse(response.Data2);
                        if ($.trim(Data2[0].Department) == 'IT') {
                            $("input[name=rdoType]:radio").removeAttr("disabled");
                            //$("input[name=rdoType][value=A]").prop("checked", true);
                        }
                        $("#txtPORefNo").val(Data2[0].PONo);
                        $("#txtPODate").val(Data2[0].PODate);
                        $("#txtPOEndDate").val(Data2[0].POEndDate);
                        $("#txtRaisedBy").val(Data2[0].RaisedBy);
                        $("#ddlProductOwner").val(Data2[0].ProjectOwnerId);
                        $("#txtDepartment").val(Data2[0].Department);
                        $("#txtVendorName").val(Data2[0].Vendor);
                        $("#txtPanno").val(Data2[0].panno);
                        $("#txtVendorCode").val(Data2[0].code)
                        $("#hfVendorName").val(Data2[0].VendorId);
                        $("#txtVendorNote").val(Data2[0].VendorNote);
                        $("#txtPOTemplate").val(Data2[0].TermAndCondName);
                        $("input[name=rdoType][value=" + Data2[0].ITType + "]").prop("checked", true);
                        $("#ddlTemplate").val(Data2[0].TermAndCondGId);
                        $("#txtTemrms").val(Data2[0].TermAndCondContent);
                        $("#txtConditions").val(Data2[0].TermAndCondAdded);
                        $("#lblTotalAmount").text(Data2[0].POTotalAmount);
                        $("#txtCancelRemarks").val(Data2[0].CancelMkrRemarks);
                        $("#txtCloserRemarks").val(Data2[0].ClosureMkrRemarks);
                        $("#hfStatus").val(Data2[0].Status);
                        $("#txtVersion").text(Data2[0].Version);

                        valueChange($("#txtPORefNo"));
                        valueChange($("#txtPODate"));
                        valueChange($("#txtPOEndDate"));
                        valueChange($("#txtRaisedBy"));
                        valueChange($("#txtDepartment"));
                        valueChange($("#txtVendorName"));
                        valueChange($("#txtVendorCode"));
                        valueChange($("#txtPanno"));
                        valueChange($("#txtVendorNote"));
                        valueChange($("#txtPOTemplate"));
                        selectChange($("#ddlProductOwner"));
                        selectChange($("#ddlTemplate"));

                        if (parseInt(Data2[0].Status) > 1 && parseInt(Data2[0].Status) != 6) {
                            $(".hideStatus").css("display", "none");
                            $(".viewStatus").css("display", "");
                            $(".disableView").attr("disabled", "disabled");
                            $(".btn-primary").css("display", "none");
                            $(".attdelete").css("display", "none");
                        }

                        if (parseInt($.trim($("#hfViewMode").val())) == 0 || parseInt($.trim($("#hfViewMode").val())) == 2) {
                            $(".hideStatus").css("display", "none");
                            $(".viewStatus").css("display", "");
                            $(".disableView").attr("disabled", "disabled");
                            $(".btn-primary").css("display", "none");
                            $(".attdelete").css("display", "none");
                        }
                        if (parseInt($.trim($("#hfViewMode").val())) == 2) {
                            $("#btnPODelete").css("display", "");
                        }
                        if (parseInt(Data2[0].Status) == 1) {
                            $(".viewResubmit").css("display", "none");
                        }
                        if (parseInt(Data2[0].Status) == 6) {
                            $(".viewResubmit").css("display", "");
                        }

                        var Type = $("input[name=rdoType]:radio[checked=checked]").attr("value");
                        if ($.trim(Data2[0].Department) == 'IT' && (typeof Type === "undefined" || typeof Type === "null" || Type == "")) {
                            $("#rdoApplication").attr("checked", "checked");
                        }
                    }

                    if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null) {
                        Data3 = JSON.parse(response.Data3);
                        $("[name=ProdServFlagNew]").val(Data3[0].ProdServiceFlag);
                    }
                      
                    self.PODetailsArray(Data3);

                    if (response.Data4 != null && response.Data4 != "" && JSON.parse(response.Data4) != null)
                        Data4 = JSON.parse(response.Data4);
                    self.POAuditTrailArray(Data4);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //alert(errorThrown);
                }
            });
        }

    };

    this.SetPOTemplate = function () {

        var TemplateGId = "0";
        var TemplateName = $("#txtTemplateName").val();
        var Content = $("#txtTermsCoditions").val();
        var IsRemoved = "0";

        if ($.trim(TemplateName) == "") {
            jAlert("Ensure Template Name!", "Message");
            return false;
        }

        if ($.trim(Content) == "") {
            jAlert("Ensure Terms And Conditions!", "Message");
            return false;
        }

        var inputFilter = {
            TemplateGId: TemplateGId,
            TemplateName: TemplateName,
            Content: Content,
            IsRemoved: IsRemoved
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetPOTemplate",
            data: JSON.stringify(inputFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == true) {
                        objDialogyAddTemplate.dialog("close");
                        self.LoadPOTemplate();
                        jAlert(Data1[0].Message, "Message");
                        $("#ddlTemplate").val("0");
                        $("#txtTemrms").val("");
                        $("#txtPOTemplate").val("");
                        selectChange($("#ddlTemplate"));
                        valueChange($("#txtPOTemplate"));
                    }
                    else {
                        jAlert(Data1[0].Message, "Message");
                    }

                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });


    };

    this.SetShipment = function () {

        var ShipmentGId = $("#hfShipmentId").val();
        var PODetailGId = $("#hfPODetailsGId").val();
        var ShipmentTypeGId = $("#ddlShipmentType").val();
        var BranchGId = $("#hfBranchName").val();
        var InchargeGId = $("#hfInchargeID").val();
        var ShippedQty = $("#txtShipmentQty").val();
        var IsRemoved = "0";

        if ($.trim(PODetailGId) == "0") {
            jAlert("Ensure Valid Data!", "Message");
            return false;
        }
        if ($.trim(ShipmentTypeGId) == "0") {
            jAlert("Ensure Shipment Type!", "Message");
            return false;
        }
        if ($.trim(BranchGId) == "0") {
            jAlert("Ensure Branch Code & Name!", "Message");
            return false;
        }
        if ($.trim(InchargeGId) == "0" || $.trim(InchargeGId) == "") {
            jAlert("Ensure Incharge ID!", "Message");
            return false;
        }
        if ($.trim(ShippedQty) == "" || parseFloat(ShippedQty) < 0.01) {
            jAlert("Ensure Shipment Qty!", "Message");
            return false;
        }

        var inputFilter = {
            ShipmentGId: ShipmentGId,
            PODetailGId: PODetailGId,
            ShipmentTypeGId: ShipmentTypeGId,
            BranchGId: BranchGId,
            InchargeGId: InchargeGId,
            ShippedQty: ShippedQty,
            IsRemoved: IsRemoved
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetShipment",
            data: JSON.stringify(inputFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == true) {
                        objDialogyAddShipment.dialog("close");
                        jAlert(Data1[0].Message, "Message");
                    }
                    else {
                        jAlert(Data1[0].Message, "Message");
                    }
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                self.POShipmentArray(Data2);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.EditShipment = function (item) {

        $('#DialogShipmentCreate').attr("style", "display:block;");

        $('#hfShipmentId').val(item.ShipmentGId);
        $('#ddlShipmentType').val(item.ShipmentTypeGId);
        $('#txtBranchName').val(item.BranchCode + " - " + item.BranchName);
        $('#hfBranchName').val(item.BranchGId);
        $('#txtAddress').val(item.BranchAddress);
        $('#txtLocation').val(item.LocationName);
        $('#txtInchargeID').val(item.InchargeName);
        $('#hfInchargeID').val(item.InchargeId);
        $('#txtShipmentQty').val(item.Qty);

        selectChange($("#ddlShipmentType"));
        valueChange($("#txtBranchName"));
        valueChange($("#txtAddress"));
        valueChange($("#txtLocation"));
        valueChange($("#txtInchargeID"));
        valueChange1($("#txtShipmentQty"));

        objDialogyAddShipment.dialog({ title: 'Add Shipment Details', width: '600', height: '290' });
        objDialogyAddShipment.dialog("open");
        return false;
    };

    this.DeleteShipment = function (ShipmentGId) {

        jConfirm("Are you sure? Want to delete Shipment Details!", "Confirm", function (callback) {
            if (callback == true) {
                var PODetailGId = $("#hfPODetailsGId").val();
                var ShipmentTypeGId = "0";
                var BranchGId = "0";
                var InchargeGId = "0";
                var ShippedQty = "0";
                var IsRemoved = "1";

                var inputFilter = {
                    ShipmentGId: ShipmentGId,
                    PODetailGId: PODetailGId,
                    ShipmentTypeGId: ShipmentTypeGId,
                    BranchGId: BranchGId,
                    InchargeGId: InchargeGId,
                    ShippedQty: ShippedQty,
                    IsRemoved: IsRemoved
                };
                $.ajax({
                    type: "post",
                    url: UrlDet + "SetShipment",
                    data: JSON.stringify(inputFilter),
                    contentType: "application/json;",
                    success: function (response) {
                        var Data1 = "", Data2 = "";
                        if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                            Data1 = JSON.parse(response.Data1);
                            if (Data1[0].Clear == true) {
                                jAlert(Data1[0].Message, "Message");
                            }
                            else {
                                jAlert(Data1[0].Message, "Message");
                            }
                        }
                        if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                            Data2 = JSON.parse(response.Data2);
                        self.POShipmentArray(Data2);
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

    this.SetPODetails = function () {

        var POHeaderGId = $("#hfPO").val();
        var PODetailGId = $("#hfPODetailsGId").val();
        var Description = $("#txtProductDesc").val();

        var ProductGId = $("#ddlProductName").val();
        var UOM = $("#ddlUOM").val();

        var Qty = $.trim($("#txtQty").val()) == "" ? 0 : parseFloat($.trim($("#txtQty").val()));
        var UnitPrice = $.trim($("#txtRate").val()) == "" ? 0 : parseFloat($.trim($("#txtRate").val()));
        var BaseAmount = $.trim($("#txtBaseAmount").val()) == "" ? 0 : parseFloat($.trim($("#txtBaseAmount").val()));
        var Discount = $.trim($("#txtDiscount").val()) == "" ? 0 : parseFloat($.trim($("#txtDiscount").val()));
        var Taxes = $.trim($("#txtTaxes").val()) == "" ? 0 : parseFloat($.trim($("#txtTaxes").val()));
        var TaxPercent = $.trim($("#txtTaxPercent").val()) == "" ? 0 : parseFloat($.trim($("#txtTaxPercent").val()));
        var OtherCharges = $.trim($("#txtOtherCharges").val()) == "" ? 0 : parseFloat($.trim($("#txtOtherCharges").val()));
        var NetAmount = $.trim($("#txtNetAmount").val()) == "" ? 0 : parseFloat($.trim($("#txtNetAmount").val()));
        var IsRemoved = "0";

        if ($.trim(ProductGId) == "" || $.trim(ProductGId) == "0") {
            jAlert("Ensure Product!", "Message");
            return false;
        }
        if ($.trim(Description) == "") {
            jAlert("Ensure Product Description!", "Message");
            return false;
        }
        if ($.trim(UOM) == "" || $.trim(UOM) == "0") {
            jAlert("Ensure UOM!", "Message");
            return false;
        }
        if (Qty == 0) {
            jAlert("Ensure Qty!", "Message");
            return false;
        }
        if (UnitPrice == 0) {
            jAlert("Ensure Rate!", "Message");
            return false;
        }
        var flag = $('#hfPODetailsFlag').val();
        if (flag == "0") {
            var inputFilter = {
                POHeaderGId: POHeaderGId,
                PODetailGId: PODetailGId,
                ProductGId: ProductGId,
                Description: Description,
                UOM: UOM,
                Qty: Qty,
                UnitPrice: UnitPrice,
                BaseAmount: BaseAmount,
                Discount: Discount,
                Taxes: Taxes,
                TaxPercent: TaxPercent,
                OtherCharges: OtherCharges,
                NetAmount: NetAmount,
                IsRemoved: IsRemoved
            };
            var Url = "SetPODetails";
        }
        else {
            var parenttotal = $("[name=hidNetParent]").val();
            if (parseFloat(parenttotal) <= parseFloat(NetAmount)) {
                jAlert("Split Amount Exceeds the Parent Item Amount!", "Message", function () {
                    return false;
                });
                return false;
            }
            else {
                
                var isService = $("[name=ProdServFlagNew]").val();
                var Perc = "0.0";
                if ($("[name=txtIsPercentage]").is(':checked')) {
                    Perc = $.trim($("#txtPercentNew").val()) == "" ? 0 : parseFloat($.trim($("#txtPercentNew").val()));
                    if (Perc == "0" || Perc == "" || Perc == null) {
                        jAlert("Ensure Valid Percentage", "Message", function () {
                            $("#txtPercentNew").focus();
                            return false;
                        });
                        return false;
                    }
                }
               
                var inputFilter = {
                    POHeaderGId: POHeaderGId,
                    PODetailGId: PODetailGId,
                    ProductGId: ProductGId,
                    Description: Description,
                    UOM: UOM,
                    Qty: Qty,
                    UnitPrice: UnitPrice,
                    BaseAmount: BaseAmount,
                    Discount: Discount,
                    Taxes: Taxes,
                    TaxPercent: TaxPercent,
                    OtherCharges: OtherCharges,
                    NetAmount: NetAmount,
                    POPercent:Perc
                };
                var Url = "SplitPODetails";
            }
        }
      //  self.PODetailsArray.removeAll();
        $.ajax({
            type: "post",
            cache: false,
            async: false,
            url: UrlDet + Url,
            data: JSON.stringify(inputFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                // $('#CBFDetailsGrid tbody').empty();
              //  objDialogyPODetails.dialog("close");
              
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == true) {
                        if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                            Data2 = JSON.parse(response.Data2);
                        self.PODetailsArray(Data2);
                     
                        jAlert(Data1[0].Message, "Message", function () {
                            $("#lblTotalAmount").text(Data1[0].POTotalAmount);
                            objDialogyPODetails.dialog("close");
                           // return false;
                        });
                    
                    }
                    else {
                        jAlert(Data1[0].Message, "Message");
                    }
                }
                

                
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.EditPODetails = function (item) {
        $('#PODetailsDialog').attr("style", "display:block;");
        $("#isServiceDiv").css("display", "none");

        self.LoadProductName(item.ProductGroupGId);

        $('#hfPODetailsGId').val(item.PODetailsGId);
        $('#txtProductGroup').val(item.ProductGroup);
        $('#txtProductName').val(item.ProductName);
        $('#ddlProductName').val(item.ProductGId);
        $('#txtProductDesc').val(item.ProductDesc);
        $('#txtUOM').val(item.UOM);
        $('#ddlUOM').val(item.UOMGId);
        $('#txtQty').val(item.Qty);
        $('#txtRate').val(item.UnitPrice);
        $('#txtBaseAmount').val(item.BaseAmount);
        $('#txtDiscount').val(item.Discount);
        $('#txtTaxes').val(item.Taxes);
        $('#txtTaxPercent').val(item.TaxPercent);
        $('#txtOtherCharges').val(item.OtherCharges);
        $('#txtNetAmount').val(item.Total);

        valueChange($("#txtProductDesc"));
        valueChange($("#txtUOM"));
        valueChange1($("#txtQty"));
        valueChange1($("#txtRate"));
        selectChange($("#ddlProductName"));
        selectChange($("#ddlUOM"));

        $('#hfPODetailsFlag').val("0");

        objDialogyPODetails.dialog({ title: 'Add PO Details', width: '900', height: '390' });
        objDialogyPODetails.dialog("open");
        return false;
    };

    this.SharePODetails = function (item) {
        //if (parseInt(item.Qty) < 2) {
        //    jAlert("Product Quantity Should be greater than 1", "Message");
        //    return false;
        //}
        var isService = $("[name=ProdServFlagNew]").val();
        $("[name=txtIsPercentage]").attr('checked', false);
     //   if (isService == "S") {
        //    $("#isServiceDiv").css("display", "");
            $("[name=hidPerc]").val(item.POPercentage);
            $("[id=txtPercentNewMax]").val(item.POPercentage);
            var totold = parseFloat(item.Total) * 100 / parseFloat(item.POPercentage);
            totold = parseFloat(totold).toFixed(2);
            $("[name=hidPercTotal]").val(totold);
     //   }
     //   else{
            $("#isservicediv").css("display", "none");
            $("#isservicediv1").css("display", "none");
     //   }
      
        $('#PODetailsDialog').attr("style", "display:block;");
        self.LoadProductName(item.ProductGroupGId);
        $('#hfPODetailsGId').val(item.PODetailsGId);
        $('#txtProductGroup').val(item.ProductGroup);
        $('#txtProductName').val(item.ProductName);
        $('#ddlProductName').val(item.ProductGId);
        $('#txtProductDesc').val(item.ProductDesc);
        $('#txtUOM').val(item.UOM);
        $('#ddlUOM').val(item.UOMGId);
        $('#txtQty').val("1");
        $('#txtRate').val(item.UnitPrice);
        $('#txtBaseAmount').val(item.UnitPrice);
        $('#txtDiscount').val("0");
        $('#txtTaxes').val("0");
        $('#txtOtherCharges').val("0");
        $('#txtNetAmount').val(item.UnitPrice);

        valueChange($("#txtProductDesc"));
        valueChange($("#txtUOM"));
        valueChange1($("#txtQty"));
        valueChange1($("#txtRate"));

        $('#hfPODetailsFlag').val("1");
        $("[name=hidNetParent]").val(item.Total);

        objDialogyPODetails.dialog({ title: 'Split PO Details', width: '900', height: '390' });
        objDialogyPODetails.dialog("open");
        return false;
    };

    this.DeletePODetails = function (PODetailsGId) {

        jConfirm("Are you sure? Want to delete PO Details!", "Confirm", function (callback) {
            if (callback == true) {

                var POHeaderGId = $("#hfPO").val();
                var Description = "";
                var Qty = "0";
                var UnitPrice = "0";
                var BaseAmount = "0";
                var Discount = "0";
                var Taxes = "0";
                var TaxPercent = "0";
                var OtherCharges = "0";
                var NetAmount = "0";
                var IsRemoved = "1";

                var inputFilter = {
                    POHeaderGId: POHeaderGId,
                    PODetailGId: PODetailsGId,
                    Description: Description,
                    Qty: Qty,
                    UnitPrice: UnitPrice,
                    BaseAmount: BaseAmount,
                    Discount: Discount,
                    Taxes: Taxes,
                    TaxPercent: TaxPercent,
                    OtherCharges: OtherCharges,
                    NetAmount: NetAmount,
                    IsRemoved: IsRemoved
                };
                $.ajax({
                    type: "post",
                    url: UrlDet + "SetPODetails",
                    data: JSON.stringify(inputFilter),
                    contentType: "application/json;",
                    success: function (response) {
                        var Data1 = "", Data2 = "";
                        if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                            Data1 = JSON.parse(response.Data1);
                            if (Data1[0].Clear == true) {
                                jAlert(Data1[0].Message, "Message");
                                $("#lblTotalAmount").text(Data1[0].POTotalAmount);
                            }
                            else {
                                jAlert(Data1[0].Message, "Message");
                            }
                        }
                        if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                            Data2 = JSON.parse(response.Data2);
                        self.PODetailsArray(Data2);
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

    this.SubmitPO = function () {

        var POHeaderGId = $("#hfPO").val();
        var POEndDate = $("#txtPOEndDate").val();
        var ProjectOwnerGId = $("#ddlProductOwner").val();
        var VendorGId = $("#hfVendorName").val();
        var Type = $("input[name=rdoType]:radio[checked=checked]").attr("value");
        var VendorNote = $("#txtVendorNote").val();
        var TemplateGId = $("#ddlTemplate").val();
        var AddedTermAndCondtion = $("#txtConditions").val();
        var IsRemoved = "0";
        var ViewType = "1";
        var IsReject = "0";
        var Remarks = "";
        var Department = $("#txtDepartment").val();

        if ($.trim(POHeaderGId) == "0") {
            jAlert("Ensure Valid Data!", "Message");
            return false;
        }
        if ($.trim(POEndDate) == "") {
            jAlert("Ensure PO End Date!", "Message");
            return false;
        }
        if ($.trim(ProjectOwnerGId) == "0") {
            jAlert("Ensure Project Owner!", "Message");
            return false;
        }
        if ($.trim(VendorGId) == "0") {
            jAlert("Ensure Vendor Name!", "Message");
            return false;
        }
        if ($.trim(Department) == 'IT' && (typeof Type === "undefined" || typeof Type === "null" || Type == "")) {
            jAlert("Ensure Type!", "Message");
            return false;
        }
        if ($.trim(VendorNote) == "") {
            jAlert("Ensure Vendor Note!", "Message");
            return false;
        }

        if ($.trim(TemplateGId) == "0") {
            jAlert("Ensure PO Template!", "Message");
            return false;
        }

        var inputFilter = {
            POHeaderGId: POHeaderGId,
            POEndDate: POEndDate,
            ProjectOwnerGId: ProjectOwnerGId,
            VendorGId: VendorGId,
            Type: Type,
            VendorNote: VendorNote,
            TemplateGId: TemplateGId,
            AddedTermAndCondtion: AddedTermAndCondtion,
            IsRemoved: IsRemoved,
            ViewType: ViewType,
            IsReject: IsReject,
            Remarks: Remarks

        };
        $.ajax({
            type: "post",
            url: UrlDet + "SubmitPO",
            data: JSON.stringify(inputFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == true) {
                        jAlert(Data1[0].Message, "Success", function () {
                            location = UrlReturn;
                        });
                    }
                    else {
                        jAlert(Data1[0].Message, "Message");
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.ApprovePO = function (reject) {

        var POHeaderGId = $("#hfPO").val();
        var POEndDate = $("#txtPOEndDate").val();
        var ProjectOwnerGId = $("#ddlProductOwner").val();
        var VendorGId = $("#hfVendorName").val();
        var Type = $("input[name=CBFMode]:radio[checked=checked]").attr("value");
        var VendorNote = $("#txtVendorNote").val();
        var TemplateGId = $("#ddlTemplate").val();
        var AddedTermAndCondtion = $("#txtConditions").val();
        var IsRemoved = "0";
        var ViewType = $("#hfType").val();
        var IsReject = reject;
        var Remarks = $("#txtApproveRemarks").val();

        if ($.trim(POHeaderGId) == "0") {
            jAlert("Ensure Valid Data!", "Message");
            return false;
        }
        if (IsReject == "1") {
            if ($.trim(Remarks) == "") {
                jAlert("Ensure Remarks!", "Message");
                return false;
            }
        }
        var inputFilter = {
            POHeaderGId: POHeaderGId,
            POEndDate: POEndDate,
            ProjectOwnerGId: ProjectOwnerGId,
            VendorGId: VendorGId,
            Type: Type,
            VendorNote: VendorNote,
            TemplateGId: TemplateGId,
            AddedTermAndCondtion: AddedTermAndCondtion,
            IsRemoved: IsRemoved,
            ViewType: ViewType,
            IsReject: IsReject,
            Remarks: Remarks

        };
        $.ajax({
            type: "post",
            url: UrlDet + "SubmitPO",
            data: JSON.stringify(inputFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == true) {
                        jAlert(Data1[0].Message, "Success", function () {
                            location = UrlReturn;
                        });
                    }
                    else {
                        jAlert(Data1[0].Message, "Message");
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.AddShipmentDetails = function (PODetailsGId, Qty) {

        $("#hfPODetailsGId").val(PODetailsGId);
        $("#hfQty").val(Qty);

        var inputFilter = {
            PODetailGId: PODetailsGId
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetShipment",
            data: JSON.stringify(inputFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.POShipmentArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });

        $('#ShipmentDialog').attr("style", "display:block;");
        objDialogyShipment.dialog({ title: 'Shipment Details', width: '1000', height: '400' });
        objDialogyShipment.dialog("open");
        return false;
    };

    this.LoadProjectOwner = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetProjectOwner",
            data: "{}",
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.ProjectOwnerArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.LoadPOTemplate = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetPOTemplate",
            data: "{}",
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.POTemplateArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.LoadShipmentType = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetShipmentType",
            data: "{}",
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.ShipmentTypeArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.ChangePOTemplate = function () {
        for (var i = 0; i < self.POTemplateArray().length; i++) {
            if (self.POTemplateArray()[i].Id == $("#ddlTemplate").val()) {
                $("#txtTemrms").val(self.POTemplateArray()[i].Content);
                return false;
            }
        }
    };

    this.TableViewBind = function () {
        if (parseInt($("#hfStatus").val()) > 1 && parseInt($("#hfStatus").val()) != 6) {
            $(".hideStatus").css("display", "none");
        }
        if (parseInt($.trim($("#hfViewMode").val())) == 0 || parseInt($.trim($("#hfViewMode").val())) == 2) {
            $(".hideStatus").css("display", "none");
        }
    };

    this.LoadProductName = function (CatId) {
        var CatFilter = {
            CatId: CatId
        };
        $.ajax({
            type: "post",
            url: UrlHelper + "GetProductName",
            data: JSON.stringify(CatFilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.ProductNameArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.LoadUOM = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetUOM",
            data: "{}",
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.UOMArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.ChangeProductName = function () {
        for (var i = 0; i < self.ProductNameArray().length; i++) {
            if (self.ProductNameArray()[i].Id == $("#ddlProductName").val()) {
                $("#txtProductDesc").val(self.ProductNameArray()[i].Description);
                valueChange($("#txtProductDesc"));
                return false;
            }
        }
    };
    //Dhasarathan
    this.LoadPOAttachment = function () {
        var POHeaderGId = $("#hfPO").val();
        if (POHeaderGId != "0") {
            var inputFilter = {
                POHeaderGId: POHeaderGId
            };
            $.ajax({
                type: "post",
                url: UrlDet + "GetPoAttachments",
                data: JSON.stringify(inputFilter),
                async: false,
                contentType: "application/json;",
                success: function (response) {
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                        Data1 = JSON.parse(response.Data1);
                        self.POAttachment(Data1);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //alert(errorThrown);
                }
            });
        }
    };
    this.SavePoAttachment = function () {

        var POAttachFileName = "";
        var POHeaderGId = $("#hfPO").val();
        POAttachFileName = $("#AttachedActualFileName").val();
        var AttachDescription = $("#AttachDescription").val();
        var AttachmentRefGid = POHeaderGId;

        if ($.trim(POAttachFileName) == "" || $.trim(POAttachFileName) == null) {
            jAlert("Please Select Valid File", "Error", function () {
                return false;
            });
            return false;
        }

        var POAttachmentInsert = {
            "AttachedActualFileName": POAttachFileName,
            "AttachDescription": AttachDescription,
            "AttachmentRefGid": AttachmentRefGid,
            "AttachmentFor": "PO"
        };

        $.ajax({
            type: "post",
            url: UrlDet + "CreatePOAttachment",
            data: JSON.stringify(POAttachmentInsert),
            contentType: "application/json;",
            success: function (response) {
                if (response.ErrorMsg == "0") {
                    jAlert("Saved Successfully", "Message");
                    self.LoadPOAttachment();
                }
                else { jAlert("Application error", "Message"); }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    }
    this.DownloadAttachment = function (id, filename) {
        var id = {
            id: id,
            filename: filename
        };
        location = UrlDet + "/DownloadDocument?id=" + id.id + "&filename=" + filename;
    }
    this.DeleteAttachment = function (id) {
        var id = {
            id: id
        };

        $.ajax({
            type: "post",
            url: UrlDet + "DeleteAttachment",
            data: JSON.stringify(id),
            contentType: "application/json;",
            success: function (response) {
                if (response.ErrorMsg == "0") {
                    jAlert("Deleted Successfully", "Message");
                    self.LoadPOAttachment();
                }
                else { jAlert("Application error", "Message"); }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    }
    //Dhasarathan

    self.LoadProjectOwner();
    self.LoadPOTemplate();
    self.LoadShipmentType();
    self.LoadProductName("0");
    self.LoadUOM();
    self.GetPO();
    self.LoadPOAttachment();
};

var mainViewModel = new SearchModel();
ko.applyBindings(mainViewModel);

$(document).ready(function () {
    objDialogyTemplate = $("[id$='POTemplateDialog']");
    objDialogyTemplate.dialog({
        autoOpen: false,
        modal: true,
        width: 1100,
        height: 650,
        duration: 300

    });

    objDialogyAddTemplate = $("[id$='CreateTemplateDialog']");
    objDialogyAddTemplate.dialog({
        autoOpen: false,
        modal: true,
        width: 1000,
        height: 450,
        duration: 300
    });

    objDialogyShipment = $("[id$='ShipmentDialog']");
    objDialogyShipment.dialog({
        autoOpen: false,
        modal: true,
        width: 1000,
        height: 400,
        duration: 300
    });

    objDialogyAddShipment = $("[id$='DialogShipmentCreate']");
    objDialogyAddShipment.dialog({
        autoOpen: false,
        modal: true,
        width: 600,
        height: 400,
        duration: 300
    });

    objDialogyPODetails = $("[id$='PODetailsDialog']");
    objDialogyPODetails.dialog({
        autoOpen: false,
        modal: true,
        width: 800,
        height: 300,
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

    $("#txtPODate").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        maxDate: 'd',
        onSelect: function (selected) {
            valueChange($("#txtPODate"));
        }
    });

    $("#txtPOEndDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        numberOfMonths: 1,
        minDate: 'd',
        onSelect: function (selected) {
            valueChange($("#txtPOEndDate"));
        }
    });

    //PO Header Section
    $("#txtVendorNote").change(function () {
        valueChange($("#txtVendorNote"));
    });

    $("#ddlProductOwner").change(function () {
        selectChange($("#ddlProductOwner"));
    });

    $("#ddlTemplate").change(function () {
        selectChange($("#ddlTemplate"));
        if ($("#ddlTemplate").val() == "0")
            $("#txtPOTemplate").val("");
        else
            $("#txtPOTemplate").val($("#ddlTemplate option:selected").text());
        valueChange($("#txtPOTemplate"));
    });

    $("#txtVendorName").keyup(function (e) {
        if (e.which != 13) {
            $("#hfVendorName").val("0");
            $("#txtVendorName").removeClass("valid");
            $("#txtVendorName").addClass("required");
        }

        $("#txtVendorName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteVendor",
                    data: "{ 'txt' : '" + $("#txtVendorName").val() + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                label: item.split('~')[1],
                                val: item.split('~')[0],
                                code:item.split('~')[2],
                                panno:item.split('~')[3]
                            }
                        }))
                    },
                    error: function (response) {
                    },
                    failure: function (response) {
                    }
                });
            },
            select: function (e, i) {
                $("#hfVendorName").val(i.item.val);
                $("#txtVendorName").val(i.item.label);
                $("#txtVendorCode").val($.trim(i.item.code));
                $("#txtPanno").val(i.item.panno);
                $("#txtVendorName").removeClass("required");
                $("#txtVendorName").addClass("valid");
                $("#txtPanno").removeClass("required");
                $("#txtPanno").addClass("valid");
                $("#txtVendorCode").removeClass("required");
                $("#txtVendorCode").addClass("valid");
            },
            minLength: 1
        });

    });

    $("#txtVendorName").focusout(function () {
        var hdfId = $("#hfVendorName").val();
        var txtCurName = $("#txtVendorName").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtVendorName").val("");
        }
        if (txtCurName == "") {
            $("#txtPanno").val("");
            $("#txtVendorCode").val("");
        }
    });

    $("#txtTemplateName").keyup(function (e) {
        valueChange($("#txtTemplateName"));
    });

    $("#txtTermsCoditions").keyup(function (e) {
        valueChange($("#txtTermsCoditions"));
    });

    $("#txtBranchName").keyup(function (e) {
        if (e.which != 13) {
            $("#hfBranchName").val("0");
            $("#txtBranchName").removeClass("valid");
            $("#txtBranchName").addClass("required");
        }

        $("#txtBranchName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteBranchPO",
                    data: "{ 'txt' : '" + $("#txtBranchName").val() + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                label: item.split('~')[1],
                                val: item.split('~')[0],
                                address: item.split('~')[2],
                                location: item.split('~')[3],
                                incharge: item.split('~')[4],
                                empid: item.split('~')[5]
                            }
                        }))
                    },
                    error: function (response) {
                    },
                    failure: function (response) {
                    }
                });
            },
            select: function (e, i) {
                $("#hfBranchName").val(i.item.val);
                $("#txtBranchName").val(i.item.label);
                $("#txtAddress").val($.trim(i.item.address));
                valueChange($("#txtAddress"));
                $("#txtLocation").val($.trim(i.item.location));
                valueChange($("#txtLocation"));
                $("#txtInchargeID").val($.trim(i.item.incharge));
                $("#hfInchargeID").val($.trim(i.item.empid));
                valueChange($("#txtInchargeID"));
                $("#txtBranchName").removeClass("required");
                $("#txtBranchName").addClass("valid");
            },
            minLength: 1
        });

    });

    $("#txtBranchName").focusout(function () {
        var hdfId = $("#hfBranchName").val();
        var txtCurName = $("#txtBranchName").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtBranchName").val("");
            $("#txtAddress").val("");
            $("#txtLocation").val("");
            $("#hfInchargeID").val("0");
            $("#txtInchargeID").val("");
            valueChange($("#txtAddress"));
            valueChange($("#txtLocation"));
            valueChange($("#txtInchargeID"));
        }
    });

    $("#txtInchargeID").keyup(function (e) {
        if (e.which != 13) {
            $("#hfInchargeID").val("0");
            $("#txtInchargeID").removeClass("valid");
            $("#txtInchargeID").addClass("required");
        }

        $("#txtInchargeID").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteEmployee",
                    data: "{ 'txt' : '" + $("#txtInchargeID").val() + "'}",
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
                    },
                    failure: function (response) {
                    }
                });
            },
            select: function (e, i) {
                $("#hfInchargeID").val(i.item.val);
                $("#txtInchargeID").val(i.item.label);
                $("#txtInchargeID").removeClass("required");
                $("#txtInchargeID").addClass("valid");
            },
            minLength: 1
        });

    });

    $("#txtInchargeID").focusout(function () {
        var hdfId = $("#hfInchargeID").val();
        var txtCurName = $("#txtInchargeID").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtInchargeID").val("");
        }
    });

    $("#ddlShipmentType").change(function () {
        selectChange($("#ddlShipmentType"));
    });

    $("#txtShipmentQty").keyup(function (e) {
        valueChange1($("#txtShipmentQty"));
    });

    $("#txtQty").keyup(function (e) {
        TotalCalculation();
    });

    $("#txtRate").keyup(function (e) {
        TotalCalculation();
    });
    $("#txtPercentNew").keyup(function (e) {
        TotalCalculationNew();
    });
    $("#txtTaxPercent").keyup(function (e) {
        TotalCalculation();
    });

    $("#txtDiscount").keyup(function (e) {
        var Discount = $.trim($("#txtDiscount").val()) == "" ? 0 : parseFloat($.trim($("#txtDiscount").val()));
        var BaseAmount = $.trim($("#txtBaseAmount").val()) == "" ? 0 : parseFloat($.trim($("#txtBaseAmount").val()));
        if (BaseAmount < Discount) {
            jAlert("Discount should less than Base Amount!", "Message");
            $("#txtDiscount").val("0");
        }
        TotalCalculation();
    });

    $("#txtTaxes").keyup(function (e) {
        var Tax = $.trim($("#txtTaxes").val()) == "" ? 0 : parseFloat($.trim($("#txtTaxes").val()));
        var BaseAmount = $.trim($("#txtBaseAmount").val()) == "" ? 0 : parseFloat($.trim($("#txtBaseAmount").val()));
        if (BaseAmount < Tax) {
            jAlert("Taxes should less than Base Amount!", "Message");
            $("#txtTaxes").val("0");
        }
        TotalCalculation();
    });

    $("#txtOtherCharges").keyup(function (e) {
        var Others = $.trim($("#txtOtherCharges").val()) == "" ? 0 : parseFloat($.trim($("#txtOtherCharges").val()));
        var BaseAmount = $.trim($("#txtBaseAmount").val()) == "" ? 0 : parseFloat($.trim($("#txtBaseAmount").val()));
        TotalCalculation();
    });

    $("#txtApproveRemarks").keyup(function (e) {
        valueChange($("#txtApproveRemarks"));
    });

    $(".NumberValidation").keypress(function (e) {
        var keyCode = e.which;
        if (keyCode > 31 && (keyCode < 48 || keyCode > 57) && keyCode != 46)
            return false;
        return true;
    });

    $(".OnlyNumeric").keypress(function (e) {
        var keyCode = e.which;
        if (keyCode > 31 && (keyCode < 48 || keyCode > 57))
            return false;
        return true;
    });
});


function isEvent(evt) {
    return false;
}

function valueChange(obj) {
    var ReturnDate = $.trim(obj.val());
    if (ReturnDate != "") {
        obj.addClass("valid");
        obj.removeClass("required");
    }
    else {
        obj.addClass("required");
        obj.removeClass("valid");
    }
}

function valueChange1(obj) {
    var ReturnDate = $.trim(obj.val());
    if (ReturnDate != "" && parseFloat(ReturnDate) != 0) {
        obj.addClass("valid");
        obj.removeClass("required");
    }
    else {
        obj.addClass("required");
        obj.removeClass("valid");
    }
}

function selectChange(obj) {
    var ReturnDate = $.trim(obj.val());
    if (ReturnDate != "0" && ReturnDate != "") {
        obj.addClass("valid");
        obj.removeClass("required");
    }
    else {
        obj.addClass("required");
        obj.removeClass("valid");
    }
}

function SelectPOTemplate() {
    $('#POTemplateDialog').attr("style", "display:block;");

    selectChange($("#ddlTemplate"));

    objDialogyTemplate.dialog({ title: 'PO Template', width: '1100', height: '650' });
    objDialogyTemplate.dialog("open");
    return false;
}

function AddPOTemplate() {
    $('#CreateTemplateDialog').attr("style", "display:block;");
    $('#txtTemplateName').val("");
    $('#txtTermsCoditions').val("");

    valueChange($("#txtTemplateName"));
    valueChange($("#txtTermsCoditions"));

    objDialogyAddTemplate.dialog({ title: 'Add PO Template', width: '1000', height: '450' });
    objDialogyAddTemplate.dialog("open");
    return false;
}

function AddShipment() {
    $('#DialogShipmentCreate').attr("style", "display:block;");
    $('#hfShipmentId').val("0");
    $('#ddlShipmentType').val("0");
    $('#txtBranchName').val("");
    $('#hfBranchName').val("0");
    $('#txtAddress').val("");
    $('#txtLocation').val("");
    $('#txtInchargeID').val("");
    $('#hfInchargeID').val("0");
    $('#txtShipmentQty').val("");

    selectChange($("#ddlShipmentType"));
    valueChange($("#txtBranchName"));
    valueChange($("#txtAddress"));
    valueChange($("#txtLocation"));
    valueChange($("#txtInchargeID"));
    valueChange1($("#txtShipmentQty"));

    objDialogyAddShipment.dialog({ title: 'Add Shipment Details', width: '600', height: '290' });
    objDialogyAddShipment.dialog("open");
    return false;
}

function SetPODetails() {
    $('#PODetailsDialog').attr("style", "display:block;");

    objDialogyPODetails.dialog({ title: 'Add PO Details', width: '900', height: '290' });
    objDialogyPODetails.dialog("open");
    return false;
}

function CloaseDialog(flag) {
    if (flag == "0")
        objDialogyTemplate.dialog("close");
    else if (flag == "1")
        objDialogyAddTemplate.dialog("close");
    else if (flag == "2")
        objDialogyShipment.dialog("close");
    else if (flag == "3")
        objDialogyAddShipment.dialog("close");
    else if (flag == "4")
        objDialogyPODetails.dialog("close");
    return false;
}

function TotalCalculation() {
    var Quantity = $.trim($("#txtQty").val()) == "" ? 0 : parseFloat($.trim($("#txtQty").val()));
    var Rate = $.trim($("#txtRate").val()) == "" ? 0 : parseFloat($.trim($("#txtRate").val())); 
    var Discount = $.trim($("#txtDiscount").val()) == "" ? 0 : parseFloat($.trim($("#txtDiscount").val()));
    var TaxPercent = $.trim($("#txtTaxPercent").val()) == "" ? 0 : parseFloat($.trim($("#txtTaxPercent").val()));
    //var Tax = $.trim($("#txtTaxes").val()) == "" ? 0 : parseFloat($.trim($("#txtTaxes").val()));
    var Others = $.trim($("#txtOtherCharges").val()) == "" ? 0 : parseFloat($.trim($("#txtOtherCharges").val()));
    var BaseAmount = Quantity * Rate;
    var Tax = BaseAmount * TaxPercent / 100;
    var Total = BaseAmount - Discount + Tax + Others;
    BaseAmount = parseFloat(BaseAmount).toFixed(2);
    Total = parseFloat(Total).toFixed(2);
    $("#txtBaseAmount").val(BaseAmount);
    $("#txtNetAmount").val(Total);
    $("#txtTaxes").val(Tax);
    valueChange1($("#txtQty"));
    valueChange1($("#txtRate"));
}
function TotalCalculationNew() {
    //var Quantity = $.trim($("#txtQty").val()) == "" ? 0 : parseFloat($.trim($("#txtQty").val()));
    //var Rate = $.trim($("#txtRate").val()) == "" ? 0 : parseFloat($.trim($("#txtRate").val()));
    //var Discount = $.trim($("#txtDiscount").val()) == "" ? 0 : parseFloat($.trim($("#txtDiscount").val()));
    //var TaxPercent = $.trim($("#txtTaxPercent").val()) == "" ? 0 : parseFloat($.trim($("#txtTaxPercent").val()));
    ////var Tax = $.trim($("#txtTaxes").val()) == "" ? 0 : parseFloat($.trim($("#txtTaxes").val()));
    //var Others = $.trim($("#txtOtherCharges").val()) == "" ? 0 : parseFloat($.trim($("#txtOtherCharges").val()));
    //var BaseAmount = Quantity * Rate;
    //var Tax = BaseAmount * TaxPercent / 100;
    //var Total = BaseAmount - Discount + Tax + Others;
    var Total = $("[name=hidPercTotal]").val();
    var Perc = $.trim($("#txtPercentNew").val()) == "" ? 0 : parseFloat($.trim($("#txtPercentNew").val()));
    var maxperc = $("[name=hidPerc]").val();
    if (parseFloat(maxperc) < parseFloat(Perc)) {
        jAlert("Split Percentage Exceeds the Parent Percentage!", "Message", function () {
            $("#txtPercentNew").val("0");
            return false;
        });
        return false;
    }

    var percamnt = (parseFloat(Total) / 100) * parseFloat(Perc);
    percamnt = parseFloat(percamnt).toFixed(2);
    $("#txtBaseAmount").val(percamnt);
    $("#txtNetAmount").val(percamnt);
    $("#txtRate").val(percamnt);
    $("#txtDiscount").val("0");
    $("#txtTaxPercent").val("0");
    $("#txtTaxes").val("0");
    $("#txtQty").val("1");
    $("#txtOtherCharges").val("0");
    valueChange1($("#txtQty"));
    valueChange1($("#txtRate"));
}
$("#txtPercentNew,#txtQty,#txtShipmentQty").on("keypress", function (evt) {
    var $txtBox = $(this);
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46)
        return false;
    else {
        var len = $txtBox.val().length;
        var index = $txtBox.val().indexOf('.');

        if (index > 0 && charCode == 46) {
            return false;
        }
        if (index > 0) {
            var charAfterdot = (len + 1) - index;
            if (charAfterdot > 3) {
                return false;
            }
        }
    }
    return $txtBox; //for chaining
});
$('#txtIsPercentage').click(function () {
    if ($(this).is(':checked'))
    {
        $("#isservicediv").css("display", "");
        $("#isservicediv1").css("display", "");
        $("#txtRate").attr("readonly", "readonly");
        $("#txtDiscount").attr("readonly", "readonly");
        $("#txtTaxPercent").attr("readonly", "readonly");
        $("#txtQty").attr("readonly", "readonly");
        $("#txtOtherCharges").attr("readonly", "readonly");
    }
    else
    {
        $("#isservicediv").css("display", "none");
        $("#isservicediv1").css("display", "none");
        $("#txtRate").removeAttr("readonly");
        $("#txtDiscount").removeAttr("readonly");
        $("#txtTaxPercent").removeAttr("readonly");
        $("#txtQty").removeAttr("readonly");
        $("#txtOtherCharges").removeAttr("readonly");
    }
});




