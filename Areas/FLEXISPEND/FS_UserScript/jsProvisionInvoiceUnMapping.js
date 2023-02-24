var objDialog;
UrlDet = UrlDet.replace("GetProvisionUnMapping", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");
var InvoiceMappingModel = function () {
    var self = this;
    self.InvoiceDetailArray = ko.observableArray();
    self.tmpArray = ko.observableArray();
    self.InnerInvoiceDetailArray = ko.observableArray();

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
            InvFrom: $("#txtInvoiceFrom").val(),
            InvTo: $("#txtInvoiceTo").val(),
            InvNo: $("#txtInvoiceNo").val(),
            InvAmt: $("#txtInvoiceAmount").val().replace(/,/g, ''),
            ECFFrom: $("#txtECFFrom").val(),
            ECFTo: $("#txtECFTo").val(),
            ECFNo: $("#txtECFNo").val(),
            ECFAmt: $("#txtECFAmount").val().replace(/,/g, ''),
            SCode: $("#hdfSupplierCode").val(),
            SName: $("#hdfSupplierName").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetProvisionUnMapping",
            async: false,
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                self.loadGrid();
                self.tmpArray.removeAll();

                var Data1 = "", Data2 = "", Data3 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Msg != "") {
                        jAlert(Data1[0].Msg, "Message");
                    }
                }

                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    self.InvoiceDetailArray(Data2);
                } else if (self.InvoiceDetailArray().length == 0) {
                    jAlert("Sorry no records found!", "Message");
                }

                if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null) {
                    Data3 = JSON.parse(response.Data3);
                    self.tmpArray(Data3);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.DatatableCall = function () {
        if ($("#gvPaymentInvoice > tbody > tr").length == self.InvoiceDetailArray().length) {
            $("#gvPaymentInvoice").DataTable({
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
        $("#txtInvoiceFrom").val("");
        $("#txtInvoiceTo").val("");
        $("#txtInvoiceNo").val("");
        $("#txtInvoiceAmount").val("");
        $("#txtECFFrom").val("");
        $("#txtECFTo").val("");
        $("#txtECFNo").val("");
        $("#txtECFAmount").val("");
        $("#hdfSupplierCode").val("0");
        $("#hdfSupplierName").val("0");
        $("#txtSupplierName").val("");
        $("#txtSupplierCode").val("");

        self.tmpArray.removeAll();
        self.loadGrid();
    };

    self.select = function (item) {
        $("#hdfInvid").val("0");
        $("#hdfInvid").val(item.invid);

        $("#lblInwardDate").text(item.invdate);
        $("#lblInwardNo").text(item.invno);
        $("#lblInwardAmount").text(self.formatNumber(item.invamt));
        $("#lblDescription").text(item.invoicedesc);
        $("#lblSupplier").text(item.SCode + ' - ' + item.Sname);

        $("#lblECFDate").text(item.ecfdate);
        $("#lblECFNo").text(item.ecfno);
        $("#lblECFAmount").text(self.formatNumber(item.ecfamt));
        
        self.InnerInvoiceDetailArray.removeAll();
        
        var tmpArray = new Array();
        tmpArray = ko.utils.arrayFilter(self.tmpArray(), function (aitem) {
            return item.invid == aitem.invid;
        });
        self.InnerInvoiceDetailArray(tmpArray.slice());
        ShowPopUp();
    };

    self.unMap = function (item) {
        var _mapDesc = $("#txtMapDesc_" + item.Provisionmapgid).val();

       if (_mapDesc.length == 0) {
            jAlert("Ensure UnMap Description", "Message");
            return false;
        }

        var data = {
            provisionmapgid: item.Provisionmapgid,
            Invoicegid: $("#hdfInvid").val(),
            MDesc: _mapDesc
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetProvisionUnMapping",
            async: false,
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message
                    if (Data1[0].Clear == "True" || Data1[0].Clear == "true" || Data1[0].Clear == "1") {

                        objDialog.dialog("close");
                        jConfirm(Data1[0].Msg, "Message", function (callback) {
                            if (callback == true) {
                                self.Search();
                            } else {
                                self.Search();
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

    self.loadGrid = function () {
        $("#gvPaymentInvoice").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();

        self.InvoiceDetailArray.removeAll();
    }
    self.loadGrid();
};

var mainViewModel = new InvoiceMappingModel();
ko.applyBindings(mainViewModel);

$(document).ready(function () {
    objDialog = $("[id$='ShowInvoice']");
    objDialog.dialog({
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
        var _data = $("#txtSupplierCode").val();
        if (_data.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
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
        var _data = $("#txtSupplierName").val();
        if (_data.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtSupplierName").val("");
        }
    });

    $("#txtInvoiceAmount,#txtECFAmount").keyup(function (event) {
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

function ShowPopUp() {
    $('#ShowInvoice').attr("style", "display:block;");

    objDialog.dialog({ title: 'Provision UnMapping' });
    objDialog.dialog("open");
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