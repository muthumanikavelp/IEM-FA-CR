var objDialogy;
var countArray = "0";
UrlDet = UrlDet.replace("FetchPrepaidChecker", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");
var ViewModel = function () {
    var self = this;

    //bank dropdown array
    self.TypeArray = ko.observableArray();
    self.PaymentModeArray = ko.observableArray();

    //load grid array
    self.PrepaidDetailArray = ko.observableArray();

    self.loadType = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "LoadClaimTypeWA",
            data: '{}',
            contentType: "application/json;",
            success: function (response) {
                self.TypeArray(response);
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

    self.select = function (item) {
        $("#hdfgltransfergid").val(item.gltransfergid);      
        $("#txtEARFDate").val(item.ARFDate);
        $("#txtEARFNo").val(item.ARFNo);
        $("#txtESupplier").val(item.Name);
        $("#txtEAdvanceType").val(item.Advancetype);
        $("#txtEPaymode").val(item.paymode);
        $("#txtESupplierEmployee").val(item.Name);
        $("#txtEAdvanceGl").val(item.advancegl);
        $("#txtEARFAmount").val(self.formatNumber(item.ARFAmount));
        $("#txtLiquidationData").val(item.LiqDate);
        $("#txtENewAdvanceDate").val(item.newadvancedate);
        $("#txtENewAdvanceType").val(item.newadvancetype);
        $("#txtENewAdvanceGL").val(item.newadvanceglno);
        $("#txtMakerRemark").val(item.makerremarks);

        $("#txtCheckerRemark").val("");
        $("#txtCheckerRemark").removeClass('required').removeClass('valid');
        $("#txtCheckerRemark").addClass('required');

        ShowDialog();
    };
 
    self.search = function () {
        var data = {
            RequestFrom: $("#txtRequestFrom").val(),
            RequestTo: $("#txtRequestTo").val(),
            RequestCode: $("#hdfRaiserCode").val(),
            RequestName: $("#hdfRaiserName").val(),
            Paymode: $("#ddlPayMode").val(),
            SCode: $("#hdfSupplierCode").val(),
            SName: $("#hdfSupplierName").val(),
            Empcode: $("#hdfEmpCode").val(),
            Empname: $("#hdfEmpName").val(),
            Type: $("#ddlType").val(),
            ARFNo: $("#txtARFNo").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "FetchPrepaidChecker",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                $("#gvMaker").DataTable({
                    "autoWidth": false,
                    "destroy": true
                }).destroy();
                self.PrepaidDetailArray.removeAll();

                var Data1 = "", Data2 = "";
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    self.PrepaidDetailArray(Data2);
                }

                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Msg != "" && countArray == "0") {
                        jAlert(Data1[0].Msg, "Message");
                    } /*else {
                        if (self.PrepaidDetailArray().length == 0) {
                            jAlert("Sorry no records found!", "Message");
                        }
                    }*/
                    countArray = "0";
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.DatatableCall = function () {
        if ($("#gvMaker > tbody > tr").length == self.PrepaidDetailArray().length) {
            $("#gvMaker").DataTable({
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

    self.Approve = function () {
        var data = {
            gltransfergid: $("#hdfgltransfergid").val(),
            Status: "1",
            remarks: $("#txtCheckerRemark").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetPrepaidChecker",
            data: JSON.stringify(data),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 == "Unauthorized User!") {
                    jAlert("Unauthorized User!");
                }
                else if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Msg != "") {
                        jAlert(Data1[0].Msg, "Message");
                    }

                    if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {
                        objDialogy.dialog("close");
                        countArray == "1";
                        self.search();
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
            gltransfergid: $("#hdfgltransfergid").val(),
            Status: "2",
            remarks: $("#txtCheckerRemark").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetPrepaidChecker",
            data: JSON.stringify(data),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 == "Unauthorized User!") {
                    jAlert("Unauthorized User!");
                }
                else if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Msg != "") {
                        jAlert(Data1[0].Msg, "Message");
                    }

                    if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {
                        objDialogy.dialog("close");
                        countArray == "1";
                        self.search();
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
        $("#txtARFFrom").val("");
        $("#txtARFTo").val("");
        $("#txtARFNumber").val("");
        $("#txtARFAmount").val("");
        $("#txtLiquidationFrom").val("");
        $("#txtLiquidationTo,#txtSupplierName,#txtSupplierCode,#txtEmpName,#txtEmpCode").val("");
        $("#ddlType").val("");
        $("#ddlPayMode").val("");
        $("#hdfSupplierCode").val("0");
        $("#hdfSupplierName").val("0");
        $("#hdfEmpCode").val("0");
        $("#hdfEmpName").val("0");
        $("#hdfRaiserCode").val("0");
        $("#hdfRaiserName").val("0");
        $("#ddlAdvanceType").val("");
        $("#txtAdvanceGL").val("");

        $("#gvMaker").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.PrepaidDetailArray.removeAll();
    };

    self.loadGrid = function () {
        $("#gvMaker").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.PrepaidDetailArray.removeAll();
    }
    self.loadGrid();

    //load DropDown.
    self.loadType();
    self.loadPaymentMode();    
};

var mainViewModel = new ViewModel();
ko.applyBindings(mainViewModel);


$(document).ready(function () {
    //added for date sorting option for dataGrid.
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

    //dialog box option
    objDialogy = $("[id$='ShowDialog']");
    objDialogy.dialog({
        autoOpen: false,
        modal: true,
        width: 900,
        height: 500,
        duration: 300
    });

    //calander event
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
            $("#hdfEmpCode").val("0");
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
                $("#hdfEmpCode").val(i.item.val);
                $("#txtEmpCode").val(i.item.label);
            }
        });
    });

    $("#txtEmpCode").focusout(function () {
        var hdfId = $("#hdfEmpCode").val();
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

    //Load txtSSupplier Code Auto Complete
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

    //Load Raiser Code Auto Complete
    $("#txtRaiserCode").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfRaiserCode").val("0");
        }

        $("#txtRaiserCode").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteEmployeeCode",
                    data: "{ 'txt' : '" + $("#txtRaiserCode").val() + "'}",
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
                $("#hdfRaiserCode").val(i.item.val);
                $("#txtRaiserCode").val(i.item.label);
            }
        });
    });

    $("#txtRaiserCode").focusout(function () {
        var hdfId = $("#hdfRaiserCode").val();
        var txtCurName = $("#txtRaiserCode").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtRaiserCode").val("");
        }
    });

    //Load Raiser Name Auto Complete
    $("#txtRaiserName").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfRaiserName").val("0");
        }

        $("#txtRaiserName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteEmployee",
                    data: "{ 'txt' : '" + $("#txtRaiserName").val() + "'}",
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
                $("#hdfRaiserName").val(i.item.val);
                $("#txtRaiserName").val(i.item.label);
            }
        });
    });

    $("#txtRaiserName").focusout(function () {
        var hdfId = $("#hdfRaiserName").val();
        var txtCurName = $("#txtRaiserName").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtRaiserName").val("");
        }
    });


    //popup required strip validation part.
    $("#txtCheckerRemark").keyup(function () {        
        var _data = $("#txtCheckerRemark").val();
        if (_data.trim() == "0" || _data.trim() == "") {
            $("#txtCheckerRemark").removeClass('required').removeClass('valid');
            $("#txtCheckerRemark").addClass('required');
        } else {
            $("#txtCheckerRemark").removeClass('required').removeClass('valid');
            $("#txtCheckerRemark").addClass('valid');
        }
        checkEntryValidation();
    });
});

function ShowDialog() {
    $('#ShowDialog').attr("style", "display:block;");

    checkEntryValidation();

    objDialogy.dialog({ title: 'PrePaid Detail - Checker', width: '900', height: '500' });
    objDialogy.dialog("open");
    return false;
}

function checkEntryValidation() {
    var _remarks = $("#txtCheckerRemark").val();
    if (_remarks.trim() == "") {
        $('#btnReject').attr('disabled', true);
        $('#btnApprove').attr('disabled', true);
    }
    else {
        $('#btnReject').attr('disabled', false);
        $('#btnApprove').attr('disabled', false);
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