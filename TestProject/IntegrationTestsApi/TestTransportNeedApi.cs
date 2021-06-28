using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BLL.App.DTO;
using BLL.App.DTO.Enums;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using PublicApi.DTO.v1;
using TestProject.Helpers;
using WebApp;
using Xunit;
using Xunit.Abstractions;
using ETransportType = PublicApi.DTO.v1.Enums.ETransportType;
using TransportMeta = BLL.App.DTO.TransportMeta;
using TransportNeed = BLL.App.DTO.TransportNeed;

namespace TestProject.IntegrationTestsApi
{
    public class TestTransportNeedApi: IClassFixture<CustomWebApplicationFactory<WebApp.Startup>>
    {
        private readonly CustomWebApplicationFactory<WebApp.Startup> _factory;
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _testOutputHelper;

        public TestTransportNeedApi(CustomWebApplicationFactory<Startup> factory,
            ITestOutputHelper testOutputHelper)
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
        public async Task Api_Get_TransportNeeds()
        {
            // ARRANGE
            var uri = "/api/v1/TransportNeeds";

            // ACT
            var getTestResponse = await _client.GetAsync(uri);

            // ASSERT
            getTestResponse.EnsureSuccessStatusCode();
            
            var body = await getTestResponse.Content.ReadAsStringAsync();
            _testOutputHelper.WriteLine(body);
            
            var data = JsonHelper.DeserializeWithWebDefaults<List<TransportNeed>>(body);

            Assert.NotNull(data);
            Assert.NotEmpty(data);
            Assert.Single(data);
        }

        [Fact]
        public async Task Test_Flow_WithAuth()
        {
            // ARRANGE
            var uri = "/api/v1/Account/Register";

            var register = new Register()
            {
                Email = "api@test.com",
                Firstname = "API",
                Lastname = "test",
                Password = ".Tere123"
            };
           
            // ACT
            var getRegResponse = await _client.PostAsJsonAsync(uri, register);

            // ASSERT
            getRegResponse.EnsureSuccessStatusCode();
            var body = await getRegResponse.Content.ReadAsStringAsync();
            _testOutputHelper.WriteLine(getRegResponse.StatusCode.ToString());
            var data = JsonHelper.DeserializeWithWebDefaults<JwtResponse>(body);
            
            Assert.NotNull(data);
            _testOutputHelper.WriteLine(data!.Token);

           await AddLocations(data.Token);

        }

        public async Task AddLocations(string token)
        {
            var transportNeedAdd = new PublicApi.DTO.v1.TransportNeedAdd()
            {
                TransportType = ETransportType.PersonsOnly,
                PersonCount = 2,
                TransportNeedInfo = "tere",
            };
            var meta = new TransportMetaAdd()
            {
                StartTime = DateTime.Now.AddDays(2),
            };
            var startLoc = new PublicApi.DTO.v1.Location()
            {
                Country = "est",
                City = "Tartu",
                Address = "cc-2",
            };
            var destLoc = new PublicApi.DTO.v1.LocationAdd()
            {
                Country = "est2",
                City = "Tln",
                Address = "vvv",
            };
            
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var locUri = "/api/v1/Locations";
            // start location
            var startLocRes = await _client.PostAsJsonAsync(locUri, startLoc);
            
            startLocRes.EnsureSuccessStatusCode();
            var body = await startLocRes.Content.ReadAsStringAsync();
            
            _testOutputHelper.WriteLine(startLocRes.StatusCode.ToString());
            
            var startLocation = JsonHelper.DeserializeWithWebDefaults<PublicApi.DTO.v1.Location>(body);
            Assert.NotNull(startLocation);
            meta.StartLocationId = startLocation!.Id;
            
            // dest location
            var destLocRes = await _client.PostAsJsonAsync(locUri, destLoc);
            
            destLocRes.EnsureSuccessStatusCode();
            var destLocBody = await destLocRes.Content.ReadAsStringAsync();
            
            _testOutputHelper.WriteLine(destLocRes.StatusCode.ToString());
            
            var destinationLocation = JsonHelper.DeserializeWithWebDefaults<PublicApi.DTO.v1.Location>(destLocBody);
            Assert.NotNull(destinationLocation);
            meta.DestinationLocationId = destinationLocation!.Id;

            await AddMeta(meta, transportNeedAdd);

        }

        public async Task AddMeta(TransportMetaAdd meta, TransportNeedAdd transportNeedAdd)
        {
            var locUri = "/api/v1/TransportMeta";
            
            // start location
            var metaRes = await _client.PostAsJsonAsync(locUri, meta);
            
            metaRes.EnsureSuccessStatusCode();
            var body = await metaRes.Content.ReadAsStringAsync();
            
            _testOutputHelper.WriteLine(metaRes.StatusCode.ToString());
            
            var transportMeta = JsonHelper.DeserializeWithWebDefaults<PublicApi.DTO.v1.TransportMeta>(body);
            Assert.NotNull(transportMeta);
            transportNeedAdd.TransportMetaId = transportMeta!.Id;
            await AddTransportNeed(transportNeedAdd);
        }
        
        public async Task AddTransportNeed(TransportNeedAdd transportNeedAdd)
        {
            var uri = "/api/v1/TransportNeeds";
            
    
            var response = await _client.PostAsJsonAsync(uri, transportNeedAdd);
            
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            
            _testOutputHelper.WriteLine(response.StatusCode.ToString());
            
            var transportNeed = JsonHelper.DeserializeWithWebDefaults<PublicApi.DTO.v1.TransportNeed>(body);
            Assert.NotNull(transportNeed);
            Assert.IsType<Guid>(transportNeed!.Id);
            var id = transportNeed!.Id;
            await GetTransportNeedById(id, uri);
        }

        public async Task GetTransportNeedById(Guid id, string uri)
        {
            var uriWithId = uri + "/" + id;
            var response = await _client.GetAsync(uriWithId);
            
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            
            _testOutputHelper.WriteLine(response.StatusCode.ToString());
            _testOutputHelper.WriteLine(body);
            var transportNeed = JsonHelper.DeserializeWithWebDefaults<PublicApi.DTO.v1.TransportNeed>(body);
            Assert.NotNull(transportNeed);
            Assert.IsType<PublicApi.DTO.v1.TransportNeed>(transportNeed);

            await CanUpdateTransportNeed(transportNeed!, uri);
        }
        
        public async Task CanUpdateTransportNeed(PublicApi.DTO.v1.TransportNeed transportNeed, string uri)
        {
            var uriWithId = uri + "/" + transportNeed!.Id;
            var newTransportNeed = transportNeed;
            newTransportNeed.TransportNeedInfo = "abc";
            
            var response = await _client.PutAsJsonAsync(uriWithId, newTransportNeed);
            
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            
            _testOutputHelper.WriteLine(response.StatusCode.ToString());
            _testOutputHelper.WriteLine(body);
            var updatedTransportNeedRes = await _client.GetAsync(uriWithId);
            updatedTransportNeedRes.EnsureSuccessStatusCode();
            var updatedBody = await updatedTransportNeedRes.Content.ReadAsStringAsync();
            var updatedTransportNeedFromApi = JsonHelper.DeserializeWithWebDefaults<PublicApi.DTO.v1.TransportNeed>(updatedBody);
            Assert.NotNull(updatedTransportNeedFromApi);
            Assert.Equal("abc", updatedTransportNeedFromApi!.TransportNeedInfo);
        }
        
    }
}