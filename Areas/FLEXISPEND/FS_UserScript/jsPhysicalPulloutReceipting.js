var objDialogyAdd;
UrlDet = UrlDet.replace("FetchPullout", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");
var SearchModel = function (id) {

    var self = this;
    self.DateFrom = ko.observable("");
    self.DateTo = ko.observable("");
    self.InwardNo = ko.observable("");


    self.PulloutArray = ko.observableArray();
    self.DocTypeArray = ko.observableArray();

    this.search = function (id) {
        var DateFrom = $("#txtDateFrom").val();
        var DateTo = $("#txtDateTo").val();
        var Code = $("#hfRequestCode").val();
        var Employee = $("#hfRequestBy").val();
        var ReqBy = Code == "0" ? Employee : Code;

        if ($.trim(DateFrom) == "" && $.trim(DateTo) != "" && id == 1) {
            jAlert("Ensure Pullout Date From!", "Message");
            return false;
        }
        if ($.trim(DateFrom) != "" && $.trim(DateTo) == "" && id == 1) {
            jAlert("Ensure Pullout Date To!", "Message");
            return false;
        }
        var Pullout = {
            DateFrom: DateFrom,
            DateTo: DateTo,
            DocInwNo: $("#txtInwardNo").val(),
            DocRefNo: $("#txtDocRefNo").val(),
            DocType: $("#ddlDocType").val(),
            DocAmount: $("#txtDocAmount").val(),
            ReqBy: ReqBy
        };
        $.ajax({
            type: "post",
            url: UrlDet + "FetchPullout",
            data: ko.toJSON(Pullout),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                $("#gvPullout").DataTable({
                    "autoWidth": false,
                    "destroy": true
                }).destroy();
                self.PulloutArray([]);
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == false) {
                        jAlert(Data1[0].Message, "Message");
                        return false;
                    }
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                self.PulloutArray(Data2);
                if (Data2 == "" && id == 1) {
                    jAlert("Sorry No Record Found!", "Message");
                    return false;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.DatatableCall = function () {
        if ($("#gvPullout > tbody > tr").length == self.PulloutArray().length) {
            $("#gvPullout").DataTable({
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

    this.EditPullout = function (PulloutId) {

        $('#ShowDialog').attr("style", "display:block;");

        objDialogyAdd.dialog({ title: 'Pullout Received Entry', width: '650', height: '280' });

        $("#hfPloId").val(PulloutId);
        $("#txtRequestedDate").val($("#lblRequestDate" + PulloutId).text());
        $("#txtPulloutDate").val($("#lblPulloutDate" + PulloutId).text());
        $("#txtRequestedBy").val($("#lblRequestBy" + PulloutId).text());
        $("#txtDocInwardNo").val($("#lblDocInwNo" + PulloutId).text());
        $("#txtReason").val($("#lblReason" + PulloutId).text());
        $("#txtReturnDate").val("");
        $("#txtRemarks").val("");

        $("#txtReturnDate").addClass("required");
        $("#txtReturnDate").removeClass("valid");
        //$("#txtRemarks").addClass("required");
        //$("#txtRemarks").removeClass("valid");

        objDialogyAdd.dialog("open");
        return false;
    };

    this.LoadDocType = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetDocType",
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

    this.AddPullout = function () {
        var PloId = $("#hfPloId").val();
        var ReturnDate = $("#txtReturnDate").val();
        var Remarks = $("#txtRemarks").val();
        var ReturnBy = $("#hfReturnBy").val();

        if ($.trim(PloId) == "0" || $.trim(PloId) == "") {
            jAlert("Some Error Please Try Again!", "Message");
            return false;
        }
        if ($.trim(ReturnDate) == "") {
            jAlert("Ensure Return Date!", "Message");
            return false;
        }
        //if ($.trim(Remarks) == "") {
        //    jAlert("Ensure Remarks!", "Error");
        //    return false;
        //}
        var PPullout = {
            PloRId: "0",
            PloId: PloId,
            ReturnDate: ReturnDate,
            Remarks: Remarks,
            ReturnBy: ReturnBy
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetPullout",
            data: ko.toJSON(PPullout),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == false) {
                        jAlert(Data1[0].Message, "Error");
                    }
                    else {
                        jAlert(Data1[0].Message, "Message");
                        self.search(0);
                        $("#txtRequestedDate").val("");
                        $("#txtPulloutDate").val("");
                        $("#txtRequestedBy").val("");
                        $("#txtDocInwardNo").val("");
                        $("#txtReason").val("");
                        $("#txtReturnDate").val("");
                        $("#txtRemarks").val("");
                        $("#hfPloId").val("0");
                        $("#hfReturnBy").val("0");
                        $("#txtReturnDate").addClass("required");
                        $("#txtReturnDate").removeClass("valid");
                        //$("#txtRemarks").addClass("required");
                        //$("#txtRemarks").removeClass("valid");
                        objDialogyAdd.dialog("close");

                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                // alert(errorThrown);
            }
        });
    };

    self.clearFilter = function () {
        $('#btnsearch').attr('disabled', true);
        $("#txtDateFrom").removeClass('required').removeClass('valid');
        $("#txtDateFrom").addClass('required');

        $("#txtDateTo").removeClass('required').removeClass('valid');
        $("#txtDateTo").addClass('required');

        $("#txtDateFrom").val("");
        $("#txtDateTo").val("");
        $("#txtInwardNo").val("");
        $("#ddlDocType").val(0);
        $("#txtDocRefNo").val("");
        $("#txtDocAmount").val("");
        $("#txtRequestCode").val("");
        $("#txtRequestBy").val("");

        $("#hfRequestBy").val("0");
        $("#hfRequestCode").val("0");
        self.search(0);
    };

    self.LoadDocType();
    self.search(0);
};

var mainViewModel = new SearchModel();
ko.applyBindings(mainViewModel);

$(document).ready(function () {
    objDialogyAdd = $("[id$='ShowDialog']");
    objDialogyAdd.dialog({
        autoOpen: false,
        modal: true,
        width: 650,
        height: 280,
        duration: 300

    });

    var objDate = new Date();
    var Presentyear = objDate.getFullYear();
    $(".fsDatepicker").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
    });

    $("#txtReturnDate").change(function () {
        var ReturnDate = $.trim($("#txtReturnDate").val());
        if (ReturnDate != "") {
            $("#txtReturnDate").addClass("valid");
            $("#txtReturnDate").removeClass("required");
        }
        else {
            $("#txtReturnDate").addClass("required");
            $("#txtReturnDate").removeClass("valid");
        }
    });

    //$("#txtRemarks").change(function () {
    //    var Remarks = $.trim($("#txtRemarks").val());
    //    if (Remarks != "") {
    //        $("#txtRemarks").addClass("valid");
    //        $("#txtRemarks").removeClass("required");
    //    }
    //    else {
    //        $("#txtRemarks").addClass("required");
    //        $("#txtRemarks").removeClass("valid");
    //    }
    //});

    $("#txtRequestBy").keyup(function (e) {
        if (e.which != 13) {
            $("#hfRequestBy").val("0");
        }

        $("#txtRequestBy").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteEmployee",
                    data: "{ 'txt' : '" + $("#txtRequestBy").val() + "'}",
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
                    },
                    failure: function (response) {
                    }
                });
            },
            select: function (e, i) {
                $("#hfRequestBy").val(i.item.val);
                $("#txtRequestBy").val(i.item.label);
            },
            minLength: 1
        });

    });

    $("#txtRequestBy").focusout(function () {
        var hdfId = $("#hfRequestBy").val();
        var txtCurName = $("#txtRequestBy").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtRequestBy").val("");
        }
    });

    $("#txtRequestCode").keyup(function (e) {
        if (e.which != 13) {
            $("#hfRequestCode").val("0");
        }

        $("#txtRequestCode").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteEmployeeCode",
                    data: "{ 'txt' : '" + $("#txtRequestCode").val() + "'}",
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
                    },
                    failure: function (response) {
                    }
                });
            },
            select: function (e, i) {
                $("#hfRequestCode").val(i.item.val);
                $("#txtRequestCode").val(i.item.label);
            },
            minLength: 1
        });
    });

    $("#txtRequestCode").focusout(function () {
        var hdfId = $("#hfRequestCode").val();
        var txtCurName = $("#txtRequestCode").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtRequestCode").val("");
        }
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

function AddPullout() {
    $('#ShowDialog').attr("style", "display:block;");

    objDialogyAdd.dialog({ title: 'Pullout Received Entry', width: '650', height: '280' });
    objDialogyAdd.dialog("open");
    return false;
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
