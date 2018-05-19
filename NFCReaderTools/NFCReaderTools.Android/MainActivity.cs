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

namespace NFCReaderTools.Droid
{
    [Activity(Label = "NFCReaderTools", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private bool _isNFCCapable = false;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());

            _isNFCCapable = CrossNFCReaderFeature.Current.Verify().GetValueOrDefault();
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            if (intent != null)
            {
                switch (intent.Action)
                {
                    case NfcAdapter.ActionNdefDiscovered:
                    case NfcAdapter.ActionTagDiscovered:
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
                                NdefRecord[] record = m.GetRecords();

                            }
                        }
                        break;
                    case NfcAdapter.ActionTechDiscovered:

                        break;
                                           
                }                
            }
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
    }
}

