using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace furniture
{
    public class MostOrderedItemDTO
    {
        [DisplayName("Брой поръчки")]
        public int NumberOfOrders { get; set; }

        [DisplayName("Код на артикул")]
        public int ItemId { get; set; }

        [DisplayName("Име на артикул")]
        public string ItemName { get; set; }
        public string DeliveryAddress { get; set; }
    }
}