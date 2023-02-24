var objDialogyAdd;
UrlDet = UrlDet.replace("FetchRetentionReport", "");
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

    this.search = function (id) {
        
        var Date = $("#txtDate").val();
        var ECFNo = $("#txtECFNo").val();
        var EmpId = "0";
        var SuppId = $("#hfVendor").val();

        var InputFilter = {
            Date: Date,
            ECFNo: ECFNo,
            EmpId: EmpId,
            SuppId: SuppId
        };
        $.ajax({
            type: "post",
            url: UrlDet + "FetchRetentionReport",
            data: ko.toJSON(InputFilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                $("#gvReport").DataTable({ "destroy": true }).destroy();                
                self.ResultArray([]);

                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == false) {
                        jAlert(Data1[0].Message, "Message");
                        return false;
                    }
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                self.ResultArray(Data2);
                if (Data2 == "" && id != 0) {
                    jAlert("Sorry No Record Found!", "Message");
                    return false;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.Download = function () {
        var Date = $("#txtDate").val();
        var ECFNo = $("#txtECFNo").val();
        var EmpId = "0";
        var SuppId = $("#hfVendor").val();

        var InputFilter = {
            Date: Date,
            ECFNo: ECFNo,
            EmpId: EmpId,
            SuppId: SuppId
        };
        $.ajax({
            type: "post",
            url: UrlDet + "DownloadRetentionReport",
            data: ko.toJSON(InputFilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                if (response == "process") {
                    ko.utils.postJson(UrlDet + "DownloadExcelRetentionRpt");
                } else {
                    jAlert("Sorry No Record Found!", "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    };

    this.DatatableCall = function () {
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
        self.search(0);
    };

    self.search(0);
};

var mainViewModel = new SearchModel();
ko.applyBindings(mainViewModel);

$(document).ready(function () {
    
    var objDate = new Date();
    var Presentyear = objDate.getFullYear();
    $("#txtDate").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy',
        setDate: objDate
    });

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
