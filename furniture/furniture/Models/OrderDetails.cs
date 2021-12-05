using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace furniture.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }

        [DisplayName("Код на продажба")]
        public int OrderId { get; set; }

        public Item Item { get; set; }

        [DisplayName("Продадено количество")]
        public int QuantityOrdered { get; set; }

        [DisplayName("Цена за количество")]
        public double TotalPrice { get; set; }

        [DisplayName("Дата")]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}