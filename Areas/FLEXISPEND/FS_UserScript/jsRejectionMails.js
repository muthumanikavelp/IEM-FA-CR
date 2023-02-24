UrlDet = UrlDet.replace("GetSingleRejectionDetails", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");
var PaymentRunModel = function () {
    var self = this;

    self.PaymentModeArray = ko.observableArray();

    self.EFTRejectionArray = ko.observableArray();
    self.EFTBulkRejectionArray = ko.observableArray();

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

    //search filter
    self.searchFilter = function () {
        var data = {
            BounceDateFrom: $("#txtBounceFrom").val(),
            BounceDateTo: $("#txtBounceTo").val(),
            PayMode: $("#ddlPayMode").val(),
            Amount: $("#txtAmount").val().replace(/,/g, ''),
            EmpCodeId: $("#hdfEmpCodeId").val(),
            EmpNameId: $("#hdfEmpName").val(),
            SuppCodeId: $("#hdfSupplierCode").val(),
            SuppNameId: $("#hdfSupplierName").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetSingleRejectionDetails",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {

                self.loadGrid();

                var Data1 = "", Data2 = "";

                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    self.EFTRejectionArray(Data2);
                }

                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    if (Data1[0].Message.length > 0) {
                        jAlert(Data1[0].Message, "Message");
                    } else if (Data1[0].Message.length == 0 && self.EFTRejectionArray().length==0) {
                        jAlert("Sorry no records found!", "Message");
                    }
                }

                
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    };
    self.search = function () {
        var data = {
            BounceDateFrom: $("#txtBounceFrom").val(),
            BounceDateTo: $("#txtBounceTo").val(),
            PayMode: $("#ddlPayMode").val(),
            Amount: $("#txtAmount").val().replace(/,/g, ''),
            EmpCodeId: $("#hdfEmpCodeId").val(),
            EmpNameId: $("#hdfEmpName").val(),
            SuppCodeId: $("#hdfSupplierCode").val(),
            SuppNameId: $("#hdfSupplierName").val()
        };
        ShowLoading(true);
        $.ajax({
            type: "post",
            url:  UrlDet + "GetSingleRejectionEmailAlert",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {

                ShowLoading(false);
                var Data1 = "";

                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    if (Data1[0].Message.length > 0) {
                        jAlert(Data1[0].Message, "Message");
                    } else if (Data1[0].Message.length == 0 && self.EFTRejectionArray().length == 0) {
                        jAlert("Sorry no records found!", "Message");
                    }
                }


            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                ShowLoading(false );
            }
        });
    };
    self.SendEmailAlert = function () {
        
         var data = {
             BounceDateFrom: $("#txtBounceFrom").val(),
             BounceDateTo: $("#txtBounceTo").val(),
             PayMode: $("#ddlPayMode").val(),
             Amount: $("#txtAmount").val().replace(/,/g, ''),
             EmpCodeId: $("#hdfEmpCodeId").val(),
             EmpNameId: $("#hdfEmpName").val(),
             SuppCodeId: $("#hdfSupplierCode").val(),
             SuppNameId: $("#hdfSupplierName").val()
         };
         ShowLoading(true);
         $.ajax({
             type: "post",
             url: UrlDet + "GetSingleRejectionEmailAlert",
             data: JSON.stringify(data),
             contentType: "application/json;",
             success: function (response) {
                 ShowLoading(false);
                 var Data1 = "";              
 
                 if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                     Data1 = JSON.parse(response.Data1);
 
                     if (Data1[0].Message.length > 0) {
                         jAlert(Data1[0].Message, "Message");
                     } else if (Data1[0].Message.length == 0 && self.EFTRejectionArray().length == 0) {
                         jAlert("Sorry no records found!", "Message");
                     }
                 }
 
 
             },
             error: function (XMLHttpRequest, textStatus, errorThrown) {
                 ShowLoading(false);
             }
         });
    };

    self.checkChange = function (obj, event) {
        if (event.originalEvent) {
            var array = new Array();
            $('#ContentECF input:checkbox:checked').each(function (index) {
                array[index] = $(this).attr('id');
            });

            var chkBit = array.length == self.EFTRejectionArray().length ? true : false;
            $("#chkHSelete").prop('checked', chkBit);
        }
    };


    //Clear the form Entry
    self.clearFilter = function () {
        $("#hdfSupplierCode").val("0");
        $("#txtSupplierCode").val("");
        $("#hdfSupplierName").val("0");
        $("#txtSupplierName").val("");
        $("#hdfEmpCodeId").val("0");
        $("#txtEmpCode").val("");
        $("#hdfEmpName").val("0");
        $("#txtEmpName").val("");

        $("#ddlPayMode").val("0");
        $("#txtBounceFrom").val("");
        $("#txtBounceTo").val("");
        $("#txtAmount").val("");

        $("#chkHSelete").prop('checked', false);

        self.loadGrid();
    };

    //Datatable Plugin codeing
    self.DatatableCall = function () {
        if ($("#gvRejectionMails > tbody > tr").length == self.EFTRejectionArray().length) {
            $("#gvRejectionMails").DataTable({
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
        $("#gvRejectionMails").DataTable({
            "autoWidth": false,
            "destroy": true,
            "bFilter": false,
            "bLengthChange": false,
            "bPaginate": false,
            "bInfo": false
        }).destroy();
        self.EFTRejectionArray.removeAll();
    }
    self.loadGrid();

    //Bulk Rejection Mails
    self.searchBulkFilter = function () {
        var data = {
            BounceDate: $("#txtBulkBounceDate").val(),
            Paymode: $("#ddlBulkPayMode").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetBulkRejectionDetails",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                self.EFTBulkRejectionArray.removeAll();

                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    self.EFTBulkRejectionArray(Data1);
                } else {
                    jAlert("Sorry no records found!", "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    };

    self.clearBulk = function () {
        $("#ddlBulkPayMode").val("0");
        $("#txtBulkBounceDate").val("");

        $("#txtBulkBounceDate").removeClass('required').removeClass('valid');
        $("#txtBulkBounceDate").addClass('required');

        chkBulkSearch();
        self.EFTBulkRejectionArray.removeAll();
    };


    self.searchBulk = function () {
        var data = {
            BounceDate: $("#txtBulkBounceDate").val(),
            Paymode: $("#ddlBulkPayMode").val()
        };
        ShowLoadingBulk(true);
        $.ajax({
            type: "post",
            url: UrlDet + "GetBulkRejectionDetailsEmailAlert",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                ShowLoadingBulk(false);
                var Data1 = "";

                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    if (Data1[0].Message.length > 0) {
                        jAlert(Data1[0].Message, "Message");
                    } else if (Data1[0].Message.length == 0 && self.EFTRejectionArray().length == 0) {
                        jAlert("Sorry no records found!", "Message");
                    }
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                ShowLoadingBulk(false);
            }
        });
    };

    //load the paymode
    self.loadPaymentMode();
};

var mainViewModel = new PaymentRunModel();
ko.applyBindings(mainViewModel);

$(document).ready(function () {
    var objDate = new Date();
    var Presentyear = objDate.getFullYear();
    $(".fsDatepicker").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
    });

    $('#btnBulksearch').attr('disabled', true);

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

    $("#chkHSelete").click(function (e) {
        $(this).closest('table').find('td input:checkbox').prop('checked', this.checked);
    });

    $("#txtBulkBounceDate").change(function () {
        chkBulkSearch();
        var _data = $("#txtBulkBounceDate").val();
        if (_data.trim() != "") {
            $("#txtBulkBounceDate").removeClass('required');
            $("#txtBulkBounceDate").addClass('valid');
        }
        else {
            $("#txtBulkBounceDate").removeClass('valid');
            $("#txtBulkBounceDate").addClass('required');
        }
    });

    $("#txtAmount").keyup(function (event) {
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

function chkBulkSearch() {
    var _reason = $("#txtBulkBounceDate").val();

    if (_reason == "" || _reason.length != 10) {
        $('#btnBulksearch').attr('disabled', true);
    } else {
        $('#btnBulksearch').attr('disabled', false);
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

function ShowLoading(IsShow) {
    if (IsShow)
        $('#loadingImg').attr("style", "display:block; text-align:center; padding-top:5px; color:maroon;");
    else
        $('#loadingImg').attr("style", "display:none; text-align:center; padding-top:5px; color:maroon;");
}

function ShowLoadingBulk(IsShow) {
    if (IsShow)
        $('#loadingImgBulk').attr("style", "display:block; text-align:center; padding-top:5px; color:maroon;");
    else
        $('#loadingImgBulk').attr("style", "display:none; text-align:center; padding-top:5px; color:maroon;");
}