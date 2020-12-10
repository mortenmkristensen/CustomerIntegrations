function validate() {
    var error = false;
    var errorText = "";
    var errorName = false;
    var numberOfErrors = 0;
    var numberOfErrorsProcessed = 0;
    // name
    var inputName = "";
    try {
        inputName = document.TableCreate.tableName.value;
    }
    catch (err) { return true; }
    if (inputName.length < 1) {
        error = true;
        errorName = true;
        errorText = "Please give the game table a name!";
        numberOfErrors++;
    }
    if (error) {
        if (numberOfErrors > 1) {
            errorText = "Specify a name:";
            if (errorName) {
                errorText += " Name";
                numberOfErrorsProcessed++;
            }
        }
        alert(errorText);
        return false;
    }
    return true;
}