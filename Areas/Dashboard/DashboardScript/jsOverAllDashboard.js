var objHoldECFDialogy;

UrlDet = UrlDet.replace("GetOverAllDashboard", "");
MUrlHelper = MUrlHelper.replace("GetAutoCompleteCourier", "");
EOWUrlDet = EOWUrlDet.replace("MyDocDetails", "");
EOWUrlDet = EOWUrlDet.replace("MyDashDocDetails/", "");
ASMSUrlDet = ASMSUrlDet.replace("Modification", "");
EProcureUrlDet = EProcureUrlDet.replace("flexibuydashboard", "");

//TOA FOR Fixed Asset For My Approval Dashboard Click 
IFAMSUrlDet = IFAMSUrlDet.replace("TransferedSummary", "");
//WOA Draft,Inprocess,ForMyapproval Dashboard Link Click
IFAMSUrlDetWOA = IFAMSUrlDetWOA.replace("WMSummary", "");
IFAMSUrlDetSOA = IFAMSUrlDetSOA.replace("SASummary", "");


var DashboardModel = function () {
    var self = this;
    self.IsDashbaord = ko.observable(true);
    self.RequestType = ko.observable("");
    self.RequestStatus = ko.observable("");
    self.IsRecordIndex = ko.observable(0);
    self.IsApprovalSummary = ko.observable(false);
    self.IsApprovalClick = ko.observable(false);
    //Vendor Options
    self.IsVendorSummary = ko.observable(false);
    self.IsVendorAppSummary = ko.observable(false);
    self.IsVendorDetail = ko.observable(false);
    self.VendorTitle = ko.observable("");
    self.IsVendorAction = ko.observable(false);
    self.IsVendorACTIVATION = ko.observable(false);
    self.IsVendorDEACTIVATION = ko.observable(false);
    self.IsVendorONBOARDING = ko.observable(false);
    self.IsVendorRENEWAL = ko.observable(false);
    self.Suppliergid = ko.observable(0);
    //Eprocure Options
    self.EProcureID = ko.observable(0);
    self.IsEProcure = ko.observable(false);
    self.IsEProcureDetail = ko.observable(false);
    self.EProcureTitle = ko.observable("");
    self.IsEProcureAction = ko.observable(false);
    self.IsPARDetails = ko.observable(false);
    self.IsCBFDetails = ko.observable(false);
    self.IsPRDetails = ko.observable(false);
    self.IsPODetails = ko.observable(false);
    self.IsWODetails = ko.observable(false);




    //EClaim Options
    self.EClaimQueueGID = ko.observable(0);
    self.EClaimID = ko.observable(0);
    self.DocumentTypeId = ko.observable(0);
    self.IsEclaims = ko.observable(false);
    self.EClaimTitle = ko.observable("");
    self.IsEclaimsDetail = ko.observable(false);
    self.DocumentSubType = ko.observable("");
    self.EOWArray1 = ko.observableArray();
    self.TravalArray = ko.observableArray();
    self.IsEClaimAction = ko.observable(false);

    self.SIDSA = ko.observable(false);
    self.EclaimLC = ko.observable(false);
    self.EclaimTravalType = ko.observable(false);
    self.SI_NON_PO = ko.observable(false);
    self.EclaimARFEP = ko.observable(false);
    self.EclaimARFF = ko.observable(false);

    self.POType = ko.observable("");
    self.EClaimPOArray = ko.observableArray();
    self.VendorArray = ko.observableArray();
    self.EProcureArray = ko.observableArray();
    self.EOWArray = ko.observableArray();
    self.VendorStatusArray = ko.observableArray();


    self.VendorSummaryArray = ko.observableArray();
    self.EOWSummaryArray = ko.observableArray();

    self.EProcureSummaryArray = ko.observableArray();

    //PAR
    self.FBArray2 = ko.observableArray();
    self.FBArray1 = ko.observableArray();
    self.CBFArray = ko.observableArray();
    self.POArray = ko.observableArray();
    self.PRArray = ko.observableArray();
    self.WOArray = ko.observableArray();

    //Hold Tab
    self.HoldArray = ko.observableArray();

    //My Request Tab
    self.MyReqASMSArray = ko.observableArray();
    self.MyReqEProcureArray = ko.observableArray();
    self.MyReqEOWArray = ko.observableArray();

    //My Approval Tab
    self.MyAppASMSArray = ko.observableArray();
    self.MyAppEProcureArray = ko.observableArray();
    self.MyAppEOWArray = ko.observableArray();

    //Fixed Asset 
    self.FixedassetArray = ko.observableArray(); //For Dashboard
    self.FixedassetArray1 = ko.observableArray();// For Workbench

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
        if (data != null && data.length > 37) {
            data = data.substring(0, 37) + '...';
        } return data;
    };
    self.IndexChecker = function (_Type) {
        if (_Type == "1") { //ASMS
            if (self.VendorSummaryArray().length == 0) {
                return null;
            }
            if (self.VendorSummaryArray().length > self.IsRecordIndex() + 1) {
                return self.VendorSummaryArray()[self.IsRecordIndex()];
            } else {
                self.IsRecordIndex(0);
                return self.VendorSummaryArray()[0];
            }
        }
        if (_Type == "2") { //EPROCURE
            if (self.EProcureSummaryArray().length == 0) {
                return null;
            }
            if (self.EProcureSummaryArray().length > self.IsRecordIndex() + 1) {
                return self.EProcureSummaryArray()[self.IsRecordIndex()];
            } else {
                self.IsRecordIndex(0);
                return self.EProcureSummaryArray()[0];
            }
        }
        if (_Type == "3") { //ECLAIM
            if (self.EOWSummaryArray().length == 0) {
                return null;
            }
            if (self.EOWSummaryArray().length > self.IsRecordIndex() + 1) {
                return self.EOWSummaryArray()[self.IsRecordIndex()];
            } else {
                self.IsRecordIndex(0);
                return self.EOWSummaryArray()[0];
            }
        }

    };

    //Vendor management
    self.VMProcess = function (item) {
        self.IsApprovalClick(false);
        self.IsRecordIndex(self.VendorSummaryArray.indexOf(item));
        self.IsVendorACTIVATION(false);
        self.IsVendorDEACTIVATION(false);
        self.IsVendorONBOARDING(false);
        self.IsVendorRENEWAL(false);

        if (self.VendorTitle() == "ACTIVATION") { self.IsVendorACTIVATION(true); }
        if (self.VendorTitle() == "DEACTIVATION") { self.IsVendorDEACTIVATION(true); }
        if (self.VendorTitle() == "ONBOARDING") { self.IsVendorONBOARDING(true); }
        if (self.VendorTitle() == "RENEWAL") { self.IsVendorRENEWAL(true); }
        //if (self.VendorTitle() == "MODIFICATION") { }

        self.Suppliergid(0);
        $("#txtASMSSupplierDetails,#txtASMSRaiserDetails,#txtASMSPrevContractStrDate,#txtASMSPrevContractEndDate,#txtASMSContractStrDate,#txtASMSContractEndDate,#txtASMSActivationFrom,#txtASMSActivationTo,#txtASMSApproxContractValue,#txtASMSGSTINProvided,#txtASMSActivationReason,#txtASMSDeActivationReason,#txtASMSRaiserReason,#txtASMSPreviousApproverDetails,#txtASMSPreviousApproverReason,#txtASMSCheckerRemark").val("");
        $("#txtASMSCheckerRemark").removeClass("valid").removeClass("required").addClass("required");
        checkASMSValidationUpdate();
        showProgress();
        setTimeout(function () {
            var _Filter = {
                Suppliergid: item.Supplierheadergid,
                Action: item.supplierheaderrequesttype
            };
            $.ajax({
                type: "post",
                url: UrlDet + "GetASMSSupplierHeaderDetails",
                data: JSON.stringify(_Filter),
                contentType: "application/json;",
                success: function (response) {
                    hideProgress();
                    var Data1 = "";
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                        Data1 = JSON.parse(response.Data1);

                        if (Data1.length > 0) {
                            self.Suppliergid(Data1[0].Suppliergid);
                            $("#txtASMSSupplierDetails").val(Data1[0].Supplier);
                            $("#txtASMSRaiserDetails").val(Data1[0].Raiser);
                            $("#txtASMSContractStrDate").val(Data1[0].ContractFrom);
                            $("#txtASMSContractEndDate").val(Data1[0].ContractTo);
                            $("#txtASMSRaiserReason").val(Data1[0].RaiserComments);
                            $("#txtASMSPreviousApproverDetails").val(Data1[0].PreviousApprover);
                            $("#txtASMSPreviousApproverReason").val(Data1[0].PreviousApproverRemarks);

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
                            self.IsVendorSummary(false);
                            self.IsVendorDetail(true);
                        }
                    } else {
                        //show error detail
                        jAlert("In-valid Venor Details!", "Error");
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    hideProgress();
                }
            });
        }, 400);
    };
    self.ASMSSubmit = function (IsReject) {
        //IsReject -- [1-Approve,  0-Rejected]
        var Remarks = $("#txtASMSCheckerRemark").val();
        if ($.trim(self.Suppliergid()) == "0" || $.trim(self.Suppliergid()) == null) {
            jAlert("Ensure Details!", "Message");
            return false;
        }

        var InputFilter = {
            refgid: self.Suppliergid(),
            requesttype: self.VendorTitle(),
            actionremark: Remarks,
            queueto: '',
            actionstatus: IsReject,
            skipflag: '0',
            action: 'submittonextapproval'
        };
        showProgress();
        setTimeout(function () {
            $.ajax({
                type: "post",
                url: UrlDet + "SubmitASMS",
                data: JSON.stringify(InputFilter),
                contentType: "application/json;",
                success: function (response) {
                    hideProgress();
                    var Data1 = "";
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                        Data1 = JSON.parse(response.Data1);
                        if (Data1[0].Clear == true) {
                            jAlert(Data1[0].Message, "Success", function () {
                                if (self.IsApprovalClick()) {
                                    self.IsApprovalClick(false);
                                    self.Suppliergid(0);
                                    self.IsApprovalSummary(true);
                                    self.IsVendorDetail(false);
                                    self.LoadApprovalTab();
                                } else {
                                    //Load Next Record
                                    self.Suppliergid(0);
                                    self.clickASMSSummaryLink("1");
                                }
                            });
                        }
                        else {
                            jAlert(Data1[0].Message, "Message");
                        }
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    hideProgress();
                }
            });
        }, 300);
    };

    self.ASMSURL = function (item) {
        var _tmpUrl = "";
        var reqType = self.RequestType().toUpperCase();
        var requeststatus = self.RequestStatus().toLowerCase();
        var _mode = "5";

        if (requeststatus == "draft" || requeststatus == "rejected") {
            _mode = "4"
        } else if (requeststatus == "waitingforapproval") {
            _mode = "3"
        }
        _tmpUrl = ASMSUrlDet + "Onboarding/Index?pagefor=" + _mode + "&supid=" + item.Supplierheadergid + '&' + new Date().getTime();
        var win = window.open(_tmpUrl, '_self');
        if (win) {
            win.focus();
        } else {
            //Browser has blocked it
            jAlert("Please allow popups for this website", "Message");
        }
    }
    self.ViewASMS = function () {
        var item = self.IndexChecker("1");
        if (item == null) {
            jAlert("Sorry! Unable to process your request.", "Message");
            return false;
        }
        self.ASMSURL(item);
    };

    self.clickASMS = function (RequestType, Status, data) {
        self.IsRecordIndex(0);
        //Block Value with zero
        if (data[Status] == "0") {
            return false;
        }
        showProgress();

        self.Suppliergid(0);
        self.RequestType(RequestType);
        self.RequestStatus(Status);

        setTimeout(function () {
            var _Filter = {
                action: 'ASMS',
                requesttype: RequestType,
                requeststatus: Status
            };

            if (Status != "waitingforapproval" || RequestType == "MODIFICATION") {
                self.IsVendorAction(false);
            } else {
                self.IsVendorAction(true);
            }
            //self.IsVendorAction(true);
            self.loadVendorSummaryGrid();
            $.ajax({
                type: "post",
                url: UrlDet + "GetDashboardSummary",
                data: JSON.stringify(_Filter),
                contentType: "application/json;",
                success: function (response) {
                    hideProgress();
                    var Data1 = "";
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                        Data1 = JSON.parse(response.Data1);
                    }
                    self.VendorSummaryArray(Data1);
                    if (self.VendorSummaryArray().length > 0) {
                        self.VendorTitle(RequestType);
                        self.IsDashbaord(false);
                        self.IsVendorSummary(true);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    hideProgress();
                }
            });
        }, 400);
    };
    self.clickASMSSummaryLink = function (IsSubmit) {
        self.Suppliergid(0);
        showProgress();
        setTimeout(function () {
            var _Filter = {
                action: 'ASMS',
                requesttype: self.RequestType(),
                requeststatus: self.RequestStatus()
            };

            if (self.RequestStatus() != "waitingforapproval" || self.RequestStatus() == "MODIFICATION") {
                self.IsVendorAction(false);
            } else {
                self.IsVendorAction(true);
            }
            //self.IsVendorAction(true);
            self.loadVendorSummaryGrid();
            $.ajax({
                type: "post",
                url: UrlDet + "GetDashboardSummary",
                data: JSON.stringify(_Filter),
                contentType: "application/json;",
                success: function (response) {
                    hideProgress();
                    var Data1 = "";
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                        Data1 = JSON.parse(response.Data1);
                    }
                    self.VendorSummaryArray(Data1);
                    if (IsSubmit == "0" && self.VendorSummaryArray().length > 0) {
                        self.VendorTitle(self.RequestType());
                        self.IsVendorSummary(true);
                        self.IsVendorDetail(false);
                    }

                    if (IsSubmit == "1" && self.VendorSummaryArray().length > 0) {
                        jConfirm("Want to load Next Record?", "Message", function (callback) {
                            if (callback == true) {
                                var item = self.IndexChecker("1");
                                if (item == null) {
                                    self.IsRecordIndex(0);
                                    self.LoadDashboard();
                                } else {
                                    self.VMProcess(item);
                                }
                            } else if (callback == false) {
                                self.IsRecordIndex(0);
                                self.IsVendorDetail(false);
                                self.IsVendorSummary(true);
                            }
                        });
                        $("#popup_ok").attr("style", "margin-left: 0px;");
                        $("#popup_cancel").attr("style", "margin-left: 5px;");
                    } else if (IsSubmit == "1" && self.VendorSummaryArray().length == 0) {
                        self.IsRecordIndex(0);
                        self.LoadDashboard();
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    hideProgress();
                }
            });
        }, 400);
    };
    self.DatatableCall = function () {
        if ($("#gvVendorSummary > tbody > tr").length == self.VendorSummaryArray().length) {
            $("#gvVendorSummary").DataTable({
                "autoWidth": false,
                "destroy": true,
                "scrollX": true,
                "aoColumnDefs": [{
                    "aTargets": ["nosort"],
                    "bSortable": false
                }, {
                    "aTargets": ["colDate"],
                    "bSortable": true,
                    "sType": "date-uk"
                }
                //, { "targets": [1], "visible": self.IsVendorAction() }
                ]
            });
        }
    };
    self.loadVendorSummaryGrid = function () {
        $("#gvVendorSummary").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.VendorSummaryArray.removeAll();
    }
    self.loadVendorSummaryGrid();

    //Eprocure
    self.FBProcess = function (item) {
        self.IsRecordIndex(self.EProcureSummaryArray.indexOf(item));
        self.IsPARDetails(false);
        self.IsCBFDetails(false);
        self.IsPRDetails(false);
        self.IsPODetails(false);
        self.IsWODetails(false);
        self.IsApprovalClick(false);
        if (self.RequestType() == "PAR") { self.IsPARDetails(true); }
        if (self.RequestType() == "CBF") { self.IsCBFDetails(true); }
        if (self.RequestType() == "PR") { self.IsPRDetails(true); }
        if (self.RequestType() == "PO") { self.IsPODetails(true); self.loadPOGrid(); }
        if (self.RequestType() == "WO") { self.IsWODetails(true); self.loadWOGrid(); }

        showProgress();
        setTimeout(function () {
            var _Filter = {
                Refgid: item.GId,
                Action: item.RequestType
            };

            $.ajax({
                type: "post",
                url: UrlDet + "GetFBDetails",
                data: JSON.stringify(_Filter),
                contentType: "application/json;",
                success: function (response) {
                    hideProgress();
                    var Data1 = "", Data2 = "", Data3 = "";
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                        Data1 = JSON.parse(response.Data1);
                        if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                            Data2 = JSON.parse(response.Data2);
                        }
                        if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null) {
                            Data3 = JSON.parse(response.Data3);
                        }
                        if (Data1.length > 0) {
                            if (self.RequestType() == "PAR") {
                                self.EProcureID(Data1[0].Refgid);
                                self.FBArray1(Data2);
                                self.FBArray2(Data3);
                                //Refgid  
                                $("#txtPARNo").val(Data1[0].RefNo);
                                $("#txtPARDate").val(Data1[0].RefDate);
                                $("#txtPARRaiserDetails").val(Data1[0].Raiser);
                                $("#txtBudgeted").val(Data1[0].IsBudgeted);
                                $("#txtPARAmount").val(Data1[0].Amount);
                                $("#txtPARRaiserReason").val(Data1[0].RaiserComments);

                                $("#txtPARCheckerRemark").val("");
                                $("#txtPARCheckerRemark").removeClass("valid").removeClass("required").addClass("required");
                                checkPARValidationUpdate();
                            }
                            if (self.RequestType() == "CBF") {
                                self.EProcureID(Data1[0].Refgid);
                                self.CBFArray(Data2);
                                //Refgid  
                                $("#txtCBFNo").val(Data1[0].RefNo);
                                $("#txtCBFDate").val(Data1[0].RefDate);
                                $("#txtCBFRaiserDetails").val(Data1[0].Raiser);
                                $("#txtCBFRequestFor").val(Data1[0].RequestFor);
                                $("#txtCBFAmount").val(Data1[0].Amount);
                                $("#txtCBFMode").val(Data1[0].Mode);
                                $("#txtCBFPARPRNo").val(Data1[0].PARPRRefNo);
                                $("#txtCBFPARPRAmt").val(Data1[0].PARPRAmount);
                                $("#txtCBFApproval").val(Data1[0].Approvaltype);
                                $("#txtCBFBudgeted").val(Data1[0].IsBudgeted);
                                $("#txtCBFBranch").val(Data1[0].BranchName);

                                $("#txtCBFPreviousApproverDetails").val(Data1[0].PreviousApprover);
                                $("#txtCBFPreviousApproverReason").val(Data1[0].PreviousApproverRemarks);
                                $("#txtCBFJustification").val(Data1[0].Justification);

                                $("#txtCBFCheckerRemark").val("");
                                $("#txtCBFCheckerRemark").removeClass("valid").removeClass("required").addClass("required");
                                checkCBFValidationUpdate();
                            }

                            if (self.RequestType() == "PR") {
                                self.EProcureID(Data1[0].Refgid);
                                self.PRArray(Data2);
                                //Refgid
                                $("#txtPRNo").val(Data1[0].RefNo);
                                $("#txtPRDate").val(Data1[0].RefDate);
                                $("#txtPRRaiserDetails").val(Data1[0].Raiser);
                                $("#txtPRBudgeted").val(Data1[0].IsBudgeted);
                                $("#txtPRAmount").val(Data1[0].Amount);
                                $("#txtPRRequestFor").val(Data1[0].RequestFor);
                                $("#txtPRMode").val(Data1[0].ExpenseType);
                                $("#txtPRBSCC").val(Data1[0].BSCC);
                                $("#txtPRBranch").val(Data1[0].BranchName);

                                $("#txtPRPreviousApproverDetails").val(Data1[0].PreviousApprover);
                                $("#txtPRPreviousApproverReason").val(Data1[0].PreviousApproverRemarks);
                                $("#txtPRRaiserComments").val(Data1[0].RaiserComments);

                                $("#txtPRCheckerRemark").val("");
                                $("#txtPRCheckerRemark").removeClass("valid").removeClass("required").addClass("required");
                                checkPRValidationUpdate();
                            }

                            if (self.RequestType() == "PO") {
                                self.EProcureID(Data1[0].Refgid);
                                self.POArray(Data2);
                                //Refgid
                                $("#txtPONo").val(Data1[0].RefNo);
                                $("#txtPODate").val(Data1[0].RefDate);
                                $("#txtPORaiserDetails").val(Data1[0].Raiser);
                                $("#txtPOSupplierDetails").val(Data1[0].Supplier);
                                $("#txtPOCBFNo").val(Data1[0].CBFNo);
                                $("#txtPOCBFLineAmount").val(Data1[0].CBFLineAmount);
                                $("#txtPOMode").val(Data1[0].Mode);
                                $("#txtPOPARPRNo").val(Data1[0].PARPRRefNo);
                                $("#txtPOBranchSingle").val(Data1[0].BranchSingle);
                                $("#txtPOBranch").val(Data1[0].BranchName);
                                $("#txtPOCC").val(Data1[0].CC);
                                $("#txtPOAmt").val(Data1[0].Amount);

                                $("#txtPOPreviousApproverDetails").val(Data1[0].PreviousApprover);
                                $("#txtPOPreviousApproverReason").val(Data1[0].PreviousApproverRemarks);

                                $("#txtPOCheckerRemark").val("");
                                $("#txtPOCheckerRemark").removeClass("valid").removeClass("required").addClass("required");
                                checkPOValidationUpdate();
                            }

                            if (self.RequestType() == "WO") {
                                self.EProcureID(Data1[0].Refgid);
                                self.WOArray(Data2);
                                //Refgid
                                $("#txtWONo").val(Data1[0].RefNo);
                                $("#txtWODate").val(Data1[0].RefDate);
                                $("#txtWORaiserDetails").val(Data1[0].Raiser);
                                $("#txtWOSupplierDetails").val(Data1[0].Supplier);
                                $("#txtWOOBFNo").val(Data1[0].CBFNo);
                                $("#txtWOOBFLineAmount").val(Data1[0].CBFLineAmount);
                                $("#txtWOMode").val(Data1[0].Mode);
                                $("#txtWOPARPRNo").val(Data1[0].PARPRRefNo);
                                $("#txtWOBranchSingle").val(Data1[0].BranchSingle);
                                $("#txtWOBranch").val(Data1[0].BranchName);
                                $("#txtWOCC").val(Data1[0].CC);
                                $("#txtWOAmt").val(Data1[0].Amount);

                                $("#txtWOPreviousApproverDetails").val(Data1[0].PreviousApprover);
                                $("#txtWOPreviousApproverReason").val(Data1[0].PreviousApproverRemarks);

                                $("#txtWOCheckerRemark").val("");
                                $("#txtWOCheckerRemark").removeClass("valid").removeClass("required").addClass("required");
                                checkWOValidationUpdate();
                            }

                            self.IsEProcure(false);
                            self.IsEProcureDetail(true);
                        }
                    } else {
                        //show error detail
                        jAlert("In-valid Venor Details!", "Error");
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    hideProgress();
                }
            });
        }, 400);
    };
    self.clickEProcureSummaryLink = function (IsSubmit) {
        self.loadEProcureSummaryGrid();
        var _Filter = {
            action: 'EPROCURE',
            requesttype: self.RequestType(),
            requeststatus: self.RequestStatus()
        };
        self.EProcureID(0);
        if (self.RequestStatus() != "WAITINGFORAPPROVAL") {
            self.IsEProcureAction(false);
        } else {
            self.IsEProcureAction(true);
        }
        $.ajax({
            type: "post",
            url: UrlDet + "GetDashboardSummary",
            data: JSON.stringify(_Filter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                }
                self.EProcureSummaryArray(Data1);
                if (IsSubmit == "0" && self.EProcureSummaryArray().length > 0) {
                    self.EProcureTitle(self.RequestType());
                    self.IsEProcureDetail(false);
                    self.IsEProcure(true);
                }
                if (IsSubmit == "1" && self.EProcureSummaryArray().length > 0) {
                    jConfirm("Want to load Next Record?", "Message", function (callback) {
                        if (callback == true) {
                            var item = self.IndexChecker("2");
                            if (item == null) {
                                self.IsRecordIndex(0);
                                self.LoadDashboard();
                            } else {
                                self.FBProcess(item);
                            }
                        } else if (callback == false) {
                            self.IsRecordIndex(0);

                            self.IsEProcureDetail(false);
                            self.IsEProcure(true);
                        }
                    });
                    $("#popup_ok").attr("style", "margin-left: 0px;");
                    $("#popup_cancel").attr("style", "margin-left: 5px;");
                } else if (IsSubmit == "1" && self.EProcureSummaryArray().length == 0) {
                    self.IsRecordIndex(0);
                    self.LoadDashboard();
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    };


    //For Dashboard Draft,Reject,Waitingforapproval

    self.clickEPROCURE = function (RequestType, Status, data) {
        //bharathidhasan kumar;
        debugger;


        if (Status == "WAITINGFORAPPROVAL") {
            // location = ('~/FLEXIBUY/flexibuydashboard/ForMyApprovalSummaryIndex?type=' + RequestType + '&' + new Date().getTime());
            location = EProcureUrlDet + "flexibuydashboard/ForMyApprovalSummaryIndex?type=" + RequestType + '&' + new Date().getTime();
        }
        else if (Status == "REJECTED") {
            Status = "reject";
            //location = ('~/FLEXIBUY/flexibuydashboard/myDocuments?type=' + RequestType + '&' + 'Process=' + Status + '&' + new Date().getTime());
            location = EProcureUrlDet + "flexibuydashboard/myDocuments?type=" + RequestType + '&' + 'Process=' + Status + '&' + new Date().getTime();
        }
        else if (Status == "DRAFT") {
            Status = "draft";
            //location = ('~/FLEXIBUY/flexibuydashboard/myDocuments?type=' + RequestType + '&' + 'Process=' + Status + '&' + new Date().getTime());
            location = EProcureUrlDet + "flexibuydashboard/myDocuments?type=" + RequestType + '&' + 'Process=' + Status + '&' + new Date().getTime();

        }
        else if (Status == "INPROCESS") {
            Status = "inprocess";
            //location = ('~/FLEXIBUY/flexibuydashboard/myDocuments?type=' + RequestType + '&' + 'Process=' + Status + '&' + new Date().getTime());
            location = EProcureUrlDet + "flexibuydashboard/myDocuments?type=" + RequestType + '&' + 'Process=' + Status + '&' + new Date().getTime();
        }



        //self.IsRecordIndex(0);
        ////Block Value with zero
        //if (data[Status] == "0") {
        //    return false;
        //}
        //self.RequestType(RequestType);
        //self.RequestStatus(Status);
        //self.EProcureID(0);
        //self.loadEProcureSummaryGrid();
        //var _Filter = {
        //    action: 'EPROCURE',
        //    requesttype: RequestType,
        //    requeststatus: Status
        //};
        //if (Status != "WAITINGFORAPPROVAL") {
        //    self.IsEProcureAction(false);
        //} else {
        //    self.IsEProcureAction(true);
        //}

        //$.ajax({
        //    type: "post",
        //    url: UrlDet + "GetDashboardSummary",
        //    data: JSON.stringify(_Filter),
        //    contentType: "application/json;",
        //    success: function (response) {
        //        var Data1 = "";
        //        if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
        //            Data1 = JSON.parse(response.Data1);
        //        }
        //        self.EProcureSummaryArray(Data1 == "" ? [] : Data1);
        //        if (Data1.length > 0) {
        //            self.EProcureTitle(RequestType);
        //            self.IsDashbaord(false);
        //            self.IsEProcure(true);
        //        }
        //    },
        //    error: function (XMLHttpRequest, textStatus, errorThrown) {
        //    }
        //});
    };

    self.EPROCUREURL = function (item) {
        var _tmpUrl = "";
        var reqType = self.RequestType().toUpperCase();
        var requeststatus = self.RequestStatus().toLowerCase();

        //Draft,Rejected ../FLEXIBUY/PRNew?prgid=1278&viewtype=edit&1490353079425
        if (reqType == "PR" && (requeststatus == "draft" || requeststatus == "rejected")) {
            _tmpUrl = EProcureUrlDet + "PRNew?prgid=" + item.GId + "&viewtype=edit&" + new Date().getTime();
        }
        //Inprocess,Approved ../FLEXIBUY/PRNew?prgid=1857&viewtype=view&1490352842885
        if (reqType == "PR" && (requeststatus == "waitingforapproval" || requeststatus == "inprocess")) {
            _tmpUrl = EProcureUrlDet + "PRNew?prgid=" + item.GId + "&viewtype=view&" + new Date().getTime();
        }
        //Draft,Rejected ../FLEXIBUY/CBF/Edit/2003
        if (reqType == "CBF" && (requeststatus == "draft" || requeststatus == "rejected")) {
            _tmpUrl = EProcureUrlDet + "CBF/Edit/" + item.GId;
        }
        //Inprocess,Approved ../FLEXIBUY/CBF/Edit/2003/1
        if (reqType == "CBF" && (requeststatus == "waitingforapproval" || requeststatus == "inprocess")) {
            _tmpUrl = EProcureUrlDet + "CBF/Edit/" + item.GId + "/1";
        }
        //Draft,Rejected ../FLEXIBUY/PARNew?pargid=33&viewtype=edit&1490354950705
        if (reqType == "PAR" && (requeststatus == "draft" || requeststatus == "rejected")) {
            _tmpUrl = EProcureUrlDet + "PARNew?pargid=" + item.GId + "&viewtype=edit&" + new Date().getTime();
        }
        //Inprocess,Approved ../FLEXIBUY/PARNew?pargid=33&viewtype=view&1490354915033
        if (reqType == "PAR" && (requeststatus == "waitingforapproval" || requeststatus == "inprocess")) {
            _tmpUrl = EProcureUrlDet + "PARNew?pargid=" + item.GId + "&viewtype=view&" + new Date().getTime();
        }
        //Draft,Rejected ../FLEXIBUY/PO/RaisePO/5552
        if (reqType == "PO" && (requeststatus == "draft" || requeststatus == "rejected")) {
            _tmpUrl = EProcureUrlDet + "PO/RaisePO/" + item.GId;
        }
        //Inprocess,Approved ..FLEXIBUY/PO/RaisePO/5552/0
        if (reqType == "PO" && (requeststatus == "waitingforapproval" || requeststatus == "inprocess")) {
            _tmpUrl = EProcureUrlDet + "PO/RaisePO/" + item.GId + "/0";
        }
        //Draft,Rejected ../FLEXIBUY/WONew?wogid=5611&1490354217603
        if (reqType == "WO" && (requeststatus == "draft" || requeststatus == "rejected")) {
            _tmpUrl = EProcureUrlDet + "WONew?wogid=" + item.GId + "&" + new Date().getTime();
        }
        //Inprocess,Approved ../FLEXIBUY/WONew?wogid=5607&viewtype=view&1490354292437
        if (reqType == "WO" && (requeststatus == "waitingforapproval" || requeststatus == "inprocess")) {
            _tmpUrl = EProcureUrlDet + "WONew?wogid=" + item.GId + "&viewtype=view&" + new Date().getTime();
        }

        var win = window.open(_tmpUrl, '_self');
        if (win) {
            win.focus();
        } else {
            //Browser has blocked it
            jAlert("Please allow popups for this website", "Message");
        }
    }
    self.ViewEPROCURE = function () {
        var item = self.IndexChecker("2");
        if (item == null) {
            jAlert("Sorry! Unable to process your request.", "Message");
            return false;
        }
        self.EPROCUREURL(item);
    };

    //EProcure Approve (or) Reject
    self.EProcureCBF = function (IsReject) {
        var Remarks = $("#txtCBFCheckerRemark").val();

        if ($.trim(self.EProcureID()) == "0" || $.trim(self.EProcureID()) == null) {
            jAlert("Ensure CBF Details!", "Message");
            return false;
        }
        var InputFilter = {
            CBFHeaderGId: self.EProcureID(),
            Type: "4",
            IsReject: IsReject,
            Remarks: Remarks
        };
        showProgress();
        setTimeout(function () {
            $.ajax({
                type: "post",
                url: UrlDet + "ApproveCBF",
                data: JSON.stringify(InputFilter),
                contentType: "application/json;",
                success: function (response) {
                    hideProgress();
                    var Data1 = "";
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                        Data1 = JSON.parse(response.Data1);
                        if (Data1[0].Clear == true) {
                            jAlert(Data1[0].Message, "Success", function () {
                                if (self.IsApprovalClick()) {
                                    self.IsApprovalClick(false);
                                    self.EProcureID(0);
                                    self.IsApprovalSummary(true);
                                    self.IsEProcureDetail(false);
                                    self.LoadApprovalTab();
                                } else {
                                    //Load Next Record
                                    self.EProcureID(0);
                                    self.clickEProcureSummaryLink("1");
                                }
                            });
                        }
                        else {
                            jAlert(Data1[0].Message, "Message");
                        }
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    hideProgress();
                }
            });
        }, 300);
    };
    self.EProcurePO = function (IsReject) {
        var Remarks = $("#txtPOCheckerRemark").val();

        if ($.trim(self.EProcureID()) == "0" || $.trim(self.EProcureID()) == null) {
            jAlert("Ensure PO Details!", "Message");
            return false;
        }
        var InputFilter = {
            POHeaderGId: self.EProcureID(),
            ViewType: "3",
            IsReject: IsReject,
            Remarks: Remarks
        };
        showProgress();
        setTimeout(function () {
            $.ajax({
                type: "post",
                url: UrlDet + "ApprovePO",
                data: JSON.stringify(InputFilter),
                contentType: "application/json;",
                success: function (response) {
                    hideProgress();
                    var Data1 = "";
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                        Data1 = JSON.parse(response.Data1);
                        if (Data1[0].Clear == true) {

                            jAlert(Data1[0].Message, "Success", function () {
                                if (self.IsApprovalClick()) {
                                    self.IsApprovalClick(false);
                                    self.EProcureID(0);
                                    self.IsApprovalSummary(true);
                                    self.IsEProcureDetail(false);
                                    self.LoadApprovalTab();
                                } else {
                                    //Load Next Record
                                    self.EProcureID(0);
                                    self.clickEProcureSummaryLink("1");
                                }
                            });
                        }
                        else {
                            jAlert(Data1[0].Message, "Message");
                        }
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    hideProgress();
                }
            });
        }, 300);
    };
    self.EProcureSubmit = function (Action, IsSubmit) {
        //IsSubmit -- [1-Approve,  2-Rejected]
        var Remarks = $("#txt" + Action + "CheckerRemark").val();
        if ($.trim(self.EProcureID()) == "0" || $.trim(self.EProcureID()) == null) {
            jAlert("Ensure PO Details!", "Message");
            return false;
        }
        var InputFilter = {
            RefGId: self.EProcureID(),
            Action: Action,
            Submit: IsSubmit,
            Remarks: Remarks
        };
        showProgress();
        setTimeout(function () {
            $.ajax({
                type: "post",
                url: UrlDet + "SubmitEprocure",
                data: JSON.stringify(InputFilter),
                contentType: "application/json;",
                success: function (response) {
                    hideProgress();
                    var Data1 = "";
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                        Data1 = JSON.parse(response.Data1);
                        if (Data1[0].Clear == true) {

                            jAlert(Data1[0].Message, "Success", function () {
                                if (self.IsApprovalClick()) {
                                    self.IsApprovalClick(false);
                                    self.EProcureID(0);
                                    self.IsApprovalSummary(true);
                                    self.IsEProcureDetail(false);
                                    self.LoadApprovalTab();
                                } else {
                                    //Load Next Record
                                    self.EProcureID(0);
                                    self.clickEProcureSummaryLink("1");
                                }
                            });
                        }
                        else {
                            jAlert(Data1[0].Message, "Message");
                        }
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    hideProgress();
                }
            });
        }, 300);
    };
    self.PODatatableCall = function () {
        if ($("#gvPO > tbody > tr").length == self.POArray().length) {
            $("#gvPO").DataTable({
                "autoWidth": false,
                "destroy": true,
                "scrollX": true,
                "bFilter": false,
                "bLengthChange": false,
                "aoColumnDefs": [{
                    "aTargets": ["nosort"],
                    "bSortable": false
                }, {
                    "aTargets": ["colDate"],
                    "bSortable": true,
                    "sType": "date-uk"
                }]
            });
        }
    };
    self.loadPOGrid = function () {
        $("#gvPO").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.POArray.removeAll();
    }
    self.loadPOGrid();
    self.WODatatableCall = function () {
        if ($("#gvWO > tbody > tr").length == self.WOArray().length) {
            $("#gvWO").DataTable({
                "autoWidth": false,
                "destroy": true,
                "scrollX": true,
                "bFilter": false,
                "bLengthChange": false,
                "aoColumnDefs": [{
                    "aTargets": ["nosort"],
                    "bSortable": false
                }, {
                    "aTargets": ["colDate"],
                    "bSortable": true,
                    "sType": "date-uk"
                }]
            });
        }
    };
    self.loadWOGrid = function () {
        $("#gvWO").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.WOArray.removeAll();
    }
    self.loadWOGrid();
    self.EProcureSummaryDatatableCall = function () {
        if ($("#gvEProcureSummary > tbody > tr").length == self.EProcureSummaryArray().length) {
            $("#gvEProcureSummary").DataTable({
                "autoWidth": false,
                "destroy": true,
                "scrollX": true,
                "aoColumnDefs": [{
                    "aTargets": ["nosort"],
                    "bSortable": false
                }, {
                    "aTargets": ["colDate"],
                    "bSortable": true,
                    "sType": "date-uk"
                }
                //, { "targets": [1], "visible": self.IsEProcureAction() }
                ]
            });
        }
    };
    self.loadEProcureSummaryGrid = function () {
        $("#gvEProcureSummary").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.EProcureSummaryArray.removeAll();
    }
    self.loadEProcureSummaryGrid();

    //E-Claims
    self.ECLAIMProcess = function (item) {
        self.IsRecordIndex(self.EOWSummaryArray.indexOf(item));
        self.IsApprovalClick(false);
        self.SI_NON_PO(false);
        self.SIDSA(false);
        self.EclaimLC(false);
        self.EclaimTravalType(false);
        self.EclaimARFEP(false);
        self.EclaimARFF(false);

        if (item.ecfdocsubtypegid == "1" || item.ecfdocsubtypegid == "3") { self.EclaimTravalType(true); }
        else if (item.ecfdocsubtypegid == "2") { self.EclaimLC(true); }
        else if (item.ecfdocsubtypegid == "5") { self.SIDSA(true); }
        else if (item.ecfdocsubtypegid == "4") { self.SI_NON_PO(true); }
        else if (item.ecfdocsubtypegid == "6" || item.ecfdocsubtypegid == "8") { self.EclaimARFEP(true); }
        else if (item.ecfdocsubtypegid == "7") { self.EclaimARFF(true); }
        self.DocumentSubType(item.ecfdocsubtypegid);
        self.DocumentTypeId(item.ecfdoctypegid);
        showProgress();
        setTimeout(function () {
            var _Filter = {
                ECFId: item.ecfgid,
                DocSubtype: item.ecfdocsubtypegid
            };
            self.loadEClaimPOGrid();
            self.TravalArray.removeAll();
            $.ajax({
                type: "post",
                url: UrlDet + "GetEClaimsDetails",
                data: JSON.stringify(_Filter),
                contentType: "application/json;",
                success: function (response) {
                    hideProgress();
                    var Data1 = "", Data2 = "";
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                        Data1 = JSON.parse(response.Data1);
                        if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                            Data2 = JSON.parse(response.Data2);
                        }

                        if (Data1.length > 0) {
                            if (self.DocumentSubType() == "1" || self.DocumentSubType() == "3") {
                                self.TravalArray(Data2);
                                self.EClaimID(Data1[0].ECFId);
                                //ECFId  
                                $("#txtEClaimTravalRaiserDetails").val(Data1[0].Raiser);
                                $("#txtEClaimTravalECFNo").val(Data1[0].ECFNo);
                                $("#txtEClaimTravalECFAmount").val(Data1[0].ECFAmount);
                                $("#txtEClaimTravalPeriodFrom").val(Data1[0].PeriodFrom);
                                $("#txtEClaimTravalPeriodTo").val(Data1[0].PeriodTo);
                                $("#txtEClaimTravalServiceMonth").val(Data1[0].ServiceMonth);

                                $("#txtEClaimTravalBranch").val(Data1[0].BranchName);
                                $("#txtEClaimTravalRaiserComments").val(Data1[0].RaiserComments);
                                $("#txtEClaimTravalPreviousApproverDetails").val(Data1[0].PreviousApprover);
                                $("#txtEClaimTravalPreviousApproverReason").val(Data1[0].PreviousApproverRemarks);

                                $("#txtEClaimTravalCheckerRemark").val("");
                                $("#txtEClaimTravalCheckerRemark").removeClass("valid").removeClass("required").addClass("required");
                                checkEClaimTravalValidationUpdate();
                            }
                            else if (self.DocumentSubType() == "2") {
                                //ECFId
                                self.EClaimID(Data1[0].ECFId);
                                $("#txtEClaimLCRaiserDetails").val(Data1[0].Raiser);
                                $("#txtEClaimLCECFNo").val(Data1[0].ECFNo);
                                $("#txtEClaimLCECFAmount").val(Data1[0].ECFAmount);
                                $("#txtEClaimLCNoOfEmployee").val(Data1[0].NoOfPersons);
                                $("#txtEClaimLCServiceMonth").val(Data1[0].ServiceMonth);
                                $("#txtEClaimLCHighestClaimAmt").val(Data1[0].HighestClaimAmount);
                                $("#txtEClaimLCBranch").val(Data1[0].BranchName);
                                $("#txtEClaimLCRaiserComments").val(Data1[0].RaiserComments);
                                $("#txtEClaimLCPreviousApproverDetails").val(Data1[0].PreviousApprover);
                                $("#txtEClaimLCPreviousApproverReason").val(Data1[0].PreviousApproverRemarks);


                                $("#txtEClaimLCCheckerRemark").val("");
                                $("#txtEClaimLCCheckerRemark").removeClass("valid").removeClass("required").addClass("required");
                                checkEClaimLCValidationUpdate();
                            }

                            else if (self.DocumentSubType() == "5") {
                                //ECFId
                                self.EClaimID(Data1[0].ECFId);
                                $("#txtEClaimDSASupplierDetails").val(Data1[0].Supplier);
                                $("#txtEClaimDSARaiserDetails").val(Data1[0].Raiser);
                                $("#txtEClaimDSAECFNo").val(Data1[0].ECFNo);
                                $("#txtEClaimDSAECFAmount").val(Data1[0].ECFAmount);
                                $("#txtEClaimDSANoOfVendor").val(Data1[0].NoOfPersons);
                                $("#txtEClaimDSAServiceMonth").val(Data1[0].ServiceMonth);
                                $("#txtEClaimDSARaiserComments").val(Data1[0].RaiserComments);
                                $("#txtEClaimDSAPreviousApproverDetails").val(Data1[0].PreviousApprover);
                                $("#txtEClaimDSAPreviousApproverReason").val(Data1[0].PreviousApproverRemarks);

                                $("#txtEClaimDSACheckerRemark").val("");
                                $("#txtEClaimDSACheckerRemark").removeClass("valid").removeClass("required").addClass("required");
                                checkEClaimDSAValidationUpdate();
                            }

                            else if (self.DocumentSubType() == "4") {
                                self.POType(Data1[0].POType);

                                //ECFId
                                self.EClaimID(Data1[0].ECFId);
                                $("#txtEClaimPOECFNo").val(Data1[0].ECFNo);
                                $("#txtEClaimPOECFAmount").val(Data1[0].ECFAmount);
                                $("#txtEClaimPORaiserDetails").val(Data1[0].Raiser);
                                $("#txtEClaimPOSupplierDetails").val(Data1[0].Supplier);

                                $("#txtEClaimPORaiserComments").val(Data1[0].RaiserComments);
                                $("#txtEClaimPOPreviousApproverDetails").val(Data1[0].PreviousApprover);
                                $("#txtEClaimPOPreviousApproverReason").val(Data1[0].PreviousApproverRemarks);

                                $("#txtEClaimPOCheckerRemark").val("");
                                $("#txtEClaimPOCheckerRemark").removeClass("valid").removeClass("required").addClass("required");

                                checkEClaimPOValidationUpdate();
                                self.EClaimPOArray(Data2);
                            }
                            else if (self.DocumentSubType() == "6" || self.DocumentSubType() == "8") {
                                self.EClaimID(Data1[0].ARFId);
                                //ARFId
                                $("#txtEClaimARFEPRaiserDetails").val(Data1[0].Raiser);
                                $("#txtEClaimARFEPECFNo").val(Data1[0].ARFNo);
                                $("#txtEClaimARFEPECFAmount").val(Data1[0].ARFAmount);
                                $("#txtEClaimARFEPARFType").val(Data1[0].ARFType);
                                $("#txtEClaimARFEPAdvanceType").val(Data1[0].Advancetype);
                                $("#txtEClaimARFEPLiqDate").val(Data1[0].TargetLiqDate);
                                $("#txtEClaimARFEPBranch").val(Data1[0].BranchName);
                                $("#txtEClaimARFEPDescription").val(Data1[0].ARFDescription);
                                $("#txtEClaimARFEPRaiserComments").val(Data1[0].RaiserComments);
                                $("#txtEClaimARFEPPreviousApproverDetails").val(Data1[0].PreviousApprover);
                                $("#txtEClaimARFEPPreviousApproverReason").val(Data1[0].PreviousApproverRemarks);

                                $("#txtEClaimARFEPCheckerRemark").val("");
                                $("#txtEClaimARFEPCheckerRemark").removeClass("valid").removeClass("required").addClass("required");
                                checkEClaimARFEPValidationUpdate();
                            }
                            else if (self.DocumentSubType() == "7") {
                                self.EClaimID(Data1[0].ARFId);
                                //ARFId
                                $("#txtEClaimARFFRaiserDetails").val(Data1[0].Raiser);
                                $("#txtEClaimARFFSupplierDetails").val(Data1[0].SupplierEmployeeName);
                                $("#txtEClaimARFFECFNo").val(Data1[0].ARFNo);
                                $("#txtEClaimARFFAmount").val(Data1[0].ARFAmount);
                                $("#txtEClaimARFFARFType").val(Data1[0].ARFType);
                                $("#txtEClaimARFFAdvanceType").val(Data1[0].Advancetype);
                                $("#txtEClaimARFFLiqDate").val(Data1[0].TargetLiqDate);
                                $("#txtEClaimARFFBranch").val(Data1[0].BranchName);

                                $("#txtEClaimARFFProformaInv").val(Data1[0].PromoInvoice);
                                $("#txtEClaimARFFPO").val(Data1[0].PONo);

                                $("#txtEClaimARFFDescription").val(Data1[0].ARFDescription);
                                $("#txtEClaimARFFRaiserComments").val(Data1[0].RaiserComments);
                                $("#txtEClaimARFFPreviousApproverDetails").val(Data1[0].PreviousApprover);
                                $("#txtEClaimARFFPreviousApproverReason").val(Data1[0].PreviousApproverRemarks);

                                $("#txtEClaimARFFCheckerRemark").val("");
                                $("#txtEClaimARFFCheckerRemark").removeClass("valid").removeClass("required").addClass("required");
                                checkEClaimARFFValidationUpdate();
                            }
                            self.IsEclaims(false);
                            self.IsEclaimsDetail(true);
                        }
                    } else {
                        //show error detail
                        self.DocumentTypeId(0);
                        jAlert("In-valid ECF Details!", "Error");
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    hideProgress();
                }
            });
        }, 400);
    };
    self.EClaimSubmit = function (Action, IsSubmit) {
        //IsSubmit -- [1-Approve,  2-Rejected]
        var Remarks = $("#txt" + Action + "CheckerRemark").val();
        var _concurrentApproval = $("#chk" + Action + "IsConcurrentApp").is(':checked');
        var _concurrentApprovalId = "0";
        if ($.trim(self.EClaimID()) == "0" || $.trim(self.EClaimID()) == null) {
            jAlert("Ensure Details!", "Message");
            return false;
        }

        if (_concurrentApproval) {
            _concurrentApprovalId = $("#hdfSConcurrentApprovalId").val();
            if (_concurrentApprovalId == null || _concurrentApprovalId == "" || _concurrentApprovalId == "0" || _concurrentApprovalId == undefined) {
                jAlert("Ensure Concurrent Approval User!", "Message");
                return false;
            }
        }
        /* console.log(self.EClaimID());
         console.log(self.DocumentTypeId());
         return false;*/
        var InputFilter = {
            EcfId: self.EClaimID(),
            DocType: self.DocumentTypeId(),
            Submit: IsSubmit,
            Remarks: Remarks,
            ConcurrentTo: _concurrentApprovalId
        };

        showProgress();
        setTimeout(function () {
            $.ajax({
                type: "post",
                url: UrlDet + "SubmitEClaim",
                data: JSON.stringify(InputFilter),
                contentType: "application/json;",
                success: function (response) {
                    hideProgress();
                    var Data1 = "";

                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                        Data1 = JSON.parse(response.Data1);
                        if (Data1[0].Clear == true) {
                            jAlert(Data1[0].Message, "Success", function () {
                                if (self.IsApprovalClick()) {
                                    self.IsApprovalClick(false);
                                    self.EClaimID(0);
                                    self.EClaimQueueGID(0);
                                    self.IsApprovalSummary(true);
                                    self.IsEclaimsDetail(false);
                                    self.LoadApprovalTab();
                                } else {
                                    //Load Next Record
                                    self.EClaimID(0);
                                    self.clickECLAIMSSummaryLink("1");
                                }
                            });
                        }
                        else {
                            jAlert(Data1[0].Message, "Message");
                        }
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    hideProgress();
                }
            });
        }, 300);
    };

    self.ECLAIMURL = function (item) {
        debugger;
        var _tmpUrl = "";
        var reqType = self.RequestType().toUpperCase();
        var requeststatus = self.RequestStatus().toLowerCase();

        if (requeststatus == "getmydocdraft") {
            //location.href = '@Url.Action("selectdatadraft", "DashBoard")?id=' + ecfnum + '&' + new Date().getTime();
            _tmpUrl = EOWUrlDet + "DashBoard/selectdatadraft?id=" + item.ecfgid + "&" + new Date().getTime();
        }
        else if (requeststatus == "getmydocreject") {
            //location.href = '@Url.Action("selectdata", "DashBoard")?id=' + ecfnum + '&' + new Date().getTime();
            _tmpUrl = EOWUrlDet + "DashBoard/selectdata?id=" + item.Queuegid + "&" + new Date().getTime();
        }
        if (requeststatus == "queueformyapprvolcount" || requeststatus == "getmydocappoalc" || requeststatus == "inprocess") {
            //location.href = '@Url.Action("Viewdata", "DashBoard")?id=' + ecfnum + '&' + new Date().getTime();
            _tmpUrl = EOWUrlDet + "DashBoard/Viewdata?id=" + item.Queuegid + "&" + new Date().getTime();
        }

        var win = window.open(_tmpUrl);
        if (win) {
            win.focus();
        } else {
            //Browser has blocked it
            jAlert("Please allow popups for this website", "Message");
        }
    }
    self.ViewECF = function () {
        var item = self.IndexChecker("3");
        if (item == null) {
            jAlert("Sorry! Unable to process your request.", "Message");
            return false;
        }
        self.ECLAIMURL(item);
    };

    self.clickECLAIMS = function (RequestType, Status, CheckField, data) {
        debugger;
        //location.href = '@Url.Action("MyDocDetails", "MyDashDocDetails")?queuefor=submitmode&requesttype=' + RequestType + "&reqstatus=" + CheckField;
        //bharathi kumar       location.href = "/eow/MyDashDocDetails/MyDocDetails?queuefor=submitmode&requesttype=" + RequestType + "&reqstatus=" + CheckField;
        var requeststatus;

        if (CheckField == "ForMyApproval") {
            requeststatus = "forapproval";
        }
        else if (CheckField == "DRAFT") {
            requeststatus = 'draft';
        }
        else if (CheckField == "REJECTED") {
            requeststatus = "rejected";
        }
        else if (CheckField = "INPROCESS") {
            requeststatus = 'inprocess';
        }


        if (RequestType == 'Employee Claims') {

            var reqType = 'EMPLOYEE CLAIMS';

            //   location.href = '@Url.Action("MyDocDetails", "MyDashDocDetails")?queuefor=submitmode&requesttype=' + RequestType + "&reqstatus=" + requeststatus;

            //location = ('/eow/MyDashDocDetails/MyDocDetails?queuefor= submitmode&requesttype=' + reqType + "&reqstatus=" + requeststatus);
            location = EOWUrlDet + "MyDashDocDetails/MyDocDetails?queuefor=submitmode&requesttype=" + reqType + "&reqstatus=" + requeststatus;

        }
        else if (RequestType == 'Advance Request') {
            //location.href = '@Url.Action("MyDocDetails", "MyDashDocDetails")?queuefor=submitmode&requesttype=' + RequestType + "&reqstatus=" + Status;
            var reqType = 'ADVANCE REQUEST';
            //location = "/eow/MyDashDocDetails/MyDocDetailsarf?queuefor=submitmode&requesttype=" + reqType + "&reqstatus=" + requeststatus;
            location = EOWUrlDet + "MyDashDocDetails/MyDocDetails?queuefor=submitmode&requesttype=" + reqType + "&reqstatus=" + requeststatus;
        }
        else if (RequestType == 'Supplier Invoice') {
            //location.href = '@Url.Action("MyDocDetails", "MyDashDocDetails")?queuefor=submitmode&requesttype=' + RequestType + "&reqstatus=" + Status;
            var reqType = 'SUPPLIER INVOICE';
            // location = ('/eow/MyDashDocDetails/MyDocDetails?queuefor=submitmode&requesttype=' + reqType + "&reqstatus=" + requeststatus);
            location = EOWUrlDet + "MyDashDocDetails/MyDocDetails?queuefor=submitmode&requesttype=" + reqType + "&reqstatus=" + requeststatus;
        }

        else if (RequestType == 'INSURANCE') {
            //location.href = '@Url.Action("MyDocDetails", "MyDashDocDetails")?queuefor=submitmode&requesttype=' + RequestType + "&reqstatus=" + Status;
            var reqType = 'INSURANCE';
            //location = ('/eow/MyDashDocDetails/MyDocDetails?queuefor=submitmode&requesttype=' + reqType + "&reqstatus=" + requeststatus);
            location = EOWUrlDet + "MyDashDocDetails/MyDocDetails?queuefor=submitmode&requesttype=" + reqType + "&reqstatus=" + requeststatus;
        }
        else if (RequestType == 'INSURANCE ADVANCE') {
            // location.href = '@Url.Action("MyDocDetails", "MyDashDocDetails")?queuefor=submitmode&requesttype=' + RequestType + "&reqstatus=" + Status;
            var reqType = 'INSURANCE ADVANCE';
            //location = ('/eow/MyDashDocDetails/MyDocDetails?queuefor=submitmode&requesttype=' + reqType + "&reqstatus=" + requeststatus);
            location = EOWUrlDet + "MyDashDocDetails/MyDocDetails?queuefor=submitmode&requesttype=" + reqType + "&reqstatus=" + requeststatus;
        }

        /*  self.IsRecordIndex(0);
          //Block Value with zero
          //if (data[CheckField] == "0") {
          //    return false;
          //}
          self.EClaimID(0);
          self.DocumentTypeId(0);
          showProgress();
          setTimeout(function () {
              var _RequestType = "";
              if (RequestType == "Employee Claims") { _RequestType = "1"; }
              if (RequestType == "Supplier Invoice") { _RequestType = "3"; }
              if (RequestType == "Advance Request") { _RequestType = "2"; }
  
              self.RequestType(RequestType);
              self.RequestStatus(Status);
  
              self.loadEOWGrid();
              var _Filter = {
                  action: 'ECLAIMS',
                  requesttype: _RequestType,
                  requeststatus: Status
              };
  
              if (Status != "queueformyapprvolcount") {
                  self.IsEClaimAction(false);
              } else {
                  self.IsEClaimAction(true);
              }
  
              $.ajax({
                  type: "post",
                  url: UrlDet + "GetDashboardSummary",
                  data: JSON.stringify(_Filter),
                  contentType: "application/json;",
                  success: function (response) {
                      hideProgress();
                      var Data1 = "";
                      if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                          Data1 = JSON.parse(response.Data1);
                      }
                      self.EOWSummaryArray(Data1 == "" ? [] : Data1);
  
                      if (Data1.length > 0) {
                          self.EClaimTitle(RequestType);
                          self.IsDashbaord(false);
                          self.IsVendorSummary(false);
                          self.IsEProcure(false);
                          self.IsEclaims(true);
                      }
                  },
                  error: function (XMLHttpRequest, textStatus, errorThrown) {
                      hideProgress();
                  }
              });
          }, 500); */
    };


    //For Fixed Asset Dasshboard Click 11-13-2019 WOA,TOA,SOA
    self.clickFIXEDASSET = function (RequestType, Status, data) {
        debugger;
        var requeststatus;

        if (Status == "ForMyApproval") {
            requeststatus = "forapproval";
        }
        if (Status == "DRAFT") {
            requeststatus = "DRAFT";
        }
        if (Status == "INPROCESS") {
            requeststatus = "INPROCESS";
        }
        if (Status == "REJECTED") {
            requeststatus = "REJECTED";
        }
        if (RequestType == "TOA" && Status == "FORMYAPPROVAL") {
            location = IFAMSUrlDet + "TransferedSummary?queuefor=submitmode&requesttype=" + RequestType + "&reqstatus=" + requeststatus;
        }
        if (RequestType == "TOA" && Status == "DRAFT") {
            location = IFAMSUrlDet + "TMSummary?queuefor=submitmode&requesttype=" + RequestType + "&reqstatus=" + requeststatus;
        }

        if (RequestType == "TOA" && Status == "INPROCESS") {
            location = IFAMSUrlDet + "TrfDasboardInprocessSummary?queuefor=submitmode&requesttype=" + RequestType + "&reqstatus=" + requeststatus;
        }

        if (RequestType == "TOA" && Status == "REJECTED") {
            location = IFAMSUrlDet + "TMSummary?queuefor=submitmode&requesttype=" + RequestType + "&reqstatus=" + requeststatus;
        }

        if (RequestType == "WOA" && Status == "DRAFT") {
            location = IFAMSUrlDetWOA + "WMSummary?queuefor=submitmode&requesttype=" + RequestType + "&reqstatus=" + requeststatus;
        }
        if (RequestType == "WOA" && Status == "FORMYAPPROVAL") {
            location = IFAMSUrlDetWOA + "WCSummary?queuefor=submitmode&requesttype=" + RequestType + "&reqstatus=" + requeststatus;
        }
        if (RequestType == "WOA" && Status == "INPROCESS") {

            location = IFAMSUrlDetWOA + "WritoffDasboardInprocessSummary?queuefor=submitmode&requesttype=" + RequestType + "&reqstatus=" + requeststatus;
        }
        if (RequestType == "WOA" && Status == "REJECTED") {
            location = IFAMSUrlDetWOA + "WMSummary?queuefor=submitmode&requesttype=" + RequestType + "&reqstatus=" + requeststatus;
        }

        if (RequestType == "SOA" && Status == "DRAFT") {
            location = IFAMSUrlDetSOA + "SASummary?queuefor=submitmode&requesttype=" + RequestType + "&reqstatus=" + requeststatus;
        }
        if (RequestType == "SOA" && Status == "FORMYAPPROVAL") {
            location = IFAMSUrlDetSOA + "SAChkSummary?queuefor=submitmode&requesttype=" + RequestType + "&reqstatus=" + requeststatus;
        }
        if (RequestType == "SOA" && Status == "INPROCESS") {
            location = IFAMSUrlDetSOA + "SaleDasboardInprocessSummary?queuefor=submitmode&requesttype=" + RequestType + "&reqstatus=" + requeststatus;
        }

        if (RequestType == "SOA" && Status == "REJECTED") {
            location = IFAMSUrlDetSOA + "SASummary?queuefor=submitmode&requesttype=" + RequestType + "&reqstatus=" + requeststatus;
        }

    };

    self.clickECLAIMSSummaryLink = function (IsSubmit) {
        debugger;
        self.EClaimID(0);
        self.DocumentTypeId(0);
        showProgress();
        setTimeout(function () {
            var _RequestType = "";
            if (self.RequestType() == "Employee Claims") { _RequestType = "1"; }
            if (self.RequestType() == "Supplier Invoice") { _RequestType = "3"; }
            if (self.RequestType() == "Advance Request") { _RequestType = "2"; }
            self.loadEOWGrid();
            var _Filter = {
                action: 'ECLAIMS',
                requesttype: _RequestType,
                requeststatus: self.RequestStatus()
            };
            if (self.RequestStatus() != "queueformyapprvolcount") {
                self.IsEClaimAction(false);
            } else {
                self.IsEClaimAction(true);
            }
            $.ajax({
                type: "post",
                url: UrlDet + "GetDashboardSummary",
                data: JSON.stringify(_Filter),
                contentType: "application/json;",
                success: function (response) {
                    hideProgress();
                    var Data1 = "";
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                        Data1 = JSON.parse(response.Data1);
                    }
                    self.EOWSummaryArray(Data1);
                    if (IsSubmit == "0" && self.EOWSummaryArray().length > 0) {
                        self.EClaimTitle(self.RequestType());
                        self.IsEclaimsDetail(false);
                        self.IsEclaims(true);
                    }
                    if (IsSubmit == "1" && self.EOWSummaryArray().length > 0) {
                        jConfirm("Want to load Next Record?", "Message", function (callback) {
                            if (callback == true) {
                                var item = self.IndexChecker("3");
                                if (item == null) {
                                    self.IsRecordIndex(0);
                                    self.LoadDashboard();
                                } else {
                                    self.ECLAIMProcess(item);
                                }
                            } else if (callback == false) {
                                self.IsRecordIndex(0);
                                self.IsEclaimsDetail(false);
                                self.IsEclaims(true);
                            }
                        });
                        $("#popup_ok").attr("style", "margin-left: 0px;");
                        $("#popup_cancel").attr("style", "margin-left: 5px;");
                    } else if (IsSubmit == "1" && self.EOWSummaryArray().length == 0) {
                        self.IsRecordIndex(0);
                        self.LoadDashboard();
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    hideProgress();
                }
            });
        }, 500);
    };
    self.EClaimPODatatableCall = function () {
        if ($("#gvEClaimNONPO > tbody > tr").length == self.EClaimPOArray().length) {
            $("#gvEClaimNONPO").DataTable({
                "autoWidth": false,
                "destroy": true,
                "scrollX": true,
                "bFilter": false,
                "bLengthChange": false,
                "aoColumnDefs": [
                    { "aTargets": ["nosort"], "bSortable": false },
                    { "aTargets": ["colDate"], "bSortable": true, "sType": "date-uk" },
                    { "targets": [4], "visible": self.POType() == "N" ? false : true }
                ]
            });
        }
    };
    self.loadEClaimPOGrid = function () {
        $("#gvEClaimNONPO").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.EClaimPOArray.removeAll();
    }
    self.loadEClaimPOGrid();
    self.EOWDatatableCall = function () {
        if ($("#gvEOW > tbody > tr").length == self.EOWSummaryArray().length) {
            $("#gvEOW").DataTable({
                "autoWidth": false,
                "destroy": true,
                "scrollX": true,
                "aoColumnDefs": [{
                    "aTargets": ["nosort"],
                    "bSortable": false
                }, {
                    "aTargets": ["colDate"],
                    "bSortable": true,
                    "sType": "date-uk"
                }
                //, { "targets": [1], "visible": self.IsEClaimAction() }
                ]
            });
        }
    };
    self.loadEOWGrid = function () {
        $("#gvEOW").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.EOWSummaryArray.removeAll();
    }
    self.loadEOWGrid();

    //Load Dashboard data
    self.LoadDashboard = function () {
        self.IsDashbaord(true);
        self.IsVendorSummary(false);
        self.IsEProcure(false);
        self.IsEclaims(false);
        self.IsVendorDetail(false);
        self.IsEProcureDetail(false);
        self.IsEclaimsDetail(false);
        self.IsRecordIndex(0);
        self.VendorTitle("");
        self.FillData();
    };
    self.FillData = function () {
        debugger;
        showProgress();
        $.ajax({
            type: "post",
            url: UrlDet + "GetOverAllDashboard",
            data: '{}',
            contentType: "application/json;",
            success: function (response) {
                debugger;
                hideProgress();
                var Data1 = "", Data2 = "", Data3 = "", Data4 = "", Data5 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                }
                self.VendorArray(Data1);

                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                }
                self.EProcureArray(Data2);

                if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null) {
                    Data3 = JSON.parse(response.Data3);
                }
                self.EOWArray(Data3);

                if (response.Data4 != null && response.Data4 != "" && JSON.parse(response.Data4) != null) {
                    Data4 = JSON.parse(response.Data4);
                }

                //For Fixed Asset TOA AND WOA Dash board
                if (response.Data5 != null && response.Data5 != "" && JSON.parse(response.Data5) != null) {
                    Data5 = JSON.parse(response.Data5);
                    self.FixedassetArray(Data5);
                }


                var labelsForPieChart = [];
                var data = [];
                ko.utils.arrayForEach(Data4, function (obj) {
                    if (obj.StatusType != "Total Suppliers") {
                        labelsForPieChart.push(obj.StatusType);
                        data.push(obj.TotCount);
                    }
                });

                var ctx = document.getElementById("supplierPieChart");
                var data = {
                    labels: labelsForPieChart,
                    datasets: [{
                        data: data,
                        backgroundColor: ["#4BC0C0", "#36A2EB", "#FF6384", "#FFCE56"],
                        hoverBackgroundColor: ["#4BC0C0", "#36A2EB", "#FF6384", "#FFCE56"]
                    }]
                };
                var myPieChart = new Chart(ctx, {
                    type: 'pie',
                    data: data,
                    responsive: true
                });
                self.VendorStatusArray(Data4);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
                hideProgress();
            }
        });
    };
    self.FillData();

    //Load My Request
    self.LoadRequestTab = function () {
        debugger;
        showProgress();
        setTimeout(function () {
            debugger;
            self.loadMyReqASMSGrid();
            self.loadgvMyReqEPROCUREGrid();
            self.loadMyReqEOWGrid();
            self.loadgvMyReqFixedAssetGrid();  // For Fixed Asset Workbench grid
            self.IsDashbaord(true);
            self.IsVendorSummary(false);
            self.IsEProcure(false);
            self.IsEclaims(false);
            self.IsVendorDetail(false);
            self.IsEProcureDetail(false);
            self.IsEclaimsDetail(false);
            self.IsRecordIndex(0);
            $.ajax({
                type: "post",
                url: UrlDet + "GetMyRequestTabDetails",
                data: '{}',
                contentType: "application/json;",
                success: function (response) {
                    debugger
                    hideProgress();
                    var Data1 = "", Data2 = "", Data3 = "", Data4 = "";
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                        Data1 = JSON.parse(response.Data1);
                    }
                    self.MyReqASMSArray(Data1);
                    if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                        Data2 = JSON.parse(response.Data2);
                    }
                    self.MyReqEProcureArray(Data2);
                    if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null) {
                        Data3 = JSON.parse(response.Data3);
                    }
                    self.MyReqEOWArray(Data3);

                    //For Fixed Asset  Work bench
                    if (response.Data4 != null && response.Data4 != "" && JSON.parse(response.Data4) != null) {
                        Data4 = JSON.parse(response.Data4);
                    }
                    self.FixedassetArray1(Data4);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {

                    hideProgress();
                }
            });
        }, 300);
    };

    //ASMS
    self.MyReqASMSURL = function (item) {
        debugger;
        var _tmpUrl = "";
        var reqType = item.supplierheaderrequesttype.toUpperCase();
        var requeststatus = item.supplierheaderrequeststatus.toLowerCase();
        var _mode = "5";

        if (requeststatus == "draft" || requeststatus == "rejected") {
            _mode = "4"
        } else if (requeststatus == "inprocess") {
            _mode = "5"
        }
        _tmpUrl = ASMSUrlDet + "Onboarding/Index?pagefor=" + _mode + "&supid=" + item.Supplierheadergid + '&' + new Date().getTime();
        if (_tmpUrl == "") return false;
        var win = window.open(_tmpUrl, '_self');
        if (win) {
            win.focus();
        } else {
            //Browser has blocked it
            jAlert("Please allow popups for this website", "Message");
        }
    }
    self.MyReqASMSDatatableCall = function () {
        if ($("#gvMyReqASMS > tbody > tr").length == self.MyReqASMSArray().length) {
            $("#gvMyReqASMS").DataTable({
                "autoWidth": false,
                "scrollX": true,
                "iDisplayLength": 5,
                "bLengthChange": false,
                "aoColumnDefs": [{
                    "aTargets": ["nosort"],
                    "bSortable": false
                }, {
                    "aTargets": ["colDate"],
                    "bSortable": true,
                    "sType": "date-uk"
                }
                ]
            });
        }
    };
    self.loadMyReqASMSGrid = function () {
        $("#gvMyReqASMS").DataTable({
            "autoWidth": false,
            "bLengthChange": false,
            "destroy": true
        }).destroy();
        self.MyReqASMSArray([]);
    }
    self.loadMyReqASMSGrid();

    //EPROCURE
    self.MyReqEPROCUREURL = function (item) {
        debugger;
        var _tmpUrl = "";
        var reqType = item.RequestType.toUpperCase();
        var requeststatus = item.RequestStatus.toLowerCase();

        if (reqType == "PR" && (requeststatus == "draft" || requeststatus == "rejected")) {
            _tmpUrl = EProcureUrlDet + "PRNew?prgid=" + item.GId + "&viewtype=edit&" + new Date().getTime();
        }
        if (reqType == "PR" && (requeststatus != "draft" && requeststatus != "rejected")) {
            _tmpUrl = EProcureUrlDet + "PRNew?prgid=" + item.GId + "&viewtype=view&" + new Date().getTime();
        }

        if (reqType == "CBF" && (requeststatus == "draft" || requeststatus == "rejected")) {
            _tmpUrl = EProcureUrlDet + "CBF/Edit/" + item.GId;
        }
        if (reqType == "CBF" && (requeststatus != "draft" && requeststatus != "rejected")) {
            _tmpUrl = EProcureUrlDet + "CBF/Edit/" + item.GId + "/1";
        }

        if (reqType == "PAR" && (requeststatus == "draft" || requeststatus == "rejected")) {
            _tmpUrl = EProcureUrlDet + "PARNew?pargid=" + item.GId + "&viewtype=edit&" + new Date().getTime();
        }
        if (reqType == "PAR" && (requeststatus != "draft" && requeststatus != "rejected")) {
            _tmpUrl = EProcureUrlDet + "PARNew?pargid=" + item.GId + "&viewtype=view&" + new Date().getTime();
        }

        if (reqType == "PO" && (requeststatus == "draft" || requeststatus == "rejected")) {
            _tmpUrl = EProcureUrlDet + "PO/RaisePO/" + item.GId;
        }
        if (reqType == "PO" && (requeststatus != "draft" && requeststatus != "rejected")) {
            _tmpUrl = EProcureUrlDet + "PO/RaisePO/" + item.GId + "/0";
        }

        if (reqType == "WO" && (requeststatus == "draft" || requeststatus == "rejected")) {
            _tmpUrl = EProcureUrlDet + "WONew?wogid=" + item.GId + "&" + new Date().getTime();
        }
        if (reqType == "WO" && (requeststatus != "draft" && requeststatus != "rejected")) {
            _tmpUrl = EProcureUrlDet + "WONew?wogid=" + item.GId + "&viewtype=view&" + new Date().getTime();
        }
        if (_tmpUrl == "") return false;
        var win = window.open(_tmpUrl, '_self');
        if (win) {
            win.focus();
        } else {
            //Browser has blocked it
            jAlert("Please allow popups for this website", "Message");
        }
    }
    self.MyReqEPROCUREDatatableCall = function () {
        debugger
        if ($("#gvMyReqEPROCURE > tbody > tr").length == self.MyReqEProcureArray().length) {
            $("#gvMyReqEPROCURE").DataTable({
                "autoWidth": false,
                "destroy": true,
                "scrollX": true,
                "iDisplayLength": 5,
                "bLengthChange": false,
                "aoColumnDefs": [{
                    "aTargets": ["nosort"],
                    "bSortable": false
                }, {
                    "aTargets": ["colDate"],
                    "bSortable": true,
                    "sType": "date-uk"
                }
                ]
            });
        }
    };
    self.loadgvMyReqEPROCUREGrid = function () {
        $("#gvMyReqEPROCURE").DataTable({
            "autoWidth": false,
            "bLengthChange": false,
            "destroy": true
        }).destroy();
        self.MyReqEProcureArray([]);
    }
    self.loadgvMyReqEPROCUREGrid();

    //For Fixed Asset Load Work bench grid

    self.MyReqFAURL = function (item) {  //Work bench click link button 
        debugger;
        var _tmpUrl = "";
        var reqType = item.RequestType.toUpperCase();
        var requeststatus = item.status_name.toLowerCase();
        var Gid = item.Gid;

        if (reqType == "TOA" && (requeststatus == "draft" || requeststatus == "rejected")) {
            //_tmpUrl = IFAMSUrlDet + "TMSummary?toagid=" + item.Gid + new Date().getTime();
            _tmpUrl = IFAMSUrlDet + "TMSummary?toagid=" + item.Gid + "";

        }
        if (reqType == "TOA" && (requeststatus == "inprocess")) {
            //_tmpUrl = IFAMSUrlDet + "TrfDasboardInprocessSummary?toagid=" + item.Gid + new Date().getTime();
            _tmpUrl = IFAMSUrlDet + "TrfDasboardInprocessSummary?toagid=" + item.Gid + "";
        }
        if (reqType == "TOA" && (requeststatus == "waiting for approval")) {
            _tmpUrl = IFAMSUrlDet + "TransferedSummary?toagid=" + item.Gid + new Date().getTime();
        }
        if (reqType == "SOA" && (requeststatus == "draft" || requeststatus == "rejected")) {
            _tmpUrl = IFAMSUrlDetSOA + "SASummary?soagid=" + item.Gid + new Date().getTime();
        }
        if (reqType == "SOA" && (requeststatus == "inprocess")) {
            _tmpUrl = IFAMSUrlDetSOA + "SaleDasboardInprocessSummary?soagid=" + item.Gid + new Date().getTime();
        }
        if (reqType == "SOA" && (requeststatus == "delmat approved")) {
            _tmpUrl = IFAMSUrlDetSOA + "SASummary?soagid=" + item.Gid + new Date().getTime();
        }

        if (reqType == "WOA" && (requeststatus == "draft" || requeststatus == "rejected")) {
            _tmpUrl = IFAMSUrlDetWOA + "WMSummary?woagid=" + item.Gid + new Date().getTime();
        }
        if (reqType == "WOA" && (requeststatus == "inprocess")) {
            _tmpUrl = IFAMSUrlDetWOA + "WCSummary?woagid=" + item.Gid + new Date().getTime();
        }
        if (reqType == "WOA" && (requeststatus == "delmat approved")) {
            _tmpUrl = IFAMSUrlDetWOA + "WCSummary?woagid=" + item.Gid + new Date().getTime();
        }

        if (_tmpUrl == "") return false;
        var win = window.open(_tmpUrl, '_self');
        if (win) {
            win.focus();
        } else {
            //Browser has blocked it
            jAlert("Please allow popups for this website", "Message");
        }
    }

    self.MyReqFixedAssetDatatableCall = function () {
        debugger;
        if ($("#gvMyReqFixedAsset > tbody > tr").length == self.FixedassetArray1().length) {
            $("#gvMyReqFixedAsset").DataTable({
                "autoWidth": false,
                "destroy": true,
                "scrollX": true,
                "iDisplayLength": 5,
                "bLengthChange": false,
                "aoColumnDefs": [{
                    "aTargets": ["nosort"],
                    "bSortable": false
                }, {
                    "aTargets": ["colDate"],
                    "bSortable": true,
                    "sType": "date-uk"
                }
                ]
            });
        }
    };

    self.loadgvMyReqFixedAssetGrid = function () {
        debugger;
        $("#gvMyReqFixedAsset").DataTable({
            "autoWidth": false,
            "bLengthChange": false,
            "destroy": true
        }).destroy();
        self.FixedassetArray1([]);
    }

    self.loadgvMyReqFixedAssetGrid();

    // End

    //EOW
    self.MyReqECLAIMURL = function (item) {
        debugger
        var _tmpUrl = "";
        var reqType = item.DocType.toUpperCase();
        var requeststatus = item.DocStatus.toLowerCase();

        if (requeststatus == "draft") {
            _tmpUrl = EOWUrlDet + "DashBoard/selectdatadraft?id=" + item.ecfGid + "&" + new Date().getTime();
        }
        else if (requeststatus == "rejected") {
            _tmpUrl = EOWUrlDet + "DashBoard/selectdata?id=" + (item.Queuegid == 0 ? item.ecfGid : item.Queuegid) + "&" + new Date().getTime();
        }
        if (requeststatus == "inprocess") {
            _tmpUrl = EOWUrlDet + "DashBoard/Viewdata?id=" + (item.Queuegid == 0 ? item.ecfGid : item.Queuegid) + "&" + new Date().getTime();
        }

        if (requeststatus == "approved") {
            _tmpUrl = EOWUrlDet + "DashBoard/Viewdata?id=" + (item.Queuegid == 0 ? item.ecfGid : item.Queuegid) + "&" + new Date().getTime();
        }
        if (requeststatus == "hold") {
            $("#hfECFHoldData").val(item.Queuegid);
            $('#ShowHoldECFDig').attr("style", "display:block;");
            objHoldECFDialogy.dialog({ title: 'Hold Release Entry' });
            objHoldECFDialogy.dialog("open");
            return false;
        }
        if (_tmpUrl == "") return false;
        var win = window.open(_tmpUrl, '_self');
        if (win) {
            win.focus();
        }
        else {
            //Browser has blocked it
            jAlert("Please allow popups for this website", "Message");
        }
    }


    self.MyReqEOWDatatableCall = function () {
        if ($("#gvMyReqEOW > tbody > tr").length == self.MyReqEOWArray().length) {
            $("#gvMyReqEOW").DataTable({
                "autoWidth": false,
                "destroy": true,
                "scrollX": true,
                "iDisplayLength": 5,
                "bLengthChange": false,
                "aoColumnDefs": [{
                    "aTargets": ["nosort"],
                    "bSortable": false
                }, {
                    "aTargets": ["colDate"],
                    "bSortable": true,
                    "sType": "date-uk"
                }
                ]
            });
        }
    };
    self.loadMyReqEOWGrid = function () {
        $("#gvMyReqEOW").DataTable({
            "autoWidth": false,
            "bLengthChange": false,
            "destroy": true
        }).destroy();
        self.MyReqEOWArray([]);
    }
    self.loadMyReqEOWGrid();

    //Load For My Approval
    self.LoadApprovalTab = function () {
        showProgress();
        setTimeout(function () {
            self.IsApprovalSummary(true);
            self.loadMyAppASMSGrid();
            self.loadgvMyAppEPROCUREGrid();
            self.loadMyAppEOWGrid();
            self.IsDashbaord(true);
            self.IsVendorSummary(false);
            self.IsEProcure(false);
            self.IsEclaims(false);
            self.VendorTitle("");
            self.IsVendorDetail(false);
            self.IsEProcureDetail(false);
            self.IsEclaimsDetail(false);
            self.IsRecordIndex(0);
            $.ajax({
                type: "post",
                url: UrlDet + "GetMyApprovalTabDetails",
                data: '{}',
                contentType: "application/json;",
                success: function (response) {
                    hideProgress();
                    var Data1 = "", Data2 = "", Data3 = "";
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                        Data1 = JSON.parse(response.Data1);
                    }
                    self.MyAppASMSArray(Data1);
                    if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                        Data2 = JSON.parse(response.Data2);
                    }
                    self.MyAppEProcureArray(Data2);
                    if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null) {
                        Data3 = JSON.parse(response.Data3);
                    }
                    self.MyAppEOWArray(Data3);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //alert(errorThrown);
                    hideProgress();
                }
            });
        }, 300);
    };

    //ASMS
    self.ViewMyAppASMS = function () {
        var item = [];
        item.push({ 'Supplierheadergid': self.Suppliergid() });
        self.ASMSURL(item[0]);
    };
    self.ProcessASMSDetail = function (item) {
        var _reqType = item.supplierheaderrequesttype;
        if (_reqType == "MODIFICATION") {
            return false;
        }
        self.IsApprovalClick(true);
        self.IsRecordIndex(0);
        self.IsVendorACTIVATION(false);
        self.IsVendorDEACTIVATION(false);
        self.IsVendorONBOARDING(false);
        self.IsVendorRENEWAL(false);
        self.VendorTitle(_reqType);
        if (_reqType == "ACTIVATION") { self.IsVendorACTIVATION(true); }
        if (_reqType == "DEACTIVATION") { self.IsVendorDEACTIVATION(true); }
        if (_reqType == "ONBOARDING") { self.IsVendorONBOARDING(true); }
        if (_reqType == "RENEWAL") { self.IsVendorRENEWAL(true); }

        self.RequestType(item.supplierheaderrequesttype);
        self.RequestStatus(item.supplierheaderrequeststatus);

        self.Suppliergid(0);
        $("#txtASMSSupplierDetails,#txtASMSRaiserDetails,#txtASMSPrevContractStrDate,#txtASMSPrevContractEndDate,#txtASMSContractStrDate,#txtASMSContractEndDate,#txtASMSActivationFrom,#txtASMSActivationTo,#txtASMSApproxContractValue,#txtASMSGSTINProvided,#txtASMSActivationReason,#txtASMSDeActivationReason,#txtASMSRaiserReason,#txtASMSPreviousApproverDetails,#txtASMSPreviousApproverReason,#txtASMSCheckerRemark").val("");
        $("#txtASMSCheckerRemark").removeClass("valid").removeClass("required").addClass("required");
        checkASMSValidationUpdate();
        showProgress();
        setTimeout(function () {
            var _Filter = {
                Suppliergid: item.Supplierheadergid,
                Action: _reqType
            };
            $.ajax({
                type: "post",
                url: UrlDet + "GetASMSSupplierHeaderDetails",
                data: JSON.stringify(_Filter),
                contentType: "application/json;",
                success: function (response) {
                    hideProgress();
                    var Data1 = "";
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                        Data1 = JSON.parse(response.Data1);

                        if (Data1.length > 0) {
                            self.Suppliergid(Data1[0].Suppliergid);
                            $("#txtASMSSupplierDetails").val(Data1[0].Supplier);
                            $("#txtASMSRaiserDetails").val(Data1[0].Raiser);
                            $("#txtASMSContractStrDate").val(Data1[0].ContractFrom);
                            $("#txtASMSContractEndDate").val(Data1[0].ContractTo);
                            $("#txtASMSRaiserReason").val(Data1[0].RaiserComments);
                            $("#txtASMSPreviousApproverDetails").val(Data1[0].PreviousApprover);
                            $("#txtASMSPreviousApproverReason").val(Data1[0].PreviousApproverRemarks);

                            if (_reqType == "ACTIVATION") {
                                $("#txtASMSActivationFrom").val(Data1[0].ActivationFrom);
                                $("#txtASMSActivationTo").val(Data1[0].ActivationTo);
                                $("#txtASMSActivationReason").val(Data1[0].ActivateReason);
                            }
                            if (_reqType == "DEACTIVATION") {
                                $("#txtASMSDeActivationReason").val(Data1[0].DeActivateReason);
                            }
                            if (_reqType == "ONBOARDING") {
                                $("#txtASMSApproxContractValue").val(Data1[0].ApproxContractValue == null ? "" : Data1[0].ApproxContractValue);
                                $("#txtASMSGSTINProvided").val(Data1[0].GSTINProvided);
                            }
                            if (_reqType == "RENEWAL") {
                                $("#txtASMSApproxContractValue").val(Data1[0].ApproxContractValue == null ? "" : Data1[0].ApproxContractValue);
                                $("#txtASMSGSTINProvided").val(Data1[0].GSTINProvided);
                                $("#txtASMSPrevContractStrDate").val(Data1[0].PreviousContractFrom);
                                $("#txtASMSPrevContractEndDate").val(Data1[0].PreviousContractTo);
                            }
                            self.IsApprovalSummary(false);
                            self.IsVendorDetail(true);
                        }
                    } else {
                        //show error detail
                        jAlert("In-valid Venor Details!", "Error");
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    hideProgress();
                }
            });
        }, 400);
    };
    self.MyAppASMSDatatableCall = function () {
        if ($("#gvMyAppASMS > tbody > tr").length == self.MyAppASMSArray().length) {
            $("#gvMyAppASMS").DataTable({
                "autoWidth": false,
                "destroy": true,
                "scrollX": true,
                "iDisplayLength": 5,
                "bLengthChange": false,
                "aoColumnDefs": [{
                    "aTargets": ["nosort"],
                    "bSortable": false
                }, {
                    "aTargets": ["colDate"],
                    "bSortable": true,
                    "sType": "date-uk"
                }
                // , { "targets": [1], "visible": false }
                ]
            });
        }
    };
    self.loadMyAppASMSGrid = function () {
        $("#gvMyAppASMS").DataTable({
            "autoWidth": false,
            "bLengthChange": false,
            "destroy": true
        }).destroy();
        self.MyAppASMSArray([]);
    }
    self.loadMyAppASMSGrid();

    //EPROCURE
    self.ViewMyAppEPROCURE = function () {
        var item = [];
        item.push({ 'GId': self.EProcureID() });
        self.EPROCUREURL(item[0]);
    };
    self.ProcessEprocureDetail = function (item) {
        self.IsRecordIndex(0);
        self.IsApprovalClick(true);
        showProgress();
        setTimeout(function () {
            self.RequestType(item.RequestType);
            self.RequestStatus(item.RequestStatus);

            self.IsPARDetails(false); self.IsCBFDetails(false); self.IsPRDetails(false); self.IsPODetails(false); self.IsWODetails(false);
            if (self.RequestType() == "PAR") { self.IsPARDetails(true); }
            if (self.RequestType() == "CBF") { self.IsCBFDetails(true); }
            if (self.RequestType() == "PR") { self.IsPRDetails(true); }
            if (self.RequestType() == "PO") { self.IsPODetails(true); self.loadPOGrid(); }
            if (self.RequestType() == "WO") { self.IsWODetails(true); self.loadWOGrid(); }

            var _Filter = {
                Refgid: item.GId,
                Action: item.RequestType
            };

            $.ajax({
                type: "post",
                url: UrlDet + "GetFBDetails",
                data: JSON.stringify(_Filter),
                contentType: "application/json;",
                success: function (response) {
                    hideProgress();
                    var Data1 = "", Data2 = "", Data3 = "";
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                        Data1 = JSON.parse(response.Data1);
                        if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                            Data2 = JSON.parse(response.Data2);
                        }
                        if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null) {
                            Data3 = JSON.parse(response.Data3);
                        }
                        if (Data1.length > 0) {
                            if (self.RequestType() == "PAR") {
                                self.EProcureID(Data1[0].Refgid);
                                self.FBArray1(Data2);
                                self.FBArray2(Data3);
                                //Refgid  
                                $("#txtPARNo").val(Data1[0].RefNo);
                                $("#txtPARDate").val(Data1[0].RefDate);
                                $("#txtPARRaiserDetails").val(Data1[0].Raiser);
                                $("#txtBudgeted").val(Data1[0].IsBudgeted);
                                $("#txtPARAmount").val(Data1[0].Amount);
                                $("#txtPARRaiserReason").val(Data1[0].RaiserComments);

                                $("#txtPARCheckerRemark").val("");
                                $("#txtPARCheckerRemark").removeClass("valid").removeClass("required").addClass("required");
                                checkPARValidationUpdate();
                            }
                            if (self.RequestType() == "CBF") {
                                self.EProcureID(Data1[0].Refgid);
                                self.CBFArray(Data2);
                                //Refgid  
                                $("#txtCBFNo").val(Data1[0].RefNo);
                                $("#txtCBFDate").val(Data1[0].RefDate);
                                $("#txtCBFRaiserDetails").val(Data1[0].Raiser);
                                $("#txtCBFRequestFor").val(Data1[0].RequestFor);
                                $("#txtCBFAmount").val(Data1[0].Amount);
                                $("#txtCBFMode").val(Data1[0].Mode);
                                $("#txtCBFPARPRNo").val(Data1[0].PARPRRefNo);
                                $("#txtCBFPARPRAmt").val(Data1[0].PARPRAmount);
                                $("#txtCBFApproval").val(Data1[0].Approvaltype);
                                $("#txtCBFBudgeted").val(Data1[0].IsBudgeted);
                                $("#txtCBFBranch").val(Data1[0].BranchName);

                                $("#txtCBFPreviousApproverDetails").val(Data1[0].PreviousApprover);
                                $("#txtCBFPreviousApproverReason").val(Data1[0].PreviousApproverRemarks);
                                $("#txtCBFJustification").val(Data1[0].Justification);

                                $("#txtCBFCheckerRemark").val("");
                                $("#txtCBFCheckerRemark").removeClass("valid").removeClass("required").addClass("required");
                                checkCBFValidationUpdate();
                            }

                            if (self.RequestType() == "PR") {
                                self.EProcureID(Data1[0].Refgid);
                                self.PRArray(Data2);
                                //Refgid
                                $("#txtPRNo").val(Data1[0].RefNo);
                                $("#txtPRDate").val(Data1[0].RefDate);
                                $("#txtPRRaiserDetails").val(Data1[0].Raiser);
                                $("#txtPRBudgeted").val(Data1[0].IsBudgeted);
                                $("#txtPRAmount").val(Data1[0].Amount);
                                $("#txtPRRequestFor").val(Data1[0].RequestFor);
                                $("#txtPRMode").val(Data1[0].ExpenseType);
                                $("#txtPRBSCC").val(Data1[0].BSCC);
                                $("#txtPRBranch").val(Data1[0].BranchName);

                                $("#txtPRPreviousApproverDetails").val(Data1[0].PreviousApprover);
                                $("#txtPRPreviousApproverReason").val(Data1[0].PreviousApproverRemarks);
                                $("#txtPRRaiserComments").val(Data1[0].RaiserComments);

                                $("#txtPRCheckerRemark").val("");
                                $("#txtPRCheckerRemark").removeClass("valid").removeClass("required").addClass("required");
                                checkPRValidationUpdate();
                            }

                            if (self.RequestType() == "PO") {
                                self.EProcureID(Data1[0].Refgid);
                                self.POArray(Data2);
                                //Refgid
                                $("#txtPONo").val(Data1[0].RefNo);
                                $("#txtPODate").val(Data1[0].RefDate);
                                $("#txtPORaiserDetails").val(Data1[0].Raiser);
                                $("#txtPOSupplierDetails").val(Data1[0].Supplier);
                                $("#txtPOCBFNo").val(Data1[0].CBFNo);
                                $("#txtPOCBFLineAmount").val(Data1[0].CBFLineAmount);
                                $("#txtPOMode").val(Data1[0].Mode);
                                $("#txtPOPARPRNo").val(Data1[0].PARPRRefNo);
                                $("#txtPOBranchSingle").val(Data1[0].BranchSingle);
                                $("#txtPOBranch").val(Data1[0].BranchName);
                                $("#txtPOCC").val(Data1[0].CC);
                                $("#txtPOAmt").val(Data1[0].Amount);

                                $("#txtPOPreviousApproverDetails").val(Data1[0].PreviousApprover);
                                $("#txtPOPreviousApproverReason").val(Data1[0].PreviousApproverRemarks);

                                $("#txtPOCheckerRemark").val("");
                                $("#txtPOCheckerRemark").removeClass("valid").removeClass("required").addClass("required");
                                checkPOValidationUpdate();
                            }

                            if (self.RequestType() == "WO") {
                                self.EProcureID(Data1[0].Refgid);
                                self.WOArray(Data2);
                                //Refgid
                                $("#txtWONo").val(Data1[0].RefNo);
                                $("#txtWODate").val(Data1[0].RefDate);
                                $("#txtWORaiserDetails").val(Data1[0].Raiser);
                                $("#txtWOSupplierDetails").val(Data1[0].Supplier);
                                $("#txtWOOBFNo").val(Data1[0].CBFNo);
                                $("#txtWOOBFLineAmount").val(Data1[0].CBFLineAmount);
                                $("#txtWOMode").val(Data1[0].Mode);
                                $("#txtWOPARPRNo").val(Data1[0].PARPRRefNo);
                                $("#txtWOBranchSingle").val(Data1[0].BranchSingle);
                                $("#txtWOBranch").val(Data1[0].BranchName);
                                $("#txtWOCC").val(Data1[0].CC);
                                $("#txtWOAmt").val(Data1[0].Amount);

                                $("#txtWOPreviousApproverDetails").val(Data1[0].PreviousApprover);
                                $("#txtWOPreviousApproverReason").val(Data1[0].PreviousApproverRemarks);

                                $("#txtWOCheckerRemark").val("");
                                $("#txtWOCheckerRemark").removeClass("valid").removeClass("required").addClass("required");
                                checkWOValidationUpdate();
                            }
                            self.IsApprovalSummary(false);
                            self.IsEProcureDetail(true);
                        }
                    } else {
                        //show error detail
                        jAlert("In-valid Venor Details!", "Error");
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    hideProgress();
                }
            });
        }, 400);
    };
    self.MyAppEPROCUREDatatableCall = function () {
        if ($("#gvMyAppEPROCURE > tbody > tr").length == self.MyAppEProcureArray().length) {
            $("#gvMyAppEPROCURE").DataTable({
                "autoWidth": false,
                "destroy": true,
                "scrollX": true,
                "iDisplayLength": 5,
                "bLengthChange": false,
                "aoColumnDefs": [{
                    "aTargets": ["nosort"],
                    "bSortable": false
                }, {
                    "aTargets": ["colDate"],
                    "bSortable": true,
                    "sType": "date-uk"
                }
                 //, { "targets": [1], "visible": false }
                ]
            });
        }
    };
    self.loadgvMyAppEPROCUREGrid = function () {
        $("#gvMyAppEPROCURE").DataTable({
            "autoWidth": false,
            "bLengthChange": false,
            "destroy": true
        }).destroy();
        self.MyAppEProcureArray([]);
    }
    self.loadgvMyAppEPROCUREGrid();

    //EOW
    self.ViewMyAppECF = function () {
        var item = [];
        item.push({ 'ecfgid': self.EClaimID(), 'Queuegid': self.EClaimQueueGID() });
        self.ECLAIMURL(item[0]);
    };
    self.ProcessECLAIMDetail = function (item) {
        self.IsRecordIndex(0);
        self.IsApprovalClick(true);
        self.SI_NON_PO(false);
        self.SIDSA(false);
        self.EclaimLC(false);
        self.EclaimTravalType(false);
        self.EclaimARFEP(false);
        self.EclaimARFF(false);

        if (item.ecfdocsubtypegid == "1" || item.ecfdocsubtypegid == "3") { self.EclaimTravalType(true); }
        else if (item.ecfdocsubtypegid == "2") { self.EclaimLC(true); }
        else if (item.ecfdocsubtypegid == "5") { self.SIDSA(true); }
        else if (item.ecfdocsubtypegid == "4") { self.SI_NON_PO(true); }
        else if (item.ecfdocsubtypegid == "6" || item.ecfdocsubtypegid == "8") { self.EclaimARFEP(true); }
        else if (item.ecfdocsubtypegid == "7") { self.EclaimARFF(true); }
        self.DocumentSubType(item.ecfdocsubtypegid);
        self.DocumentTypeId(item.ecfdoctypegid);
        self.EClaimQueueGID(item.Queuegid);
        self.RequestType(item.DocType);
        self.RequestStatus(item.DocStatus);

        showProgress();
        setTimeout(function () {
            var _Filter = {
                ECFId: item.ecfGid,
                DocSubtype: item.ecfdocsubtypegid
            };
            self.loadEClaimPOGrid();
            self.TravalArray.removeAll();
            $.ajax({
                type: "post",
                url: UrlDet + "GetEClaimsDetails",
                data: JSON.stringify(_Filter),
                contentType: "application/json;",
                success: function (response) {
                    hideProgress();
                    var Data1 = "", Data2 = "";
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                        Data1 = JSON.parse(response.Data1);
                        if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                            Data2 = JSON.parse(response.Data2);
                        }

                        if (Data1.length > 0) {
                            if (self.DocumentSubType() == "1" || self.DocumentSubType() == "3") {
                                self.TravalArray(Data2);
                                self.EClaimID(Data1[0].ECFId);
                                //ECFId  
                                $("#txtEClaimTravalRaiserDetails").val(Data1[0].Raiser);
                                $("#txtEClaimTravalECFNo").val(Data1[0].ECFNo);
                                $("#txtEClaimTravalECFAmount").val(Data1[0].ECFAmount);
                                $("#txtEClaimTravalPeriodFrom").val(Data1[0].PeriodFrom);
                                $("#txtEClaimTravalPeriodTo").val(Data1[0].PeriodTo);
                                $("#txtEClaimTravalServiceMonth").val(Data1[0].ServiceMonth);

                                $("#txtEClaimTravalBranch").val(Data1[0].BranchName);
                                $("#txtEClaimTravalRaiserComments").val(Data1[0].RaiserComments);
                                $("#txtEClaimTravalPreviousApproverDetails").val(Data1[0].PreviousApprover);
                                $("#txtEClaimTravalPreviousApproverReason").val(Data1[0].PreviousApproverRemarks);

                                $("#txtEClaimTravalCheckerRemark").val("");
                                $("#txtEClaimTravalCheckerRemark").removeClass("valid").removeClass("required").addClass("required");
                                checkEClaimTravalValidationUpdate();
                            }
                            else if (self.DocumentSubType() == "2") {
                                //ECFId
                                self.EClaimID(Data1[0].ECFId);
                                $("#txtEClaimLCRaiserDetails").val(Data1[0].Raiser);
                                $("#txtEClaimLCECFNo").val(Data1[0].ECFNo);
                                $("#txtEClaimLCECFAmount").val(Data1[0].ECFAmount);
                                $("#txtEClaimLCNoOfEmployee").val(Data1[0].NoOfPersons);
                                $("#txtEClaimLCServiceMonth").val(Data1[0].ServiceMonth);
                                $("#txtEClaimLCHighestClaimAmt").val(Data1[0].HighestClaimAmount);
                                $("#txtEClaimLCBranch").val(Data1[0].BranchName);
                                $("#txtEClaimLCRaiserComments").val(Data1[0].RaiserComments);
                                $("#txtEClaimLCPreviousApproverDetails").val(Data1[0].PreviousApprover);
                                $("#txtEClaimLCPreviousApproverReason").val(Data1[0].PreviousApproverRemarks);


                                $("#txtEClaimLCCheckerRemark").val("");
                                $("#txtEClaimLCCheckerRemark").removeClass("valid").removeClass("required").addClass("required");
                                checkEClaimLCValidationUpdate();
                            }

                            else if (self.DocumentSubType() == "5") {
                                //ECFId
                                self.EClaimID(Data1[0].ECFId);
                                $("#txtEClaimDSASupplierDetails").val(Data1[0].Supplier);
                                $("#txtEClaimDSARaiserDetails").val(Data1[0].Raiser);
                                $("#txtEClaimDSAECFNo").val(Data1[0].ECFNo);
                                $("#txtEClaimDSAECFAmount").val(Data1[0].ECFAmount);
                                $("#txtEClaimDSANoOfVendor").val(Data1[0].NoOfPersons);
                                $("#txtEClaimDSAServiceMonth").val(Data1[0].ServiceMonth);
                                $("#txtEClaimDSARaiserComments").val(Data1[0].RaiserComments);
                                $("#txtEClaimDSAPreviousApproverDetails").val(Data1[0].PreviousApprover);
                                $("#txtEClaimDSAPreviousApproverReason").val(Data1[0].PreviousApproverRemarks);

                                $("#txtEClaimDSACheckerRemark").val("");
                                $("#txtEClaimDSACheckerRemark").removeClass("valid").removeClass("required").addClass("required");
                                checkEClaimDSAValidationUpdate();
                            }

                            else if (self.DocumentSubType() == "4") {
                                self.POType(Data1[0].POType);

                                //ECFId
                                self.EClaimID(Data1[0].ECFId);
                                $("#txtEClaimPOECFNo").val(Data1[0].ECFNo);
                                $("#txtEClaimPOECFAmount").val(Data1[0].ECFAmount);
                                $("#txtEClaimPORaiserDetails").val(Data1[0].Raiser);
                                $("#txtEClaimPOSupplierDetails").val(Data1[0].Supplier);

                                $("#txtEClaimPORaiserComments").val(Data1[0].RaiserComments);
                                $("#txtEClaimPOPreviousApproverDetails").val(Data1[0].PreviousApprover);
                                $("#txtEClaimPOPreviousApproverReason").val(Data1[0].PreviousApproverRemarks);

                                $("#txtEClaimPOCheckerRemark").val("");
                                $("#txtEClaimPOCheckerRemark").removeClass("valid").removeClass("required").addClass("required");

                                checkEClaimPOValidationUpdate();
                                self.EClaimPOArray(Data2);
                            }
                            else if (self.DocumentSubType() == "6" || self.DocumentSubType() == "8") {
                                self.EClaimID(Data1[0].ARFId);
                                //ARFId
                                $("#txtEClaimARFEPRaiserDetails").val(Data1[0].Raiser);
                                $("#txtEClaimARFEPECFNo").val(Data1[0].ARFNo);
                                $("#txtEClaimARFEPECFAmount").val(Data1[0].ARFAmount);
                                $("#txtEClaimARFEPARFType").val(Data1[0].ARFType);
                                $("#txtEClaimARFEPAdvanceType").val(Data1[0].Advancetype);
                                $("#txtEClaimARFEPLiqDate").val(Data1[0].TargetLiqDate);
                                $("#txtEClaimARFEPBranch").val(Data1[0].BranchName);
                                $("#txtEClaimARFEPDescription").val(Data1[0].ARFDescription);
                                $("#txtEClaimARFEPRaiserComments").val(Data1[0].RaiserComments);
                                $("#txtEClaimARFEPPreviousApproverDetails").val(Data1[0].PreviousApprover);
                                $("#txtEClaimARFEPPreviousApproverReason").val(Data1[0].PreviousApproverRemarks);

                                $("#txtEClaimARFEPCheckerRemark").val("");
                                $("#txtEClaimARFEPCheckerRemark").removeClass("valid").removeClass("required").addClass("required");
                                checkEClaimARFEPValidationUpdate();
                            }
                            else if (self.DocumentSubType() == "7") {
                                self.EClaimID(Data1[0].ARFId);
                                //ARFId
                                $("#txtEClaimARFFRaiserDetails").val(Data1[0].Raiser);
                                $("#txtEClaimARFFSupplierDetails").val(Data1[0].SupplierEmployeeName);
                                $("#txtEClaimARFFECFNo").val(Data1[0].ARFNo);
                                $("#txtEClaimARFFAmount").val(Data1[0].ARFAmount);
                                $("#txtEClaimARFFARFType").val(Data1[0].ARFType);
                                $("#txtEClaimARFFAdvanceType").val(Data1[0].Advancetype);
                                $("#txtEClaimARFFLiqDate").val(Data1[0].TargetLiqDate);
                                $("#txtEClaimARFFBranch").val(Data1[0].BranchName);

                                $("#txtEClaimARFFProformaInv").val(Data1[0].PromoInvoice);
                                $("#txtEClaimARFFPO").val(Data1[0].PONo);

                                $("#txtEClaimARFFDescription").val(Data1[0].ARFDescription);
                                $("#txtEClaimARFFRaiserComments").val(Data1[0].RaiserComments);
                                $("#txtEClaimARFFPreviousApproverDetails").val(Data1[0].PreviousApprover);
                                $("#txtEClaimARFFPreviousApproverReason").val(Data1[0].PreviousApproverRemarks);

                                $("#txtEClaimARFFCheckerRemark").val("");
                                $("#txtEClaimARFFCheckerRemark").removeClass("valid").removeClass("required").addClass("required");
                                checkEClaimARFFValidationUpdate();
                            }
                            self.IsApprovalSummary(false);
                            self.IsEclaimsDetail(true);
                        }
                    } else {
                        //show error detail
                        self.DocumentTypeId(0);
                        jAlert("In-valid ECF Details!", "Error");
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    hideProgress();
                }
            });
        }, 400);
    };
    self.MyAppEOWDatatableCall = function () {
        if ($("#gvMyAppEOW > tbody > tr").length == self.MyAppEOWArray().length) {
            $("#gvMyAppEOW").DataTable({
                "autoWidth": false,
                "destroy": true,
                "scrollX": true,
                "iDisplayLength": 5,
                "bLengthChange": false,
                "aoColumnDefs": [{
                    "aTargets": ["nosort"],
                    "bSortable": false
                }, {
                    "aTargets": ["colDate"],
                    "bSortable": true,
                    "sType": "date-uk"
                }
                 //, { "targets": [1], "visible": false }
                ]
            });
        }
    };
    self.loadMyAppEOWGrid = function () {
        $("#gvMyAppEOW").DataTable({
            "autoWidth": false,
            "bLengthChange": false,
            "destroy": true
        }).destroy();
        self.MyAppEOWArray([]);
    }
    self.loadMyAppEOWGrid();

    //Load Hold Data
    self.LoadHoldTab = function () {
        showProgress();
        setTimeout(function () {
            self.loadHoldGrid();
            self.IsDashbaord(true);
            self.IsVendorSummary(false);
            self.IsEProcure(false);
            self.IsEclaims(false);
            self.IsVendorDetail(false);
            self.IsEProcureDetail(false);
            self.IsEclaimsDetail(false);
            self.IsRecordIndex(0);
            $.ajax({
                type: "post",
                url: UrlDet + "GetHoldTabDetails",
                data: '{}',
                contentType: "application/json;",
                success: function (response) {
                    hideProgress();
                    var Data1 = "";
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                        Data1 = JSON.parse(response.Data1);
                    }
                    self.HoldArray(Data1);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //alert(errorThrown);
                    hideProgress();
                }
            });
        }, 300);
    };
    self.ViewHoldECF = function (item) {
        $("#hfECFHoldData").val("0");
        $('#btnECFHold').attr('disabled', true);
        $("#txtECFHoldRemarks").val("");
        $("#txtECFHoldRemarks").removeClass('required').removeClass('valid').addClass('required');
        $("#hfECFHoldData").val(item.ecfhold_queue_gid);
        if ($("#hfECFHoldData").val() == "" || $("#hfECFHoldData").val() == "0" || $("#hfECFHoldData").val() == null) {
            jAlert("In-valid ECF Queue Details!", "Error");
            return false;
        }

        $('#ShowHoldECFDig').attr("style", "display:block;");

        objHoldECFDialogy.dialog({ title: 'Hold Release Entry' });
        objHoldECFDialogy.dialog("open");
        return false;
    };
    self.ReleaseHold = function () {
        var _Filter = {
            ecfgid: $("#hfECFHoldData").val(),
            remarks: $("#txtECFHoldRemarks").val()
        };

        $.ajax({
            type: "post",
            url: UrlDet + "UpdateHoldData",
            data: JSON.stringify(_Filter),
            contentType: "application/json;",
            success: function (response) {
                hideProgress();
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    if (Data1[0].Clear == true) {
                        jAlert(Data1[0].Message, "Success", function () {
                            $("#hfECFHoldData").val("0");
                            objHoldECFDialogy.dialog("close");
                            self.LoadHoldTab();
                            location = ('../Dashboard/Index');
                        });
                    }
                    else {
                        jAlert(Data1[0].Message, "Message");
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
                hideProgress();
            }
        });
    };
    self.HoldDatatableCall = function () {
        if ($("#gvHold > tbody > tr").length == self.HoldArray().length) {
            $("#gvHold").DataTable({
                "autoWidth": false,
                "destroy": true,
                "scrollX": true,
                "bLengthChange": false,
                "aoColumnDefs": [{
                    "aTargets": ["nosort"],
                    "bSortable": false
                }, {
                    "aTargets": ["colDate"],
                    "bSortable": true,
                    "sType": "date-uk"
                }
                ]
            });
        }
    };
    self.loadHoldGrid = function () {
        $("#gvHold").DataTable({
            "autoWidth": false,
            "bLengthChange": false,
            "destroy": true
        }).destroy();
        self.HoldArray([]);
    }
    self.loadHoldGrid();

    self.CloseDetails = function () {
        objHoldECFDialogy.dialog("close");
    };
    self.TestSavePDF = function (item) {
        /*
        jConfirm("Want to load Next Record?", "Message", function (callback) {
            if (callback == true) {
                var item = self.IndexChecker("1");
                if (item == null) {
                    self.IsRecordIndex(0);
                    self.LoadDashboard();
                } else {
                    self.VMProcess(item);
                }
            } else if (callback == false) {
                self.clickASMSSummaryLink();
            }
        });
        $("#popup_ok").attr("style", "margin-left: 0px;");
        $("#popup_cancel").attr("style", "margin-left: 5px;");
        */
        $.ajax({
            type: "post",
            url: UrlDet + "TestSavePDF",
            data: '{}',
            contentType: "application/json;",
            success: function (response) {
                hideProgress();
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1.length > 0) {
                    }
                } else {
                    //show error detail
                    jAlert("In-valid Venor Details!", "Error");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                hideProgress();
            }
        });
    };
};
var _DashboardViewModel = new DashboardModel();
ko.applyBindings(_DashboardViewModel);

