using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.App;
using BLL.App.DTO;
using BLL.App.DTO.Enums;
using BLL.App.Services;
using Contracts.BLL.App;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.App.EF;
using DAL.App.EF.Mappers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Resources.BLL.App.DTO;
using Xunit;
using Xunit.Abstractions;
using ETransportType = DAL.App.DTO.Enums.ETransportType;
using Location = BLL.App.DTO.Location;
using TransportMeta = BLL.App.DTO.TransportMeta;
using TransportNeed = DAL.App.DTO.TransportNeed;

namespace TestProject.UnitTests
{
    public class TestTransportNeedService
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly AppDbContext _ctx;
        // private readonly Mock<ITransportNeedRepository> _transportNeedRepository;
        // private readonly Mock<IAppUnitOfWork> _uow;
        // private readonly Mock<IMapper> _mapper;
        // private readonly TransportNeedService _transportNeedService;
        // private readonly Mock<IMapper> _bllMapper;
        //private readonly Mock<BLL.App.Mappers.TransportNeedMapper> _bllTransportNeedMapper;
        private readonly AppBLL _bll;
        
        public TestTransportNeedService(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;

            // set up db context for testing - using InMemory db engine
            var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
            // provide new random database name here
            // or parallel test methods will conflict each other
            optionBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            _ctx = new AppDbContext(optionBuilder.Options);
            _ctx.Database.EnsureDeleted();
            _ctx.Database.EnsureCreated();
            using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var config2 = new MapperConfiguration(cfg => {
                cfg.CreateMap<DAL.App.DTO.TransportNeed, Domain.App.TransportNeed>().ReverseMap();
                cfg.CreateMap<DAL.App.DTO.TransportMeta, Domain.App.TransportMeta>().ReverseMap();
                cfg.CreateMap<DAL.App.DTO.Location, Domain.App.Location>().ReverseMap();
            });
            IMapper iMapperDom = config2.CreateMapper();
            
            
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<BLL.App.DTO.TransportNeed, DAL.App.DTO.TransportNeed>().ReverseMap();
                cfg.CreateMap<BLL.App.DTO.TransportMeta, DAL.App.DTO.TransportMeta>().ReverseMap();
                cfg.CreateMap<BLL.App.DTO.Location, DAL.App.DTO.Location>().ReverseMap();
            });
            IMapper iMapper = config.CreateMapper();
            // // _transportNeedRepository = new Mock<ITransportNeedRepository>();
            // _uow = new Mock<IAppUnitOfWork>();
            // _mapper = new Mock<IMapper>();
            // _bllMapper = new Mock<IMapper>();
            // _transportNeedRepository = new Mock<ITransportNeedRepository>();
            //
            // _transportNeedService = new TransportNeedService(_uow.Object, _transportNeedRepository.Object, iMapper);
            //     
            //
            // _bll = new AppBLL(_uow.Object, _mapper.Object);
            //
            // _uow.Setup(w => w.TransportNeeds.Add(
            //     It.Is<TransportNeed>(
            //         t => t == 
            //              _mapper.Object.Map<DAL.App.DTO.TransportNeed>( GetTransportNeed())
            //              ))).Returns(_mapper.Object.Map<DAL.App.DTO.TransportNeed>(GetTransportNeed()));
            
            // _mapper.Setup(d => d.Map<TransportNeed>(
            //     It.Is<TransportNeed>(
            //         t => t == GetDalTransportNeed()))).Returns(GetDalTransportNeed());
            // _mapper.Setup(d => d.Map<TransportNeed>(
            //     It.Is<TransportNeed>(
            // //         t => t == GetDalTransportNeed()))).Returns(GetDalTransportNeed());
            // _uow.Setup(w => w.TransportNeeds.Add(
            //     It.Is<TransportNeed>(
            //         t => t ==
            //              GetDalTransportNeed()
            //     ))).Returns(GetDalTransportNeed());
            //
            // _transportNeedRepository.Setup(n => n.Add(
            //     It.Is<TransportNeed>(
            //         t => t == GetDalTransportNeed()
            //     ))).Returns(GetDalTransportNeed);


            var work = new AppUnitOfWork(_ctx, iMapperDom);
            _bll = new AppBLL(work, iMapper);
        }

       [Fact]
        public async Task Add_TransportNeed()
        {
           
            var result = _bll.TransportNeeds.Add(GetBLLTransportNeed());
            await _bll.SaveChangesAsync();
            _testOutputHelper.WriteLine(result.Id.ToString());
            Assert.NotNull(result);

            var getAllRes = await _bll.TransportNeeds.GetAllAsync();
            getAllRes.Should().NotBeNull().And.HaveCount(1);
        }

        [Fact]
        public async Task Test_Update_TransportNeed()
        {
            var result = await SaveTransportNeed(_bll);
            _testOutputHelper.WriteLine(result.Id.ToString());
            Assert.NotNull(result);
            var oldInfo = result.TransportNeedInfo;

            _ctx.ChangeTracker.Clear();
            result.TransportNeedInfo = "updated";
            
            var updatedTransportNeed = _bll.TransportNeeds.Update(result);
            await _bll.SaveChangesAsync();
            Assert.Equal("updated", updatedTransportNeed.TransportNeedInfo);
            Assert.Equal(result.Id, updatedTransportNeed.Id);
        }

        //[Fact]
        // public async Task GetsById_Test()
        // {
        //     var result = _bll.TransportNeeds.Add(GetBLLTransportNeed());
        //     await _bll.SaveChangesAsync();
        //     var id = result.Id;
        //     _testOutputHelper.WriteLine("ID: " + id.ToString());
        //     _ctx.ChangeTracker.Clear();
        //     
        //     var all = await _bll.TransportNeeds.GetAllAsync();
        //     _testOutputHelper.WriteLine(all.Count().ToString());
        //
        //     var byId = await _bll.TransportNeeds.FirstOrDefaultAsync(id);
        //     // if (byId == null) throw new ArgumentNullException(nameof(byId));
        //     // _testOutputHelper.WriteLine(byId!.Id.ToString());
        //     // Assert.NotNull(byId!);
        // }

        [Fact]
        public async Task GetByCount_Test()
        {
            await AddMultipleTransportNeeds(6, _bll);

            var result = await _bll.TransportNeeds.GetByCountAsync(3);
            result.Should().NotContainNulls().And.HaveCount(3);
        }

        // [Fact]
        // public async Task CanRemove_TransportNeed_Test()
        // {
        //     await AddMultipleTransportNeeds(6, _bll);
        //
        //     var transportNeeds = await _bll.TransportNeeds.GetAllAsync();
        //     var list = transportNeeds.ToList();
        //     var count = list.Count();
        //
        //     await _bll.TransportNeeds.RemoveAsync(list.First().Id, list.First().AppUserId);
        //
        //     var newTransportNeedList = await _bll.TransportNeeds.GetAllAsync();
        //     var newCount = newTransportNeedList.Count();
        //     newCount.Should().BeLessThan(count).And.Should().BeEquivalentTo(count - 1);
        // }


        public async Task AddMultipleTransportNeeds(int count, IAppBLL bll)
        {
            for (int i = 0; i < count; i++)
            {
                var transportNeed = GetBLLTransportNeed();
                transportNeed.PersonCount = i + 1;
                transportNeed.TransportMeta!.StartTime = DateTime.Now.AddDays(i + 1);
                bll.TransportNeeds.Add(transportNeed);
            }

            await bll.SaveChangesAsync();
        }
        public async Task<BLL.App.DTO.TransportNeed> SaveTransportNeed(IAppBLL bll)
        {
            
            var result = _bll.TransportNeeds.Add(GetBLLTransportNeed());
            await _bll.SaveChangesAsync();
            return result;
        }
        public BLL.App.DTO.TransportNeed GetBLLTransportNeed()
        {
            var transportNeed = new  BLL.App.DTO.TransportNeed()
            {
                AppUserId = Guid.Parse("10000000-0000-0000-0000-000000000001"),
                TransportType = BLL.App.DTO.Enums.ETransportType.PersonsOnly,
                PersonCount = 2,
                TransportNeedInfo = "eeee",
                TransportMeta = new  BLL.App.DTO.TransportMeta()
                {
                    StartTime = DateTime.Now.AddDays(4),
                    StartLocation = new  BLL.App.DTO.Location()
                    {
                        Country = "Est",
                        City = "Tln",
                        Address = "abc-123"
                    },
                    DestinationLocation = new  BLL.App.DTO.Location()
                    {
                        Country = "Est",
                        City = "tartu",
                        Address = "cba",
                    } 
                }
                
            };

            return transportNeed;
        }
        
        public DAL.App.DTO.TransportNeed GetDalTransportNeed()
        {
            var transportNeed = new  DAL.App.DTO.TransportNeed()
            {
                AppUserId = Guid.Parse("10000000-0000-0000-0000-000000000001"),
                TransportType = DAL.App.DTO.Enums.ETransportType.PersonsOnly,
                PersonCount = 2,
                TransportNeedInfo = "eeee",
                TransportMeta = new  DAL.App.DTO.TransportMeta()
                {
                    StartTime = DateTime.Now.AddDays(4),
                    StartLocation = new  DAL.App.DTO.Location()
                    {
                        Country = "Est",
                        City = "Tln",
                        Address = "abc-123"
                    },
                    DestinationLocation = new  DAL.App.DTO.Location()
                    {
                        Country = "Est",
                        City = "tartu",
                        Address = "cba",
                    } 
                }
                
            };

            return transportNeed;
        }
    }
}
