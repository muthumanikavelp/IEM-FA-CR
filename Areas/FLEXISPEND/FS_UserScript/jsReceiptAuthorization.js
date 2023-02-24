var objDialogyReceipt;
var objDialogGL;

UrlDet = UrlDet.replace("GetReceiptChecker", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");

var ChequeInventoryModel = function () {
    var self = this;

    //dropdown array
    self.SearchReceiptFromArray = ko.observableArray();
    self.BankArray = ko.observableArray();
    self.SourceArray = ko.observableArray();
    self.ReceiptModeArray = ko.observableArray();

    //Grid Details
    self.ReceiptEntryArray = ko.observableArray();
    self.tmpReceiptDetArray = ko.observableArray();
    self.ReceiptEntryDetailArray = ko.observableArray();

    self.loadReceiptFromWA = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "LoadClaimTypeWA",
            data: '{}',
            contentType: "application/json;",
            success: function (response) {
                self.SearchReceiptFromArray(response);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.loadBank = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "LoadBankDropDown",
            data: '{}',
            contentType: "application/json;",
            success: function (response) {
                self.BankArray(response);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.loadSource = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "LoadReceiptSource",
            data: '{}',
            contentType: "application/json;",
            success: function (response) {
                self.SourceArray(response);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.loadReceiptMode = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "LoadReceiptMode",
            data: '{}',
            contentType: "application/json;",
            success: function (response) {
                self.ReceiptModeArray(response);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.ReceiptFromChanged = function (obj, event) {
        if (event.originalEvent) {
            $("#txtEmpCode").removeAttr("disabled").removeAttr("style");
            $("#txtEmpName").removeAttr("disabled").removeAttr("style");
            $("#txtSupplierCode").removeAttr("disabled").removeAttr("style");
            $("#txtSupplierName").removeAttr("disabled").removeAttr("style");

            $("#hdfEmpCodeId").val("0");
            $("#txtEmpCode").val("");
            $("#hdfEmpName").val("0");
            $("#txtEmpName").val("");
            $("#hdfSupplierId").val("0");
            $("#txtSupplierCode").val("");
            $("#hdfSupplierName").val("0");
            $("#txtSupplierName").val("");

            var chkbit = $("#ddlReceiptFrom").val();
            if (chkbit == "E") {
                $("#txtSupplierCode").attr("disabled", "disabled");
                $("#txtSupplierName").attr("disabled", "disabled");
                $("#txtSupplierCode").attr("style", "background-color: gainsboro;");
                $("#txtSupplierName").attr("style", "background-color: gainsboro;");
            }

            if (chkbit == "S") {
                $("#txtEmpCode").attr("disabled", "disabled");
                $("#txtEmpName").attr("disabled", "disabled");
                $("#txtEmpCode").attr("style", "background-color: gainsboro;");
                $("#txtEmpName").attr("style", "background-color: gainsboro;");
            }
        } else {
        }
    }

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

    self.Search = function () {
        var data = {
            ReceiptDateFrom: $("#txtReceiptDateFrom").val(),
            ReceiptDateTo: $("#txtReceiptDateTo").val(),
            ReceiptNo: $("#txtReceiptNo").val(),
            ReceiptFrom: $("#ddlReceiptFrom").val(),
            EmpCodeId: $("#hdfEmpCodeId").val(),
            EmpNameId: $("#hdfEmpName").val(),
            SuppCodeId: $("#hdfSupplierId").val(),
            SuppNameId: $("#hdfSupplierName").val(),
            PayDateFrom: $("#txtPayFrom").val(),
            PayDateTo: $("#txtPayTo").val(),
            BankId: $("#ddlBank").val(),
            ReceiptMode: $("#ddlReceiptSource").val(),
            PayMode: $("#ddlReceiptMode").val(),
            Amount: $("#txtAmount").val().replace(/,/g, ''),
            PayRefNo: $("#txtPayRefNo").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetReceiptChecker",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                self.loadGrid();
                var Data1 = "", Data2 = "", Data3 = "";

                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    self.ReceiptEntryArray(Data2);
                }

                if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null) {
                    Data3 = JSON.parse(response.Data3);
                    self.tmpReceiptDetArray(Data3);
                } else {
                    self.tmpReceiptDetArray.removeAll();
                }

                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");
                    } else if (self.ReceiptEntryArray().length == 0) {
                        jAlert("Sorry no records found!", "Message");
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    };

    self.ClearFilter = function () {
        $("#txtReceiptDateFrom").val("");
        $("#txtReceiptDateTo").val("");
        $("#txtReceiptNo").val("");
        $("#ddlReceiptFrom").val("0");

        $("#hdfEmpCodeId").val("0");
        $("#txtEmpCode").val("");
        $("#hdfEmpName").val("0");
        $("#txtEmpName").val("");
        $("#hdfSupplierId").val("0");
        $("#txtSupplierCode").val("");
        $("#hdfSupplierName").val("0");
        $("#txtSupplierName").val("");

        $("#ddlClaimType").val("0");
        $("#ddlBank").val("0");
        $("#txtPayBatchNo").val("");
        $("#txtMemoNo").val("");

        $("#txtPayFrom").val("");
        $("#txtPayTo").val("");
        $("#ddlBank").val("");
        $("#ddlReceiptSource").val("0");
        $("#ddlReceiptMode").val("0");
        $("#txtAmount").val("");
        $("#txtPayRefNo").val("");

        $("#txtEmpCode").removeAttr("disabled").removeAttr("style");
        $("#txtEmpName").removeAttr("disabled").removeAttr("style");
        $("#txtSupplierCode").removeAttr("disabled").removeAttr("style");
        $("#txtSupplierName").removeAttr("disabled").removeAttr("style");

        self.loadGrid();
    };

    self.Select = function (item) {
        var tmpArray = new Array();
        $("#hdfReceiptId").val(item.receiptId);
        $("#ddlEBankName").val(item.bankName);
        $("#ddlEReceiptMode").val(item.payMode);
        $("#ddlESource").val(item.receiptMode);
        $("#ddlEReceiptFrom").val(item.receiptFrom);

        $("#txtRaiserName").val(item.Raiser);

        $("#txtEReceiptDate").val(item.receiptDate);
        $("#txtEReceiptName").val(item.EmployeeSupplierName);
        $("#txtEAmount").val(self.formatNumber(item.amount));
        $("#txtEPayRefNo").val(item.payRefNo);
        $("#txtETranDate").val(item.payDate);
        $("#txtERemarks").val(item.makerRemark);

        $("#hdfEReceiptName").val(item.EmployeeSupplierId);

        if (self.ReceiptEntryDetailArray() != null) {
            self.ReceiptEntryDetailArray([]);
        }

        tmpArray = ko.utils.arrayFilter(self.tmpReceiptDetArray(), function (aitem) {
            return item.receiptId == aitem.receiptId;
        });

        $("#txtCheckerRemarks").val("");

        $('#txtCheckerRemarks').removeClass('required').removeClass('valid');
        $('#txtCheckerRemarks').addClass('required');

        self.ReceiptEntryDetailArray(tmpArray.slice());
        ShowReceiptDetails("0");
        IsValidSubmit();
    };

    self.Approve = function () {
        var data = {
            ReceiptId: $("#hdfReceiptId").val(),
            Status: "Approve",
            Remarks: $("#txtCheckerRemarks").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "UpdateReceiptStatus",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                self.loadGrid();
                var Data1 = "";
                if (response.Data1 == "Unauthorized User!") {
                    jAlert("Unauthorized User!");
                    return false;
                }
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    jAlert(Data1[0].Message, "Message");

                    if (Data1[0].Clear == true) {
                        ShowReceiptDetails("1");
                        self.Search();
                    }
                } else {
                    jAlert("unable to process your request. please try again!", "Message");
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    };

    self.Reject = function () {
        var data = {
            ReceiptId: $("#hdfReceiptId").val(),
            Status: "Reject",
            Remarks: $("#txtCheckerRemarks").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "UpdateReceiptStatus",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                self.loadGrid();
                var Data1 = "";
                if (response.Data1 == "Unauthorized User!") {
                    jAlert("Unauthorized User!");
                    return false;
                }
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    jAlert(Data1[0].Message, "Message");

                    if (Data1[0].Clear == true) {
                        ShowReceiptDetails("1");
                        self.Search();
                    }
                } else {
                    jAlert("unable to process your request. please try again!", "Message");
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    };

    self.DatatableCall = function () {
        if ($("#gvReceiptEntry > tbody > tr").length == self.ReceiptEntryArray().length) {
            $("#gvReceiptEntry").DataTable({
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
        $("#gvReceiptEntry").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.ReceiptEntryArray.removeAll();
    }
    self.loadGrid();

    //load the Bank DropDown.
    self.loadReceiptFromWA();
    self.loadBank();
    self.loadSource();
    self.loadReceiptMode();
};

var mainViewModel = new ChequeInventoryModel();
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

    objDialogyReceipt = $("[id$='digReceiptDet']");
    objDialogyReceipt.dialog({
        autoOpen: false,
        modal: true,
        width: 850,
        height: 750,
        duration: 300
    });

    $(".fsDatePicker").datepicker({
        yearRange: '1900:+nn',
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
    });

    $(".fsDatePicker").keyup(function (e) {
        if (e.keyCode == 8 || e.keyCode == 46)
            $.datepicker._clearDate(this);

        $(this).datepicker('show');
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

    $("#txtCheckerRemarks").keyup(function (e) {
        IsValidSubmit();
        var _data = $("#txtCheckerRemarks").val();
        if (_data.trim() != "") {
            $("#txtCheckerRemarks").removeClass('required').removeClass('valid');
            $("#txtCheckerRemarks").addClass('valid');
        }
        else {
            $("#txtCheckerRemarks").removeClass('valid').removeClass('required');
            $("#txtCheckerRemarks").addClass('required');
        }
    });

    $("#txtAmount").keyup(function (event) {
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

function IsValidSubmit() {
    var _recRemarks = $("#txtCheckerRemarks").val();

    if (_recRemarks.trim() == "") {
        $('#btnApprove').attr('disabled', true);
        $('#btnReject').attr('disabled', true);
    }
    else {
        $('#btnApprove').attr('disabled', false);
        $('#btnReject').attr('disabled', false);
    }
}

function ShowReceiptDetails(IsShow) {
    if (IsShow == "0") {
        $('#digReceiptDet').attr("style", "display:block;");
        objDialogyReceipt.dialog({ title: 'Receipt Entry : Checker' });
        objDialogyReceipt.dialog("open");
    } else {
        objDialogyReceipt.dialog("close");
    }
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