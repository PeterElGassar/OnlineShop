
var inputFile = document.getElementById("file");
var ImagePreview = document.querySelector(".imagePreview");

inputFile.addEventListener("change", function (e) {
    debugger;
    var $this = this;

    if ($this.files && $this.files[0]) {

        const reader = new FileReader();
        reader.onload = function () {
            ImagePreview.setAttribute("src",reader.result)
        }
        reader.readAsDataURL($this.files[0]);

    }
});