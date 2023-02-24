var objDialogy;
var chkBit = "0";

UrlDet = UrlDet.replace("GetPaymentReversalsMaker", "");

var PaymentReversalsModel = function () {
    var self = this;

    //load grid array
    self.PRMakerArray = ko.observableArray();
    self.PRMakerNewArray = ko.observableArray();
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

    self.Search = function () {
        var data = {
            PVDateFrom: $("#txtDateFromS").val(),
            PVDateTo: $("#txtDateToS").val(),
            PVNo: $("#txtPvNoS").val(),
            Status: "",
            ViewType: "1"
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetPaymentReversalsMaker",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                self.loadGrid();
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
                    self.PRMakerArray(Data2);
                }
                else {
                    jAlert("Sorry! No records found.", "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.NewSearch = function () {
        var data = {
            PVDateFrom: "",
            PVDateTo: "",
            PVNo: $("#txtNewPvNo").val(),
            Status: "",
            ViewType: "0"
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetPaymentReversalsMaker",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                self.loadGridNew();
                var Data1 = "", Data2 = "";

                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    self.PRMakerNewArray(Data2);
                }
                else {
                    if (chkBit == "0") {
                        jAlert("Sorry! No records found.", "Message");
                    }
                }

                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Message != "" && chkBit == "0") {
                        jAlert(Data1[0].Message, "Message");
                    } else {
                        chkBit = "0";
                    }

                    $("#txtReversalDate").val(Data1[0].CurrentDate);
                }
                
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.create = function () {
        var _PvId = "0";
       // alert('a');
       // _PvId = $("#hdPVId").val();
        //if (_PvId.trim() == "" || _PvId == "0") {
        //    jAlert("Ensure valid data!", "Message");
        //    return false;
        //}

        var data = {
            PvId: $("#txtPVNo").val(),
            ReversalDate: $("#txtReversalDate").val(),
            Reason: $("#txtReason").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetPaymentReversalsMaker",
            data: JSON.stringify(data),
            contentType: "application/json;",
            async: false,
            success: function (response) {
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message
                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");
                    }

                    if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {
                        $('#ShowDialog').attr("style", "display:none;");
                        objDialogy.dialog("close");
                        chkBit = "1";
                        self.NewSearch();
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });

    };

    self.view = function (item) {
        $("#hdPVId").val("0");
        $("#txtPVNo").val("");
        $("#txtPVDate").val("");
       // $("#txtPayMode").val("");
        $("#txtAmount").val("");
        $("#txtBeneficiary").val("");
        $("#txtReason").val("");

        //assign data's
        $("#hdPVId").val(item.PvId);
        $("#txtPVNo").val(item.PvNo);
        $("#txtPVDate").val(item.PvDate);
      //  $("#txtPayMode").val(item.PayMode);
        $("#txtAmount").val(self.formatNumber(item.PvAmount));
        $("#txtBeneficiary").val(item.Beneficiary);
        
        $("#txtReason").removeClass('required').removeClass('valid');

        $("#txtReason").addClass('required');

        PREntryValidator();

        $('#ShowDialog').attr("style", "display:block;");
        objDialogy.dialog({ title: 'Payment Reversals' });
        objDialogy.dialog("open");
    };
        
    self.Clear = function () {
        $("#txtDateFromS").val("");
        $("#txtDateToS").val("");
        $("#txtPvNoS").val("");
        $("#ddlStatusS").val("0");

        self.loadGrid();
    }
    self.NewClear = function () {
        $("#txtNewPvNo").val("");
        self.loadGridNew();
    }

    self.NewDatatableCall = function () {
        if ($("#gvNewPRDetails > tbody > tr").length == self.PRMakerNewArray().length) {
            $("#gvNewPRDetails").DataTable({
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
    }
    self.DatatableCall = function () {
        if ($("#gvPRDetails > tbody > tr").length == self.PRMakerArray().length) {
            $("#gvPRDetails").DataTable({
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

    self.loadGridNew = function () {
        $("#gvNewPRDetails").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.PRMakerNewArray.removeAll();
    }
    self.loadGrid = function () {
        $("#gvPRDetails").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.PRMakerArray.removeAll();
    }
    self.loadGrid();
    self.loadGridNew();
};

var mainViewModel = new PaymentReversalsModel();
ko.applyBindings(mainViewModel);


$(document).ready(function () {
    objDialogy = $("[id$='ShowDialog']");
    objDialogy.dialog({
        autoOpen: false,
        modal: true,
        width: 600,
        height: 400,
        duration: 300
    });

    var objDate = new Date();
    var Presentyear = objDate.getFullYear();
    $(".fsDatePicker").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
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

    PREntryValidator();

    //popup validation
    $("#txtPVNo").change(function () {
        PREntryValidator();

        var _data = $("#txtPVNo").val();
        if (_data.trim() != "") {
            $("#txtPVNo").removeClass('required');
            $("#txtPVNo").addClass('valid');
        }
        else {
            $("#txtPVNo").removeClass('valid');
            $("#txtPVNo").addClass('required');
        }
    });

    $("#txtReason").keyup(function () {
        PREntryValidator();

        var _data = $("#txtReason").val();
        if (_data.trim() != "") {
            $("#txtReason").removeClass('required');
            $("#txtReason").addClass('valid');
        }
        else {
            $("#txtReason").removeClass('valid');
            $("#txtReason").addClass('required');
        }
    });
});

function PREntryValidator() {
    var _reason = $("#txtReason").val();

    if (_reason.trim() == "") {
        $('#btnUpdate').attr('disabled', true);
    } else {
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


/*self.GetPVDetails = function () {
        if ($("#txtPVNo").val().trim() == "") {
            return false;
        }

       var tmpData = {
            "MemoNo": $("#txtMemoNo").val(),
            "PVNo": $("#txtPVNo").val()
        };
        $.ajax({
            type: "POST",
            url: '../EFT/GetMemoDetails',
            data: JSON.stringify(tmpData),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.eftMemoDetails != undefined) {
                    self.EFTStatus(data.eftMemoDetails[0]);
                    $("#txtMemoNo").removeClass('required').removeClass('valid');
                    $("#txtPVNo").removeClass('required').removeClass('valid');
                    $("#txtMemoNo").addClass('valid');
                    $("#txtPVNo").addClass('valid');
                    var Date = $("#txtDate").val();
                    var Reason = $("#txtRemarks").val();
                    if (Date != "" && Reason != "") {
                        $("#btnSubmitEFTStatus").attr("disabled", false);
                    }
                    else
                        $("#btnSubmitEFTStatus").attr("disabled", true);
                }
                else {
                    $("#txtMemoNo").removeClass('required').removeClass('valid');
                    $("#txtPVNo").removeClass('required').removeClass('valid');
                    $("#txtMemoNo").addClass('required');
                    $("#txtPVNo").addClass('required');
                    $("#btnSubmitEFTStatus").attr("disabled", true);
                    self.EFTStatus(null);
                    jAlert(data.msg.MessageText, "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    };*/