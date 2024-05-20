using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infra.EFCore.Contexts;
internal class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext> {   
    public AppDbContext CreateDbContext(string[] args) {
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseSqlServer(ConnectionStrings.GRPCChatDb);
        return new AppDbContext(builder.Options);
    }
}
