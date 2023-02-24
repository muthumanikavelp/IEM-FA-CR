

MUrlDet = MUrlDet.replace("GetBankSummary", "");
MUrlHelper = MUrlHelper.replace("GetAutoCompleteCourier", "");

var SearchModel = function (id) {
    var self = this;

    self.BankSummaryArray = ko.observableArray();
    self.AmortReportArray = ko.observableArray();
    self.AdvanceReportArray = ko.observableArray();
    self.PPXReportArray = ko.observableArray();
    self.CreditReportArray = ko.observableArray();
    self.DebitReportArray = ko.observableArray();
    self.CenvatReportArray = ko.observableArray();

    //Bank Summary Report

    this.SearchBankSummary = function () {
        var DateFrom = $("#txtDateFrom").val();
        var DateTo = $("#txtDateTo").val();
        var ECFNo = "";
        var View = $("#ddlType").val();
        if ($.trim(View) == "0") {
            jAlert("Ensure Report Type!", "Message");
            return false;
        }

        if ($.trim(DateFrom) == "") {
            jAlert("Ensure Date From!", "Message");
            return false;
        }

        if ($.trim(DateTo) == "") {
            jAlert("Ensure Date To!", "Message");
            return false;
        }
        var Filter = {
            DateFrom: DateFrom,
            DateTo: DateTo,
            ECFNo: ECFNo,
            View: View
        };

        $.ajax({
            type: "post",
            async: false,
            url: MUrlDet + "GetBankSummary",
            data: ko.toJSON(Filter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                $("#gvBankSummary").DataTable({
                    "autoWidth": false,
                    "destroy": true
                }).destroy();
                self.BankSummaryArray("");
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    $("#txtDateFrom").attr('disabled', 'disabled');
                    $("#txtDateTo").attr('disabled', 'disabled');
                    $("#ddlType").attr('disabled', 'disabled');
                    self.BankSummaryArray(Data2);
                }
                else {
                    jAlert("Sorry no record found!", "Message");
                    return false;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.DownloadBankSummary = function()
    {
        var DateFrom = $("#txtDateFrom").val();
        var DateTo = $("#txtDateTo").val();
        var ECFNo = "";
        var View = $("#ddlType").val();
        ko.utils.postJson(MUrlDet + "DownloadBankSummary?DateFrom=" + DateFrom + "&DateTo=" + DateTo + "&ECFNo=" + ECFNo + "&View=" + View);
    }

    this.ClearBankSummary = function () {
        $("#txtDateFrom").val("");
        $("#txtDateTo").val("");
        $("#ddlType").val("0");
        $("#txtDateFrom").removeClass('valid').addClass('required');
        $("#txtDateTo").removeClass('valid').addClass('required');
        $("#ddlType").removeClass('valid').addClass('required');
        $("#txtDateFrom").removeAttr('disabled');
        $("#txtDateTo").removeAttr('disabled');
        $("#ddlType").removeAttr('disabled');
        $("#gvBankSummary").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.BankSummaryArray([]);
    };

    this.DatatableCall = function () {
        if ($("#gvBankSummary > tbody > tr").length == self.BankSummaryArray().length && self.BankSummaryArray().length != 0) {
            $("#gvBankSummary").DataTable({
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

    //Amort Report

    this.SearchAmortReport = function () {
        var DateFrom = $("#txtDateFrom").val();
        var DateTo = $("#txtDateTo").val();
        var ECFNo = $("#txtEcfNo").val();

        if ($.trim(DateFrom) == "") {
            jAlert("Ensure Date From!", "Message");
            return false;
        }

        if ($.trim(DateTo) == "") {
            jAlert("Ensure Date To!", "Message");
            return false;
        }
        var Filter = {
            DateFrom: DateFrom,
            DateTo: DateTo,
            ECFNo: ECFNo
        };

        $.ajax({
            type: "post",
            async: false,
            url: MUrlDet + "GetAmortReport",
            data: JSON.stringify(Filter),
            contentType: "application/json;",
            datatype: "json",
            success: function (response) {
                var Data1 = "", Data2 = "";
                $("#gvReport").DataTable({
                    "autoWidth": false,
                    "destroy": true
                }).destroy();
                self.AmortReportArray("");
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                {
                    Data2 = JSON.parse(response.Data2);
                    $("#txtDateFrom").attr('disabled', 'disabled');
                    $("#txtDateTo").attr('disabled', 'disabled');
                    $("#txtEcfNo").attr('disabled', 'disabled');
                    self.AmortReportArray(Data2);
                }
                else {
                    jAlert("Sorry no record found!", "Message");
                    return false;
                }
                
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.DownloadAmortReport = function () {
        var DateFrom = $("#txtDateFrom").val();
        var DateTo = $("#txtDateTo").val();
        var ECFNo = $("#txtEcfNo").val();
        ko.utils.postJson(MUrlDet + "DownloadAmortReport?DateFrom=" + DateFrom + "&DateTo=" + DateTo + "&ECFNo=" + ECFNo);
    }

    this.ClearAmortReport = function () {
        $("#txtDateFrom").val("");
        $("#txtDateTo").val("");
        $("#txtEcfNo").val("");
        $("#txtDateFrom").removeClass('valid').addClass('required');
        $("#txtDateTo").removeClass('valid').addClass('required');
        
        $("#txtDateFrom").removeAttr('disabled');
        $("#txtDateTo").removeAttr('disabled');
        $("#txtEcfNo").removeAttr('disabled');
        $("#gvReport").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.AmortReportArray([]);
    };

    this.DatatableStyleApply = function () {
        

        if ($("#gvReport > tbody > tr").length == self.AmortReportArray().length) {
            setTimeout(function () {
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
            }, 600);
        }
    };

    //Advance Report
    
    this.SearchAdvanceReport = function () {
        var DateFrom = $("#txtDateFrom").val();
        var DateTo = $("#txtDateTo").val();
        var LiqDateFrom = $("#txtLiqDateFrom").val();
        var LiqDateTo = $("#txtLiqDateTo").val();
        var ECFNo = $("#txtEcfNo").val();
        var Raiser = $("#txtRaiser").val();
        var Vendor = $("#txtVendor").val();
        var Glcode = $("#txtGL").val();
        var Location = $("#txtLoc").val();

        //if ($.trim(DateFrom) == "") {
        //    jAlert("Ensure Date From!", "Message");
        //    return false;
        //}

        //if ($.trim(DateTo) == "") {
        //    jAlert("Ensure Date To!", "Message");
        //    return false;
        //}

        //if ($.trim(LiqDateFrom) == "" && $.trim(LiqDateTo) != "") {
        //    jAlert("Ensure Liquidation Date From!", "Message");
        //    return false;
        //}

        //if ($.trim(LiqDateTo) == "" && $.trim(LiqDateFrom) != "") {
        //    jAlert("Ensure Liquidation Date To!", "Message");
        //    return false;
        //}

        var Filter = {
            DateFrom: DateFrom,
            DateTo: DateTo,
            LiqDateFrom: LiqDateFrom,
            LiqDateTo: LiqDateTo,
            ECFNo: ECFNo,
            Raiser: Raiser,
            Vendor: Vendor,
            Glcode: Glcode,
            Location: Location
        };

        $.ajax({
            type: "post",
            async: false,
            url: MUrlDet + "GetAdvanceReport",
            data: JSON.stringify(Filter),
            contentType: "application/json;",
            datatype: "json",
            success: function (response) {
                var Data1 = "", Data2 = "";
                $("#gvReport").DataTable({
                    "autoWidth": false,
                    "destroy": true
                }).destroy();
                self.AdvanceReportArray("");
                $("#gvReport tbody").empty();
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    //$("#txtDateFrom").attr('disabled', 'disabled');
                    //$("#txtDateTo").attr('disabled', 'disabled');
                    //$("#txtEcfNo").attr('disabled', 'disabled');
                    //$("#txtRaiser").attr('disabled', 'disabled');
                    //$("#txtVendor").attr('disabled', 'disabled');
                    //$("#txtGL").attr('disabled', 'disabled');
                    //$("#txtLoc").attr('disabled', 'disabled');
                    self.AdvanceReportArray(Data2);
                }
                else {
                    jAlert("Sorry no record found!", "Message");
                    return false;
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.DownloadAdvanceReport = function () {
        var DateFrom = $("#txtDateFrom").val();
        var DateTo = $("#txtDateTo").val();
        var LiqDateFrom = $("#txtLiqDateFrom").val();
        var LiqDateTo = $("#txtLiqDateTo").val();
        var ECFNo = $("#txtEcfNo").val();
        var Raiser = $("#txtRaiser").val();
        var Vendor = $("#txtVendor").val();
        var Glcode = $("#txtGL").val();
        var Location = $("#txtLoc").val();
        ko.utils.postJson(MUrlDet + "DownloadAdvanceReport?DateFrom=" + DateFrom + "&DateTo=" + DateTo + "&LiqDateFrom=" + LiqDateFrom + "&LiqDateTo=" + LiqDateTo + "&ECFNo=" + ECFNo + "&Raiser=" + Raiser + "&Vendor=" + Vendor + "&Glcode=" + Glcode + "&Location=" + Location);
    }

    this.ClearAdvanceReport = function () {
        $("#txtDateFrom").val("");
        $("#txtDateTo").val("");
        $("#txtLiqDateFrom").val("");
        $("#txtLiqDateTo").val("");
        $("#txtEcfNo").val("");
        $("#txtRaiser").val("");
        $("#txtVendor").val("");
        $("#txtGL").val("");
        $("#txtLoc").val("");
        //$("#txtDateFrom").removeClass('valid').addClass('required');
        //$("#txtDateTo").removeClass('valid').addClass('required');

        //$("#txtDateFrom").removeAttr('disabled');
        //$("#txtDateTo").removeAttr('disabled');
        //$("#txtLiqDateFrom").removeAttr('disabled');
        //$("#txtLiqDateTo").removeAttr('disabled');
        //$("#txtEcfNo").removeAttr('disabled');
        //$("#txtRaiser").removeAttr('disabled');
        //$("#txtVendor").removeAttr('disabled');
        //$("#txtGL").removeAttr('disabled');
        //$("#txtLoc").removeAttr('disabled');
        $("#gvReport").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.AdvanceReportArray([]);
    };

    this.DatatableAdvance = function () {
        if ($("#gvReport > tbody > tr").length == self.AdvanceReportArray().length) {
            setTimeout(function () {
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
            }, 600);
        }
    };

    //PPX Liquidation Report

    this.SearchPPXLiquidationReport = function () {
        var DateFrom = $("#txtDateFrom").val();
        var DateTo = $("#txtDateTo").val();
        var ECFNo = $("#txtEcfNo").val();

        //if ($.trim(DateFrom) == "") {
        //    jAlert("Ensure Date From!", "Message");
        //    return false;
        //}

        //if ($.trim(DateTo) == "") {
        //    jAlert("Ensure Date To!", "Message");
        //    return false;
        //}
        var Filter = {
            DateFrom: DateFrom,
            DateTo: DateTo,
            ECFNo: ECFNo
        };

        $.ajax({
            type: "post",
            async: false,
            url: MUrlDet + "GetPPXLiquidationReport",
            data: JSON.stringify(Filter),
            contentType: "application/json;",
            datatype: "json",
            success: function (response) {
                var Data1 = "", Data2 = "";
                $("#gvReport").DataTable({
                    "autoWidth": false,
                    "destroy": true
                }).destroy();
                self.PPXReportArray("");
                $("#gvReport tbody").empty();
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    //$("#txtDateFrom").attr('disabled', 'disabled');
                    //$("#txtDateTo").attr('disabled', 'disabled');
                    //$("#txtEcfNo").attr('disabled', 'disabled');
                    self.PPXReportArray(Data2);
                }
                else {
                    jAlert("Sorry no record found!", "Message");
                    return false;
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.DownloadPPXLiquidationReport = function () {
        var DateFrom = $("#txtDateFrom").val();
        var DateTo = $("#txtDateTo").val();
        var ECFNo = $("#txtEcfNo").val();
        ko.utils.postJson(MUrlDet + "DownloadPPXLiquidationReport?DateFrom=" + DateFrom + "&DateTo=" + DateTo + "&ECFNo=" + ECFNo);
    }

    this.ClearPPXLiquidationReport = function () {
        $("#txtDateFrom").val("");
        $("#txtDateTo").val("");
        $("#txtEcfNo").val("");
        //$("#txtDateFrom").removeClass('valid').addClass('required');
        //$("#txtDateTo").removeClass('valid').addClass('required');

        $("#txtDateFrom").removeAttr('disabled');
        $("#txtDateTo").removeAttr('disabled');
        $("#txtEcfNo").removeAttr('disabled');
        $("#gvReport").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.PPXReportArray([]);
    };

    this.DatatablePPXLiquidation = function () {

        if ($("#gvReport > tbody > tr").length == self.PPXReportArray().length) {
            setTimeout(function () {
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
            }, 600);
        }
    };

    //Credit Report

    this.SearchCreditReport = function () {
        var DateFrom = $("#txtDateFrom").val();
        var DateTo = $("#txtDateTo").val();
        var ECFNo = $("#txtEcfNo").val();
        var Raiser = $("#txtRaiser").val();
        var Vendor = $("#txtVendor").val();
        var Glcode = $("#txtGL").val();
        var Location = $("#txtLoc").val();
        //if ($.trim(DateFrom) == "") {
        //    jAlert("Ensure Date From!", "Message");
        //    return false;
        //}

        //if ($.trim(DateTo) == "") {
        //    jAlert("Ensure Date To!", "Message");
        //    return false;
        //}
        var Filter = {
            DateFrom: DateFrom,
            DateTo: DateTo,
            ECFNo: ECFNo,
            Raiser: Raiser,
            Vendor: Vendor,
            Glcode: Glcode,
            Location:Location
        };

        $.ajax({
            type: "post",
            async: false,
            url: MUrlDet + "GetCreditReport",
            data: JSON.stringify(Filter),
            contentType: "application/json;",
            datatype: "json",
            success: function (response) {
                var Data1 = "", Data2 = "";
                $("#gvReport").DataTable({
                    "autoWidth": false,
                    "destroy": true
                }).destroy();
                self.CreditReportArray("");
                $("#gvReport tbody").empty();
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    //$("#txtDateFrom").attr('disabled', 'disabled');
                    //$("#txtDateTo").attr('disabled', 'disabled');
                    //$("#txtEcfNo").attr('disabled', 'disabled');
                    //$("#txtRaiser").attr('disabled', 'disabled');
                    //$("#txtVendor").attr('disabled', 'disabled');
                    //$("#txtGL").attr('disabled', 'disabled');
                    //$("#txtLoc").attr('disabled', 'disabled');
                    self.CreditReportArray(Data2);
                }
                else {
                    jAlert("Sorry no record found!", "Message");
                    return false;
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.DownloadCreditReport = function () {
        var DateFrom = $("#txtDateFrom").val();
        var DateTo = $("#txtDateTo").val();
        var ECFNo = $("#txtEcfNo").val();
        var Raiser = $("#txtRaiser").val();
        var Vendor = $("#txtVendor").val();
        var Glcode = $("#txtGL").val();
        var Location = $("#txtLoc").val();
        ko.utils.postJson(MUrlDet + "DownloadCreditReport?DateFrom=" + DateFrom + "&DateTo=" + DateTo + "&ECFNo=" + ECFNo + "&Raiser=" + Raiser + "&Vendor=" +Vendor + "&Glcode=" +Glcode +"&Location=" +Location);
    }

    this.ClearCreditReport = function () {
        $("#txtDateFrom").val("");
        $("#txtDateTo").val("");
        $("#txtEcfNo").val("");
        $("#txtRaiser").val("");
        $("#txtVendor").val("");
        $("#txtGL").val("");
        $("#txtLoc").val("");
        //$("#txtDateFrom").removeClass('valid').addClass('required');
        //$("#txtDateTo").removeClass('valid').addClass('required');

        //$("#txtDateFrom").removeAttr('disabled');
        //$("#txtDateTo").removeAttr('disabled');
        //$("#txtEcfNo").removeAttr('disabled');
        //$("#txtRaiser").removeAttr('disabled');
        //$("#txtVendor").removeAttr('disabled');
        //$("#txtGL").removeAttr('disabled');
        //$("#txtLoc").removeAttr('disabled');
        $("#gvReport").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.CreditReportArray([]);
    };

    this.DatatableCredit = function () {

        if ($("#gvReport > tbody > tr").length == self.CreditReportArray().length) {
            setTimeout(function () {
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
            }, 600);
        }
    };

    //Debit Report

    this.SearchDebitReport = function () {
        var DateFrom = $("#txtDateFrom").val();
        var DateTo = $("#txtDateTo").val();
        var ECFNo = $("#txtEcfNo").val();
        var Raiser = $("#txtRaiser").val();
        var Vendor = $("#txtVendor").val();
        var Glcode = $("#txtGL").val();
        var Location = $("#txtLoc").val();
        //if ($.trim(DateFrom) == "") {
        //    jAlert("Ensure Date From!", "Message");
        //    return false;
        //}

        //if ($.trim(DateTo) == "") {
        //    jAlert("Ensure Date To!", "Message");
        //    return false;
        //}
        var Filter = {
            DateFrom: DateFrom,
            DateTo: DateTo,
            ECFNo: ECFNo,
            Raiser: Raiser,
            Vendor: Vendor,
            Glcode: Glcode,
            Location: Location
        };

        $.ajax({
            type: "post",
            async: false,
            url: MUrlDet + "GetDebitReport",
            data: JSON.stringify(Filter),
            contentType: "application/json;",
            datatype: "json",
            success: function (response) {
                var Data1 = "", Data2 = "";
                $("#gvReport").DataTable({
                    "autoWidth": false,
                    "destroy": true
                }).destroy();
                self.DebitReportArray("");
                $("#gvReport tbody").empty();
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    //$("#txtDateFrom").attr('disabled', 'disabled');
                    //$("#txtDateTo").attr('disabled', 'disabled');
                    //$("#txtEcfNo").attr('disabled', 'disabled');
                    //$("#txtRaiser").attr('disabled', 'disabled');
                    //$("#txtVendor").attr('disabled', 'disabled');
                    //$("#txtGL").attr('disabled', 'disabled');
                    //$("#txtLoc").attr('disabled', 'disabled');
                    self.DebitReportArray(Data2);
                }
                else {
                    jAlert("Sorry no record found!", "Message");
                    return false;
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.DownloadDebitReport = function () {
        var DateFrom = $("#txtDateFrom").val();
        var DateTo = $("#txtDateTo").val();
        var ECFNo = $("#txtEcfNo").val();
        var Raiser = $("#txtRaiser").val();
        var Vendor = $("#txtVendor").val();
        var Glcode = $("#txtGL").val();
        var Location = $("#txtLoc").val();
        ko.utils.postJson(MUrlDet + "DownloadDebitReport?DateFrom=" + DateFrom + "&DateTo=" + DateTo + "&ECFNo=" + ECFNo + "&Raiser=" + Raiser + "&Vendor=" + Vendor + "&Glcode=" + Glcode + "&Location=" + Location);
    }

    this.ClearDebitReport = function () {
        $("#txtDateFrom").val("");
        $("#txtDateTo").val("");
        $("#txtEcfNo").val("");
        $("#txtRaiser").val("");
        $("#txtVendor").val("");
        $("#txtGL").val("");
        $("#txtLoc").val("");
        //$("#txtDateFrom").removeClass('valid').addClass('required');
        //$("#txtDateTo").removeClass('valid').addClass('required');

        //$("#txtDateFrom").removeAttr('disabled');
        //$("#txtDateTo").removeAttr('disabled');
        //$("#txtEcfNo").removeAttr('disabled');
        //$("#txtRaiser").removeAttr('disabled');
        //$("#txtVendor").removeAttr('disabled');
        //$("#txtGL").removeAttr('disabled');
        //$("#txtLoc").removeAttr('disabled');
        $("#gvReport").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.DebitReportArray([]);
    };

    this.DatatableDebit = function () {
      
        if ($("#gvReport > tbody > tr").length == self.DebitReportArray().length) {
            setTimeout(function () {
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
            }, 600);
        }
    };

    //Cenvat Report

    this.SearchCenvatReport = function () {
        var DateFrom = $("#txtDateFrom").val();
        var DateTo = $("#txtDateTo").val();
        var ECFNo = $("#txtEcfNo").val();

        if ($.trim(DateFrom) == "") {
            jAlert("Ensure Date From!", "Message");
            return false;
        }

        if ($.trim(DateTo) == "") {
            jAlert("Ensure Date To!", "Message");
            return false;
        }
        var Filter = {
            DateFrom: DateFrom,
            DateTo: DateTo,
            ECFNo: ECFNo
        };

        $.ajax({
            type: "post",
            async: false,
            url: MUrlDet + "GetCenvatReport",
            data: JSON.stringify(Filter),
            contentType: "application/json;",
            datatype: "json",
            success: function (response) {
                var Data1 = "", Data2 = "";
                //$("#gvReport").DataTable({
                //    "autoWidth": false,
                //    "destroy": true
                //}).destroy();
                //self.CenvatReportArray.removeAll();
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    $("#txtDateFrom").attr('disabled', 'disabled');
                    $("#txtDateTo").attr('disabled', 'disabled');
                    $("#txtEcfNo").attr('disabled', 'disabled');
                    self.CenvatReportArray(Data2);
                }
                else {
                    jAlert("Sorry no record found!", "Message");
                    return false;
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.DownloadCenvatReport = function () {
        var DateFrom = $("#txtDateFrom").val();
        var DateTo = $("#txtDateTo").val();
        var ECFNo = $("#txtEcfNo").val();
        ko.utils.postJson(MUrlDet + "DownloadCenvatReport?DateFrom=" + DateFrom + "&DateTo=" + DateTo + "&ECFNo=" + ECFNo);
    }

    this.ClearCenvatReport = function () {
        $("#txtDateFrom").val("");
        $("#txtDateTo").val("");
        $("#txtEcfNo").val("");
        $("#txtDateFrom").removeClass('valid').addClass('required');
        $("#txtDateTo").removeClass('valid').addClass('required');

        $("#txtDateFrom").removeAttr('disabled');
        $("#txtDateTo").removeAttr('disabled');
        $("#txtEcfNo").removeAttr('disabled');
        $("#gvReport").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.CenvatReportArray([]);
    };

    this.DatatableCenvat = function () {

        if ($("#gvReport > tbody > tr").length == self.CenvatReportArray().length) {
            setTimeout(function () {
                $("#gvReport").DataTable({
                    "bSort": false,
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
            }, 600);
        }
    };

    self.columnNames = ko.computed(function () {
        if (self.CenvatReportArray().length === 0) {
            return [];

        } else {
            var props = [];
            var obj = self.CenvatReportArray()[0];
            var i = 0;
            for (var name in obj) {
                if (i != 1)
                    props.push(name);
                i++;
            }

            return props;
        }
    });

    //QC Report

    this.SearchQCReport = function () {
        var DateFrom = $("#txtDateFrom").val();
        var DateTo = $("#txtDateTo").val();
        var ECFNo = $("#txtEcfNo").val();
        var EmpId = $("#hfEmployee").val();
        var SuppId = $("#hfSupplier").val();

        if ($.trim(DateFrom) == "") {
            jAlert("Ensure Date From!", "Message");
            return false;
        }

        if ($.trim(DateTo) == "") {
            jAlert("Ensure Date To!", "Message");
            return false;
        }
        var Filter = {
            DateFrom: DateFrom,
            DateTo: DateTo,
            ECFNo: ECFNo,
            EmpId: EmpId,
            SuppId: SuppId
        };

        $.ajax({
            type: "post",
            url: MUrlDet + "GetQCReport",
            data: JSON.stringify(Filter),
            contentType: "application/json;",
            datatype: "json",
            success: function (response) {
                var Data1 = "", Data2 = "";
                //$("#gvReport").DataTable({
                //    "autoWidth": false,
                //    "destroy": true
                //}).destroy();
                //self.CenvatReportArray.removeAll();
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                }
                
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    $("#txtDateFrom").attr('disabled', 'disabled');
                    $("#txtDateTo").attr('disabled', 'disabled');
                    $("#txtEcfNo").attr('disabled', 'disabled');
                    $("#txtEmployee").attr('disabled', 'disabled');
                    $("#txtSupplier").attr('disabled', 'disabled');
                    self.CenvatReportArray(Data2);
                }
                else {
                    jAlert("Sorry no record found!", "Message");
                    return false;
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.DownloadQCReport = function () {
        var DateFrom = $("#txtDateFrom").val();
        var DateTo = $("#txtDateTo").val();
        var ECFNo = $("#txtEcfNo").val();
        var EmpId = $("#hfEmployee").val();
        var SuppId = $("#hfSupplier").val();
        ko.utils.postJson(MUrlDet + "DownloadQCReport?DateFrom=" + DateFrom + "&DateTo=" + DateTo + "&ECFNo=" + ECFNo + "&EmpId=" + EmpId + "&SuppId=" + SuppId);
    }

    this.ClearQCReport = function () {
        $("#txtDateFrom").val("");
        $("#txtDateTo").val("");
        $("#txtEcfNo").val("");
        $("#hfEmployee").val("0");
        $("#hfSupplier").val("0");
        $("#txtEmployee").val("");
        $("#txtSupplier").val("");
        $("#txtDateFrom").removeClass('valid').addClass('required');
        $("#txtDateTo").removeClass('valid').addClass('required');

        $("#txtDateFrom").removeAttr('disabled');
        $("#txtDateTo").removeAttr('disabled');
        $("#txtEcfNo").removeAttr('disabled');
        $("#txtEmployee").removeAttr('disabled');
        $("#txtSupplier").removeAttr('disabled');
        $("#gvReport").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.CenvatReportArray([]);
    };

};

var mainViewModel = new SearchModel();
ko.applyBindings(mainViewModel);

$(document).ready(function () {

    var objDate = new Date();
    var Presentyear = objDate.getFullYear();
    $(".fsDatepicker").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
    });

    $("#txtDateFrom").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        maxDate: 'd',
        onSelect: function (selected) {
            $("#txtDateFrom").removeClass('required').addClass('valid');
            $("#txtDateTo").datepicker("option", "minDate", selected)
        }
    });

    $("#txtDateTo").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        maxDate: 'd',
        onSelect: function (selected) {
            var month = selected.split("/")[1];
            $("#txtDateTo").removeClass('required').addClass('valid');
            $("#txtDateFrom").datepicker("option", "maxDate", selected)
        }
    });

    $("#txtLiqDateFrom").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        maxDate: 'd',
        onSelect: function (selected) {
            //$("#txtLiqDateFrom").removeClass('required').addClass('valid');
            $("#txtLiqDateTo").datepicker("option", "minDate", selected)
        }
    });

    $("#txtLiqDateTo").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        maxDate: 'd',
        onSelect: function (selected) {
            //$("#txtLiqDateTo").removeClass('required').addClass('valid');
            $("#txtLiqDateFrom").datepicker("option", "maxDate", selected)
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

    $("#ddlType").change(function () {
        var Type = $.trim($("#ddlType").val());
        if (Type != "0") {
            $("#ddlType").addClass("valid");
            $("#ddlType").removeClass("required");
        }
        else {
            $("#ddlType").addClass("required");
            $("#ddlType").removeClass("valid");
        }
    });


    //Load Raiser Code Auto Complete
    $("#txtEmployee").keyup(function (e) {
        if (e.which != 13) {
            $("#hfEmployee").val("0");
        }

        $("#txtEmployee").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: MUrlHelper + "GetAutoReceiptEmployee",
                    data: "{ 'txt' : '" + $("#txtEmployee").val() + "'}",
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
                $("#hfEmployee").val(i.item.val);
                $("#txtEmployee").val(i.item.label);
            }
        });
    });

    $("#txtEmployee").focusout(function () {
        var hdfId = $("#hfEmployee").val();
        var _data = $("#txtEmployee").val();
        if (_data.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtEmployee").val("");
        }
    });

    //Load Vendor Auto Complete
    $("#txtSupplier").keyup(function (e) {
        if (e.which != 13) {
            $("#hfSupplier").val("0");
        }

        $("#txtSupplier").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: MUrlHelper + "GetAutoCompleteSupplierAll",
                    data: "{ 'txt' : '" + $("#txtSupplier").val() + "'}",
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
                $("#hfSupplier").val(i.item.val);
                $("#txtSupplier").val(i.item.label);
            }
        });
    });

    $("#txtSupplier").focusout(function () {
        var hdfId = $("#hfSupplier").val();
        var _data = $("#txtSupplier").val();
        if (_data.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtSupplier").val("");
        }
    });

});

function isEvent(evt) {
    return false;
}
