using System;
using System.IO;

namespace Unitronics.Utils
{
    public class FileLogger
    {
        private readonly FileLoggerSettings _settings;
        private readonly object _lockObject=new object();

        public FileLogger(FileLoggerSettings settings)
        {
            _settings = settings;
        }

        public  void Log(byte[] data, string operation)
        {
          
            if (string.IsNullOrWhiteSpace(_settings.FileName)) return;
            lock (_lockObject)
            {
                File.AppendAllText(_settings.FileName, Environment.NewLine + operation + Environment.NewLine + Hex.Dump(data,
                                                           _settings.BytesPerLine,
                                                           _settings.ShowHeader,
                                                           _settings.ShowOffset,
                                                           _settings.ShowAscii) + Environment.NewLine);                
            }

        }
    }
}
