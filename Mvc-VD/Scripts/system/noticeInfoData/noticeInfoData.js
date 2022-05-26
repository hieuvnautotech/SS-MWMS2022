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
                   name: "title", width: 200, align: 'left', label: "Title", formatter: notice_popup,

               },
                              { name: 'content', hidden: true },

               { label: 'Create Name', name: 'reg_id', sortable: true, align: 'left' },
                {
                    label: 'Create Date', name: 'reg_dt', sortable: true, formatter: "date",
                    formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }
                },
               { name: "chg_id", width: 100, label: "Change Name", align: 'left' },
               {
                   label: 'Change Date', name: 'chg_dt', sortable: true, formatter: "date",
                   formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }
               },
            ],
            onSelectRow: function (rowid, status, e) {
                var selectedRowId = $("#noticeMgt").jqGrid("getGridParam", 'selrow');
                row_id = $("#noticeMgt").getRowData(selectedRowId);
                var bno = row_id.bno;
                $('#s_bno').val(bno);
                $("#del_save_but").attr("disabled", false);

            },
            pager: jQuery('#jqGridPager2'),
            rowNum: 50,
            rowList: [50, 100, 200, 500, 1000],
            rownumbers: true,
            autowidth: true,
            shrinkToFit: false,
            viewrecords: true,
            height: '500px',
            loadonce: true,
            
            caption: 'Notice Information',
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
            url: "/system/searchnoticeinfodata",
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
function notice_popup(cellvalue, options, rowobject) {
    var bno = rowobject.bno;
    var regex = /(<([^>]+)>)/ig;
    if (cellvalue != null) {
        var html = '<a  data-toggle="modal"   style="color: dodgerblue;" data-target="#modifyModal" data-backdrop="static" data-keyboard="false" onclick="notice(' + bno + ');" class="poupgbom">' + cellvalue.replace(regex, "") + '</a>';
        return html;
    } else {
        return cellvalue;
    }
}
function notice(bno) {
    $('.dialog').dialog('open');
    viewdetailnotice(bno);
}


function viewdetailnotice(bno) {
    $("#s_bno").val(bno);
    $.get("/system/viewdetail_notice?bno=" + bno, function (data) {
        var html1 = '';
        var html = '';
        html += '<div class="to"><h4 class="title_c"> Writer </h4><div class="title_name">' + data.reg_id + '</div></div>'
        html += '<div class="to"><h4 class="title_c"> Subject </h4><div class="title_name">' + data.title + '</div></div>'
        html += '<div class="to"><h4 class="title_c"> Content </h4><textarea id="content">' + data.content + '</textarea></div>'
        $("#info").html(html);

        html1 = data.content;
        $('#bno').val(data.bno);
        var htmlx = '<textarea name="title"   id="title_x" >' + data.title + '</textarea>';
        var html2 = '<textarea name="content"   id="c_content2" >' + html1 + '</textarea>';
        $("#content2").html(html2);
        $("#title").html(htmlx);
        CKEDITOR.replace("title_x",
            {
                height: 100
            });
        CKEDITOR.replace("c_content2",
                {
                    height: 400
                });
        CKEDITOR.replace("content",
                {
                    //removePlugins: 'toolbar',
                    //toolbarStartupExpanded: false,
                    height: 400,
                    //allowedContent: 'p h1 h2 strong em; a[!href]; img[!src,width,height];',
                    allowedContent: true,

                    toolbar:[]
                });

    });
}
$("#tab_1").on("click", "a", function (event) {
    document.getElementById("form1").reset();
    $("#tab_2").removeClass("active");
    $("#tab_1").addClass("active");
    $("#tab_c2").removeClass("active");
    $("#tab_c1").removeClass("hidden");
    $("#tab_c2").addClass("hidden");
    $("#tab_c1").addClass("active");

});
$("#tab_2").on("click", "a", function (event) {

    document.getElementById("form2").reset();
    $("#tab_1").removeClass("active");
    $("#tab_2").addClass("active");
    $("#tab_c1").removeClass("active");
    $("#tab_c2").removeClass("hidden");
    $("#tab_c1").addClass("hidden");
    $("#tab_c2").addClass("active");
});
$(function () {
    $(".dialog").dialog({
        width: '100%',
        height: 850,
        maxWidth: 1000,
        maxHeight: 450,
        minWidth: '50%',
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

    });
    $('#close').click(function () {
        $('.dialog').dialog('close');
    });

});