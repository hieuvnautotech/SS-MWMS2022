$(function () {
    $("#list").jqGrid
    ({
        url: "/QCInformation/GetQCList_M",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
           { key: true, label: 'icdno', name: 'icdno', width: 80, align: 'center',hidden:true },
            { key: false, label: 'MMPO', name: 'mpo_no', sortable: true, width: '100', align: 'center' },
            { key: false, label: 'MPO', name: 'mdpo_no', sortable: true, width: '100', align: 'center' },
            { key: false, label: 'MT No', name: 'mt_no', sortable: true, width: '180' },
            { key: false, label: 'MT Name', name: 'mt_nm', sortable: true, width: '200' },
            { key: false, label: 'Receive date', name: 'rec_input_dt', sortable: true, width: '100px', align: 'center' },
            { key: false, label: 'Work date', name: 'work_dt', sortable: true, width: '150px', align: 'center' },
            { key: false, label: 'MQ NO', name: 'mq_no', sortable: true, width: '100px', align: 'center' },
            { key: false, label: 'Purchase qty', name: 'act_pur_qty', sortable: true, width: '100px', align: 'right', formatter: 'integer' },
            { key: false, label: 'Receive qty', name: 'rec_qty', sortable: true, width: '100', align: 'right', formatter: 'integer' },
            { key: false, label: 'QC Code', name: 'item_vcd', sortable: true, width: '100', align: 'center' },
            { key: false, label: 'QC Qty', name: 'qc_qty', sortable: true, width: '150', align: 'right', formatter: 'integer' },
            { key: false, label: 'QC Rate', name: 'qc_rate', sortable: true, width: '150', align: 'right', },
        ],
        onSelectRow: function ( rowid, selected, status, e) {
            var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
            row_id = $("#list").getRowData(selectedRowId);
            $("#list2").setGridParam({ url: "/QCInformation/Getdetai_qc_list?" + "item_vcd=" + row_id.item_vcd + "&mq_no=" + row_id.mq_no, datatype: "json" }).trigger("reloadGrid");


        },
        pager: "#gridpager",
        pager: jQuery('#gridpager'),
        viewrecords: true,
        rowList: [50, 100, 200, 500, 1000],
        height: 250,
        width: $(".box-body").width() - 5,
        autowidth: false,
        rowNum: 50,
        loadtext: "Loading...",
        emptyrecords: "No data.",
        rownumbers: true,
        gridview: true,
        shrinkToFit: false,
        loadonce: true,
        viewrecords: true,
        jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
    })
              
          
    $("#searchBtn").click(function () {
        
        var mpo_no = $('#mpo_no').val().trim();
        var mt_no = $('#s_mt_no').val().trim();
        var start = $('#start').val().trim();
        var end = $('#end').val().trim();
        $.ajax({
            url: "/QCInformation/searchQClist",
            type: "get",
            dataType: "json",
            data: {
                mpo_no: mpo_no,
                mt_no: mt_no,
                start: start,
                end: end,
            },
            success: function (result) {
                $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
            }
        });
    });

    $(document).ready(function (e) {
        $('#htmlBtn').click(function (e) {
            $("#list").jqGrid('exportToHtml',
                options = {
                    title: '',
                    onBeforeExport: null,
                    includeLabels: true,
                    includeGroupHeader: true,
                    includeFooter: true,
                    tableClass: 'jqgridprint',
                    autoPrint: false,
                    topText: '',
                    bottomText: '',
                    fileName: "IQC List",
                    returnAsString: false
                }
             );
        });

    });
    $(document).ready(function (e) {
        $('#excelBtn').click(function (e) {
            $("#list").jqGrid('exportToExcel',
                 options = {
                     includeLabels: true,
                     includeGroupHeader: true,
                     includeFooter: true,
                     fileName: "IQC List.xlsx",
                     mimetype: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                     maxlength: 40,
                     onBeforeExport: null,
                     replaceStr: null
                 }
             );
        });
    });

});//grid1

$("#start").datepicker({ dateFormat: 'yy-mm-dd' }).val();

$("#end").datepicker({ dateFormat: 'yy-mm-dd' }).val();
$(function () {
    $grid = $("#list2").jqGrid
({
    url: "/QCInformation/Getdetai_qc_list",
    datatype: 'json',
    mtype: 'post',
    colModel: [
            { key: true, label: 'mqhno', name: 'mqhno', width: 200, align: 'center', hidden: true },
            { key: false, label: 'MQ NO', name: 'mq_no', sortable: true, width: '100px', align: 'center' },
            { key: false, label: 'MPO', name: 'mdpo_no', width: 100, align: 'center' },
            { key: false, label: 'Subject', name: 'check_subject', width: 100, align: 'center' },
            { key: false, label: 'Value', name: 'check_value', sortable: true, width: '100px', align: 'left' },
            { key: false, label: 'Qty', name: 'qc_qty', sortable: true, width: '100px', align: 'right' },
    ],
    pager: jQuery('#grid2pager'),
    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
    rownumbers: true,
    autowidth: true,
    shrinkToFit: false,
    viewrecords: true,
    loadonce: true,
    height: '300',
    //caption: 'Part',
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
}).navGrid('#partpager',
{
    edit: false, add: false, del: false, search: false,
    searchtext: "Search Student", refresh: true
},
{
    zIndex: 100,
    caption: "Search",
    sopt: ['cn']
});
});//grid2
