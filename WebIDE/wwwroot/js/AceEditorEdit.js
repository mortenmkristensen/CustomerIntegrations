
var editor = ace.edit("editor");
var textarea = $('#editorContent');
editor.getSession().on('change', function () {
    textarea.val(editor.getSession().getValue());
});

var editor = ace.edit("editor");
editor.setTheme("ace/theme/xcode");
var input = document.getElementById("language");
input.onchange = function () {
    var newMode = input.getValue();
    editor.getSession().setMode("ace/mode/" + newMode);
}