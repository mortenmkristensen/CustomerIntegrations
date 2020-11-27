using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Exceptions {
    class ScriptFailedException : Exception{

        public string ScriptId { get; }
        public ScriptFailedException() {
        }
        public ScriptFailedException(string message): base(message) {
        }
        public ScriptFailedException(string message, Exception inner) : base(message, inner) {
        }
        public ScriptFailedException(string scriptId, string message) : this(message) {
            ScriptId = scriptId;
        }
        public ScriptFailedException(string scriptId, string message, Exception inner) : this(message, inner) {
            ScriptId = scriptId;
        }
    }
}