$(document).ready(function () {
    jQuery.extend(jQuery.fn.dataTableExt.oSort, {
        "date-uk-pre": function (a) {
            a = a.split(">")[1].split("<")[0];
            var ukDatea = "";
            if (a.indexOf('/') == -1) {
                // will not be triggered because str has _..
                ukDatea = a.split("-");
            } else {
                ukDatea = a.split("/");
            }
            return (ukDatea[2] + ukDatea[1] + ukDatea[0]) * 1;
        },
        "date-uk-asc": function (a, b) {
            return ((a < b) ? -1 : ((a > b) ? 1 : 0));
        },
        "date-uk-desc": function (a, b) {
            return ((a < b) ? 1 : ((a > b) ? -1 : 0));
        }
    });

    objHoldECFDialogy = $("[id$='ShowHoldECFDig']");
    objHoldECFDialogy.dialog({
        autoOpen: false,
        modal: true,
        duration: 300,
        width: 600,
        height: 250
    });

    //Search Concurrent Approval
    $(".searchConcurrentApproval").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfSConcurrentApprovalId").val("0");
        }

        $(".searchConcurrentApproval").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: MUrlHelper + "GetAutoReceiptEmployee",
                    data: "{ 'txt' : '" + request.term + "'}",
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
                $("#hdfSConcurrentApprovalId").val(i.item.val);
                $(this).val(i.item.label);
            }
        });
    });

    /*$(".searchConcurrentApproval").keydown(function () {
        checkEClaimTravalValidationUpdate();
        checkEClaimLCValidationUpdate();
        checkEClaimDSAValidationUpdate();
        checkEClaimPOValidationUpdate();
        checkEClaimARFEPValidationUpdate();
        checkEClaimARFFValidationUpdate();
    });*/

    $(".searchConcurrentApproval").focusout(function () {
        var hdfId = $("#hdfSConcurrentApprovalId").val();
        var txtCurName = $(this).val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $(this).val("");
        }

        checkEClaimTravalValidationUpdate();
        checkEClaimLCValidationUpdate();
        checkEClaimDSAValidationUpdate();
        checkEClaimPOValidationUpdate();
        checkEClaimARFEPValidationUpdate();
        checkEClaimARFFValidationUpdate();
    });

    $(".eclaimCheckBox").change(function () {
        if (this.checked) {
            //Do stuff
            $(".DivconcurrentApproval").css("display", "block");
            $(".searchConcurrentApproval").val("");
            $("#hdfSConcurrentApprovalId").val("0");

        } else {
            $(".DivconcurrentApproval").css("display", "none");
            $(".searchConcurrentApproval").val("");
            $("#hdfSConcurrentApprovalId").val("0");
        }

        checkEClaimTravalValidationUpdate();
        checkEClaimLCValidationUpdate();
        checkEClaimDSAValidationUpdate();
        checkEClaimPOValidationUpdate();
        checkEClaimARFEPValidationUpdate();
        checkEClaimARFFValidationUpdate();
    });

    $("#txtASMSCheckerRemark").keyup(function () {
        checkASMSValidationUpdate();

        var _data = $("#txtASMSCheckerRemark").val();
        $("#txtASMSCheckerRemark").removeClass('required').removeClass('valid');
        if (_data.trim().length > 1) {
            $("#txtASMSCheckerRemark").addClass('valid');
        }
        else {
            $("#txtASMSCheckerRemark").addClass('required');
        }
    });

    $("#txtPARCheckerRemark").keyup(function () {
        checkPARValidationUpdate();

        var _data = $("#txtPARCheckerRemark").val();
        $("#txtPARCheckerRemark").removeClass('required').removeClass('valid');
        if (_data.trim().length > 1) {
            $("#txtPARCheckerRemark").addClass('valid');
        }
        else {
            $("#txtPARCheckerRemark").addClass('required');
        }
    });

    $("#txtCBFCheckerRemark").keyup(function () {
        checkCBFValidationUpdate();

        var _data = $("#txtCBFCheckerRemark").val();
        $("#txtCBFCheckerRemark").removeClass('required').removeClass('valid');
        if (_data.trim().length > 1) {
            $("#txtCBFCheckerRemark").addClass('valid');
        }
        else {
            $("#txtCBFCheckerRemark").addClass('required');
        }
    });

    $("#txtPRCheckerRemark").keyup(function () {
        checkPRValidationUpdate();

        var _data = $("#txtPRCheckerRemark").val();
        $("#txtPRCheckerRemark").removeClass('required').removeClass('valid');
        if (_data.trim().length > 1) {
            $("#txtPRCheckerRemark").addClass('valid');
        }
        else {
            $("#txtPRCheckerRemark").addClass('required');
        }
    });

    $("#txtPOCheckerRemark").keyup(function () {
        checkPOValidationUpdate();

        var _data = $("#txtPOCheckerRemark").val();
        $("#txtPOCheckerRemark").removeClass('required').removeClass('valid');
        if (_data.trim().length > 1) {
            $("#txtPOCheckerRemark").addClass('valid');
        }
        else {
            $("#txtPOCheckerRemark").addClass('required');
        }
    });

    $("#txtWOCheckerRemark").keyup(function () {
        checkWOValidationUpdate();

        var _data = $("#txtWOCheckerRemark").val();
        $("#txtWOCheckerRemark").removeClass('required').removeClass('valid');
        if (_data.trim().length > 1) {
            $("#txtWOCheckerRemark").addClass('valid');
        }
        else {
            $("#txtWOCheckerRemark").addClass('required');
        }
    });

    //EOW
    $("#txtEClaimTravalCheckerRemark").keyup(function () {
        checkEClaimTravalValidationUpdate();

        var _data = $("#txtEClaimTravalCheckerRemark").val();
        $("#txtEClaimTravalCheckerRemark").removeClass('required').removeClass('valid');
        if (_data.trim().length > 1) {
            $("#txtEClaimTravalCheckerRemark").addClass('valid');
        }
        else {
            $("#txtEClaimTravalCheckerRemark").addClass('required');
        }
    });

    $("#txtEClaimLCCheckerRemark").keyup(function () {
        checkEClaimLCValidationUpdate();

        var _data = $("#txtEClaimLCCheckerRemark").val();
        $("#txtEClaimLCCheckerRemark").removeClass('required').removeClass('valid');
        if (_data.trim().length > 1) {
            $("#txtEClaimLCCheckerRemark").addClass('valid');
        }
        else {
            $("#txtEClaimLCCheckerRemark").addClass('required');
        }
    });

    $("#txtEClaimDSACheckerRemark").keyup(function () {
        checkEClaimDSAValidationUpdate();

        var _data = $("#txtEClaimDSACheckerRemark").val();
        $("#txtEClaimDSACheckerRemark").removeClass('required').removeClass('valid');
        if (_data.trim().length > 1) {
            $("#txtEClaimDSACheckerRemark").addClass('valid');
        }
        else {
            $("#txtEClaimDSACheckerRemark").addClass('required');
        }
    });

    $("#txtEClaimPOCheckerRemark").keyup(function () {
        checkEClaimPOValidationUpdate();

        var _data = $("#txtEClaimPOCheckerRemark").val();
        $("#txtEClaimPOCheckerRemark").removeClass('required').removeClass('valid');
        if (_data.trim().length > 1) {
            $("#txtEClaimPOCheckerRemark").addClass('valid');
        }
        else {
            $("#txtEClaimPOCheckerRemark").addClass('required');
        }
    });

    $("#txtEClaimARFEPCheckerRemark").keyup(function () {
        checkEClaimARFEPValidationUpdate();

        var _data = $("#txtEClaimARFEPCheckerRemark").val();
        $("#txtEClaimARFEPCheckerRemark").removeClass('required').removeClass('valid');
        if (_data.trim().length > 1) {
            $("#txtEClaimARFEPCheckerRemark").addClass('valid');
        }
        else {
            $("#txtEClaimARFEPCheckerRemark").addClass('required');
        }
    });

    $("#txtEClaimARFFCheckerRemark").keyup(function () {
        checkEClaimARFFValidationUpdate();

        var _data = $("#txtEClaimARFFCheckerRemark").val();
        $("#txtEClaimARFFCheckerRemark").removeClass('required').removeClass('valid');
        if (_data.trim().length > 1) {
            $("#txtEClaimARFFCheckerRemark").addClass('valid');
        }
        else {
            $("#txtEClaimARFFCheckerRemark").addClass('required');
        }
    });

    $("#txtECFHoldRemarks").keyup(function () {
        var _data = $("#txtECFHoldRemarks").val();
        $("#txtECFHoldRemarks").removeClass('required').removeClass('valid');
        if (_data.trim().length > 1) {
            $("#txtECFHoldRemarks").addClass('valid');
            $('#btnECFHold').attr('disabled', false);
        }
        else {
            $("#txtECFHoldRemarks").addClass('required');
            $('#btnECFHold').attr('disabled', true);
        }
    });
});

