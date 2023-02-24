var objDialogy;
var countArray = "0";
UrlDet = UrlDet.replace("FetchPrepaidMaker", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");
var ViewModel = function () {
    var self = this;

    //bank dropdown array
    self.TypeArray = ko.observableArray();
    self.AdvanceTypeArray = ko.observableArray();
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

    self.loadAdvanceType = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "LoadAdvanceType",
            data: '{}',
            contentType: "application/json;",
            success: function (response) {
                self.AdvanceTypeArray(response);
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

    self.TypeSelectChanged = function (obj, event) {
        if (event.originalEvent) {
            $("#txtEmpCode").removeAttr("disabled").removeAttr("style");
            $("#txtEmpName").removeAttr("disabled").removeAttr("style");
            $("#txtSupplierCode").removeAttr("disabled").removeAttr("style");
            $("#txtSupplierName").removeAttr("disabled").removeAttr("style");

            $("#hdfEmpCode").val("0");
            $("#txtEmpCode").val("");
            $("#hdfEmpName").val("0");
            $("#txtEmpName").val("");
            $("#hdfSupplierCode").val("0");
            $("#txtSupplierCode").val("");
            $("#hdfSupplierName").val("0");
            $("#txtSupplierName").val("");

            var chkbit = $("#ddlType").val();
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

    self.select = function (item) {
        debugger;
        
        $("#hdfecfarfgid").val(item.ecfarfgid);
        $("#hdfecfid").val(item.ecfid);
        $("#txtEARFDate").val(item.ARFDate);

        $("#txtEARFNo").val(item.ARFNo);
        $("#txtESupplier").val(item.Name);
        $("#txtEAdvanceType").val(item.ARFTtype);
        $("#txtEPaymode").val(item.paymode);
        $("#txtESupplierEmployee").val(item.Name);
        $("#txtEAdvanceGl").val(item.advancegl);
        $("#txtEARFAmount").val(self.formatNumber(item.ARFAmount));
        $("#txtLiquidationData").val(item.LiqDate);

        $("#txtEAdvanceDate").removeClass('required').removeClass('valid');
        $("#ddlEAdvanceType").removeClass('required').removeClass('valid');
        $("#txtEAdvanceGL").removeClass('required').removeClass('valid');

        //$("#txtEAdvanceDate").addClass('required');
        $("#ddlEAdvanceType").addClass('required');
        $("#txtEAdvanceGL").addClass('required');

        //Ramya Added
        $("#txtEAdvanceDate").val(item.LiqDate);
        //$("#txtEAdvanceDate").val("");
        $("#ddlEAdvanceType").val("0");
        $("#txtEAdvanceGL").val("");
        $("#txtMakerRemark").val("");

        ShowDialog();
    };

    self.search = function () {
        var data = {
            ARFFrom: $("#txtARFFrom").val(),
            ARFTo: $("#txtARFTo").val(),
            ARFNo: $("#txtARFNumber").val(),
            ARFAmt: $("#txtARFAmount").val().replace(/,/g, ''),
            liqdatefrom: $("#txtLiquidationFrom").val(),
            liqdateto: $("#txtLiquidationTo").val(),
            Type: $("#ddlType").val(),
            Paymode: $("#ddlPayMode").val(),
            SCode: $("#hdfSupplierCode").val(),
            SName: $("#hdfSupplierName").val(),
            Empcode: $("#hdfEmpCode").val(),
            Empname: $("#hdfEmpName").val(),
            Raisercode: $("#hdfRaiserCode").val(),
            Raisername: $("#hdfRaiserName").val(),
            Advancetype: $("#ddlAdvanceType").val(),
            Advancegl: $("#txtAdvanceGL").val(),
            rejected: $("input[name=modeStatus]:checked").val(),
        };
        $.ajax({
            type: "post",
            url: UrlDet + "FetchPrepaidMaker",
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
                        if (self.PrepaidDetailArray().length == 0 && countArray == "0") {
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

    self.clearFilter = function () {
        $("#txtARFFrom").val("");
        $("#txtARFTo").val("");
        $("#txtARFNumber").val("");
        $("#txtARFAmount").val("");
        $("#txtLiquidationFrom").val("");
        $("#txtLiquidationTo").val("");
        $("#ddlType").val("");
        $("#ddlPayMode").val("");
        $("#hdfSupplierCode").val("0");
        $("#hdfSupplierName").val("0");
        $("#hdfEmpCode").val("0");
        $("#hdfEmpName").val("0");
        $("#hdfRaiserCode").val("0");
        $("#hdfRaiserName").val("0");

        $("#txtSupplierCode").val("");
        $("#txtSupplierName").val("");
        $("#txtEmpCode").val("");
        $("#txtEmpName").val("");
        $("#txtRaiserCode").val("");
        $("#txtRaiserName").val("");

        $("#ddlAdvanceType").val("");
        $("#txtAdvanceGL").val("");

        $("#gvMaker").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.PrepaidDetailArray.removeAll();
    };

    self.Submit = function () {
        var data = {
            ecfid: $("#hdfecfid").val(),
            ecfarfid: $("#hdfecfarfgid").val(),
            newadvancedate: $("#txtEAdvanceDate").val(),
            newadvencetype: $("#ddlEAdvanceType").val(),
            newadvancegl: $("#txtEAdvanceGL").val(),
            remarks: $("#txtMakerRemark").val(),
            isremoved: "0"
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetPrepaidMaker",
            data: JSON.stringify(data),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 == "Unauthorized User!") {
                    jAlert("Unauthorized User!");
                    return false;
                }
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Msg != "") {
                        jAlert(Data1[0].Msg, "Message");
                    }

                    if (Data1[0].Clears == "true" || Data1[0].Clears == "True" || Data1[0].Clears == "1") {
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
    self.loadAdvanceType();
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
        height: 400,
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
    //Ramya commentted as per EPU team need
   /* $("#txtEAdvanceDate").change(function () {
        checkEntryValidation();
        var _data = $("#txtEAdvanceDate").val();
        if (_data.trim() == "0" || _data.trim() == "") {
            $("#txtEAdvanceDate").removeClass('required').removeClass('valid');
            $("#txtEAdvanceDate").addClass('required');
        } else {
            $("#txtEAdvanceDate").removeClass('required').removeClass('valid');
            $("#txtEAdvanceDate").addClass('valid');
        }
    });*/

    $("#ddlEAdvanceType").focusout(function () {
        checkEntryValidation();
        var _data = $("#ddlEAdvanceType").val();
        if (_data.trim() == "0" || _data.trim() == "") {
            $("#ddlEAdvanceType").removeClass('required').removeClass('valid');
            $("#ddlEAdvanceType").addClass('required');
        } else {
            $("#ddlEAdvanceType").removeClass('required').removeClass('valid');
            $("#ddlEAdvanceType").addClass('valid');
        }
    });

    $("#txtEAdvanceGL").focusout(function () {
        checkEntryValidation();
        var _data = $("#txtEAdvanceGL").val();
        if (_data.trim() == "0" || _data.trim() == "") {
            $("#txtEAdvanceGL").removeClass('required').removeClass('valid');
            $("#txtEAdvanceGL").addClass('required');
        } else {
            $("#txtEAdvanceGL").removeClass('required').removeClass('valid');
            $("#txtEAdvanceGL").addClass('valid');
        }
    });

    $("#txtARFAmount").keyup(function (event) {
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

function ShowDialog() {
    $('#ShowDialog').attr("style", "display:block;");

    checkEntryValidation();

    objDialogy.dialog({ title: 'PrePaid Detail', width: '900', height: '400' });
    objDialogy.dialog("open");
    return false;
}

function checkEntryValidation() {
    var _AdvanceDate = $("#txtEAdvanceDate").val();
    var _AdvanceType = $("#ddlEAdvanceType").val();
    var _AdvanceGL = $("#txtEAdvanceGL").val();

    if (_AdvanceDate.trim() == "" || _AdvanceType.trim() == "" || _AdvanceGL.trim() == "0" || _AdvanceGL.trim() == "") {
        $('#btnUpdate').attr('disabled', true);
    }
    else {
        $('#btnUpdate').attr('disabled', false);
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