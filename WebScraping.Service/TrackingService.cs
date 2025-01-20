
using WebScraping.Core.Repositories;
using WebScraping.Core.Services;
using System.Net.Http;
using HtmlAgilityPack;
using WebScraping.Core.UnitOfWork;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

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
            using var driver = new ChromeDriver();

            try
            {
                driver.Navigate().GoToUrl("https://gonderitakip.ptt.gov.tr/");

                var inputField = driver.FindElement(By.Id("search-area"));
                inputField.SendKeys(trackingNumber);

                var searchButton = driver.FindElement(By.Id("searchButton"));
                searchButton.Click();

                await Task.Delay(2000); 

                var statusElement = driver.FindElement(By.XPath("//div[@class='col-8']//span"));

                return statusElement.Text;
            }
            catch (NoSuchElementException ex)
            {
                return "Girilen Takip Numarasına Ait Sonuç Bulunamadı. KAYIT YOK";
            }
            finally
            {
                driver.Quit();
            }
        }

    }
}
