function validate() {
    var error = false;
    var errorId = false;

    //Search script by ID
    var scriptId = null;
    try {
        scriptId = document.search.scriptID.value;
    }
    catch (err) { return true; }
    if (scriptId.length < 1 || scriptId == null) {
        error = true;
        errorId = true;
        errorText = "Provide an ID";

        alert(errorText);
        return false;
    }
    return true;
}
