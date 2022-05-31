using Fourth.Platform.RealTimeWorkerService.Model;
using Microsoft.EntityFrameworkCore;

namespace Fourth.Platform.NearRealTimeWorkerService.Data
{
    public class ModelContext : DbContext
    {
        public ModelContext(DbContextOptions<ModelContext> options) : base(options)
        {
        }

        public DbSet<Transact> Transact { get; set; }
    }
}
