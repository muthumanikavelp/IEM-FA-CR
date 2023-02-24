var objInexDet;
var objECFDoc;
var objDialogyReprocess;

MUrlDet = MUrlDet.replace("FetchInexDetails", "");
MUrlHelper = MUrlHelper.replace("GetAutoCompleteCourier", "");

var InexReviewDispatchModel = function () {

    var self = this;

    self.inexId = ko.observable("");
    self.DocType = ko.observable("");
    self.DocNo = ko.observable("");
    self.InexDate = ko.observable("");
    self.InexByName = ko.observable("");

    var InexDetails = {
        inexId: self.inexId,
        DocType: self.DocType,
        DocNo: self.DocNo,
        InexDate: self.InexDate,
        InexByName: self.InexByName
    };
        
    self.InexReviewArray = ko.observableArray();
    self.DocDetails = ko.observableArray();
    //document type dropdown array
    self.DocumentTypeArray = ko.observableArray();

    this.loadDocType = function () {
        $.ajax({
            type: "post",
            url: MUrlHelper + "MasterDocumentType",
            data: '{}',
            contentType: "application/json;",
            success: function (response) {
                self.DocumentTypeArray(response);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.search = function () {
        var InexFilter = {
            InexFrom: $("#txtSInexFrom").val(),
            InexTo: $("#txtSInexTo").val(),
            InexById: $("#hdfSInexById").val(),
            DocAmount: $("#txtSDocAmount").val().replace(/,/g, ''),
            DocFrom: $("#txtSDocFrom").val(),
            DocTo: $("#txtSDocTo").val(),
            DocTypeId: $("#ddlSDocType").val(),
            DocNo: $("#txtSDocNumber").val(),
            EmpCodeId: $("#hdfSEmpCodeId").val(),
            EmpNameId: $("#hdfSEmpName").val(),
            SuppCodeId: $("#hdfSSupplierId").val(),
            SuppNameId: $("#hdfSSupplierName").val(),
            ViewType: 1
        };
        $.ajax({
            type: "post",
            url: MUrlDet + "FetchInexDetails",
            data: JSON.stringify(InexFilter),
            contentType: "application/json;",
            success: function (response) {

                $("#gvDispatch").DataTable({
                    "autoWidth": false,
                    "destroy": true
                }).destroy();
                self.InexReviewArray.removeAll();

                var Data1 = "", Data2 = "";
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    self.InexReviewArray(Data2);
                }

                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");
                    }

                    if (self.InexReviewArray().length == 0 && Data1[0].Message == "") {
                        jAlert("Sorry! No Records Found.", "Message");
                    }
                }
                
                
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };
    //-----------------------Coded By Subburaj  19.04.2016---------------------------------//

    this.DownloadInexDespatchReport = function () {

        var InexFrom = $("#txtSInexFrom").val();
        var InexTo = $("#txtSInexTo").val();
        var InexById = $("#hdfSInexById").val();
        var DocAmount = $("#txtSDocAmount").val();
        var DocFrom = $("#txtSDocFrom").val();
        var DocTo = $("#txtSDocTo").val();
        var DocTypeId = $("#ddlSDocType").val();
        var DocNo = $("#txtSDocNumber").val();
        var EmpCodeId = $("#hdfSEmpCodeId").val();
        var EmpNameId = $("#hdfSEmpName").val();
        var SuppCodeId = $("#hdfSSupplierId").val();
        var SuppNameId = $("#hdfSSupplierName").val();
        var ViewType = '1';

        ko.utils.postJson(MUrlDet + "DownloadInexDespatchReport?InexFrom=" + InexFrom + "&InexTo=" + InexTo + "&InexID=" + InexById + "&Docamount=" + DocAmount + "&Docfrom=" + DocFrom + "&Docto=" + DocTo + "&doctypeid=" + DocTypeId + "&Docno=" + DocNo + "&empcodeid=" + EmpCodeId + "&Empnameid=" + EmpNameId + "&suppcodeid=" + SuppCodeId + "&suppnameid=" + SuppNameId + "&Viewtype=" + ViewType);
    }

    //--------------------------End------------------------------------------//
    self.ViewDocument = function () {
        var _tmpData = {
            DocNo: $("#lblDocNo").val()
        };
        var ecfId = "0", DocSubTypeId = "0";
        $.ajax({
            type: "post",
            url: MUrlHelper + "GetDocumentECFDetails",
            data: JSON.stringify(_tmpData),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                if (response.ecfId != null && response.ecfId != "0" && JSON.parse(response.ecfId) != null) {
                    ecfId = response.ecfId;
                }
                if (response.docSubTypeId != null && response.docSubTypeId != "0" && JSON.parse(response.docSubTypeId) != null) {
                    DocSubTypeId = response.docSubTypeId;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
                ecfId = "0";
                DocSubTypeId = "0";
            }
        });

        if (ecfId == "0" || DocSubTypeId == "0") {
            jAlert("Invalid Document Number!", "Message");
            return false;
        } else {
            ECFDoc(ecfId, DocSubTypeId);
        }
    };

    self.ViewDocument1 = function () {
        var _tmpData = {
            DocNo: $("#lblRPDocNo").val()
        };
        var ecfId = "0", DocSubTypeId = "0";
        $.ajax({
            type: "post",
            url: MUrlHelper + "GetDocumentECFDetails",
            data: JSON.stringify(_tmpData),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                if (response.ecfId != null && response.ecfId != "0" && JSON.parse(response.ecfId) != null) {
                    ecfId = response.ecfId;
                }
                if (response.docSubTypeId != null && response.docSubTypeId != "0" && JSON.parse(response.docSubTypeId) != null) {
                    DocSubTypeId = response.docSubTypeId;
                }                
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
                ecfId = "0";
                DocSubTypeId = "0";
            }
        });

        if (ecfId == "0" || DocSubTypeId == "0") {
            jAlert("Invalid Document Number!", "Message");
            return false;
        } else {
            ECFDoc(ecfId, DocSubTypeId);
        }
    };

    self.Dispatch = function (InexDetails) {
        $("#hdfInexId").val("");
        $("#hdfInexId").val(InexDetails.inexId);
        $("#lblDocType").val(InexDetails.DocType);
        $("#lblDocNo").val(InexDetails.DocNo);
        $("#txtInexDate").val(InexDetails.InexDate);
        $("#txtInexBy").val(InexDetails.InexByName);
        $("#txtInexReason").val(InexDetails.InexReason);
        
        $("#txtDispatchTo").val("");
        $("#hdfDispatchId").val("0");
        $("#txtBranch").val("");
        $("#hdfBranchId").val("0");
        $("#txtDispatchAddress").val("");
        $("#txtDispatchDate").val("");
        $("#txtCourierName").val("");
        $("#hdfCourierId").val("0");
        $("#txtAWBNo").val("");        
        $("#txtRemarks").val("");
        
        $("#hdfDispatchId").val(InexDetails.RaiserId);
        $("#txtDispatchTo").val(InexDetails.RaiserName);

        $("#hdfBranchId").val(InexDetails.branchId);
        $("#txtBranch").val(InexDetails.branchName);
        $("#txtDispatchAddress").val(InexDetails.branchAddress);

        var _dispId = $("#hdfDispatchId").val();
        var _dispName = $("#txtDispatchTo").val();
        if ((_dispName.trim() == "" || _dispName.trim() != "") && (_dispId.trim() == "" || _dispId.trim() == "0")) {
            $("#txtDispatchTo").val("");
            $("#hdfDispatchId").val("0");

            $("#txtDispatchTo").removeClass('required').removeClass('valid');
            $("#txtDispatchTo").addClass('required');
        } else {
            $("#txtDispatchTo").removeClass('required').removeClass('valid');
            $("#txtDispatchTo").addClass('valid');
        }

        var _branId = $("#hdfBranchId").val();
        var _branName = $("#txtBranch").val();
        if ((_branName.trim() == "" || _branName.trim() != "") && (_branId.trim() == "" || _branId.trim() == "0")) {
            $("#txtBranch").val("");
            $("#hdfBranchId").val("0");

            $("#txtBranch").removeClass('required').removeClass('valid');
            $("#txtBranch").addClass('required');
        } else {
            $("#txtBranch").removeClass('required').removeClass('valid');
            $("#txtBranch").addClass('valid');
        }

        var _dispAdd = $("#txtDispatchAddress").val();
        if (_dispAdd.trim() != "") {
            $("#txtDispatchAddress").removeClass('required').removeClass('valid');
            $("#txtDispatchAddress").addClass('valid');
        }
        else {
            $("#txtDispatchAddress").removeClass('valid').removeClass('required');
            $("#txtDispatchAddress").addClass('required');
        }
        InexDet();
    };

    self.showReprocess = function (InexDetails) {
        $("#hdfRPInexId").val("");
        $("#hdfRPInexId").val(InexDetails.inexId);
        $("#lblRPDocType").val(InexDetails.DocType);
        $("#lblRPDocNo").val(InexDetails.DocNo);
        $("#txtRPInexDate").val(InexDetails.InexDate);
        $("#txtRPInexBy").val(InexDetails.InexByName);
        $("#txtRPInexReason").val(InexDetails.InexReason);
            
        $("#txtRPRemarks").val("");
        
        ShowDialogReprocess();
    };

    self.Reprocess = function () {
        var data = {
            InexId: $("#hdfRPInexId").val(),
            Remarks: $("#txtRPRemarks").val()
        };
        $.ajax({
            type: "post",
            url: MUrlDet + "DispatchReprocessInex",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");
                    }

                    if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {
                        //self.InexReviewArray.removeAll();
                        objDialogyReprocess.dialog("close");

                        self.search();
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.updateReview = function () {
        var data = {
            InexId: $("#hdfInexId").val(),
            DispatchTo: $("#hdfDispatchId").val(),
            BranchId: $("#hdfBranchId").val(),
            DispatchDate: $("#txtDispatchDate").val(),
            DispatchAddress: $("#txtDispatchAddress").val(),
            CourierId: $("#hdfCourierId").val(),
            AWB_no: $("#txtAWBNo").val(),
            Remarks: $("#txtRemarks").val()
        };
        $.ajax({
            type: "post",
            url: MUrlDet + "DispatchInex",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");
                    }

                    if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {
                        //self.InexReviewArray.removeAll();

                        objInexDet.dialog("close");

                        self.search();
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };
    
    self.clearDetails = function () {
        $("#txtSInexFrom").val("");
        $("#txtSInexTo").val("");
        $("#hdfSInexById").val("0");
        $("#txtSInexBy").val("");
        $("#txtSDocAmount").val("");
        $("#txtSDocFrom").val("");
        $("#txtSDocTo").val("");
        $("#ddlSDocType").val("");
        $("#txtSDocNumber").val("");

        $("#hdfSEmpCodeId").val("0");
        $("#txtSEmpCode").val("");

        $("#hdfSEmpName").val("0");
        $("#txtSEmpName").val("");

        $("#hdfSSupplierId").val("0");
        $("#txtSSupplierCode").val("");

        $("#hdfSSupplierName").val("0");
        $("#txtSSupplierName").val("");

        //self.InexDetails(null);
        $("#gvDispatch").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();        
        self.InexReviewArray.removeAll();
    };

    self.DatatableCall = function () {
        if ($("#gvDispatch > tbody > tr").length == self.InexReviewArray().length) {
            $("#gvDispatch").DataTable({
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

    self.loadGrid = function () {
        $("#gvDispatch").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.InexReviewArray.removeAll();
    }

    //load the Doctument Type DropDown.
    self.loadDocType();

    self.loadGrid();
};

var mainViewModel = new InexReviewDispatchModel();
ko.applyBindings(mainViewModel);

$(document).ready(function () {
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

    objInexDet = $("[id$='ShowInexDialog']");
    objInexDet.dialog({
        autoOpen: false,
        modal: true,
        width: 900,
        height: 375,
        duration: 300
    });

    objECFDoc = $("[id$='ShwECFDet']");
    objECFDoc.dialog({
        autoOpen: false,
        modal: true,
        width: 1000,
        height: 500,
        duration: 300
    });

    objDialogyReprocess = $("[id$='ShowDialogReprocess']");
    objDialogyReprocess.dialog({
        autoOpen: false,
        modal: true,
        width: 900,
        height: 300,
        duration: 300
    });

    var objDate = new Date();
    var Presentyear = objDate.getFullYear();
    $("#txtSInexFrom").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
    });
    $("#txtSInexTo").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
    });
    $("#txtSDocFrom").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
    });
    $("#txtSDocTo").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
    });
    $("#txtDispatchDate").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
    });

    //Load Employee Code Auto Complete
    $("#txtSEmpCode").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfSEmpCodeId").val("0");
        }

        $("#txtSEmpCode").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: MUrlHelper + "GetAutoCompleteEmployeeCode",
                    data: "{ 'txt' : '" + $("#txtSEmpCode").val() + "'}",
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
                $("#hdfSEmpCodeId").val(i.item.val);
                $("#txtSEmpCode").val(i.item.label);
            }
        });
    });

    $("#txtSEmpCode").focusout(function () {
        var hdfId = $("#hdfSEmpCodeId").val();
        var txtCurName = $("#txtSEmpCode").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtSEmpCode").val("");
        }
    });

    //Load txtSSupplier Code Auto Complete
    $("#txtSSupplierCode").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfSSupplierId").val("0");
        }

        $("#txtSSupplierCode").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: MUrlHelper + "GetAutoCompleteSupplierCode",
                    data: "{ 'txt' : '" + $("#txtSSupplierCode").val() + "'}",
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
                $("#hdfSSupplierId").val(i.item.val);
                $("#txtSSupplierCode").val(i.item.label);
            }
        });
    });

    $("#txtSSupplierCode").focusout(function () {
        var hdfId = $("#hdfSSupplierId").val();
        var txtCurName = $("#txtSSupplierCode").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtSSupplierCode").val("");
        }
    });

    //Load Employee Name Auto Complete
    $("#txtSEmpName").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfSEmpName").val("0");
        }

        $("#txtSEmpName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: MUrlHelper + "GetAutoCompleteEmployee",
                    data: "{ 'txt' : '" + $("#txtSEmpName").val() + "'}",
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
                $("#hdfSEmpName").val(i.item.val);
                $("#txtSEmpName").val(i.item.label);
            }
        });
    });

    $("#txtSEmpName").focusout(function () {
        var hdfId = $("#hdfSEmpName").val();
        var txtCurName = $("#txtSEmpName").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtSEmpName").val("");
        }
    });

    //Load Supplier Name Auto Complete
    $("#txtSSupplierName").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfSSupplierName").val("0");
        }

        $("#txtSSupplierName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: MUrlHelper + "GetAutoCompleteSupplier",
                    data: "{ 'txt' : '" + $("#txtSSupplierName").val() + "'}",
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
                $("#hdfSSupplierName").val(i.item.val);
                $("#txtSSupplierName").val(i.item.label);
            }
        });
    });

    $("#txtSSupplierName").focusout(function () {
        var hdfId = $("#hdfSSupplierName").val();
        var txtCurName = $("#txtSSupplierName").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtSSupplierName").val("");
        }
    });

    //Load Employee Name Auto Complete
    $("#txtSInexBy").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfSInexById").val("0");
        }

        $("#txtSInexBy").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: MUrlHelper + "GetAutoCompleteEmployee",
                    data: "{ 'txt' : '" + $("#txtSInexBy").val() + "'}",
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
                $("#hdfSInexById").val(i.item.val);
                $("#txtSInexBy").val(i.item.label);
            }
        });
    });

    $("#txtSInexBy").focusout(function () {
       var hdfId = $("#hdfSInexById").val();
        var txtCurName = $("#txtSInexBy").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtSInexBy").val("");
        }
    });    

    //popup validation
    $("#txtDispatchAddress").focusin(function () {
        chkInexValidation();

        var _dispAddress = $("#txtDispatchAddress").val();
        if (_dispAddress.trim() != "") {
            $("#txtDispatchAddress").removeClass('required');
            $("#txtDispatchAddress").addClass('valid');
        }
        else {
            $("#txtDispatchAddress").removeClass('valid');
            $("#txtDispatchAddress").addClass('required');
        }
    });

    $("#txtDispatchAddress").keyup(function () {
        chkInexValidation();

        var _dispAddress = $("#txtDispatchAddress").val();
        if (_dispAddress.trim() != "") {
            $("#txtDispatchAddress").removeClass('required');
            $("#txtDispatchAddress").addClass('valid');
        }
        else {
            $("#txtDispatchAddress").removeClass('valid');
            $("#txtDispatchAddress").addClass('required');
        }
    });

    $("#txtAWBNo").keyup(function () {
        chkInexValidation();
        var _awbno = $("#txtAWBNo").val();
        if (_awbno.trim() != "") {
            $("#txtAWBNo").removeClass('required');
            $("#txtAWBNo").addClass('valid');
        }
        else {
            $("#txtAWBNo").removeClass('valid');
            $("#txtAWBNo").addClass('required');
        }
    });

    $("#txtDispatchDate").change(function () {
        chkInexValidation();
        var _date = $("#txtDispatchDate").val();
        if (_date.trim() != "") {
            $("#txtDispatchDate").removeClass('required');
            $("#txtDispatchDate").addClass('valid');
        }
        else {
            $("#txtDispatchDate").removeClass('valid');
            $("#txtDispatchDate").addClass('required');
        }
    });

    //Load Dispatch To Auto Complete
    $("#txtDispatchTo").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfDispatchId").val("0");
        }

        $("#txtDispatchTo").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: MUrlHelper + "GetAutoCompleteEmployee",
                    data: "{ 'txt' : '" + $("#txtDispatchTo").val() + "'}",
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
                $("#hdfDispatchId").val(i.item.val);
                $("#txtDispatchTo").val(i.item.label);
            }
        });
    });

    $("#txtDispatchTo").focusout(function () {
        var hdfId = $("#hdfDispatchId").val();
        var txtName = $("#txtDispatchTo").val();
        if ((txtName.trim() == "" || txtName.trim() != "") && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtDispatchTo").val("");

            $("#txtDispatchTo").removeClass('valid');
            $("#txtDispatchTo").addClass('required');
        } else {
            $("#txtDispatchTo").removeClass('required');
            $("#txtDispatchTo").addClass('valid');
        }
    });

    //Load Branch Auto Complete
    $("#txtBranch").keyup(function (e) {        
        if (e.which != 13) {           
            $("#hdfBranchId").val("0");            
        }

        $("#txtBranch").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: MUrlHelper + "GetAutoCompleteBranch",
                    data: "{ 'txt' : '" + $("#txtBranch").val() + "'}",
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
                $("#hdfBranchId").val(i.item.val);
                $("#txtBranch").val(i.item.label);
            }
        });
    });

    $("#txtBranch").focusout(function () {        
        var hdfId = $("#hdfBranchId").val();
        var _branch = $("#txtBranch").val();
        if ((_branch.trim() == "" || _branch.trim() != "") && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtBranch").val("");
            $("#txtBranch").removeClass('valid');
            $("#txtBranch").addClass('required');
        } else {
            $("#txtBranch").removeClass('required');
            $("#txtBranch").addClass('valid');

            //fetch the branch address details
            var address="";
            $.ajax({
                type: "post",
                url: MUrlHelper + "GetBranchAddressDetails",
                data: "{ 'LocationId' : '" + $("#hdfBranchId").val() + "'}",
                async: false,
                contentType: "application/json;",
                success: function (response) {
                    address = response.Address;
                    $("#txtDispatchAddress").val(address);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //alert(errorThrown);
                }
            });
        }
    });

    //Load Courier Auto Complete
    $("#txtCourierName").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfCourierId").val("0");
        }

        $("#txtCourierName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: MUrlHelper + "GetAutoCompleteCourier",
                    data: "{ 'txt' : '" + $("#txtCourierName").val() + "'}",
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
                $("#hdfCourierId").val(i.item.val);
                $("#txtCourierName").val(i.item.label);
            }
        });
    });

    $("#txtCourierName").focusout(function () {
        var hdfId = $("#hdfCourierId").val();
        var courName = $("#txtCourierName").val();
        if ((courName.trim() == "" || courName.trim() != "") && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtCourierName").val("");

            $("#txtCourierName").removeClass('valid');
            $("#txtCourierName").addClass('required');
        } else {
            $("#txtCourierName").removeClass('required');
            $("#txtCourierName").addClass('valid');
        }
    });

    $("#txtRPRemarks").keyup(function () {
        chkReprocess();

        var _RPRemarks = $("#txtRPRemarks").val();
        if (_RPRemarks.trim() != "") {
            $("#txtRPRemarks").removeClass('required');
            $("#txtRPRemarks").addClass('valid');
        }
        else {
            $("#txtRPRemarks").removeClass('valid');
            $("#txtRPRemarks").addClass('required');
        }
    });

    $("#txtSDocAmount").keyup(function (event) {
        if (event.which >= 37 && event.which <= 40) {
            event.preventDefault();
        }

        var currentval = $(this).val();
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
        $(this).val(components.join("."));
    });
   
});

