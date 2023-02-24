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
        var inputval = "";
        if ($("#txtFrom").val() == null || $("#txtFrom").val() == "") {
            jAlert("From Date Should not be empty !", "Message");
            return false;
        }
        else if ($("#txtto").val() == null || $("#txtto").val() == "") {
            jAlert("To Date Should not be empty!", "Message");
            return false;
        }
        var Inputfilter = {
            cbfno: inputval,
            Fromdate: $("#txtFrom").val(),
            Todate: $("#txtto").val()
        }
        $.ajax({
            type: "post",
            url: "../Report/FetchReportCBFData",
            data: ko.toJSON(Inputfilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                if (response != null && response != "") {
                    var Data1 = "", Data2 = "";
                    $("#gvReport").DataTable({
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
    }
    self.Download = function () {
        var inputval = "";
        if ($("#txtFrom").val() == null || $("#txtFrom").val() == "") {
            jAlert("From Date Should not be empty !", "Message");
            return false;
        }
        else if ($("#txtto").val() == null || $("#txtto").val() == "") {
            jAlert("To Date Should not be empty!", "Message");
            return false;
        }
        var Inputfilter = {
            cbfno: inputval,
            Fromdate: $("#txtFrom").val(),
            Todate: $("#txtto").val()
        }
        $.ajax({
            type: "post",
            url: "../Report/DownloadcbfReportData",
            data: ko.toJSON(Inputfilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                if (response == "process") {
                    ko.utils.postJson("../Report/DownloadCBFRpt");
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
        $("#txtcbfno").val("");
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