using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoundlessDBWeb.Data;

namespace BoundlessDBWeb.Models
{
    public class Transaction
    {
        private BoundlessDbContext context;
        public int TransactionId { set; get; }
        public DateTime Date { set; get; }
        public int ItemId { set; get; }
        public double Coins { set; get; }
        public double UnitaryValue { set; get; }
        public int Quantity { set; get; }
    }
}
