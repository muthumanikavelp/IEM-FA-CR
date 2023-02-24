var objShowDg;
UrlDet = UrlDet.replace("FetchPrepaidExtension", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");
var PrepaidExtensionModel = function () {
    var self = this;
    self.DocumentTypeArray = ko.observableArray();
    self.PaymentModeArray = ko.observableArray();

    self.DetailArray = ko.observableArray();

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

    self.Search = function () {
        var data = {
            ARFDateFrom: $("#txtArfFromDate").val(),
            ARFDateTo: $("#txtArfToDate").val(),
            ARFNo: $("#txtArfNumber").val(),
            ARFAmount: $("#txtArfAmount").val().replace(/,/g, ''),
            LiquidDateFrom: $("#txtLiqFromDate").val(),
            LiquidDateTo: $("#txtLiqToDate").val(),
            Type: $("#ddlType").val(),
            PayMode: $("#ddlPayMode").val(),
            EmpCodeId: $("#hdfEmpCode").val(),
            EmpNameId: $("#hdfEmpName").val(),
            SuppCodeId: $("#hdfSupplierCode").val(),
            SuppNameId: $("#hdfSupplierName").val(),
            RaiserCodeId: $("#hdfRaiserCode").val(),
            RaiserNameId: $("#hdfRaiserName").val()
        };
        self.loadGrid();
        $.ajax({
            type: "post",
            url: UrlDet + "FetchPrepaidExtension",
            async: false,
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
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
                    self.DetailArray(Data2);
                } else if (self.DetailArray().length == 0) {
                    jAlert("Sorry no records found!", "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.DatatableCall = function () {
        if ($("#gv > tbody > tr").length == self.DetailArray().length) {
            $("#gv").DataTable({
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

    self.Extend = function (item) {
        $("#hdfId").val("");
        $("#hdfId").val(item.ECFARFGId);

        $("#lblArfDate").text(item.ARFDate);
        $("#lblArfNo").text(item.ARFNo);
        $("#lblType").text(item.EmployeeSupplierType);
        $("#lblPayMode").text(item.PayMode);
        $("#lblSupplierEmployee").text(item.EmployeeSupplierName);
        
        $("#lblArfAmt").text(self.formatNumber(item.ARFAmount));
        $("#lblOutStandingAmt").text(self.formatNumber(item.OutStandingAmount));
        $("#lblliquidationDate").text(item.LiquidationDate);

        $("#txtRemarks").val("");
        $("#txtLiquidationDate").val("");
        $("#attUploader").val("");
 
        $("#txtLiquidationDate").removeClass('required').removeClass('valid');
        $("#txtLiquidationDate").addClass('required');
        $("#txtRemarks").removeClass('required').removeClass('valid');
        $("#txtRemarks").addClass('required');

        checkValidation();
        ShowDg();
    };

    self.Update = function () {
        var data = {
            ECFARFGId: $("#hdfId").val(), 
            OldLiquidationDate: $("#lblliquidationDate").text(),
            NewLiquidationDate: $("#txtLiquidationDate").val(),
            Remarks: $("#txtRemarks").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetPrepaidExtension",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");
                    }

                    if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {
                        objShowDg.dialog("close");
                        self.Search();
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.Clear = function () {
        $("#ddlType").val(0);
        $("#ddlPayMode").val(0);

        $(".datePicker").val("");
        $("#txtArfNumber").val("");
        $("#txtArfAmount").val("");

        $("#txtLiqFromDate").val("");
        $("#txtLiqToDate").val("");

        $("#hdfEmpCode").val("0");
        $("#hdfEmpName").val("0");
        $("#txtEmpCode").val("");
        $("#txtEmpName").val("");

        $("#hdfSupplierCode").val("0");
        $("#hdfSupplierName").val("0");
        $("#txtSupplierCode").val("");
        $("#txtSupplierName").val("");

        $("#hdfRaiserCode").val("0");
        $("#hdfRaiserName").val("0");
        $("#txtRaiserCode").val("");
        $("#txtRaiserName").val("");
        
        self.loadGrid();
    };

    self.loadGrid = function () {
        $("#gv").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.DetailArray.removeAll();
    }
    self.loadGrid();

    //load the Doctument Type DropDown.
    self.loadDocType();
    self.loadPaymentMode();
};
var mainViewModel = new PrepaidExtensionModel();
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

    objShowDg = $("[id$='ShowDg']");
    objShowDg.dialog({
        autoOpen: false,
        modal: true,
        width: 800,
        height: 450,
        duration: 300
    });

    $("#txtLiquidationDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        numberOfMonths: 1
    });

    $("#txtLiquidationDate").keyup(function (e) {
        if (e.keyCode == 8 || e.keyCode == 46) {
            $.datepicker._clearDate(this);
        }
        $(this).datepicker('show');
    });

    $(".datePicker").datepicker({
        yearRange: '1900:+nn',
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
    });

    $(".datePicker").keyup(function (e) {
        if (e.keyCode == 8 || e.keyCode == 46) {
            $.datepicker._clearDate(this);
        }
        $(this).datepicker('show');
    });

    $("#txtLiqFromDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        numberOfMonths: 1,
        onSelect: function (selected) {
            $("#txtLiqToDate").datepicker("option", "minDate", selected)
        }
    });

    $("#txtLiqFromDate").keyup(function (e) {
        if (e.keyCode == 8 || e.keyCode == 46) {
            $.datepicker._clearDate(this);
        }
        $(this).datepicker('show');
    });

    $("#txtLiqToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        numberOfMonths: 1,
        onSelect: function (selected) {
            $("#txtLiqFromDate").datepicker("option", "maxDate", selected)
        }
    });

    $("#txtLiqToDate").keyup(function (e) {
        if (e.keyCode == 8 || e.keyCode == 46) {
            $.datepicker._clearDate(this);
        }
        $(this).datepicker('show');
    });

    //Search Filter's
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

    //User Entry Fields
    $("#txtLiquidationDate").change(function () {
        checkValidation();
        var txtDate = $("#txtLiquidationDate").val();
        if (txtDate.trim() != "") {
            $("#txtLiquidationDate").removeClass('required');
            $("#txtLiquidationDate").addClass('valid');
        }
        else {
            $("#txtLiquidationDate").removeClass('valid');
            $("#txtLiquidationDate").addClass('required');
        }
    });

    $("#txtRemarks").keyup(function () {
        checkValidation();

        var _remarks = $("#txtRemarks").val();
        if (_remarks.trim() != "") {
            $("#txtRemarks").removeClass('required');
            $("#txtRemarks").addClass('valid');
        }
        else {
            $("#txtRemarks").removeClass('valid');
            $("#txtRemarks").addClass('required');
        }
    });

    $("#txtArfAmount").keyup(function (event) {
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

function ShowDg() {
    $('#ShowDg').attr("style", "display:block;");

    objShowDg.dialog({ title: 'Prepaid Extension' });
    objShowDg.dialog("open");
    return false;
}

function checkValidation() {
    var _dt = $("#txtLiquidationDate").val();
    var _remarks = $("#txtRemarks").val();
    
    if (_dt.trim() == "" || _remarks.trim()=="") {
        $('#btnSubmit').attr('disabled', true);
    }
    else {
        $('#btnSubmit').attr('disabled', false);
    }
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