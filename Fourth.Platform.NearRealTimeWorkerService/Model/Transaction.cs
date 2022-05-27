using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fourth.Platform.RealTimeWorkerService.Model
{
    public class Transaction
    {
        public string Id { get; set; }
        public string PartitionKey { get; set; }
        public string LocationId { get; set; }
        public string ClientId { get; set; }
        public DateTime BusinessDate { get; set; }
        public string UnitId { get; set; }
        public string SiteLocationCode { get; set; }
        public DateTime TradingDate { get; set; }
        public string Time { get; set; }
        public string TerminalCode { get; set; }
        public string RecordActivityCode { get; set; }
        public string ReceiptCode { get; set; }
        public string CheckCode { get; set; }
        public string TableCode { get; set; }
        public string RevenueCentreCode { get; set; }
        public string RevenueCentreDesc { get; set; }
        public string TransactionTypeCode { get; set; }
        public string SalesItemPLU { get; set; }
        public string SalesItemDesc { get; set; }
        public int Covers { get; set; }
        public decimal Qty { get; set; }
        public string Currency { get; set; }
        public decimal ListPrice { get; set; }
        public decimal Tax { get; set; }
        public decimal PricePaid { get; set; }
        public decimal Discount { get; set; }
        public string TabOwner { get; set; }
        public string TabOwnerDesc { get; set; }
        public string UniversalTimeSlotId { get; set; }
    }
}
