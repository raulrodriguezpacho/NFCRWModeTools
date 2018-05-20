using Newtonsoft.Json.Linq;
using NFCWriterTools.Model;
using Plugin.NFCWriterFeature;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NFCWriterTools.ViewModels
{
    public class MainViewModel : ViewModelBase
    {        
        public string AbsoluteUriData
        {
            get { return "http://www.raulrodriguezpacho.com"; }            
        }

        //"es.aeat.pin24h" //raul.PalentinoROM_Android //com.rrp.EveryCent //com.oxylane.android.decathlon //com.skype.raider //es.aeat.pin24h
        public string ApplicationData
        {
            get { return "com.oxylane.android.decathlon"; }
        }

        public string MimeCardData
        {
            get
            {
                return 
                    new StringBuilder()
                    .AppendLine("BEGIN:VCARD")
                    .AppendLine("VERSION:3.0")
                    .AppendLine("N:Rodríguez Pacho;Raúl;;;")
                    .AppendLine("FN:Raúl Rodríguez Pacho")
                    .AppendLine("TEL;TYPE=CELL:+34 654 70 01 24")
                    .AppendLine("EMAIL:raulrodriguezpacho@gmail.com")
                    .AppendLine("PHOTO;TYPE=JPEG;VALUE=URI:http://www.raulrodriguezpacho.com/perfil.jpg")
                    .AppendLine("URL:http://www.raulrodriguezpacho.com")
                    .AppendLine("TITLE:Xamarin Developer & Professional Certified")
                    .AppendLine("END:VCARD")
                    .ToString();
            }
        }

        public string MimeJsonData
        {
            get
            {                
                return
                    new JObject
                    {
                        { "ssid", "MOVISTAR_71D2" },
                        { "password", "V0y a mi Air3" }
                    }
                    .ToString();                
            }
        }

        public MainViewModel(INavigation navigation) : base(navigation)
        {
            if (CrossNFCWriterFeature.Current.Verify().GetValueOrDefault())
            {
                MessagingCenter.Subscribe<string>(this, "result", (result) =>
                {
                    IsBusy = false;
                    ShowMessage("Data written?", result.ToString(), "ok");
                });
            }
        }        

        private ICommand _writeAbsoluteUriCommand;
        public ICommand WriteAbsoluteUriCommand
        {
            get
            {
                return _writeAbsoluteUriCommand ?? (_writeAbsoluteUriCommand = new Command(() =>
                {
                    IsBusy = true;
                    var data = new WriteData()
                    {
                        Content = AbsoluteUriData,
                        Type = WriteDataType.AbsoluteUri
                    };
                    MessagingCenter.Send<WriteData>(data, "data");                    
                }));
            }
        }

        private ICommand _writeApplicationDataCommand;
        public ICommand WriteApplicationDataCommand
        {
            get
            {
                return _writeApplicationDataCommand ?? (_writeApplicationDataCommand = new Command(() =>
                {
                    IsBusy = true;
                    var data = new WriteData()
                    {
                        Content = ApplicationData,
                        Type = WriteDataType.Aplication
                    };
                    MessagingCenter.Send<WriteData>(data, "data");
                }));
            }
        }

        private ICommand _writeMimeCardDataCommand;
        public ICommand WriteMimeCardDataCommand
        {
            get
            {
                return _writeMimeCardDataCommand ?? (_writeMimeCardDataCommand = new Command(() =>
                {
                    IsBusy = true;
                    var data = new WriteData()
                    {
                        Content = MimeCardData,
                        Type = WriteDataType.MimeCard
                    };
                    MessagingCenter.Send<WriteData>(data, "data");
                }));
            }
        }

        private ICommand _writeMimeJsonDataCommand;
        public ICommand WriteMimeJsonDataCommand
        {
            get
            {
                return _writeMimeJsonDataCommand ?? (_writeMimeJsonDataCommand = new Command(() =>
                {
                    IsBusy = true;
                    var data = new WriteData()
                    {
                        Content = MimeJsonData,
                        Type = WriteDataType.MimeJson
                    };
                    MessagingCenter.Send<WriteData>(data, "data");
                }));
            }
        }
    }
}
