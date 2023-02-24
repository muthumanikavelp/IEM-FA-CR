var objShowReceiptDet;
var objECFDet;

MUrlDet = MUrlDet.replace("GetPouchInward", "");
MUrlHelper = MUrlHelper.replace("GetAutoCompleteCourier", "");

var SearchModel = function () {

    var self = this;
    self.SlNo = ko.observable("");
    self.InwardId = ko.observable("");
    self.PouchId = ko.observable("");
    self.InwardDate = ko.observable("");
    self.InwardNo = ko.observable("");
    self.DocType = ko.observable("");
    self.DocRefNo = ko.observable("");
    self.PhysicalVerification = ko.observable("");
    self.Remarks = ko.observable("");
    self.DocTypeCode = ko.observable("");

    var InnerDetails = {
        SlNo: self.SlNo,
        InwardId: self.InwardId,
        PouchId: self.PouchId,
        InwardDate: self.InwardDate,
        InwardNo: self.InwardNo,
        DocType: self.DocType,
        DocRefNo: self.DocRefNo,
        PhysicalVerification: self.PhysicalVerification,
        Remarks: self.Remarks,
        DocTypeCode: self.DocTypeCode
    };
    self.InnerDetails = ko.observable();

    self.PouchInwardArray = ko.observableArray();
    self.PIInnerDetails = ko.observableArray();
    self.DocTypeArray = ko.observableArray();
    //self.DocDetails = ko.observableArray();

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
        var PInward = {
            DateFrom: $("#txtDateFrom").val(),
            DateTo: $("#txtDateTo").val(),
            CourierId: $("#hdfCourierId").val(),
            AWBNo: $("#AWBNoId").val()
        };
        $.ajax({
            type: "post",
            async: false,
            url: MUrlDet + "GetPouchInward",
            data: ko.toJSON(PInward),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                $("#gvReceipting").DataTable({
                    "autoWidth": false,
                    "destroy": true
                }).destroy();
                self.PouchInwardArray([]);

                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    $("#lblTotalCount").text(Data1[0].TotCount);
                    //show Message if error
                    if (Data1[0].Message != "" && id == 1) {
                        jAlert(Data1[0].Message, "Message");
                    }
                } else {
                    $("#lblTotalCount").text("0");
                }

                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                self.PouchInwardArray(Data2);

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.LoadDocType = function () {
        $.ajax({
            type: "post",
            url: MUrlHelper + "GetPRDocType",
            data: "{ }",
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.DocTypeArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    };

    self.DatatableCall = function () {
        if ($("#gvReceipting > tbody > tr").length == self.PouchInwardArray().length) {
            $("#gvReceipting").DataTable({
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

    self.view = function (Id) {

        $("#hdfPRGId").val("0");
        $("#hdfPouch_GId").val("0");
        $("#lblReceivedDate").val("");
        $("#lblCourier").val("");
        $("#lblSenderType").val("");
        $("#lblReceivedFrom").val("");
        $("#lblAWBNo").val("");
        //$("#lblSenderName").val("");
        $("#lblNoofDocs").val("");
        //$("#lblKeyedBy").val("");

        self.InnerDetails(null);
        self.PIInnerDetails.removeAll();

        $("#hdfPRGId").val("0");
        $("#txtPRDocRefNo").val("");
        $("#txtPRRemarks").val("");
        $("#txtPRInwardNo").val("");
        $("#txtPRECFAmount").val("");
        $("#ddlPRVerification").val("Y");
        $("#ddlPRDocType").val("E");

        $('#shwPhysicalReceiptDet').attr("style", "display:block;");
        objShowReceiptDet.dialog({ title: 'Document Inward Entry', width: '930', height: '500' });
        objShowReceiptDet.dialog("open");

        $.ajax({
            type: "post",
            url: MUrlDet + "GetPhysicalReceipt",
            data: "{ 'PouchId' : '" + Id + "'}",
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "", Data3 = "";
                if (response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    //show Message if error
                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");
                    }
                }

                if (response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);

                    $("#hdfPouch_GId").val(Data2[0].PouchId);
                    $("#lblReceivedDate").val(Data2[0].ReceivedDate);
                    $("#lblCourier").val(Data2[0].Courier);
                    $("#lblSenderType").val(Data2[0].SenderType);
                    $("#lblReceivedFrom").val(Data2[0].ReceivedFrom);
                    $("#lblAWBNo").val(Data2[0].AWBNo);
                    //$("#lblSenderName").val(Data2[0].Sender);
                    $("#lblNoofDocs").val(Data2[0].Noofdocs);
                    //$("#lblKeyedBy").val(Data2[0].KeyedBy);
                } else {
                    $("#hdfPouch_GId").val("0");
                    $("#lblReceivedDate").val("");
                    $("#lblCourier").val("");
                    $("#lblSenderType").val("");
                    $("#lblReceivedFrom").val("");
                    $("#lblAWBNo").val("");
                    //$("#lblSenderName").val("");
                    $("#lblNoofDocs").val("");
                    //$("#lblKeyedBy").val("");
                }

                if (response.Data3 != "" && JSON.parse(response.Data3) != null) {
                    Data3 = JSON.parse(response.Data3);
                    self.PIInnerDetails(Data3);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    };

    this.ClearDetails = function () {
        $("#hdfPRGId").val("0");
        $("#txtPRDocRefNo").val("");
        $("#txtPRRemarks").val("");
        $("#txtPRInwardNo").val("");
        $("#txtPRECFAmount").val("");
        $("#ddlPRVerification").val("Y");
        $("#ddlPRDocType").val("E");
        $("#txtSlNo").val($('#tblInnerDet tr').length);

    };

    self.create = function () {
        var data = {
            PRGId: $("#hdfPRGId").val(),
            Pouch_GId: $("#hdfPouch_GId").val(),
            DocType: $("#ddlPRDocType").val(),
            DocRefNo: $("#txtPRDocRefNo").val(),
            Phy_Verification: $("#ddlPRVerification").val(),
            Remarks: $("#txtPRRemarks").val(),
            IsRemoved: 0
        };
        $.ajax({
            type: "post",
            url: MUrlDet + "SavePhysicalReceipt",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);

                //show Message if error
                if (Data1[0].Message != "") {
                    jAlert(Data1[0].Message, "Message");
                }

                if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {
                    self.PIInnerDetails.removeAll();
                    $("#hdfPRGId").val("0");
                    $("#txtPRDocRefNo").val("");
                    $("#txtPRRemarks").val("");
                    $("#txtPRInwardNo").val("");
                    $("#txtPRECFAmount").val("");
                    $("#ddlPRVerification").val("Y");
                    $("#ddlPRDocType").val("E");
                }

                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    self.PIInnerDetails(Data2);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.edit = function (InnerDetails) {
        $("#txtSlNo").val(InnerDetails.SlNo);
        $("#hdfPRGId").val(InnerDetails.InwardId);
        $("#txtPRDocRefNo").val(InnerDetails.DocRefNo);
        $("#txtPRRemarks").val(InnerDetails.Remarks);
        $("#txtPRInwardNo").val(InnerDetails.InwardNo);
        $("#txtPRECFAmount").val(self.formatNumber(InnerDetails.EcfAmount));
        $("#ddlPRVerification").val((InnerDetails.PhysicalVerification == "Ok" || InnerDetails.PhysicalVerification == "ok") ? "Y" : "N");
        $("#ddlPRDocType").val(InnerDetails.DocTypeCode);
    };

    self.delete = function (InnerDetails) {
        jConfirm("Do you want to delete the record?", "Message", function (callback) {
            if (callback == true) {
                $("#hdfPRGId").val(InnerDetails.InwardId);

                var data = {
                    PRGId: $("#hdfPRGId").val(),
                    Pouch_GId: $("#hdfPouch_GId").val(),
                    DocType: '',
                    DocRefNo: '',
                    Phy_Verification: '',
                    Remarks: '',
                    IsRemoved: 1
                };
                $.ajax({
                    type: "post",
                    url: MUrlDet + "SavePhysicalReceipt",
                    data: JSON.stringify(data),
                    contentType: "application/json;",
                    success: function (response) {
                        var Data1 = "", Data2 = "";
                        if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                            Data1 = JSON.parse(response.Data1);

                        //show Message if error
                        if (Data1[0].Message != "") {
                            jAlert(Data1[0].Message, "Message");
                        }

                        if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {
                            self.PIInnerDetails.removeAll();
                            $("#hdfPRGId").val("0");
                            $("#txtPRDocRefNo").val("");
                            $("#txtRemarks").val("");
                            $("#ddlPRVerification").val("Y");
                            $("#ddlPRDocType").val("E");
                        }

                        if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                            Data2 = JSON.parse(response.Data2);
                            self.PIInnerDetails(Data2);
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        //alert(errorThrown);
                    }
                });
            } else {
                return false;
            }
        });
    };

    // Cancel PouchInward
    self.ClearSearch = function () {
        $("#txtDateFrom").val("");
        $("#txtDateTo").val("");
        $("#hdfCourierId").val("");
        $("#txtCourierName").val("");
        $("#AWBNoId").val("");

        $("#txtDateFrom").removeClass('required');
        $("#txtDateFrom").removeClass('valid');
        $("#txtDateTo").removeClass('required');
        $("#txtDateTo").removeClass('valid');

        $("#txtDateFrom").addClass('required');
        $("#txtDateTo").addClass('required');
        $('#btnsearch').attr('disabled', true);
        self.search("0");
    }

    self.ViewDocument = function () {
        var _tmpData = {
            DocNo: $("#txtPRDocRefNo").val()
        };

        if ($.trim($("#txtPRDocRefNo").val()) == "") {
            jAlert("Ensure Document Number!", "Message");
            return false;
        }

        var ecfId = "0", DocSubTypeId = "0";

        $.ajax({
            type: "post",
            url: MUrlHelper + "GetDocumentECFDetails",
            data: JSON.stringify(_tmpData),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                /*self.DocDetails.removeAll();
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    self.DocDetails(Data1);
                }
                ShowDocumentDet();*/

                if (response.ecfId != null && response.ecfId != "0" && JSON.parse(response.ecfId) != null) {
                    ecfId = response.ecfId;
                }
                if (response.docSubTypeId != null && response.docSubTypeId != "0" && JSON.parse(response.docSubTypeId) != null) {
                    DocSubTypeId = response.docSubTypeId;
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
                ecfId = "0";
                DocSubTypeId = "0";
            }
        });

        if (ecfId == "0" || DocSubTypeId == "0") {            
            jAlert("Invalid Document Number!", "Message");
            return false;
        } else {
            ShowECFDet(ecfId, DocSubTypeId);
        }
    };

    //load the get sp 
    self.search(0);
    self.LoadDocType();
};

var mainViewModel = new SearchModel();
ko.applyBindings(mainViewModel);

$(document).ready(function () {
    objShowReceiptDet = $("[id$='shwPhysicalReceiptDet']");
    objShowReceiptDet.dialog({
        autoOpen: false,
        modal: true,
        width: 930,
        height: 500,
        duration: 300
    });

    objECFDet = $("[id$='ShwECFDet']");
    objECFDet.dialog({
        autoOpen: false,
        modal: true,
        width: 1000,
        height: 550,
        duration: 300
    });

    var objDate = new Date();
    var Presentyear = objDate.getFullYear();
    $("#txtDateFrom").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
    });
    $("#txtDateTo").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
    });

    $('#btnsearch').attr('disabled', true);

    $("#txtDateFrom").change(function () {
        checkValidationSearch();
        var txtDate = $("#txtDateFrom").val();
        if (txtDate.trim() != "") {
            $("#txtDateFrom").removeClass('required');
            $("#txtDateFrom").addClass('valid');
        }
        else {
            $("#txtDateFrom").removeClass('valid');
            $("#txtDateFrom").addClass('required');
        }
    });

    $("#txtDateTo").change(function () {
        checkValidationSearch();
        var txtDate = $("#txtDateTo").val();
        if (txtDate.trim() != "") {
            $("#txtDateTo").removeClass('required');
            $("#txtDateTo").addClass('valid');
        }
        else {
            $("#txtDateTo").removeClass('valid');
            $("#txtDateTo").addClass('required');
        }
    });

    $("#txtCourierName").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfCourierId").val("0");
        }

        $("#txtCourierName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: MUrlHelper + "GetAutoCompleteCourier",
                    data: "{ 'txt' : '" + $("#txtCourierName").val() + "'}",
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
                        //alert(response.responseText);
                    },
                    failure: function (response) {
                        //alert(response.responseText);
                    }
                });
            },
            minLength: 1,
            select: function (e, i) {
                $("#hdfCourierId").val(i.item.val);
                $("#txtCourierName").val(i.item.label);
            }
        });
    });

    $("#txtCourierName").focusout(function () {
        var hdfId = $("#hdfCourierId").val();
        var txtCurName = $("#txtCourierName").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtCourierName").val("");
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

function ShowECFDet(ecfId, DocSubTypeId) {
    $('#ShwECFDet').attr("style", "display:block;");

    var url = 'PhysicalReceipting/DocumentDetails/' + ecfId + '/' + DocSubTypeId;
    objECFDet.load(url);
    objECFDet.dialog({ title: 'Document Detail' });
    objECFDet.dialog("open");
    return false;
    /*
    objECFDet.dialog({ title: 'Document Detail', width: '900', height: '700' });
    objECFDet.dialog("open");
    return false;
    */
}

function isEvent(evt) {
    return false;
}

function checkValidationSearch() {
    var _dtFrm = $("#txtDateFrom").val();
    var _dtTo = $("#txtDateTo").val();

    if (_dtFrm.trim() == "" || _dtTo.trim() == "") {
        $('#btnsearch').attr('disabled', true);
    }
    else {
        $('#btnsearch').attr('disabled', false);
    }
}