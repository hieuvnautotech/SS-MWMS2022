QCRange_data = [];

Item_vcd_data = [];

$("#list").jqGrid
    ({
        mtype: 'GET',
        datatype: 'json',
        colModel: [
            { label: 'ID', name: 'mtid', key: true, width: 50, align: 'center', hidden: true },
            { label: 'Type', name: 'mt_type', sortable: true, width: 60, align: 'center', hidden: true },
            { label: 'Type', name: 'mt_type_nm', sortable: true, width: 120 },
            { label: 'Barcode', name: 'barcode', sortable: true, width: 80, align: 'center', },
            { label: 'MT NO', name: 'mt_no', width: 200, align: 'left' },
            { label: 'Name', name: 'mt_nm', sortable: true, width: 500, },
            { label: 'Bundle Qty', name: 'bundle_qty', width: 100, align: 'right' },
            { label: 'Bundle Unit', name: 'bundle_unit', sortable: true, width: 100, align: 'left' },
            { label: 'Width', name: 'width', sortable: true, width: 80, align: 'right', formatter: 'integer' },
            { label: 'Unit', name: 'width_unit', align: 'left', width: 50, },
            { label: 'Length', name: 'spec', sortable: true, width: 100, align: 'right', formatter: 'integer' },
            { label: 'Unit', name: 'spec_unit', width: 50, align: 'left' },
            { label: 'Supplier', name: 'sp_cd', sortable: true, width: 100, align: 'left', },
            { label: 'Unit', name: 'unit_cd', sortable: true, width: 60, align: 'left', },
          
            { label: 'Create Date', name: 'reg_dt', width: 100, align: 'center', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d" } },
            { label: 'Create User', name: 'reg_id', width: 90, align: 'left' },
            { label: 'Change Name', name: 'chg_id', width: 90, align: 'left', hidden: true },
            { label: 'Change Date', name: 'chg_dt', width: 100, align: 'center', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d" }, hidden: true },
        ],
        cmTemplate: { title: false },
        gridComplete: function () {
            var rows = $("#list").getDataIDs();
            for (var i = 0; i < rows.length; i++) {
                var use_yn = $("#list").getCell(rows[i], "barcode");
                if (use_yn == "Y") {
                    $("#list").jqGrid('setRowData', rows[i], false, { background: 'rgb(241 206 151)' });
                }
            }
        },
        onSelectRow: function (rowid, status, e) {
            var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
            row_id = $("#list").getRowData(selectedRowId);
           
            var mtid = row_id.mtid;
            var mt_type = row_id.mt_type;
            var mt_no = row_id.mt_no;
            var mt_nm = row_id.mt_nm;
            var width = row_id.width;
            var width_unit = row_id.width_unit;
            var spec = row_id.spec;
            var spec_unit = row_id.spec_unit;
            var price = row_id.price;
            var price_unit = row_id.price_unit;
            var re_mark = row_id.re_mark;
            var photo_file = row_id.photo_file;

            $('#m_bundle_qty').val(row_id.bundle_qty);
            $('#lot_mt_no').val(row_id.mt_no);
            $('#m_stick').val(row_id.stick);
            $('#m_stick_unit').val(row_id.stick_unit);
            $('#m_price_unit').val(row_id.price_unit);
            $('#m_tot_price').val(row_id.tot_price);
            $('#m_price').val(row_id.price);
            $('#m_price_least_unit').val(row_id.price_least_unit);
            //$('#m_mt_cd').val(row_id.mt_cd);
            $('#m_thick').val(row_id.thick);
            $('#m_thick_unit').val(row_id.thick_unit);

            $('#m_barcode').val(row_id.barcode);

            $('#m_unit_cd').val(row_id.unit_cd);
            $('#m_bundle_unit').val(row_id.bundle_unit);
            $('#m_mt_no_origin').val(row_id.mt_no_origin);
            $('#m_area').val(row_id.area);
            $('#m_s_lot_no').val(row_id.s_lot_no);
            $('#m_area_unit').val(row_id.area_unit);
            $('#m_item_vcd').val(row_id.item_vcd);
            $('#m_qc_range').val(row_id.qc_range_cd);
            $('#m_logo1').val(photo_file);
            $('#m_manufac').val(row_id.mf_cd);
            $('#m_supllier').val(row_id.sp_cd);
            $('#m_mtid').val(mtid);
            $('#m_mt_no').val(mt_no);
            $('#m_mt_nm').val(mt_nm);
            $('#m_width').val(width);
            $('#m_width_unit').val(width_unit);
            $('#m_spec').val(spec);
            $('#m_spec_unit').val(spec_unit);

            $('#m_re_mark').val(re_mark);
            $('#m_gr_qty').val(row_id.gr_qty);
            $('#m_consumable').val(row_id.consumable);

            $('#m_mt_type').val(row_id.mt_type);

           
        },

        viewrecords: true,
        height: 400,
        rowNum: 50,
        rownumbers: true,      //컬럼 맨 앞에 순번컬럼 붙일지 말지( 1,2,3...)
        rowList: [50, 100, 200, 500, 1000], //한 번에 보여줄 레코드 수를 변동할 때 선택 값
        // reload 여부이면 true로 설정하며 한번만 데이터를 받아오고 그 다음부터는 데이터를 받아오지 않음
        loadtext: "Loading...",			// 서버연동시 loading 중이라는 표시에 문자열 지정
        emptyrecords: "No data.",	// 데이터가 없을 경우 보열줄 문자열 
        gridview: true,
        shrinkToFit: false,
        datatype: function (postData) { getDataOutBox(postData); },
        pager: "#jqGridPager",
        jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        ajaxGridOptions: { contentType: "application/json" },
        autowidth: true,
    });

$('#list').jqGrid('setGridWidth', $(".boxlist").width());

function getDataOutBox(pdata) {
    $('.loading').show();

    var type = $("#s_mt_type").val().trim();
    var code = $("#mt_no").val().trim();
    var name = $("#mt_nm").val().trim();
    var start1 = $("#start1").val();
    var end1 = $("#end1").val();
    var params = new Object();
    if (jQuery('#list').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params.type = type;
    params.code = code;
    params.name = name;
    params.start = start1;
    params.end = end1;
    //params.end1Data = type;
    $("#list").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    params._search = pdata._search;

    $.ajax({
        url: '/Supplier/searchMachiningQR',
        type: "Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var grid = $("#list")[0];
                grid.addJSONData(data);
                $('.loading').hide();
            }
        }
    })
};


$("#searchBtn").click(function () {
    $("#list").clearGridData();
    $('.loading').show();
    var grid = $("#list");
    grid.jqGrid('setGridParam', { search: true });
    var pdata = grid.jqGrid('getGridParam', 'postData');
    getDataOutBox(pdata);
});

$(window).on("resize", function () {
    var newWidth = $("#list").closest(".ui-jqgrid").parent().width();
    $("#list").jqGrid("setGridWidth", newWidth, false);
});

function formatNumber(nStr, decSeperate, groupSeperate) {
    nStr += '';
    x = nStr.split(decSeperate);
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + groupSeperate + '$2');
    }
    return x1 + x2;
}