function checkASMSValidationUpdate() {
    var _data = $("#txtASMSCheckerRemark").val();
    if (_data.trim().length > 1) {
        $('#btnApprove,#btnReject').attr('disabled', false);
    }
    else {
        $('#btnApprove,#btnReject').attr('disabled', true);
    }
}

function checkPARValidationUpdate() {
    var _data = $("#txtPARCheckerRemark").val();
    if (_data.trim().length > 1) {
        $('#btnPARApprove,#btnPARReject').attr('disabled', false);
    }
    else {
        $('#btnPARApprove,#btnPARReject').attr('disabled', true);
    }
}

function checkCBFValidationUpdate() {
    var _data = $("#txtCBFCheckerRemark").val();
    if (_data.trim().length > 1) {
        $('#btnCBFApprove,#btnCBFReject').attr('disabled', false);
    }
    else {
        $('#btnCBFApprove,#btnCBFReject').attr('disabled', true);
    }
}

function checkPRValidationUpdate() {
    var _data = $("#txtPRCheckerRemark").val();
    if (_data.trim().length > 1) {
        $('#btnPRApprove,#btnPRReject').attr('disabled', false);
    }
    else {
        $('#btnPRApprove,#btnPRReject').attr('disabled', true);
    }
}

function checkPOValidationUpdate() {
    var _data = $("#txtPOCheckerRemark").val();
    if (_data.trim().length > 1) {
        $('#btnPOApprove,#btnPOReject').attr('disabled', false);
    }
    else {
        $('#btnPOApprove,#btnPOReject').attr('disabled', true);
    }
}

