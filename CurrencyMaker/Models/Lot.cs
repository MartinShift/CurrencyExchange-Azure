namespace CurrencyMaker.Models
{
    public class Lot
    {
        public string FromCurrency { get; set; }

        public string ToCurrency { get; set; }

        public double Amount { get; set; }

        public string Seller { get; set; }

        public string ToCurrencyCourse { get; set; }

    }
}
