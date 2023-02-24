UrlDet = UrlDet.replace("GetPaymentDump", "");
var SearchModel = function () {
    var self = this;
    self.PaymentDumpArray = ko.observableArray();

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

    this.search = function () {
        var BatchNo = $("#txtPaymentBatchNo").val();
        var PaymentDate = $("#txtPaymentDate").val();

        var data = {
            PaymentBatchNo: BatchNo,
            PaymentDate: PaymentDate
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetPaymentDump",
            data: JSON.stringify(data),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                self.loadGrid();

                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    self.PaymentDumpArray(Data1);

                    //Download the Excel Format
                    //ko.utils.postJson("/PaymentDump/DownloadExcel");
                } else {
                    jAlert("Sorry No Record Found!", "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.Download = function () {
        $.ajax({
            type: "post",
            url: UrlDet + "IsDownloadAvailable",
            data: '{}',
            async: false,
            cache : false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response != null && response != "" && response == "OK") {

                    //Download the Excel Format
                        ko.utils.postJson("/PaymentDump/DownloadExcel");
                   // ko.utils.postJson("/iem_miga/flexispend/PaymentDump/DownloadExcel");
                } else {
                    jAlert("Sorry, unable to process your request.please refresh page & try again!", "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.DatatableCall = function () {
        if ($("#gvDump > tbody > tr").length == self.PaymentDumpArray().length) {
            $("#gvDump").DataTable({
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
        $('#btnsearch').attr('disabled', true);

        $("#txtPaymentBatchNo").removeClass('required').removeClass('valid');
        $("#txtPaymentDate").removeClass('required').removeClass('valid');

        $("#txtPaymentBatchNo").addClass('required');
        $("#txtPaymentDate").addClass('required');

        $("#txtPaymentBatchNo").val("");
        $("#txtPaymentDate").val("");

        self.loadGrid();
    };

    self.loadGrid = function () {
        $("#gvDump").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.PaymentDumpArray.removeAll();
    }

    self.loadGrid();
};

var mainViewModel = new SearchModel();
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

    $(".fsDatepicker").keyup(function (e) {
        if (e.keyCode == 8) {
            $.datepicker._clearDate(this);
        }
        if (e.keyCode == 46) {
            $.datepicker._clearDate(this);
        }
        $(this).datepicker('show');
    });

    $('#btnsearch').attr('disabled', true);

    $("#txtPaymentBatchNo").change(function () {
        checkValidationSearch();
        var _data = $("#txtPaymentBatchNo").val();
        if (_data.trim() != "") {
            $("#txtPaymentBatchNo").removeClass('required');
            $("#txtPaymentBatchNo").addClass('valid');
        }
        else {
            $("#txtPaymentBatchNo").removeClass('valid');
            $("#txtPaymentBatchNo").addClass('required');
        }
    });

    $("#txtPaymentDate").change(function () {
        checkValidationSearch();
        var _data = $("#txtPaymentDate").val();
        if (_data.trim() != "") {
            $("#txtPaymentDate").removeClass('required');
            $("#txtPaymentDate").addClass('valid');
        }
        else {
            $("#txtPaymentDate").removeClass('valid');
            $("#txtPaymentDate").addClass('required');
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

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

function isEvent(evt) {
    return false;
}

function checkValidationSearch() {
    var _batchNo = $("#txtPaymentBatchNo").val();
    var _dtTo = $("#txtPaymentDate").val();

    if (_batchNo.trim() == "" || _dtTo.trim() == "") {
        $('#btnsearch').attr('disabled', true);
    }
    else {
        $('#btnsearch').attr('disabled', false);
    }
}
