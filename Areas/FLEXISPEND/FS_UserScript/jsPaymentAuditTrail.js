var objDialogyInvoice;
var objDialogCheque;
UrlDet = UrlDet.replace("GetPaymentAuditTrail", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");
var PaymentAuditTrailModel = function () {
    var self = this;
    self.BankArray = ko.observableArray();
    self.PaymentAuditTrail = ko.observableArray();
    self.tmpArray1 = ko.observableArray();
    self.tmpArray2 = ko.observableArray();

    self.InvoiceDetailsArray = ko.observableArray();
    self.ChequeDetailsArray = ko.observableArray();

    self.formatNumber = function (num) {
        return Number(num).toFixed(2);
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

    self.Search = function () {
        var data = {
            PVNo: $("#txtSPVNo").val(),
            ECFNo: $("#txtSECFNo").val(),
            InvoiceNo: $("#txtSInvoiceNo").val(),
            EmpCodeId: $("#hdfSEmpCodeId").val(),
            EmpNameId: $("#hdfSEmpName").val(),
            SuppCodeId: $("#hdfSSupplierId").val(),
            SuppNameId: $("#hdfSSupplierName").val(),
            BankId: $("#ddlSBank").val(),
            ChqNo: $("#txtSChqNo").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetPaymentAuditTrail",
            async: false,
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                self.loadGrid();

                self.tmpArray1.removeAll();
                self.tmpArray2.removeAll();

                var Data1 = "", Data2 = "", Data3 = "", Data4 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");
                    }
                }

                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    self.PaymentAuditTrail(Data2);
                } else if (self.PaymentAuditTrail().length == 0) {
                    jAlert("Sorry no records found!", "Message");
                }

                if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null) {
                    Data3 = JSON.parse(response.Data3);
                    self.tmpArray1(Data3);
                }

                if (response.Data4 != null && response.Data4 != "" && JSON.parse(response.Data4) != null) {
                    Data4 = JSON.parse(response.Data4);
                    self.tmpArray2(Data4);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };
    //------------------------------------Coded By Subburaj 07.05.2016--------------------------------//

    this.DownloadPaymentAuditTrail = function () {

        var PVNo = $("#txtSPVNo").val();
        var ECFNo = $("#txtSECFNo").val();
        var InvoiceNo = $("#txtSInvoiceNo").val();
        var EmpCodeId = $("#hdfSEmpCodeId").val();
        var EmpNameId = $("#hdfSEmpName").val();
        var SuppCodeId = $("#hdfSSupplierId").val();
        var SuppNameId = $("#hdfSSupplierName").val();
        var BankId = $("#ddlSBank").val();
        var ChqNo = $("#txtSChqNo").val();

        ko.utils.postJson(UrlDet + "DownloadPaymentAuditTrail?ECFNo=" + ECFNo + "&InvoiceNo=" + InvoiceNo + "&PVNo=" + PVNo + "&EmpCodeId=" + EmpCodeId + "&EmpNameId=" + EmpNameId + "&SuppCodeId=" + SuppCodeId + "&SuppNameId=" + SuppNameId + "&BankId=" + BankId + "&ChqNo=" + ChqNo);

    }
    //----------------------------------------End-----------------------------------------------------//


    self.viewInvoice = function (item) {
        var tmpArray = new Array();

        if (self.InvoiceDetailsArray() != null) {
            self.InvoiceDetailsArray([]);
        }
        
        tmpArray = ko.utils.arrayFilter(self.tmpArray1(), function (aitem) {
            return item.ecfId == aitem.ecfId;
        });

        self.InvoiceDetailsArray(tmpArray.slice());

        if (self.InvoiceDetailsArray().length == 0) {
            jAlert("Sorry no records found!", "Message");
        } else {
            ShowInvoice();
        }
    };

    self.viewCheque = function (item) {
        var tmpArray = new Array();

        if (self.ChequeDetailsArray() != null) {
            self.ChequeDetailsArray([]);
        }

        tmpArray = ko.utils.arrayFilter(self.tmpArray2(), function (aitem) {
            return item.ecfId == aitem.ecfId && item.InvoiceId == aitem.InvoiceId;
        });

        self.ChequeDetailsArray(tmpArray.slice());

        if (self.ChequeDetailsArray().length == 0) {
            jAlert("Sorry no records found!", "Message");
        } else {
            ShowCheque();
        }
    };

    self.DatatableCall = function () {
        if ($("#gvPaymentAuditTrail > tbody > tr").length == self.PaymentAuditTrail().length) {
            $("#gvPaymentAuditTrail").DataTable({
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

    self.clear = function () {
        $("#txtSPVNo").val("");
        $("#txtSECFNo").val("");
        $("#txtSInvoiceNo").val("");

        $("#hdfSEmpCodeId").val("0");
        $("#txtSEmpCode").val("");

        $("#hdfSEmpName").val("0");
        $("#txtSEmpName").val("");

        $("#hdfSSupplierId").val("0");
        $("#txtSSupplierCode").val("");

        $("#hdfSSupplierName").val("0");
        $("#txtSSupplierName").val("");

        $("#ddlSBank").val(0);
        $("#txtSChqNo").val("");
        
        self.loadGrid();
    };

    self.loadGrid = function () {
        $("#gvPaymentAuditTrail").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();

        self.PaymentAuditTrail.removeAll();
    }

    self.loadGrid();
    self.loadBank();
};

var mainViewModel = new PaymentAuditTrailModel();
ko.applyBindings(mainViewModel);

$(document).ready(function () {
    objDialogyInvoice = $("[id$='ShowInvoice']");
    objDialogyInvoice.dialog({
        autoOpen: false,
        modal: true,
        width: 900,
        height: 500,
        duration: 300
    });

    objDialogCheque = $("[id$='ShowCheque']");
    objDialogCheque.dialog({
        autoOpen: false,
        modal: true,
        width: 700,
        height: 500,
        duration: 300
    });

    //Load Employee Code Auto Complete
    $("#txtSEmpCode").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfSEmpCodeId").val("0");
        }

        $("#txtSEmpCode").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteEmployeeCode",
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
                    url: UrlHelper + "GetAutoCompleteSupplierCode",
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
                    url: UrlHelper + "GetAutoCompleteEmployee",
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
                    url: UrlHelper + "GetAutoCompleteSupplier",
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
});

function ShowInvoice() {
    $('#ShowInvoice').attr("style", "display:block;");

    objDialogyInvoice.dialog({ title: 'Invoice Details' });
    objDialogyInvoice.dialog("open");
    return false;
}

function ShowCheque() {
    $('#ShowCheque').attr("style", "display:block;");

    objDialogCheque.dialog({ title: 'Cheque Details' });
    objDialogCheque.dialog("open");
    return false;
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

function isEvent(evt) {
    return false;
}