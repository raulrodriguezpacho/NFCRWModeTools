using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreNFC;
using Foundation;
using Plugin.NFCReaderFeature;
using UIKit;
using Xamarin.Forms;

namespace NFCReaderTools.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, INFCNdefReaderSessionDelegate
    {
        private bool _isNFCCapable = false;
        private NFCNdefReaderSession _nfcSession = null;
        private StringBuilder _data = new StringBuilder();

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            _isNFCCapable = CrossNFCReaderFeature.Current.Verify().GetValueOrDefault();
            StartSession();

            return base.FinishedLaunching(app, options);            
        }

        public void DidInvalidate(NFCNdefReaderSession session, NSError error)
        {
            var readerError = (NFCReaderError)(long)error.Code;
            if (readerError != NFCReaderError.ReaderSessionInvalidationErrorFirstNDEFTagRead &&
                readerError != NFCReaderError.ReaderSessionInvalidationErrorUserCanceled)
            {
                EndSession();
                // some error handling
                MessagingCenter.Send<string>(readerError.ToString(), "data");
            }
        }

        public void DidDetect(NFCNdefReaderSession session, NFCNdefMessage[] messages)
        {
            _data?.Clear();
            foreach (NFCNdefMessage message in messages)
            {
                foreach (NFCNdefPayload record in message.Records)
                {
                    var typeNameFormat = record.TypeNameFormat;
                    //var identifier = record.Identifier?.ToArray();
                    var type = record.Type;
                    var payload = record.Payload;
                    if (payload.Length > 0)
                        ReadData(payload.ToArray());
                    else
                        ReadData(type.ToArray());
                }
            }
            if (_data.ToString().Length > 0)
            {
                MessagingCenter.Send<string>(_data.ToString(), "data");
            }
        }

        private void StartSession()
        {
            if (_isNFCCapable)
            {
                try
                {
                    _nfcSession = new NFCNdefReaderSession(this, null, false);
                    _nfcSession?.BeginSession();
                }
                catch { }
            }
        }

        private void EndSession()
        {
            if (_isNFCCapable)
            {
                try
                {
                    _nfcSession?.InvalidateSession();
                }
                catch { }
            }
        }

        private void ReadData(byte[] payload)
        {
            try
            {
                _data?.AppendLine(Encoding.UTF8.GetString(payload));
            }
            catch { }
        }
    }
}
