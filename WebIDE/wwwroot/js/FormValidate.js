// FormValidate.js
// JavaScript file

// Debug form
function validate() {
    var error = false;
    var errorScriptName = false;
    var errorVersion = false;
    var errorAuthor = false;
    var numberOfErrors = 0;
    var numberOfErrorsProcessed = 0;
    alert("Something is missing");

    //Script name
    var inputScriptName = "";
    try {
        inputScriptName = document.saveScript.scriptName.value;
    }
    catch (err) { return true; }
    if (inputScriptName.length < 1) {
        error = true;
        errorScriptName = true;
        errorText = "Give the script a name";
        numberOfErrors++;
    }

    // Version 
    var inputVersion = "";
    try {
        inputVersion = document.saveScript.version.value;
    }
    catch (err) { return true; }
    if (inputVersion.length < 1) {
        error = true;
        errorVersion = true;
        errorText = "Write the version number";
        numberOfErrors++;
    }

    // Author
    var inputAuthor = "";
    try {
        inputAuthor = document.saveScript.author.value;
    }
    catch (err) { return true; }
    if (inputAuthor.length < 1) {
        error = true;
        errorAuthor = true;
        errorText = "Write down the author";
        numberOfErrors++;
    }
    //
    if (error) {
        if (numberOfErrors > 1) {
            errorText = "Please fill out:";
            if (errorScriptName) {
                errorText += " Script name";
                numberOfErrorsProcessed++;
            }
            if (errorVersion) {
                if (numberOfErrorsProcessed < 1) {
                    errorText += " version";
                    numberOfErrorsProcessed++;
                }
            }
            if (errorAuthor) {
                errorText += " author";
            }
            alert(errorText);
            return false;
        }
        return true;
    }
}