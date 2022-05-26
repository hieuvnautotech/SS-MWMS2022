    $(function () {

        $grid = $("#locMgt").jqGrid({
            url: '/System/Getlocation',
            mtype: 'GET',
            datatype: 'json',
            colModel: [
                {
                    label: 'ID', name: 'lctno', width: 60, align: 'right',hidden: true
                },
                { label: 'Location Code', name: 'lct_cd', sortable: true, editable: true, editrules: { required: true, number: true }, width: 120, align: 'center' },
                { label: 'Location Name', name: 'mn_full', sortable: true, editrules: { required: true, number: true }, width: 120, },
                {
                    label: 'Full Name', name: 'lct_nm', sortable: true, editable: true, width: 250, editrules: { required: true, number: true },
                    cellattr: function (rowId, cellValue, rowObject) {
                        return ' title="' + cellValue + '"';
                    }
                },
                { label: 'Levle NM', name: 'level_cd', sortable: true, editrules: { required: true, number: true }, width: 60, align: 'center' },
                { label: 'Maintenace', name: 'mt_yn', sortable: true, editable: true, align: 'center' },
                { label: 'Movement', name: 'mv_yn', sortable: true, width: 60, align: 'center' },
                { label: 'Rental', name: 'rt_yn', sortable: true, width: 60, align: 'center' },
                { label: 'Use YN', name: 'use_yn', sortable: true, width: 60, align: 'center' },
                {
                    label: 'Remark', name: 're_mark', width: 100, sortable: true, cellattr: function (rowId, cellValue, rowObject) {
                        return ' title="' + cellValue + '"';
                    }
                },
                { label: 'Reg Name', name: 'reg_id', sortable: true, width: 70, },
                {
                    label: 'Reg Date', width: 110, name: 'reg_dt', sortable: true, formatter: "date",
                    formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i A" }
                },
                { label: 'Change Name', name: 'chg_id', sortable: true, width: 80, },
                {
                    label: 'Change Date', name: 'chg_dt', width: 110, sortable: true, formatter: "date",
                    formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i A" }
                },

            ],
            loadonce: true,
            shrinkToFit: false,
            pager: '#jqGridPager1',
            rowNum: 2000,
            rownumbers: true,

            rowList: [60, 80, 100, 120],
            viewrecords: true,
            height: 360,
            jsonReader:
          {
              root: "rows",
              page: "page",
              total: "total",
              records: "records",
              repeatitems: false,
              Id: "0"
          },
            width: $(".box-body").width() - 5,
            autowidth: false,
            onSelectRow: function (rowid, status, e) {
                var selectedRowId = $("#locMgt").jqGrid("getGridParam", 'selrow');
                row_id = $("#locMgt").getRowData(selectedRowId);

                var lctno = row_id.lctno;
                var lct_nm = row_id.lct_nm;
                var use_yn = row_id.use_yn;
                var mt_yn = row_id.mt_yn;
                var mv_yn = row_id.mv_yn;
                var rt_yn = row_id.rt_yn;
                var re_mark = row_id.re_mark;

                $('#m_lct_nm').val(lct_nm);
                $("#m_use_yn").val(use_yn);

                if (mt_yn == 'Y') {
                    $('#m_mt_yn').prop('checked', true);

                }
                if (mt_yn == 'N') {
                    $('#m_mt_yn').prop('checked', false);

                }
                if (mv_yn == 'Y') {
                    $('#m_mv_yn').prop('checked', true);
                }
                if (mv_yn == 'N') {
                    $('#m_mv_yn').prop('checked', false);
                }
                if (rt_yn == 'Y') {
                    $('#m_rt_yn').prop('checked', true);
                }
                if (rt_yn == 'N') {
                    $('#m_rt_yn').prop('checked', false);
                }
                $('#m_re_mark').val(re_mark);
                $('#m_lctno').val(lctno);
                $('#c_lctno').val(lctno);
                if (row_id.level_cd == "004") {
                    $("#c_re_mark").prop("disabled", true);
                    $("#c_lct_nm").prop("disabled", true);
                    $("#c_save_but").prop("disabled", true);
                    $("#c_reset_but").prop("disabled", true);
                    $("#c_mt_yn").prop("disabled", true);
                    $("#c_mv_yn").prop("disabled", true);
                    $("#c_ds_yn").prop("disabled", true);

                } else {
                    $("#c_re_mark").prop("disabled", false);
                    $("#c_lct_nm").prop("disabled", false);
                    $("#c_save_but").prop("disabled", false);
                    $("#c_reset_but").prop("disabled", false);
                    $("#c_mt_yn").prop("disabled", false);
                    $("#c_mv_yn").prop("disabled", false);
                    $("#c_ds_yn").prop("disabled", false);
                }
                  
            },
        });
            
        $("#locMgt").jqGrid('navGrid', '#jqGridPager1', { edit: false, add: false, del: false })
        //xuất file pdf,excel,doc
        //$(document).ready(function (e) {
        //    $('#excelBtn').click(function (e) {
        //        $("#locMgt").jqGrid('exportToExcel',
        //             options = {
        //                 includeLabels: true,
        //                 includeGroupHeader: true,
        //                 includeFooter: true,
        //                 fileName: "Location Information.xlsx",
        //                 mimetype: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //                 maxlength: 40,
        //                 onBeforeExport: null,
        //                 replaceStr: null
        //             }
        //         );
        //    });
        //});
        //$(document).ready(function (e) {
        //    $('#htmlBtn').click(function (e) {
        //        $("#locMgt").jqGrid('exportToHtml',
        //            options = {
        //                title: '',
        //                onBeforeExport: null,
        //                includeLabels: true,
        //                includeGroupHeader: true,
        //                includeFooter: true,
        //                tableClass: 'jqgridprint',
        //                autoPrint: false,
        //                topText: '',
        //                bottomText: '',
        //                fileName: "Location Information  ",
        //                returnAsString: false
        //            }
        //         );
        //    });

        //});
        //$(document).ready(function (e) {
        //    $('#pdfBtn').click(function (e) {
        //        $("#locMgt").jqGrid('exportToPdf',
        //          options = {
        //              title: null,
        //              orientation: 'landscape',
        //              pageSize: 'A4',
        //              description: null,
        //              onBeforeExport: null,
        //              download: 'download',
        //              includeLabels: true,
        //              includeGroupHeader: true,
        //              includeFooter: true,
        //              fileName: "Location.pdf",
        //              mimetype: "application/pdf"
        //          }
        //         );
        //    });

        //});

        $(document).ready(function (e) {
            $('#pdfBtn').click(function (e) {
                $('table').tableExport({
                    headers: true,
                    type: 'pdf',
                    pdfFontSize: '12',
                    fileName: 'LocationType_Infor',
                    escape: false,
                });
            });
        });
        $(document).ready(function (e) {
            $('#excelBtn').click(function (e) {
                $('table').tableExport({
                    headers: true,
                    type: 'xls',
                    fileName: 'LocationType_Infor',
                    escape: false,
                    headers: true,
                });
            });
        });
        $(document).ready(function (e) {
            $('#htmlBtn').click(function (e) {
                $('table').tableExport({
                    type: 'doc',
                    fileName: 'LocationType_Infor',
                    escape: false,
                    headers: true,
                });
            });
        });

        $(document).ready(function () {
            $('input[name=root_yn]').click(function () {
                if ($(this).prop("checked") == true) {
                    $("#c_root_yn").val("1");
                }
                else if ($(this).prop("checked") == false) {
                    $("#c_root_yn").val("");
                }
            });
        });
        $("#c_save_but").click(function () {
            if ($('input[name=root_yn]').is(":checked")) {
                if ($("#c_lctno").val() == "") {
                    $("#c_lctno").val("1");
                }
                  
            } else {
                var selRowId = $('#locMgt').jqGrid("getGridParam", "selrow");
                if (selRowId == null) {
                    alert("Please select the top Location on the grid.");
                    return false;
                }
            }
            if ($("#c_lct_nm").val().trim() == "") {
                alert("Please enter Location Name");
                $("#c_lct_nm").val("");
                $("#c_lct_nm").focus();
                return false;
            }

            if ($("#c_re_mark").val() == "") {
                alert("Please enter Remark");
                $("#c_re_mark").val("");
                $("#c_re_mark").focus();
                return false;
            } else {
                var c_root_yn = $("#c_root_yn").val();
                $.ajax({
                    url: "/system/insertlocMgt",
                    type: "get",
                    dataType: "json",
                    data: {
                        lct_nm: $('#c_lct_nm').val(),
                        use_yn: $('#c_use_yn').val(),
                        mt_yn: $('#c_mt_yn:checked').val(),
                        mv_yn: $('#c_mv_yn:checked').val(),
                        rt_yn: $('#c_rt_yn:checked').val(),
                        re_mark: $('#c_re_mark').val(),
                        lctno: $('#c_lctno').val(),
                        c_root_yn: $('#c_root_yn').val(),
                            
                    },
                    success: function (data) {
                        jQuery("#locMgt").setGridParam({ rowNum: 2000, datatype: "json" }).trigger('reloadGrid');
                        location.reload();
                    }

                });
            }
        });
        $("#m_save_but").click(function () {
            $.ajax({
                url: "/system/updatelocMgt",
                type: "get",
                dataType: "json",
                data: {
                    lct_nm: $('#m_lct_nm').val(),
                    use_yn: $('#m_use_yn option:selected').val(),
                    mt_yn: $('#m_mt_yn:checked').val(),
                    mv_yn: $('#m_mv_yn:checked').val(),
                    rt_yn: $('#m_rt_yn:checked').val(),
                    re_mark: $('#m_re_mark').val(),
                    lctno: $('#m_lctno').val(),
                },
                success: function (data) {
                    jQuery("#locMgt").setGridParam({ rowNum: 2000, datatype: "json" }).trigger('reloadGrid');
                    document.getElementById("form2").reset();
                }


            });

        });


    });
