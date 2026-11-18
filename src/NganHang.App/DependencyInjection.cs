using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NganHang.Core.Data;
using NganHang.Core.Services;

namespace NganHang.Core;

public static class DependencyInjection
{
    /// <summary>Đăng ký DbContext và toàn bộ service nghiệp vụ.</summary>
    public static IServiceCollection AddNganHangCore(this IServiceCollection services, string connectionString)
    {
        services.AddDbContextFactory<CoreBankingDbContext>(o =>
            o.UseSqlServer(connectionString, sql => sql.CommandTimeout(30)));

        services.AddSingleton<PhienLamViec>();
        services.AddSingleton<AuthService>();
        services.AddSingleton<CustomerService>();
        services.AddSingleton<AccountService>();
        services.AddSingleton<SavingsService>();
        services.AddSingleton<LoanService>();
        services.AddSingleton<AmlService>();
        services.AddSingleton<ReportService>();
        return services;
    }
}
