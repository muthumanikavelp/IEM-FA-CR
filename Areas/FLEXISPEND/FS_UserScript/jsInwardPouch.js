var InwardPouchModel = function () {

    var self = this;

    self.SlNo = ko.observable("");
    self.Id = ko.observable("");
    self.ReceivedFrom = ko.observable("");
    self.ReceivedDate = ko.observable("");
    self.SenderType = ko.observable("");
    self.SenderTypeCode = ko.observable("");
    self.SenderId = ko.observable("");
    self.Sender = ko.observable("");
    self.CourierId = ko.observable("");
    self.Courier = ko.observable("");
    self.AWBNo = ko.observable("");
    self.Noofdocs = ko.observable("");
    self.Remarks = ko.observable("");
    self.KeyedBy = ko.observable("");

    var InwardPouch = {
        SlNo: self.SlNo,
        Id: self.Id,
        ReceivedFrom: self.ReceivedFrom,
        ReceivedDate: self.ReceivedDate,
        SenderType: self.SenderType,
        SenderTypeCode: self.SenderTypeCode,
        SenderId: self.SenderId,
        Sender: self.Sender,
        CourierId: self.CourierId,
        Courier: self.Courier,
        AWBNo: self.AWBNo,
        Noofdocs: self.Noofdocs,
        Remarks: self.Remarks,
        KeyedBy: self.KeyedBy
    };

    self.InwardPouch = ko.observable();
    self.InwardPouchDetails = ko.observableArray();

    self.search = function (id) {
        var tmpData = {
            "frmDate": $("#txtSDateFrom").val(),
            "toDate": $("#txtSDateTo").val(),
            "courId": $("#hdfSCourierId").val(),
            "AWBNo": $("#txtSAWBNo").val()
        }

        $.ajax({
            url: '../PouchInward/FetchInwardDetails',
            cache: false,
            type: 'POST',
            async: false,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(tmpData),
            success: function (data) {
                $("#gvPouchInward").DataTable({
                    "autoWidth": false,
                    "destroy": true
                }).destroy();

                $("#lblTotalCount").text(data.msg.TotalCount);

                var clear = data.msg.Clear;
                if (clear != null && clear == "true") {
                    self.InwardPouchDetails(data.dept);

                } else {
                    self.InwardPouchDetails.removeAll();
                    if (id == "0") {
                        jAlert(data.msg.MessageText, "Message");
                    }
                }
            }
        }).fail(
             function (xhr, textStatus, err) {
                 //alert(err);
             });
    }

    self.DatatableCall = function () {
        if ($("#gvPouchInward > tbody > tr").length == self.InwardPouchDetails().length) {
            $("#gvPouchInward").DataTable({
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

    self.showAdd = function () {
        self.InwardPouch(null);
        self.SenderTypeCode("I");

        $("#txtASenderName").removeClass('required').removeClass('valid');
        $("#txtAAWBNo").removeClass('required').removeClass('valid');
        $("#txtANoOfDocuments").removeClass('required').removeClass('valid');
        $("#txtADate").removeClass('required').removeClass('valid');
        $("#txtACurName").removeClass('required').removeClass('valid');

        $("#txtASenderName").addClass('required');
        $("#txtAAWBNo").addClass('required');
        $("#txtANoOfDocuments").addClass('required');
        $("#txtADate").addClass('required');
        $("#txtACurName").addClass('required');

        $('#btnCreate').attr('disabled', true);

        AddPouch();
    }

    //Add New Pouch Inward
    self.create = function () {
        self.Courier($("#txtACurName").val());
        self.CourierId($("#hdfACourierId").val());

        self.Sender($("#txtASenderName").val());
        self.SenderId($("#hdfASenderId").val());

        $.ajax({
            url: '../PouchInward/SavePouchDetails',
            cache: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: ko.toJSON(InwardPouch),
            success: function (response) {
                var clear = response.Clear;
                if (clear != null && clear == "true") {

                    jAlert(response.MessageText, "Message");

                    $("#txtSDateFrom").val("");
                    $("#txtSDateTo").val("");
                    $("#hdfSCourierId").val("");
                    $("#txtSAWBNo").val("");

                    self.reset();

                    objDialogyAdd.dialog("close");
                } else {
                    jAlert(response.MessageText, "Message");
                }
            }
        }).fail(
                     function (xhr, textStatus, err) {
                         //alert(err);
                     });
    }

    // Delete Pouch Inward
    self.delete = function (InwardPouch) {
        jConfirm("Do you want to delete the record?", "Message", function (callback) {
            if (callback == true) {
                var tmpData = {
                    "Id": InwardPouch.Id
                }

                $.ajax({
                    type: "POST",
                    url: '../FLEXISPEND/PouchInward/DeletePouchDetails',
                    data: JSON.stringify(tmpData),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (response) {
                        var clear = response.Clear;
                        if (clear != null && clear == "true") {

                            jAlert(response.MessageText, "Message");
                            self.reset();

                        } else {
                            jAlert(response.MessageText, "Message");
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                    }
                });
            } else {
                return false;
            }
        });
    }

    // Edit Inward Pouch
    self.edit = function (InwardPouch) {
        var tmpData = {
            PouchId: InwardPouch.Id
        }
        $.ajax({
            url: '../PouchInward/GetInwardDetails',
            cache: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(tmpData),
            success: function (data) {
                var clear = data.msg.Clear;
                if (clear != null && clear == "true") {
                    self.InwardPouch(data.dept[0]);
                }

                $("#txtSenderName").removeClass('required').removeClass('valid');
                $("#txtAWBNo").removeClass('required').removeClass('valid');
                $("#txtFrom").removeClass('required').removeClass('valid');
                $("#txtNoOfDocuments").removeClass('required').removeClass('valid');
                $("#txtDate").removeClass('required').removeClass('valid');
                $("#txtCurName").removeClass('required').removeClass('valid');

                if (InwardPouch.SenderTypeCode == 'I') {
                    $('#hdfSenderId').val(InwardPouch.Id);
                } else {
                    $('#hdfSenderId').val("0");
                }

                InwardPouch.Sender = InwardPouch.ReceivedFrom;

                $("#txtSenderName").val(InwardPouch.Sender);

                $("#txtSenderName").addClass('valid');
                $("#txtFrom").addClass('valid');
                $("#txtAWBNo").addClass('valid');
                $("#txtNoOfDocuments").addClass('valid');
                $("#txtDate").addClass('valid');
                $("#txtCurName").addClass('valid');

                EditPouch();
            }
        }).fail(
             function (xhr, textStatus, err) {
                 //alert(err);
             });
    }

    //// Update Inward Pouch
    self.update = function () {

        var InwardPouch = self.InwardPouch();
        var dataInput = {
            "SlNo": InwardPouch.SlNo,
            "Id": InwardPouch.Id,
            "ReceivedFrom": InwardPouch.ReceivedFrom,
            "ReceivedDate": InwardPouch.ReceivedDate,
            "SenderType": InwardPouch.SenderType,
            "SenderTypeCode": InwardPouch.SenderTypeCode,
            "SenderId": $("#hdfSenderId").val(),
            "Sender": $("#txtSenderName").val(),
            "CourierId": $("#hdfCourierId").val(),
            "Courier": $("#txtCurName").val(),
            "AWBNo": InwardPouch.AWBNo,
            "Noofdocs": InwardPouch.Noofdocs,
            "Remarks": InwardPouch.Remarks,
            "KeyedBy": InwardPouch.KeyedBy
        }

        $.ajax({
            url: '../PouchInward/SavePouchDetails',
            cache: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: ko.toJSON(dataInput),
            success: function (response) {
                var clear = response.Clear;
                if (clear != null && clear == "true") {
                    jAlert(response.MessageText, "Message");
                    self.reset();
                    objDialogyEdit.dialog("close");
                } else {
                    jAlert(response.MessageText, "Message");
                }
            }
        }).fail(
                     function (xhr, textStatus, err) {
                         //alert(err);
                     });
    }

    // Reset Pouch Inward
    self.reset = function () {
        //self.InwardPouchDetails.removeAll();
        self.InwardPouch(null);

        self.SlNo("");
        self.Id("");
        self.ReceivedFrom("");
        self.ReceivedDate("");
        self.SenderType("");
        self.SenderTypeCode("I");
        self.SenderId("");
        self.Sender("");
        self.CourierId("");
        self.Courier("");
        self.AWBNo("");
        self.Noofdocs("");
        self.Remarks("");
        self.KeyedBy("");

        self.search("0");
    }

    // Cancel PouchInward
    self.ClearDetails = function () {
        self.InwardPouch(null);
        self.SlNo("");
        self.Id("");
        self.ReceivedFrom("");
        self.ReceivedDate("");
        self.SenderType("");
        self.SenderTypeCode("I");
        self.SenderId("");
        self.Sender("");
        self.CourierId("");
        self.Courier("");
        self.AWBNo("");
        self.Noofdocs("");
        self.Remarks("");
        self.KeyedBy("");

        $("#txtSDateFrom").val("");
        $("#txtSDateTo").val("");
        $("#hdfSCourierId").val("");
        $("#txtSCourierName").val("");
        $("#txtSAWBNo").val("");

        $("#txtSDateTo").removeClass('required');
        $("#txtSDateTo").removeClass('valid');
        $("#txtSDateFrom").removeClass('required');
        $("#txtSDateFrom").removeClass('valid');

        $("#txtSDateFrom").addClass('required');
        $("#txtSDateTo").addClass('required');
        $('#btnsearch').attr('disabled', true);
        self.search("1");
    }

    self.search('1');
};

var mainViewModel = new InwardPouchModel();
ko.applyBindings(mainViewModel);

