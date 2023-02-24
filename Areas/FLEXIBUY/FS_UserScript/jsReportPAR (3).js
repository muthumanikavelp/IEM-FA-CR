var SearchModel = function (id) {

    var self = this;

    self.ResultArray = ko.observableArray();

    self.formatNumber = function (num) {
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

    self.search = function () {
        var Regionval = 0;
        if ($("#txtFrom").val() == null || $("#txtFrom").val() == "") {
            jAlert("From Date Should not be empty !", "Message");
            return false;
        }
        else if ($("#txtto").val() == null || $("#txtto").val() == "") {
            jAlert("To Date Should not be empty!", "Message");
            return false;
        }
        else if ($("#txtFrom").val() != null && $("#txtto").val() != null) {
            Regionval = 2;
        }
        else {
            Regionval = $("#hdfRegionId").val();
        }
        var InputFilter = {
            RegionId: Regionval,
            Type: "0",
            Fromdate: $("#txtFrom").val(),
            Todate: $("#txtto").val()
        };
        $.ajax({
            type: "post",
            url: "../Report/FetchReportData",
            data: ko.toJSON(InputFilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                if (response != null && response != "") {
                    var Data1 = "", Data2 = "";
                    $("#gvReport").DataTable({
                        "autoWidth": false,
                        "destroy": true
                    }).destroy();
                    self.ResultArray([]);

                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                        Data1 = JSON.parse(response.Data1);

                        if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                            Data2 = JSON.parse(response.Data2);
                        self.ResultArray(Data2);

                        if (Data1[0].Clear == false) {
                            jAlert(Data1[0].Message, "Message");
                            return false;
                        } else {
                            if (Data2 == "") {
                                jAlert("Sorry No Record Found!", "Message");
                                return false;
                            }
                        }
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

    self.Download = function () {
        var Regionval = 0;
        if ($("#txtFrom").val() == null || $("#txtFrom").val() == "") {
            jAlert("From Date Should not be empty !", "Message");
            return false;
        }
        else if ($("#txtto").val() == null || $("#txtto").val() == "") {
            jAlert("To Date Should not be empty!", "Message");
            return false;
        }
        else if ($("#txtFrom").val() != null && $("#txtto").val() != null) {
            Regionval = 2;
        }
        else {
            Regionval = $("#hdfRegionId").val();
        }
        var InputFilter = {
            RegionId: Regionval,
            Type: "0",
            Fromdate: $("#txtFrom").val(),
            Todate: $("#txtto").val()
        };
        $.ajax({
            type: "post",
            url: "../Report/DownloadReportData",
            data: ko.toJSON(InputFilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                if (response == "process") {
                    ko.utils.postJson("../Report/DownloadRpt");
                } else {
                    jAlert("Sorry No Record Found!", "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    };

    self.DatatableCall = function () {
        if ($("#gvReport > tbody > tr").length == self.ResultArray().length) {
            $("#gvReport").DataTable({
                "autoWidth": false,
                "destroy": true,
                "scrollX": true,
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

    self.clearFilter = function () {
        $("#txtRegion").val("");
        $("#hdfRegionId").val("0");
        $("#txtFrom").val("");
        $("#txtto").val("");
        //self.search();
        if (self.ResultArray().length > 0) {
            $("#gvReport").DataTable({
                "autoWidth": false,
                "destroy": true
            }).destroy();
            self.ResultArray.removeAll();
        }
    };

    self.loadGrid = function () {
        $("#gvReport").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.ResultArray.removeAll();
    }
    self.loadGrid();
};

var mainViewModel = new SearchModel();
ko.applyBindings(mainViewModel);

$(document).ready(function () {
    //Region Auto Complete
    $("#txtRegion").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfRegionId").val("0");
        }

        $("#txtRegion").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "../Report/GetAutoCompleteRegion",
                    data: "{ 'txt' : '" + $("#txtRegion").val() + "'}",
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
                $("#hdfRegionId").val(i.item.val);
                $("#txtRegion").val(i.item.label);
            }
        });
    });

    $("#txtRegion").focusout(function () {
        var hdfId = $("#hdfRegionId").val();
        var txtCurName = $("#txtRegion").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtRegion").val("");
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