using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using NetworkExtension;
using NFCReaderTools.iOS.Services;
using NFCReaderTools.Services;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(WifiService))]
namespace NFCReaderTools.iOS.Services
{
    public class WifiService : IWifiService
    {
        public bool Connect(string ssid, string password)
        {
            bool ret = false;
            try
            {
                var wifiManager = new NEHotspotConfigurationManager();
                var wifiConfig = new NEHotspotConfiguration(ssid, password, false);
                wifiManager.ApplyConfiguration(wifiConfig, (error) =>
                {
                    ret = error == null;
                });
            }
            catch (Exception ex) { }
            return ret;
        }
    }
}