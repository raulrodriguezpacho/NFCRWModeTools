using System;
using System.Collections.Generic;
using System.Text;

namespace NFCWriterTools.Services
{
    public interface INFCWriter
    {
        void WriteAbsoluteUri(string uri);
        void WriteMimeMedia(string mimeType, byte[] mimeData);
        void WriteApplication(string packageName);
    }
}
