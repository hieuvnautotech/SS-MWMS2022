$(function () {
    $("#list").jqGrid
        ({
            //url: "/HieuShippingMgt/GetPickingScan",
            datatype: function (postData) { getData_primary(postData); },
          
            mtype: 'Get',
            colModel: [
                { key: true, label: 'sid', name: 'sid', width: 50, align: 'center', hidden: true },
                { label: 'SD NO', name: 'sd_no', width: 100, align: 'center', formatter: SD_popup },
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
            viewrecords: true,
            rowList: [50, 100, 200, 500, 1000],
            height: 200,
            width: $(".box-body").width() - 5,
            autowidth: false,
            rowNum: 50,
            caption: 'Picking Scan',
            loadtext: "Loading...",
            emptyrecords: "No data.",
            rownumbers: true,
            gridview: true,
            loadonce: false, //tải lại dữ liệu, false thì cho nhấn nút phân trang
            shrinkToFit: false,
            multiselect: false,
            subGrid: false,
            
            
            
            
            
            
            //hoverrows: false,
            
           
            
            
            
            
            jsonReader:
            {
                root: "rows",
                page: "page",
                total: "total",
                records: "records",
                repeatitems: false,
                Id: "0"
            },
            //gridComplete: function () {

            //    var id_no = $("#id_no").val();
            //    if (id_no != "") {
            //        $("#list").jqGrid('setSelection', id_no, { background: "#28a745 !important", color: "#fff" });
            //    }
            //    var rows = $("#list").getDataIDs();

            //    for (var i = 0; i < rows.length; i++) {
            //        var sd_sts_cd = $("#list").getCell(rows[i], "sd_sts_cd");
            //        var alert = $("#list").getCell(rows[i], "alert");

            //        if (alert == "2") {
            //            $("#list").jqGrid('setCell', rows[i], "sts_nm", "", {
            //                'background-color': 'rgb(244,177,131) ',
            //            });
            //        } else if (alert == "0") {
            //            $("#list").jqGrid('setCell', rows[i], "sts_nm", "", {
            //                'background-color': 'rgb(164, 204, 145)',
            //            });
            //        }
            //        if (sd_sts_cd == "001") {
            //            $("#list").jqGrid('setCell', rows[i], "sts_nm", "", {
            //                background: 'rgb(164,204,145)',
            //            });
            //        }

            //    }

            //}
        });
});
// phân trang //
function getData_primary(pdata) {
    //debugger;
    var params = new Object();

    if ($('#list').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }

    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;

   

    $('#list').jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    //debugger;
    $.ajax({
        url: `/HieuShippingMgt/GetPickingScan`,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $('#list')[0];
                showing.addJSONData(data);
            }
        },
        error: function () {
            return;
        }
    });

    
};

//-------INSERT------------//

$("#c_save_but").click(function () {
    var isValid = $("#form1").valid();
    if (isValid == true) {
        var sd_nm = $("#c_sd_nam").val();
        var remark = $("#c_remark").val();
        var product_cd = $("#c_style_no").val().trim();
        $.ajax({
            url: "/HieuShippingMgt/InsertSDInfo",
            type: "Post",
            dataType: "json",
            data: {
                sd_nm: sd_nm,
                remark: remark,
                product_cd : product_cd,
            },

            success: function (response) {
                if (response.result) {
                    var id = response.data[0].sid;
                    $("#list").jqGrid("addRowData", id, response.data[0], "first");
                    $("#list").setRowData(id, false, { background: "#28a745", color: "#fff" });
                    SuccessAlert(response.message)
                }
                else {
                    ErrorAlert("Data already exists, Please check again");
                }

            }
        });

    }

    //----- SD POPUP------------------//
    function SD_popup(cellValue) {
        var html = '<a class="css_pp_cilck" data-sd_no="' + cellValue + '" onclick="ViewSDPopup(this);">' + cellValue + '</a>';
        return html;
    };

    function ViewSDPopup(e) {
        $('.popup-dialog.SD_Info_Popup').dialog('open');
        var sd_no = $(e).data('sd_no');


        $.get("/HieuShippingMgt/PartialView_SD_Info_Popup?" +
            "sd_no=" + sd_no + ""
            , function (html) {
                $("#PartialView_SD_Info_Popup").html(html);
            });
    }

    $("#Select_completed").on("click", function () {

        var sd_no = $("#m_sd_no").val();
        if (sd_no == "" || sd_no == null || sd_no == undefined) {
            ErrorAlert("Vui lòng chọn mã SD!!!")
            return false;
        }


        $('.popup-dialog.List_ML_NO_Info_Popup').dialog('open');



        $.get("/HieuShippingMgt/PartialView_List_ML_NO_Info_Popup?" +
            "sd_no=" + sd_no + ""
            , function (html) {
                $("#PartialView_List_ML_NO_Info_Popup").html(html);
            });


    });
});
