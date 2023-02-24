UrlDet = UrlDet.replace("GetProductivityTracking", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");
var ProductivityTrackingModel = function () {
    var self = this;

    self.ActivityArray = ko.observableArray();
    self.ProductTrackingArray = ko.observableArray();
    self.EProductTrackingArray = ko.observableArray();

    self.loadActivity = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "LoadActivityDropDown",
            data: '{}',
            contentType: "application/json;",
            success: function (response) {
                self.ActivityArray(response);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    //Date Wise Productivity Tracking
    self.Search = function () {
        var data = {
            DateFrom: $("#txtDateFrom").val(),
            DateTo: $("#txtDateTo").val(),
            Activity: $("#ddlActivity").val()
        };

        ShowLoading(true);
        self.ProductTrackingArray.removeAll();
        $.ajax({
            type: "post",
            url: UrlDet + "GetProductivityTracking",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                ShowLoading(false);
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    
                    self.ProductTrackingArray(Data1);
                } else if (self.ProductTrackingArray().length == 0) {
                    jAlert("Sorry no records found!", "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                ShowLoading(false);
            }
        });
    };


    //------------------------------------Coded By Subburaj 07.05.2016--------------------------------//

    this.Downloadproductivitydatewise = function () {

        var DateFrom = $("#txtDateFrom").val();
        var DateTo = $("#txtDateTo").val();
        var Activity = $("#ddlActivity").val();


        ko.utils.postJson(UrlDet + "Downloadproductivitydatewise?DateFrom=" + DateFrom + "&DateTo=" + DateTo + "&Activity=" + Activity);

    }
    //----------------------------------------End-----------------------------------------------------//

    self.columnNames = ko.computed(function () {
        if (self.ProductTrackingArray().length === 0) {
            return [];
        } else {
            var props = [];
            var obj = self.ProductTrackingArray()[0];
            for (var name in obj)
                props.push(name);
            return props;
        }
    });

    self.Clear = function () {
        $("#txtDateFrom").val("");
        $("#txtDateTo").val("");
        $("#ddlActivity").val("0");

        $('.glEntry').removeClass('required').removeClass('valid');
        $('.glEntry').addClass('required');

        self.ProductTrackingArray.removeAll();

        IsValidSubmit();
    };
   
    //Employee Wise Productivity Tracking
    self.ESearch = function () {
        var data = {
            DateFrom: $("#txtEDateFrom").val(),
            DateTo: $("#txtEDateTo").val(),
            EmpNameId: $("#hdfEEmpCodeId").val(),
            Activity: $("#ddlEActivity").val()
        };

        EShowLoading(true);
        self.EProductTrackingArray.removeAll();
        $.ajax({
            type: "post",
            url: UrlDet +"GetProductivityTrackingEmployee",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                EShowLoading(false);
                var Data2 = "";
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);

                    self.EProductTrackingArray(Data2);

                    $('#sumtable thead th').each(function (i) {
                        calculateColumn(i);
                    });

                } else if (self.EProductTrackingArray().length == 0) {
                    jAlert("Sorry no records found!", "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                EShowLoading(false);
            }
        });
    };

    //------------------------------------Coded By Subburaj 07.05.2016--------------------------------//

    this.Downloadproductivitymonthwise = function () {

        var DateFrom = $("#txtEDateFrom").val();
        var DateTo = $("#txtEDateTo").val();
        var Activity = $("#ddlEActivity").val();


        ko.utils.postJson(UrlDet + "Downloadproductivitymonthwise?DateFrom=" + DateFrom + "&DateTo=" + DateTo + "&Activity=" + Activity);

    }
    //----------------------------------------End-----------------------------------------------------//


    function calculateColumn(index) {
        var total = 0;
        $('#sumtable tr').each(function () {
            var value = parseInt($('td', this).eq(index).text());
            if (isNaN(value)) {
                value = 0;
            }
            
            if (!isNaN(value)) {
                total += value;
            }
        });

        if(index == 0)
            $('#sumtable tfoot td').eq(index).text("Total");
        else 
            $('#sumtable tfoot td').eq(index).text(total);
    }

    self.EcolumnNames = ko.computed(function () {
        if (self.EProductTrackingArray().length === 0) {
            return [];
        } else {
            var props = [];
            var obj = self.EProductTrackingArray()[0];
            for (var name in obj)
                props.push(name);
            return props;
        }
        /*return ko.utils.arrayMap(self.ProductTrackingArray(), function (item) {
            var updatedItem = {};
            ko.utils.arrayForEach(item, function (column) {
                updatedItem[column.ColumnName] = column.Value;
            });
            return updatedItem;
        });*/
    });

    self.EClear = function () {
        $("#txtEDateFrom").val("");
        $("#txtEDateTo").val("");
        $("#ddlEActivity").val("0");
        
        $("#txtEEmpCode").val("");
        $("#hdfEEmpCodeId").val("0");

        $('.glEEntry').removeClass('required').removeClass('valid');
        $('.glEEntry').addClass('required');

        self.EProductTrackingArray.removeAll();

        IsValidSubmitE();
    };
    
    self.loadActivity();
};

var mainViewModel = new ProductivityTrackingModel();
ko.applyBindings(mainViewModel);

$(document).ready(function () {
    $(".fsDatePicker").datepicker({
        yearRange: '1900:+nn',
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
    });

    $(".fsDatePicker").keyup(function (e) {
        if (e.keyCode == 8 || e.keyCode == 46)
            $.datepicker._clearDate(this);

        $(this).datepicker('show');
    });

    //Date Wise Productivity Tracking
    $('.glEntry').change(function () {
        IsValidSubmit();
        var _data = $(this).val();
        var trigerName = $(this).attr('id');
        if (_data.trim() != "") {
            $("#" + trigerName).removeClass('required').removeClass('valid');
            $("#" + trigerName).addClass('valid');
        }
        else {
            $("#" + trigerName).removeClass('valid').removeClass('required');
            $("#" + trigerName).addClass('required');
        }
    });

    //Employee Wise Productivity Tracking

    //Load Employee Code Auto Complete
    $("#txtEEmpCode").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfEEmpCodeId").val("0");
        }

        $("#txtEEmpCode").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoReceiptEmployee",
                    data: "{ 'txt' : '" + $("#txtEEmpCode").val() + "'}",
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
                $("#hdfEEmpCodeId").val(i.item.val);
                $("#txtEEmpCode").val(i.item.label);
            }
        });
    });

    $("#txtEEmpCode").focusout(function () {
        var hdfId = $("#hdfEEmpCodeId").val();
        var txtCurName = $("#txtEEmpCode").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtEEmpCode").val("");
        }
    });

    $('.glEEntry').change(function () {
        IsValidSubmitE();
        var _data = $(this).val();
        var trigerName = $(this).attr('id');
        if (_data.trim() != "") {
            $("#" + trigerName).removeClass('required').removeClass('valid');
            $("#" + trigerName).addClass('valid');
        }
        else {
            $("#" + trigerName).removeClass('valid').removeClass('required');
            $("#" + trigerName).addClass('required');
        }
    });

    IsValidSubmitE();

    IsValidSubmit();
});

