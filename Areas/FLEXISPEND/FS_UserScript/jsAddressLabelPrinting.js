UrlDet = UrlDet.replace("GetAddressLabelPrint", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");

var ChequeInventoryModel = function () {
    var self = this;

    self.ClaimTypeArray = ko.observableArray();
    self.PaymentModeArray = ko.observableArray();

    self.LabelPrintingArray = ko.observableArray();

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

    self.DatatableCall = function () {
        if ($("#gvAddressPrinting > tbody > tr").length == self.LabelPrintingArray().length) {
            $("#gvAddressPrinting").DataTable({
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

    self.checkChange = function (obj, event) {
        if (event.originalEvent) {
            var array = new Array();
            $('#ContentPrintAddress input:checkbox:checked').each(function (index) {
                array[index] = $(this).attr('id');
            });

            var arrayTmp = new Array();
            $('#ContentPrintAddress input:checkbox:visible').each(function (index) {
                arrayTmp[index] = $(this).attr('id');
            });

            var chkBit = array.length == arrayTmp.length ? true : false;
            $("#chkHSelete").prop('checked', chkBit);
        }
    };

    self.loadClaimType = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "LoadClaimTypeWA",
            data: '{}',
            contentType: "application/json;",
            success: function (response) {
                self.ClaimTypeArray(response);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
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

    self.ClaimTypeSelectChanged = function (obj, event) {
        if (event.originalEvent) {
            $("#txtEmployeeCode").removeAttr("disabled").removeAttr("style");
            $("#txtEmployeeName").removeAttr("disabled").removeAttr("style");
            $("#txtSupplierCode").removeAttr("disabled").removeAttr("style");
            $("#txtSupplierName").removeAttr("disabled").removeAttr("style");

            $("#hfEmployeeCode").val("0");
            $("#txtEmployeeCode").val("");
            $("#hfEmployeeName").val("0");
            $("#txtEmployeeName").val("");
            $("#hfSupplierCode").val("0");
            $("#txtSupplierCode").val("");
            $("#hfSupplierName").val("0");
            $("#txtSupplierName").val("");

            var chkbit = $("#ddlClaimType").val();
            if (chkbit == "E") {
                $("#txtSupplierCode").attr("disabled", "disabled");
                $("#txtSupplierName").attr("disabled", "disabled");
                $("#txtSupplierCode").attr("style", "background-color: gainsboro;");
                $("#txtSupplierName").attr("style", "background-color: gainsboro;");
            }

            if (chkbit == "S") {
                $("#txtEmployeeCode").attr("disabled", "disabled");
                $("#txtEmployeeName").attr("disabled", "disabled");
                $("#txtEmployeeCode").attr("style", "background-color: gainsboro;");
                $("#txtEmployeeName").attr("style", "background-color: gainsboro;");
            }
        } else {
        }
    }

    self.Search = function () {
        var data = {
            PVDateFrom: $("#txtPVFrom").val(),
            PVDateTo: $("#txtPVTo").val(),
            PVNo: $("#txtPVNo").val(),
            PVAmount: $("#txtPVAmount").val().replace(/,/g, '') == "" ? "0" : $("#txtPVAmount").val().replace(/,/g, ''),
            EmpCodeId: $("#hfEmployeeCode").val(),
            EmpNameId: $("#hfEmployeeName").val(),
            SuppCodeId: $("#hfSupplierCode").val(),
            SuppNameId: $("#hfSupplierName").val(),
            PayMode: $("#ddlPayMode").val(),
            ClaimType: $("#ddlClaimType").val(),
            BatchNo: $("#txtPaymentBatchNo").val(),
            Location: $("#hfBranchName").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetAddressLabelPrint",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                //self.LabelPrintingArray.removeAll();
                self.loadGrid();

                var Data1 = "", Data2 = "";
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                }
                self.LabelPrintingArray(Data2);
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");
                    } else {
                        if (self.LabelPrintingArray().length == 0) {
                            jAlert("Sorry no records found!", "Message");
                        }
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.Clear = function () {
        $("#txtEmployeeCode").removeAttr("disabled").removeAttr("style");
        $("#txtEmployeeName").removeAttr("disabled").removeAttr("style");
        $("#txtSupplierCode").removeAttr("disabled").removeAttr("style");
        $("#txtSupplierName").removeAttr("disabled").removeAttr("style");

        $("#hfEmployeeCode").val("0");
        $("#txtEmployeeCode").val("");
        $("#hfEmployeeName").val("0");
        $("#txtEmployeeName").val("");
        $("#hfSupplierCode").val("0");
        $("#txtSupplierCode").val("");
        $("#hfSupplierName").val("0");
        $("#txtSupplierName").val("");
        $("#hfBranchName").val("0");
        $("#txtBranchName").val("");

        $("#txtPVFrom").val("");
        $("#txtPVTo").val("");
        $("#txtPVNo").val("");
        $("#txtPVAmount").val("");
        $("#ddlPayMode").val($("#ddlPayMode option:first").val());
        $("#ddlClaimType").val($("#ddlClaimType option:first").val());
        $("#txtPaymentBatchNo").val("");

        self.loadGrid();
        //self.LabelPrintingArray.removeAll();
        $("#chkHSelete").prop('checked', false);
    };

    self.PrintBranchAddress = function () {
        var array = new Array();
        $('#ContentPrintAddress input:checkbox:checked:visible').each(function (index) {
            array[index] = $(this).attr('id');
        });

        var _chkPrintDetails = "";
        $.each(array, function (index) {
            _chkPrintDetails += array[index].split('_')[1] + '|';
        });

        if (_chkPrintDetails.trim() == "") {
            jAlert("Please select a record!", "Message");
            return false;
        }

        var data = {
            PvIds: _chkPrintDetails,
            Status: "Branch Address"
        };

        $.ajax({
            type: "post",
            url: UrlDet + "PrintAddressLable",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                if (response == "") {
                    var url = UrlDet + 'Print';
                    window.open(url, "PopUpWindows", 'width=900px, height=800px, top=100,left=100, scrollbars=yes');
                } else {
                    jAlert("unable to process your request. please try again!", "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.PrintSupplierAddress = function () {
        var array = new Array();
        $('#ContentPrintAddress input:checkbox:checked:visible').each(function (index) {
            array[index] = $(this).attr('id');
        });

        var _chkPrintDetails = "";
        $.each(array, function (index) {
            _chkPrintDetails += array[index].split('_')[1] + '|';
        });

        if (_chkPrintDetails.trim() == "") {
            jAlert("Please select a record!", "Message");
            return false;
        }

        var data = {
            PvIds: _chkPrintDetails,
            Status: "Supplier Address"
        };

        $.ajax({
            type: "post",
            url: UrlDet + "PrintAddressLable",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                if (response == "") {
                    var url = UrlDet + 'Print';
                    window.open(url, "PopUpWindows", 'width=900px, height=800px, top=100,left=100, scrollbars=yes');
                } else {
                    jAlert("unable to process your request. please try again!", "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.Print = function () {
        var count = $("#hdfBranchId").val();
        if (count == "" || count == "0") {
            jAlert("Ensure Branch!", "Message");
            return false;
        }
        count = $("#txtCount").val();
        if (count == "" || count == "0") {
            jAlert("Ensure Count!", "Message");
            return false;
        }
        //var url = '../AddressLabelPrinting/Print?Id=' + $("#hdfBranchId").val() + '&Count=' + $("#txtCount").val();
        var url = UrlDet + 'Print?Id=' + $("#hdfBranchId").val() + '&Count=' + $("#txtCount").val();
        var newwindow = window.open(url, "PopUpWindows", 'width=700px, height=500px, top=100,left=250');
    };

    self.loadGrid = function () {
        $("#gvAddressPrinting").DataTable({
            "autoWidth": false,
            "destroy": true,
            "bFilter": false,
            "bLengthChange": false,
            "bPaginate": false,
            "bInfo": false
        }).destroy();
        self.LabelPrintingArray.removeAll();
    }

    self.loadGrid();

    self.loadClaimType();
    self.loadPaymentMode();
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

    var objDate = new Date();
    var Presentyear = objDate.getFullYear();
    $("#txtPVFrom").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        numberOfMonths: 1,
        maxDate: 'd',
        onSelect: function (selected) {
            $("#txtPVTo").datepicker("option", "minDate", selected)
        }
    });

    $("#txtPVTo").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        numberOfMonths: 1,
        maxDate: 'd',
        onSelect: function (selected) {
            $("#txtPVFrom").datepicker("option", "maxDate", selected)
        }
    });

    //Load Employee Code Auto Complete
    $("#txtEmployeeCode").keyup(function (e) {
        if (e.which != 13) {
            $("#hfEmployeeCode").val("0");
        }

        $("#txtEmployeeCode").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "../Helper/GetAutoCompleteEmployeeCode",
                    data: "{ 'txt' : '" + $("#txtEmployeeCode").val() + "'}",
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
                $("#hfEmployeeCode").val(i.item.val);
                $("#txtEmployeeCode").val(i.item.label);
            }
        });
    });

    $("#txtEmployeeCode").focusout(function () {
        var hdfId = $("#hfEmployeeCode").val();
        var txtCurName = $("#txtEmployeeCode").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtEmployeeCode").val("");
        }
    });

    //Load txtSSupplier Code Auto Complete
    $("#txtSupplierCode").keyup(function (e) {
        if (e.which != 13) {
            $("#hfSupplierCode").val("0");
        }

        $("#txtSupplierCode").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "../Helper/GetAutoCompleteSupplierCode",
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
                $("#hfSupplierCode").val(i.item.val);
                $("#txtSupplierCode").val(i.item.label);
            }
        });
    });

    $("#txtSupplierCode").focusout(function () {
        var hdfId = $("#hfSupplierCode").val();
        var txtCurName = $("#txtSupplierCode").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtSupplierCode").val("");
        }
    });

    //Load Employee Name Auto Complete
    $("#txtEmployeeName").keyup(function (e) {
        if (e.which != 13) {
            $("#hfEmployeeName").val("0");
        }

        $("#txtEmployeeName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "../Helper/GetAutoCompleteEmployee",
                    data: "{ 'txt' : '" + $("#txtEmployeeName").val() + "'}",
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
                $("#hfEmployeeName").val(i.item.val);
                $("#txtEmployeeName").val(i.item.label);
            }
        });
    });

    $("#txtEmployeeName").focusout(function () {
        var hdfId = $("#hfEmployeeName").val();
        var txtCurName = $("#txtEmployeeName").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtEmployeeName").val("");
        }
    });

    //Load Supplier Name Auto Complete
    $("#txtSupplierName").keyup(function (e) {
        if (e.which != 13) {
            $("#hfSupplierName").val("0");
        }

        $("#txtSupplierName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "../Helper/GetAutoCompleteSupplier",
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
                $("#hfSupplierName").val(i.item.val);
                $("#txtSupplierName").val(i.item.label);
            }
        });
    });

    $("#txtSupplierName").focusout(function () {
        var hdfId = $("#hfSupplierName").val();
        var txtCurName = $("#txtSupplierName").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtSupplierName").val("");
        }
    });

    //Load Branch Name Auto Complete
    $("#txtBranchName").keyup(function (e) {
        if (e.which != 13) {
            $("#hfBranchName").val("0");
        }

        $("#txtBranchName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteBranchPouch",
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
                        //alert(response.responseText);
                    },
                    failure: function (response) {
                        //alert(response.responseText);
                    }
                });
            },
            minLength: 1,
            select: function (e, i) {
                $("#hfBranchName").val(i.item.val);
                $("#txtBranchName").val(i.item.label);
            }
        });
    });

    $("#txtBranchName").focusout(function () {
        var hdfId = $("#hfBranchName").val();
        var txtCurName = $("#txtBranchName").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtBranchName").val("");
        }
    });

    $("#chkHSelete").click(function (e) {
        $(this).closest('table').find('td input:checkbox:visible').prop('checked', this.checked);
    });

    $("#txtPVAmount").keyup(function (event) {
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

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

function isEvent(evt) {
    return false;
}

function checkValidationSearch() {
    var hdfId = $("#hdfBranchId").val();
    var _count = $("#txtCount").val();

    if ((_count.trim() == "" || _count.trim() == "0") || (hdfId.trim() == "" || hdfId.trim() == "0")) {
        $('#btnsearch').attr('disabled', true);
    }
    else {
        $('#btnsearch').attr('disabled', false);
    }
}