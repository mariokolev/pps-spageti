using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace furniture.Models
{
    public class Item
    {
        [DisplayName("Код на артикул")]
        public int Id { get; set; }

        [DisplayName("Име на артикул")]
        public string ItemName { get; set; }

        [DisplayName("Описание")]
        public string Description { get; set; }

        [DisplayName("Цена за бр.")]
        public double Price { get; set; }

        [DisplayName("Дата на производство")]
        public DateTime DateOfManufacture { get; set; }

        [DisplayName("Количество")]
        public int Quantity { get; set; }

        [DisplayName("Дата на добавяне")]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public override string ToString()
        {
            return "ID: " + Id + " itemName: " + ItemName + " Description: " + Description +
                " Price: " + Price + " Date of manufacture: " + DateOfManufacture + " Quantity: " + Quantity
                + " Created at: " + CreatedAt + " Updated at: " + UpdatedAt;
        }
    }
}