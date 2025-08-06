using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace ProjectManagementAPI
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

    }
}
