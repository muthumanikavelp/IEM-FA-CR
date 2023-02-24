var objDialogy;
UrlDet = UrlDet.replace("GetPaymodeConversionChecker", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");
var CreditNoteCheckerModel = function () {
    var self = this;


    self.CheckerDetailsArray = ko.observableArray();
    self.PaymentModeArray = ko.observableArray();

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

    self.loadPaymentMode = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "LoadPayModeType",
            data: '{}',
            contentType: "application/json;",
            success: function (response) {
                self.PaymentModeArray(response);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.search = function () {
        var data = {
            PVNo: "",
            ReqDateFrom: $("#txtRequestFrom").val(),
            ReqDateTo: $("#txtRequestTo").val(),
            ReqCodeId: $("#hdfReqCodeId").val(),
            ReqNameId: $("#hdfReqNameId").val(),
            EmpCodeId: $("#hdfEmpCodeId").val(),
            EmpNameId: $("#hdfEmpName").val(),
            SuppCodeId: $("#hdfSupplierId").val(),
            SuppNameId: $("#hdfSupplierName").val(),
            PayMode: $("#ddlPayMode").val(),
            ViewType: "0"
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetPaymodeConversionChecker",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                self.loadGrid();

                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");
                    }

                    if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                        Data2 = JSON.parse(response.Data2);
                        self.CheckerDetailsArray(Data2);
                    }
                    //show Message if error
                    if (Data1[0].Message == "" && self.CheckerDetailsArray().length == 0) {
                        jAlert("Sorry no record found!", "Message");
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.Reissue = function (item) {
        ShowDialog();
        var data = {
            PVNo: item.PvNo,
            ReqDateFrom: $("#txtRequestFrom").val(),
            ReqDateTo: $("#txtRequestTo").val(),
            ReqCodeId: $("#hdfReqCodeId").val(),
            ReqNameId: $("#hdfReqNameId").val(),
            EmpCodeId: $("#hdfEmpCodeId").val(),
            EmpNameId: $("#hdfEmpName").val(),
            SuppCodeId: $("#hdfSupplierId").val(),
            SuppNameId: $("#hdfSupplierName").val(),
            PayMode: $("#ddlPayMode").val(),
            ViewType: "1"
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetPaymodeConversionChecker",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";

                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Message != "" && id == 1) {
                        jAlert(Data1[0].Message, "Message");
                    }
                }

                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    $("#lblPVDate").text(Data2[0].PvDate);
                    $("#lblPVNo").text(Data2[0].PvNo);
                    $("#lblPVAmount").text(self.formatNumber(Data2[0].PvAmount));
                    $("#lblPayMode").text(Data2[0].PayMode);
                    $("#lblChequeNo").text(Data2[0].ChqNo);
                    $("#lblMemoNo").text(Data2[0].MemoNo);
                    $("#lblBankName").text(Data2[0].Bank);
                    $("#lblAcNo").text(Data2[0].accNo);
                    $("#lblIFSCCode").text(Data2[0].IFSCcode);
                    $("#lblType").text(Data2[0].EmployeeSupplierType);
                    $("#lblSECode").text(Data2[0].EmployeeSupplierCode);
                    $("#lblSEName").text(Data2[0].EmployeeSupplierName);
                    $("#lblbounceDate").text(Data2[0].RejectDate);
                    $("#txtBounceReason").text(Data2[0].RejectReason);

                    $("#txtMBeneficiaryName").text(Data2[0].NewBeneficiary);
                    $("#txtMAcNo").text(Data2[0].NewaccNo);
                    $("#txtMIFSCCode").text(Data2[0].NewIFSCcode);
                    $("#txtMBankName").text(Data2[0].NewBank);
                    $("#hfEcfIdReissue").val(Data2[0].PaymentReissuegid);
                    $("#hfBankIdReissue").val(Data2[0].NewBankId);
                    $("#txtMPayMode").text(Data2[0].NewPayMode);
                    $("#txtMDescription").text(Data2[0].Description);
                    $("#txtMRemark").text(Data2[0].makerRemark);
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.Approve = function () {
        //var count = $("#txtChekerRemark").val();
        //if (count == "" || count == "0") {
        //    jAlert("Ensure Checker Remark!", "Error");
        //    return false;
        //}
        var data = {
            PaymentReIssueGId: $("#hfEcfIdReissue").val(),
            Status: "Approve",
            Remarks: $("#txtChekerRemark").val(),
            chqno: $("#lblChequeNo").text(),
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetPaymodeConversionChecker",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 == "Unauntorized User!") {
                    jAlert("Unauthorized User!");
                }
                else if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message
                    if (Data1[0].Clear == "True" || Data1[0].Clear == "true" || Data1[0].Clear == "1") {
                        objDialogy.dialog("close");
                        jConfirm(Data1[0].Message, "Message", function (callback) {
                            if (callback == true) {
                                self.search();
                            } else {
                                self.search();
                            }
                        });

                        //remove the cancel button from dialog box
                        $("#popup_ok").attr("style", "margin-left: 50px;");
                        $("#popup_cancel").attr("style", "visibility: hidden");
                    } else {
                        //show Message if error
                        if (Data1[0].Message != "") {
                            jAlert(Data1[0].Message, "Message");
                        }
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.Reject = function () {
        //var count = $("#txtChekerRemark").val();
        //if (count == "" || count == "0") {
        //    jAlert("Ensure Checker Remark!", "Error");
        //    return false;
        //}
        var data = {
            PaymentReIssueGId: $("#hfEcfIdReissue").val(),
            Status: "Reject",
            Remarks: $("#txtChekerRemark").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetPaymodeConversionChecker",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 == "Unauntorized User!") {
                    jAlert("Unauthorized User!");
                }
                else if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message
                    if (Data1[0].Clear == "True" || Data1[0].Clear == "true" || Data1[0].Clear == "1") {
                        objDialogy.dialog("close");
                        jConfirm(Data1[0].Message, "Message", function (callback) {
                            if (callback == true) {
                                self.search();
                            } else {
                                self.search();
                            }
                        });

                        //remove the cancel button from dialog box
                        $("#popup_ok").attr("style", "margin-left: 50px;");
                        $("#popup_cancel").attr("style", "visibility: hidden");
                    } else {
                        //show Message if error
                        if (Data1[0].Message != "") {
                            jAlert(Data1[0].Message, "Message");
                        }
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.DatatableCall = function () {
        if ($("#gvChecker > tbody > tr").length == self.CheckerDetailsArray().length) {
            $("#gvChecker").DataTable({
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

    self.clearFilter = function () {
        $("#txtRequestFrom").val("");
        $("#txtRequestTo").val("");
        $("#txtReqCode").val("");
        $("#hdfReqCodeId").val("0");
        $("#txtReqName").val("");
        $("#hdfReqNameId").val("0");

        $("#txtEmpCode").val("");
        $("#hdfEmpCodeId").val("0");
        $("#txtEmpName").val("");
        $("#hdfEmpName").val("0");
        $("#txtSupplierCode").val("");
        $("#hdfSupplierId").val("0");
        $("#txtSupplierName").val("");
        $("#hdfSupplierName").val("0");
        $("#ddlPayMode").val("");

        self.loadGrid();
    };

    self.loadGrid = function () {
        $("#gvChecker").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.CheckerDetailsArray.removeAll();
    }
    self.loadGrid();
    self.loadPaymentMode();
};

var mainViewModel = new CreditNoteCheckerModel();
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

    objDialogy = $("[id$='ShowDialog']");
    objDialogy.dialog({
        autoOpen: false,
        modal: true,
        width: 800,
        height: 350,
        duration: 300
    });


    var objDate = new Date();
    var Presentyear = objDate.getFullYear();
    $(".fsdatepicker").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
    });

    //Load Employee Code Auto Complete
    $("#txtEmpCode").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfEmpCodeId").val("0");
        }

        $("#txtEmpCode").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteEmployeeCode",
                    data: "{ 'txt' : '" + $("#txtEmpCode").val() + "'}",
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
                $("#hdfEmpCodeId").val(i.item.val);
                $("#txtEmpCode").val(i.item.label);
            }
        });
    });

    $("#txtEmpCode").focusout(function () {
        var hdfId = $("#hdfEmpCodeId").val();
        var txtCurName = $("#txtEmpCode").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtEmpCode").val("");
        }
    });

    //Load Employee Name Auto Complete
    $("#txtEmpName").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfEmpName").val("0");
        }

        $("#txtEmpName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteEmployee",
                    data: "{ 'txt' : '" + $("#txtEmpName").val() + "'}",
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
                $("#hdfEmpName").val(i.item.val);
                $("#txtEmpName").val(i.item.label);
            }
        });
    });

    $("#txtEmpName").focusout(function () {
        var hdfId = $("#hdfEmpName").val();
        var txtCurName = $("#txtEmpName").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtEmpName").val("");
        }
    });

    //Load txtSSupplier Code Auto Complete
    $("#txtSupplierCode").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfSupplierId").val("0");
        }

        $("#txtSupplierCode").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteSupplierCode",
                    data: "{ 'txt' : '" + $("#txtSupplierCode").val() + "'}",
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
                $("#hdfSupplierId").val(i.item.val);
                $("#txtSupplierCode").val(i.item.label);
            }
        });
    });

    $("#txtSupplierCode").focusout(function () {
        var hdfId = $("#hdfSupplierId").val();
        var txtCurName = $("#txtSupplierCode").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtSupplierCode").val("");
        }
    });

    //Load Supplier Name Auto Complete
    $("#txtSupplierName").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfSupplierName").val("0");
        }

        $("#txtSupplierName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteSupplier",
                    data: "{ 'txt' : '" + $("#txtSupplierName").val() + "'}",
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
                $("#hdfSupplierName").val(i.item.val);
                $("#txtSupplierName").val(i.item.label);
            }
        });
    });

    $("#txtSupplierName").focusout(function () {
        var hdfId = $("#hdfSupplierName").val();
        var txtCurName = $("#txtSupplierName").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtSupplierName").val("");
        }
    });

    //Load Request Code Auto Complete
    $("#txtReqCode").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfReqCodeId").val("0");
        }

        $("#txtReqCode").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteEmployeeCode",
                    data: "{ 'txt' : '" + $("#txtReqCode").val() + "'}",
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
                $("#hdfReqCodeId").val(i.item.val);
                $("#txtReqCode").val(i.item.label);
            }
        });
    });

    $("#txtReqCode").focusout(function () {
        var hdfId = $("#hdfReqCodeId").val();
        var txtCurName = $("#txtReqCode").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtReqCode").val("");
        }
    });

    //Load Request Name Auto Complete
    $("#txtReqName").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfReqNameId").val("0");
        }

        $("#txtReqName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteEmployee",
                    data: "{ 'txt' : '" + $("#txtReqName").val() + "'}",
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
                $("#hdfReqNameId").val(i.item.val);
                $("#txtReqName").val(i.item.label);
            }
        });
    });

    $("#txtReqName").focusout(function () {
        var hdfId = $("#hdfReqNameId").val();
        var txtCurName = $("#txtReqName").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtReqName").val("");
        }
    });

    $("#txtChekerRemark").keyup(function () {
        checkValidation();

        var _data = $("#txtChekerRemark").val();
        if (_data.trim() != "") {
            $("#txtChekerRemark").removeClass('required');
            $("#txtChekerRemark").addClass('valid');
        }
        else {
            $("#txtChekerRemark").removeClass('valid');
            $("#txtChekerRemark").addClass('required');
        }
    });
});

function checkValidation() {
    var _data = $("#txtChekerRemark").val();

    if (_data.trim() == "") {
        $('#btnApprove').attr('disabled', true);
        $('#btnReject').attr('disabled', true);
    }
    else {
        $('#btnApprove').attr('disabled', false);
        $('#btnReject').attr('disabled', false);
    }
}

function ShowDialog() {
    $("#txtChekerRemark").val("");
    $("#txtChekerRemark").removeClass('required').removeClass('valid');
    $("#txtChekerRemark").addClass('required');

    checkValidation();

    $('#ShowDialog').attr("style", "display:block;");
    objDialogy.dialog({ title: 'payment Reissue - Checker', width: '900', height: '630' });
    objDialogy.dialog("open");
    return false;
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

function isNumberAndLetterAndSpace(evt) {
    var key = evt.keyCode;
    return ((key >= 65 && key <= 90) || key == 8 || (key >= 97 && key <= 122) || (key >= 48 && key <= 57) || key == 127 || key == 32);
}

function isEvent(evt) {
    return false;
}