function ShowLoading(IsShow) {
    if (IsShow)
        $('#loadingImg').attr("style", "display:block; text-align:center; padding-top:5px; color:maroon;");
    else
        $('#loadingImg').attr("style", "display:none; text-align:center; padding-top:5px; color:maroon;");
}

function EShowLoading(IsShow) {
    if (IsShow)
        $('#EloadingImg').attr("style", "display:block; text-align:center; padding-top:5px; color:maroon;");
    else
        $('#EloadingImg').attr("style", "display:none; text-align:center; padding-top:5px; color:maroon;");
}

function IsValidSubmit() {
    var _glFrom = $("#txtDateFrom").val();
    var _glTo = $("#txtDateTo").val();
    //var _glac = $("#ddlActivity").val();

    if (_glFrom.trim() == "" || _glTo.trim() == "" /*|| _glac == "" || _glac == "0"*/) {
        $('#btnsearch').attr('disabled', true);
    }
    else {
        $('#btnsearch').attr('disabled', false);
    }
}

function IsValidSubmitE() {
    var _glFrom = $("#txtEDateFrom").val();
    var _glTo = $("#txtEDateTo").val();

    if (_glFrom.trim() == "" || _glTo.trim() == "") {
        $('#btnEsearch').attr('disabled', true);
    }
    else {
        $('#btnEsearch').attr('disabled', false);
    }
}

function isEvent(evt) {
    return false;
}