function checkWOValidationUpdate() {
    var _data = $("#txtWOCheckerRemark").val();
    if (_data.trim().length > 1) {
        $('#btnWOApprove,#btnWOReject').attr('disabled', false);
    }
    else {
        $('#btnWOApprove,#btnWOReject').attr('disabled', true);
    }
}

//EOW
function checkEClaimTravalValidationUpdate() {
    var _data = $("#txtEClaimTravalCheckerRemark").val();
    var _concurrentAppId = $("#hdfSConcurrentApprovalId").val();

    if (_data.trim().length > 1) {
        $('#btnEClaimTravalApprove,#btnEClaimTravalReject,#btnEClaimTravalHold,#btnEClaimTravalCApproval').attr('disabled', true);
        if ($('#chkEClaimTravalIsConcurrentApp').is(':checked')) {
            if (_concurrentAppId != null && _concurrentAppId != "" && _concurrentAppId != "0" && _concurrentAppId != undefined) {
                $('#btnEClaimTravalCApproval').attr('disabled', false);
            } else {
                $('#btnEClaimTravalCApproval').attr('disabled', true);
            }
        } else {
            $('#btnEClaimTravalApprove,#btnEClaimTravalReject,#btnEClaimTravalHold').attr('disabled', false);
        }
    }
    else {
        $('#btnEClaimTravalApprove,#btnEClaimTravalReject,#btnEClaimTravalHold,#btnEClaimTravalCApproval').attr('disabled', true);
    }
}

