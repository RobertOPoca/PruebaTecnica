var busqueda = document.getElementById('buscar');
var table = document.getElementById("tb").tBodies[0];



$("#buscar").change(function () {
    var valor = $(this).val();
    var r = 0;
    if (valor == "Todas") {
        while (row = table.rows[r++]) {
            
             row.style.display = null;
        }

    } else {
        var texto = valor.toLowerCase();
        while (row = table.rows[r++]) {
            if (row.innerText.toLowerCase().indexOf(texto) !== -1)
                row.style.display = null;
            else
                row.style.display = 'none';
        }
    }

    
});