UrlDet = UrlDet.replace("SetProvisionUpload", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");
var ProvisionModel = function () {
    var self = this;

    //grid array
    self.ProvisionMappingArray = ko.observableArray();

    self.BusinessSegmentArray = ko.observableArray();
    self.CostCenterArray = ko.observableArray();

    self.formatNumber = function (num) {
        return Number(num).toFixed(2);
    };

    self.LoadBusinessSegment = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetBusinessSegment",
            data: "{}",
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.BusinessSegmentArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.LoadCostCenter = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetCostCenter",
            data: "{}",
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.CostCenterArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.upload = function () {
        var _d  = $("#attUploader").val();
        if (_d == "" || _d == "0") {
            jAlert("Ensure FileName!", "Message");
            return false;
        }

        $.ajax({
            type: "post",
            url: UrlDet + "SetProvisionUpload",
            data: "{}",
            contentType: "application/json;",
            async: false,
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);

                if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {
                    $("#attUploader").val("");
                    $('#uploaderForm')[0].reset();
                }

                //show Message if error
                if (Data1[0].Msg != "") {
                    jAlert(Data1[0].Msg, "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.Clear = function () {
        $("#attUploader").val("");
        $('#uploaderForm')[0].reset();
    }

    self.Download = function () {
        window.location = "/Template/Provision Mapping.xlsx";
    };

    self.Search = function () {
        var data = {
            PDateFrom: $("#txtProvisionFrom").val(),
            PDateTo: $("#txtProvisionTo").val(),
            PAmount: $("#txtProvisionAmount").val().replace(/,/g, ''),
            CCCode: $("#ddlCC").val(),
            FCCode: $("#ddlFC").val(),
            FC: $("#txtBusiness").val(),
            CC: $("#txtUnit").val(),
            ProvisionBy: $("#txtProvisionBy").val()
        };
        
        self.loadGrid();
        $.ajax({
            type: "post",
            url: UrlDet + "SearchProvisionMapping",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
        
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);

                    self.ProvisionMappingArray(Data2);
                }

                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Msg != "") {
                        jAlert(Data1[0].Msg, "Message");
                    } else if (self.ProvisionMappingArray().length == 0) {
                        jAlert("Sorry no records found!", "Message");
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {

            }
        });
    };

    self.ClearFilter = function () {
        $("#txtProvisionFrom").val("");
        $("#txtProvisionTo").val("");
        $("#txtProvisionAmount").val("");
        $("#txtProvisionBy").val("");

        $("#ddlCC").val(0);
        $("#ddlFC").val(0);

        $("#hdfBusiness").val("0");
        $("#txtBusiness").val("");

        $("#hdfUnit").val("0");
        $("#txtUnit").val("");

        self.loadGrid();
    };

    //Datatable Plugin codeing
    self.DatatableCall = function () {
        if ($("#gvMapping > tbody > tr").length == self.ProvisionMappingArray().length) {
            $("#gvMapping").DataTable({
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

    self.loadGrid = function () {
        $("#gvMapping").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.ProvisionMappingArray.removeAll();
    }
    self.loadGrid();

    self.LoadBusinessSegment();
    self.LoadCostCenter();
};

var mainViewModel = new ProvisionModel();
ko.applyBindings(mainViewModel);

$(document).ready(function () { 
    $(".fsDatepicker").datepicker({
        yearRange: '1900:+nn',
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
    });

    $(".fsDatepicker").keyup(function (e) {
        if (e.keyCode == 8 || e.keyCode == 46) {
            $.datepicker._clearDate(this);
        }
        $(this).datepicker('show');
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

    //Unit Auto Complete
    $("#txtUnit").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfUnit").val("0");
        }

        $("#txtUnit").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteCCUnit",
                    data: "{ 'txt' : '" + $("#txtUnit").val() + "'}",
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
                $("#hdfUnit").val(i.item.val);
                $("#txtUnit").val(i.item.label);
            }
        });
    });

    $("#txtUnit").focusout(function () {
        var hdfId = $("#hdfUnit").val();
        var _data = $("#txtUnit").val();
        if (_data.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtUnit").val("");
        }
    });

    //FC Business Auto Complete
    $("#txtBusiness").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfBusiness").val("0");
        }

        $("#txtBusiness").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteFCBusiness",
                    data: "{ 'txt' : '" + $("#txtBusiness").val() + "'}",
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
                $("#hdfBusiness").val(i.item.val);
                $("#txtBusiness").val(i.item.label);
            }
        });
    });

    $("#txtBusiness").focusout(function () {
        var hdfId = $("#hdfBusiness").val();
        var _data = $("#txtBusiness").val();
        if (_data.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtBusiness").val("");
        }
    });

    $("#txtProvisionAmount").keyup(function (event) {
        if (event.which >= 37 && event.which <= 40) {
            event.preventDefault();
        }

        var currentval = $(this).val();
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
        $(this).val(components.join("."));
    });

    $(".attUploader").on('change', function (e) {
        var Attach_list = Attachment_list();
        var Attachment_fomat = Attached_fileformat();  //Pandiaraj 18-11-2019 
        var iframe = $('<iframe name="postiframe" id="postiframe" style="display: none"></iframe>');
        $("body").append(iframe);
        var form = $('#uploaderForm');
        //form.attr("action", UrlDet + "UploadAttachment");
        form.attr("action", UrlDet + "UploadAttachment/?attach=" + Attach_list + '&attaching_format=' + Attachment_fomat);  //Pandiaraj 18-11-2019 
        form.attr("method", "post");
        form.attr("encoding", "multipart/form-data");
        form.attr("enctype", "multipart/form-data");
        form.attr("target", "postiframe");
        form.attr("file", $('#attUploader').val());
        form.submit();
        return false;
    });
});

function isEvent(evt) {
    return false;
}
