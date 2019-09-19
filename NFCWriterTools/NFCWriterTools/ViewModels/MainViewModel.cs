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
    public class MyOwnData
    {
        public Guid MyOwnId { get; set; }
        public string MyOwnDescription { get; set; }
    }

    public class MainViewModel : ViewModelBase
    {        
        public string AbsoluteUriData
        {
            get { return "http://www.raulrodriguezpacho.com"; }     
        }

        // package
        public string ApplicationData
        {
            get { return "raul.PalentinoROM_Android"; }
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
                    .AppendLine("TEL;TYPE=CELL:+34 123 456 789")
                    .AppendLine("EMAIL:raulrodriguezpacho@gmail.com")                    
                    .AppendLine("URL:http://www.raulrodriguezpacho.com")
                    .AppendLine("TITLE:Xamarin Developer & Professional Certified")
                    .AppendLine("ORG:Software Development")
                    .AppendLine("NOTE:Mobile - AR/VR/MR - AI Machine Learning & Deep Learning")
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
                        { "ssid", "YOUR_SSID" },
                        { "password", "yOuRwIfIpAsSwOrD" }
                    }
                    .ToString();                
            }
        }

        public MyOwnData ExternalData
        {
            get
            {
                return
                    new MyOwnData
                    {
                        MyOwnId = Guid.NewGuid(),
                        MyOwnDescription = "Xamarin Forms and what to do with NFC tags.."
                    };
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

        private ICommand _writeExternalTypeCommand;
        public ICommand WriteExternalTypeCommand
        {
            get
            {
                return _writeExternalTypeCommand ?? (_writeExternalTypeCommand = new Command(() =>
                {
                    IsBusy = true;
                    var data = new WriteData()
                    {
                        Content = JObject.FromObject(ExternalData).ToString(),
                        Type = WriteDataType.ExternalType
                    };
                    MessagingCenter.Send<WriteData>(data, "data");
                }));
            }
        }
    }
}
