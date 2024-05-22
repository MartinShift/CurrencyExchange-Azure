using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using CurrencyMaker.Models;
using System.Text.Json;

namespace CurrencyMaker.Services
{
    using Azure.Storage.Queues;
    using System.Text.Json;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Globalization;
    using CurrencyMaker.ViewModels;

    public class QueueService
    {
        private QueueClient queueClient;

        public QueueService()
        {
            string connectionString = File.ReadAllText("D:\\secure\\azure-connection-string.txt");
            queueClient = new QueueClient(connectionString, "lots");
            queueClient.CreateIfNotExists();
        }

        public async Task AddMessageAsync(Lot lot)
        {
            string message = JsonSerializer.Serialize(lot);
            await queueClient.SendMessageAsync(message, timeToLive: TimeSpan.FromDays(1));
        }

        public async Task<LotModel[]> GetMessagesAsync(string fromCurrency, string toCurrency)
        {
            QueueMessage[] receivedMessages = (await queueClient.ReceiveMessagesAsync(10)).Value;
            return receivedMessages
                .Select(m => new LotModel
                {
                    Lot = JsonSerializer.Deserialize<Lot>(m.MessageText),
                    Data = new MessageData { Id = m.MessageId, PopReceipt = m.PopReceipt }
                })
                .Where(lotModel => lotModel.Lot.FromCurrency == fromCurrency && lotModel.Lot.ToCurrency == toCurrency)
                .OrderBy(lotModel => double.Parse(lotModel.Lot.ToCurrencyCourse,CultureInfo.InvariantCulture))
                .ToArray();
        }

        public async Task DeleteMessageAsync(string messageId, string popReceipt)
        {
            await queueClient.DeleteMessageAsync(messageId, popReceipt);
        }
    }

}
