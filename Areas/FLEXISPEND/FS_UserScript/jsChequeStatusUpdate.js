
UrlDet = UrlDet.replace("GetChequeStatusUpdate", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");

var ChequeInventoryModel = function () {
    var self = this;

    //grid array
    self.ChqStatusUpdArray = ko.observableArray();

    self.ShortDate = function (data) {
        return data.substring(0,10);
    };

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

    self.GetDetails = function (obj, event) {
        if ($("#txtChequeNo").val().trim() == "" || $("#txtPVNo").val().trim() == "") {
            return false;
        }

        var data = {
            ChqNo: $("#txtChequeNo").val(),
            PvNo: $("#txtPVNo").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetChequeStatusUpdate",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {                
                var Data1 = "", Data2 = "";
                if (response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");

                        $("#txtPVNo").removeClass('required').removeClass('valid');
                        $("#txtPVNo").addClass('required');
                        $("#txtPVNo").val("");
                    }
                }

                if (response.Data2 != "" && JSON.parse(response.Data2) != null) {                    
                    Data2 = JSON.parse(response.Data2);

                    $("#lblDocAmount").text(self.formatNumber(Data2[0].Amount));
                    $("#lblChequeDate").text(Data2[0].ChqDate);
                    $("#lblBank").text(Data2[0].Bank);
                    $("#lblBeneficiary").text(Data2[0].Beneficiary);
                } else {
                    $("#lblDocAmount").text("");
                    $("#lblChequeDate").text("");
                    $("#lblBank").text("");
                    $("#lblBeneficiary").text("");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
        checkValidationSearch();
    };

    this.Checked = function (obj, event) {
        if (event.originalEvent) {
            var _inBit = event.originalEvent.target.defaultValue;
            if (_inBit == "1") {
                $("#txtRemarks").attr("disabled", "disabled");
                $("#txtRemarks").removeClass('required').removeClass('valid');
                $("#txtRemarks").val("");
            }
            else {
                $("#txtRemarks").removeAttr("disabled");
                $("#txtRemarks").addClass('required');
            }
            checkValidationSearch();
        }
    };

    //Submit
    self.create = function () {
        /*var array = new Array();
        $('#tabs-1 input:radio:checked').each(function (index) {
            array[index] = $(this).attr('value');
        });

        var _ChkValue = "", _ChkText = "";
        $.each(array, function (index) {
            _ChkValue += array[index];
        });*/
        /*if (_ChkValue == "2") {
            count = $("#txtRemarks").val();
            if (count == "" || count == "0") {
                jAlert("Ensure Reject Reason!", "Message");
                return false;
            }
            else {
                _ChkText = "Reject";
            }
        }
        else {
            _ChkText = "Cleared"
        }*/

        var count = $("#txtChequeNo").val();
        if (count == "" || count == "0") {
            jAlert("Ensure Cheque No!", "Message");
            return false;
        }
        count = $("#txtPVNo").val();
        if (count == "" || count == "0") {
            jAlert("Ensure PV No!", "Message");
            return false;
        }
        count = $("#txtDate").val();
        if (count == "" || count == "0") {
            jAlert("Ensure Date!", "Message");
            return false;
        }
        
        var _ChkValue = "", _ChkText = "";
        _ChkValue = $("input[name=modeANf]:checked").val();
        _ChkText = _ChkValue == "2" ? "Reject" : "Cleared";
        
        var data = {
            ChqNo: $("#txtChequeNo").val(),
            PvNo: $("#txtPVNo").val(),
            ChqStatus: _ChkText,
            Date: $("#txtDate").val(),
            Reason: $("#txtRemarks").val(),
            ViewType: "0"

        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetChequeStatusUpdate",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);

                //show Message if error
                if (Data1[0].Message != "") {
                    jAlert(Data1[0].Message, "Message");
                }
                if (Data1[0].Clear == "True" || Data1[0].Clear == "true" || Data1[0].Clear == "1") {
                    self.ClearSearch();
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };
    // clear
    self.ClearSearch = function () {
        $("#txtChequeNo").val("");
        $("#txtPVNo").val("");
        $("#lblDocAmount").text("");
        $("#lblChequeDate").text("");
        $("#lblBank").text("");
        $("#lblBeneficiary").text("");
        $("#txtDate").val("");
        $("#txtRemarks").val("");

        $('input:radio[name=modeANf][value=1]').prop('checked', true);
        $("#txtRemarks").removeAttr("disabled");
        $("#txtRemarks").removeClass('required').removeClass('valid');
        $("#txtRemarks").val("");
        $("#txtRemarks").attr("disabled", "disabled");

        $("#txtDate").removeClass('required').removeClass('valid');
        $("#txtDate").addClass('required');

        $("#txtChequeNo").removeClass('required').removeClass('valid');
        $("#txtChequeNo").addClass('required');

        $("#txtPVNo").removeClass('required').removeClass('valid');
        $("#txtPVNo").addClass('required');
        checkValidationSearch();
    };

    self.upload = function () {
        var count = $("#txtBulkDate").val();
        if (count == "" || count == "0") {
            jAlert("Ensure Date!", "Message");
            $("#attUploader").val("");
            return false;
        }
        count = $("#attUploader").val();
        if (count == "" || count == "0") {
            jAlert("Ensure FileName!", "Message");
            return false;
        }

        var data = {
            ChqNo: "0",
            PvNo: "0",
            ChqStatus: "",
            Date: "",
            Reason: "",
            ViewType: "1"

        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetChqStatusUpdateExcel",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);

                if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {
                    jAlert(Data1[0].Message, "Message");                    
                }
                else {
                    self.ChqStatusUpdArray.removeAll();
                }

                //show Message if error
                if (Data1[0].Message != "") {
                    jAlert(Data1[0].Message, "Message");
                }

                if (response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);                    
                    self.ChqStatusUpdArray(Data2);
                }
                $("#attUploader").val("");
                $('#uploaderForm')[0].reset();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.ClearSearchExcel = function () {
        $("#txtBulkDate").val("");
        $("#attUploader").val("");

        $('#uploaderForm')[0].reset();

        $('#btnSubmitUpload').attr('disabled', true);
        $("#txtBulkDate").removeClass('valid');
        $("#txtBulkDate").addClass('required');

        self.ChqStatusUpdArray.removeAll();
    }

    self.Download = function () {
        window.location = UrlDef + "/Template/ChequeStatusUploadTemplate.xls";
    };
};

var mainViewModel = new ChequeInventoryModel();

ko.applyBindings(mainViewModel);


$(document).ready(function () { 
    var objDate = new Date();
    var Presentyear = objDate.getFullYear();
    $("#txtDate").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
    });

    $("#txtBulkDate").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
    });
    
    $('#btnsearch').attr('disabled', true);
    $("#btnSubmitUpload").attr("disabled", true);

    $("#txtChequeNo").change(function () {
        checkValidationSearch();
        var txtDate = $("#txtChequeNo").val();
        if (txtDate.trim() != "") {
            $("#txtChequeNo").removeClass('required');
            $("#txtChequeNo").addClass('valid');
        }
        else {
            $("#txtChequeNo").removeClass('valid');
            $("#txtChequeNo").addClass('required');
        }
    });

    $("#txtPVNo").change(function () {
        checkValidationSearch();
        var txtDate = $("#txtPVNo").val();
        if (txtDate.trim() != "") {
            $("#txtPVNo").removeClass('required');
            $("#txtPVNo").addClass('valid');
        }
        else {
            $("#txtPVNo").removeClass('valid');
            $("#txtPVNo").addClass('required');
        }
    });

    $("#txtChequeNo").keyup(function () {
        var txtFrom = $("#txtChequeNo").val();
        if (txtFrom.trim() != "") {
            $("#txtChequeNo").removeClass('required');
            $("#txtChequeNo").addClass('valid');
        }
        else {
            $("#txtChequeNo").removeClass('valid');
            $("#txtChequeNo").addClass('required');
        }
    });

    $("#txtPVNo").keyup(function () {

        var txtFrom = $("#txtPVNo").val();
        if (txtFrom.trim() != "") {
            $("#txtPVNo").removeClass('required');
            $("#txtPVNo").addClass('valid');
        }
        else {
            $("#txtPVNo").removeClass('valid');
            $("#txtPVNo").addClass('required');
        }
    });

    $("#txtDate").change(function () {
        checkValidationSearch();

        var txtFrom = $("#txtDate").val();
        if (txtFrom.trim() != "") {
            $("#txtDate").removeClass('required');
            $("#txtDate").addClass('valid');
        }
        else {
            $("#txtDate").removeClass('valid');
            $("#txtDate").addClass('required');
        }
    });

    function fnCheckValidationForBulkUpload() {
        var Date = $("#txtBulkDate").val();
        var FileUplaod = $("#attUploader").val();
        if (Date != "" && FileUplaod != "")
            $("#btnSubmitUpload").attr("disabled", false);
        else
            $("#btnSubmitUpload").attr("disabled", true);
    };

    $("#txtBulkDate").change(function () {
        fnValidateIsRequired("txtBulkDate");
        fnCheckValidationForBulkUpload();
    });

    $("#txtRemarks").keyup(function () {
        checkValidationSearch();
        var txtFrom = $("#txtRemarks").val();
        if (txtFrom.trim() != "") {
            $("#txtRemarks").removeClass('required');
            $("#txtRemarks").addClass('valid');
        }
        else {
            $("#txtRemarks").removeClass('valid');
            $("#txtRemarks").addClass('required');
        }
    });

    $(".attUploader").on('change', function (e) {
        fnCheckValidationForBulkUpload();
        var iframe = $('<iframe name="postiframe" id="postiframe" style="display: none"></iframe>');
        $("body").append(iframe);
        var form = $('#uploaderForm');
        //form.attr("action", UrlDet + "UploadAttachment");
        var Attach_list = Attachment_list();
            var Attachment_fomat = Attached_fileformat();
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

function isEvent(evt) {
    return false;
}
function checkValidationSearch() {
    var chqNo = $("#txtChequeNo").val();
    var pvno = $("#txtPVNo").val();
    var dt = $("#txtDate").val();
    var _rmks = $("#txtRemarks").val();

    var _tmpStatus = $("input[name=modeANf]:checked").val();

    if (chqNo.trim() == "" || pvno.trim() == "" || dt.trim() == "" || (_tmpStatus == "2" && _rmks.trim()=="")) {
        $('#btnsearch').attr('disabled', true);
    }
    else {
        $('#btnsearch').attr('disabled', false);
    }
}