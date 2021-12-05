using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace furniture
{
    public class ClientMostOrdersDTO
    {
        [DisplayName("Брой поръчки")]
        public int NumberOfOrders { get; set; }

        [DisplayName("Име")]
        public string FirstName { get; set; }

        [DisplayName("Фамилия")]
        public string SecondName { get; set; }

        [DisplayName("Адрес")]
        public string DeliveryAddress { get; set; }

        [DisplayName("Телефон")]
        public string PhoneNumber { get; set; }
    }
}