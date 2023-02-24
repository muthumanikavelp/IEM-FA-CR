var objDialog;
UrlDet = UrlDet.replace("GetShopStatus", "");
var ShopStatusModel = function () {
    var self = this;

    self.ShopDetailsArray = ko.observableArray();
    self.ShopDetailSummary = ko.observableArray();

    self.Search = function () {
        var data = {
            DateFrom: $("#txtDateFrom").val(),
            DateTo: $("#txtDateTo").val()
        };
        ShowLoading(true);
        self.ShopDetailsArray.removeAll();
        $.ajax({
            type: "post",
            url: UrlDet + "GetShopStatus",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                ShowLoading(false);
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    
                    self.ShopDetailsArray(Data1);
                } else if (self.ShopDetailsArray().length == 0) {
                    jAlert("Sorry no records found!", "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                ShowLoading(false);
            }
        });
    };

    self.Received = function (item) {
        if (item.Received == 0) {
            jAlert("Sorry no records found!", "Message");
            return false;
        } else {
            var _dateFrom = "", _dateTo = "";

            if (item.SNo != null && item.SNo != '') {
                _dateFrom = item.InwardDate;
                _dateTo = "";
            } else {
                _dateFrom = $("#txtDateFrom").val();
                _dateTo = $("#txtDateTo").val();
            }

            var data = {
                DateFrom: _dateFrom,
                DateTo: _dateTo,
                Activity: "Received"
            };
            ShowLoading(true);
            self.ShopDetailSummary.removeAll();
            $.ajax({
                type: "post",
                url: UrlDet + "GetShopStatusSummary",
                data: JSON.stringify(data),
                contentType: "application/json;",
                success: function (response) {
                    ShowLoading(false);
                    var Data1 = "";
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                        Data1 = JSON.parse(response.Data1);

                        self.ShopDetailSummary(Data1);
                    }
                    if (self.ShopDetailSummary().length == 0) {
                        jAlert("Sorry no records found!", "Message");
                    } else {
                        ShowDialog();
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    ShowLoading(false);
                }
            });
        }
    };

    self.Inprocess = function (item) {
        if (item.Inprocess == 0) {
            jAlert("Sorry no records found!", "Message");
            return false;
        } else {
            var _dateFrom = "", _dateTo = "";

            if (item.SNo != null && item.SNo != '') {
                _dateFrom = item.InwardDate;
                _dateTo = "";
            } else {
                _dateFrom = $("#txtDateFrom").val();
                _dateTo = $("#txtDateTo").val();
            }

            var data = {
                DateFrom: _dateFrom,
                DateTo: _dateTo,
                Activity: "Inprocess"
            };

            self.ShopDetailSummary.removeAll();
            ShowLoading(true);
            $.ajax({
                type: "post",
                url: UrlDet + "GetShopStatusSummary",
                data: JSON.stringify(data),
                contentType: "application/json;",
                success: function (response) {
                    ShowLoading(false);
                    var Data1 = "";
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                        Data1 = JSON.parse(response.Data1);

                        self.ShopDetailSummary(Data1);
                    }
                    if (self.ShopDetailSummary().length == 0) {
                        jAlert("Sorry no records found!", "Message");
                    } else {
                        ShowDialog();
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    ShowLoading(false);
                }
            });
        }
    };

    self.Paid = function (item) {
        if (item.Paid == 0) {
            jAlert("Sorry no records found!", "Message");
            return false;
        } else {
            var _dateFrom = "", _dateTo = "";

            if (item.SNo != null && item.SNo != '') {
                _dateFrom = item.InwardDate;
                _dateTo = "";
            } else {
                _dateFrom = $("#txtDateFrom").val();
                _dateTo = $("#txtDateTo").val();
            }

            var data = {
                DateFrom: _dateFrom,
                DateTo: _dateTo,
                Activity: "Paid"
            };
            ShowLoading(true);
            self.ShopDetailSummary.removeAll();
            $.ajax({
                type: "post",
                url: UrlDet + "GetShopStatusSummary",
                data: JSON.stringify(data),
                contentType: "application/json;",
                success: function (response) {
                    ShowLoading(false);
                    var Data1 = "";
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                        Data1 = JSON.parse(response.Data1);

                        self.ShopDetailSummary(Data1);
                    }
                    if (self.ShopDetailSummary().length == 0) {
                        jAlert("Sorry no records found!", "Message");
                    } else {
                        ShowDialog();
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    ShowLoading(false);
                }
            });
        }
    };

    self.Inex = function (item) {
        if (item.Inex == 0) {
            jAlert("Sorry no records found!", "Message");
            return false;
        } else {
            var _dateFrom = "", _dateTo = "";

            if (item.SNo != null && item.SNo != '') {
                _dateFrom = item.InwardDate;
                _dateTo = "";
            } else {
                _dateFrom = $("#txtDateFrom").val();
                _dateTo = $("#txtDateTo").val();
            }
           
            var data = {
                DateFrom: _dateFrom,
                DateTo: _dateTo,
                Activity: "Inex"
            };
            ShowLoading(true);
            self.ShopDetailSummary.removeAll();
            $.ajax({
                type: "post",
                url: UrlDet + "GetShopStatusSummary",
                data: JSON.stringify(data),
                contentType: "application/json;",
                success: function (response) {
                    ShowLoading(false);
                    var Data1 = "";
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                        Data1 = JSON.parse(response.Data1);

                        self.ShopDetailSummary(Data1);
                    }
                    if (self.ShopDetailSummary().length == 0) {
                        jAlert("Sorry no records found!", "Message");
                    } else {
                        ShowDialog();
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    ShowLoading(false);
                }
            });
        }
    };

    self.Hold = function (item) {
        if (item.Hold == 0) {
            jAlert("Sorry no records found!", "Message");
            return false;
        } else {
            var _dateFrom = "", _dateTo = "";

            if (item.SNo != null && item.SNo != '') {
                _dateFrom = item.InwardDate;
                _dateTo = "";
            } else {
                _dateFrom = $("#txtDateFrom").val();
                _dateTo = $("#txtDateTo").val();
            }

            var data = {
                DateFrom: _dateFrom,
                DateTo: _dateTo,
                Activity: "Hold"
            };
            ShowLoading(true);
            self.ShopDetailSummary.removeAll();
            $.ajax({
                type: "post",
                url: UrlDet + "GetShopStatusSummary",
                data: JSON.stringify(data),
                contentType: "application/json;",
                success: function (response) {
                    ShowLoading(false);
                    var Data1 = "";
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                        Data1 = JSON.parse(response.Data1);

                        self.ShopDetailSummary(Data1);
                    }
                    if (self.ShopDetailSummary().length == 0) {
                        jAlert("Sorry no records found!", "Message");
                    } else {
                        ShowDialog();
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    ShowLoading(false);
                }
            });
        }
    };

    self.columnNames = ko.computed(function () {
        if (self.ShopDetailSummary().length === 0) {
            return [];
        } else {
            var props = [];
            var obj = self.ShopDetailSummary()[0];
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

        self.ShopDetailsArray.removeAll();
        self.ShopDetailSummary.removeAll();
        IsValidSubmit();
    };
};

var mainViewModel = new ShopStatusModel();
ko.applyBindings(mainViewModel);

$(document).ready(function () {
    objDialog = $("[id$='ShowSummary']");
    objDialog.dialog({
        autoOpen: false,
        modal: true,
        width: 800,
        height: 450,
        duration: 300
    });

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
    IsValidSubmit();
});

function IsValidSubmit() {
    var _DateFrom = $("#txtDateFrom").val();
    var _DateTo = $("#txtDateTo").val();

    if (_DateFrom.trim() == "" || _DateTo == "") {
        $('#btnsearch').attr('disabled', true);
    }
    else {
        $('#btnsearch').attr('disabled', false);
    }
}

function ShowLoading(IsShow) {
    if (IsShow)
        $('#loadingImg').attr("style", "display:block; text-align:center; padding-top:5px; color:maroon;");
    else
        $('#loadingImg').attr("style", "display:none; text-align:center; padding-top:5px; color:maroon;");
}

function ShowDialog() {
    $('#ShowSummary').attr("style", "display:block;");
    objDialog.dialog({ title: 'Shop Status Summary' });
    objDialog.dialog("open");
    return false;
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

function isNumberAndLetterAndSpace(evt) {
    var key = evt.keyCode;
    return ((key >= 65 && key <= 90) || key == 8 || (key >= 97 && key <= 122) || (key >= 48 && key <= 57) || key == 127 || key == 32);
}

function isEvent(evt) {
    return false;
}