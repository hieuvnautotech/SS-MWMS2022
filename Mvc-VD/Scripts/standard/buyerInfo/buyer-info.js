var headers = {
    buyer_cd: "Buyer Code".replace(/,/g, ''), // remove commas to avoid errors
    buyer_nm: "Buyer Name",
    brd_nm: "Brand Name",
    re_mark: "Description",
    logo: "Logo",
    web_site: "Website",
    phone_nb: "Phone Number",
    cell_nb: "Cell Number",
    e_mail: "E-Mail",
    fax_nb: "Fax",
    address: "Address",
    use_yn: "Use YN",
    reg_id: "Create Name",
    reg_dt: "Create date",
    chg_id: "Change Name",
    chg_dt: "Change date"
};

$(function () {
    $("#list").jqGrid
    ({
        url: "/standard/GetBuyerInfo",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { key: true, label: 'byno', name: 'byno', width: 80, align: 'center', hidden: true },
            { key: false, label: 'Buyer Code', name: 'buyer_cd', sortable: true, width: '80', align: 'center' },
            { key: false, label: 'Buyer Name', name: 'buyer_nm', sortable: true, width: '200' },
            { key: false, label: 'Brand Name', name: 'brd_nm', editable: true, sortable: true, width: '150' },
            { key: false, label: 'Description', name: 're_mark', editable: true, width: '100px' },
            //{ key: false, label: 'Logo', name: 'logo', editable: true, width: '100px', formatter: image_logo, align: 'center', exportoptions: { excel_parser: false } },
            //{ key: false, label: 'Logo', name: 'logo', editable: true, width: '100px', hidden: true },
            { key: false, label: 'Website', name: 'web_site', editable: true, width: '180' },
            { key: false, label: 'Phone Number', name: 'phone_nb', editable: true, width: '100px', align: 'center' },
            { key: false, label: 'Cell Number', name: 'cell_nb', editable: true, width: '100px', align: 'center' },
            { key: false, label: 'E-Mail', name: 'e_mail', editable: true, width: '200px' },
            { key: false, label: 'Fax', name: 'fax_nb', editable: true, width: '100px', align: 'center' },
            { key: false, label: 'Address', name: 'address', editable: true, width: '250' },
            { key: false, label: 'Use YN', name: 'use_yn', editable: true, width: '50', align: 'center' },
            { key: false, label: 'Create Name', name: 'reg_id', index: 'reg_id', width: '100px' },
            { key: false, label: 'Create date', name: 'reg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }, width: '150' },
            { key: false, label: 'Change Name', name: 'chg_id', editable: true, width: '100px' },
            { key: false, label: 'Change date', name: 'chg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }, width: '150' }

        ],
        gridComplete: function () {
            var rows = $("#list").getDataIDs();
            for (var i = 0; i < rows.length; i++) {
                var buyer_cd = $("#list").getCell(rows[i], "buyer_cd");
                var giatri = $('#remark_color').val();
                if (buyer_cd == giatri) {
                    $("#list").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
                }
            }
        },
        onSelectRow: function (rowid, selected, status, e) {
            $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
            var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
            row_id = $("#list").getRowData(selectedRowId);

            var byno = row_id.byno;
            var buyer_cd = row_id.buyer_cd;
            var buyer_nm = row_id.buyer_nm;
            var brd_nm = row_id.brd_nm;
            var web_site = row_id.web_site;
            var cell_nb = row_id.cell_nb;
            var address = row_id.address;
            var phone_nb = row_id.phone_nb;
            var e_mail = row_id.e_mail;
            var fax_nb = row_id.fax_nb;
            var use_yn = row_id.use_yn;
            var re_mark = row_id.re_mark;
            var logo = row_id.logo;

            document.getElementById("form2").reset();
            $("#tab_1").removeClass("active");
            $("#tab_2").addClass("active");
            $("#tab_c1").removeClass("active");
            $("#tab_c2").removeClass("hidden");
            $("#tab_c1").addClass("hidden");
            $("#tab_c2").addClass("active");
            $("#tab_1").removeClass("active");
            $("#m_save_but").attr("disabled", false);
            $("#del_save_but").attr("disabled", false);

            $('#m_byno').val(byno);
            $('#m_logo1').val(logo);
            $('#m_buyer_cd').val(buyer_cd);
            $('#m_buyer_nm').val(buyer_nm);
            $('#m_brd_nm').val(brd_nm);
            $("#m_web_site").val(web_site);
            $("#m_cell_nb").val(cell_nb);
            $("#m_address").val(address);
            $("#m_phone_nb").val(phone_nb);
            $("#m_e_mail").val(e_mail);
            $("#m_fax_nb").val(fax_nb);
            $("#m_use_yn").val(use_yn);
            $("#m_re_mark").val(re_mark);

            if (logo != null) {
                $("#m_logo").html('<img src="../Images/BuyerImg/' + logo + '" style="height:50px" />');

            } else {
                $("#m_logo").html("");
            }
        },

        pager: '#gridpager',
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],
        rownumbers: true,
        autowidth: false,
        shrinkToFit: false,
        viewrecords: true,
        height: 400,
        width: $(".box-body").width() - 5,
        loadonce: true,
        caption: 'Buyer Information',
        emptyrecords: 'No Data',
        jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        multiselect: false,
    });

    //$.jgrid.defaults.responsive = true;
    //$.jgrid.defaults.styleUI = 'Bootstrap';
    $('#list').jqGrid('setGridWidth', $(".box-body").width());
    $(window).on("resize", function () {
        var newWidth = $("#list").closest(".ui-jqgrid").parent().width();
        $("#list").jqGrid("setGridWidth", newWidth, false);
    });

    $("#c_save_but").click(function () {
        if ($("#form1").valid() == true) {
            var formData = new FormData();
         /*   var files = $("#imgupload").get(0).files;*/

           /* formData.append("file", files[0]);*/
            formData.append("buyer_cd", $('#c_buyer_cd').val());
            formData.append("buyer_nm", $('#c_buyer_nm').val());
            formData.append("brd_nm", $('#c_brd_nm').val());
            formData.append("web_site", $('#c_web_site').val());
            formData.append("cell_nb", $('#c_cell_nb').val());
            formData.append("address", $('#c_address').val());
            formData.append("phone_nb", $('#c_phone_nb').val());
            formData.append("e_mail", $('#c_e_mail').val());
            formData.append("fax_nb", $('#c_fax_nb').val());
            formData.append("use_yn", $('#c_use_yn').val());
            formData.append("re_mark", $('#c_re_mark').val());

            $.ajax({
                type: 'POST',
                url: "/standard/InsertBuyer",
                data: formData,
                cache: false,
                contentType: false,
                processData: false,
                success: function (data) {
                    if (data.result == true) {
                        var id = data.value.byno;
                        $("#list").jqGrid('addRowData', id, data.value, 'first');
                        $("#list").setRowData(id, false, { background: "#d0e9c6" });
                        SuccessAlert(data.message);
                    }
                    else {
                        ErrorAlert(data.message);
                    }
                }
            });
        }
    });
    $("#m_save_but").click(function () {
        if ($("#form2").valid() == true) {
            var formData = new FormData();
         /*   var files = $("#imgupload2").get(0).files;*/

          /*  formData.append("file", files[0]);*/
            formData.append("buyer_cd", $('#m_buyer_cd').val());
            formData.append("byno", $('#m_byno').val());
            formData.append("buyer_nm", $('#m_buyer_nm').val());
            formData.append("brd_nm", $('#m_brd_nm').val());
            formData.append("web_site", $('#m_web_site').val());
            formData.append("cell_nb", $('#m_cell_nb').val());
            formData.append("address", $('#m_address').val());
            formData.append("phone_nb", $('#m_phone_nb').val());
            formData.append("e_mail", $('#m_e_mail').val());
            formData.append("fax_nb", $('#m_fax_nb').val());
            formData.append("use_yn", $('#m_use_yn').val());
            formData.append("re_mark", $('#m_re_mark').val());
            formData.append("logo", $('#m_logo1').val());

            $.ajax({
                type: "PUT",
                url: "/standard/Editbuyer",
                data: formData,
                cache: false,
                contentType: false,
                processData: false,
                success: function (data) {
                    if (data.result == true) {
                        var m_buyer_cd = $('#m_buyer_cd').val();
                        $('#remark_color').val(m_buyer_cd.toUpperCase());
                        jQuery("#list").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                        if (data.imgname != "") { $("#m_logo").html('<img src="../Images/BuyerImg/' + data.imgname + '" style="height:50px" />'); };
                        SuccessAlert(data.message);
                    }
                    else {
                        ErrorAlert(data.message);
                    }
                }
            });
        }
    });

    $('#del_save_but').click(function () {
        $('#dialogDangerous').dialog('open');
    });

    $("#searchBtn").click(function () {
        var sp_cd = $('#sp_cd').val().trim();
        var sp_nm = $('#sp_nm').val().trim();
        $.ajax({
            url: "/standard/searchBuyer",
            type: "get",
            dataType: "json",
            data: {
                buyer_cd: sp_cd,
                buyer_nm: sp_nm,
            },
            success: function (result) {
               $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
            },
             error: function (result) {
                alert('Not found');
            }
        });

    });

    function image_logo(cellValue, options, rowdata, action) {
        if (cellValue != null) {
            var html = '<img src="../Images/BuyerImg/' + cellValue + '" accept=".jpg, .png, .jpeg" style="height:20px;" />';
            return html;

        } else {
            return "";
        }
    }

    $("#tab_1").on("click", "a", function (event) {
        document.getElementById("form1").reset();
        $("#tab_2").removeClass("active");
        $("#tab_1").addClass("active");
        $("#tab_c2").removeClass("active");
        $("#tab_c1").addClass("active");
        $("#tab_c1").removeClass("hidden");
        $("#tab_c2").addClass("hidden");

    });
    $("#tab_2").on("click", "a", function (event) {

        document.getElementById("form2").reset();
        $("#tab_1").removeClass("active");
        $("#tab_2").addClass("active");
        $("#tab_c1").removeClass("active");
        $("#tab_c2").removeClass("hidden");
        $("#tab_c1").addClass("hidden");
        $("#tab_c2").addClass("active");
        $("#m_save_but").attr("disabled", true);
        $("#del_save_but").attr("disabled", true);
        $("#m_logo").empty();
    });


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
                   fileName: "Buyer Information  ",
                   returnAsString: false
               }
           );
    });

    $('#excelBtn').click(function (e) {
        jQuery("#list").jqGrid('hideCol', ["logoShow"]);
        jQuery("#list").jqGrid('showCol', ["logo"]);
        $("#list").jqGrid('exportToExcel',
            options = {
                includeLabels: true,
                includeGroupHeader: true,
                includeFooter: true,
                fileName: "Buyer Information.xlsx",
                mimetype: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                maxlength: 40,
                onBeforeExport: null,
                replaceStr: null
            }
        );
        jQuery("#list").jqGrid('showCol', ["logoShow"]);
        jQuery("#list").jqGrid('hideCol', ["logo"]);
    });
});

