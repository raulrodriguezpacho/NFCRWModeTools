using Android.Nfc;
using Plugin.CurrentActivity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.NFCReaderFeature
{
    /// <summary>
    /// Interface for $safeprojectgroupname$
    /// </summary>
    public class NFCReaderFeatureImplementation : INFCReaderFeature
    {
        private object _nfcAdapter;
        public object Adapter
        {
            get
            {
                return _nfcAdapter;
            }
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
    }
}
