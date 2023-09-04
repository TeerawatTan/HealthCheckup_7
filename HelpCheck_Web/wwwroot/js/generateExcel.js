const downloadXLS = () => {

    var createXLSLFormatObj = [];

    /* XLS Head Columns */
    var xlsHeader = ["EmployeeID", "Full Name"];

    /* XLS Rows Data */
    var xlsRows = [
        {
            "EmployeeID": "EMP001",
            "FullName": "Jolly"
        },
        {
            "EmployeeID": "EMP002",
            "FullName": "Macias"
        },
        {
            "EmployeeID": "EMP003",
            "FullName": "Lucian"
        },
        {
            "EmployeeID": "EMP004",
            "FullName": "Blaze"
        },
        {
            "EmployeeID": "EMP005",
            "FullName": "Blossom"
        },
        {
            "EmployeeID": "EMP006",
            "FullName": "Kerry"
        },
        {
            "EmployeeID": "EMP007",
            "FullName": "Adele"
        },
        {
            "EmployeeID": "EMP008",
            "FullName": "Freaky"
        },
        {
            "EmployeeID": "EMP009",
            "FullName": "Brooke"
        },
        {
            "EmployeeID": "EMP010",
            "FullName": "FreakyJolly.Com"
        }
    ];


    createXLSLFormatObj.push(xlsHeader);
    $.each(xlsRows, function (index, value) {
        var innerRowData = [];
        $("tbody").append('<tr><td>' + value.EmployeeID + '</td><td>' + value.FullName + '</td></tr>');
        $.each(value, function (ind, val) {

            innerRowData.push(val);
        });
        createXLSLFormatObj.push(innerRowData);
    });


    /* File Name */
    var filename = "FreakyJSON_To_XLS.xlsx";

    /* Sheet Name */
    var ws_name = "FreakySheet";

    if (typeof console !== 'undefined') console.log(new Date());
    var wb = XLSX.utils.book_new(),
        ws = XLSX.utils.aoa_to_sheet(createXLSLFormatObj);

    /* Add worksheet to workbook */
    XLSX.utils.book_append_sheet(wb, ws, ws_name);

    /* Write workbook and Download */
    if (typeof console !== 'undefined') console.log(new Date());
    XLSX.writeFile(wb, filename);
    if (typeof console !== 'undefined') console.log(new Date());
}