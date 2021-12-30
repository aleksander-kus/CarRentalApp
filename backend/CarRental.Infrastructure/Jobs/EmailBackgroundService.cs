using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CarRental.Domain.Ports.In;
using Microsoft.Extensions.DependencyInjection;

namespace CarRental.Infrastructure.Jobs
{
    public class EmailBackgroundService: CronBackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        
        public EmailBackgroundService(IScheduleConfig<EmailBackgroundService> config, IServiceScopeFactory scopeFactory):
            base(config.CronExpression, config.TimeZoneInfo)
        {
            _scopeFactory = scopeFactory;
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            
            await scope.ServiceProvider.GetRequiredService<ISendNewCarsEventUseCase>().SendNewCars();
        }
    }
}