var objDialogyPARHeader, objDialogyCBFDetails, objDialogyPARAttachment, objDialogyCBFDetailsAttachment;

UrlDet = UrlDet.replace("GetCBFPARHeader", "");
UrlHelper = UrlHelper.replace("GetProjectOwner", "");


 
// var token1 = document.getElementById('antiForgeryForm').childNodes[1].value;
var token = $('[name=__RequestVerificationToken]').val();//$("#txtantiforgery").val();
//alert(token);
debugger
$.ajaxSetup({
    cache: false,
    headers: { "__RequestVerificationToken": token }
});





var SearchModel = function (id) {

    var self = this;

    self.CBFPARHeaderArray = ko.observableArray();
    self.CBFPARDetailsArray = ko.observableArray();
    self.CBFDetailsArray = ko.observableArray();
    self.CBFPRHeaderArray = ko.observableArray();
    self.CBFPRDetailsArray = ko.observableArray();
    self.PARAttachmentArray = ko.observableArray();
    self.CBFAttachmentArray = ko.observableArray();
    self.CBFDetailsAttachmentArray = ko.observableArray();
    self.CBFAuditTrailArray = ko.observableArray();

    self.ProjectOwnerArray = ko.observableArray();
    self.RequestByArray = ko.observableArray();
    self.BudgetSpocArray = ko.observableArray();
    self.ProductCategoryArray = ko.observableArray();
    self.ProductNameArray = ko.observableArray();
    self.UOMArray = ko.observableArray();

    this.GetCBFDetails = function () {

        var CBFHeaderGId = $("#hfCBFId").val();
        if (CBFHeaderGId != "0") {
            var inputFilter = {
                CBFHeaderGId: CBFHeaderGId
            };
            $.ajax({
                type: "post",
                url: UrlDet + "GetCBFHeader",
                data: JSON.stringify(inputFilter),
                contentType: "application/json;",
                success: function (response) {
                    var Data1 = "", Data2 = "", Data3 = "", Data4 = "", Data5 = "";
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                        Data1 = JSON.parse(response.Data1);
                    }
                    if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                        Data2 = JSON.parse(response.Data2);
                        $("#txtRaiserName").val(Data2[0].RaiserName);
                        $("#txtRaiserCode").val(Data2[0].RaiserCode);
                        $("#txtCBFNo").val(Data2[0].CBFHeaderRefNo);
                        $("#txtCBFDate").val(Data2[0].CBFDate);
                        $("#txtCBFEndDate").val(Data2[0].CBFEndDate);
                        $("#txtCBFAmount").val(Data2[0].CBFAmount);
                        $("#lblTotalAmount").text(parseFloat(Data2[0].CBFAmount).toFixed(2));
                        $("#txtDeviationAmount").val(Data2[0].DeviationAmount);
                        $("#ddlProjectOwner").val(Data2[0].ProjectOwnerId);
                        $("#hfBranchName").val(Data2[0].BranchId);
                        $("#txtBranchName").val(Data2[0].BranchName);
                        $("#ddlRequestBy").val(Data2[0].RequestById);
                        $("input[name=CBFMode][value=" + Data2[0].CBFMode + "]").prop("checked", true);
                        $("input[name=Budgeted][value=" + Data2[0].IsBudgeted + "]").prop("checked", true);
                        $("input[name=CBFApproval][value=" + Data2[0].Approvaltype + "]").prop("checked", true);
                        $("input[name=BranchType][value=" + Data2[0].IsBranchSingleId + "]").prop("checked", true);
                        $("#ddlBudgetSPOC").val(Data2[0].BudgetOwnerGId);
                        $("#txtDescription").val(Data2[0].Description);
                        $("#txtJustification").val(Data2[0].Remarks);
                        $("#hfPARHeader").val(Data2[0].PARHeaderId);
                        $("#txtPARHeader").val(Data2[0].PARHeaderRefNo);
                        $("#hfStatus").val(Data2[0].Status);
                        $("#txtCancelRemarks").val(Data2[0].CancelMkrRemarks);
                        $("#txtCloserRemarks").val(Data2[0].ClosureMkrRemarks);
                        $("#txtReopenRemarks").val(Data2[0].ReopenMkrRemarks);

                        $("#txtCloseCBFAmount").val(Data2[0].CBFAmount);
                        $("#txtCloseUtiAmount").val(Data2[0].UtilizedAmount);
                        $("#txtCloseBalanceAmount").val(parseFloat(Data2[0].CBFAmount) - parseFloat(Data2[0].UtilizedAmount));

                        $("#ddlRequestBy").attr("disabled", "disabled");
                        $("input[name=CBFMode]:radio").attr("disabled", "disabled");
                        $("input[name=Budgeted]:radio").attr("disabled", "disabled");

                        if ($("input[name=CBFMode]:radio[checked=checked]").attr("value") == "PAR") {
                            $(".visblePR").css("display", "none");
                            $(".visblePAR").css("display", "");
                            //$("input[name=Budgeted]:radio").removeAttr("disabled");
                            self.GetCBFPARDetails();
                        }

                        else {
                            $(".visblePR").css("display", "");
                            $(".visblePAR").css("display", "none");
                            //$("input[name=Budgeted][value=Y]").prop("checked", true);
                            //$("input[name=Budgeted]:radio").attr("disabled", "disabled");
                            self.GetCBFPRHeader();
                        }
                        if (parseInt(Data2[0].Status) > 1 && parseInt(Data2[0].Status) != 6) {
                            self.GetCBFPRDetails();
                            $(".hideStatus").css("display", "");
                            $(".viewStatus").css("display", "none");
                            $(".disableStatus").attr("disabled", "disabled");
                        }

                        if (parseInt($.trim($("#hfViewMode").val())) == 0 || parseInt($.trim($("#hfViewMode").val())) == 2) {
                            self.GetCBFPRDetails();
                            $(".hideStatus").css("display", "");
                            $(".viewStatus").css("display", "none");
                            $(".disableStatus").attr("disabled", "disabled");
                        }
                        if (parseInt($.trim($("#hfViewMode").val())) == 2) {
                            $("#btnCBFDelete").css("display", "");
                        }
                        if (parseInt(Data2[0].Status) == 1) {
                            $(".viewResubmit").css("display", "none");
                        }
                        if (parseInt(Data2[0].Status) == 6) {
                            $(".viewResubmit").css("display", "");
                        }


                        valueChange($("#txtCBFDate"));
                        valueChange($("#txtCBFEndDate"));
                        //valueChange($("#txtDeviationAmount"));
                        valueChange($("#txtDescription"));
                        valueChange($("#txtJustification"));
                        selectChange($("#ddlProjectOwner"));
                        selectChange($("#ddlRequestBy"));
                        selectChange($("#ddlBudgetSPOC"));

                        valueChange($("#txtBranchName"));
                        valueChange($("#txtPARHeader"));

                        if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null)
                            Data3 = JSON.parse(response.Data3);
                        self.CBFDetailsArray(Data3);

                        if (response.Data4 != null && response.Data4 != "" && JSON.parse(response.Data4) != null)
                            Data4 = JSON.parse(response.Data4);
                        self.CBFAttachmentArray(Data4);

                        if (response.Data5 != null && response.Data5 != "" && JSON.parse(response.Data5) != null)
                            Data5 = JSON.parse(response.Data5);
                        self.CBFAuditTrailArray(Data5);

                        if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null) {
                            var ProdServiceFlag = Data3[0].ProdServiceFlag;
                            if(ProdServiceFlag =="P"){
                                $('input[id="txtprodserviceP"]').prop('checked', true);
                                $('input[id="txtprodserviceS"]').prop('checked', false);
                            }
                            else{
                                $('input[id="txtprodserviceP"]').prop('checked', false);
                                $('input[id="txtprodserviceS"]').prop('checked', true);
                            }
                              
                        }
                            
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //alert(errorThrown);
                }
            });
        }

    };

    this.PARPRHeaderSelect = function () {

        var CBFHeaderGId = $("#hfCBFId").val();
        var RequestForGId = $("#ddlRequestBy").val();
        var IsBudgeted = $("input[name=Budgeted]:radio[checked=checked]").attr("value");
        if (CBFHeaderGId != "0")
            $("#tblDiv").css("display", "none");
        else
            $("#tblDiv").css("display", "");

        var inputFilter = {
            CBFHeaderGId: CBFHeaderGId,
            IsBudgeted: IsBudgeted,
            RequestForGId: RequestForGId
        };

        $.ajax({
            type: "post",
            url: UrlDet + "GetCBFPARHeader",
            data: JSON.stringify(inputFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                self.CBFPARHeaderArray(Data2);
                $('#PARHeaderDialog').attr("style", "display:block;");
                objDialogyPARHeader.dialog({ title: 'PAR Header', width: '1000', height: '450' });
                objDialogyPARHeader.dialog("open");
                return false;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });

    };

    this.PARSelect = function (PARId, PARRefNo) {
        $("#hfPARHeader").val(PARId);
        $("#txtPARHeader").val(PARRefNo);
        $("#txtPARHeader").removeClass("required");
        $("#txtPARHeader").addClass("valid");
        objDialogyPARHeader.dialog("close");
    };

    this.PARSearch = function (flag) {

        if (flag == "0") {
            $("#txtPARDate").val("");
            $("#txtPARNo").val("");
            $("#txtPARAmount").val("");
        }

        var IsBudgeted = $("input[name=Budgeted]:radio[checked=checked]").attr("value");
        var PARDate = $("#txtPARDate").val();
        var PARNo = $("#txtPARNo").val();
        var PARAmount = $("#txtPARAmount").val();
        var RequestForGId = $("#ddlRequestBy").val();
        if ($.trim(PARAmount) == "")
            PARAmount = "0";

        var inputFilter = {
            IsBudgeted: IsBudgeted,
            PARDate: PARDate,
            PARNo: PARNo,
            PARAmount: PARAmount,
            RequestForGId: RequestForGId
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetCBFPARHeaderSearch",
            data: JSON.stringify(inputFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.CBFPARHeaderArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });

    };

    this.SetCBFHeader = function () {

        var CBFHeaderGId = $("#hfCBFId").val();
        var CBFDate = $("#txtCBFDate").val();
        var CBFEndDate = $("#txtCBFEndDate").val();
        var DeviationAmount = $("#txtDeviationAmount").val();
        var ProjectOwnerId = $("#ddlProjectOwner").val();
        var BranchId = $("#hfBranchName").val();
        var ReqBy = $("#ddlRequestBy").val();
        var CBFMode = $("input[name=CBFMode]:radio[checked=checked]").attr("value");
        var IsBudgeted = $("input[name=Budgeted]:radio[checked=checked]").attr("value");
        var CBFApproval = $("input[name=CBFApproval]:radio[checked=checked]").attr("value");
        var BranchType = $("input[name=BranchType]:radio[checked=checked]").attr("value");
        var BudgetOwnerId = $("#ddlBudgetSPOC").val();
        var Description = $("#txtDescription").val();
        var Justification = $("#txtJustification").val();
        var PARPRHeaderGId = $("#hfPARHeader").val();
        var IsRemoved = "0";

        if ($.trim(DeviationAmount) == "") {
            DeviationAmount = "0";
        }

        if ($.trim(CBFDate) == "") {
            jAlert("Ensure CBF Date!", "Message");
            return false;
        }
        if ($.trim(CBFEndDate) == "") {
            jAlert("Ensure CBF End Date!", "Message");
            return false;
        }
        if ($.trim(ProjectOwnerId) == "0") {
            jAlert("Ensure Project Owner!", "Message");
            return false;
        }
        if ($.trim(BranchId) == "0") {
            jAlert("Ensure Branch Name", "Message");
            return false;
        }
        if ($.trim(ReqBy) == "0") {
            jAlert("Ensure Requested By!", "Message");
            return false;
        }
        if ($.trim(BudgetOwnerId) == "0") {
            jAlert("Ensure Budget SPOC!", "Message");
            return false;
        }
        if ($.trim(Description) == "") {
            jAlert("Ensure Description", "Message");
            return false;
        }
        if ($.trim(Justification) == "") {
            jAlert("Ensure Justification!", "Message");
            return false;
        }
        if ($.trim(PARPRHeaderGId) == "0" && CBFMode == "PAR") {
            jAlert("Ensure PAR Header!", "Message");
            return false;
        }

        var CBFHeader = {
            CBFHeaderGId: CBFHeaderGId,
            CBFDate: CBFDate,
            CBFEndDate: CBFEndDate,
            PARPRHeaderGId: PARPRHeaderGId,
            DeviationAmount: DeviationAmount,
            ProjectOwnerId: ProjectOwnerId,
            BranchId: BranchId,
            ReqBy: ReqBy,
            CBFMode: CBFMode,
            IsBudgeted: IsBudgeted,
            CBFApproval: CBFApproval,
            BranchType: BranchType,
            BudgetOwnerId: BudgetOwnerId,
            Description: Description,
            Justification: Justification,
            IsRemoved: IsRemoved
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetCBFHeader",
            data: JSON.stringify(CBFHeader),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == true) {
                        $("#ddlRequestBy").attr("disabled", "disabled");
                        $("input[name=CBFMode]:radio").attr("disabled", "disabled");
                        $("input[name=Budgeted]:radio").attr("disabled", "disabled");
                        //jAlert(Data1[0].Message, "Message");
                        $("#hfCBFId").val(Data1[0].CBFHeaderGId);
                        $("#txtCBFNo").val(Data1[0].CBFRefNo);
                        if (CBFMode == "PAR")
                            self.GetCBFPARDetails();
                        else
                            self.GetCBFPRHeader();
                        jAlert(Data1[0].Message, "Success", function () {
                            $("#mytab2").addClass("ui-tabs-selected");
                            $("#mytab2").addClass("ui-state-active");
                            $("#mytab1").removeClass("ui-tabs-selected");
                            $("#mytab1").removeClass("ui-state-active");
                            $("#tabs-1").addClass("ui-tabs-hide");
                            $("#tabs-2").removeClass("ui-tabs-hide");
                        });

                    }
                    else
                        jAlert(Data1[0].Message, "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.GetCBFPARDetails = function () {

        var CBFHeaderGId = $("#hfCBFId").val();
        var PARHeaderGId = $("#hfPARHeader").val();
        var RequestForGId = $("#ddlRequestBy").val();
        var IsBudgeted = $("input[name=Budgeted]:radio[checked=checked]").attr("value");
        var inputFilter = {
            CBFHeaderGId: CBFHeaderGId,
            PARHeaderGId: PARHeaderGId,
            RequestForGId: RequestForGId,
            IsBudgeted: IsBudgeted
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetCBFPARDetails",
            data: JSON.stringify(inputFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                }

                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                self.CBFPARDetailsArray(Data2);
                $("#btnAddCBFDetails").css("display", "none");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });

    };

    this.PARDetailsSelect = function (PARDetailGId) {
        $("#hfPARPRDetailGId").val(PARDetailGId);
        $("#ParDetailsGrid > tbody > tr").removeClass("parSelect");
        $("#ParDetails" + PARDetailGId).addClass("parSelect");
        $("#btnAddCBFDetails").css("display", "");
    };

    this.PARDetailsBOQ = function (PARDetailGId) {

        var inputFilter = {
            RefGId: PARDetailGId,
            RefFlag: "12",
            AttachTypeGId: "2"
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetPARAttachment",
            data: JSON.stringify(inputFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.PARAttachmentArray(Data1);

                $('#PARDetailsAttachment').attr("style", "display:block;");
                objDialogyPARAttachment.dialog({ title: 'PAR Details Attachment BOQ', width: '800', height: '400' });
                objDialogyPARAttachment.dialog("open");

                return false;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.SetCBFDetails = function () {
        var CBFDetailGId = $("#hfCBFDetailsId").val();
        var CBFHeaderGId = $("#hfCBFId").val();
        var PARPRDetailGId = $("#hfPARPRDetailGId").val();
        var PARPRDesc = $("#txtProductDesc").val();
        var ProductGId = $("#ddlProductName").val();
        var ProductGroupGId = $("#ddlProductGroup").val();
        var Remarks = $("#txtRemarks").val();
        var UOMGId = $("#ddlUOM").val();
        var Qty = $("#txtQty").val();
        var UnitPrice = $("#txtUnitAmount").val();
        var TotalAmount = $("#txtTotalAmount").val();
        var ChartOfAcc = $("#txtChartOfAccount").val();
        var FccCode = $("#hfFCCC").val();
        var BudgetLine = $("#txtBudgetLine").val();
        var IsRemoved = "0";

        var CBFMode = $("input[name=CBFMode]:radio[checked=checked]").attr("value");

        if ($.trim(CBFHeaderGId) == "0") {
            jAlert("Ensure CBF Header Details!", "Message");
            return false;
        }
        if ($.trim(PARPRDetailGId) == "0") {
            if (CBFMode == 'PAR')
                jAlert("Ensure Select PAR Details!", "Message");
            else
                jAlert("Ensure Select PR Details!", "Message");
            return false;
        }
        if ($.trim(ProductGroupGId) == "0") {
            jAlert("Ensure Product Group!", "Message");
            return false;
        }
        if ($.trim(ProductGId) == "0") {
            jAlert("Ensure Product Name", "Message");
            return false;
        }
        if ($.trim(PARPRDesc) == "") {
            jAlert("Ensure Product Description", "Message");
            return false;
        }
        if ($.trim(UOMGId) == "0") {
            jAlert("Ensure UOM!", "Message");
            return false;
        }
        if ($.trim(Qty) == "" || parseFloat(Qty) == 0) {
            jAlert("Ensure Quantity!", "Message");
            return false;
        }
        if ($.trim(UnitPrice) == "" || parseFloat(UnitPrice) == 0) {
            jAlert("Ensure Unit Price!", "Message");
            return false;
        }
        if ($.trim(Remarks) == "" && $("input[name=CBFMode]:radio[checked=checked]").attr("value") == "PAR") {
            jAlert("Ensure Remarks!", "Message");
            return false;
        }

        if ($.trim(ChartOfAcc) == "") {
            jAlert("Ensure Chart Of Account!", "Message");
            return false;
        }
        if ($.trim(FccCode) == "" || parseFloat(FccCode) == 0) {
            jAlert("Ensure FCCC!", "Message");
            return false;
        }

        if ($.trim(BudgetLine) == "" || parseFloat(BudgetLine) == 0) {
            jAlert("Ensure Budget Line!", "Message");
            return false;
        }

        var CBFDetails = {
            CBFDetailGId: CBFDetailGId,
            CBFHeaderGId: CBFHeaderGId,
            PARPRDetailGId: PARPRDetailGId,
            PARPRDesc: PARPRDesc,
            ProductGId: ProductGId,
            ProductGroupGId: ProductGroupGId,
            Remarks: Remarks,
            UOMGId: UOMGId,
            Qty: Qty,
            UnitPrice: UnitPrice,
            TotalAmount: TotalAmount,
            ChartOfAcc: ChartOfAcc,
            FccCode: FccCode,
            BudgetLine: BudgetLine,
            IsRemoved: IsRemoved,
            IsContigency: "0"
        };
        showProgress();
        $.ajax({
            type: "post",
            url: UrlDet + "SetCBFDeatails",
            data: JSON.stringify(CBFDetails),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == true) {
                        objDialogyCBFDetails.dialog("close");
                        hideProgress();
                        jAlert(Data1[0].Message, "Message");
                    }
                    else {
                        if (Data1[0].IsConfirm == true) {
                            jConfirm(Data1[0].Message, "Confirm", function (callback) {
                                showProgress();
                                if (callback == true) {
                                    
                                    var CBFDetails = {
                                        CBFDetailGId: CBFDetailGId,
                                        CBFHeaderGId: CBFHeaderGId,
                                        PARPRDetailGId: PARPRDetailGId,
                                        PARPRDesc: PARPRDesc,
                                        ProductGId: ProductGId,
                                        ProductGroupGId: ProductGroupGId,
                                        Remarks: Remarks,
                                        UOMGId: UOMGId,
                                        Qty: Qty,
                                        UnitPrice: UnitPrice,
                                        TotalAmount: TotalAmount,
                                        ChartOfAcc: ChartOfAcc,
                                        FccCode: FccCode,
                                        BudgetLine: BudgetLine,
                                        IsRemoved: IsRemoved,
                                        IsContigency: "1"
                                    };
                                    
                                    $.ajax({
                                        type: "post",
                                        url: UrlDet + "SetCBFDeatails",
                                        data: JSON.stringify(CBFDetails),
                                        contentType: "application/json;",
                                        success: function (response) {
                                            var Data1 = "", Data2 = "";
                                            if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                                                Data1 = JSON.parse(response.Data1);
                                                if (Data1[0].Clear == true) 
                                                    objDialogyCBFDetails.dialog("close");
                                                hideProgress();
                                                jAlert(Data1[0].Message, "Message");
                                                $("#txtCBFAmount").val(Data1[0].TotalAmount);
                                                $("#lblTotalAmount").text(parseFloat(Data1[0].TotalAmount).toFixed(2));
                                            }
                                            if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                                                Data2 = JSON.parse(response.Data2);
                                            self.CBFDetailsArray(Data2);

                                            if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                                                var ProdServiceFlag = Data2[0].ProdServiceFlag;
                                                if (ProdServiceFlag == "P") {
                                                    $('input[id="txtprodserviceP"]').prop('checked', true);
                                                    $('input[id="txtprodserviceS"]').prop('checked', false);
                                                }
                                                else {
                                                    $('input[id="txtprodserviceP"]').prop('checked', false);
                                                    $('input[id="txtprodserviceS"]').prop('checked', true);
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
                        }
                        else
                        {
                            hideProgress();
                            jAlert(Data1[0].Message, "Message");
                        }
                    }
                    $("#txtCBFAmount").val(Data1[0].TotalAmount);
                    $("#lblTotalAmount").text(parseFloat(Data1[0].TotalAmount).toFixed(2));
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                self.CBFDetailsArray(Data2);
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    var ProdServiceFlag = Data2[0].ProdServiceFlag;
                    if (ProdServiceFlag == "P") {
                        $('input[id="txtprodserviceP"]').prop('checked', true);
                        $('input[id="txtprodserviceS"]').prop('checked', false);
                    }
                    else {
                        $('input[id="txtprodserviceP"]').prop('checked', false);
                        $('input[id="txtprodserviceS"]').prop('checked', true);
                    }

                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.EditCBFDetails = function (Index, flag) {
        var _tmpData = self.CBFDetailsArray()[Index];

        var ProdServiceFlag = _tmpData.ProdServiceFlag;
        if (ProdServiceFlag == "P") {
            $('input[id="txtprodserviceP"]').prop('checked', true);
            $('input[id="txtprodserviceS"]').prop('checked', false);
        }
        else {
            $('input[id="txtprodserviceP"]').prop('checked', false);
            $('input[id="txtprodserviceS"]').prop('checked', true);
        }
        self.LoadProductCategory();
        $('#CBFDetailsDialog').attr("style", "display:block;");
        $("#hfCBFDetailsId").val(_tmpData.CBFDetailGId);
        $("#hfPARPRDetailGId").val(_tmpData.PARPRDetailGId);
        $('#ddlProductGroup').val(_tmpData.ProductGroupGId);
        self.LoadProductName();
        $('#ddlProductName').val(_tmpData.ProductGId);
        $('#txtProductDesc').val(_tmpData.ProductDescription);
        $('#ddlUOM').val(_tmpData.UOMGId);
        $('#txtQty').val(_tmpData.Qty);
        $('#txtUnitAmount').val(_tmpData.UnitPrice);
        $('#txtTotalAmount').val(_tmpData.TotalAmount);
        $('#txtRemarks').val(_tmpData.Remarks);
        $('#txtChartOfAccount').val(_tmpData.ChartOfAcc);
        $('#hfFCCC').val(_tmpData.FccCode);
        $('#txtFCCC').val(_tmpData.FccName);
        $('#txtBudgetLine').val(_tmpData.BudgetLine);

        selectChange($("#ddlProductGroup"));
        selectChange($("#ddlProductName"));
        valueChange($("#txtProductDesc"));
        selectChange($("#ddlUOM"));
        valueChange($("#txtQty"));
        valueChange($("#txtUnitAmount"));
        valueChange($("#txtRemarks"));
        valueChange($("#txtChartOfAccount"));
        valueChange($("#txtFCCC"));
        valueChange1($("#txtBudgetLine"));

        if ($("input[name=CBFMode]:radio[checked=checked]").attr("value") == "PAR" && (parseInt($("#hfStatus").val()) < 2 || parseInt($("#hfStatus").val()) == 6) && parseInt($.trim($("#hfViewMode").val())) == 1) {
            $("#ddlProductGroup").removeAttr("disabled");
            $("#ddlProductName").removeAttr("disabled");
            $("#txtProductDesc").removeAttr("disabled");
            $("#ddlUOM").removeAttr("disabled");
            $("#txtChartOfAccount").removeAttr("disabled");
            $("#txtRemarks").removeAttr("disabled");
        }
        else {
            $("#ddlProductGroup").attr("disabled", "disabled");
            $("#ddlProductName").attr("disabled", "disabled");
            $("#txtProductDesc").attr("disabled", "disabled");
            $("#ddlUOM").attr("disabled", "disabled");
            $("#txtChartOfAccount").attr("disabled", "disabled");
            $("#txtRemarks").attr("disabled", "disabled");
            $("#txtRemarks").removeClass("required");
        }

      
        $('input[id="txtprodserviceP"]').attr("disabled", "disabled");
        $('input[id="txtprodserviceS"]').attr("disabled", "disabled");
        //if ($("#tblCBFDet > tbody > tr").length < 1) {
        //    $('input[id="txtprodserviceP"]').removeAttr("disabled");
        //    $('input[id="txtprodserviceS"]').removeAttr("disabled");
        //    $('input[id="txtprodserviceP"]').prop('checked', true);
        //    $('input[id="txtprodserviceS"]').prop('checked', false);
        //}
        //else {
        //    $('input[id="txtprodserviceP"]').attr("disabled", "disabled");
        //    $('input[id="txtprodserviceS"]').attr("disabled", "disabled");
        //}


        if (flag == 0)
            $("#btnCBFDetailsSubmit").css("display", "none");
        else
            $("#btnCBFDetailsSubmit").css("display", "");
        objDialogyCBFDetails.dialog({ title: 'CBF Details', width: '800', height: '350' });
        objDialogyCBFDetails.dialog("open");
        return false;
    };

    this.DeleteCBFDetails = function (CBFDetailGId) {

        jConfirm("Are you sure? Want to delete CBF Details!", "Confirm", function (callback) {
            if (callback == true) {

                var CBFHeaderGId = $("#hfCBFId").val();
                var PARPRDetailGId = "0";
                var PARPRDesc = "0";
                var ProductGId = "0";
                var ProductGroupGId = "0";
                var Remarks = "";
                var UOMGId = "0";
                var Qty = "0";
                var UnitPrice = "0";
                var TotalAmount = "0";
                var ChartOfAcc = "";
                var FccCode = "0";
                var BudgetLine = "0";
                var IsRemoved = "1";

                var CBFDetails = {
                    CBFDetailGId: CBFDetailGId,
                    CBFHeaderGId: CBFHeaderGId,
                    PARPRDetailGId: PARPRDetailGId,
                    PARPRDesc: PARPRDesc,
                    ProductGId: ProductGId,
                    ProductGroupGId: ProductGroupGId,
                    Remarks: Remarks,
                    UOMGId: UOMGId,
                    Qty: Qty,
                    UnitPrice: UnitPrice,
                    TotalAmount: TotalAmount,
                    ChartOfAcc: ChartOfAcc,
                    FccCode: FccCode,
                    BudgetLine: BudgetLine,
                    IsRemoved: IsRemoved,
                    IsContigency: "0"
                };
                $.ajax({
                    type: "post",
                    url: UrlDet + "SetCBFDeatails",
                    data: JSON.stringify(CBFDetails),
                    contentType: "application/json;",
                    success: function (response) {
                        var Data1 = "", Data2 = "";
                        if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                            Data1 = JSON.parse(response.Data1);
                            if (Data1[0].Clear == true)
                                objDialogyCBFDetails.dialog("close");
                            jAlert(Data1[0].Message, "Message");
                            $("#txtCBFAmount").val(Data1[0].TotalAmount);
                            $("#lblTotalAmount").text(parseFloat(Data1[0].TotalAmount).toFixed(2));
                        }
                        if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                            Data2 = JSON.parse(response.Data2);
                        self.CBFDetailsArray(Data2);
                        if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                            var ProdServiceFlag = Data2[0].ProdServiceFlag;
                            if (ProdServiceFlag == "P") {
                                $('input[id="txtprodserviceP"]').prop('checked', true);
                                $('input[id="txtprodserviceS"]').prop('checked', false);
                            }
                            else {
                                $('input[id="txtprodserviceP"]').prop('checked', false);
                                $('input[id="txtprodserviceS"]').prop('checked', true);
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

    this.GetCBFPRHeader = function () {

        var CBFHeaderGId = $("#hfCBFId").val();
        var RequestForGId = $("#ddlRequestBy").val();
        var inputFilter = {
            CBFHeaderGId: CBFHeaderGId,
            RequestForGId: RequestForGId
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetCBFPRHeader",
            data: JSON.stringify(inputFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                self.CBFPRHeaderArray(Data2);
                return false;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });

    };

    this.PRSearch = function (flag) {

        if (flag == "0") {
            $("#txtPRDate").val("");
            $("#txtPRNo").val("");
        }

        var RequestForGId = $("#ddlRequestBy").val();
        var PRDate = $("#txtPRDate").val();
        var PRNo = $("#txtPRNo").val();

        var inputFilter = {
            RequestForGId: RequestForGId,
            PRDate: PRDate,
            PRNo: PRNo
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetCBFPRHeaderSearch",
            data: JSON.stringify(inputFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.CBFPRHeaderArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });

    };

    this.PRSelect = function (PRHeaderGId) {
        $("#hfPRHeader").val(PRHeaderGId);
        $("#PRHeaderGrid > tbody > tr").removeClass("parSelect");
        $("#PRHeader" + PRHeaderGId).addClass("parSelect");
        self.GetCBFPRDetails();
    };

    this.GetCBFPRDetails = function () {

        var CBFHeaderGId = $("#hfCBFId").val();
        var PRHeaderGId = $("#hfPRHeader").val();
        var inputFilter = {
            CBFHeaderGId: CBFHeaderGId,
            PRHeaderGId: PRHeaderGId
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetCBFPRDetails",
            data: JSON.stringify(inputFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                self.CBFPRDetailsArray(Data2);
                return false;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.PRDetailsSelect = function (item) {

        var CBFHeaderGId = $("#hfCBFId").val();
        var IsRemoved = "0";
        $("#hfPARPRDetailGId").val(item.PRDetailGId);
        if ($("#rdoPRDetails" + item.PRDetailGId).is(":checked")) {
            IsRemoved = "0"
            $("#PRDetails" + item.PRDetailGId).addClass("parSelect");
        }
        else {
            IsRemoved = "1"
            $("#PRDetails" + item.PRDetailGId).removeClass("parSelect");
        }

        var inputFilter = {
            CBFHeaderGId: CBFHeaderGId,
            PRDetailGId: item.PRDetailGId,
            Qty: item.Qty,
            IsRemoved: IsRemoved
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetCBFPRDetails",
            data: JSON.stringify(inputFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    $("#txtCBFAmount").val(Data1[0].TotalAmount);
                    $("#lblTotalAmount").text(parseFloat(Data1[0].TotalAmount).toFixed(2));
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                self.CBFDetailsArray(Data2);
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    var ProdServiceFlag = Data2[0].ProdServiceFlag;
                    if (ProdServiceFlag == "P") {
                        $('input[id="txtprodserviceP"]').prop('checked', true);
                        $('input[id="txtprodserviceS"]').prop('checked', false);
                    }
                    else {
                        $('input[id="txtprodserviceP"]').prop('checked', false);
                        $('input[id="txtprodserviceS"]').prop('checked', true);
                    }

                }
                return false;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });

        return true;

    };

    this.CBFDetailsBOQ = function (CBFDetailGId) {
        $("#hfCBFAttachDetailsId").val(CBFDetailGId);
        self.ClearCBFDetailsAttachment();
        self.CBFDetailsAttachmentArray("");
        var inputFilter = {
            RefGId: CBFDetailGId,
            RefFlag: "3",
            AttachTypeGId: "3"
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetPARAttachment",
            data: JSON.stringify(inputFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.CBFDetailsAttachmentArray(Data1);
                return false;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
        $('#CBFDetailsAttachment').attr("style", "display:block;");
        objDialogyCBFDetailsAttachment.dialog({ title: 'CBF Details Attachment', width: '900', height: '450' });
        objDialogyCBFDetailsAttachment.dialog("open");

    };

    this.SetCBFAttachment = function () {

        var RefGId = $("#hfCBFId").val();
        var AttachmentId = "0";
        var AttachmentName = "";
        var Description = $("#txtCBFAttachDescription").val();
        var RefFlag = "2";
        var IsRemoved = "0";

        if ($.trim(RefGId) == "0") {
            jAlert("Ensure CBF Header Details!", "Message");
            return false;
        }
        if ($.trim($("#txtCBFUploadFile").val()) == "") {
            jAlert("Ensure File Name!", "Message");
            return false;
        }
        if ($.trim(Description) == "") {
            jAlert("Ensure Description!", "Message");
            return false;
        }

        var CBFAttachment = {
            RefGId: RefGId,
            AttachmentId: AttachmentId,
            AttachmentName: AttachmentName,
            Description: Description,
            RefFlag: RefFlag,
            IsRemoved: IsRemoved
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetCBFAttachment",
            data: JSON.stringify(CBFAttachment),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == false)
                        jAlert(Data1[0].Message, "Message");
                    else {
                        jAlert(Data1[0].Message, "Message");
                        self.ClearCBFAttachment();
                    }
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                if (Data1[0].Clear == true)
                    self.CBFAttachmentArray(Data2);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.DeleteCBFAttachment = function (AttachmentGId) {

        jConfirm("Are you sure? Want to delete Attachment!", "Confirm", function (callback) {
            if (callback == true) {
                var RefGId = $("#hfCBFId").val();
                var AttachmentName = "Delete.txt";
                var Description = "";
                var RefFlag = "2";
                var IsRemoved = "1";

                if ($.trim(RefGId) == "0") {
                    jAlert("Ensure CBF Header Details!", "Message");
                    return false;
                }

                var CBFAttachment = {
                    RefGId: RefGId,
                    AttachmentId: AttachmentGId,
                    AttachmentName: AttachmentName,
                    Description: Description,
                    RefFlag: RefFlag,
                    IsRemoved: IsRemoved
                };
                $.ajax({
                    type: "post",
                    url: UrlDet + "SetCBFAttachment",
                    data: JSON.stringify(CBFAttachment),
                    contentType: "application/json;",
                    success: function (response) {
                        var Data1 = "", Data2 = "";
                        if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                            Data1 = JSON.parse(response.Data1);
                            if (Data1[0].Clear == false)
                                jAlert(Data1[0].Message, "Message");
                            else {
                                jAlert(Data1[0].Message, "Message");
                                self.ClearCBFAttachment();
                            }
                        }
                        if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                            Data2 = JSON.parse(response.Data2);
                        if (Data1[0].Clear == true)
                            self.CBFAttachmentArray(Data2);
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

    this.ClearCBFAttachment = function () {
        $("#txtCBFUploadFile").val("");
        $("#txtCBFUploadFile").replaceWith($("#txtCBFUploadFile").clone(true));
        $("#txtCBFAttachDescription").val("");
    };

    this.SetCBFDetailsAttachment = function () {

        var RefGId = $("#hfCBFAttachDetailsId").val();
        var AttachmentId = "0";
        var AttachmentName = "";
        var Description = $("#txtCBFDAttachDescription").val();
        var RefFlag = "3";
        var IsRemoved = "0";

        if ($.trim(RefGId) == "0") {
            jAlert("Ensure CBF Details!", "Message");
            return false;
        }
        if ($.trim($("#txtCBFDUploadFile").val()) == "") {
            jAlert("Ensure File Name!", "Message");
            return false;
        }
        if ($.trim(Description) == "") {
            jAlert("Ensure Description!", "Message");
            return false;
        }

        var CBFAttachment = {
            RefGId: RefGId,
            AttachmentId: AttachmentId,
            AttachmentName: AttachmentName,
            Description: Description,
            RefFlag: RefFlag,
            IsRemoved: IsRemoved
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetCBFAttachment",
            data: JSON.stringify(CBFAttachment),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == false)
                        jAlert(Data1[0].Message, "Message");
                    else {
                        jAlert(Data1[0].Message, "Message");
                        self.ClearCBFDetailsAttachment();
                    }
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                if (Data1[0].Clear == true)
                    self.CBFDetailsAttachmentArray(Data2);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.DeleteCBFDetailsAttachment = function (AttachmentGId) {

        jConfirm("Are you sure? Want to delete Attachment!", "Confirm", function (callback) {
            if (callback == true) {
                var RefGId = $("#hfCBFAttachDetailsId").val();
                var AttachmentName = "";
                var Description = "";
                var RefFlag = "3";
                var IsRemoved = "1";

                if ($.trim(RefGId) == "0") {
                    jAlert("Ensure CBF Details!", "Message");
                    return false;
                }

                var CBFAttachment = {
                    RefGId: RefGId,
                    AttachmentId: AttachmentGId,
                    AttachmentName: AttachmentName,
                    Description: Description,
                    RefFlag: RefFlag,
                    IsRemoved: IsRemoved
                };
                $.ajax({
                    type: "post",
                    url: UrlDet + "SetCBFAttachment",
                    data: JSON.stringify(CBFAttachment),
                    contentType: "application/json;",
                    success: function (response) {
                        var Data1 = "", Data2 = "";
                        if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                            Data1 = JSON.parse(response.Data1);
                            if (Data1[0].Clear == false)
                                jAlert(Data1[0].Message, "Message");
                            else {
                                jAlert(Data1[0].Message, "Message");
                                self.ClearCBFDetailsAttachment();
                            }
                        }
                        if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                            Data2 = JSON.parse(response.Data2);
                        if (Data1[0].Clear == true)
                            self.CBFDetailsAttachmentArray(Data2);
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

    this.ClearCBFDetailsAttachment = function () {
        $("#txtCBFDUploadFile").val("");
        $("#txtCBFDUploadFile").replaceWith($("#txtCBFDUploadFile").clone(true));
        $("#txtCBFDAttachDescription").val("");
    };

    this.AttachmentDownLoad = function (AttachmentGId, FileName) {
        var iframe = $('<iframe name="postiframe1" id="postiframe1" style="display: none"></iframe>');
        $("body").append(iframe);
        var form = $('#frmDeactivation');
        form.attr("action", UrlDet + "Download?Id=" + AttachmentGId + "&FileName=" + FileName);
        form.attr("method", "post");
        form.attr("encoding", "multipart/form-data");
        form.attr("enctype", "multipart/form-data");
        form.attr("target", "postiframe");
        form.attr("file", $('#attUploader').val());
        form.submit();
    };

    this.PARAttachmentDownLoad = function (FileName) {

        var iframe = $('<iframe name="postiframe1" id="postiframe1" style="display: none"></iframe>');
        $("body").append(iframe);
        var form = $('#frmDeactivation');
        form.attr("action", UrlDet + "DownloadPARDetails?FileName=" + FileName);
        form.attr("method", "post");
        form.attr("encoding", "multipart/form-data");
        form.attr("enctype", "multipart/form-data");
        form.attr("target", "postiframe");
        form.attr("file", $('#attUploader').val());
        form.submit();
    };

    this.SubmitCBF = function (reject) {

        var CBFHeaderGId = $("#hfCBFId").val();
        var Type = "1";
        var IsReject = reject;
        var Remarks = "";

        if ($.trim(CBFHeaderGId) == "0") {
            jAlert("Ensure CBF Header Details!", "Message");
            return false;
        }
        var InputFilter = {
            CBFHeaderGId: CBFHeaderGId,
            Type: Type,
            IsReject: IsReject,
            Remarks: Remarks
        };
        showProgress();
        $.ajax({
            type: "post",
            url: UrlDet + "SubmitCBF",
            data: JSON.stringify(InputFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == true) {
                        hideProgress();
                        jAlert(Data1[0].Message, "Success", function () {
                            location = UrlReturn;
                        });
                    }
                    else {
                        hideProgress();
                        jAlert(Data1[0].Message, "Message");
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.ApproveCBF = function (reject) {

        var CBFHeaderGId = $("#hfCBFId").val();
        var Type = $("#hfType").val();
        var IsReject = reject;
        var Remarks = $("#txtApproveRemarks").val();

        if ($.trim(CBFHeaderGId) == "0" ) {
            jAlert("Ensure CBF Header Details!", "Message");
            return false;
        }

        if ($.trim(Remarks) == "" && IsReject == "1") {
            jAlert("Ensure Remarks!", "Message");
            return false;
        }

        var InputFilter = {
            CBFHeaderGId: CBFHeaderGId,
            Type: Type,
            IsReject: IsReject,
            Remarks: Remarks
        };
        showProgress();
        $.ajax({
            type: "post",
            url: UrlDet + "SubmitCBF",
            data: JSON.stringify(InputFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == true) {
                        hideProgress();
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

    this.CloseDetails = function (flag) {
        if (flag == 0)
            objDialogyPARHeader.dialog("close");
        if (flag == 1)
            objDialogyCBFDetails.dialog("close");
        if (flag == 2)
            objDialogyPARAttachment.dialog("close");
        if (flag == 3)
            objDialogyCBFDetailsAttachment.dialog("close");
    };

    this.PRHeaderBind = function () {
        if (parseInt($("#hfStatus").val()) > 1 && parseInt($("#hfStatus").val()) != 6) {
            $(".viewStatus").css("display", "none");
        }
        if (parseInt($.trim($("#hfViewMode").val())) == 0 || parseInt($.trim($("#hfViewMode").val())) == 2) {
            $(".viewStatus").css("display", "none");
        }
    };

    this.StylesTypeCall = function () {
        if ($("input[name=CBFMode]:radio[checked=checked]").attr("value") == "PAR") {
            $(".visblePR").css("display", "none");
            $(".visblePAR").css("display", "");
        }
        else {
            $(".visblePR").css("display", "");
            $(".visblePAR").css("display", "none");
        }
        if (parseInt($("#hfStatus").val()) > 1 && parseInt($("#hfStatus").val()) != 6) {
            $(".viewStatus").css("display", "none");
            $(".changeAttach").html("View BOQ");
        }
        if (parseInt($.trim($("#hfViewMode").val())) == 0 || parseInt($.trim($("#hfViewMode").val())) == 2) {
            $(".viewStatus").css("display", "none");
        }
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

    this.LoadRequestBy = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetRequestBy",
            data: "{}",
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.RequestByArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.LoadBudgetSpoc = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetBudgetSPOC",
            data: "{}",
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.BudgetSpocArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.LoadProductCategory = function () {
      
        if ($("input[name=CBFMode]:radio[checked=checked]").attr("value") == "PAR") {
            var prodserviceflag = "0";
            prodserviceflag = $("input[name=txtprodservice]:radio[checked=checked]").attr("value");
            if (prodserviceflag == null || prodserviceflag == "" || prodserviceflag == "undefined")
                prodserviceflag = "0";

            var prodserviceFilter = {
                prodservice: prodserviceflag
            }; 
            $.ajax({
                type: "post",
                url: UrlHelper + "GetProductGroup",
                data: JSON.stringify(prodserviceFilter),
                async: false,
                contentType: "application/json;",
                success: function (response) {
                    var Data1 = "";
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                        Data1 = JSON.parse(response.Data1);
                    self.ProductCategoryArray(Data1);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //alert(errorThrown);
                }
            });
        }
        else {
            $.ajax({
                type: "post",
                url: UrlHelper + "GetProductGroup",
                data: "{}",
                async: false,
                contentType: "application/json;",
                success: function (response) {
                    var Data1 = "";
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                        Data1 = JSON.parse(response.Data1);
                    self.ProductCategoryArray(Data1);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //alert(errorThrown);
                }
            });
        }
      //  var CatId = $("#ddlProductGroup").val();
     
    };
    this.ChangeProductFlag = function () {
        if ($("#ddlProductGroup").hasClass("valid")) {
            $("#ddlProductGroup").removeClass("valid");
            $("#ddlProductGroup").addClass("required");
        }
        self.LoadProductCategory();
    };

    this.ChangeProductGroup = function () {
        if ($("#ddlProductName").hasClass("valid")) {
            $("#ddlProductName").removeClass("valid");
            $("#ddlProductName").addClass("required");
        }
        self.LoadProductName();
    };

    this.ChangeProductName = function () {
        for (var i = 0; i < self.ProductNameArray().length; i++) {
            if (self.ProductNameArray()[i].Id == $("#ddlProductName").val()) {
                $("#txtProductDesc").val(self.ProductNameArray()[i].Description);
                $("#txtChartOfAccount").val(self.ProductNameArray()[i].GLNo);
                valueChange($("#txtProductDesc"));
                valueChange($("#txtChartOfAccount"));
                return false;
            }
        }
    };

    this.LoadProductName = function () {
        var CatId = $("#ddlProductGroup").val();
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

    this.GetRequestedBy = function () {
        $.ajax({
            type: "post",
            url: UrlDet + "GetRequestBy",
            data: "{}",
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    $("#ddlRequestBy").val(Data1[0].RequestForId);
                    selectChange($("#ddlRequestBy"));
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.LoadProjectOwner();
    self.LoadRequestBy();
    self.LoadBudgetSpoc();
    self.LoadProductCategory();
    self.LoadProductName();
    self.LoadUOM();
    self.GetRequestedBy();
    self.GetCBFPARDetails();

    self.GetCBFDetails();


};

var mainViewModel = new SearchModel();
ko.applyBindings(mainViewModel);

$(document).ready(function () {
    objDialogyPARHeader = $("[id$='PARHeaderDialog']");
    objDialogyPARHeader.dialog({
        autoOpen: false,
        modal: true,
        width: 1000,
        height: 600,
        duration: 300

    });

    objDialogyCBFDetails = $("[id$='CBFDetailsDialog']");
    objDialogyCBFDetails.dialog({
        autoOpen: false,
        modal: true,
        width: 650,
        height: 400,
        duration: 300

    });

    objDialogyPARAttachment = $("[id$='PARDetailsAttachment']");
    objDialogyPARAttachment.dialog({
        autoOpen: false,
        modal: true,
        width: 700,
        height: 400,
        duration: 300

    });

    objDialogyCBFDetailsAttachment = $("[id$='CBFDetailsAttachment']");
    objDialogyCBFDetailsAttachment.dialog({
        autoOpen: false,
        modal: true,
        width: 800,
        height: 400,
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

    $("#txtCBFDate").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        maxDate: 'd',
        onSelect: function (selected) {
            $("#txtCBFEndDate").datepicker("option", "minDate", selected)
            $("#txtCBFEndDate").val("");
            valueChange($("#txtCBFDate"));
            valueChange($("#txtCBFEndDate"));
        }
    });

    $("#txtCBFEndDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        numberOfMonths: 1,
        minDate: 'd',
        onSelect: function (selected) {
            valueChange($("#txtCBFEndDate"));
        }
    });

    $("input[name=CBFMode]:radio").change(function () {
        if ($("input[name=CBFMode]:radio[checked=checked]").attr("value") == "PAR") {
            $(".visblePR").css("display", "none");
            $(".visblePAR").css("display", "");
            //$("input[name=Budgeted]:radio").removeAttr("disabled");
        }

        else {
            $(".visblePR").css("display", "");
            $(".visblePAR").css("display", "none");
            $("input[name=Budgeted][value=Y]").prop("checked", true);
            //$("input[name=Budgeted]:radio").attr("disabled", "disabled");
        }
        $("#hfPARHeader").val("0");
        $("#txtPARHeader").val("");
        valueChange($("#txtPARHeader"));
    });

    $("input[name=Budgeted]:radio").change(function () {
        $("#hfPARHeader").val("0");
        $("#txtPARHeader").val("");
        valueChange($("#txtPARHeader"));
    });


    $("#txtCBFDate").change(function () {
        valueChange($("#txtCBFDate"));
    });

    $("#txtCBFEndDate").change(function () {
        valueChange($("#txtCBFEndDate"));
    });

    //CBF Header Section
    //$("#txtDeviationAmount").change(function () {
    //    valueChange($("#txtDeviationAmount"));
    //});

    $("#txtDescription").change(function () {
        valueChange($("#txtDescription"));
    });

    $("#txtJustification").change(function () {
        valueChange($("#txtJustification"));
    });

    $("#ddlProjectOwner").change(function () {
        selectChange($("#ddlProjectOwner"));
    });

    $("#ddlRequestBy").change(function () {
        selectChange($("#ddlRequestBy"));
    });

    $("#ddlBudgetSPOC").change(function () {
        selectChange($("#ddlBudgetSPOC"));
    });

    $("#txtApproveRemarks").keyup(function (e) {
        valueChange($("#txtApproveRemarks"));
    });

    $(".EnterValidation").keypress(function (e) {
        if (e.which == 13) {
            return false;
        }
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
                    url: UrlHelper + "GetAutoCompleteBranch",
                    data: "{ 'txt' : '" + $("#txtBranchName").val() + "'}",
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
                $("#hfBranchName").val(i.item.val);
                $("#txtBranchName").val(i.item.label);
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
        }
    });

    //CBF Details Section
    $("#ddlProductGroup").change(function () {
        selectChange($("#ddlProductGroup"));
    });

    $("#ddlProductName").change(function () {
        selectChange($("#ddlProductName"));
    });

    $("#ddlUOM").change(function () {
        selectChange($("#ddlUOM"));
    });

    $("#txtProductDesc").change(function () {
        valueChange($("#txtProductDesc"));
    });

    $("#txtQty").change(function () {
        valueChange1($("#txtQty"));
        var Qty = $.trim($("#txtQty").val()) == "" ? 0 : parseFloat($("#txtQty").val());
        var Amount = $.trim($("#txtUnitAmount").val()) == "" ? 0 : parseFloat($("#txtUnitAmount").val());
        var Total = (Qty * Amount);
        Total = parseFloat(Total).toFixed(2);
        $("#txtTotalAmount").val(Total);
    });

    $("#txtUnitAmount").change(function () {
        valueChange1($("#txtUnitAmount"));
        var Qty = $.trim($("#txtQty").val()) == "" ? 0 : parseFloat($("#txtQty").val());
        var Amount = $.trim($("#txtUnitAmount").val()) == "" ? 0 : parseFloat($("#txtUnitAmount").val());
        var Total = (Qty * Amount);
        Total = parseFloat(Total).toFixed(2);
        $("#txtTotalAmount").val(Total);
    });

    $("#txtRemarks").change(function () {
        valueChange($("#txtRemarks"));
    });

    $("#txtChartOfAccount").change(function () {
        valueChange($("#txtChartOfAccount"));
    });

    $("#txtBudgetLine").change(function () {
        valueChange($("#txtBudgetLine"));
    });

    $("#txtFCCC").keyup(function (e) {
        if (e.which != 13) {
            $("#hfFCCC").val("0");
            $("#txtFCCC").removeClass("valid");
            $("#txtFCCC").addClass("required");
        }

        $("#txtFCCC").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteFCCC",
                    data: "{ 'txt' : '" + $("#txtFCCC").val() + "'}",
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
                $("#hfFCCC").val(i.item.val);
                $("#txtFCCC").val(i.item.label);
                $("#txtFCCC").removeClass("required");
                $("#txtFCCC").addClass("valid");
            },
            minLength: 1
        });

    });

    $("#txtFCCC").focusout(function () {
        var hdfId = $("#hfFCCC").val();
        var txtCurName = $("#txtFCCC").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtFCCC").val("");
        }
    });

    $(".txtUploadSupAttachmentFile").on('change', function (e) {
        var iframe = $('<iframe name="postiframe" id="postiframe" style="display: none"></iframe>');
        $("body").append(iframe);
        var form = $('#frmDeactivation');
        form.attr("action", UrlDet + "UploadCBFAttachment");
        form.attr("method", "post");
        form.attr("encoding", "multipart/form-data");
        form.attr("enctype", "multipart/form-data");
        form.attr("target", "postiframe");
        form.attr("file", $('#txtCBFUploadFile').val());
        form.submit();
        
    });

    $(".txtUploadDAttachment").on('change', function (e) {
        var iframe = $('<iframe name="postiframe" id="postiframe" style="display: none"></iframe>');
        $("body").append(iframe);
        var form = $('#frmDeactivation1');
        form.attr("action", UrlDet + "UploadCBFAttachment");
        form.attr("method", "post");
        form.attr("encoding", "multipart/form-data");
        form.attr("enctype", "multipart/form-data");
        form.attr("target", "postiframe");
        form.attr("file", $('#txtCBFDUploadFile').val());
        form.submit();
        return false;
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

function AddCBFDetails() {
    $('#CBFDetailsDialog').attr("style", "display:block;");
    $('#hfCBFDetailsId').val("0");
    $('#ddlProductGroup').val("0");
    $('#ddlProductName').val("0");
    $('#txtProductDesc').val("");
    $('#ddlUOM').val("0");
    $('#txtQty').val("");
    $('#txtUnitAmount').val("");
    $('#txtTotalAmount').val("");
    $('#txtRemarks').val("");
    $('#txtChartOfAccount').val("");
    $('#txtFCCC').val("");
    $('#txtBudgetLine').val("");

    selectChange($("#ddlProductGroup"));
    selectChange($("#ddlProductName"));
    valueChange($("#txtProductDesc"));
    selectChange($("#ddlUOM"));
    valueChange($("#txtQty"));
    valueChange($("#txtUnitAmount"));
    valueChange($("#txtRemarks"));
    valueChange($("#txtChartOfAccount"));
    valueChange($("#txtFCCC"));
    valueChange($("#txtBudgetLine"));

    if ($("input[name=CBFMode]:radio[checked=checked]").attr("value") == "PAR") {
        $("#ddlProductGroup").removeAttr("disabled");
        $("#ddlProductName").removeAttr("disabled");
        $("#txtProductDesc").removeAttr("disabled");
        $("#ddlUOM").removeAttr("disabled");
        //$("#txtChartOfAccount").removeAttr("disabled");
        $("#txtRemarks").removeAttr("disabled");
     //   $("#hideforpr").css("display", "");
        if ($("#tblCBFDet > tbody > tr").length < 1) {
            $('input[id="txtprodserviceP"]').removeAttr("disabled");
            $('input[id="txtprodserviceS"]').removeAttr("disabled");
            $('input[id="txtprodserviceP"]').prop('checked', true);
            $('input[id="txtprodserviceS"]').prop('checked', false);
        }
        else {
            $('input[id="txtprodserviceP"]').attr("disabled", "disabled");
            $('input[id="txtprodserviceS"]').attr("disabled", "disabled");
        }

    }
    else {
      //  $("#hideforpr").css("display", "none");
        $("#ddlProductGroup").attr("disabled", "disabled");
        $("#ddlProductName").attr("disabled", "disabled");
        $("#txtProductDesc").attr("disabled", "disabled");
        $("#ddlUOM").attr("disabled", "disabled");
        //$("#txtChartOfAccount").attr("disabled", "disabled");
        $("#txtRemarks").attr("disabled", "disabled");
        $("#txtRemarks").removeClass("required");
    }

    $("#btnCBFDetailsSubmit").css("display", "");

    objDialogyCBFDetails.dialog({ title: 'CBF Details', width: '800', height: '350' });
    objDialogyCBFDetails.dialog("open");
    return false;
}
$("#txtQty,#txtUnitAmount").on("keypress", function (evt) {
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

