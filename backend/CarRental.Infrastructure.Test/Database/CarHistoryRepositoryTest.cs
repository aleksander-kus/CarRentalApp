using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CarRental.Domain.Entity;
using CarRental.Infrastructure.Database;
using CarRental.Infrastructure.Test.Async;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CarRental.Infrastructure.Test.Database
{
    public class CarHistoryRepositoryTest
    {
        private readonly CarHistoryRepository _carHistoryRepository;
        private readonly CarHistoryEntry[] _entries = {
            new()
            {
                ID = 0,
                UserId = "xxx",
                UserEmail = "xxx@gmail.com",
                CarId = "1",
                ProviderId = "1",
                CarModel = "Opel",
                CarBrand = "Astra",
                CarProvider = "DNR",
                StartDate = DateTime.Parse("2022-01-01"),
                EndDate = DateTime.Parse("2022-01-04"),
                Returned = false
            },
            new()
            {
                ID = 1,
                UserId = "xxx",
                UserEmail = "xxx@gmail.com",
                CarId = "1",
                ProviderId = "1",
                CarModel = "Opel",
                CarBrand = "Astra",
                CarProvider = "DNR",
                StartDate = DateTime.Parse("2022-01-01"),
                EndDate = DateTime.Parse("2022-01-04"),
                Returned = true
            },
            new()
            {
                ID = 2,
                UserId = "yyy",
                UserEmail = "yyy@gmail.com",
                CarId = "2",
                ProviderId = "1",
                CarModel = "Opel",
                CarBrand = "Corsa",
                CarProvider = "DNR",
                StartDate = DateTime.Parse("2022-01-01"),
                EndDate = DateTime.Parse("2022-01-04"),
                Returned = false
            },
        };

        public CarHistoryRepositoryTest()
        {
            var data = _entries.AsQueryable();
            var mockSet = new Mock<DbSet<CarHistoryEntry>>();
            mockSet.As<IAsyncEnumerable<CarHistoryEntry>>().Setup(d => d.GetAsyncEnumerator(new CancellationToken()))
                .Returns(new AsyncEnumerator<CarHistoryEntry>(data.GetEnumerator()));
            mockSet.As<IQueryable<CarHistoryEntry>>().Setup(m => m.Provider).Returns(new AsyncQueryProvider<CarHistoryEntry>(data.Provider));
            mockSet.As<IQueryable<CarHistoryEntry>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<CarHistoryEntry>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<CarHistoryEntry>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        
            var mockContext = new Mock<CarRentalContext>();
            mockContext.Setup(c => c.CarHistory).Returns(mockSet.Object);
            
            _carHistoryRepository = new CarHistoryRepository(mockContext.Object);
        }

        private int[] GetIDs(IEnumerable<CarHistoryEntry> entries) => entries.Select(entry => entry.ID).ToArray();

        [Fact]
        public async Task ShouldReturnNotReturnedCarsByUserAsync()
        {
            var entries = await _carHistoryRepository.GetActiveHistoryEntriesOfUserAsync("xxx");
            
            Assert.Equal(new []{0}, GetIDs(entries));
        }

        [Fact]
        public async Task ShouldReturnAllCarsRentedByUserAsync()
        {
            var entries = await _carHistoryRepository.GetHistoryEntriesOfUserAsync("xxx");
            
            Assert.Equal(new []{0, 1}, GetIDs(entries));
        }
        
        [Fact]
        public async Task ShouldReturnNotReturnedCarsAsync()
        {
            var entries = await _carHistoryRepository.GetActiveHistoryEntriesAsync();
            
            Assert.Equal(new []{0, 2}, GetIDs(entries));
        }
        
        [Fact]
        public async Task ShouldReturnAllEntriesAsync()
        {
            var entries = await _carHistoryRepository.GetHistoryEntriesAsync();
            
            Assert.Equal(new []{0, 1, 2}, GetIDs(entries));
        }
    }
}