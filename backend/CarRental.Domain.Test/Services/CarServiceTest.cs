using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Entity;
using CarRental.Domain.Ports.In;
using CarRental.Domain.Ports.Out;
using CarRental.Domain.Services;
using Moq;
using Xunit;

namespace CarRental.Domain.Test.Services
{
    public class CarServiceTest
    {
        private Mock<ICarProviderFactory> _carProviderFactory;
        private Mock<ICarProvider> _carProvider;
        private Mock<IGetUserDetailsUseCase> _getUserDetailsUseCase;
        private Mock<ICarReturnEntryRepository> _carReturnRepo;
        private Mock<StorageService> _storageService;
        private Mock<CarHistoryService> _carHistoryService;
        private Mock<CarReturnService> _carReturnService;
        private Mock<EmailService> _emailService;
        private Mock<ICarEmailedEventRepository> _carEmailedEventRepository;
        private CarService _carService;


        public CarServiceTest()
        {
            _carProviderFactory = new Mock<ICarProviderFactory>();
            _getUserDetailsUseCase = new Mock<IGetUserDetailsUseCase>();
            _carProvider = new Mock<ICarProvider>();
            _carHistoryService = new Mock<CarHistoryService>();
            _emailService = new Mock<EmailService>();
            _carEmailedEventRepository = new Mock<ICarEmailedEventRepository>();
            _carReturnRepo = new Mock<ICarReturnEntryRepository>();
            _storageService = new Mock<StorageService>();
            _carReturnService = new Mock<CarReturnService>();

            _carService = new CarService(_carProviderFactory.Object, _getUserDetailsUseCase.Object, _carHistoryService.Object, _emailService.Object,
                _carEmailedEventRepository.Object, _carReturnRepo.Object, _storageService.Object, _carReturnService.Object);
        }
        
        [Fact]
        public async Task ShouldGetCarsFromProvider()
        {
            var expected = new ApiResponse<List<CarDetails>>() { Data = new List<CarDetails>() };

            _carProvider.Setup(p => p.GetCarsAsync(It.IsAny<CarListFilter>())).Returns(Task.FromResult(expected));
            _carProviderFactory.Setup(p => p.GetProviderAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(_carProvider.Object));

            var req = await _carService.GetCarsAsync("test", CarListFilter.All);

            Assert.Equal(expected, req);
        }
        
        [Fact]
        public async Task ShouldBookCarInProvider()
        {
            var expected = new ApiResponse<CarRentResponse>() { Data = new CarRentResponse() { } };

            _carHistoryService.Setup(p => p.MarkHistoryEntryAsConfirmed(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(Task.CompletedTask);
            _carHistoryService.Setup(p => p.GetByProviderAndPriceId(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult<CarHistoryEntry>(null));
            _emailService.Setup(p => p.NotifyUserAfterCarRent(It.IsAny<UserDetails>(), It.IsAny<CarRentRequest>(),
                It.IsAny<CarHistoryEntry>(), It.IsAny<string>())).Returns(Task.CompletedTask);
            _carProvider.Setup(p => p.TryBookCar(It.IsAny<string>(), It.IsAny<CarRentRequest>())).Returns(Task.FromResult(expected));
            _carProviderFactory.Setup(p => p.GetProviderAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(_carProvider.Object));

            var req = await _carService.TryBookCar("xxx", "xxx", "xxx", new CarRentRequest());
            _emailService.Verify(p => p.NotifyUserAfterCarRent(It.IsAny<UserDetails>(), It.IsAny<CarRentRequest>(),
                It.IsAny<CarHistoryEntry>(), It.IsAny<string>()));
            
            Assert.Equal(expected, req);
        }
        
        [Fact]
        public async Task ShouldReturnCarInDatabaseAndCallProvider()
        {
            var expected = new ApiResponse<CarReturnResponse>() { Data = new CarReturnResponse() { } };

            _carReturnService.Setup(p =>
                    p.RegisterCarReturnAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CarReturnRequest>()))
                .Returns(Task.CompletedTask);
            _carHistoryService.Setup(p => p.UpdateCarToReturnedAsync(It.IsAny<int>()))
                .Returns(Task.CompletedTask);
            _emailService.Setup(p => p.NotifyUserAfterCarReturn(It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            _carProvider.Setup(p => p.TryReturnCar(It.IsAny<string>())).Returns(Task.FromResult(expected));
            _carProviderFactory.Setup(p => p.GetProviderAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(_carProvider.Object));
            _storageService.Setup(p => p.ExistsFile(It.IsAny<string>())).Returns(Task.FromResult(true));

            var req = await _carService.TryReturnCar("xxx", "xxx", new CarReturnRequest() { RentId = "xxx", HistoryEntryId = "2"});
            Assert.Equal(expected, req);
        }
        
        [Fact]
        public async Task ShouldCheckPriceInProvider()
        {
            var expected = new ApiResponse<CarPrice>() { Data = new CarPrice() { } };
            var userDetails = new UserDetails() { UserId = "xxx" };

            _getUserDetailsUseCase.Setup(p => p.GetUserDetailsAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(userDetails));
            _carHistoryService.Setup(p => p.RegisterCarRentProcessStartAsync(It.IsAny<string>(), It.IsAny<CarDetails>(),
                It.IsAny<UserDetails>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            _carProvider.Setup(p => p.GetCarsAsync(CarListFilter.All))
                .Returns(Task.FromResult(new ApiResponse<List<CarDetails>>() { Data = new List<CarDetails>() { new CarDetails() { Id = "xxx" } }}));
            _carProvider.Setup(p => p.CheckPrice(It.IsAny<string>(), It.IsAny<int>(), userDetails))
                .Returns(Task.FromResult(expected));
            _carProviderFactory.Setup(p => p.GetProviderAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(_carProvider.Object));

            var req = await _carService.CheckPrice(new CarCheckPrice() { DaysCount = 1 },"xxx", "xxx", "xxx");
            
            _carHistoryService.Verify(p => p.RegisterCarRentProcessStartAsync(It.IsAny<string>(), It.IsAny<CarDetails>(),
                It.IsAny<UserDetails>(), It.IsAny<string>()));
            Assert.Equal(expected, req);
        }
    }
}