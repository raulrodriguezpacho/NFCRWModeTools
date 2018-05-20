using System;
using System.Collections.Generic;
using System.Text;

namespace NFCReaderTools.Services
{
    public interface IWifiService
    {
        bool Connect(string ssid, string password);
    }
}
