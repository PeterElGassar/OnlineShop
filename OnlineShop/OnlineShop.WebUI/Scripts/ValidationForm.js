

$("#editeForm, #createForm").on("submit", function () {
    var returnVal = true;
    if ($("#Product_Name").val().trim() === "") {
        //$("#Product_Name").next('span').text("please Enter Product Name").show();
        alert("please Enter Product Name");
        returnVal = false;
    }

    else if ($("#Product_Description").val().trim() === "") {
        alert("please Enter Product Description");
        returnVal = false;
    }

    else if ($("#Product_Price").val().trim() === "" || $("#Product_Price").val() === 0.00) {
        alert("please Enter Product price");
        returnVal = false;
    }

    else if ($("#Product_ProductCategoryId").val() === "") {
        alert("please Enter Product Category");
        returnVal = false;
    }
    //else if (!$("#file").files || !$("#file").files[0]) {
    //    alert("please Enter Product Category");
    //    returnVal = false;
    //}




    //Final Result
    if (!returnVal) {
        event.preventDefault();
    }
});

//$("#createForm").submit(function (event) {

//    var returnVal = true;
//    if ($("#Product_Name").val().trim() === "") {
//        debugger;
//        //$("#Product_Name").next('span').text("please Enter Product Name").show();
//        alert("please Enter Product Name");
//        returnVal = false;
//    }

//    else if ($("#Product_Description").val().trim() === "") {
//        alert("please Enter Product Description");
//        returnVal = false;
//    }

//    else if ($("#Product_Price").val().trim() === "" || $("#Product_Price").val() === 0.00) {
//        alert("please Enter Product price");
//        returnVal = false;
//    }

//    else if ($("#Product_ProductCategoryId").val() === "" || $("#Product_ProductCategoryId").val() === null) {
//        alert("please Enter Product Category");
//        returnVal = false;
//    }
//    else if ($("#file").val() === "" || $("#file").cont() === null) {
//        alert("please Enter Product Category");
//        returnVal = false;
//    }




//    //Final Result
//    if (!returnVal) {
//        event.preventDefault();
//    }
//});

//function validationUser(event) {
//    var returnVal = true;

//    if ($("#Product_Name").val().trim() === "") {
//        debugger;
//        $("#Product_Name").next('span').text("please Enter Product Name").show();
//        returnVal= false;
//    } else {
//        $("#Product_Name").next('span').hide();
//    }


//    //Final Result 
//    if (!returnVal) {
//        event.preventDefault();
//    }

//}