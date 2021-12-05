using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace furniture.Models
{
    public class Order
    {
        [DisplayName("Код на продажба")]
        public int Id { get; set; }

        public Client Client { get; set; }

        [DisplayName("Цена с ДДС")]
        public double TotalPrice { get; set; }

        [DisplayName("Дата на продажба")]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public List<OrderDetails> OrderDetails { get; set; }
    }
}