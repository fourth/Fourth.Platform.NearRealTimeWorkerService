using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fourth.Platform.RealTimeWorkerService.Model
{
    public partial class Sales
    {
        public int Id { get; set; }
        public string SalesDate { get; set; }
        public int? Amount { get; set; }
    }
}
