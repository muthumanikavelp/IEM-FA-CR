
var Sectorreport = function () {
    var self = this;

    self.SlNo = ko.observable();
    self.Branchcode = ko.observable();
    self.Employeecode = ko.observable();
    self.Employeename = ko.observable();
    self.Amount = ko.observable();
    self.ChqNO = ko.observable();

    var sectorreports = {

        SlNo: self.SlNo,
        Branchcode: self.Branchcode,
        Employeecode: self.Employeecode,
        Amount: sef.Amount,
        ChqNO: self.ChqNO

    }

    self.Sectorreports = ko.observable();
    self.Sectorreportsummary = ko.observableArray();



    self.DatatableCall = function () {
        if ($("#gvsectorreport > tbody > tr").length == self.Sectorreportsummary().length) {
            $("#gvsectorreport").DataTable({
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




}

var mainreport = new Sectorreport();
ko.applyBindings(mainreport);