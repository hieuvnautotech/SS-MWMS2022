$(function () {
    $("#list").jqGrid
        ({
            url: "/HieuShippingMgt/GetPickingScan",
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'sid', name: 'sid', width: 50, align: 'center', hidden: true },
                //{ label: 'SD NO', name: 'sd_no', width: 100, align: 'center', formatter: SD_popup },
                { label: 'SD NO', name: 'sd_no', width: 100, align: 'left', hidden: true },
                { label: 'SD Name', name: 'sd_nm', width: 300, align: 'left' },
                { label: 'Product', name: 'product_cd', width: 150, align: 'left' },
                { label: 'Status', name: 'sts_nm', width: 150, align: 'center' },
                { label: 'Status', name: 'sd_sts_cd', width: 150, align: 'center', hidden: true },
                { label: 'Location', name: 'lct_nm', width: 150, align: 'left', hidden: true },
                { label: 'Location', name: 'lct_cd', width: 150, align: 'left', hidden: true },
                { label: 'alert', name: 'alert', width: 180, align: 'left', hidden: true },
                { label: 'Remark', name: 'remark', width: 180, align: 'left' },

            ],
            onSelectRow: function (rowid, selected, status, e) {
                var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
                var rowData = $("#list").getRowData(selectedRowId);



                $("#m_sid").val(rowData.sid);
                $("#m_sd_nm").val(rowData.sd_nm);
                $("#m_sd_no").val(rowData.sd_no);
                $("#m_style_no").val(rowData.product_cd);
                $("#m_remark").val(rowData.remark);

                $("#tab_1").removeClass("active");
                $("#tab_2").addClass("active");
                $("#tab_c1").removeClass("show");
                $("#tab_c1").removeClass("active");
                $("#tab_c2").addClass("active");

                $("#m_save_but").attr("disabled", false);
                $("#del_save_but").attr("disabled", false);


                $("#list2").setGridParam({ url: "/ShippingMgt/Getshippingsdmaterial?sd_no=" + rowData.sd_no, datatype: "json" }).trigger("reloadGrid");


            },

            pager: jQuery('#listPager'),
            rowNum: 50,
            rowList: [50, 100, 200, 500, 1000],
            loadonce: true, //tải lại dữ liệu
            viewrecords: true,
            rownumbers: true,
            hoverrows: false,
            caption: 'Picking Scan',
            emptyrecords: "No data.",
            height: 200,
            width: null,
            autowidth: false,
            shrinkToFit: false,
            jsonReader:
            {
                root: "rows",
                page: "page",
                total: "total",
                records: "records",
                repeatitems: false,
                Id: "0"
            },
            gridComplete: function () {

                var id_no = $("#id_no").val();
                if (id_no != "") {
                    $("#list").jqGrid('setSelection', id_no, { background: "#28a745 !important", color: "#fff" });
                }
                var rows = $("#list").getDataIDs();

                for (var i = 0; i < rows.length; i++) {
                    var sd_sts_cd = $("#list").getCell(rows[i], "sd_sts_cd");
                    var alert = $("#list").getCell(rows[i], "alert");

                    if (alert == "2") {
                        $("#list").jqGrid('setCell', rows[i], "sts_nm", "", {
                            'background-color': 'rgb(244,177,131) ',
                        });
                    } else if (alert == "0") {
                        $("#list").jqGrid('setCell', rows[i], "sts_nm", "", {
                            'background-color': 'rgb(164, 204, 145)',
                        });
                    }
                    if (sd_sts_cd == "001") {
                        $("#list").jqGrid('setCell', rows[i], "sts_nm", "", {
                            background: 'rgb(164,204,145)',
                        });
                    }

                }

            }
        });
});

