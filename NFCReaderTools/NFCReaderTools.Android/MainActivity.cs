using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.NFCReaderFeature;
using Android.Nfc;
using Android.Content;
using Android.Nfc.Tech;
using System.Text;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Threading;

namespace NFCReaderTools.Droid
{
    [Activity(Label = "NFCReaderTools", Icon = "@drawable/icon", Theme = "@style/MainTheme", 
        MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait, 
        LaunchMode = LaunchMode.SingleInstance)]
    [IntentFilter(new[] { "android.nfc.action.NDEF_DISCOVERED" },
        Categories = new[] { Intent.CategoryDefault },
        DataScheme = "vnd.android.nfc", DataHost = "ext", DataPathPrefix = "/nfcreadertools.rrp.com:myowntype"
        )]
    [IntentFilter(new[] { "android.nfc.action.TAG_DISCOVERED" },
        Categories = new[] { Intent.CategoryDefault },
        DataScheme = "vnd.android.nfc", DataHost = "ext", DataPathPrefix = "/nfcreadertools.rrp.com:myowntype"
        )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private bool _isNFCCapable = false;
        private StringBuilder _data = new StringBuilder();

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());

            _isNFCCapable = CrossNFCReaderFeature.Current.Verify().GetValueOrDefault();
            SendData(Intent);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            SendData(intent);
        }

        protected override void OnPause()
        {
            base.OnPause();            
            ((NfcAdapter)CrossNFCReaderFeature.Current.Adapter)?.DisableForegroundDispatch(this);
        }

        protected override void OnResume()
        {
            base.OnResume();
            try
            {
                if (_isNFCCapable)
                {
                    var pendingIntent = PendingIntent.GetActivity(this, 0, new Intent(this, typeof(MainActivity)).AddFlags(ActivityFlags.SingleTop), 0);
                    var intentFilterNdef = new IntentFilter(NfcAdapter.ActionNdefDiscovered);                    
                    var intentFilterTag = new IntentFilter(NfcAdapter.ActionTagDiscovered);
                    intentFilterNdef.AddCategory(Intent.CategoryDefault);                    
                    intentFilterTag.AddCategory(Intent.CategoryDefault);
                    var intentFilters = new IntentFilter[] { intentFilterNdef, intentFilterTag };
                    string[][] techLists = new string[][] { new string[] { typeof(Ndef).Name, typeof(NdefFormatable).Name } };
                    ((NfcAdapter)CrossNFCReaderFeature.Current.Adapter)?.EnableForegroundDispatch(this, pendingIntent, intentFilters, techLists);
                }
            }
            catch { }
        }

        private void ReadData(byte[] payload)
        {
            try
            {                
                _data?.AppendLine(Encoding.UTF8.GetString(payload));
            }
            catch { }
        }

        private void SendData(Intent intent)
        {
            try
            {
                if (intent != null)
                {
                    switch (intent.Action)
                    {
                        case NfcAdapter.ActionNdefDiscovered:
                        case NfcAdapter.ActionTagDiscovered:
                            _data?.Clear();
                            MessagingCenter.Send<string>(true.ToString(), "reading");
                            try
                            {
                                IParcelable[] rawMessages = intent.GetParcelableArrayExtra(NfcAdapter.ExtraNdefMessages);
                                if (rawMessages != null)
                                {
                                    NdefMessage[] messages = new NdefMessage[rawMessages.Length];
                                    for (int i = 0; i < rawMessages.Length; i++)
                                    {
                                        messages[i] = (NdefMessage)rawMessages[i];
                                    }
                                    // Process the messages array..
                                    foreach (var m in messages)
                                    {
                                        NdefRecord[] records = m.GetRecords();
                                        foreach (var r in records)
                                        {
                                            ReadData(r.GetPayload());
                                        }
                                    }
                                    if (_data.ToString().Length > 0)
                                    {
                                        MessagingCenter.Send<string>(_data.ToString(), "data");
                                    }
                                }
                            }
                            catch { }
                            finally
                            {
                                MessagingCenter.Send<string>(false.ToString(), "reading");
                            }
                            break;
                        case NfcAdapter.ActionTechDiscovered:

                            break;
                    }
                }

            }
            catch { }
        }
    }
}

