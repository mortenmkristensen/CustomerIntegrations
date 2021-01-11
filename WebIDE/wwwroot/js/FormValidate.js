// FormValidate.js
// JavaScript file

// Debug form
function validate() {
    var error = false;
    var errorId = false;
    var errorScriptName = false;
    var errorLanguage = false;
    var errorVersion = false;
    var errorAuthor = false;
    var numberOfErrors = 0;
    var numberOfErrorsProcessed = 0;

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

    //Script language
    var inputLanguage = "";
    try {
        inputLanguage = document.saveScript.language.value;
    }
    catch (err) { return true; }
    if (inputLanguage.length < 1) {
        error = true;
        errorLanguage = true;
        errorText = "Select a script language from the dropdown list";
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

    if (error) {
        if (numberOfErrors > 1) {
            errorText = "Please fill out:";
            if (errorScriptName) {
                if (numberOfErrorsProcessed < 1) {
                    errorText += " Script name";
                } else {
                    errorText += ", Script name";
                }
                numberOfErrorsProcessed++;
            }
            if (errorLanguage) {
                if (numberOfErrorsProcessed < 1) {
                    errorText += " Language";
                } else {
                    errorText += ", Language";
                }
                numberOfErrorsProcessed++;
            }
            if (errorVersion) {
                if (numberOfErrorsProcessed < 1) {
                    errorText += " Version";
                } else {
                    errorText += ", Version";
                }
                numberOfErrorsProcessed++;
            }
            if (errorAuthor) {
                errorText += ", Author";
            }
        }
        alert(errorText);
        return false;
    }
    return true;
}