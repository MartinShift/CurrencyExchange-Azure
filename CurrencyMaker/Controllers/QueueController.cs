using CurrencyMaker.Models;
using CurrencyMaker.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CurrencyMaker.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class LotController : ControllerBase
    {
        private readonly QueueService queueService;

        public LotController(QueueService queueService)
        {
            this.queueService = queueService;
        }

        [HttpPost("/lot")]
        public async Task<IActionResult> AddLot([FromBody] Lot lot)
        {
            await queueService.AddMessageAsync(lot);
            return Ok();
        }

        [HttpGet("/lots/{fromCurrency}/{toCurrency}")]
        public async Task<IActionResult> GetLots(string fromCurrency, string toCurrency)
        {
            var lots = await queueService.GetMessagesAsync(fromCurrency, toCurrency);
            return Ok(lots);
        }

        [HttpDelete("/lots/{messageId}/{popRecepit}")]
        public async Task<IActionResult> DeleteLot(string messageId, string popReceipt)
        {
            await queueService.DeleteMessageAsync(messageId, popReceipt);
            return Ok();
        }
    }
}