function widthformat(cellValue, options, rowdata, action) {
    if (cellValue != null) {
        var html = formatNumber(cellValue, '.', ',') + " " + ((rowdata.width_unit != null) ? rowdata.width_unit : "");
        return html;

    } else {
        return "";
    }
}

function specformat(cellValue, options, rowdata, action) {
    if (cellValue != null) {

        var html = formatNumber(cellValue, '.', ',') + " " + ((rowdata.spec_unit != null) ? rowdata.spec_unit : "");
        return html;

    } else {
        return "";
    }
}

function areaformat(cellValue, options, rowdata, action) {
    if (cellValue != null) {

        var html = formatNumber(cellValue, '.', ',') + " " + ((rowdata.area_unit != null) ? rowdata.area_unit : "");
        return html;

    } else {
        return "";
    }
}

function priceformat(cellValue, options, rowdata, action) {
    if (cellValue != null) {
        var html = formatNumber(cellValue, '.', ',') + " " + ((rowdata.price_least_unit != null) ? rowdata.price_least_unit : "");
        return html;

    } else {
        return "";
    }
}

function totalpriceformat(cellValue, options, rowdata, action) {
    if (cellValue != null) {
        var html = formatNumber(cellValue, '.', ',') + " " + ((rowdata.price_unit != null) ? rowdata.price_unit : "");
        return html;

    } else {
        return "";
    }
}

