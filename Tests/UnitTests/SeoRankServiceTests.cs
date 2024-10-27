using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using Smokeball_SEORank_Api.Models;
using Smokeball_SEORank_Api.Services;

namespace Smokeball_SEORank_Api.Tests.Services
{
    [TestFixture]
    public class SeoRankServiceTests
    {
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private HttpClient _httpClient;
        private IOptions<SearchEngineSettings> _searchEngineSettings;
        private SeoRankService _seoRankService;
        private string _htmlFixtureContent;

        [SetUp]
        public void SetUp()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _searchEngineSettings = Options.Create(new SearchEngineSettings
            {
                GoogleBaseUrl = "https://www.google.com/search"
            });

            _seoRankService = new SeoRankService(_httpClient, _searchEngineSettings);

            _htmlFixtureContent = System.IO.File.ReadAllText("Tests/Fixtures/ResponseContent.html");
        }

        [Test]
        public async Task PerformSeoRankSearch_UrlFoundInSearchResults_ReturnsPositions()
        {
            // Arrange
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(() =>
                {
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(_htmlFixtureContent)
                    };
                });

            // Act
            var result = await _seoRankService.PerformSeoRankSearch("conveyancing software", "www.smokeball.com");

            // Assert
            Assert.AreEqual("1, 11, 21, 31, 41, 51, 61, 71, 81, 91", result); // Result is based on the fixture content.
        }

        [Test]
        public async Task PerformSeoRankSearch_UrlNotFoundInSearchResults_ReturnsZero()
        {
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(() => new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(_htmlFixtureContent)
                });

            // Act
            var result = await _seoRankService.PerformSeoRankSearch("conveyancing software", "www.webjet.com.au");

            // Assert
            Assert.AreEqual("0", result);
        }

        [Test]
        public void PerformSeoRankSearch_InvalidResponse_ThrowsException()
        {
            // Arrange
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(() => new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest
                });

            // Act & Assert
            Assert.ThrowsAsync<HttpRequestException>(async () =>
                await _seoRankService.PerformSeoRankSearch("conveyancing software", "www.smokeball.com"));
        }
    }
}
