using Newtonsoft.Json;
using NFCReaderTools.Services;
using Plugin.NFCReaderFeature;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NFCReaderTools.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _dataRead = string.Empty;
        public string DataRead
        {
            get { return _dataRead; }
            set
            {
                _dataRead = value;
                OnPropertyChanged(nameof(DataRead));
            }
        }

        private string _doResult = string.Empty;
        public string DoResult
        {
            get { return _doResult; }
            set
            {
                _doResult = value;
                OnPropertyChanged(nameof(DoResult));
            }
        }

        private ICommand _clearDataCommand;
        public ICommand ClearDataCommand
        {
            get
            {
                return _clearDataCommand ?? (_clearDataCommand = new Command(() =>
                {
                    DataRead = string.Empty;
                    DoResult = string.Empty;
                }));
            }
        }

        private ICommand _doCommand;
        public ICommand DoCommand
        {
            get
            {
                return _doCommand ?? (_doCommand = new Command(() =>
                {
                    if (!string.IsNullOrEmpty(_dataRead))
                        DoActionWithData(_dataRead);
                }));
            }
        }

        public MainViewModel(INavigation navigation) : base(navigation)
        {
            if (CrossNFCReaderFeature.Current.Verify().GetValueOrDefault())
            {
                MessagingCenter.Subscribe<string>(this, "reading", (result) =>
                {
                    IsBusy = bool.Parse(result);                    
                });
                MessagingCenter.Subscribe<string>(this, "data", (result) =>
                {                    
                    DataRead = result;
                });
            }
        }

        private void DoActionWithData(string dataRead)
        {
            try
            {
                // just this case!
                var wifiPoint =JsonConvert.DeserializeObject<WifiPoint>(dataRead);
                var wifiService = DependencyService.Get<IWifiService>();
                var result = wifiService.Connect(wifiPoint.SSID, wifiPoint.Password);
                DoResult = result.ToString();
            }
            catch (Exception ex) { }
        }
    }

    public class WifiPoint
    {
        [JsonProperty(PropertyName = "ssid")]
        public string SSID { get; set; }
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
    }
}
