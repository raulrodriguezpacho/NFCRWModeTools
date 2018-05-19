using Android.Nfc;
using Android.Nfc.Tech;
using Plugin.CurrentActivity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.NFCWriterFeature
{
    /// <summary>
    /// Interface for $safeprojectgroupname$
    /// </summary>
    public class NFCWriterFeatureImplementation : NDEFWriterBase, INFCWriterFeature
    {
        private object _nfcAdapter;
        public object Adapter
        {
            get
            {
                return _nfcAdapter;
            }
        }

        public override WriteResult WriteTag(NdefMessage message, Tag tag)
        {
            if (tag == null)
                return WriteResult.NULL;
            
            try
            {
                var ndefTag = Ndef.Get(tag);                
                if (ndefTag == null)
                {
                    NdefFormatable nForm = NdefFormatable.Get(tag);                    
                    if (nForm != null)
                    {
                        try
                        {
                            nForm.Connect();
                            nForm.Format(message);
                            nForm.Close();
                            return WriteResult.OK;
                        } catch { return WriteResult.FORMATFAILED; }
                    }
                    return WriteResult.NOTSUPPORTED;
                }
                else
                {
                    ndefTag.Connect();
                    if (ndefTag.MaxSize < message.ToByteArray().Length)
                        return WriteResult.TOOLARGE;
                    if (ndefTag.IsWritable)
                    {                        
                        ndefTag.WriteNdefMessage(message);
                        ndefTag.Close();
                        return WriteResult.OK;
                    }
                    return WriteResult.READONLY;
                }
            }
            catch { return WriteResult.FAILED; }
        }

        public bool? Verify()
        {
            bool? result = null;
            try
            {
                _nfcAdapter = NfcAdapter.GetDefaultAdapter(CrossCurrentActivity.Current.AppContext);
                if (_nfcAdapter != null && _nfcAdapter is NfcAdapter)
                    result = ((NfcAdapter)_nfcAdapter).IsEnabled;
            }
            catch { }
            return result;
        }

        public bool WriteAbsoluteUri(string uri, object tag)
        {
            if (!(tag is Tag))
                return false;
            if (string.IsNullOrEmpty(uri))
                return false;

            WriteResult writeResult = WriteResult.FAILED;
            try
            {
                NdefRecord uriRecord = new NdefRecord(
                    (short)TypeNameFormat.TNF_ABSOLUTE_URI,
                    Encoding.ASCII.GetBytes(uri),
                    new byte[0],
                    new byte[0]);
                NdefMessage ndefMessage = new NdefMessage(new NdefRecord[] { uriRecord });
                writeResult = WriteTag(ndefMessage, (Tag)tag);
            }
            catch { }
            return writeResult == WriteResult.OK;
        }

        public void WriteEmpty()
        {
            throw new NotImplementedException();
        }

        public bool WriteExternalType(string pathPrefix, string payload, object tag)
        {
            if (!(tag is Tag))
                return false;
            if (string.IsNullOrEmpty(pathPrefix) || string.IsNullOrEmpty(payload))
                return false;

            WriteResult writeResult = WriteResult.FAILED;
            try
            {
                NdefRecord extRecord = new NdefRecord(
                    (short)TypeNameFormat.TNF_EXTERNAL_TYPE,
                    Encoding.UTF8.GetBytes(pathPrefix),
                    new byte[0],
                    Encoding.UTF8.GetBytes(payload));
                NdefMessage ndefMessage = new NdefMessage(new NdefRecord[] { extRecord });
                writeResult = WriteTag(ndefMessage, (Tag)tag);
            }
            catch { }
            return writeResult == WriteResult.OK;
        }

        public bool WriteMimeMedia(string mimeType, byte[] mimeData, object tag)
        {
            if (!(tag is Tag))
                return false;
            if (string.IsNullOrEmpty(mimeType) || mimeData.Length == 0)
                return false;

            WriteResult writeResult = WriteResult.FAILED;
            try
            {
                NdefRecord mimeRecord = NdefRecord.CreateMime(mimeType, mimeData);
                NdefMessage ndefMessage = new NdefMessage(new NdefRecord[] { mimeRecord });
                writeResult = WriteTag(ndefMessage, (Tag)tag);
            }
            catch { }
            return writeResult == WriteResult.OK;
        }

        public void WriteUnchanged()
        {
            throw new NotImplementedException();
        }

        public void WriteUnknown()
        {
            throw new NotImplementedException();
        }

        public bool WriteWellKnown(RecordTypeDefinition type, string content, object tag)
        {
            if (!(tag is Tag))
                return false;
            if (string.IsNullOrEmpty(content))
                return false;

            WriteResult writeResult = WriteResult.FAILED;
            NdefMessage ndefMessage = null;
            try
            {
                switch (type)
                {
                    case RecordTypeDefinition.RTD_TEXT:

                        break;
                    case RecordTypeDefinition.RTD_URI:
                        NdefRecord rtdUriRecord = NdefRecord.CreateUri("http://www.raulrodriguezpacho.com");
                        ndefMessage = new NdefMessage(new NdefRecord[] { rtdUriRecord });
                        break;
                }
                if (ndefMessage == null)
                    return false;
                writeResult = WriteTag(ndefMessage, (Tag)tag);
            }
            catch { }
            return writeResult == WriteResult.OK;
        }

        public bool WriteApplication(string packageName, object tag)
        {
            if (!(tag is Tag))
                return false;
            if (string.IsNullOrEmpty(packageName))
                return false;

            WriteResult writeResult = WriteResult.FAILED;
            try
            {
                NdefRecord packageRecord = NdefRecord.CreateApplicationRecord(packageName);
                NdefMessage ndefMessage = new NdefMessage(new NdefRecord[] { packageRecord });
                writeResult = WriteTag(ndefMessage, (Tag)tag);
            }
            catch { }
            return writeResult == WriteResult.OK;
        }
    }

    public abstract class NDEFWriterBase
    {
        public virtual WriteResult WriteTag(NdefMessage message, Tag tag) { return WriteResult.NULL; }
    }    
}
