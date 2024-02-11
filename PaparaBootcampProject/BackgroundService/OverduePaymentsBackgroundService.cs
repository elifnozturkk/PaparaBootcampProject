namespace PaparaApp.Project.API.BackgroundService;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PaparaApp.Project.API.Enums;
using PaparaApp.Project.API.Models;
using System.Threading;
using System.Threading.Tasks;

public class OverduePaymentsBackgroundService(IServiceProvider serviceProvider) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await CheckAndApplyOverdueCharges(stoppingToken);
            await Task.Delay(TimeSpan.FromDays(30), stoppingToken);
        }
    }

    private async Task CheckAndApplyOverdueCharges(CancellationToken stoppingToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;

            var overduePayments = await dbContext.Payments
                .Where(p => !p.PaymentDate.HasValue
                            && ((p.Year < currentYear) || (p.Year == currentYear && p.Month < currentMonth))
                            && p.PaymentStatus == PaymentStatus.Pending)
                .ToListAsync(stoppingToken);

            overduePayments.ForEach(p => p.Amount += p.Amount * 0.1m);

            await dbContext.SaveChangesAsync(stoppingToken);
        }

       
    }
}

