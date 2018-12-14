var InsCompany = function () {
    "use strict"
    return {
        init: function () {

        },

        onSuccess: function (jqXHR, settings) {
            debugger
            $.msgbox("Transacción realizada satisfactoriamente", { type: "success" });
        },

        validateForm: function () {
            debugger
            alert("hola");
        },
    }
}();

//function onSuccess() {
//    debugger
//    $.msgbox("Transacción realizada satisfactoriamente", { type: "success" });
//}