function ExportDataToExcel(tableCtrl) {
    //  Export the data from our jqGrid into a "real" Excel 2007 file
    ExportJQGridDataToExcel(tableCtrl, "CustomerOrders.xlsx");
}

$("#form1").validate({
    rules: {
        "buyer_cd": {
            required: true,
        },
        "buyer_nm": {
            required: true,
            maxlength: 50,
        },
        "brd_nm": {
            required: true,
            maxlength: 50,
        },

        "phone_nb": {
            digits: true,
        },
        "web_site": {
            url: false,
        },
        "cell_nb": {
            digits: true,
        },
        "e_mail": {
            email: true,
        },
        "fax_nb": {
            digits: true,
        },
    },
});

$("#form2").validate({
    rules: {
        "buyer_cd": {
            required: true,
        },
        "buyer_nm": {
            required: true,
            maxlength: 50,
        },
        "brd_nm": {
            required: true,
            maxlength: 50,
        },

        "phone_nb": {
            digits: true,
        },
        "web_site": {
            url: false,
        },
        "cell_nb": {
            digits: true,
        },
        "e_mail": {
            email: true,
        },
        "fax_nb": {
            digits: true,
        },
    },
});

//$('#excelBtn').click(function () {
//    //JSONToCSVConvertor(JSON.stringify($('#list').jqGrid('getRowData')), 'Title', true);
//    exportCSVFile(headers, $('#list').jqGrid('getRowData'), 'Title');
//});

