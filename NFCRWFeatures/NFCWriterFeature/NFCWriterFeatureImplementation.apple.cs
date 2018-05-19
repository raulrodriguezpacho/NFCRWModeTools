using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.NFCWriterFeature
{
    /// <summary>
    /// Interface for $safeprojectgroupname$
    /// </summary>
    public class NFCWriterFeatureImplementation : INFCWriterFeature
    {
        public object Adapter => throw new NotImplementedException();

        public bool? Verify()
        {
            throw new NotImplementedException();
        }

        public bool WriteAbsoluteUri(string uri, object tag)
        {
            throw new NotImplementedException();
        }

        public bool WriteApplication(string packageName, object tag)
        {
            throw new NotImplementedException();
        }

        public void WriteEmpty()
        {
            throw new NotImplementedException();
        }

        public bool WriteExternalType(string pathPrefix, string payload, object tag)
        {
            throw new NotImplementedException();
        }

        public bool WriteMimeMedia(string mimeType, byte[] mimeData, object tag)
        {
            throw new NotImplementedException();
        }

        public void WriteUnchanged()
        {
            throw new NotImplementedException();
        }

        public void WriteUnknown()
        {
            throw new NotImplementedException();
        }

        public void WriteWellKnown()
        {
            throw new NotImplementedException();
        }
    }
}
