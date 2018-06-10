using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.NFCWriterFeature;
using Android.Content;
using Android.Nfc;
using Android.Nfc.Tech;
using Newtonsoft.Json.Linq;
using System.Text;
using Xamarin.Forms;
using NFCWriterTools.Model;

namespace NFCWriterTools.Droid
{
    [Activity(Label = "NFCWriterTools", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private bool _isNFCCapable = false;
        private WriteData _data = null;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());

            _isNFCCapable = CrossNFCWriterFeature.Current.Verify().GetValueOrDefault();
        }

        protected override void OnNewIntent(Intent intent)
        {
            if (NfcAdapter.ActionTagDiscovered == intent.Action)
            {
                bool result = false;
                try
                {
                    var tag = (Tag)intent.GetParcelableExtra(NfcAdapter.ExtraTag);
                    // write data
                    switch (_data.Type)
                    {
                        case WriteDataType.AbsoluteUri:
                            result = CrossNFCWriterFeature.Current.WriteAbsoluteUri(_data.Content, tag);
                            //result = CrossNFCWriterFeature.Current.WriteWellKnown(RecordTypeDefinition.RTD_URI, _data.Content, tag);
                            break;
                        case WriteDataType.Aplication:
                            result = CrossNFCWriterFeature.Current.WriteApplication(_data.Content, tag); 
                            break;
                        case WriteDataType.MimeCard:
                            result = CrossNFCWriterFeature.Current.WriteMimeMedia("text/vcard", Encoding.UTF8.GetBytes(_data.Content), tag);
                            break;
                        case WriteDataType.MimeJson:                            
                            result = CrossNFCWriterFeature.Current.WriteMimeMedia("application/json", Encoding.UTF8.GetBytes(_data.Content), tag);
                            break;
                        case WriteDataType.ExternalType:
                            result = CrossNFCWriterFeature.Current.WriteExternalType("nfcreadertools.rrp.com:myowntype", _data.Content, tag);
                            //result = CrossNFCWriterFeature.Current.WriteExternalType("nfcreadertools.rrp.com", "myowntype", _data.Content, tag);
                            break;
                    }
                    MessagingCenter.Send<string>(result.ToString(), "result");
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
            MessagingCenter.Unsubscribe<WriteData>(this, "data");
            ((NfcAdapter)CrossNFCWriterFeature.Current.Adapter)?.DisableForegroundDispatch(this);
        }

        protected override void OnResume()
        {
            base.OnResume();
            MessagingCenter.Subscribe<WriteData>(this, "data", (data) =>
            {
                try
                {
                    if (_isNFCCapable)
                    {
                        var pendingIntent = PendingIntent.GetActivity(this, 0, new Intent(this, typeof(MainActivity)).AddFlags(ActivityFlags.SingleTop), 0);
                        var intentFilterTag = new IntentFilter(NfcAdapter.ActionTagDiscovered);
                        intentFilterTag.AddCategory(Intent.CategoryDefault);
                        var intentFilters = new IntentFilter[] { intentFilterTag };
                        string[][] techLists = new string[][] { new string[] { typeof(Ndef).Name, typeof(NdefFormatable).Name } };
                        ((NfcAdapter)CrossNFCWriterFeature.Current.Adapter)?.EnableForegroundDispatch(this, pendingIntent, intentFilters, techLists);
                        // get data
                        _data = data;
                    }
                }
                catch { }
            });                                    
        }
    }
}

