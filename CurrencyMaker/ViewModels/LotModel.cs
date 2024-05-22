using Azure.Storage.Queues.Models;
using CurrencyMaker.Models;

namespace CurrencyMaker.ViewModels
{
    public class LotModel
    {
        public Lot Lot { get; set; }

        public MessageData Data { get; set; }
    }
}
