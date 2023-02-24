UrlDet = UrlDet.replace("GetChequeListDetails", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");
var ChequeInventoryModel = function () {
    var self = this;

    //bank dropdown array
    self.BankArray = ko.observableArray();

    self.ShortDescription = function (data) {
        if(data.length > 40){
            data = data.substring(0, 40)+ '...';
        } return data;
    };

    self.ChequeBookDetails = ko.observableArray();

    self.loadBank = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "LoadBankDropDown",
            data: '{}',
            contentType: "application/json;",
            success: function (response) {
                self.BankArray(response);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.GetBankBookDetails = function (obj, event) {
        var BankId = ($("#ddlEBank").val() == null || $("#ddlEBank").val() == "0" || $("#ddlEBank").val() == "") ? "0" : $("#ddlEBank").val();
        var tmpQuery = {
            BankId: BankId,
            ChqBookNo: '',
            ViewType: '0'
        };

        $.ajax({
            type: "post",
            url: UrlDet + "GetChequeListDetails",
            data: JSON.stringify(tmpQuery),
            contentType: "application/json;",
            success: function (response) {
                self.ChequeBookDetails.removeAll();
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    self.ChequeBookDetails(Data1);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    }

    self.SaveCHQEntry = function () {

        var valueChqNoFrom = $("#txtECHQNoFrom").val();
        var valueChqNoTo = $("#txtECHQNoTo").val();
        if (valueChqNoFrom.length != 6) {
            jAlert("Ensure Valid ChequeNo From", "Message");
            return false;
        }
        if (valueChqNoTo.length != 6) {
            jAlert("Ensure Valid chequeNo To", "Message");
            return false;
        }

        var data = {
            BankId: $("#ddlEBank").val(),
            Date: $("#txtEDate").val(),
            ChqBookNo: $("#txtECHQBookNo").val(),
            ChqNoFrom: $("#txtECHQNoFrom").val(),
            ChqNoTo: $("#txtECHQNoTo").val(),
            Reason: "",
            ViewType: "0"
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SaveChequeInventory",
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
                        self.GetBankBookDetails();
                        self.ClearCHQEntry();
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

    self.SaveDmg = function () {
        var data = {
            BankId: $("#ddlDBank").val(),
            Date: "",
            ChqBookNo: $("#txtDCHQBookNo").val(),
            ChqNoFrom: $("#txtDCHQNo").val(),
            ChqNoTo: "",
            Reason: $("#txtDReason").val(),
            ViewType: "1"
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SaveChequeInventory",
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
                        self.ClearDamageEntry();
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

    self.ClearDamageEntry = function () {
        //$("#ddlDBank").val("0");
        $("#txtDReason").val("");
        $("#txtDCHQNo").val("");
        $("#txtDCHQBookNo").val("");

        $('#btnDSave').attr('disabled', true);

        $("#txtDCHQBookNo").removeClass('required').removeClass('valid');
        $("#txtDCHQBookNo").addClass('required');

        $("#txtDCHQNo").removeClass('required').removeClass('valid');
        $("#txtDCHQNo").addClass('required');

        $("#txtDReason").removeClass('required').removeClass('valid');
        $("#txtDReason").addClass('required');
    };

    self.ClearCHQEntry = function () {
        //$("#ddlEBank").val("0");
        $("#txtEDate").val("");
        $("#txtECHQBookNo").val("");
        $("#txtECHQNoFrom").val("");
        $("#txtECHQNoTo").val("");

        $('#btnESave').attr('disabled', true);

        $("#txtEDate").removeClass('required').removeClass('valid');
        $("#txtEDate").addClass('required');

        $("#txtECHQBookNo").removeClass('required').removeClass('valid');
        $("#txtECHQBookNo").addClass('required');

        $("#txtECHQNoFrom").removeClass('required').removeClass('valid');
        $("#txtECHQNoFrom").addClass('required');

        $("#txtECHQNoTo").removeClass('required').removeClass('valid');
        $("#txtECHQNoTo").addClass('required');
    };

    //load the Bank DropDown.
    self.loadBank();
};

var mainViewModel = new ChequeInventoryModel();
ko.applyBindings(mainViewModel);


$(document).ready(function () {
    var objDate = new Date();
    var Presentyear = objDate.getFullYear();
    $(".fsdatepicker").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
    });

    $('#btnESave').attr('disabled', true);
    $('#btnDSave').attr('disabled', true);

    //cheque Book Entry Validation
    $("#txtEDate").change(function () {
        CHQBookEntry();
        var _date = $("#txtEDate").val();
        if (_date.trim() != "") {
            $("#txtEDate").removeClass('required');
            $("#txtEDate").addClass('valid');
        }
        else {
            $("#txtEDate").removeClass('valid');
            $("#txtEDate").addClass('required');
        }
    });

    $("#txtECHQBookNo").keyup(function () {
        CHQBookEntry();
        var _ECHQBookNo = $("#txtECHQBookNo").val();
        if (_ECHQBookNo.trim() != "") {
            $("#txtECHQBookNo").removeClass('required');
            $("#txtECHQBookNo").addClass('valid');
        }
        else {
            $("#txtECHQBookNo").removeClass('valid');
            $("#txtECHQBookNo").addClass('required');
        }
    });

    $("#txtECHQNoFrom").keyup(function () {
        CHQBookEntry();
        var _ECHQNoFrom = $("#txtECHQNoFrom").val();
        if (_ECHQNoFrom.trim() != "" && _ECHQNoFrom.length == 6 ) {
            $("#txtECHQNoFrom").removeClass('required');
            $("#txtECHQNoFrom").addClass('valid');
        }
        else {
            $("#txtECHQNoFrom").removeClass('valid');
            $("#txtECHQNoFrom").addClass('required');
        }
    });

    $("#txtECHQNoTo").keyup(function () {
        CHQBookEntry();
        var _ECHQNoTo = $("#txtECHQNoTo").val();
        if (_ECHQNoTo.trim() != "" && _ECHQNoTo.length == 6) {
            $("#txtECHQNoTo").removeClass('required');
            $("#txtECHQNoTo").addClass('valid');
        }
        else {
            $("#txtECHQNoTo").removeClass('valid');
            $("#txtECHQNoTo").addClass('required');
        }
    });

    //Damage cheque Book Entry Validation
    $("#txtDCHQBookNo").keyup(function () {
        DMGCHQEntry();
        var _DCHQBookNo = $("#txtDCHQBookNo").val();
        if (_DCHQBookNo.trim() != "") {
            $("#txtDCHQBookNo").removeClass('required');
            $("#txtDCHQBookNo").addClass('valid');
        }
        else {
            $("#txtDCHQBookNo").removeClass('valid');
            $("#txtDCHQBookNo").addClass('required');
        }
    });

    $("#txtDCHQNo").keyup(function () {
        DMGCHQEntry();
        var _DCHQNo = $("#txtDCHQNo").val();
        if (_DCHQNo.trim() != "" && _DCHQNo.length == 6) {
            $("#txtDCHQNo").removeClass('required');
            $("#txtDCHQNo").addClass('valid');
        }
        else {
            $("#txtDCHQNo").removeClass('valid');
            $("#txtDCHQNo").addClass('required');
        }
    });

    $("#txtDReason").keyup(function () {
        DMGCHQEntry();
        var _DReason = $("#txtDReason").val();
        if (_DReason.trim() != "") {
            $("#txtDReason").removeClass('required');
            $("#txtDReason").addClass('valid');
        }
        else {
            $("#txtDReason").removeClass('valid');
            $("#txtDReason").addClass('required');
        }
    });
});

function CHQBookEntry() {
    var _EDate = $("#txtEDate").val();
    var _ECHQBookNo = $("#txtECHQBookNo").val();
    var _ECHQNoFrom = $("#txtECHQNoFrom").val();
    var _ECHQNoTo = $("#txtECHQNoTo").val();

    if (_EDate == "" || _ECHQBookNo == "" || _ECHQNoFrom == "" || _ECHQNoFrom.length != 6 || _ECHQNoTo == "" || _ECHQNoTo.length != 6) {
        $('#btnESave').attr('disabled', true);
    } else {
        $('#btnESave').attr('disabled', false);
    }
}

function DMGCHQEntry() {
    var _DReason = $("#txtDReason").val();
    var _DCHQNo = $("#txtDCHQNo").val();
    var _DCHQBookNo = $("#txtDCHQBookNo").val();

    if (_DReason == "" || _DCHQNo == "" || _DCHQNo.length != 6 || _DCHQBookNo == "") {
        $('#btnDSave').attr('disabled', true);
    } else {
        $('#btnDSave').attr('disabled', false);
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