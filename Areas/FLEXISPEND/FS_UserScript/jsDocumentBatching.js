var objDialogyAdd;

UrlDet = UrlDet.replace("GetDocumentBatching", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");

var SearchModel = function () {
    var self = this;

    self.DBCountDetails = ko.observableArray();
    self.DocumentBatchingArray = ko.observableArray();

    this.search = function (id) {
        var DBatching = {
            InwardDate: "",
            DateFrom: $("#txtDateFrom").val(),
            DateTo: $("#txtDateTo").val(),
            BatchNo: $("#BatchNo").val(),
            ViewType: "0"
        };
        $.ajax({
            type: "post",
            async: false,
            url: UrlDet + "GetDocumentBatching",
            data: ko.toJSON(DBatching),
            contentType: "application/json;",
            success: function (response) {
                
                var Data1 = "", Data2 = "";
                $("#gvBatching").DataTable({
                    "autoWidth": false,
                    "destroy": true
                }).destroy();
                self.DocumentBatchingArray([]);

                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    $("#lblTotalCount").text(Data1[0].TotCount);
                    //show Message if error
                    if (Data1[0].Message != "" && id == 1) {
                        jAlert(Data1[0].Message, "Message");
                    }
                }

                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    self.DocumentBatchingArray(Data2);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.DatatableCall = function () {
        if ($("#gvBatching > tbody > tr").length == self.DocumentBatchingArray().length) {
            $("#gvBatching").DataTable({
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

    this.AddNew = function (Id) {
        $('#AddShowDialog').attr("style", "display:block;");
        objDialogyAdd.dialog({ title: 'Batching Entry', width: '600', height: '300' });
        objDialogyAdd.dialog("open");

        $("#lblInwardDate").val("");
        $("#lblInwFrm").val("");
        $("#lblInwTo").val("");
        $("#lblBatchNoSave").val("");
        $("#hfBatchId").val("0");

        $("#lblInwardDate").removeClass("required").removeClass("valid");
        $("#lblInwFrm").removeClass("required").removeClass("valid");
        $("#lblInwTo").removeClass("required").removeClass("valid");

        $("#lblInwardDate").removeAttr("disabled");
        $("#lblInwFrm").removeAttr("disabled");
        $("#lblInwTo").removeAttr("disabled");
        $("#btnSubmit").attr("style", "display:block;");
        $("#lblInwardDate").addClass("fsDatePicker");

        $("#lblInwardDate").addClass("required");
        $("#lblInwFrm").addClass("required");
        $("#lblInwTo").addClass("required");
        self.Count();
        self.showAdd();
    };

    self.showAdd = function () {
        if ($("#lblInwardDate").hasClass("valid")) {
            $("#lblInwardDate").removeClass('valid');
            $("#lblInwardDate").addClass('required');
        }

        if ($("#lblInwFrm").hasClass("valid")) {
            $("#lblInwFrm").removeClass('valid');
            $("#lblInwFrm").addClass('required');
        }

        if ($("#lblInwTo").hasClass("valid")) {
            $("#lblInwTo").removeClass('valid');
            $("#lblInwTo").addClass('required');
        }
    }

    self.create = function () {
        var data = {
            BatchId: $("#hfBatchId").val(),
            InwardDate: $("#lblInwardDate").val(),
            InwardFrom: $("#lblInwFrm").val(),
            InwardTo: $("#lblInwTo").val(),
            IsRemoved: 0
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetDocumentBatching",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {
                        self.search(0);
                        $("#lblBatchNoSave").val(Data1[0].BatchNo);
                    }

                    //show Message if error
                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");
                    }
                }

                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    self.DBCountDetails(Data2);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.edit = function (InnerDetails) {

        $('#AddShowDialog').attr("style", "display:block;");
        objDialogyAdd.dialog({ title: 'Batching Entry', width: '600', height: '300' });

        $("#hfBatchId").val(InnerDetails.BatchId);
        $("#lblInwardDate").val(InnerDetails.InwardDate);
        $("#lblInwFrm").val(InnerDetails.InwardFrom);
        $("#lblInwTo").val(InnerDetails.InwardTo);
        $("#lblBatchNoSave").val(InnerDetails.BatchNo);

        $("#lblInwardDate").removeClass("required").removeClass("valid");
        $("#lblInwFrm").removeClass("required").removeClass("valid");
        $("#lblInwTo").removeClass("required").removeClass("valid");

        $("#lblInwardDate").attr("disabled", "disabled");
        $("#lblInwFrm").attr("disabled", "disabled");
        $("#lblInwTo").attr("disabled", "disabled");
        $("#btnSubmit").attr("style", "display:none;");
        objDialogyAdd.dialog("open");

        self.Count();
    };

    self.delete = function (InnerDetails) {
        jConfirm("Do you want to delete the record?", "Message", function (callback) {
            if (callback == true) {
                var data = {
                    BatchId: InnerDetails.BatchId,
                    InwardDate: $("#lblInwardDate").val(),
                    InwardFrom: $("#lblInwFrm").val(),
                    InwardTo: $("#lblInwTo").val(),
                    IsRemoved: 1
                };
                $.ajax({
                    type: "post",
                    url: UrlDet + "SetDocumentBatching",
                    data: JSON.stringify(data),
                    contentType: "application/json;",
                    success: function (response) {
                        var Data1 = "", Data2 = "";
                        if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                            Data1 = JSON.parse(response.Data1);

                            if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {
                                self.search(0);
                            }

                            //show Message if error
                            if (Data1[0].Message != "") {
                                jAlert(Data1[0].Message, "Message");
                            }
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

    //Load default count
    self.Count = function () {
        if ($("#lblInwardDate").val() == "") {
            self.DBCountDetails.removeAll();
            return false;
        }
        var DBatching = {
            InwardDate: $("#lblInwardDate").val(),
            DateFrom: '',
            DateTo: '',
            BatchNo: '',
            ViewType: "1"
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetDocumentBatching",
            data: ko.toJSON(DBatching),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";

                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    self.DBCountDetails(Data1);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.GetCount = function (obj, event) {
        if (event.originalEvent) {
            self.reset();
            self.Count();
            
        } else {
            self.reset();
            self.Count();
        }
    }

    self.reset = function () {        
        $("#lblInwFrm").val("");
        $("#lblInwTo").val("");
        $("#lblBatchNoSave").val("");
        $("#hfBatchId").val("0");
        
        if ($("#lblInwardDate").val() == "") {
            $("#lblInwardDate").removeClass("valid").removeClass("required");
            $("#lblInwardDate").addClass("required");
        }
        $("#lblInwFrm").removeClass("valid").removeClass("required");
        $("#lblInwTo").removeClass("valid").removeClass("required");
                
        $("#lblInwFrm").addClass("required");
        $("#lblInwTo").addClass("required");
    };

    self.clearFilter = function () {
        $('#btnsearch').attr('disabled', true);
        $("#txtDateFrom").removeClass('required').removeClass('valid');
        $("#txtDateFrom").addClass('required');

        $("#txtDateTo").removeClass('required').removeClass('valid');
        $("#txtDateTo").addClass('required');

        $("#txtDateFrom").val("");
        $("#txtDateTo").val("");
        $("#BatchNo").val("");
        self.search(0);
    };
    //load the get sp 
    self.search(0);
};

var mainViewModel = new SearchModel();
ko.applyBindings(mainViewModel);

$(document).ready(function () {
    objDialogyAdd = $("[id$='ShowDialog']");
    objDialogyAdd.dialog({
        autoOpen: false,
        modal: true,
        width: 850,
        height: 500,
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

    $(".fsDatePicker").datepicker({
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

    $("#lblInwardDate").change(function () {
        var txtFrom = $("#lblInwardDate").val();
        if (txtFrom.trim() != "") {
            $("#lblInwardDate").removeClass('required');
            $("#lblInwardDate").addClass('valid');
        }
        else {
            $("#lblInwardDate").removeClass('valid');
            $("#lblInwardDate").addClass('required');
        }
    });
    $("#lblInwFrm").keyup(function () {

        var txtFrom = $("#lblInwFrm").val();
        if (txtFrom.trim() != "") {
            $("#lblInwFrm").removeClass('required');
            $("#lblInwFrm").addClass('valid');
        }
        else {
            $("#lblInwFrm").removeClass('valid');
            $("#lblInwFrm").addClass('required');
        }
    });
    $("#lblInwTo").keyup(function () {

        var txtFrom = $("#lblInwTo").val();
        if (txtFrom.trim() != "") {
            $("#lblInwTo").removeClass('required');
            $("#lblInwTo").addClass('valid');
        }
        else {
            $("#lblInwTo").removeClass('valid');
            $("#lblInwTo").addClass('required');
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
