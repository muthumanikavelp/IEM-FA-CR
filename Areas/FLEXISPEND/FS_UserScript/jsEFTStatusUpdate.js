UrlDet = UrlDet.replace("SaveMemoDetails", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");

var EFTStatusUpdateModel = function () {
    var self = this;
    self.SNo = ko.observable("");
    self.MemoNo = ko.observable("");
    self.PVNo = ko.observable("");
    self.Amount = ko.observable("");
    self.StatusCode = ko.observable("Cleared");
    self.PvDate = ko.observable("");
    self.Beneficiary = ko.observable("");
    self.BankName = ko.observable("");
    self.Status = ko.observable("");
    self.Date = ko.observable("");
    self.Remarks = ko.observable("");

    var EFTStatus = {
        SNo: self.SNo,
        MemoNo: self.MemoNo,
        PVNo: self.PVNo,
        Amount: self.Amount,
        PvDate: self.PvDate,
        Beneficiary: self.Beneficiary,
        BankName: self.BankName,
        Status: self.StatusCode,
        Date: self.Date,
        Remarks: self.Remarks
    };

    self.EFTStatus = ko.observable();

    self.EFTStatusArray = ko.observableArray();

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

    self.SubmitStatus = function () {
        var status = EFTStatus;
        $.ajax({
            type: "POST",
            url: UrlDet + 'SaveMemoDetails',
            data: ko.toJSON(EFTStatus),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                var clear = response.Clear;
                if (clear != null && (clear == "true" || clear == "True" || clear == "1")) {
                    self.reset();
                    jAlert(response.MessageText, "Message");

                } else {
                    jAlert(response.MessageText, "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });

    };

    self.reset = function () {
        $("#txtMemoNo").val("");
        $("#txtPVNo").val("");
        $("#txtDocAmount").val("");

        $("#txtBankName").val("");
        $("#txtBeneficiary").val("");
        $("#txtDate").val("");
        $("#txtRemarks").val("");

        self.EFTStatus(null);
        self.Amount("");
        self.BankName("");
        self.Beneficiary("");
        self.Date("");
        self.MemoNo("");
        self.PvDate("");
        self.PVNo("");
        self.Remarks("");
        self.StatusCode("Cleared");

        //$("#txtMemoNo").removeClass('valid').removeClass('required');
        $("#txtPVNo").removeClass('valid').removeClass('required');
        $("#txtDate").removeClass('valid').removeClass('required');
        $("#txtRemarks").removeClass('valid').removeClass('required');

        //$("#txtMemoNo").addClass('required');
        $("#txtPVNo").addClass('required');
        $("#txtDate").addClass('required');

        $("#txtRemarks").attr("disabled", "disabled");
        $("#txtRemarks").removeClass('required').removeClass('valid');
        $("#txtRemarks").val("");

        $("#btnSubmitEFTStatus").attr("disabled", true);
    };

    self.textchange = function () {
        // if ($("#txtMemoNo").val().trim() == "" || $("#txtPVNo").val().trim() == "") {
        if ($("#txtPVNo").val().trim() == "") {
            return false;
        }
        var tmpData = {
            "MemoNo": $("#txtMemoNo").val(),
            "PVNo": $("#txtPVNo").val()
        };
        $.ajax({
            type: "POST",
            url: UrlDet + 'GetMemoDetails',
            data: JSON.stringify(tmpData),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.eftMemoDetails != undefined) {
                    self.EFTStatus(data.eftMemoDetails[0]);
                   // $("#txtMemoNo").removeClass('required').removeClass('valid');
                    $("#txtPVNo").removeClass('required').removeClass('valid');
                    //$("#txtMemoNo").addClass('valid');
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
                   // $("#txtMemoNo").removeClass('required').removeClass('valid');
                    $("#txtPVNo").removeClass('required').removeClass('valid');
                    //$("#txtMemoNo").addClass('required');
                    $("#txtPVNo").addClass('required');
                    $("#btnSubmitEFTStatus").attr("disabled", true);
                    self.EFTStatus(null);
                    jAlert(data.msg.MessageText, "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    };

    this.Checked = function (obj, event) {
        if (event.originalEvent) {
            var _inBit = event.originalEvent.target.defaultValue;
            if (_inBit == "Cleared") {
                $("#txtRemarks").attr("disabled", "disabled");
                $("#txtRemarks").removeClass('required').removeClass('valid');
                $("#txtRemarks").val("");
            }
            else {
                $("#txtRemarks").removeAttr("disabled");
                $("#txtRemarks").removeClass('required').removeClass('valid');
                $("#txtRemarks").addClass('required');
            }
            fnCheckValidation();
        }
    };

    self.ClearSearch = function () {
        $("#txtBulkDate").val("");
        $("#attUploader").val("");

        $('#uploaderForm')[0].reset();

        $("#txtBulkDate").removeClass('valid');
        $("#txtBulkDate").addClass('required');
        $("#btnSubmit").attr("disabled", true);
        self.loadGrid();
    };

    self.upload = function () {
       /* var BulkDate = $("#txtBulkDate").val();
        if (BulkDate == "") {
            jAlert("Ensure Date!", "Message");
            $("#attUploader").val("");
            return false;
        }*/
        $("#btnSubmit").attr("disabled", true);
        var FileUpload = $("#attUploader").val();
        if (FileUpload == "" || FileUpload == "0") {
            jAlert("Ensure FileName!", "Message");
            return false;
        }

        var data = {
            MemoNo: "",
            PVNo: "0",
            Status: "",
            Date: "",
            Remarks: "",
            ViewType: "1",
            PvDate: ""
        };
        $.ajax({
            type: "post",
            url: UrlDet + "BulkUploadStatusUpdate",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {
                    if (response.Data2 != "" && JSON.parse(response.Data2) != null) {
                        Data2 = JSON.parse(response.Data2);
                        self.loadGrid();
                        self.EFTStatusArray(Data2);
                    }
                    jAlert(Data1[0].Message, "Message");
                }
                else {
                    self.loadGrid();
                }

                //show Message if error
                if (Data1[0].Message != "") {
                    jAlert(Data1[0].Message, "Message");
                }

                if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {
                   
                }

                $("#attUploader").val("");
                $('#uploaderForm')[0].reset();

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.Download = function () {
        //indow.location = UrlDef + "/Template/EFTStatusUpdateTemplate.xls";
        ko.utils.postJson(UrlDet + "DownloadTemplate");
    };

    self.DatatableCall = function () {
        if ($("#gvChequeStatuUpdation > tbody > tr").length == self.EFTStatusArray().length) {
            $("#gvChequeStatuUpdation").DataTable({
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
        $("#gvChequeStatuUpdation").DataTable({
            "autoWidth": false,
            "destroy": true,
            "bFilter": false,
            "bLengthChange": false,
            "bPaginate": false,
            "bInfo": false
        }).destroy();
        self.EFTStatusArray.removeAll();
    }

    self.loadGrid();

};

var mainViewModel = new EFTStatusUpdateModel();
ko.applyBindings(mainViewModel);

$(document).ready(function () {
    $("#btnSubmitEFTStatus").attr("disabled", true);
    $("#btnSubmit").attr("disabled", true);

    $("#txtRemarks").attr("disabled", "disabled");
    $("#txtRemarks").removeClass('required').removeClass('valid');
    $("#txtRemarks").val("");

    $(".attUploader").on('change', function (e) {
        fnCheckValidationForBulkUpload();
        var iframe = $('<iframe name="postiframe" id="postiframe" style="display: none"></iframe>');
        $("body").append(iframe);
        var form = $('#uploaderForm');
        form.attr("action", UrlDet + "Upload");
        form.attr("method", "post");
        form.attr("encoding", "multipart/form-data");
        form.attr("enctype", "multipart/form-data");
        form.attr("target", "postiframe");
        form.attr("file", $('#attUploader').val());
        form.submit();
        return false;
    });
});

