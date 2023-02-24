UrlDet = UrlDet.replace("GetLocalConveyanceDateWaise", "");
UrlQuery = UrlQuery.replace("GetQueryInvoiceDateWaise", "");
//Region Reports
var LocalConveyanceModel = function () {
    var self = this;
    
    self.LocalConveyanceArray = ko.observableArray();
    self.QueryInvoiceArray = ko.observableArray();

    //Date Wise Local Conveyance    
    self.Search = function () {
        var data = {
            DateFrom: $("#txtDateFrom").val(),
            DateTo: $("#txtDateTo").val()
        }; 
        var IsDateValid = ValidateDate();
        if (IsDateValid == false) {
            jAlert("Please Select Date between 31 days!", "Message");
            self.LocalConveyanceArray.removeAll();
            IsValidSubmit();
            return;
        }
        ShowLoading(true);
        self.LocalConveyanceArray.removeAll();
        $.ajax({
            type: "post",           
            url: UrlDet + "GetLocalConveyanceDateWaise",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                ShowLoading(false);
                var Data1 = ""; 
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    self.LocalConveyanceArray(Data1);
                } else if (self.LocalConveyanceArray().length == 0) {
                    jAlert("Sorry no records found!", "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                ShowLoading(false);
            }
        });
    };
    this.DownloadLocalConveyanceDateWise = function () {

        var DateFrom = $("#txtDateFrom").val();
        var DateTo = $("#txtDateTo").val();

        ko.utils.postJson(UrlDet + "DownloadLocalConveyanceDateWise?DateFrom=" + DateFrom + "&DateTo=" + DateTo);

    }
    self.columnNames = ko.computed(function () {
        if (self.LocalConveyanceArray().length === 0) {
            return [];
        } else {
            var props = [];
            var obj = self.LocalConveyanceArray()[0];
            for (var name in obj)
                props.push(name);
            return props;
        }
         
    });
    self.Clear = function () {
        $("#txtDateFrom").val("");
        $("#txtDateTo").val("");

        $('.glEntry').removeClass('required').removeClass('valid');
        $('.glEntry').addClass('required');

        self.LocalConveyanceArray.removeAll();

        IsValidSubmit();
    };

    // Query Invoice
    self.SearchQuery = function () {
        var data = {
            DateFrom: $("#txtDateFromQ").val(),
            DateTo: $("#txtDateToQ").val()
        };
        ShowLoading(true);
        var IsDateValid = ValidateDateQ(); 
        if (IsDateValid == false) {
            jAlert("Please Select Date between 31 days!", "Message");
            self.QueryInvoiceArray.removeAll();
            IsValidSubmitQ();
            return;
        }
        
        self.QueryInvoiceArray.removeAll();
        $.ajax({
            type: "post",
            url: UrlQuery + "GetQueryInvoiceDateWaise",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) { 
                ShowLoading(false);
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    self.QueryInvoiceArray(Data1);
                } else if (self.QueryInvoiceArray().length == 0) {
                    jAlert("Sorry no records found!", "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                ShowLoading(false);
            }
        });
    };
    this.DownloadQueryInvoiceDateWise = function () {

        var DateFrom = $("#txtDateFromQ").val();
        var DateTo = $("#txtDateToQ").val();

        ko.utils.postJson(UrlQuery + "DownloadQueryInvoiceDateWise?DateFrom=" + DateFrom + "&DateTo=" + DateTo);

    }
    self.columnNamesQ = ko.computed(function () {
        if (self.QueryInvoiceArray().length === 0) {
            return [];
        } else {
            var props = [];
            var obj = self.QueryInvoiceArray()[0];
            for (var name in obj)
                props.push(name);
            return props;
        }

    });
    self.ClearQ = function () {
        $("#txtDateFromQ").val("");
        $("#txtDateToQ").val("");

        $('.glQEntry').removeClass('required').removeClass('valid');
        $('.glQEntry').addClass('required');

        self.QueryInvoiceArray.removeAll();

        IsValidSubmitQ();
    };
};
//End Region
 
// a and b are javascript Date objects
function dateDiffInDays(a, b) {
    // Discard the time and time-zone information.
    var utc1 = Date.UTC(a.getFullYear(), a.getMonth(), a.getDate());
    var utc2 = Date.UTC(b.getFullYear(), b.getMonth(), b.getDate());
    alert(utc1);
    alert(utc2);
    return Math.floor((utc2 - utc1) / _MS_PER_DAY);
}

// test it

function ShowLoading(IsShow) {
    if (IsShow)
        $('#loadingImg').attr("style", "display:block; text-align:center; padding-top:5px; color:maroon;");
    else
        $('#loadingImg').attr("style", "display:none; text-align:center; padding-top:5px; color:maroon;");
}
function ValidateDate() {
    var DateFrom = $("#txtDateFrom").val();
    var DateTo = $("#txtDateTo").val();

    var d = DateFrom.split('/');
    var d1 = DateTo.split('/');
    DateFrom = d[1] + '/' + d[0] + '/' + d[2];
    DateTo = d1[1] + '/' + d1[0] + '/' + d1[2];
    var dt1 = new Date(DateFrom);
    var dt2 = new Date(DateTo);

    var timeDiff = Math.abs(dt2.getTime() - dt1.getTime());
    var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));

    var diff = Math.floor((Date.UTC(dt2.getFullYear(), dt2.getMonth(), dt2.getDate()) - Date.UTC(dt1.getFullYear(), dt1.getMonth(), dt1.getDate())) / (1000 * 60 * 60 * 24)) + 1;
     
    if (diff > 0 && diff <= 31) {
        return true;
    }
    else {
        return false;
    }
}

function ValidateDateQ() {
    var DateFrom = $("#txtDateFromQ").val();
    var DateTo = $("#txtDateToQ").val();

    var d = DateFrom.split('/');
    var d1 = DateTo.split('/');
    DateFrom = d[1] + '/' + d[0] + '/' + d[2];
    DateTo = d1[1] + '/' + d1[0] + '/' + d1[2];
    var dt1 = new Date(DateFrom);
    var dt2 = new Date(DateTo);

    var timeDiff = Math.abs(dt2.getTime() - dt1.getTime());
    var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));

    var diff = Math.floor((Date.UTC(dt2.getFullYear(), dt2.getMonth(), dt2.getDate()) - Date.UTC(dt1.getFullYear(), dt1.getMonth(), dt1.getDate())) / (1000 * 60 * 60 * 24)) + 1;

    if (diff > 0 && diff <= 31) {

        return true;
    }
    else {

        return false;
    }

}

var mainViewModel = new LocalConveyanceModel();
ko.applyBindings(mainViewModel); 
 
 