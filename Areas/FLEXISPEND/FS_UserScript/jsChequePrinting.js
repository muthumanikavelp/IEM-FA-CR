UrlDet = UrlDet.replace("FetchChequePrinting", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");
var ChequeInventoryModel = function () {
    var self = this;

    //bank dropdown array
    self.BankArray = ko.observableArray();
    self.ClaimTypeArray = ko.observableArray();

    //load grid array
    self.ChequePrintingArray = ko.observableArray();

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

    self.Search = function () {
        var data = {
            BatchNo: $("#txtPaymentBatchNo").val(),
            PayDateFrom: $("#txtPayDateFrom").val(),
            PayDateTo: $("#txtPayDateTo").val(),
            PVNoFrom: $("#txtPVFrmNo").val(),
            PVNoTo: $("#txtPVTo").val(),
            ClaimType: $("#ddlClaimType").val(),
            PVAmountFrom: $("#txtFPVAmount").val().replace(/,/g, ''),
            PVAmountTo: $("#txtTPVAmount").val().replace(/,/g, ''),
            EmpNameId: $("#hdfEmpName").val(),
            EmpCodeId: $("#hdfEmpCodeId").val(),
            SuppCodeId: $("#hdfSupplierId").val(),
            SuppNameId: $("#hdfSupplierName").val()
        };
        $.ajax({
            type: "post",
            url:  UrlDet + "FetchChequePrinting",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                $("#chkHSelete").prop('checked', false);
                $("#gvChequePrinting").DataTable({
                    "autoWidth": false,
                    "destroy": true,
                    "bFilter": false,
                    "bLengthChange": false,
                    "bPaginate": false,
                    "bInfo": false
                }).destroy();
                self.ChequePrintingArray.removeAll();
                $("#lblTotalCount").text("0");
                self.reset();
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");
                    }

                    if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                        Data2 = JSON.parse(response.Data2);
                        self.ChequePrintingArray(Data2);
                    }

                    if (self.ChequePrintingArray().length == 0 && Data1[0].Message == "") {
                        jAlert("Sorry! No Records Found.", "Message");
                    }
                }
                
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.checkChange = function (obj, event) {
        if (event.originalEvent) {
            var array = new Array();
            $('#contentChequePrinting input:checkbox:checked').each(function (index) {
                array[index] = $(this).attr('id');
            });
            $("#lblTotalCount").text(array.length);
            
            var chkBit = array.length == self.ChequePrintingArray().length ? true : false;
            $("#chkHSelete").prop('checked', chkBit);
            
        }
    };

    self.PrintCheque = function () {
        var array = new Array();
        $('#contentChequePrinting input:checkbox:checked').each(function (index) {
            array[index] = $(this).attr('id');
        });

        var _chkPrintDetails = "";
        $.each(array, function (index) {
            _chkPrintDetails += array[index].split('_')[1] + '|';
        });

        var count = $("#lblTotalCount").text();
        if (count == "" || count == "0" || _chkPrintDetails.trim() == "") {
            jAlert("Please select a record!", "Error");
            return false;
        }
        
        var data = {
            ChqDetails: _chkPrintDetails,
            BankId: $("#ddlBank").val(),  
            ChqBookNo: $("#txtCHQBookNo").val(),  
            ChqNoFrom: $("#txtCHQNOFrm").val(),  
            ChqNoTo: $("#txtCHQNoTo").val(),  
            ChqCount: count
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetChequePrinting",
            data: JSON.stringify(data),
            contentType: "application/json;",
            async: false,
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    
                    if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {
                        jConfirm(Data1[0].Message, "Message", function (callback) {
                            if (callback == true) {
                                window.open(UrlDet + 'PrintCheque/', "PopUpWindows", 'scrollbars=yes, width=700px, height=500px, top=100,left=100');
                                //self.TriggerPrint();
                                self.clearFilter();
                            }
                        });

                        //remove the cancel button from dialog box
                        $("#popup_ok").attr("style", "margin-left: 50px;");
                        $("#popup_cancel").attr("style", "visibility: hidden");
                    }

                    //show Message if error
                    if (Data1[0].Message != "" && (Data1[0].Clear == "false" || Data1[0].Clear == "False" || Data1[0].Clear == "0")) {
                        jAlert(Data1[0].Message, "Message");
                    }
                } else {
                    jAlert("unable to process your request. please try again!", "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.TriggerPrint = function () {
        $.ajax({
            type: "post",
            url: UrlDet + "PrintChequeList",
            data: '{}',
            contentType: "application/json;",
            success: function (response) {
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    };

    self.clearFilter = function () {
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

        $("#ddlClaimType").val("");
        $("#txtPaymentBatchNo").val("");
        $("#txtPVFrmNo").val("");
        $("#txtPVTo").val("");
        $("#txtFPVAmount,#txtTPVAmount").val("");
        $("#txtPayDateTo").val("");
        $("#txtPayDateFrom").val("");
        
        $("#lblTotalCount").text("0");
        $("#chkHSelete").prop('checked', false);
        self.reset();

        $("#gvChequePrinting").DataTable({
            "autoWidth": false,
            "destroy": true,
            "bFilter": false,
            "bLengthChange": false,
            "bPaginate": false,
            "bInfo": false
        }).destroy();
        self.ChequePrintingArray.removeAll();
    };

    self.reset = function () {         
        $("#txtCHQBookNo").val("");
        $("#txtCHQNOFrm").val("");
        $("#txtCHQNoTo").val("");

        $("#txtCHQBookNo").removeClass('valid').removeClass('required');
        $("#txtCHQNOFrm").removeClass('valid').removeClass('required');
        $("#txtCHQNoTo").removeClass('valid').removeClass('required');

        $("#txtCHQBookNo").addClass('required');
        $("#txtCHQNOFrm").addClass('required');
        $("#txtCHQNoTo").addClass('required');
        $('#btnSaveChecque').attr('disabled', true);
    };

    self.DatatableCall = function () {
        if ($("#gvChequePrinting > tbody > tr").length == self.ChequePrintingArray().length) {
            $("#gvChequePrinting").DataTable({
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
        $("#gvChequePrinting").DataTable({
            "autoWidth": false,
            "destroy": true,
            "bFilter": false,
            "bLengthChange": false,
            "bPaginate": false,
            "bInfo": false
        }).destroy();
        self.ChequePrintingArray.removeAll();
    }

    self.loadGrid();

    //load the Bank DropDown.
    self.loadBank();
    self.loadClaimType();
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

    $('#btnSaveChecque').attr('disabled', true);

    var objDate = new Date();
    var Presentyear = objDate.getFullYear();
    $(".fsDatePicker").datepicker({
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

    //cheque Book Entry Validation
    $("#txtCHQBookNo").keyup(function () {
        CHQBookEntry();
        var _ECHQBookNo = $("#txtCHQBookNo").val();
        if (_ECHQBookNo.trim() != "") {
            $("#txtCHQBookNo").removeClass('required');
            $("#txtCHQBookNo").addClass('valid');
        }
        else {
            $("#txtCHQBookNo").removeClass('valid');
            $("#txtCHQBookNo").addClass('required');
        }
    });

    $("#txtCHQNOFrm").keyup(function () {
        CHQBookEntry();
        var _ECHQNoFrom = $("#txtCHQNOFrm").val();
        if (_ECHQNoFrom.trim() != "") {
            $("#txtCHQNOFrm").removeClass('required');
            $("#txtCHQNOFrm").addClass('valid');
        }
        else {
            $("#txtCHQNOFrm").removeClass('valid');
            $("#txtCHQNOFrm").addClass('required');
        }
    });

    $("#txtCHQNoTo").keyup(function () {
        CHQBookEntry();
        var _ECHQNoTo = $("#txtCHQNoTo").val();
        if (_ECHQNoTo.trim() != "") {
            $("#txtCHQNoTo").removeClass('required');
            $("#txtCHQNoTo").addClass('valid');
        }
        else {
            $("#txtCHQNoTo").removeClass('valid');
            $("#txtCHQNoTo").addClass('required');
        }
    });

    $("#chkHSelete").click(function (e) {
        $(this).closest('table').find('td input:checkbox').prop('checked', this.checked);

        var array = new Array();
        $('#contentChequePrinting input:checkbox:checked').each(function (index) {
            array[index] = $(this).attr('id');
        });
        $("#lblTotalCount").text(array.length);
    });

    $("#txtFPVAmount,#txtTPVAmount").keyup(function (event) {
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

function CHQBookEntry() {
    var _ECHQBookNo = $("#txtECHQBookNo").val();
    var _ECHQNoFrom = $("#txtCHQNOFrm").val();
    var _ECHQNoTo = $("#txtCHQNoTo").val();

    if (_ECHQBookNo == "" || _ECHQNoFrom == "" || _ECHQNoTo == "") {
        $('#btnSaveChecque').attr('disabled', true);
    } else {
        $('#btnSaveChecque').attr('disabled', false);
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