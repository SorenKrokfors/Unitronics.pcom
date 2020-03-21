namespace Unitronics.Utils
{
    public class FileLoggerSettings
    {
        public string FileName { get; set; } = string.Empty;
        public int BytesPerLine { get; set; } = 16;
        public bool ShowHeader { get; set; } = true;
        public bool ShowOffset { get; set; } = true;
        public bool ShowAscii { get; set; } = true;
    }
}