function checkEClaimLCValidationUpdate() {
    var _data = $("#txtEClaimLCCheckerRemark").val();
    var _concurrentAppId = $("#hdfSConcurrentApprovalId").val();

    if (_data.trim().length > 1) {
        $('#btnEClaimLCApprove,#btnEClaimLCReject,#btnEClaimLCHold,#btnEClaimLCCApproval').attr('disabled', true);
        if ($('#chkEClaimLCIsConcurrentApp').is(':checked')) {
            if (_concurrentAppId != null && _concurrentAppId != "" && _concurrentAppId != "0" && _concurrentAppId != undefined) {
                $('#btnEClaimLCCApproval').attr('disabled', false);
            } else {
                $('#btnEClaimLCCApproval').attr('disabled', true);
            }
        } else {
            $('#btnEClaimLCApprove,#btnEClaimLCReject,#btnEClaimLCHold').attr('disabled', false);
        }
    }
    else {
        $('#btnEClaimLCApprove,#btnEClaimLCReject,#btnEClaimLCHold,#btnEClaimLCCApproval').attr('disabled', true);
    }
}

function checkEClaimDSAValidationUpdate() {
    var _data = $("#txtEClaimDSACheckerRemark").val();
    var _concurrentAppId = $("#hdfSConcurrentApprovalId").val();
    if (_data.trim().length > 1) {
        $('#btnEClaimDSAApprove,#btnEClaimDSAReject,#btnEClaimDSAHold,#btnEClaimDSACApproval').attr('disabled', true);
        if ($('#chkEClaimDSAIsConcurrentApp').is(':checked')) {
            if (_concurrentAppId != null && _concurrentAppId != "" && _concurrentAppId != "0" && _concurrentAppId != undefined) {
                $('#btnEClaimDSACApproval').attr('disabled', false);
            } else {
                $('#btnEClaimDSACApproval').attr('disabled', true);
            }
        } else {
            $('#btnEClaimDSAApprove,#btnEClaimDSAReject,#btnEClaimDSAHold').attr('disabled', false);
        }
    }
    else {
        $('#btnEClaimDSAApprove,#btnEClaimDSAReject,#btnEClaimDSAHold,#btnEClaimDSACApproval').attr('disabled', true);
    }
}

