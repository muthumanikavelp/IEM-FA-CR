var objDialogStale;
UrlDet = UrlDet.replace("GetStaleChequeMaker", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");
var PettyCashModel = function () {
    var self = this;
    self.StaleChequeArray = ko.observableArray();

    self.DocumentTypeArray = ko.observableArray();
    self.BankArray = ko.observableArray();
    self.PayModeArray = ko.observableArray();
  //  self.PayModeDetails = ko.obsetvableArray();

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

    self.loadDocType = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "MasterDocumentType",
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

    //Muthu 16-12-2016
    self.loadPaymode = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "LoadPayModeDropDown",
            data: '{}',
            contentType: "application/json;",
            success: function (response) {

                self.PayModeArray(response);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    //search filter
    self.search = function () {
        var data = {
            ChqDateFrom: $("#txtChequeFrom").val(),
            ChqDateTo: $("#txtChequeTo").val(),
            ChqNo: $("#txtChequeNo").val(),
            DocAmount: $("#txtDocAmount").val().replace(/,/g, ''),
            SuppCodeId: $("#hdfSupplierCode").val(),
            SuppNameId: $("#hdfSupplierName").val(),
            EmpCodeId: $("#hdfEmpCodeId").val(),
            EmpNameId: $("#hdfEmpName").val(),
            DocTypeId: $("#ddlDocType").val(),
            DocNo: $("#txtDocNo").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetStaleChequeMaker",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                self.loadGrid();

                var Data1 = "", Data2 = "";
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);

                    self.StaleChequeArray(Data2);
                }
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    if (Data1[0].Message.length > 0) {
                        jAlert(Data1[0].Message, "Message");
                    } else if (self.StaleChequeArray().length == 0) {
                        jAlert("Sorry no records found!", "Message");
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    };

    self.checkChange = function (obj, event) {
        if (event.originalEvent) {
            var array = new Array();
            $('#ContentStaleCheque input:checkbox:checked').each(function (index) {
                array[index] = $(this).attr('id');
            });

            var chkBit = array.length == self.StaleChequeArray().length ? true : false;
            $("#chkHSelete").prop('checked', chkBit);
        }
    };

    self.Reissue = function (item) {
        //alert();
        $("#ddlEPUPaymode").val("");
        self.loadPaymode();
        $("#hdfPvId").val(item.PvId);
        $("#txtPUDocDate").val(item.DocDate);
        $("#txtPUDocType").val(item.DocType);
        $("#txtPUDocNo").val(item.DocNo);
        $("#txtPUDocAmount").val(self.formatNumber(item.DocAmount));
        $("#txtPUSEDetail").val(item.EmployeeSupplierName);

        $("#txtPUPVDate").val(item.PvDate);
        $("#txtPUPVNo").val(item.PvNo);
        $("#txtPUMovedDate").val(item.StaleMovedDate);
        $("#txtPUChqDate").val(item.ChqDate);
        $("#txtPUChqNo").val(item.ChqNo);
        $("#txtPUChqAmount").val(self.formatNumber(item.ChqAmount));
        $("#txtPUBankName").val(item.Bank);

        $('.entry').removeClass('required').removeClass('valid');
        $('.entry').addClass('required');

        var _Bank = $("#ddlEPUBankName").val();
        if (_Bank == "" || _Bank == "0") {
            $("#ddlEPUBankName").removeClass('valid');
            $("#ddlEPUBankName").addClass('required');
        }
        else {
            $("#ddlEPUBankName").removeClass('required');
            $("#ddlEPUBankName").addClass('valid');
        }

        var _Paymode = $("#ddlEPUPaymode").val();
        if (_Paymode == "" || _Paymode == "0") {
            $("#ddlEPUPaymode").removeClass('valid');
            $("#ddlEPUPaymode").addClass('required');
        }
        else {
            $("#ddlEPUPaymode").removeClass('required');
            $("#ddlEPUPaymode").addClass('valid');
        }

        $("#txtEPUAccountNo").val("");
        $("#txtEPUBankname").val("");
        $("#txtEPUChqBookNo").val("");
        $("#txtEPUChqNo").val("");
        $("#txtEPUChqDate").val("");
        $("#txtEPURemarks").val("");
        $("#txtEPUBenificary").val("");

        // IsValidSubmit();
        IsValidSubmitstale_M();
        ShowDialogStale();
    };


    self.Changed = function (item) {
     
        debugger
        var _Paymode = $("#ddlEPUPaymode").val();
        if (_Paymode == "CHQ")
        {
            $("#txtEPUChqBookNo").disabled;
            $("#txtEPUChqNo").disabled;
            $("#txtEPUChqDate").disabled;
        }
    };


    self.selectionChanged = function (item) {
        debugger
        var paymode = $("#ddlEPUPaymode").val();
        var ecfno = $("#txtPUDocNo").val();
        if (paymode != "-Select-")
        {
            $("#ddlEPUPaymode").removeClass('valid');
            $("#ddlEPUPaymode").addClass('required');
        }
        else {
            $("#ddlEPUPaymode").removeClass('required');
            $("#ddlEPUPaymode").addClass('valid');
        }
        var data = {
            PvId: $("#ddlEPUPaymode").val(),//Paymode
            ChqNo: $("#txtPUDocNo").val(),//ECFNO
            Remarks: $("#txtPUSEDetail").val(),
            ChqBookNo: $("#txtPUPVNo").val() //pvno
        };
        $.ajax({
            type: "post",
            url: UrlDet + "StalePaymodedetails",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                debugger
                var Data1 = "",Data2 = "",Data3 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    var Acc = Data1[0].accountno;
                    var bank = Data1[0].bankname;
                    var benificiary = Data1[0].benificiary;
                    var Msg = Data1[0].Message;
                    if (Acc != null && Acc != "" && Acc != "undefined" && bank != null && bank != "" && Acc != "undefined") {

                        $("#txtEPUAccountNo").enabled;
                        $("#txtEPUBankname").enabled;
                        $("#txtEPUBenificary").enabled;
                        $("#txtEPUAccountNo").val("");
                        $("#txtEPUBankname").val("");
                        $("#txtEPUBenificary").val("");
                        $("#txtEPUAccountNo").val(Acc);
                        $("#txtEPUBankname").val(bank);
                       // alert('A');
                        IsValidSubmitstale_M();
                        //alert('A');
                        

                    }
                    else if (benificiary != null && benificiary != "" && benificiary != "undefined")
                    {
                        $("#txtEPUBenificary").enabled;
                        $("#txtEPUAccountNo").enabled;
                        $("#txtEPUBankname").enabled;
                        $("#txtEPUAccountNo").val("");
                        $("#txtEPUBankname").val("");
                        $("#txtEPUBenificary").val("");
                        $("#txtEPUBenificary").val(benificiary);
                        IsValidSubmitstale_M();
                    }
                    else if (Msg != null && Msg !="" && Msg != "undefined") {
                        IsValidSubmitstale_M();
                        $("#txtEPUAccountNo").val("");
                        $("#txtEPUBankname").val("");
                        $("#txtEPUBenificary").val("");
                        jAlert(Msg, "Message");
                    }
                
                }
                
                    
               // IsValidSubmitstale_M();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.Submit = function () {
        var data = {
            //PvId: $("#hdfPvId").val(),
            //BankId: $("#ddlEPUBankName").val(),
            //ChqBookNo: $("#txtEPUChqBookNo").val(),
            //ChqNo: $("#txtEPUChqNo").val(),
            //ChqDate: $("#txtEPUChqDate").val(),
            //Remarks: $("#txtEPURemarks").val(),

            PvId: $("#txtPUPVNo").val(),//PVNO
            BankId: $("#txtEPUBankname").val(),//Bank_NAME
            ChqBookNo: $("#txtEPUAccountNo").val(),//ACCOUNTNO
            ChqNo: $("#txtEPUBenificary").val(),//BEFICIARYNAME
            ChqDate: $("#ddlEPUPaymode").val(),//Paymode
            Remarks: $("#txtEPURemarks").val(),
          
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetStaleChequeMaker",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";

                if (response.Data1 == "Unauthorized User!") {
                    jAlert("Unauthorized User!");
                    return false;
                }
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message
                    if (Data1[0].Clear == "True" || Data1[0].Clear == "true" || Data1[0].Clear == "1") {

                        objDialogStale.dialog("close");
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
                } else {
                    jAlert("unable to process your request. please try again!", "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    //Clear the form Entry
    self.clearFilter = function () {
        $("#hdfPvId").val("0");
        $("#txtChequeFrom").val("");
        $("#txtChequeTo").val("");
        $("#txtChequeNo").val("");
        $("#txtDocAmount").val("");

        $("#ddlDocType").val("");
        $("#txtDocNo").val("");

        $("#hdfEmpCodeId").val("0");
        $("#txtEmpCode").val("");
        $("#hdfEmpName").val("0");
        $("#txtEmpName").val("");

        $("#hdfSupplierCode").val("0");
        $("#txtSupplierCode").val("");
        $("#hdfSupplierName").val("0");
        $("#txtSupplierName").val("");

        $("#chkHSelete").prop('checked', false);
        self.loadGrid();
    };

    //Datatable Plugin codeing
    self.DatatableCall = function () {
        if ($("#gvStaleCheque > tbody > tr").length == self.StaleChequeArray().length) {
            $("#gvStaleCheque").DataTable({
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
        $("#gvStaleCheque").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.StaleChequeArray.removeAll();
    }
    self.loadGrid();

    self.loadDocType();
    self.loadBank();
    self.loadPaymode();
};

var mainViewModel = new PettyCashModel();
ko.applyBindings(mainViewModel);

$(document).ready(function () {
    objDialogStale = $("[id$='DialogStale']");
    objDialogStale.dialog({
        autoOpen: false,
        modal: true,
        width: 800,
        height: 650,
        duration: 300
    });

    $(".fsDatepicker").datepicker({
        yearRange: '1900:+nn',
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
    });

    $(".fsDatepicker").keyup(function (e) {
        if (e.keyCode == 8 || e.keyCode == 46) {
            $.datepicker._clearDate(this);
        }
        $(this).datepicker('show');
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

    //Load Supplier Code Auto Complete
    $("#txtSupplierCode").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfSupplierCode").val("0");
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
                $("#hdfSupplierCode").val(i.item.val);
                $("#txtSupplierCode").val(i.item.label);
            }
        });
    });

    $("#txtSupplierCode").focusout(function () {
        var hdfId = $("#hdfSupplierCode").val();
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

    $("#chkHSelete").click(function (e) {
        $(this).closest('table').find('td input:checkbox').prop('checked', this.checked);
    });

    $("#txtDocAmount").keyup(function (event) {
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

    $('.entry').change(function () {
        //IsValidSubmit();
        
        IsValidSubmitstale_M
        var _data = $(this).val();
        var trigerName = $(this).attr('id');
        
        if (_data.trim() != "") {
            $("#" + trigerName).removeClass('required');
            $("#" + trigerName).addClass('valid');
        }
        else {
            $("#" + trigerName).removeClass('valid');
            $("#" + trigerName).addClass('required');
        }
    });

});

//function IsValidSubmit() {
//    var _Bank = $("#ddlEPUBankName").val();
//    var _ChqBook = $("#txtEPUChqBookNo").val();
//    var _ChqNo = $("#txtEPUChqNo").val();
//    var _ChqDate = $("#txtEPUChqDate").val();
//    var _Remarks = $("#txtEPURemarks").val();

//    if (_ChqBook.trim() == "" || _ChqNo.trim() == "" || _Bank == "" || _Bank == "0" || _ChqDate.trim() == "" || _Remarks.trim() == "") {
//        $('#btnSubmit').attr('disabled', true);
//    }
//    else {
//        $('#btnSubmit').attr('disabled', false);
//    }
//}


function IsValidSubmitstale_M() {

    var _Bank = $("#txtEPUBankname").val();
    var _Benificiary = $("#txtEPUBenificary").val();
    var _AccNo = $("#txtEPUAccountNo").val();
    var _Paymode = $("#ddlEPUPaymode").val();
    var _Remarks = $("#txtEPURemarks").val();
    //alert(_Paymode + "Paymode");
    //alert(_Bank + "_Bank");
    //alert(_AccNo + "_AccNo");
    //alert(_Benificiary + "_Benificiary");

    if (_Paymode == "4" || _Paymode == "5")
    {
        if (_AccNo.trim() == "" || _AccNo.trim() == "" || _Bank == "" || _Bank == "0" || _Remarks.trim() == "") {
          //  alert();
            $('#btnSubmit').attr('disabled', true);
        }
        else {
            $('#btnSubmit').attr('disabled', false);
        }
    }

    else {
        if (_Benificiary.trim() == "" || _Benificiary.trim() == "" || _Remarks.trim() == "") {
            $('#btnSubmit').attr('disabled', true);
        }
        else {
            $('#btnSubmit').attr('disabled', false);
        }
    }

}

function ShowDialogStale() {
    $('#DialogStale').attr("style", "display:block;");
    objDialogStale.dialog({ title: 'Stale Cheque : Maker' });
    objDialogStale.dialog("open");
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