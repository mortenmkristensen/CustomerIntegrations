function validate() {
    var error = false;
    var errorText = "";
    var errorId = false;
    var numberOfErrors = 0;
    var numberOfErrorsProcessed = 0;
    // name
    var inputId = "";
    try {
        inputId = document.delete.scriptID4.value;
    }
    catch (err) { return true; }
    if (inputId.length < 1) {
        error = true;
        errorId = true;
        errorText = "Please give the script ID!";
        numberOfErrors++;
    }
    if (error) {
        if (numberOfErrors > 1) {
            errorText = "Specify a Id:";
            if (errorId) {
                errorText += " Id";
                numberOfErrorsProcessed++;
            }
        }
        alert(errorText);
        return false;
    }
    return true;
}