function stickformat(cellValue, options, rowdata, action) {
    if (cellValue != null) {
        var html = formatNumber(cellValue, '.', ',') + " " + ((rowdata.stick_unit != null) ? rowdata.stick_unit : "");
        return html;

    } else {
        return "";
    }
}

function thickformat(cellValue, options, rowdata, action) {
    if (cellValue != null) {
        var html = formatNumber(cellValue, '.', ',') + " " + ((rowdata.thick_unit != null) ? rowdata.thick_unit : "");
        return html;

    } else {
        return "";
    }
}

function qc_range_nm_format(cellValue, options, rowdata, action) {
    if (cellValue != null) {
        var html = '';
        $.each(QCRange_data, function (key, item) {
            if (cellValue == item.dt_cd) { html += item.dt_nm; }
        });
        return html;
    }
    else {
        return "";
    }
}

function item_vcd_nm_format(cellValue, options, rowdata, action) {
    if (cellValue != null) {
        var html = '';
        $.each(Item_vcd_data, function (key, item) {
            if (cellValue == item.item_vcd) { html += item.item_nm; }
        });
        return html;
    }
    else {
        return "";
    }
}


function downloadLink(cellValue, options, rowdata, action) {
    if (cellValue != null) {
        var html = '<a href="#" class="popupimg" data-img="' + cellValue + '"><img src="../Images/MarterialImg/' + cellValue + '" style="height:20px;" /></a>';
        return html;

    } else {
        return "";
    }
};





//select option

$("#start1").datepicker({ dateFormat: 'yy-mm-dd' }).val();
$("#end1").datepicker({ dateFormat: 'yy-mm-dd' }).val();

//GetType_Marterial();
function GetType_Marterial() {
    $.get("/DevManagement/GetType_Marterial", function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '<option value="">*Type*</option>';
            html += '<option value=' + data[0].dt_cd + '>' + data[0].dt_nm + '</option>';
            for (var i = 1; i < data.length; i++) {
                html += '<option value=' + data[i].dt_cd + '>' + data[i].dt_nm + '</option>';
            }
            $(".gettype").html(html);
        }


    });
};

//GetWidth_Marterial();
function GetWidth_Marterial() {
    $.get("/DevManagement/GetWidth_Marterial", function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            $(".getwidth").empty();
            $.each(data, function (key, item) {
                if (item.dt_nm == "mm") { html += '<option value=' + item.dt_cd + ' selected="selected">' + item.dt_nm + '</option>'; } else {
                    html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
                }
            });
            $(".getwidth").html(html);
        }
    });
};


//_GetUnit_qty();
function _GetUnit_qty() {

    $.get("/DevManagement/GetUnit_qty", function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            $.each(data, function (key, item) {
                if (item.dt_nm == "M") {
                    html += '<option value=' + item.dt_cd + ' selected="selected">' + item.dt_nm + '</option>';
                } else {
                    html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
                }
            });
            $("#c_unit_cd").html(html);
            $("#m_unit_cd").html(html);
            $("#pp_unit_cd").html(html);
        }
    });
}




//get bundle unit
_Getbundle();
function _Getbundle() {
    $.get("/DevManagement/Get_Getbundle", function (data) {
        if (data != null && data != undefined && data.length) {
            //var html = '';
            //html += '<option value="" selected="selected">*Bundle Unit*</option>';
            //$.each(data, function (key, item) {
            //    html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            //});
            //$("#c_bundle_unit").html(html);
            //$("#m_bundle_unit").html(html);
            //$("#pp_bundle_unit").html(html);

            var html = '';
            $("#c_bundle_unit").empty();
            $("#m_bundle_unit").empty();
            $("#pp_bundle_unit").empty();

            html += '<option value=' + data[0].dt_cd + '>' + data[0].dt_nm + '</option>';
            for (var i = 1; i < data.length; i++) {
                html += '<option value=' + data[i].dt_cd + '>' + data[i].dt_nm + '</option>';
            }
            $("#c_bundle_unit").html(html);
            $("#m_bundle_unit").html(html);
            $("#pp_bundle_unit").html(html);
        }
    });
}




