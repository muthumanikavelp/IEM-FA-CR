var objDialogyAdd;
UrlDet = UrlDet.replace("AdvanceReport", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");
var SearchModel = function (id) {

    var self = this;

    self.ResultArray = ko.observableArray();

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

    self.search = function (id) {
        showProgress();
        var Date = $("#txtDate").val();
        var Dateto = $("#txtDateto").val();
        var ECFNo = $("#txtECFNo").val();
        var EmpId = "0";
        var SuppId = $("#hfVendor").val();
        var Raiserid = $("#hdfRaiser").val();
        var GL = $("#hdfGL").val();
        var Branch = $("#hdfbranch").val();
       // alert(GL);
        var InputFilter = {
            Date: Date,
            Dateto:Dateto,
            ECFNo: ECFNo,
            EmpId: EmpId,
            SuppId: SuppId,
            Raiserid: Raiserid,
            glno: GL,
            Branch: Branch
            
        };
        $.ajax({
            type: "post",
            url: UrlDet + "FetchAdvanceReport",
            data: ko.toJSON(InputFilter),
            async: false,
            contentType: "application/json;",
            
            success: function (response) {
                
                var Data1 = "", Data2 = "";
                $("#gvReport").DataTable({
                    "destroy": true,
                    "autoWidth": false,
                }).destroy();
                $('#gvReport tbody').empty();
               
                self.ResultArray("");
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                   
                    Data1 = JSON.parse(response.Data1);
                    
                    if (Data1[0].Clear == false) {
                        hideProgress();
                        jAlert(Data1[0].Message, "Message");
                        return false;
                    }
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    hideProgress();
                    Data2 = JSON.parse(response.Data2);
                self.ResultArray("");
             
                self.ResultArray(Data2);
            
             
                if (Data2 == "" && id != 0) {
                    hideProgress();
                    jAlert("Sorry No Record Found!", "Message");
                    return false;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                hideProgress();
                //alert(XMLHttpRequest);
                //alert(textStatus);
                //alert(errorThrown);
            }
        });
    };

    self.Download = function () {
        showProgress();
        var Date = $("#txtDate").val();
        var Dateto = $("#txtDateto").val();
        var ECFNo = $("#txtECFNo").val();
        var EmpId = "0";
        var SuppId = $("#hfVendor").val();

        var InputFilter = {
            Date: Date,
            Dateto: Dateto,
            ECFNo: ECFNo,
            EmpId: EmpId,
            SuppId: SuppId
        };
        $.ajax({
            type: "post",
            url: UrlDet + "DownloadAdvanceReport",
            data: ko.toJSON(InputFilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                if (response == "process") {
                    hideProgress();
                    ko.utils.postJson("../FAReport/DownloadExcelAdavanceRpt");
                } else {
                    hideProgress();
                    jAlert("Sorry No Record Found!", "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                hideProgress();
            }
        });
    };

    self.DatatableCall = function () {
        if ($("#gvReport > tbody > tr").length == self.ResultArray().length) {
            $("#gvReport").DataTable({
                "autoWidth": false,
                "destroy": true,
                "scrollX": true,
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
        $("#txtVendor").val("");
        $("#hfVendor").val("0");
        $("#txtECFNo").val("");
        $("#txtDate").val("");
        $("#txtDateto").val("")
        $("#txtRaiser").val("");
        $("#txtGl").val("");
        $("#txtbranch").val("");
       // self.search(0);
        //self.ResultArray.removeAll();
        ////self.loadGrid();
        //// $('#gvReport').empty();
        //self.ResultArray("");
        $("#gvReport").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.ResultArray([]);

    };

    //self.search(0);
};

var mainViewModel = new SearchModel();
ko.applyBindings(mainViewModel);

$(document).ready(function () {


    //Load Employee Name Auto Complete
    $("#txtRaiser").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfRaiser").val("0");
        }

        $("#txtRaiser").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteEmployee",
                    data: "{ 'txt' : '" + $("#txtRaiser").val() + "'}",
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
                $("#hdfRaiser").val(i.item.val);
                $("#txtRaiser").val(i.item.label);
            }
        });
    });



    //Load GL Auto Complete
    $("#txtGl").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfGL").val("0");
        }

        $("#txtGl").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteGLCode",
                    data: "{ 'txt' : '" + $("#txtGl").val() + "'}",
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
                $("#hdfGL").val(i.item.val);
                $("#txtGl").val(i.item.label);
            }
        });
    });



    //Load Branch Auto Complete
    $("#txtbranch").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfbranch").val("0");
        }

        $("#txtbranch").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteBranch",
                    data: "{ 'txt' : '" + $("#txtbranch").val() + "'}",
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
                $("#hdfbranch").val(i.item.val);
                $("#txtbranch").val(i.item.label);
            }
        });
    });


    var objDate = new Date();
    var Presentyear = objDate.getFullYear();
    var pickeropts = {
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy',
        setDate: objDate
    };
    $("#txtDateto").datepicker(pickeropts);
    $("#txtDate").datepicker(pickeropts);

    //$("#txtDateto").datepicker({
    //    yearRange: '1900:' + Presentyear,
    //    changeMonth: true,
    //    changeYear: true,
    //    maxDate: 'd',
    //    dateFormat: 'dd/mm/yy',
    //    setDate: objDate
    //});

    $("#txtDateto").keyup(function (e) {
        if (e.keyCode == 8) {
            $.datepicker._clearDate(this);
        }
        if (e.keyCode == 46) {
            $.datepicker._clearDate(this);
        }
        $(this).datepicker('show');
    });
    //$("#txtDate").datepicker({
    //    yearRange: '1900:' + Presentyear,
    //    changeMonth: true,
    //    changeYear: true,
    //    maxDate: 'd',
    //    dateFormat: 'dd/mm/yy',
    //    setDate: objDate
    //});

    $("#txtDate").keyup(function (e) {
        if (e.keyCode == 8) {
            $.datepicker._clearDate(this);
        }
        if (e.keyCode == 46) {
            $.datepicker._clearDate(this);
        }
        $(this).datepicker('show');
    });

    $("#txtVendor").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: UrlHelper + "GetAutoCompleteSupplierAll",
                data: "{ 'txt' : '" + $("#txtVendor").val() + "'}",
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
                },
                failure: function (response) {
                }
            });
        },
        select: function (e, i) {
            $("#hfVendor").val(i.item.val);
            $("#txtVendor").val(i.item.label);
        },
        minLength: 1
    });

    $("#txtVendor").focusout(function () {
        var hdfId = $("#hfVendor").val();
        var txtCurName = $("#txtVendor").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtVendor").val("");
        }
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

function isEvent(evt) {
    return false;
}
