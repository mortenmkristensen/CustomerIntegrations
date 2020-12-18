function validate2() {
    var error = false;
    var errorText = "";
    var errorId = false;
    var numberOfErrors = 0;

    // Id
    var inputId = "";
    try {
        inputId = document.openScriptState.scriptID2.value;
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
            }
        }
        alert(errorText);
        return false;
    }
    return true;
}