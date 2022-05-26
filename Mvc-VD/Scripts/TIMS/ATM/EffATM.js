$(document).ready(function (e) {
    _GetBomType();
});
$grid = $("#list").jqGrid({

    mtype: 'GET',
    datatype: 'json',
    colModel: [
        { key:true, label: 'PO NO', name: 'at_no', width: 120, align: 'center' },
        {  label: 'Tuần', name: 'remark', width: 160, align: 'left' },
        { label: 'Product Code', name: 'product', width: 120, align: 'left' },
        { label: 'Product Name', name: 'product_nm', width: 100, align: 'left' },
        { label: 'Type', name: 'bom_type', width: 100, align: 'left' },
        { label: 'Sản lượng LT', name: 'SanLuongLyThuyet', width: 100, align: 'right' },
        { label: 'Số(M) LT', name: 'SoMetLyThuyet', width: 100, align: 'right', formatter: 'integer' },
        { label: 'Mã Nguyên vật liệu', name: 'mt_no', width: 150, align: 'left' },
        { label: '%Loss', name: 'loss', width: 100, align: 'right' },
        { label: 'Hiệu suất sản xuất(%)', name: 'HieuSuatSanXuat', width: 130, align: 'right'},
        { label: 'OK', name: 'OKSanXuat', width: 100, align: 'right', formatter: 'integer' },
        { label: 'NG', name: '', width: 100, align: 'right', formatter: 'integer' },
        { label: 'Hiệu suất OQC(%)', name: 'HieusuatOQC', width: 120, align: 'right' },
        { label: 'OK', name: 'OkThanhPham', width: 100, align: 'right', formatter: 'integer' },
        { label: 'NG', name: 'NGThanhPham', width: 100, align: 'right', formatter: 'integer' },
        { label: 'Hàng chờ kiểm', name: 'HangChoKiem', width: 110, align: 'right', formatter: 'integer' },
       
    ],
    pager: jQuery('#listPager'),
 
    viewrecords: true,
    rowList: [50, 100, 200, 500, 1000],
    height: 500,
    width: null,
    autowidth: true,
    rowNum: 50,
    caption: 'Hiệu suất ATM',
    loadtext: "Loading...",
    emptyrecords: "No data.",
    rownumbers: true,
    gridview: true,
    loadonce: false,
    shrinkToFit: false,
    multiselect: false,
    jsonReader:
    {
        root: "rows",
        page: "page",
        total: "total",
        records: "records",
        repeatitems: false,
        Id: "0"
    },
   
    datatype: function (postData) { getDataTIMS_AT(postData); },
});

jQuery("#list").jqGrid('setGroupHeaders', {
    useColSpanStyle: true,
    groupHeaders: [
        { startColumnName: 'OKSanXuat', numberOfColumns: 2, titleText: '<center>Sản lượng thực tế</center>' },
        { startColumnName: 'OkThanhPham', numberOfColumns: 3, titleText: '<center>Thành phẩm</center>' }
    ]
});

function _GetBomType() {
    $.get("/DevManagement/GetBomType", function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Type*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#s_bom_type").html(html);
        }
    });
};
function getDataTIMS_AT(pdata) {
    var params = new Object();
    var rows = $("#list").getDataIDs();
    if (rows.length == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;
    params.SearchProductCode = $('#SearchProductCode').val() == null ? "" : $('#SearchProductCode').val().trim();
    params.SearchProductName = $('#SearchProductName').val() == null ? "" : $('#SearchProductName').val().trim();
    params.SearchPO = $('#SearchPO').val() == null ? "" : $('#SearchPO').val().trim();
    params.SearchRemark = $('#SearchRemark').val() == null ? "" : $('#SearchRemark').val().trim();
    params.bom_type = $('#s_bom_type').val() == null ? "" : $('#s_bom_type').val().trim();
   
    $("#list").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    $.ajax({
      
        url: "/TIMS/searchEffATM",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $("#list")[0];
                showing.addJSONData(data);
               
            }
        },
        error: function () {
          
            return;
        }
    });
};

$("#searchBtn").click(function () {
    $("#list").clearGridData();
    $("#list").jqGrid('setGridParam', { search: true });
    var pdata = $("#list").jqGrid('getGridParam', 'postData');
    getDataTIMS_AT(pdata)

});



$('#excelBtn').click(function () {
    var SearchProductCode = $(`#SearchProductCode`).val() == null ? `` : $(`#SearchProductCode`).val().trim();
    var SearchProductName = $('#SearchProductName').val() == null ? "" : $('#SearchProductName').val().trim();
    var SearchPO = $(`#SearchPO`).val() == null ? '' : $(`#SearchPO`).val().trim();
    var SearchRemark = $(`#SearchRemark`).val() == null ? '' : $(`#SearchRemark`).val().trim();
    $('#exportData').attr('action', "/TIMS/ExportsearchEffATM?SearchProductCode=" + SearchProductCode + "&SearchProductName=" + SearchProductName + "&SearchPO=" + SearchPO + "&SearchRemark=" + SearchRemark);

});