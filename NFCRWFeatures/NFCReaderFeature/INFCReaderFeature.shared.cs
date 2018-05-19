using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.NFCReaderFeature
{
    public interface INFCReaderFeature
    {
        object Adapter { get; }
        bool? Verify();
    }
}
