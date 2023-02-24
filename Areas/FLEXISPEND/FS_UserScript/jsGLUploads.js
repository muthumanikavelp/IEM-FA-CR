var objShowReceiptDet;
var objECFDet;

UrlDet = UrlDet.replace("DownloadExcel", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");

var SearchModel = function () {

    var self = this;
    self.UploadTypeArray = ko.observableArray();

    self.CUploadTypeArray = ko.observableArray([]);
    self.CBatchNoArray = ko.observableArray([]);

    self.LoadUploadType = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "LoadGLUploadTypes",
            data: "{ }",
            contentType: "application/json;",
            success: function (response) {
                self.UploadTypeArray(response);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.LoadUploadTypeCancellation = function (viewType) {
        var tmpData = {
            "UploadDate": $("#txtCUploadDate").val(),
            "UploadTypeId": $("#ddlCUploadType").val() == "" ? "0" : $("#ddlCUploadType").val(),
            "ViewType": viewType
        };

        $.ajax({
            type: "post",
            url: UrlDet + "LoadCUploadDetails",
            data: JSON.stringify(tmpData),
            contentType: "application/json;",
            success: function (response) {
                if (viewType == "1") {
                    self.CUploadTypeArray(response);
                } else {
                    self.CBatchNoArray(response);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    };

    self.GLSave = function () {
        var data = {
            UploadFrom: $("#txtUploadFrom").val(),
            UploadTo: $("#txtUploadTo").val(),
            UploadTypeId: $("#ddlUploadType").val(),
            BatchNo: $("#txtBatchNo").val(),
            Status: "Submit"
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetGLUpload",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {
                        self.GLClear();

                        ko.utils.postJson(UrlDet + "DownloadExcel");
                    }
                    $("#txtBatchNo").val(Data1[0].BatchNo);
                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");
                    }

                } else {
                    jAlert("unable to process your request.. please try again.", "Message");
                    $("#txtBatchNo").val("");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.GLClear = function () {
        $("#txtUploadFrom").val("");
        $("#txtUploadTo").val("");
        $("#txtBatchNo").val("");
        $("#ddlUploadType").val("0");

        $("#txtUploadFrom").removeClass('valid').removeClass('required');
        $("#txtUploadTo").removeClass('valid').removeClass('required');
        $("#ddlUploadType").removeClass('valid').removeClass('required');

        $("#txtUploadFrom").addClass('required');
        $("#txtUploadTo").addClass('required');
        $("#ddlUploadType").addClass('required');

        $('#btnGLSubmit').attr('disabled', true);
    }

    self.dateChange = function () {
        if ($("#txtCUploadDate").val().trim() == "" || $("#txtCUploadDate").val().trim().length < 10) {
           
            if (self.CUploadTypeArray() != null && self.CUploadTypeArray().length != 0){
                self.CUploadTypeArray([]);
            }
            if (self.CBatchNoArray() != null && self.CBatchNoArray().length != 0) {
                self.CBatchNoArray([]);
            }
            return false;
        } else {
            $("#ddlCUploadType").val("0");
            $("#ddlCBatchNo").val("0");

            if (self.CUploadTypeArray() != null && self.CUploadTypeArray().length != 0) {
                self.CUploadTypeArray([]);
            }
            if (self.CBatchNoArray() != null && self.CBatchNoArray().length != 0) {
                self.CBatchNoArray([]);
            }
        }
        checkValidationCSubmit();
        self.LoadUploadTypeCancellation('1');
    };

    self.UploadTypeSelectChanged = function (obj, event) {
        checkValidationCSubmit();
        if ($("#ddlCUploadType").val() == "" || $("#ddlCUploadType").val() == "0") {
            self.CBatchNoArray.removeAll();
            return false;
        }
        self.LoadUploadTypeCancellation('2');
    }

    self.GLRegenerate = function () {
        var data = {
            UploadFrom: $("#txtCUploadDate").val(),
            UploadTo: "",
            UploadTypeId: $("#ddlCUploadType").val(),
            BatchNo: $("#ddlCBatchNo").val(),
            Status: "Re-submit"
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetGLUpload",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {
                        self.CClear();
                        ko.utils.postJson(UrlDet + "DownloadExcel");
                    }

                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");
                    }
                } else {
                    jAlert("unable to process your request.. please try again.", "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.GLDelete = function () {
        var data = {
            UploadFrom: $("#txtCUploadDate").val(),
            UploadTo: "",
            UploadTypeId: $("#ddlCUploadType").val(),
            BatchNo: $("#ddlCBatchNo").val(),
            Status: "Delete"
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetGLUpload",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {
                        self.CClear();
                    }
                   
                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");
                    }

                } else {
                    jAlert("unable to process your request.. please try again.", "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.CClear = function () {
        $("#txtCUploadDate").val("");
        $("#ddlCUploadType").val("0");
        $("#ddlCBatchNo").val("0");

        if (self.CUploadTypeArray() != null && self.CUploadTypeArray().length != 0) {
            self.CUploadTypeArray([]);
        }
        if (self.CBatchNoArray() != null && self.CBatchNoArray().length != 0) {
            self.CBatchNoArray([]);
        }

        checkValidationCSubmit();
    }

    self.LoadUploadType();
};

var mainViewModel = new SearchModel();
ko.applyBindings(mainViewModel);

$(document).ready(function () {
    var objDate = new Date();
    var Presentyear = objDate.getFullYear();
    $(".fsDatePicker").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
    });

    $('#btnGLSubmit').attr('disabled', true);
    $('#btnRGSubmit').attr('disabled', true);
    $('#btnCSubmit').attr('disabled', true);

    $("#txtUploadFrom").change(function () {
        checkValidationGLSubmit();
        var txtDate = $("#txtUploadFrom").val();
        if (txtDate.trim() != "") {
            $("#txtUploadFrom").removeClass('required');
            $("#txtUploadFrom").addClass('valid');
        }
        else {
            $("#txtUploadFrom").removeClass('valid');
            $("#txtUploadFrom").addClass('required');
        }
    });

    $("#txtUploadTo").change(function () {
        checkValidationGLSubmit();
        var txtDate = $("#txtUploadTo").val();
        if (txtDate.trim() != "") {
            $("#txtUploadTo").removeClass('required');
            $("#txtUploadTo").addClass('valid');
        }
        else {
            $("#txtUploadTo").removeClass('valid');
            $("#txtUploadTo").addClass('required');
        }
    });

    $("#ddlUploadType").change(function () {
        checkValidationGLSubmit();
        var _data = $("#ddlUploadType").val();
        if (_data != "" && _data!="0") {
            $("#ddlUploadType").removeClass('required');
            $("#ddlUploadType").addClass('valid');
        }
        else {
            $("#ddlUploadType").removeClass('valid');
            $("#ddlUploadType").addClass('required');
        }
    });

    $("#ddlCBatchNo").change(function () {
        checkValidationCSubmit();
    });

});

function isEvent(evt) {
    return false;
}

function checkValidationGLSubmit() {
    var _dtFrm = $("#txtUploadFrom").val();
    var _dtTo = $("#txtUploadTo").val();
    var _uploadType = $("#ddlUploadType").val();

    if (_dtFrm.trim() == "" || _dtTo.trim() == "" || _uploadType == "" || _uploadType == "0") {
        $('#btnGLSubmit').attr('disabled', true);
    }
    else {
        $('#btnGLSubmit').attr('disabled', false);
    }
}

function checkValidationCSubmit() {
    var _dtFrm = $("#txtCUploadDate").val();
    var _batchNo = $("#ddlCBatchNo").val();
    var _uploadType = $("#ddlCUploadType").val();

    $("#txtCUploadDate").removeClass('required').removeClass('valid');
    $("#ddlCBatchNo").removeClass('required').removeClass('valid');
    $("#ddlCUploadType").removeClass('required').removeClass('valid');

    if (_dtFrm.trim() == "" || _dtFrm.trim().length <10) {
        $("#txtCUploadDate").addClass('required');
    } else {
        $("#txtCUploadDate").addClass('valid');
    }

    if (_batchNo == "" || _batchNo == "0" || _batchNo == null) {
        $("#ddlCBatchNo").addClass('required');
    } else {
        $("#ddlCBatchNo").addClass('valid');
    }

    if (_uploadType == "" || _uploadType == "0" || _uploadType == null) {
        $("#ddlCUploadType").addClass('required');
    } else {
        $("#ddlCUploadType").addClass('valid');
    }

    if (_dtFrm.trim() == "" || _dtFrm.trim().length < 10 || _batchNo == "" || _batchNo == "0" || _batchNo == null || _uploadType == "" || _uploadType == "0" || _uploadType == null) {
        $('#btnCSubmit').attr('disabled', true);
        $('#btnRGSubmit').attr('disabled', true);
    }
    else {
        $('#btnCSubmit').attr('disabled', false);
        $('#btnRGSubmit').attr('disabled', false);
    }
}