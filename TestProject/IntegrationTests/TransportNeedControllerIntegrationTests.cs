using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Resources.BLL.App.DTO.Enums;
using TestProject.Helpers;
using WebApp;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.IntegrationTests
{
    public class TransportNeedControllerIntegrationTests: IClassFixture<CustomWebApplicationFactory<WebApp.Startup>>
    {
        private readonly CustomWebApplicationFactory<WebApp.Startup> _factory;
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _testOutputHelper;

        public TransportNeedControllerIntegrationTests(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
            _client = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.UseSetting("test_database_name", Guid.NewGuid().ToString());
                })
                .CreateClient(new WebApplicationFactoryClientOptions()
                    {
                        // dont follow redirects
                        AllowAutoRedirect = false
                    }
                );
        }
        
        [Fact]
        public async Task TransportNeeds_HasSuccessStatusCode()
        {
            // ARRANGE
            var uri = "/TransportNeeds";
            
            // ACT
            _testOutputHelper.WriteLine("before get response");
            var getTestResponse = await _client.GetAsync(uri);
            _testOutputHelper.WriteLine("after get response");

            // ASSERT
            getTestResponse.EnsureSuccessStatusCode();
            Assert.InRange((int) getTestResponse.StatusCode, 200, 299);
            
        }
        [Fact]
        public async Task TransportNeedsCreate_AuthRedirect()
        {
            // ARRANGE
            var uri = "/TransportNeeds/Create";
            
            // ACT
            _testOutputHelper.WriteLine("before get response");
            var getTestResponse = await _client.GetAsync(uri);
            _testOutputHelper.WriteLine("after get response");

            // ASSERT
            Assert.Equal(302, (int) getTestResponse.StatusCode);
            
        }
        
        [Fact]
        public async Task TransportNeeds_CreateAction_AuthFlow_WithAddingTransportNeed()
        {
            // ARRANGE
            var uri = "/TransportNeeds/Create";
            
            // ACT
            _testOutputHelper.WriteLine("before get response");
            var getTestResponse = await _client.GetAsync(uri);
            _testOutputHelper.WriteLine("after get response");

            // ASSERT
            Assert.Equal(302, (int) getTestResponse.StatusCode);
            var redirectUri = getTestResponse.Headers
                .FirstOrDefault(x => x.Key == "Location").Value.FirstOrDefault();

            redirectUri.Should().NotBeNull();

            await Get_Login_Page(redirectUri);

        }
        
        public async Task Get_Login_Page(string uri)
        {
            var getLoginPageResponse = await _client.GetAsync(uri);
            getLoginPageResponse.EnsureSuccessStatusCode();
    
            // get the document
            var getLoginDocument = await HtmlHelpers.GetDocumentAsync(getLoginPageResponse);
            var registerAnchorElement = (IHtmlAnchorElement) getLoginDocument.QuerySelector("#register");
            var registerUrl = registerAnchorElement.Href;
            _testOutputHelper.WriteLine("Register url: " + registerUrl);

            await Get_Register_Page(registerUrl);
            // find register page url
            // go and register
        }
        
        public async Task Get_Register_Page(string uri)
        {
            var getRegisterPageResponse = await _client.GetAsync(uri);
            getRegisterPageResponse.EnsureSuccessStatusCode();
            
            // get the document
            var getRegisterDocument = await HtmlHelpers.GetDocumentAsync(getRegisterPageResponse);
            var regForm = (IHtmlFormElement) getRegisterDocument.QuerySelector("#register-form");
            var regFormValues = new Dictionary<string, string>()
            {
                ["Input_Email"] = "test@user.ee",
                ["Input_Password"] = ".Tere123",
                ["Input_ConfirmPassword"] = ".Tere123",
                ["Input_Firstname"] = "Integration",
                ["Input_Lastname"] = "Test",
                [""] = "",
            };
            var regResponse = await _client.SendAsync(regForm, regFormValues);
            regResponse.StatusCode.Should().Equals(302);
            
            var redirectUri = regResponse.Headers
                .FirstOrDefault(x => x.Key == "Location").Value.FirstOrDefault();

            redirectUri.Should().NotBeNull();
            await Get_TestAuthAction_Authenticated(redirectUri);
            
        }
        
        public async Task Get_TestAuthAction_Authenticated(string uri)
        {
            var getTestResponse = await _client.GetAsync(uri);

            getTestResponse.EnsureSuccessStatusCode();
            
            _testOutputHelper.WriteLine($"Uri '{uri}' was accessed with status code '{getTestResponse.StatusCode}'.");
            
            var getHomeDocument = await HtmlHelpers.GetDocumentAsync(getTestResponse);
            var transportNeedAnchorElement = (IHtmlAnchorElement) getHomeDocument.QuerySelector("#transport-need-link");
            var transportNeedsPageUri = transportNeedAnchorElement.Href;
            await Get_TransportNeeds_Page(transportNeedsPageUri);
        }

        public async Task Get_TransportNeeds_Page(string uri)
        {
            var getTransportNeedResponse = await _client.GetAsync(uri);
            getTransportNeedResponse.EnsureSuccessStatusCode();
            _testOutputHelper.WriteLine($"Transport needs index Uri '{uri}' was accessed with status code '{getTransportNeedResponse.StatusCode}'.");
            
            var getCreateDocument = await HtmlHelpers.GetDocumentAsync(getTransportNeedResponse);
            var transportNeedAnchorElement = (IHtmlAnchorElement) getCreateDocument.QuerySelector("#create-transport-need");
            var transportNeedCreateUri = transportNeedAnchorElement.Href;
            await Get_TransportNeeds_CreatePage(transportNeedCreateUri);
        }

        public async Task Get_TransportNeeds_CreatePage(string uri)
        {
            var getTransportNeedCreateResponse = await _client.GetAsync(uri);
            getTransportNeedCreateResponse.EnsureSuccessStatusCode();
            _testOutputHelper.WriteLine($"Transport need create Uri '{uri}' was accessed with status code '{getTransportNeedCreateResponse.StatusCode}'.");
            
            // get the document
            var getCreateDocument = await HtmlHelpers.GetDocumentAsync(getTransportNeedCreateResponse);
            var createForm = (IHtmlFormElement) getCreateDocument.QuerySelector("#create-need");
            createForm.Should().NotBeNull();
            var createFormValues = new Dictionary<string, string>()
            {
                ["TransportOfferId"] = "",
                ["TransportNeed_TransportNeedInfo"] = "info",
                ["TransportNeed_TransportType"] = "2",
                ["TransportNeed_PersonCount"] = 1.ToString(),
                ["TransportMeta_StartTime"] = "2021-06-08 22:33:00.0000000",
                ["StartLocation_Country"] = "EE",
                ["StartLocation_City"] = "start",
                ["StartLocation_Address"] = "xx-12",
                ["StartLocation_LocationInfo"] = "",
                ["DestinationLocation_Country"] = "EE",
                ["DestinationLocation_City"] = "dest",
                ["DestinationLocation_Address"] = "dest-12",
                ["DestinationLocation_LocationInfo"] = "",
                [""] = "",
            };
            
            var createResponse = await _client.SendAsync(createForm, createFormValues);
            createResponse.StatusCode.Should().Equals(302);
            
            var redirectUri = createResponse.Headers
                .FirstOrDefault(x => x.Key == "Location").Value.FirstOrDefault();

            redirectUri.Should().NotBeNull();
        }
    }
}