using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BoundlessDBWeb.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BoundlessDBWeb.Models
{
    public class Transaction
    {
        private BoundlessDbContext context;
        public int TransactionId { set; get; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Required]
        public DateTime Date { set; get; }
        [Required]
        public string ItemName { set; get; }
        public double Coins { set; get; }
        [Required]
        public double UnitaryValue { set; get; }
        public int Quantity { set; get; }
        [Required]
        public string LocationName { set; get; }
        public List<SelectListItem> ItemList { set; get; }
        public List<SelectListItem> LocationList { set; get; }
    }
}
