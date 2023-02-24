UrlDet = UrlDet.replace("FetchPaymentAdvice", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");

var ChequeInventoryModel = function () {
    var self = this;

    //bank dropdown array
    self.BankArray = ko.observableArray();
    self.ClaimTypeArray = ko.observableArray();
    self.PaymentModeArray = ko.observableArray();

    //load grid array
    self.PaymentAdviceArray = ko.observableArray();

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

    self.ClaimTypeSelectChanged = function (obj, event) {
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

            var chkbit = $("#ddlClaimType").val();
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

        var BatchNo = $("#txtPayBatchNo").val();            
        var PayDateFrom = $("#txtPayDateFrom").val();
        var PayDateTo= $("#txtPayDateTo").val();
        var PVNoFrom= $("#txtPVFrom").val();
        var PVNoTo= $("#txtPVTo").val();
        var ClaimType= $("#ddlClaimType").val();
        var PVAmount= $("#txtPVAmount").val().replace(/,/g, '');
        var EmpNameId= $("#hdfEmpName").val();
        var EmpCodeId= $("#hdfEmpCodeId").val();
        var SuppCodeId= $("#hdfSupplierId").val();
        var SuppNameId= $("#hdfSupplierName").val();
        var PayMode = $("#ddlPayMode").val();
        var Location = $("#hfBranchName").val();
        
        var data = {
            BatchNo: BatchNo,
            PayDateFrom: PayDateFrom,
            PayDateTo: PayDateTo,
            PVNoFrom: PVNoFrom,
            PVNoTo: PVNoTo,
            ClaimType: ClaimType,
            PVAmount: PVAmount,
            EmpNameId: EmpNameId,
            EmpCodeId: EmpCodeId,
            SuppCodeId: SuppCodeId,
            SuppNameId: SuppNameId,
            PayMode: PayMode,
            ViewType: "0",
            Location: Location
        };

        $.ajax({
            type: "post",
            url: UrlDet + "FetchPaymentAdvice",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {

                $("#btnPrint").attr("style", "display:none");
                $("#btnPrintP").attr("style", "display:none");
                  $("#btnMail").attr("style", "display:none");

                $("#gvPaymentAdvice").DataTable({
                    "autoWidth": false,
                    "destroy": true
                }).destroy();
                self.PaymentAdviceArray.removeAll();

                var Data1 = "", Data2 = "";

                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    self.PaymentAdviceArray(Data2);
                }

                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");
                    } else {
                        if (self.PaymentAdviceArray().length == 0) {
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

    self.DatatableCall = function () {
        if ($("#gvPaymentAdvice > tbody > tr").length == self.PaymentAdviceArray().length) {
            $("#gvPaymentAdvice").DataTable({
                "autoWidth": false,
                "destroy": true,
                "scrollX": true,
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

    self.PrintAdvice = function () {
        var data = {
            BatchNo: $("#txtPayBatchNo").val(),            
            PayDateFrom: $("#txtPayDateFrom").val(),
            PayDateTo: $("#txtPayDateTo").val(),
            PVNoFrom: $("#txtPVFrom").val(),
            PVNoTo: $("#txtPVTo").val(),
            ClaimType: $("#ddlClaimType").val(),
            PVAmount: $("#txtPVAmount").val().replace(/,/g, ''),
            EmpNameId: $("#hdfEmpName").val(),
            EmpCodeId: $("#hdfEmpCodeId").val(),
            SuppCodeId: $("#hdfSupplierId").val(),
            SuppNameId: $("#hdfSupplierName").val(),
            PayMode: $("#ddlPayMode").val(),
            ViewType: "1",
            Location: $("#hfBranchName").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "FetchPaymentAdvicePrint",
            data: JSON.stringify(data),
            contentType: "application/json;",
            async: false,
            success: function (response) {
                if (response == "") {
                    var url = UrlDet + 'PrintPreview';
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

    //self.PrintAdvicePurchase = function () {
    //    window.open('../PaymentAdvice/PrintPreview/', "PopUpWindows", 'width=700px, height=500px, top=100,left=100');
    //};

    self.EmailAdvice = function () {
        var data = {
            BatchNo: $("#txtPayBatchNo").val(),
            PayDateFrom: $("#txtPayDateFrom").val(),
            PayDateTo: $("#txtPayDateTo").val(),
            PVNoFrom: $("#txtPVFrom").val(),
            PVNoTo: $("#txtPVTo").val(),
            ClaimType: $("#ddlClaimType").val(),
            PVAmount: $("#txtPVAmount").val().replace(/,/g, ''),
            EmpNameId: $("#hdfEmpName").val(),
            EmpCodeId: $("#hdfEmpCodeId").val(),
            SuppCodeId: $("#hdfSupplierId").val(),
            SuppNameId: $("#hdfSupplierName").val(),
            PayMode: $("#ddlPayMode").val(),
            ViewType: "0",
            Location: $("#hfBranchName").val()
        };

        ShowLoading(true);
        $.ajax({
            type: "post",
            url: UrlDet + "FetchEmailAlert",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                ShowLoading(false);
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");
                    } else {
                            jAlert("Sorry no records found!", "Message");
                       
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                ShowLoading(false);
                //alert(errorThrown);
            }
        });
    };

    self.clearFilter = function () {
        $("#txtPayDateFrom").val("");
        $("#txtPayDateTo").val("");
        $("#txtPayBatchNo").val("");
        $("#txtPVFrom").val("");
        $("#txtPVTo").val("");
        $("#ddlClaimType").val("");
        $("#txtPVAmount").val("");
        $("#hdfEmpCodeId").val("0");
        $("#txtEmpCode").val("");
        $("#hdfEmpName").val("0");
        $("#txtEmpName").val("");
        $("#hdfSupplierId").val("0");
        $("#txtSupplierCode").val("");
        $("#hdfSupplierName").val("0");
        $("#txtSupplierName").val("");
        $("#hfBranchName").val("0");
        $("#txtBranchName").val("");
        $("#ddlPayMode").val($("#ddlPayMode option:first").val());

        $("#txtEmpCode").removeAttr("disabled").removeAttr("style");
        $("#txtEmpName").removeAttr("disabled").removeAttr("style");
        $("#txtSupplierCode").removeAttr("disabled").removeAttr("style");
        $("#txtSupplierName").removeAttr("disabled").removeAttr("style");

        $("#gvPaymentAdvice").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.PaymentAdviceArray.removeAll();
    };

    self.loadGrid = function () {
        $("#gvPaymentAdvice").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.PaymentAdviceArray.removeAll();
    }
    self.loadGrid();

    //load the Bank DropDown.
    self.loadBank();
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

function isNumberAndLetterAndSpace(evt) {
    var key = evt.keyCode;
    return ((key >= 65 && key <= 90) || key == 8 || (key >= 97 && key <= 122) || (key >= 48 && key <= 57) || key == 127 || key == 32);
}

function isEvent(evt) {
    return false;
}

function ShowLoading(IsShow) {
    if (IsShow)
   $('#loadingImg').attr("style", "display:block; text-align:center; padding-top:5px; color:maroon;"); 
    else
    $('#loadingImg').attr("style", "display:none; text-align:center; padding-top:5px; color:maroon;"); 
}