function InexDet() {
    $('#ShowInexDialog').attr("style", "display:block;");
    $('#btnDispatch').attr('disabled', true);

    $("#txtDispatchDate").removeClass('valid');
    $("#txtDispatchDate").addClass('required');

    $("#txtCourierName").removeClass('valid');
    $("#txtCourierName").addClass('required');

    $("#txtAWBNo").removeClass('valid');
    $("#txtAWBNo").addClass('required');


    objInexDet.dialog({ title: 'Inex Dispatch', width: '900', height: '375' });
    objInexDet.dialog("open");
    return false;
}

function ECFDoc(ecfId, DocSubTypeId) {
    $('#ShwECFDet').attr("style", "display:block;");

    var url = MUrlDet + 'DocumentDetails/' + ecfId + '/' + DocSubTypeId;
    objECFDoc.load(url);
    objECFDoc.dialog({ title: 'Document Detail' });
    objECFDoc.dialog("open");    
    return false;
}

function ShowDialogReprocess() {
    $('#ShowDialogReprocess').attr("style", "display:block;");
    
    $('#btnReprocess').attr('disabled', true);

    $("#txtRPRemarks").removeClass('valid').removeClass('required');
    $("#txtRPRemarks").addClass('required');

    objDialogyReprocess.dialog({ title: 'Inex Reprocess Entry', width: '900', height: '300' });
    objDialogyReprocess.dialog("open");
    return false;
}

function chkInexValidation() {    
    var _inexId = $("#hdfInexId").val();
    var _dispId = $("#hdfDispatchId").val();
    var _BranchId = $("#hdfBranchId").val();
    var _dispAddress = $("#txtDispatchAddress").val();
    var _dispatchDate = $("#txtDispatchDate").val();
    var _courierId = $("#hdfCourierId").val();
    var _AWBNo = $("#txtAWBNo").val();

    if (_inexId == "" || _inexId == "0" || _dispId == "" || _dispId == "0" || _BranchId == "" || _BranchId == "0" || _dispAddress == "" || _dispatchDate == "" || _courierId == "" || _courierId == "0" || _AWBNo == "") {
        $('#btnDispatch').attr('disabled', true);
    }

    else {
        $('#btnDispatch').attr('disabled', false);
    }
}

function chkReprocess() {
    var _dispAddress = $("#txtRPRemarks").val();

    if (_dispAddress=="") {
        $('#btnReprocess').attr('disabled', true);
    }else {
        $('#btnReprocess').attr('disabled', false);
    }
}