namespace Models {
    public class Script {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Customer { get; set; }
        public double ScriptVersion { get; set; }
        public string Language { get; set; }
        public string LanguageVersion { get; set; }
        public string Code { get; set; }
        public string LastResult { get; set; }
        public bool HasErrors { get; set; }
    }
}
