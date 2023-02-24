var objDialogStale;
UrlDet = UrlDet.replace("GetStaleChequeChecker", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");
var PettyCashModel = function () {
    var self = this;
    self.StaleChequeArray = ko.observableArray();
        
    self.BankArray = ko.observableArray();

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
            url: UrlDet + "GetStaleChequeChecker",
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

    self.Reissue = function (item) {
        $("#hdfPvId").val(item.PvId);
       // alert(item.Paymode);
       // alert(item.PvNo);
        $("#txtPUPVDate").val(item.PvDate);
        $("#txtPUPVNo").val(item.PvNo);
        $("#txtPUMovedDate").val(item.StaleMovedDate);
        $("#txtPUpaymode").val(item.Paymode);
        $("#txtPUACCNo").val(item.EmployeeSupplierAccount);
        $("#txtPUbenificiary").val(item.Beneficiary);
        $("#txtPUBankName").val(item.EmployeeSupplierBank);

        $("#txtEPUMakerRemarks").val(item.MakerRemark);

        $('.entry').removeClass('required').removeClass('valid');
        $('.entry').addClass('required');

        
        $("#attUploader").val("");
        $("#txtEPUCheckerRemarks").val("");

        IsValidSubmit();
        ShowDialogStale();
    };

    self.Approve = function () {
        var data = {
            PvId: $("#hdfPvId").val(),
            Status: "Approve",
            Remarks: $("#txtEPUCheckerRemarks").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetStaleChequeChecker",
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

    self.Reject = function () {
        var data = {
            PvId: $("#hdfPvId").val(),
            Status: "Reject",
            Remarks: $("#txtEPUCheckerRemarks").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetStaleChequeChecker",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
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

        $("#txtDocNo").val("");

        $("#hdfEmpCodeId").val("0");
        $("#txtEmpCode").val("");
        $("#hdfEmpName").val("0");
        $("#txtEmpName").val("");

        $("#hdfSupplierCode").val("0");
        $("#txtSupplierCode").val("");
        $("#hdfSupplierName").val("0");
        $("#txtSupplierName").val("");

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

    self.loadBank();
};

var mainViewModel = new PettyCashModel();
ko.applyBindings(mainViewModel);

$(document).ready(function () {
    objDialogStale = $("[id$='DialogStale']");
    objDialogStale.dialog({
        autoOpen: false,
        modal: true,
        width: 800,
        height: 550,
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
                    url: UrlHelper +"GetAutoCompleteEmployee",
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
                    url: UrlHelper +"GetAutoCompleteSupplierCode",
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

    $('.entry').keyup(function () {
        IsValidSubmit();
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

    $(".attUploader").on('change', function (e) {
        var Attach_list = Attachment_list();
        var Attachment_fomat = Attached_fileformat();  //Pandiaraj 18-11-2019 
        var iframe = $('<iframe name="postiframe" id="postiframe" style="display: none"></iframe>');
        $("body").append(iframe);
        var form = $('#uploaderForm');
        //form.attr("action", UrlDet + "UploadAttachment");
        form.attr("action", UrlDet + "UploadAttachment/?attach=" + Attach_list + '&attaching_format=' + Attachment_fomat);  //Pandiaraj 18-11-2019 
        form.attr("method", "post");
        form.attr("encoding", "multipart/form-data");
        form.attr("enctype", "multipart/form-data");
        form.attr("target", "postiframe");
        form.attr("file", $('#attUploader').val());
        form.submit();
        return false;
    });

});

function IsValidSubmit() {
    var _Remarks = $("#txtEPUCheckerRemarks").val();

    if (_Remarks.trim() == "") {
        $('#btnApprove').attr('disabled', true);
        $('#btnReject').attr('disabled', true);
    }
    else {
        $('#btnApprove').attr('disabled', false);
        $('#btnReject').attr('disabled', false);
    }
}

function ShowDialogStale() {
    $('#DialogStale').attr("style", "display:block;");
    objDialogStale.dialog({ title: 'Stale Cheque : Checker' });
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