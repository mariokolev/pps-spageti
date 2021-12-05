using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace furniture.Models
{
    public class Client
    {
        [DisplayName("Код на клиент")]
        public int Id { get; set; }

        [DisplayName("Име")]
        public string FirstName { get; set; }

        [DisplayName("Фамилия")]
        public string SecondName { get; set; }

        [DisplayName("Телефон")]
        public string PhoneNumber { get; set; }

        [DisplayName("Адрес")]
        public string DeliveryAddress { get; set; }

        [DisplayName("Дата на добавяне")]
        public DateTime CreatedAt { get; set; }
    }
}