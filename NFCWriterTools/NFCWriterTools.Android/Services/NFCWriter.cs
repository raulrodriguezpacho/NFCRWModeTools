using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using NFCWriterTools.Droid.Services;
using NFCWriterTools.Services;
using Plugin.NFCWriterFeature;

[assembly: Xamarin.Forms.Dependency(typeof(NFCWriter))]
namespace NFCWriterTools.Droid.Services
{
    public class NFCWriter : INFCWriter
    {
        public void WriteAbsoluteUri(string uri)
        {
            
        }

        public void WriteApplication(string packageName)
        {
            throw new NotImplementedException();
        }

        public void WriteMimeMedia(string mimeType, byte[] mimeData)
        {
            throw new NotImplementedException();
        }
    }
}