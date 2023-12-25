using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinancialCounter.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FinancialCounter.Views
{
    public partial class RatesPage : ContentPage
    {
        Dictionary<string, decimal> latestData;
        Dictionary<string, string> curencyNames;
        public RatesPage()
        {
            InitializeComponent();
            LoadExchangeRates();

            // Set BindingContext for navigation
            BindingContext = this;

        }

        private async Task LoadExchangeRates()
        {
            ExchangeRates exchangeRates = new ExchangeRates();
            latestData = await exchangeRates.GetLatestDataAsync();

            CurrencyName currencyName = new CurrencyName();
            curencyNames = await currencyName.GetCurrencyNames();

            //Sort dictionary 
            curencyNames = curencyNames.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            if (curencyNames != null)
            {
                foreach (var pair in curencyNames)
                {
                    var button = new Button
                    {
                        Text = pair.Value,
                        TextColor = Color.Black,
                        BackgroundColor  = Color.Transparent
                    };

                    // Handle the button click event
                    button.Clicked += (sender, e) => OnInformButtonClicked(sender, e, pair.Key);

                    // Adding buttons as ViewCells in the TableSection
                    var viewCell = new ViewCell
                    {
                        View = new StackLayout
                        {
                            Children = { button }
                        }
                    };
                    exchangeRatesTable.Root[0].Add(viewCell);
                }
            }
            else
            {
                // Handle case where data retrieval failed
                await DisplayAlert("Error", "Failed to fetch exchange rates.", "OK");
            }
        }
        private async void OnInformButtonClicked(object sender, EventArgs e, string key)
        {
            await DisplayAlert("Additional Information", "Code:"+key+"\n Exchange rate:"+latestData[key], "OK");
        }
        private async void OnReloadButtonClicked(object sender, EventArgs e)
        {
            exchangeRatesTable.Root[0].Clear();
            await LoadExchangeRates();
        }
    }
}
