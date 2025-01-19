using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Core.Repositories;
using WebScraping.Core.Services;
using System.Net.Http;
using HtmlAgilityPack;
using WebScraping.Core.UnitOfWork;

namespace WebScraping.Service
{
    public class TrackingService : ITrackingService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TrackingService(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task UpdateAllStatusesAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            foreach (var order in orders)
            {
                var newStatus = await GetTrackingStatusAsync(order.TrackingNo);
                if (order.Status != newStatus)
                {
                    order.Status = newStatus;
                    order.UpdatedDate = DateTime.UtcNow;
                    await _orderRepository.Update(order);
                    await _unitOfWork.CommitAsync();

                }
            }

        }

        public async Task<string> GetTrackingStatusAsync(string trackingNumber)
        {
            var url = "https://gonderitakip.ptt.gov.tr/";
            using var httpClient = new HttpClient();

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "trackingNumber",trackingNumber }
            });

            var response = await httpClient.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                return "Error: Unable to connect to the tracking service.";
            }
            var responseBody = await response.Content.ReadAsStringAsync();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(responseBody);

            var deliveredNode = htmlDoc.DocumentNode.SelectSingleNode("//span[text()='TESLİM EDİLDİ']");
            if (deliveredNode != null)
            {
                return "TESLİM EDİLDİ";
            }
               
            var notDeliveredNode = htmlDoc.DocumentNode.SelectSingleNode("//span[text()='TESLİM EDİLEMEDİ']");
            if (notDeliveredNode != null)
            {
                return "TESLİM EDİLEMEDİ";
            }

            var errorNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='alert alert-warning' and contains(text(), 'Girilen Takip Numarasına Ait Sonuç Bulunamadı')]");
            if (errorNode != null)
                return "KAYIT YOK";

            return "Unknown status.";

        }

    }
}
