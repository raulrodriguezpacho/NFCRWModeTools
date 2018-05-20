using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Net.Wifi;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using NFCReaderTools.Droid.Services;
using NFCReaderTools.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(WifiService))]
namespace NFCReaderTools.Droid.Services
{
    public class WifiService : IWifiService
    {
        public bool Connect(string ssid, string password)
        {
            bool ret = false;
            try
            {
                var wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);
                var formattedSsid = $"\"{ssid}\"";
                var formattedPassword = $"\"{password}\"";
                var wifiConfig = new WifiConfiguration
                {
                    Ssid = formattedSsid,
                    PreSharedKey = formattedPassword
                };               
                var network = wifiManager.ConfiguredNetworks.FirstOrDefault(n => n.Ssid == formattedSsid);
                if (network == null)
                { 
                    var addNetwork = wifiManager.AddNetwork(wifiConfig);
                }                    
                wifiManager.Disconnect();
                var enableNetwork = wifiManager.EnableNetwork(network.NetworkId, true);
                ret = true;
            }
            catch (Exception ex) { }            
            return ret;
        }
    }
}