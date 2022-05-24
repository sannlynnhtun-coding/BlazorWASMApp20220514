var datePicker2 = function (id) {
    console.log('test datePicker2');
    //$('[data-toggle="datepicker"]').datepicker();
    $(id).datepicker({
        format: 'dd-mm-yyyy'
    });
}