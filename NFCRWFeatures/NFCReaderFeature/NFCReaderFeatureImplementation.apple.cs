using CoreNFC;
using Foundation;
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
        public object Adapter => throw new NotImplementedException();        

        public bool? Verify()
        {
            return NFCNdefReaderSession.ReadingAvailable;
        }
    }  
}