//function convertToCSV(objArray) {
//        //If JSONData is not an object then JSON.parse will parse the JSON string in an Object
//    //var array = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;
//    var array = typeof objArray != 'object' ? JSON.parse(objArray) : objArray;
//    var str = '';

//    for (var i = 0; i < array.length; i++) {
//        var line = '';
//        for (var index in array[i]) {
//            if (line != '') line += ','

//            line += array[i][index];
//        }

//        str += line + '\r\n';
//    }

//    return str;
//}

//function exportCSVFile(headers, items, fileTitle) {
//    if (headers) {
//        items.unshift(headers);
//    }

//    // Convert Object to JSON
//    var jsonObject = JSON.stringify(items);

//    var csv = this.convertToCSV(jsonObject);

//    var exportedFilenmae = fileTitle + '.csv' || 'export.csv';

//    var blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
//    if (navigator.msSaveBlob) { // IE 10+
//        navigator.msSaveBlob(blob, exportedFilenmae);
//    } else {
//        var link = document.createElement("a");
//        if (link.download !== undefined) { // feature detection
//            // Browsers that support HTML5 download attribute
//            var url = URL.createObjectURL(blob);
//            link.setAttribute("href", url);
//            link.setAttribute("download", exportedFilenmae);
//            link.style.visibility = 'hidden';
//            document.body.appendChild(link);
//            link.click();
//            document.body.removeChild(link);
//        }
//    }
//};





