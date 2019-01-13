// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var canvas  = $("#canvas"),
    context = canvas.get(0).getContext("2d"),
    $result = $('#result');

$('#fileInput').on( 'change', function(){
    if (this.files && this.files[0]) {
      if ( this.files[0].type.match(/^image\//) ) {
        var reader = new FileReader();
        reader.onload = function(evt) {
           var img = new Image();
           img.onload = function() {
             context.canvas.height = img.height;
             context.canvas.width  = img.width;
             context.drawImage(img, 0, 0);
             //var cropper = canvas.cropper({
             //  aspectRatio: 16 / 9
             //});
             //$('#btnCrop').click(function() {
             //   // Get a string base 64 data url
             //   var croppedImageDataURL = canvas.cropper('getCroppedCanvas').toDataURL("image/png"); 
             //   $result.append( $('<img>').attr('src', croppedImageDataURL) );
             //});
             //$('#btnRestore').click(function() {
             //  canvas.cropper('reset');
             //  $result.empty();
             //});
           };
           img.src = evt.target.result;
				};
        reader.readAsDataURL(this.files[0]);
      }
      else {
        alert("Invalid file type! Please select an image file.");
      }
    }
    else {
      alert('No file(s) selected.');
    }
});

// 

function Process() {

    var input = document.querySelector('input[type="file"]');

    var data = new FormData();
    data.append('file', input.files[0]);
    data.append('user', 'hubot');

    fetch(window.location.origin + "/api/Image/", {
        method: 'POST',
        body: data
    }).then(res => res.json())//response type
        .then(function (data) {
            console.log(data);
            var ul = document.getElementById("findings");

            for (var i = 0; i < data.length; i++) {
                o = data[i];
                var li = document.createElement("li");
                li.appendChild(document.createTextNode(o));
                ul.appendChild(li);
            }

        });

}

