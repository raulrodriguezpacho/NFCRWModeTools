using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.NFCWriterFeature
{
    public interface INFCWriterFeature
    {
        object Adapter { get; }
        bool? Verify();
        bool WriteAbsoluteUri(string uri, object tag);
        void WriteEmpty();
        bool WriteExternalType(string pathPrefix, string payload, object tag);
        bool WriteMimeMedia(string mimeType, byte[] mimeData, object tag);
        void WriteUnchanged();
        void WriteUnknown();
        bool WriteWellKnown(RecordTypeDefinition type, string content, object tag);
        bool WriteApplication(string packageName, object tag);
    }
}
