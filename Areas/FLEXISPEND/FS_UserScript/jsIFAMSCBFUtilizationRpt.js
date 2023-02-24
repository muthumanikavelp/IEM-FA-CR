var objDialogyAdd;
UrlDet = UrlDet.replace("FetchCBFUtilizationReport", "");
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
        var CDateFrom = $("#txtCDateFrom").val();
        var CDateTo = $("#txtCDateTo").val();
        var ADateFrom = $("#txtADateFrom").val();
        var ADateTo = $("#txtADateTo").val();
        var CBFNo = $("#txtCBFNo").val();

        if (CDateFrom == "" && CDateTo == "") {

        }
        else {
            if ((CDateFrom == "" || CDateTo == "") && id != 0) {
                jAlert("Please Enter Creation Date From & To..", "Message");
                return false;
            }
        }
        if (ADateFrom == "" && ADateTo == "") {

        } else {
            if ((ADateFrom == "" || ADateTo == "") && id != 0) {
                jAlert("Please Enter Approval Date From & To..", "Message");
                return false;
            }
        }

        var InputFilter = {
            CreationDateFrom: CDateFrom,
            CreationDateTo: CDateTo,
            ApprovalDateFrom: ADateFrom,
            ApprovalDateTo: ADateTo,
            CBFNo: CBFNo
        };
        $.ajax({
            type: "post",
            url: UrlDet + "FetchCBFUtilizationReport",
            data: ko.toJSON(InputFilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";

                $("#gvReport").DataTable({
                    "destroy": true
                }).destroy();
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
        var CDateFrom = $("#txtCDateFrom").val();
        var CDateTo = $("#txtCDateTo").val();
        var ADateFrom = $("#txtADateFrom").val();
        var ADateTo = $("#txtADateTo").val();
        var CBFNo = $("#txtCBFNo").val();

        if (CDateFrom == "" && CDateTo == "") {

        }
        else {
            if ((CDateFrom == "" || CDateTo == "") && id != 0) {
                jAlert("Please Enter Creation Date From & To..", "Message");
                return false;
            }
        }
        if (ADateFrom == "" && ADateTo == "") {

        } else {
            if ((ADateFrom == "" || ADateTo == "") && id != 0) {
                jAlert("Please Enter Approval Date From & To..", "Message");
                return false;
            }
        }

        var InputFilter = {
            CreationDateFrom: CDateFrom,
            CreationDateTo: CDateTo,
            ApprovalDateFrom: ADateFrom,
            ApprovalDateTo: ADateTo,
            CBFNo: CBFNo
        };
        $.ajax({
            type: "post",
            url: UrlDet + "DownloadUtilizationReport",
            data: ko.toJSON(InputFilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                if (response == "process") {
                    ko.utils.postJson(UrlDet + "DownloadExcelUtilizationRpt");
                } else {
                    jAlert("Sorry No Record Found!", "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
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
        $("#txtCDateFrom,#txtCDateTo,#txtADateFrom,#txtADateTo,#txtCBFNo").val("");
        self.search(0);
    };

    self.search(0);
};

var mainViewModel = new SearchModel();
ko.applyBindings(mainViewModel);

$(document).ready(function () {
    //$("#txtDateFrom,#txtDateTo").datepicker({
    //    yearRange: '1900:+nn',
    //    changeMonth: true,
    //    changeYear: true,
    //    maxDate: 'd',
    //    dateFormat: 'dd/mm/yy'
    //});
    $("#txtCDateFrom").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        numberOfMonths: 1,
        onSelect: function (selected) {
            $("#txtCDateTo").datepicker("option", "minDate", selected)
        }
    });

    $("#txtADateFrom").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        numberOfMonths: 1,
        onSelect: function (selected) {
            $("#txtADateTo").datepicker("option", "minDate", selected)
        }
    });

    $("#txtCDateTo").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        numberOfMonths: 1,
        onSelect: function (selected) {
            $("#txtCDateFrom").datepicker("option", "maxDate", selected)
        }
    });

    $("#txtADateTo").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        numberOfMonths: 1,
        onSelect: function (selected) {
            $("#txtADateFrom").datepicker("option", "maxDate", selected)
        }
    });

    $("#txtADateTo,#txtCDateTo,#txtADateFrom,#txtCDateFrom").keyup(function (e) {
        if (e.keyCode == 8) {
            $.datepicker._clearDate(this);
        }
        if (e.keyCode == 46) {
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
});

function isEvent(evt) {
    return false;
}
