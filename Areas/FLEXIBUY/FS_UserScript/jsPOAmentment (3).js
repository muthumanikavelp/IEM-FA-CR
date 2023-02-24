

UrlDet = UrlDet.replace("CBFSelection", "");
UrlHelper = UrlHelper.replace("GetProjectOwner", "");

var SearchModel = function (id) {

    var self = this;

    self.POResultArray = ko.observableArray();

    this.GetPOSearch = function (flag) {

        if (flag == "0") {
            $("#txtPONo").val("");
            $("#txtPODate").val("");
            $("#txtVendor").val("");
            $("#hfVendor").val("0");
        }

        var PONo = $("#txtPONo").val();
        var PODate = $("#txtPODate").val();
        var VendorId = $("#hfVendor").val();

        var inputFilter = {
            PONo: PONo,
            PODate: PODate,
            VendorId: VendorId
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetPOResult",
            data: JSON.stringify(inputFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                self.POResultArray(Data2);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });

    };

    this.SetPOAmendment = function (POHeaderGId) {

        jConfirm("Are you sure? Want to Amendment this PO! This Process can't be Reverted!", "Confirm", function (callback) {
            if (callback == true) {

                var inputFilter = {
                    POHeaderGId: POHeaderGId
                };
                $.ajax({
                    type: "post",
                    url: UrlDet + "SetPOAmendment",
                    data: JSON.stringify(inputFilter),
                    contentType: "application/json;",
                    success: function (response) {
                        var Data1 = "";
                        if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                            Data1 = JSON.parse(response.Data1);
                            if (Data1[0].Clear == false) {
                                jAlert(Data1[0].Message, "Message");
                                return false;
                            }
                            else {
                                location = UrlSuccess + "/" + Data1[0].POHeaderGId;
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

    self.GetPOSearch("0");
};

var mainViewModel = new SearchModel();
ko.applyBindings(mainViewModel);

$(document).ready(function () {

    var objDate = new Date();
    var Presentyear = objDate.getFullYear();

    $("#txtPODate").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        maxDate: 'd'
    });

    $("#txtVendor").keyup(function (e) {
        if (e.which != 13) {
            $("#hfVendor").val("0");
        }

        $("#txtVendor").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteVendor",
                    data: "{ 'txt' : '" + $("#txtVendor").val() + "'}",
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
                $("#hfVendor").val(i.item.val);
                $("#txtVendor").val(i.item.label);
            },
            minLength: 1
        });

    });

    $("#txtVendor").focusout(function () {
        var hdfId = $("#hfVendor").val();
        var txtCurName = $("#txtVendor").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtVendor").val("");
        }
    });


});






