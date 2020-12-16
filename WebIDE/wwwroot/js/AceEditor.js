var editor = ace.edit("editor");
var textarea = $('#editorContent');
editor.getSession().on('change', function () {
    textarea.val(editor.getSession().getValue());
});

var editor = ace.edit("editor");
editor.setTheme("ace/theme/xcode");
var box = document.getElementById("box1")
box.onchange = function () {
    var newMode = box.options[box.selectedIndex].text;
    document.getElementById("language").value = newMode;
    editor.getSession().setMode("ace/mode/" + newMode);
    if (newMode == ("javascript")) {
        editor.setValue("//This is the datamodel for Location, Source, and State." + "\n" +
            "let location = {" + "\n" +
            "   Id:" + '\u0022' + "" + '\u0022' + "," + "\n" +
            "   ParentId:" + '\u0022' + "" + '\u0022' + "," + "\n" +
            "   ExternalId:" + '\u0022' + "" + '\u0022' + "," + "\n" +
            "   customerId: 1," + "\n" +
            "   Sources: []" + "\n" +
            "};" + "\n" +

            "let source = {" + "\n" +
            "   Type:" + '\u0022' + "" + '\u0022' + "," + "\n" +
            "   TimeStamp:" + '\u0022' + "" + '\u0022' + "\n" +
            "};" + "\n" +

            "let state = {" + "\n" +
            "   Property:" + '\u0022' + "" + '\u0022' + "," + "\n" +
            "   Value:" + '\u0022' + "" + '\u0022' + "\n" +
            "};"
        );
    }
    if (newMode == ("ruby")) {
        editor.setValue("#This is the datamodel for Location, Source, and State." + "\n" + "class Location" + "\n" +
            " attr_writer :Id,:ParentId,:ExternalId,:ConsumerId,:Sources" + "\n" +
            " attr_reader :Id,:ParentId,:ExternalId,:ConsumerId,:Sources" + "\n" +

            " def to_json(*a)" + "\n" +
            "  { 'Id' => @@Id, 'ParentId' => @@ParentId, 'ExternalId' => @@ExternalId, 'ConsumerId'=> @@ConsumerId, 'Sources' => @@Sources}.to_json" + "\n" +
            " end" + "\n" +

            " def self.from_json string" + "\n" +
            "  data = JSON.load string" + "\n" +
            "  self.new data['Id'], data['ParentId'], data['ExternalId'], data['ConsumerId'], data['Sources']" + "\n" +
            " end" + "\n" +
            "end" + "\n" + "\n" +

            "class Source" + "\n" +
            " attr_writer :Type,:State,:TimeStamp" + "\n" +
            " attr_reader :Type,:State,:TimeStamp" + "\n" +

            " def to_json(*a)" + "\n" +
            "  { 'Type' => @@Type, 'State' => @@State, 'TimeStamp' => @@TimeStamp}.to_json" + "\n" +
            " end" + "\n" +

            " def self.from_json string" + "\n" +
            "  data = JSON.load string" + "\n" +
            "  self.new data['Type'], data['State'], data['TimeStamp']" + "\n" +
            " end" + "\n" +
            "end" + "\n" + "\n" +

            "class State" + "\n" +
            " attr_writer :Property,:Value" + "\n" +
            " attr_reader :Property,:Value" + "\n" +

            " def to_json(*a)" + "\n" +
            "  { 'Property' => @@Property, 'Value' => @@Value}.to_json" + "\n" +
            " end" + "\n" +

            " def self.from_json string" + "\n" +
            "  data = JSON.load string" + "\n" +
            "  self.new data['Property'], data['Value']" + "\n" +
            " end" + "\n" +

            "end"
        );
    }
    if (newMode == ("python")) {
        editor.setValue("#This is the datamodel for Location, Source, and State." + "\n" +
            "location = { 'Id':" + '\u0022' + "" + '\u0022' + "," + "'ParentId':" + '\u0022' + "" + '\u0022' + "," + "'ExternalId':" + '\u0022' + "" + '\u0022' + "," + "'ConsumerId': 2, 'Sources': [] }" + "\n" +
            "source = { 'Type':" + '\u0022' + "" + '\u0022' + "," + "'TimeStamp':" + '\u0022' + "" + '\u0022' + "," + "'State': [] }" + "\n" +
            "state = { 'Property':" + '\u0022' + "" + '\u0022' + "," + "'Value':" + '\u0022' + "" + '\u0022' + "," + "}"
        );
    }
}