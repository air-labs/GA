using System;

namespace AIRLab.Thornado {
    public class WrongFormatException : Exception {
        public string Value = string.Empty;

        public WrongFormatException(string value) {
            Value = value;
        }

        public override string Message {
            get { return string.Format("Неверный формат '{0}'", Value); }
        }
    }
}