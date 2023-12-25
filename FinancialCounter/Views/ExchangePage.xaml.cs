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
    public partial class ExchangePage : ContentPage
    {
        Dictionary<string, decimal> latestExchangeData=new Dictionary<string, decimal>();
        ExchangeRates exchangeRates = new ExchangeRates();
        public ExchangePage()
        {
            InitializeComponent();
            // Populate currencies in the dropdowns
            LoadCurrencies();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Set the text field value to null when the page appears
            convertToPicker.SelectedItem = string.Empty;
            convertFromPicker.SelectedItem = string.Empty;
            amountEntry.Text = string.Empty;
            resultLabel.Text = string.Empty;
        }
        private async Task LoadCurrencies()
        {
            latestExchangeData = await exchangeRates.GetLatestDataAsync();
            if (latestExchangeData != null)
            {
                foreach (var pair in latestExchangeData)
                {
                    convertFromPicker.Items.Add(pair.Key);
                    convertToPicker.Items.Add(pair.Key);
                }
            }
            else
            {
                // Handle case where data retrieval failed
                await DisplayAlert("Error", "Failed to fetch exchange rates.", "OK");
            }
        }

        private void OnConvertButtonClicked(object sender, EventArgs e)
        {
            // Check if amount is valid
            if (!string.IsNullOrWhiteSpace(amountEntry.Text) && decimal.TryParse(amountEntry.Text, out decimal amount))
            {
                // Perform the currency conversion here
                string fromCurrency = convertFromPicker.SelectedItem?.ToString();
                string toCurrency = convertToPicker.SelectedItem?.ToString();
                decimal ammount = Convert.ToDecimal(amountEntry.Text);
                if (!string.IsNullOrWhiteSpace(fromCurrency) && !string.IsNullOrWhiteSpace(toCurrency) && fromCurrency!=toCurrency)
                {
                    decimal result = 0;
                    result = ammount / latestExchangeData[fromCurrency] * latestExchangeData[toCurrency];
                    resultLabel.Text = $"Converting {amount} from {fromCurrency} to {toCurrency}\nResult:{Decimal.Round(result,3) }";
                }
                else
                {
                    resultLabel.Text = "Please select valid currencies.";
                }
            }
            else
            {
                resultLabel.Text = "Please enter a valid amount.";
            }
        }
        private void OnSwitchButtonClicked(object sender, EventArgs e)
        {
            string fromCurrency = convertFromPicker.SelectedItem?.ToString();
            string toCurrency = convertToPicker.SelectedItem?.ToString();
            if (!string.IsNullOrWhiteSpace(fromCurrency) && !string.IsNullOrWhiteSpace(toCurrency) && fromCurrency!=toCurrency)
            {
                string var = fromCurrency;
                fromCurrency = toCurrency;
                toCurrency = var;
                convertToPicker.SelectedItem = toCurrency;
                convertFromPicker.SelectedItem = fromCurrency; 
            }
            else
            {
                resultLabel.Text = "Please select valid currencies.";
            }
            
        }
    }
}