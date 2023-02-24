var objDialogyAdd;
UrlDet = UrlDet.replace("SetPullout", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");
var SearchModel = function (id) {

    var self = this;
    self.DateFrom = ko.observable("");
    self.DateTo = ko.observable("");
    self.InwardNo = ko.observable("");


    self.PulloutArray = ko.observableArray();

    this.search = function (id) {
        var DateFrom = $("#txtDateFrom").val();
        var DateTo = $("#txtDateTo").val();
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
            DocRefNo: $("#txtDocRefNo").val(),
            DocInwNo: $("#txtInwardNo").val()
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

    this.AddPullout = function () {
        var RequestDate = $("#txtRequestedDate").val();
        var PulloutDate = $("#txtPulloutDate").val();
        var RequestBy = $("#hfRequestedBy").val();
        var InwardNo = $("#txtDocInwardNo").val();
        var Reason = $("#txtReason").val();
        //var DocRefNo = $("#txtDocNo").val();

        if ($.trim(RequestDate) == "") {
            jAlert("Ensure Request Date!", "Message");
            return false;
        }
        if ($.trim(PulloutDate) == "") {
            jAlert("Ensure Pullout Date!", "Message");
            return false;
        }
        if ($.trim(RequestBy) == "0" || $.trim(RequestBy) == "") {
            jAlert("Ensure Request By!", "Message");
            return false;
        }
        //if ($.trim(DocRefNo) == "") {
        //    jAlert("Ensure Doc Ref No/ Doc. Inward No!", "Message");
        //    return false;
        //}
        if ($.trim(InwardNo) == "") {
            jAlert("Ensure Doc. Ref No/ Doc. Inward No!", "Message");
            return false;
        }
        
        var PPullout = {
            PloId: "0",
            RequestDate: RequestDate,
            PulloutDate: PulloutDate,
            RequestBy: RequestBy,
            InwardNo: InwardNo,
            Reason: Reason
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
                        jAlert(Data1[0].Message, "Message");
                    }
                    else {
                        jAlert(Data1[0].Message, "Message");
                        self.search(0);
                        $("#txtRequestedDate").val("");
                        $("#txtPulloutDate").val("");
                        $("#hfRequestedBy").val("0");
                        $("#txtRequestedBy").val("");
                        $("#txtDocInwardNo").val("");
                        $("#txtDocNo").val("");
                        $("#txtReason").val("");

                        objDialogyAdd.dialog('close');
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
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
        $("#txtDocRefNo").val("")
        self.search(0);
    };

    self.search(0);
};

var mainViewModel = new SearchModel();
ko.applyBindings(mainViewModel);

$(document).ready(function () {
    objDialogyAdd = $("[id$='ShowDialog']");
    objDialogyAdd.dialog({
        autoOpen: false,
        modal: true,
        width: 460,
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

    $("#txtRequestedDate").change(function () {
        var DocInwardNo = $.trim($("#txtRequestedDate").val());
        if (DocInwardNo != "") {
            $("#txtRequestedDate").addClass("valid");
            $("#txtRequestedDate").removeClass("required");
        }
        else {
            $("#txtRequestedDate").addClass("required");
            $("#txtRequestedDate").removeClass("valid");
        }
    });
    $("#txtPulloutDate").change(function () {
        var DocInwardNo = $.trim($("#txtPulloutDate").val());
        if (DocInwardNo != "") {
            $("#txtPulloutDate").addClass("valid");
            $("#txtPulloutDate").removeClass("required");
        }
        else {
            $("#txtPulloutDate").addClass("required");
            $("#txtPulloutDate").removeClass("valid");
        }
    });
    $("#txtDocInwardNo").change(function () {
        var DocInwardNo = $.trim($("#txtDocInwardNo").val());
        if (DocInwardNo != "") {
            $("#txtDocInwardNo").addClass("valid");
            $("#txtDocInwardNo").removeClass("required");
        }
        else {
            $("#txtDocInwardNo").addClass("required");
            $("#txtDocInwardNo").removeClass("valid");
        }
    });
    $("#txtDocNo").change(function () {
        var DocNo = $.trim($("#txtDocNo").val());
        if (DocNo != "") {
            $("#txtDocNo").addClass("valid");
            $("#txtDocNo").removeClass("required");
        }
        else {
            $("#txtDocNo").addClass("required");
            $("#txtDocNo").removeClass("valid");
        }
    });

    $("#txtRequestedBy").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: UrlHelper + "GetAutoCompleteEmployee",
                data: "{ 'txt' : '" + $("#txtRequestedBy").val() + "'}",
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
            $("#hfRequestedBy").val(i.item.val);
            $("#txtRequestedBy").val(i.item.label);
            var txtCurName = $("#txtRequestedBy").val();
            if (txtCurName.trim() != "") {
                $("#txtRequestedBy").removeClass('required');
                $("#txtRequestedBy").addClass('valid');
            }
            else {
                $("#txtRequestedBy").removeClass('valid');
                $("#txtRequestedBy").addClass('required');
            }
        },
        minLength: 1
    });

    $("#txtRequestedBy").focusout(function () {
        var hdfId = $("#hfRequestedBy").val();
        var txtCurName = $("#txtRequestedBy").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtRequestedBy").val("");
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

    if ($("#txtRequestedDate").hasClass("valid")) {
        $("#txtRequestedDate").removeClass("valid");
        $("#txtRequestedDate").addClass("required");
    }

    if ($("#txtPulloutDate").hasClass("valid")) {
        $("#txtPulloutDate").removeClass("valid");
        $("#txtPulloutDate").addClass("required");
    }

    if ($("#txtRequestedBy").hasClass("valid")) {
        $("#txtRequestedBy").removeClass("valid");
        $("#txtRequestedBy").addClass("required");
    }

    if ($("#txtDocInwardNo").hasClass("valid")) {
        $("#txtDocInwardNo").removeClass("valid");
        $("#txtDocInwardNo").addClass("required");
    }

    if ($("#txtDocNo").hasClass("valid")) {
        $("#txtDocNo").removeClass("valid");
        $("#txtDocNo").addClass("required");
    }

    objDialogyAdd.dialog({ title: 'Physical Pullout Entry', width: '460', height: '280' });
    $("#txtRequestedDate").val("");
    $("#txtPulloutDate").val("");
    $("#hfRequestedBy").val("0");
    $("#txtRequestedBy").val("");
    $("#txtDocInwardNo").val("");
    $("#txtDocNo").val("");
    $("#txtReason").val("");
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
