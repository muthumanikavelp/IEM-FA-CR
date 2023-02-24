UrlDet = UrlDet.replace("GetPettyCashAlert", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");

var PettyCashModel = function () {
    var self = this;
    self.PettyCashArray = ko.observableArray();
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

    //search filter
    self.search = function () {
        var data = {
            ARFDateFrom: $("#txtARFFrom").val(), 
            ARFDateTo: $("#txtARFTo").val(), 
            ARFNo: $("#txtARFNo").val(), 
            ARFAmount: $("#txtARFAmount").val().replace(/,/g, ''),
            EmpCodeId: $("#hdfEmpCodeId").val(), 
            EmpNameId: $("#hdfEmpName").val(), 
            BranchCodeId: "0",//$("#hdfBranchCode").val(), 
            BranchNameId: $("#hdfBranchName").val(), 
            RaiserCodeId: $("#hdfRaiserCode").val(), 
            RaiserNameId: $("#hdfRaiserName").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetPettyCashAlert",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                self.loadGrid();

                var Data1 = "", Data2 = "";
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);

                    self.PettyCashArray(Data2);
                }
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    if (Data1[0].Message.length > 0) {
                        jAlert(Data1[0].Message, "Message");
                    }else if(self.PettyCashArray().length == 0){
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
            $('#ContentPettyCash input:checkbox:checked').each(function (index) {
                array[index] = $(this).attr('id');
            });
            
            var chkBit = array.length == self.PettyCashArray().length ? true : false;
            $("#chkHSelete").prop('checked', chkBit);
        }
    };

    self.SendOutStandingAlert = function () {
        var data = {
            ARFDateFrom: $("#txtARFFrom").val(),
            ARFDateTo: $("#txtARFTo").val(),
            ARFNo: $("#txtARFNo").val(),
            ARFAmount: $("#txtARFAmount").val().replace(/,/g, ''),
            EmpCodeId: $("#hdfEmpCodeId").val(),
            EmpNameId: $("#hdfEmpName").val(),
            BranchCodeId: "0",//$("#hdfBranchCode").val(), 
            BranchNameId: $("#hdfBranchName").val(),
            RaiserCodeId: $("#hdfRaiserCode").val(),
            RaiserNameId: $("#hdfRaiserName").val()
        };
        ShowLoading(true);
        $.ajax({
            type: "post",
            url: UrlDet + "GetPettyCashEmailAlert",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                ShowLoading(false);
                var Data1 = "";
              
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    if (Data1[0].Message.length > 0) {
                        jAlert(Data1[0].Message, "Message");
                    } else if (self.PettyCashArray().length == 0) {
                        jAlert("Sorry no records found!", "Message");
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                ShowLoading(false);
            }
        });
    };

    //Clear the form Entry
    self.clearFilter = function () {
        $("#txtARFFrom").val("");
        $("#txtARFTo").val("");
        $("#txtARFNo").val("");
        $("#txtARFAmount").val("");

        $("#hdfEmpCodeId").val("0");
        $("#txtEmpCode").val("");
        $("#hdfEmpName").val("0");
        $("#txtEmpName").val("");

        //$("#hdfBranchCode").val("0");
        //$("#txtBranchCode").val("");
        $("#hdfBranchName").val("0");
        $("#txtBranchName").val("");

        $("#hdfRaiserCode").val("0");
        $("#txtRaiserCode").val("");
        $("#hdfRaiserName").val("0");
        $("#txtRaiserName").val("");

        $("#chkHSelete").prop('checked', false);
        self.loadGrid();
    };

    //Datatable Plugin codeing
    self.DatatableCall = function () {
        if ($("#gvPettyCash > tbody > tr").length == self.PettyCashArray().length) {
            $("#gvPettyCash").DataTable({
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
        $("#gvPettyCash").DataTable({
            "autoWidth": false,
            "destroy": true,
            "bFilter": false,
            "bLengthChange": false,
            "bPaginate": false,
            "bInfo": false
        }).destroy();
        self.PettyCashArray.removeAll();
    }
    self.loadGrid();
};

var mainViewModel = new PettyCashModel();
ko.applyBindings(mainViewModel);

$(document).ready(function () {
    //var objDate = new Date();
    //var Presentyear = objDate.getFullYear();  //+ Presentyear,

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

    //Load Branch Code Auto Complete
    /*
    $("#txtBranchCode").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfBranchCode").val("0");
        }

        $("#txtBranchCode").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "../Helper/GetAutoCompleteBranch",
                    data: "{ 'txt' : '" + $("#txtBranchCode").val() + "'}",
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
                $("#hdfBranchCode").val(i.item.val);
                $("#txtBranchCode").val(i.item.label);
            }
        });
    });

    $("#txtBranchCode").focusout(function () {
        var hdfId = $("#hdfBranchCode").val();
        var txtCurName = $("#txtBranchCode").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtBranchCode").val("");
        }
    });
    */
    //Load Branch Name Auto Complete
    $("#txtBranchName").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfBranchName").val("0");
        }

        $("#txtBranchName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteBranch",
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
                $("#hdfBranchName").val(i.item.val);
                $("#txtBranchName").val(i.item.label);
            }
        });
    });

    $("#txtBranchName").focusout(function () {
        var hdfId = $("#hdfBranchName").val();
        var txtCurName = $("#txtBranchName").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtBranchName").val("");
        }
    });

    $("#chkHSelete").click(function (e) {
        $(this).closest('table').find('td input:checkbox').prop('checked', this.checked);
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