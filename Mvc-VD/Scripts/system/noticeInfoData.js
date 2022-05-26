    $(document).ready(function () {


        $(function () {
            $grid = $("#noticeMgt").jqGrid
            ({
                url: '/System/Getnotice',
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                   { name: "bno", width: 50, align: "left", label: "ID", sortable: true, hidden: true },
                   {
                       name: "title", width: 120, align: 'left', label: "title", formatter: setBoard_link,

                   },
                                  { name: 'content', hidden: true },

                   { label: 'Reg Name', name: 'reg_id', sortable: true, align: 'left' },
                    {
                        label: 'Reg Date', name: 'reg_dt', sortable: true, formatter: "date",
                        formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i A" }
                    },
                   { name: "chg_id", width: 100, label: "Chage Name", align: 'left' },
                   {
                       label: 'Chage Date', name: 'chg_dt', sortable: true, formatter: "date",
                       formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i A" }
                   },
                ],
                onSelectRow: function (rowid, status, e) {
                    var selectedRowId = $("#noticeMgt").jqGrid("getGridParam", 'selrow');
                    row_id = $("#noticeMgt").getRowData(selectedRowId);
                    var bno = row_id.bno;
                    $('#s_bno').val(bno);
                },
                pager: jQuery('#jqGridPager'),
                rowNum: 50,
                rowList: [50, 100, 200, 500, 1000],
                rownumbers: true,
                autowidth: true,
                shrinkToFit: false,
                viewrecords: true,
                height: '100%',
                caption: 'Process Information',
                jsonReader:
                {
                    root: "rows",
                    page: "page",
                    total: "total",
                    records: "records",
                    repeatitems: false,
                    Id: "0"
                },
                autowidth: true,
                multiselect: false,
            }).navGrid('#gridpager',
                {
                    edit: false, add: false, del: false, search: false,
                    searchtext: "Search Student", refresh: true
                },
                {
                    zIndex: 100,
                    caption: "Search Students",
                    sopt: ['cn']
                });


        });//grid1


        // Init Datepicker start
        $("#start1").datepicker({
            format: 'mm/dd/yyyy',
        });
        //$('#end1').datepicker().datepicker('setDate', 'today');

        // Init Datepicker end
        $("#end1").datepicker({
            format: 'mm/dd/yyyy',
            "autoclose": true

        });

        // Search user
        $("#searchBtn1").click(function () {
            var searchType1 = $("#searchType1").val();
            var keywordInput1 = $("#keywordInput1").val().trim();
            var start1 = $("#start1").val();
            var end1 = $("#end1").val();

            $.ajax({
                url: "/system/SearchNoticeInfoData",
                type: "get",
                dataType: "json",
                data: {
                    searchType1Data: searchType1,
                    keywordInput1Data: keywordInput1,
                    start1Data: start1,
                    end1Data: end1,

                },
                success: function (result) {
                    $("#noticeMgt")
                    .jqGrid('clearGridData')
                    .jqGrid('setGridParam', {
                        datatype: 'local',
                        data: result
                    })
            .trigger("reloadGrid");

                },
                error: function (result) {
                    alert('Not found');
                }
            });
        });

    });
function setBoard_link(cellvalue, options, rowdata, action) {

    if (cellvalue != null) {
        var html = '<a  href="/system/noticeread?bno=' + rowdata.bno + '" target="_sub">' + cellvalue + '</a>';
        return html;
    } else {
        return cellvalue;
    }
} //options.rowid

