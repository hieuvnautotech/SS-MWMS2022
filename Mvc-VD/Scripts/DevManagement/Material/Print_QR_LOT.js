function update_pr(value, id) {
    //debugger;
    var giatri = value;
    $.ajax({
        url: '/DevManagement/update_gr_wmt?id=' + id + "&gr_qty=" + giatri,
        type: "get",
        dataType: "json",
        success: function (data) {
            if (data.result == true) {
                //var id = data.kq.wmtid;
                var rowData = $('#list3').jqGrid('getRowData', id);
                rowData.gr_qty = giatri;
                $("#list3").setRowData(id, rowData, { background: "#d0e9c6" });

            } else {
                alert(data.kq);
            }
        },
        error: function (data) {
            alert('erro');
        }
    });
}


$(function () {
    $(".Print_LOT").dialog({
        width: '100%',
        height: 680,
        maxWidth: 1000,
        maxHeight: 800,
        minWidth: '60%',
        minHeight: 450,
        resizable: false,
        fluid: true,
        modal: true,
        autoOpen: false,
        classes: {
            "ui-dialog": "ui-dialog",
            "ui-dialog-titlebar": "ui-dialog ui-dialog-titlebar-sm",
            "ui-dialog-titlebar-close": "visibility: hidden",
        },
        resize: function (event, ui) {
            $('.ui-dialog-content').addClass('m-0 p-0');
        },
        open: function (event, ui) {
        },
    });

    $("#lot_expore_dt").datepicker({ dateFormat: 'yy-mm-dd' }).val();
    $("#lot_expore_dt").datepicker("setDate", new Date());

    $("#lot_dt_of_receipt").datepicker({ dateFormat: 'yy-mm-dd' }).val();
    $("#c_exp_input_dt").datepicker({ dateFormat: 'yy-mm-dd' }).val();

    $("#lot_month").on("keyup", function (e) {
        var value = e.target.value.trim();
        var date = lot_expore_dt.value.trim();
        if (date == "" || date == null) {
            alert('Vui lòng chọn Export Date');
            $("#lot_month").val("");
            return false;
        }
        value = (value == "" ? "0" : value)
        $.get("/DevManagement/add_month?month=" + value + "&date=" + date, function (data) {
            $("#c_exp_input_dt").val(data);
        });
    });

    $("#print_qr").click(function () {

        var rowData = $('#list').jqGrid("getGridParam", "selrow");
        if (rowData != null) {
            $('.Print_LOT').dialog('open');
            jQuery("#list3").jqGrid("clearGridData");
        }
        else {
            ErrorAlert("Please Choose The Material");
        }

        //$("#list3").setGridParam({ url: "/DevManagement/getdatalist3_Supplier_QR_Management?" + "&mt_no=" + $("#m_mt_no").val(), datatype: "json" }).trigger("reloadGrid");
    });

    //function Save_pr(cellValue, options, rowdata, action) {

    //    var id = rowdata.wmtid;
    //    html = '<button  class="btn btn-sm btn-primary button-srh" data-id="' + id + '"onclick="update_pr(this);">Save</button>';
    //    return html;
    //}

    function date(cellValue, options, rowdata, action) {
        var html = cellValue;
        if (cellValue != null && cellValue != "" && cellValue.toString().indexOf('-') == -1) {
            html = cellValue.substr(0, 4) + '-' + cellValue.substr(4, 2) + '-' + cellValue.substr(6, 2);
        }
        return html;
    }

    $("#list3").jqGrid
        ({
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { label: 'ID', name: 'wmtid', key: true, align: 'center', hidden: true },
                { label: 'ML No', name: 'mt_cd', key: true, sortable: true, align: 'left', width: 280, },
                { label: 'Lot No', name: 'lot_no', sortable: true, align: 'left', width: 100, },
                {
                    key: false, label: 'Length/EA ', name: 'gr_qty', width: 200, align: 'right', editable: false, editoptions: {
                        dataInit: function (element) {
                            $(element).keydown(function (e) {
                                if (e.which == 13) {
                                    var value = this.value;
                                    var id = e.target.id.replace("_gr_qty", "");
                                    var row = $("#list3").jqGrid('getRowData', id);
                                    update_pr(value, id);
                                }
                            });
                        }
                    },
                },


                //{
                //    label: 'Length/EA', name: 'gr_qty', width: 81, align: 'right'
                //    , editable: true, edittype: "text", editoptions: { size: 10, maxlength: 15 }, editrules: { integer: true, required: true }, editoptions: {
                //        dataInit: function (element) {
                //            $(element).keydown(function (e) {
                //                if (e.which == 13) {
                //                    var value = this.value;
                //                    var id = e.target.id.replace("_gr_qty", "");
                //                    var row = $("#list3").jqGrid('getRowData', id);
                //                    update_pr(value, id);
                //                }
                //            });
                //        }
                //    },
                //},

                { label: 'Expiry Date', name: 'expiry_dt', sortable: true, align: 'center', width: 100, formatter: date },
                { label: 'Date of Receipt', name: 'dt_of_receipt', sortable: true, align: 'center', width: 100, formatter: date },
                { label: 'Expore Date', name: 'expore_dt', sortable: true, align: 'center', width: 100, formatter: date },
                //{ label: 'Save', name: '', width: 100, align: 'center', formatter: Save_pr },

            ],
            onSelectRow: function (rowid, selected, status, e) {
                debugger;
                var selectedRowId = $("#list3").jqGrid("getGridParam", 'selrow');
                row_id = $("#list3").getRowData(selectedRowId);
            },
            onCellSelect: function (rowid) {
                var lastSel = "";
                if (rowid && rowid !== lastSel) {
                    jQuery('#list3').restoreRow(lastSel);
                    lastSel = rowid;
                }
                jQuery('#list3').editRow(rowid, true);
            },
            pager: jQuery('#detailpager'),
            viewrecords: true,
            rowList: [200, 500, 1000, 2000],
            rowNum: 1000,
            height: 400,
            width: null,
            autowidth: false,
            caption: '',

            loadtext: "Loading...",
            emptyrecords: "No data.",
            loadonce: true,
            rownumbers: true,
            gridview: true,
            shrinkToFit: false,
            multiselect: true,
            jsonReader:
            {
                root: "rows",
                page: "page",
                total: "total",
                records: "records",
                repeatitems: false,
                Id: "0"
            },


        });
    $("#c2_save_but").click(function () {

//debugger;
        if ($("#form_lot").valid() == true) {
            $(".thongbao").addClass("active");
            $(".thongbao").removeClass("hidden");
            $("#noidung").html("Chờ .....");
            var rowID = $('#list').jqGrid("getGridParam", "selrow");
            var rowData = $('#list').jqGrid('getRowData', rowID);
            let mt_type = rowData.mt_type;
            $.ajax({
                url: "/DevManagement/pl_insertw_material",
                type: "POST",
                dataType: "json",
                data: {
                    mt_no: $("#lot_mt_no").val(),
                    gr_qty: (document.getElementById('m_spec').value == "") ? 0 : document.getElementById('m_spec').value,
                    expore_dt: $("#lot_expore_dt").val(),
                    dt_of_receipt: $("#lot_dt_of_receipt").val(),
                    exp_input_dt: $("#c_exp_input_dt").val(),
                    lot_no: $("#lot_lot_no").val(),
                    send_qty: $('#c_send_qty').val(),
                    bundle_qty: (document.getElementById('m_bundle_qty').value == "") ? 0 : document.getElementById('m_bundle_qty').value,
                    bundle_unit: $('#m_bundle_unit').val(),
                    mt_type: mt_type,
                    barcode: $('#m_barcode').val(),
                },
                success: function (data) {

                    if (data.result) {

                        $("#list3").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: data.data, background: "#d0e9c6" }).trigger("reloadGrid");
                        SuccessAlert(data.message);
                        $(".thongbao").addClass("hidden");
                        $(".thongbao").removeClass("active");
                        $("#change_legth").attr("disabled", false);
                        $("#Print_bar").attr("disabled", false);

                        return;

                    } else {
                        $(".thongbao").addClass("hidden");
                        $(".thongbao").removeClass("active");
                        ErrorAlert(data.kq);
                    }
                },
                error: function (result) {
                    alert('Material No is not existing. Please check again');
                }
            });
        }
    });


    $("#Print_bar").click(function () {
        var fruits = [];
        var i, selRowIds = $('#list3').jqGrid("getGridParam", "selarrrow"), n, rowData;
        for (i = 0, n = selRowIds.length; i < n; i++) {
            var ret = $("#list3").jqGrid('getRowData', selRowIds[i]);
            var wmtid = ret.wmtid
            var wmtid = ret.wmtid
            fruits.push(wmtid);
        };
        if (selRowIds.length > 0) {
            var mt_nm = $("#m_mt_nm").val();
            var mapForm = document.createElement("form");
            mapForm.target = "Map";
            mapForm.type = "hidden";
            mapForm.method = "POST"; // or "post" if appropriate
            mapForm.action = "/DevManagement/PrintQRSupplier";

            var mapInput = document.createElement("input");
            mapInput.type = "hidden";
            mapInput.name = "id";
            mapInput.value = fruits;
            //0
            mapForm.appendChild(mapInput);


            var width = document.createElement("input");
            width.type = "hidden";
            width.name = "width";
            width.value = $("#m_width").val();

            //1
            mapForm.appendChild(width);

            var bundle_qty = document.createElement("input");
            bundle_qty.type = "hidden";
            bundle_qty.name = "bundle_qty";
            bundle_qty.value = $("#m_bundle_qty").val();
            //2
            mapForm.appendChild(bundle_qty);

            var spec = document.createElement("input");
            spec.type = "hidden";
            spec.name = "spec";
            spec.value = $("#m_spec").val();
            //3
            mapForm.appendChild(spec);

            var bundle_unit = document.createElement("input");
            bundle_unit.type = "hidden";
            bundle_unit.name = "bundle_unit";
            bundle_unit.value = $("#m_bundle_unit").val();
            //4
            mapForm.appendChild(bundle_unit);


            var id_mt = document.createElement("input");
            id_mt.type = "hidden";
            id_mt.name = "id_mt";
            id_mt.value = $("#m_mtid").val();
            mapForm.appendChild(id_mt);
            //5
            document.body.appendChild(mapForm);

            map = window.open(" ", "Map", "width=500, height=800, left=0, top=100, location=no, status=no,");

            if (map) {
                mapForm.submit();
            }
            $("#Print_bar").attr("disabled", true);
            $("#change_legth").attr("disabled", true);
            return;

            //window.open("/DevManagement/PrintQRSupplier?" + "id=" + fruits
            //    + "&width=" + $("#m_width").val() + "&bundle_qty=" + $("#m_bundle_qty").val()
            //    + "&spec=" + $("#m_spec").val()
            //    + "&bundle_unit=" + $("#m_bundle_unit").val() + "&id_mt=" + $("#m_mtid").val()
            //    , "PRINT", "width=600px, height=800, left=10, top=100, location=no, status=no,")
        }
        else {
            alert("Vui Lòng chọn Mã Cần In!!.");
            return;
        }
    });


    $("#change_legth").click(function () {
        debugger;
        $.blockUI({ message: '<h3>Loading...</h3>' });
        var lot_mt_no = $("#lot_mt_no").val();
        var i, selRowIds = $('#list3').jqGrid("getGridParam", "selarrrow"), n, rowData;
        if (selRowIds.length == 0) {
            alert("Vui lòng chọn cuộn bất kì để tiếp tục");
            $.unblockUI();
            return false;
        }
        var grQty = $("#LEG_EA").val().trim();
        if (grQty == undefined || grQty == "") {
            alert("Vui lòng nhập số lượng");
            $.unblockUI();
            return false;
        }
        object = {};
        object['listId'] = selRowIds;
        object['gr_qty'] = $("#LEG_EA").val().trim();
        object['lot_mt_no'] = lot_mt_no;
        $.ajax({
           /* url: '/DevManagement/update_gr_wmt_select?id=' + selRowIds + "&gr_qty=" + $("#LEG_EA").val().trim() + "&lot_mt_no=" + lot_mt_no,*/
            url: '/DevManagement/update_gr_wmt_select',
            dataType: "json",
            type: 'POST',
            async: true,
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(object),
            traditonal: true,
            success: function (data) {
                if (data.result == true) {
                    var grty = parseFloat($("#LEG_EA").val().trim());
                    $.each(selRowIds, function (key, item) {

                        var rowData = $('#list3').jqGrid('getRowData', item);
                        rowData.gr_qty = grty;
                        $("#list3").setRowData(item, rowData, { background: "#d0e9c6" });

                    });
                    $.unblockUI();
                    SuccessAlert(data.message);
                } else {
                    $.unblockUI();
                    ErrorAlert(data.message);
                }


            },
            error: function (data) {
                alert('Error');
                $.unblockUI();
            }
        });

    });


    $("#form_lot").validate({
        rules: {
            "mt_no": {
                required: true,

            },
            "expore_dt": {
                required: true,

            },
            "exp_input_dt": {
                required: true,

            },
            "lot_no": {
                required: true,
            },
            "send_qty": {
                required: true,
                number: true,
            },
        },
    });

});

