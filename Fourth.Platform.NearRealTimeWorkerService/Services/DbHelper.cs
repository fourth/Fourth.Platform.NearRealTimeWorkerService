using Fourth.Platform.NearRealTimeWorkerService.Data;
using Fourth.Platform.RealTimeWorkerService.Model;
using Microsoft.EntityFrameworkCore;

namespace Fourth.Platform.NearRealTimeWorkerService.Services
{
    public class DbHelper
    {
        private ModelContext dbContext;
        private DbContextOptions<ModelContext> GetAllOptions()
        { 
            var optionBuilder = new DbContextOptionsBuilder<ModelContext>();
            optionBuilder.UseSqlServer(AppSettings.ConnectionString);
            return optionBuilder.Options;
        }

        public void SaveTransaction(Transact transaction)
        {
            using (dbContext = new ModelContext(GetAllOptions()))
            {
                dbContext.Transact.Add(transaction);
                dbContext.SaveChanges();
            }
        }
    }
}
