UrlDet = UrlDet.replace("FetchCBFTrackerReport", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");
var CBFTrackingModel = function () {
    var self = this;

    self.CBFTrackingArray = ko.observableArray();

    self.Search = function (id) {
        var data = {
            Status: $("#ddlStatus").val(),
        };
        var count = $("#gvReport").children('tbody').children('tr').children('td').length;
        //var count = $("table > tbody").find("> tr:first > td").length;
        if (count != 0 && count != 4) {
        $("#gvReport").DataTable({
            "destroy": true
        }).destroy();}
        self.CBFTrackingArray.removeAll();
  
    $.ajax({
        type: "post",
        url: UrlDet + "FetchCBFTrackerReport",
        data: JSON.stringify(data),
        contentType: "application/json;",
        async: false,
        success: function (response) {
            var Data1 = "", Data2 = "";
            if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                Data1 = JSON.parse(response.Data1);
                if (Data1[0].Clear == false) {
                    jAlert(Data1[0].Message, "Message");
                    return false;
                }
            }
            if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                Data2 = JSON.parse(response.Data2);
                self.CBFTrackingArray(Data2);
            }
            if (Data2 == "" && id != 0) {
                jAlert("Sorry No Record Found!", "Message");
                return false;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            // ShowLoading(false);
        }
    });
};

self.Download = function () {
    var data = {
        Status: $("#ddlStatus").val(),
    };

    $.ajax({
        type: "post",
        url: UrlDet + "DownloadCBFTrackerReport",
        data: JSON.stringify(data),
        async: false,
        contentType: "application/json;",
        success: function (response) {
            if (response == "process") {
                ko.utils.postJson(UrlDet + "DownloadExcelCBFTrackerRpt");
            } else {
                jAlert("Sorry No Record Found!", "Message");
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        }
    });
};

self.columnNames = ko.computed(function () {
    if (self.CBFTrackingArray().length === 0) {
        return [];

    } else {
        var props = [];
        var obj = self.CBFTrackingArray()[0];
        var i = 0;
        for (var name in obj) {
            if (i != 1)
                props.push(name);
            i++;
        }

        return props;
    }
});

self.clearFilter = function () {
    $("#ddlStatus").val("ALL");
    self.Search(0);
};

this.DatatableCall = function () {
    if ($("#gvReport > tbody > tr").length == self.CBFTrackingArray().length) {
        $("#gvReport").DataTable({
            "autoWidth": false,
            "destroy": true,
            "scrollX": true,
            "aoColumnDefs": [{
                "aTargets": ["nosort"],
                "bSortable": false
            }]
        });
    }
};

self.Search(0);
};

var mainViewModel = new CBFTrackingModel();
ko.applyBindings(mainViewModel);
//$(document).ready(function () {
//    jQuery.extend(jQuery.fn.dataTableExt.oSort, {
//        "date-uk-pre": function (a) {
//            a = a.split(">")[1].split("<")[0];
//            var ukDatea = a.split("/");
//            return (ukDatea[2] + ukDatea[1] + ukDatea[0]) * 1;
//        },
//        "date-uk-asc": function (a, b) {
//            return ((a < b) ? -1 : ((a > b) ? 1 : 0));
//        },
//        "date-uk-desc": function (a, b) {
//            return ((a < b) ? 1 : ((a > b) ? -1 : 0));
//        }
//    });
//});

function isEvent(evt) {
    return false;
}
