function table_facline_qc_value() {
    $("#list_facline_qc_value").jqGrid
        ({
            url: "/TIMS/Getfacline_qc_value",
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'fqhno', name: 'fqhno', width: 80, align: 'center', hidden: true },
                { key: false, label: 'Subject', name: 'check_subject', sortable: true, width: '150', align: 'left' },
                { key: false, label: 'Content', name: 'check_value', sortable: true, width: '110', align: 'left' },
                {
                    key: false, label: 'Defect Qty', name: 'check_qty', sortable: true, align: 'right', width: '90', editable: false,
                    editoptions: { size: 10, maxlength: 15 }, editrules: { integer: true, required: true },
                    formatter: 'integer', editoptions: {
                        dataInit: function (element) {
                            $(element).keypress(function (e) {
                                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                                    return false;
                                }
                            });
                        }
                    },
                },
                { key: false, label: 'Date', name: 'date_ymd', sortable: true, width: '90', align: 'center' },
            ],

            onCellSelect: editRow_facline,
            onSelectRow: function (rowid, selected, status, e) {
                $("#p_SaveQcfacline").attr("disabled", false);
            },

            viewrecords: true,
            rowList: [50, 100, 200, 500],
            height: "100%",
            width: null,
            rowNum: 50,
            loadtext: "Loading...",
            emptyrecords: "No data.",
            rownumbers: false,
            gridview: true,
            shrinkToFit: false,
            loadonce: false,
            viewrecords: true,
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
        })
}
table_facline_qc_value();
//delete Qc value 
function FormatDelQc_value(cellValue, options, rowdata, action) {
    var id = rowdata.wmtid;
    html = '<button  class="btn btn-sm btn-danger button-srh"  data-id="' + rowdata.fqhno + '"onclick="Click_DelQC_value(this);">X</button>';
    return html;
}
var lastSel;
function editRow_facline(id) {
    if (id && id !== lastSel) {
        var grid = $("#list_facline_qc_value");
        grid.jqGrid('editRow', id, { keys: true, focusField: 2 });
        lastSel = id;
    }
}