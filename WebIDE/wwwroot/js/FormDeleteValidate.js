function validate4() {
    var error = false;
    var errorText = "";
    var errorId = false;
    var numberOfErrors = 0;
    // Id
    var inputId = "";
    try {
        inputId = document.delete.scriptID4.value;
    }
    catch (err) { return true; }
    if (inputId.length < 1) {
        error = true;
        errorId = true;
        errorText = "Please input a script ID!";
        alert(errorText);
        return false;
    }
    if (error) {
        if (numberOfErrors > 1) {
            errorText = "Specify a Id:";
            if (errorId) {
                errorText += " Id";
            }
        }
       
    }
    return true;
}