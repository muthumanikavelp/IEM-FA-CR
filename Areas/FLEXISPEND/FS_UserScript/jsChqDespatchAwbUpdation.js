
var objDialogyAdd;
UrlDet = UrlDet.replace("GetChequeDespatchAwbUpdation", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");

var ChequeInventoryModel = function () {
    var self = this;

    //grid array
    self.ChqDespatchAwbUpdArray = ko.observableArray();
    self.ChqDespatchAwbXLArray = ko.observableArray();

    self.formatNumber = function (num) {
        //return Number(num).toFixed(2);
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
        var data = {
            ChqDate: $("#txtChqDate").val(),
            PVNo: $("#txtPVNo").val(),
            BatchNo: $("#txtPayBatchNo").val(),
            PayDate: $("#txtPayDate").val(),
            CourierName: $("#hdfCourierId1").val(),
            Location: $("#hdfBranchId").val(),
            RaiserBranch: $("#hdfRaiserBranchId").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetChequeDespatchAwbUpdation",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {

                var Data1 = "", Data2 = "";
                
                if (response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    self.loadGrid()
                    Data2 = JSON.parse(response.Data2);
                    //$('#btnSaveCancel').attr('disabled', false);
                }
                self.ChqDespatchAwbUpdArray(Data2);

                if (response.Data1 != "" && JSON.parse(response.Data1) != null) {

                    Data1 = JSON.parse(response.Data1);
                    //show Message if error
                    if (Data1[0].Message != "") {

                        jAlert(Data1[0].Message, "Message");
                    }

                    if (Data1[0].Message == "" && self.ChqDespatchAwbUpdArray().length == 0) {
                        jAlert("No Records found!", "Message");
                    }
                    else
                        $('#btnExporExcel').attr('disabled', false);
                }
                //$('#lblTotalCount').text(self.ChqDespatchAwbUpdArray().length);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.searchDefaultvalue = function () {
        var count = $("#txtDespatchDate").val();
        if (count == "" || count == "0") {
            jAlert("Ensure DespatchDate!", "Message");
            return false;
        }

        count = $("#hdfCourierId").val();
        if (count == "" || count == "0") {
            jAlert("Ensure CourierName!", "Message");
            return false;
        }

        count = $("#txtAWBNo").val();
        if (count == "" || count == "0") {
            jAlert("Ensure AWBNo!", "Message");
            return false;
        }
        var data = {
            ChqDate: $("#txtChqDate").val(),
            PVNo: $("#txtPVNo").val(),
            BatchNo: $("#txtPayBatchNo").val(),
            PayDate: $("#txtPayDate").val(),
            CourierName: $("#hdfCourierId1").val(),
            Location: $("#hdfBranchId").val(),
            RaiserBranch: $("#hdfRaiserBranchId").val()
        };

        $.ajax({
            type: "post",
            url: UrlDet + "GetChequeDespatchAwbUpdation",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {

                var Data1 = "", Data2 = "";
                if (response.Data1 != "" && JSON.parse(response.Data1) != null) {

                    Data1 = JSON.parse(response.Data1);
                    //show Message if error
                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");
                    }
                }

                if (response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    self.loadGrid()
                    $('#btnExporExcel').removeAttr('disabled');
                    $('#btnSaveUpdate').attr('disabled', false);

                    $('#btnDefaultvalue').attr('disabled', true);
                    $('#txtDespatchDate').attr('disabled', true);
                    $('#txtCourierName').attr('disabled', true);
                    $('#txtAWBNo').attr('disabled', true);
                    Data2 = JSON.parse(response.Data2);

                    for (var i = 0; i < Data2.length ; i++) {
                        Data2[i].AwbNo = $("#txtAWBNo").val();
                        Data2[i].DespatchDate = $("#txtDespatchDate").val();
                        Data2[i].CourierName = $("#txtCourierName").val();
                    }
                    self.ChqDespatchAwbUpdArray(Data2);


                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };


    self.checkChange = function (obj, event, innerdetaills) {
        if (event.originalEvent) {
            var array = new Array();
            $('#contentChequePrinting input:checkbox:checked').each(function (index) {
                array[index] = $(this).attr('id');
            });
            $("#lblTotalCount").text(array.length);
        }
    };

    self.Update = function () {
        var array = new Array();
        $('#gvChequeDespathAWBUpdation > tbody input:checkbox:checked').each(function (index) {
            array[index] = $(this).attr('id');
        });

        var _ListParam = "";
        $.each(array, function (index) {
            _ListParam += array[index].split('_')[1] + '|';
        });

        if (_ListParam == "") {
            jAlert("Please select a records to process.", "Message");
            return false;
        }
        var count = $("#txtDespatchDate").val();
        if (count == "" || count == "0") {
            jAlert("Ensure DespatchDate!", "Message");
            return false;
        }

        count = $("#hdfCourierId").val();
        if (count == "" || count == "0") {
            jAlert("Ensure CourierName!", "Message");
            return false;
        }

        count = $("#txtAWBNo").val();
        if (count == "" || count == "0") {
            jAlert("Ensure AWBNo!", "Message");
            return false;
        }
        count = $("#lblTotalCount").text();
        if (count == "" || count == "0") {
            jAlert("Please select a record!", "Message");
            return false;
        }
       
        var data = {
            DespatchDate: $("#txtDespatchDate").val(),
            PvNos: _ListParam,
            CourierId: $("#hdfCourierId").val(),
            AwbNo: $("#txtAWBNo").val(),
            ViewType: "0"
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetChequeDespatchAwbUpdation",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");
                    }

                    if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {

                        var data1 = {
                            ChqDate: $("#txtChqDate").val(),
                            PVNo: $("#txtPVNo").val(),
                            BatchNo: $("#txtPayBatchNo").val(),
                            PayDate: $("#txtPayDate").val(),
                            CourierName: $("#hdfCourierId1").val(),
                            Location: $("#hdfBranchId").val(),
                            RaiserBranch: $("#hdfRaiserBranchId").val()
                        };

                        $.ajax({
                            type: "post",
                            url: UrlDet + "GetChequeDespatchAwbUpdation",
                            data: JSON.stringify(data1),
                            contentType: "application/json;",
                            success: function (response) {

                                var Data1 = "", Data2 = "";
                                if (response.Data2 != "" && JSON.parse(response.Data2) != null) {
                                    self.loadGrid()
                                    Data2 = JSON.parse(response.Data2);
                                    self.ChqDespatchAwbUpdArray(Data2);
                                }
                                //$('#lblTotalCount').text(self.ChqDespatchAwbUpdArray().length);
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                //alert(errorThrown);
                            }
                        });
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

    self.Cancel = function () {
        var array = new Array();
        $('#gvChequeDespathAWBUpdation input:checkbox:checked').each(function (index) {
            array[index] = $(this).attr('id');
        });

        var _ListParam = "";
        $.each(array, function (index) {
            _ListParam += array[index].split('_')[1] + '|';
        });

        if (_ListParam == "") {
            jAlert("Please select a records to process.", "Message");
            return false;
        }

        count = $("#lblTotalCount").text();
        if (count == "" || count == "0") {
            jAlert("Please select a record!", "Message");
            return false;
        }

        var data = {
            DespatchDate: $("#txtDespatchDate").val(),
            PvNos: _ListParam,
            CourierId: $("#hdfCourierId").val(),
            AwbNo: $("#txtAWBNo").val(),
            ViewType: "2"
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetChequeDespatchAwbUpdation",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");
                    }

                    if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {

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

    // Cancel Cheque despatch awb updation
    self.ClearSearch = function () {
        $("#txtChqDate").val("");
        $("#txtPVNo").val("");
        $("#txtPayBatchNo").val("");
        $("#txtBranch").val("");
        $("#txtRaiserBranch").val("");
        $("#hdfBranchId").val("0");
        $("#hdfRaiserBranchId").val("0");
        $("#txtPayDate").val("");
        $("#hdfCourierId1").val("0");
        $("#txtCourierName1").val("");
        $("#txtDespatchDate").val("");
        $("#txtAWBNo").val("");
        $("#txtCourierName").val("");
        $("#hdfCourierId").val("");
        self.loadGrid();

        $("#txtDespatchDate").removeClass('required').removeClass('valid');
        $("#txtDespatchDate").addClass('required');

        $("#txtCourierName").removeClass('required').removeClass('valid');
        $("#txtCourierName").addClass('required');

        $("#txtAWBNo").removeClass('required').removeClass('valid');
        $("#txtAWBNo").addClass('required');

        $('#btnDefaultvalue').attr('disabled', false);
        $('#txtDespatchDate').attr('disabled', false);
        $('#txtCourierName').attr('disabled', false);
        $('#txtAWBNo').attr('disabled', false);


        //$('#btnExporExcel').attr('disabled', 'disabled');
        //$('#btnSaveUpdate').attr('disabled', true);

        //$('#btnSaveCancel').attr('disabled', true);
    }
    self.ClearSearch1 = function () {
        $("#txtDespatchDate").val("");
        $("#txtAWBNo").val("");
        $("#txtCourierName").val("");
        $("#hdfCourierId").val("");
        //self.loadGrid();

        $("#txtDespatchDate").removeClass('required').removeClass('valid');
        $("#txtDespatchDate").addClass('required');

        $("#txtCourierName").removeClass('required').removeClass('valid');
        $("#txtCourierName").addClass('required');

        $("#txtAWBNo").removeClass('required').removeClass('valid');
        $("#txtAWBNo").addClass('required');

        $('#btnDefaultvalue').attr('disabled', false);
        $('#txtDespatchDate').attr('disabled', false);
        $('#txtCourierName').attr('disabled', false);
        $('#txtAWBNo').attr('disabled', false);

        //$('#btnExporExcel').attr('disabled', 'disabled');
        //$('#btnSaveUpdate').attr('disabled', true);
        //$('#btnSaveCancel').attr('disabled', true);

    }


    this.XlUpdate = function (Id) {
        self.loadGridExcel();
        $('#AddShowDialog').attr("style", "display:block;");
        objDialogyAdd.dialog({ title: 'Excel Bulk Update', width: '800', height: '400' });
        objDialogyAdd.dialog("open");
    };



    self.Xlupload = function () {
        var count = $("#attUploader").val();
        if (count == "" || count == "0") {
            jAlert("Ensure FileName!", "Message");
            return false;
        }
        $("#btnSubmitUpload").attr("disabled", true);
        var data = {
            DespatchDate: "", // $("#txtDespatchDate").val(),
            PvNos: "", //_ListParam,
            CourierId: "",// $("#hdfCourierId").val(),
            AwbNo: "",//$("#txtAWBNo").val(),
            ViewType: "1"
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetChequeDespatchAwbUpdation",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                $("#attUploader").val("");
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    var control = $("#attUploader"),
                             clearBn = $("#clear");
                    control.replaceWith(control.val('').clone(true));
                    $("#btnSubmitUpload").attr("disabled", false);
                    if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {
                        
                    }
                    else {
                        $("#attUploader").val("");
                        self.loadGridExcel();
                    }

                    //show Message if error
                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");
                        $("#attUploader").val("");
                        var control = $("#attUploader"),
                             clearBn = $("#clear");
                        control.replaceWith(control.val('').clone(true));
                        $("#btnSubmitUpload").attr("disabled", false);
                    }
                } else {
                    jAlert("unable to process your request. please try again!", "Message");
                    $("#btnSubmitUpload").attr("disabled", false);
                }

                    if (response.Data2 != "" && JSON.parse(response.Data2) != null) {
                        Data2 = JSON.parse(response.Data2);
                        self.loadGridExcel();
                        self.ChqDespatchAwbXLArray(Data2);
                        $("#attUploader").val("");
                        var control = $("#attUploader"),
                              clearBn = $("#clear");
                        control.replaceWith(control.val('').clone(true));
                        $("#btnSubmitUpload").attr("disabled", false);
                    }
                

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.ClearSearchExcel = function () {
        $("#attUploader").val("");
        self.loadGridExcel();
        $("#attUploader").val('');
        $("#btnSubmitUpload").attr("disabled", false);
        var name = $("#attUploader").val();
        var control = $("#attUploader"),
            clearBn = $("#clear");
        control.replaceWith(control.val('').clone(true));
    }

    self.DatatableCallExcel = function () {
        if ($("#gvXLBulkUpdate > tbody > tr").length == self.ChqDespatchAwbXLArray().length) {
            $("#gvXLBulkUpdate").DataTable({
                "autoWidth": false,
                "destroy": true,
                "bFilter": false,
                "bLengthChange": false,
                "bPaginate": false,
                "bInfo": false,
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

    self.loadGridExcel = function () {
        $("#gvXLBulkUpdate").DataTable({
            "autoWidth": false,
            "destroy": true,
            "bFilter": false,
            "bLengthChange": false,
            "bPaginate": false,
            "bInfo": false
        }).destroy();
        self.ChqDespatchAwbXLArray.removeAll();
    }

    self.DatatableCall = function () {
        if ($("#gvChequeDespathAWBUpdation > tbody > tr").length == self.ChqDespatchAwbUpdArray().length) {
            $("#gvChequeDespathAWBUpdation").DataTable({
                "autoWidth": false,
                "destroy": true,
                "bFilter": false,
                "bLengthChange": false,
                "bPaginate": false,
                "bInfo": false,
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
        $("#gvChequeDespathAWBUpdation").DataTable({
            "autoWidth": false,
            "destroy": true,
            "bFilter": false,
            "bLengthChange": false,
            "bPaginate": false,
            "bInfo": false
        }).destroy();
        self.ChqDespatchAwbUpdArray.removeAll();
    }

    self.DownloadTemplate = function () {
        var ChqDate = $("#txtChqDate").val();
        var PVNo = $("#txtPVNo").val();
        var BatchNo = $("#txtPayBatchNo").val();
        var PayDate = $("#txtPayDate").val();
        var CourierName = $("#hdfCourierId1").val();
        var Location = $("#hdfBranchId").val();
        var RaiserBranch = $("#hdfRaiserBranchId").val();

        ko.utils.postJson(UrlDet + "DownloadChequeDespatchAwbUpdation?ChqDate=" + ChqDate + "&PVNo=" + PVNo + "&BatchNo=" + BatchNo + "&PayDate=" + PayDate + "&CourierName=" + CourierName + "&Location=" + Location + "&RaiserBranch=" + RaiserBranch);
    }


    self.loadGrid();
    self.loadGridExcel();
};

var mainViewModel = new ChequeInventoryModel();

ko.applyBindings(mainViewModel);


$(document).ready(function () {

    objDialogyAdd = $("[id$='ShowDialog']");
    objDialogyAdd.dialog({
        autoOpen: false,
        modal: true,
        width: 850,
        height: 500,
        duration: 300

    });
    var objDate = new Date();
    var Presentyear = objDate.getFullYear();
    $("#txtDespatchDate").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
    });
    $("#txtChqDate").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
    });

    $("#txtPayDate").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
    });

    //Load Branch Auto Complete
    $("#txtBranch").keyup(function (e) {
        //if (e.which != 13) {           
        //  $("#hdfBranchId").val("0");            
        //}

        $("#txtBranch").autocomplete({

            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteBranch",
                    data: "{ 'txt' : '" + $("#txtBranch").val() + "'}",
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
                $("#hdfBranchId").val(i.item.val);
                $("#txtBranch").val(i.item.label);
            }
        });
    });

    $("#txtRaiserBranch").keyup(function (e) {
        //if (e.which != 13) {           
        //  $("#hdfBranchId").val("0");            
        //}

        $("#txtRaiserBranch").autocomplete({

            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteBranch",
                    data: "{ 'txt' : '" + $("#txtRaiserBranch").val() + "'}",
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
                $("#hdfRaiserBranchId").val(i.item.val);
                $("#txtRaiserBranch").val(i.item.label);
            }
        });
    });


    $("#chkHSelete").click(function (e) {
        $(this).closest('table').find('td input:checkbox').prop('checked', this.checked);

        var array = new Array();
        $('#contentChequePrinting input:checkbox:checked').each(function (index) {
            array[index] = $(this).attr('id');
        });
        $("#lblTotalCount").text(array.length);
    });


    //Load Courier Auto Complete
    $("#txtCourierName").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfCourierId").val("0");
        }

        $("#txtCourierName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteCourier",
                    data: "{ 'txt' : '" + $("#txtCourierName").val() + "'}",
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
                $("#hdfCourierId").val(i.item.val);
                $("#txtCourierName").val(i.item.label);
            }
        });
    });

    $("#txtCourierName").focusout(function () {
        var hdfId = $("#hdfCourierId").val();
        var txtCurName = $("#txtCourierName").val();
        if ((txtCurName.trim() != "") && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtCourierName").val("");

            $("#txtCourierName").removeClass('valid');
            $("#txtCourierName").addClass('required');
        } else {
            $("#txtCourierName").removeClass('required');
            $("#txtCourierName").addClass('valid');
        }
    });


    $("#txtCourierName1").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfCourierId1").val("0");
        }

        $("#txtCourierName1").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteCourier",
                    data: "{ 'txt' : '" + $("#txtCourierName1").val() + "'}",
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
                $("#hdfCourierId1").val(i.item.val);
                $("#txtCourierName1").val(i.item.label);
            }
        });
    });

    $("#txtCourierName1").focusout(function () {
        var hdfId = $("#hdfCourierId1").val();
        var txtCurName = $("#txtCourierName1").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtCourierName1").val("");
        }
    });

    $("#txtDespatchDate").change(function () {

        var txtFrom = $("#txtDespatchDate").val();
        if (txtFrom.trim() != "") {
            $("#txtDespatchDate").removeClass('required');
            $("#txtDespatchDate").addClass('valid');
        }
        else {
            $("#txtDespatchDate").removeClass('valid');
            $("#txtDespatchDate").addClass('required');
        }
    });

    $("#txtAWBNo").keyup(function () {

        var txtFrom = $("#txtAWBNo").val();
        if (txtFrom.trim() != "") {
            $("#txtAWBNo").removeClass('required');
            $("#txtAWBNo").addClass('valid');
        }
        else {
            $("#txtAWBNo").removeClass('valid');
            $("#txtAWBNo").addClass('required');
        }
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
function fnExcelExport() {
    //alert('11');
    var tab_text = "<table border='2px'><tr bgcolor='#87AFC6'>";
    var textrange; var j = 0;
    tab = document.getElementById('gvChequeDespathAWBUpdationExcel');

    for (j = 0; j < tab.rows.length; j++) {
        tab_text = tab_text + tab.rows[j].innerHTML + "</tr>";
    }
    tab_text = tab_text + "</table>";
    tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");
    tab_text = tab_text.replace(/<img[^>]*>/gi, "");
    tab_text = tab_text.replace(/input[^>]*>|<\/input>/gi, "");

    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");

    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {  //for IE Browser
        txtArea1.document.open("txt/html", "replace");
        txtArea1.document.write(tab_text);
        txtArea1.document.close();
        txtArea1.focus();
        sa = txtArea1.document.execCommand("SaveAs", true, "AWB Posting.xls");
    }
    else {   //Other Browser
        var link = document.createElement("a");
        link.download = "AWB Posting.xls";
        link.href = 'data:application/vnd.ms-excel,' + encodeURIComponent($('#gvChequeDespathAWBUpdationExcel').html())
        link.click();
        // sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text));

    }
    return false;
}

