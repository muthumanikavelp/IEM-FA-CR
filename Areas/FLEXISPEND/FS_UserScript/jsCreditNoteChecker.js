var objDialogy;
var countArray = "0";

UrlDet = UrlDet.replace("GetCreditNoteChecker", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");

var CreditNoteCheckerModel = function () {
    var self = this;

    //load grid array
    self.CreditNoteArray = ko.observableArray();

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

    self.search = function () {
        var data = {
            DateFrom: $("#txtDateFrom").val(),
            DateTo: $("#txtDateTo").val(),
            SupplierId: $("#hdfSupplier").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetCreditNoteChecker",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {

                $("#gvChecker").DataTable({
                    "autoWidth": false,
                    "destroy": true,
                    "bFilter": false,
                    "bLengthChange": false,
                    "bPaginate": false,
                    "bInfo": false
                }).destroy();
                self.CreditNoteArray.removeAll();

                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Msg != "") {
                        jAlert(Data1[0].Msg, "Message");
                    }
                }

                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    self.CreditNoteArray(Data2);
                }
                
                if (self.CreditNoteArray().length == 0 && countArray == "0") {
                    jAlert("Sorry! No Records Found.", "Message");
                }
                countArray = "0";
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.DatatableCall = function () {
        if ($("#gvChecker > tbody > tr").length == self.CreditNoteArray().length) {
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

    self.Select = function (item) {
        $("#hdfCNMId").val(item.gid);        

        $("#txtBookDate").val(item.BookingDate);
        $("#txtESupplier").val(item.suppliercode + '-' + item.suppliername);
        $("#txtCreditNoteNo").val(item.creditnoteno);
        $("#txtCreditNoteAmount").val(self.formatNumber(item.creditnoteamt));
        $("#txtCreditNoteDate").val(item.creditnotedate);
        $("#txtECFNo").val(item.ECFNo);
        $("#txtInvoiceNo").val(item.invno);
        $("#txtInvoiceAmount").val(self.formatNumber(item.invamt));
        $("#txtMakerRemark").val(item.Remarks);
        
        $("#txtCheckerRemark").val("");

        checkValidationUpdate();
        ShowDialog();
    };

    self.Approve = function () {
        var data = {
            Creditnotegid: $("#hdfCNMId").val(),
            Status: "1",
            remarks: $("#txtCheckerRemark").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetCreditNoteChecker",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 == "Unauthorized User!") {
                    jAlert("Unauthorized User!");
                }
                else if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {
                        objDialogy.dialog("close");
                        jConfirm(Data1[0].Msg, "Message", function (callback) {
                            if (callback == true) {
                                countArray = "1";
                                self.search();
                            } else {
                                countArray = "1";
                                self.search();
                            }
                        });

                        //remove the cancel button from dialog box
                        $("#popup_ok").attr("style", "margin-left: 50px;");
                        $("#popup_cancel").attr("style", "visibility: hidden");
                    } else {
                        //show Message if error
                        if (Data1[0].Msg != "") {
                            jAlert(Data1[0].Msg, "Message");
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
            Creditnotegid: $("#hdfCNMId").val(),
            Status: "2",
            remarks: $("#txtCheckerRemark").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetCreditNoteChecker",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 == "Unauthorized User!") {
                    jAlert("Unauthorized User!");
                }
                else if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {
                        objDialogy.dialog("close");
                        
                        jConfirm(Data1[0].Msg, "Message", function (callback) {
                            if (callback == true) {
                                countArray = "1";
                                self.search();
                            } else {
                                countArray = "1";
                                self.search();
                            }
                        });

                        //remove the cancel button from dialog box
                        $("#popup_ok").attr("style", "margin-left: 50px;");
                        $("#popup_cancel").attr("style", "visibility: hidden");
                    } else {
                        //show Message if error
                        if (Data1[0].Msg != "") {
                            jAlert(Data1[0].Msg, "Message");
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

    self.clearFilter = function () {
        $("#txtDateFrom").val("");
        $("#txtDateTo").val("");

        $("#hdfSupplier").val("0");
        $("#txtSupplier").val("");

        $("#txtDateFrom").removeClass('required').removeClass('valid');
        $("#txtDateTo").removeClass('required').removeClass('valid');

        $("#txtDateFrom").addClass('required');
        $("#txtDateTo").addClass('required');

        checkValidationSearch();

        $("#gvChecker").DataTable({
            "autoWidth": false,
            "destroy": true,
            "bFilter": false,
            "bLengthChange": false,
            "bPaginate": false,
            "bInfo": false
        }).destroy();
        self.CreditNoteArray.removeAll();
    };

    self.loadGrid = function () {
        $("#gvChecker").DataTable({
            "autoWidth": false,
            "destroy": true,
            "bFilter": false,
            "bLengthChange": false,
            "bPaginate": false,
            "bInfo": false
        }).destroy();
        self.CreditNoteArray.removeAll();
    }
    self.loadGrid();
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
        height: 450,
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
    checkValidationSearch();
    //search auto complete
    $("#txtSupplier").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfSupplier").val("0");
        }

        $("#txtSupplier").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteSupplierAll",
                    data: "{ 'txt' : '" + $("#txtSupplier").val() + "'}",
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
                $("#hdfSupplier").val(i.item.val);
                $("#txtSupplier").val(i.item.label);
            }
        });
    });

    $("#txtSupplier").focusout(function () {
        var hdfId = $("#hdfSupplier").val();
        var _data = $("#txtSupplier").val();
        if (_data.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtSupplier").val("");
        }
    });

    $("#txtDateFrom").change(function () {
        checkValidationSearch();

        var txtDate = $("#txtDateFrom").val();
        if (txtDate.trim() != "") {
            $("#txtDateFrom").removeClass('required');
            $("#txtDateFrom").addClass('valid');
        }
        else {
            $("#txtDateFrom").removeClass('valid');
            $("#txtDateFrom").addClass('required');
        }
    });

    $("#txtDateTo").change(function () {
        checkValidationSearch();

        var txtDate = $("#txtDateTo").val();
        if (txtDate.trim() != "") {
            $("#txtDateTo").removeClass('required');
            $("#txtDateTo").addClass('valid');
        }
        else {
            $("#txtDateTo").removeClass('valid');
            $("#txtDateTo").addClass('required');
        }
    });

    $("#txtCheckerRemark").change(function () {
        checkValidationUpdate();

        var _data = $("#txtCheckerRemark").val();
        if (_data.trim() != "") {
            $("#txtCheckerRemark").removeClass('required');
            $("#txtCheckerRemark").addClass('valid');
        }
        else {
            $("#txtCheckerRemark").removeClass('valid');
            $("#txtCheckerRemark").addClass('required');
        }
    });

});

function ShowDialog() {
    $('#ShowDialog').attr("style", "display:block;");

    objDialogy.dialog({ title: 'Credit Note - Checker', width: '800', height: '450' });
    objDialogy.dialog("open");
    return false;
}

function checkValidationSearch() {
    var _dtFrm = $("#txtDateFrom").val();
    var _dtTo = $("#txtDateTo").val();

    if (_dtFrm.trim() == "" || _dtTo.trim() == "") {
        $('#btnsearch').attr('disabled', true);
    }
    else {
        $('#btnsearch').attr('disabled', false);
    }
}

function checkValidationUpdate() {
    var _data = $("#txtCheckerRemark").val();

    if (_data.trim() == "") {
        $('#btnApprove').attr('disabled', true);
        $('#btnReject').attr('disabled', true);
    }
    else {
        $('#btnApprove').attr('disabled', false);
        $('#btnReject').attr('disabled', false);
    }
}

function isEvent(evt) {
    return false;
}