//function JSONToCSVConvertor(JSONData, ReportTitle, ShowLabel) {
//    //If JSONData is not an object then JSON.parse will parse the JSON string in an Object
//    var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;

//    var CSV = '';
//    //Set Report title in first row or line

//    CSV += ReportTitle + '\r\n\n';

//    //This condition will generate the Label/Header
//    if (ShowLabel) {
//        var row = "";

//        //This loop will extract the label from 1st index of on array
//        for (var index in arrData[0]) {

//            //Now convert each value to string and comma-seprated
//            row += index + ',';
//        }

//        row = row.slice(0, -1);

//        //append Label row with line break
//        CSV += row + '\r\n';
//    }

//    //1st loop is to extract each row
//    for (var i = 0; i < arrData.length; i++) {
//        var row = "";

//        //2nd loop will extract each column and convert it in string comma-seprated
//        for (var index in arrData[i]) {
//            row += '"' + arrData[i][index] + '",';
//        }

//        row.slice(0, row.length - 1);

//        //add a line break after each row
//        CSV += row + '\r\n';
//    }

//    if (CSV == '') {
//        alert("Invalid data");
//        return;
//    }

//    //Generate a file name
//    var fileName = "MyReport_";
//    //this will remove the blank-spaces from the title and replace it with an underscore
//    fileName += ReportTitle.replace(/ /g, "_");

//    //Initialize file format you want csv or xls
//    var uri = 'data:text/csv;charset=utf-8,' + escape("\uFEFF"+CSV);

//    // Now the little tricky part.
//    // you can use either>> window.open(uri);
//    // but this will not work in some browsers
//    // or you will not get the correct file extension

//    //this trick will generate a temp <a /> tag
//    var link = document.createElement("a");
//    link.href = uri;

//    //set the visibility hidden so it will not effect on your web-layout
//    link.style = "visibility:hidden";
//    link.download = fileName + ".csv";

//    //this part will append the anchor tag and remove it after automatic click
//    document.body.appendChild(link);
//    link.click();
//    document.body.removeChild(link);
//}
