UrlDet = UrlDet.replace("GetASMSHistory", "");
var ASMSModel = function () {
    var self = this;
    self.VendorHistoryArray = ko.observableArray();

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

    self.ShortDescription = function (data) {
        if (data != null && data.length > 37) {
            data = data.substring(0, 37) + '...';
        } return data;
    };

    self.Clear = function () {
        $("#txtDateFrom").val("");
        $("#txtDateTo").val("");
        $("#ddlType").val("");
        $("#ddlReqStatus").val("");
        $("#txtDateFrom").removeClass('valid').addClass('required');
        $("#txtDateTo").removeClass('valid').addClass('required');
        $("#ddlType").removeClass('valid').addClass('required');

        self.loadVendorSummaryGrid();
    };

    self.Search = function () {
        var DateFrom = $("#txtDateFrom").val();
        var DateTo = $("#txtDateTo").val();
        var RequestType = $("#ddlType").val();
        if ($.trim(RequestType) == "0" || $.trim(RequestType) == "") {
            jAlert("Ensure Request Type!", "Message");
            return false;
        }
        if ($.trim(DateFrom) == "") {
            jAlert("Ensure Date From!", "Message");
            return false;
        }

        if ($.trim(DateTo) == "") {
            jAlert("Ensure Date To!", "Message");
            return false;
        }

        showProgress();
        setTimeout(function () {
            var _Filter = {
                DateFrom: DateFrom,
                DateTo: DateTo,
                RequestType: RequestType,
                RequestStatus: $("#ddlReqStatus").val()
            };
            self.loadVendorSummaryGrid();
            $.ajax({
                type: "post",
                url: UrlDet + "GetASMSHistory",
                data: JSON.stringify(_Filter),
                contentType: "application/json;",
                success: function (response) {
                    hideProgress();
                    var Data1 = "";
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                        Data1 = JSON.parse(response.Data1);
                    }
                    self.VendorHistoryArray(Data1);
                    if (self.VendorHistoryArray().length == 0) {
                        jAlert("Sorry! No Records Found.", "Message");
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    hideProgress();
                }
            });
        }, 500);
    };

    self.DatatableCall = function () {
        if ($("#gvVendorSummary > tbody > tr").length == self.VendorHistoryArray().length) {
            $("#gvVendorSummary").DataTable({
                "autoWidth": false,
                "destroy": true,
                "scrollX": true,
                "aoColumnDefs": [{
                    "aTargets": ["nosort"],
                    "bSortable": false
                }, {
                    "aTargets": ["colDate"],
                    "bSortable": true,
                    "sType": "date-uk"
                }]
                //{ "targets": [1], "visible": self.IsVendorAction() }
            });
        }
    };
    self.loadVendorSummaryGrid = function () {
        $("#gvVendorSummary").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.VendorHistoryArray([]);
    }
    self.loadVendorSummaryGrid();
};

var mainViewModel = new ASMSModel();
ko.applyBindings(mainViewModel);

$(document).ready(function () {
    var objDate = new Date();
    var Presentyear = objDate.getFullYear();
    //$(".fsDatepicker").datepicker({
    //    yearRange: '1900:' + Presentyear,
    //    changeMonth: true,
    //    changeYear: true,
    //    maxDate: 'd',
    //    dateFormat: 'dd/mm/yy'
    //});

    $("#txtDateFrom,#txtDateTo").keyup(function (e) {
        if (e.keyCode == 8 || e.keyCode == 46)
            $.datepicker._clearDate(this);

        $(this).datepicker('show');
        $(this).removeClass("valid").addClass("required");
    });

    $("#txtDateFrom").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        maxDate: 'd',
        onSelect: function (selected) {
            $("#txtDateFrom").removeClass('required').addClass('valid');
            $("#txtDateTo").datepicker("option", "minDate", selected)
        }
    });

    $("#txtDateTo").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        maxDate: 'd',
        onSelect: function (selected) {
            var month = selected.split("/")[1];
            $("#txtDateTo").removeClass('required').addClass('valid');
            $("#txtDateFrom").datepicker("option", "maxDate", selected)
        }
    });

    jQuery.extend(jQuery.fn.dataTableExt.oSort, {
        "date-uk-pre": function (a) {
            a = a.split(">")[1].split("<")[0];
            var ukDatea = "";
            if (a.indexOf('/') == -1) {
                // will not be triggered because str has _..
                ukDatea = a.split("-");
            } else {
                ukDatea = a.split("/");
            }
            return (ukDatea[2] + ukDatea[1] + ukDatea[0]) * 1;
        },
        "date-uk-asc": function (a, b) {
            return ((a < b) ? -1 : ((a > b) ? 1 : 0));
        },
        "date-uk-desc": function (a, b) {
            return ((a < b) ? 1 : ((a > b) ? -1 : 0));
        }
    });

    $("#ddlType").change(function () {
        var Type = $.trim($("#ddlType").val());
        if (Type != "0" && Type != "") {
            $("#ddlType").addClass("valid");
            $("#ddlType").removeClass("required");
        }
        else {
            $("#ddlType").addClass("required");
            $("#ddlType").removeClass("valid");
        }
    });
});

function isEvent(evt) {
    return false;
}
