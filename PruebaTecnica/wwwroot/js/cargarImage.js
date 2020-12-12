function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            // Asignamos el atributo src a la tag de imagen
            $('#imgPreview').attr('src', e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    } else {

        $('#imgPreview').attr('src', '~/img/user.jpg');
    }
}

// El listener va asignado al input
$("#files").change(function () {
    readURL(this);
});