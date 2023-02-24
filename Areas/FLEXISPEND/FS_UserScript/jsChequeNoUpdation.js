var objDialogyAdd;
var objDialogyCancel;

UrlDet = UrlDet.replace("GetChequeNoUpdation", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");

var ChequeInventoryModel = function () {
    var self = this;

    //bank dropdown array
    self.BankArray = ko.observableArray();

    //load grid array
    self.ChequeNoUpdationArray = ko.observableArray();

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
            PaymentDate: $("#txtDate").val(),
            BatchNo: $("#txtPayBatchNo").val(),
            SuppNameId: $("#hdfSupplierName").val(),
            EmpNameId: $("#hdfEmpName").val(),
            ViewType: "0"
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetChequeNoUpdation",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {

                $("#gvChequeNoUpdation").DataTable({
                    "autoWidth": false,
                    "destroy": true,
                    "bFilter": false,
                    "bLengthChange": false,
                    "bPaginate": false,
                    "bInfo": false
                }).destroy();
                self.ChequeNoUpdationArray.removeAll();

                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");
                    }
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    self.ChequeNoUpdationArray(Data2);
                } else {
                    jAlert("Sorry! No records found.", "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.create = function () {
        var _bankId = "", _pvNoFrom = "", _pvNoTo = "", _chqBookNo = "", _chqNoFrom = "", _chqNoTo = "";

        _ChqDate = $("#txtChequeDate").val();
        _bankId = $("#ddlBank").val();
        _pvNoFrom = $("#txtPVNoFrm").val();
        _pvNoTo = $("#txtPVNoTo").val();
        _chqBookNo = $("#txtCHQBookNo").val();
        _chqNoFrom = $("#txtCHQNOFrm").val();
        _chqNoTo = $("#txtCHQNoTo").val();

        if (_pvNoFrom == "" || _pvNoFrom == "0") {
            jAlert("Ensure PV No From!", "Message");
            return false;
        }
        if (_pvNoTo == "" || _pvNoTo == "0") {
            jAlert("Ensure PV No To!", "Message");
            return false;
        }
        if (_bankId == "" || _bankId == "0") {
            jAlert("Ensure Bank!", "Message");
            return false;
        }
        if (_chqBookNo == "" || _chqBookNo == "0") {
            jAlert("Ensure Cheque Book No!", "Message");
            return false;
        }
        if (_chqNoFrom == "" || _chqNoFrom == "0") {
            jAlert("Ensure Cheque No From!", "Message");
            return false;
        }
        if (_chqNoTo == "" || _chqNoTo == "0") {
            jAlert("Ensure Cheque No To!", "Message");
            return false;
        }

        var data = {
            ChqDate: _ChqDate,
            BankId: _bankId,
            ChqBookNo: _chqBookNo,
            ChqNoFrom: _chqNoFrom,
            ChqNoTo: _chqNoTo,
            PVNoFrom: _pvNoFrom,
            PVNoTo: _pvNoTo,
            Reason: "",
            ViewType: "0"
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetChequeNoUpdation",
            data: JSON.stringify(data),
            contentType: "application/json;",
            async: false,
            success: function (response) {
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Message != "") {
                        jConfirm(Data1[0].Message, "Message", function (callback) {
                            if (callback == true) {
                                if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {
                                    $("#txtSupplierName").val("");
                                    $("#txtEmpName").val("");
                                    $("#hdfSupplierName").val("0");
                                    $("#hdfEmpName").val("0");
                                    $("#txtPayBatchNo").val("");
                                    $("#txtDate").val(_ChqDate);
                                   
                                    $('#ViewShowDialog').attr("style", "display:none;");
                                    objDialogyAdd.dialog("close");

                                    self.Search();
                                }
                            }
                        });
                        //remove the cancel button from dialog box
                        $("#popup_ok").attr("style", "margin-left: 50px;");
                        $("#popup_cancel").attr("style", "visibility: hidden");
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });

    };

    self.CancelCheque = function () {
        var data = {
            ChqDate: "",
            BankId: $("#lblBankId").val(),
            ChqBookNo: $("#lblCHQBookNo").text(),
            ChqNoFrom: $("#lblCHQNo").text(),
            ChqNoTo: "",
            PVNoFrom: $("#lblPVNo").text(),
            PVNoTo: "",
            Reason: $("#txtReason").val(),
            ViewType: "1"
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetChequeNoUpdation",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Message != "") {
                        jConfirm(Data1[0].Message, "Message", function (callback) {
                            if (callback == true) {
                                if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {
                                    
                                    $('#ShowDialogCancel').attr("style", "display:none;");
                                    objDialogyCancel.dialog("close");
                                    self.Search();
                                }
                            }
                        });
                        //remove the cancel button from dialog box
                        $("#popup_ok").attr("style", "margin-left: 50px;");
                        $("#popup_cancel").attr("style", "visibility: hidden");
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.OpenCancel = function (item) {
        $("#lblPVId").val(item.PvId);
        $("#lblBankId").val(item.BankId);

        $("#lblPVNo").text(item.PvNo);
        $("#lblPaymentDate").text(item.PvDate);
        $("#lblBankDet").text(item.Bank);
        $("#lblCHQBookNo").text(item.ChqbookNo);
        $("#lblCHQNo").text(item.ChqNo);
        $("#lblCHQDate").text(item.ChqDate==null ? "" : item.ChqDate);
        $("#txtReason").val("");

        $("#txtReason").removeClass('required').removeClass('valid');
        $("#txtReason").addClass('required');

        CHQBookCancelEntry();
        if (item.ChqNo != null && item.ChqNo != "") {
            $('#ShowDialogCancel').attr("style", "display:block;");
            objDialogyCancel.dialog({ title: 'Cancel Cheque' });
            objDialogyCancel.dialog("open");
        } else {
            jAlert("Cheque Number not allocated!", "Message");
        }
    };

    self.AddCheque = function () {
        //$("#ddlBank").val(""),
        $("#txtCHQBookNo").val("");
        $("#txtCHQNOFrm").val("");
        $("#txtCHQNoTo").val("");
        $("#txtPVNoFrm").val("");
        $("#txtPVNoTo").val("");
        $("#txtChequeDate").val("");

        $("#txtChequeDate").removeClass('required').removeClass('valid');
        $("#txtCHQBookNo").removeClass('required').removeClass('valid');
        $("#txtCHQNOFrm").removeClass('required').removeClass('valid');
        $("#txtCHQNoTo").removeClass('required').removeClass('valid');
        $("#txtPVNoFrm").removeClass('required').removeClass('valid');
        $("#txtPVNoTo").removeClass('required').removeClass('valid');

        $("#txtChequeDate").addClass('required');
        $("#txtCHQBookNo").addClass('required');
        $("#txtCHQNOFrm").addClass('required');
        $("#txtCHQNoTo").addClass('required');
        $("#txtPVNoFrm").addClass('required');
        $("#txtPVNoTo").addClass('required');

        CHQBookEntry();

        $('#ViewShowDialog').attr("style", "display:block;");
        objDialogyAdd.dialog({ title: 'Cheque Updation' });
        objDialogyAdd.dialog("open");
    };

    // Cancel Cheque no updation
    self.ClearSearch = function () {
        $("#txtSupplierName").val("");
        $("#txtEmpName").val("");
        $("#hdfSupplierName").val("0");
        $("#hdfEmpName").val("0");
        $("#txtPayBatchNo").val("");
        $("#txtDate").val("");

        self.loadGrid();
    }

    self.DatatableCall = function () {
        if ($("#gvChequeNoUpdation > tbody > tr").length == self.ChequeNoUpdationArray().length) {
            $("#gvChequeNoUpdation").DataTable({
                "autoWidth": false,
                "destroy": true,
                "bFilter": false,
                "bLengthChange": false,
                "bPaginate": false,
                "bInfo": false,
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
        $("#gvChequeNoUpdation").DataTable({
            "autoWidth": false,
            "destroy": true,
            "bFilter": false,
            "bLengthChange": false,
            "bPaginate": false,
            "bInfo": false
        }).destroy();
        self.ChequeNoUpdationArray.removeAll();
    }

    self.loadGrid();

    //load the Bank DropDown.
    self.loadBank();
};

var mainViewModel = new ChequeInventoryModel();
ko.applyBindings(mainViewModel);


$(document).ready(function () {
    objDialogyAdd = $("[id$='ShowDialog']");
    objDialogyAdd.dialog({
        autoOpen: false,
        modal: true,
        width: 500,
        height: 275,
        duration: 300
    });

    objDialogyCancel = $("[id$='ShowDialogCancel']");
    objDialogyCancel.dialog({
        autoOpen: false,
        modal: true,
        width: 500,
        height: 350,
        duration: 300
    });


    var objDate = new Date();
    var Presentyear = objDate.getFullYear();
    $(".fsDatePicker").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
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
                    url: UrlHelper + "GetAutoCompleteEmployee",
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

    CHQBookEntry();

    //popup validation
    $("#txtChequeDate").change(function () {
        CHQBookEntry();

        var _data = $("#txtChequeDate").val();
        if (_data.trim() != "") {
            $("#txtChequeDate").removeClass('required');
            $("#txtChequeDate").addClass('valid');
        }
        else {
            $("#txtChequeDate").removeClass('valid');
            $("#txtChequeDate").addClass('required');
        }
    });

    $("#txtPVNoFrm").change(function () {
        CHQBookEntry();

        var _data = $("#txtPVNoFrm").val();
        if (_data.trim() != "") {
            $("#txtPVNoFrm").removeClass('required');
            $("#txtPVNoFrm").addClass('valid');
        }
        else {
            $("#txtPVNoFrm").removeClass('valid');
            $("#txtPVNoFrm").addClass('required');
        }
    });

    $("#txtPVNoTo").change(function () {
        CHQBookEntry();

        var _data = $("#txtPVNoTo").val();
        if (_data.trim() != "") {
            $("#txtPVNoTo").removeClass('required');
            $("#txtPVNoTo").addClass('valid');
        }
        else {
            $("#txtPVNoTo").removeClass('valid');
            $("#txtPVNoTo").addClass('required');
        }
    });

    $("#txtCHQBookNo").change(function () {
        CHQBookEntry();

        var _data = $("#txtCHQBookNo").val();
        if (_data.trim() != "") {
            $("#txtCHQBookNo").removeClass('required');
            $("#txtCHQBookNo").addClass('valid');
        }
        else {
            $("#txtCHQBookNo").removeClass('valid');
            $("#txtCHQBookNo").addClass('required');
        }
    });

    $("#txtCHQNOFrm").change(function () {
        CHQBookEntry();

        var _data = $("#txtCHQNOFrm").val();
        if (_data.trim() != "") {
            $("#txtCHQNOFrm").removeClass('required');
            $("#txtCHQNOFrm").addClass('valid');
        }
        else {
            $("#txtCHQNOFrm").removeClass('valid');
            $("#txtCHQNOFrm").addClass('required');
        }
    });

    $("#txtCHQNoTo").change(function () {
        CHQBookEntry();

        var _data = $("#txtCHQNoTo").val();
        if (_data.trim() != "") {
            $("#txtCHQNoTo").removeClass('required');
            $("#txtCHQNoTo").addClass('valid');
        }
        else {
            $("#txtCHQNoTo").removeClass('valid');
            $("#txtCHQNoTo").addClass('required');
        }
    });

    $("#txtReason").change(function () {
        CHQBookCancelEntry();

        var _data = $("#txtReason").val();
        if (_data.trim() != "") {
            $("#txtReason").removeClass('required');
            $("#txtReason").addClass('valid');
        }
        else {
            $("#txtReason").removeClass('valid');
            $("#txtReason").addClass('required');
        }
    });
});

function CHQBookEntry() {
    var _PVNoFrm = $("#txtPVNoFrm").val();;
    var _PVNoTo = $("#txtPVNoTo").val();
    var _CHQBookNo = $("#txtCHQBookNo").val();
    var _CHQNOFrm = $("#txtCHQNOFrm").val();
    var _CHQNoTo = $("#txtCHQNoTo").val();
    var _bank = $("#ddlBank").val();
    var _ChqDate = $("#txtChequeDate").val();

    if (_PVNoFrm == "" || _PVNoFrm == "0" || _PVNoTo == "" || _PVNoTo == "0" || _CHQBookNo == "" || _CHQBookNo == "0"
        || _CHQNOFrm == "" || _CHQNOFrm == "0" || _CHQNoTo == "" || _CHQNoTo == "0" || _bank == "" || _bank == "0" || _ChqDate.trim() == "") {
        $('#btnUpdate').attr('disabled', true);
    } else {
        $('#btnUpdate').attr('disabled', false);
    }
}

function CHQBookCancelEntry() {
    var _reason = $("#txtReason").val();

    if (_reason.trim() == "") {
        $('#btnCancel').attr('disabled', true);
    } else {
        $('#btnCancel').attr('disabled', false);
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