function checkEClaimPOValidationUpdate() {
    var _data = $("#txtEClaimPOCheckerRemark").val();
    var _concurrentAppId = $("#hdfSConcurrentApprovalId").val();
    if (_data.trim().length > 1) {
        $('#btnEClaimPOApprove,#btnEClaimPOReject,#btnEClaimPOHold,#btnEClaimPOCApproval').attr('disabled', true);
        if ($('#chkEClaimPOIsConcurrentApp').is(':checked')) {
            if (_concurrentAppId != null && _concurrentAppId != "" && _concurrentAppId != "0" && _concurrentAppId != undefined) {
                $('#btnEClaimPOCApproval').attr('disabled', false);
            } else {
                $('#btnEClaimPOCApproval').attr('disabled', true);
            }
        } else {
            $('#btnEClaimPOApprove,#btnEClaimPOReject,#btnEClaimPOHold').attr('disabled', false);
        }
    }
    else {
        $('#btnEClaimPOApprove,#btnEClaimPOReject,#btnEClaimPOHold,#btnEClaimPOCApproval').attr('disabled', true);
    }
}

function checkEClaimARFEPValidationUpdate() {
    var _data = $("#txtEClaimARFEPCheckerRemark").val();
    var _concurrentAppId = $("#hdfSConcurrentApprovalId").val();
    if (_data.trim().length > 1) {
        $('#btnEClaimARFEPApprove,#btnEClaimARFEPReject,#btnEClaimARFEPHold,#btnEClaimARFEPCApproval').attr('disabled', true);
        if ($('#chkEClaimARFEPIsConcurrentApp').is(':checked')) {
            if (_concurrentAppId != null && _concurrentAppId != "" && _concurrentAppId != "0" && _concurrentAppId != undefined) {
                $('#btnEClaimARFEPCApproval').attr('disabled', false);
            } else { $('#btnEClaimARFEPCApproval').attr('disabled', true); }
        } else {
            $('#btnEClaimARFEPHold,#btnEClaimARFEPApprove,#btnEClaimARFEPReject').attr('disabled', false);
        }
    }
    else {
        $('#btnEClaimARFEPApprove,#btnEClaimARFEPReject,#btnEClaimARFEPHold,#btnEClaimARFEPCApproval').attr('disabled', true);
    }
}

function checkEClaimARFFValidationUpdate() {
    var _data = $("#txtEClaimARFFCheckerRemark").val();
    var _concurrentAppId = $("#hdfSConcurrentApprovalId").val();
    if (_data.trim().length > 1) {
        $('#btnEClaimARFFApprove,#btnEClaimARFFReject,#btnEClaimARFFHold,#btnEClaimARFFCApproval').attr('disabled', true);
        if ($('#chkEClaimARFFIsConcurrentApp').is(':checked')) {
            if (_concurrentAppId != null && _concurrentAppId != "" && _concurrentAppId != "0" && _concurrentAppId != undefined) {
                $('#btnEClaimARFFCApproval').attr('disabled', false);
            } else { $('#btnEClaimARFFCApproval').attr('disabled', true); }
        } else {
            $('#btnEClaimARFFHold,#btnEClaimARFFApprove,#btnEClaimARFFReject').attr('disabled', false);
        }
    }
    else {
        $('#btnEClaimARFFApprove,#btnEClaimARFFReject,#btnEClaimARFFHold,#btnEClaimARFFCApproval').attr('disabled', true);
    }
}
