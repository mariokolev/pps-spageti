using furniture.Models;
using FurnitureShop;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace furniture
{
    public class OrderRepository
    {
        private NpgsqlConnection connection;
        private ItemRepository itemRepository;

        public OrderRepository()
        {
            this.connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString);
        }
        public List<Order> FindAll()
        {
            List<Order> orders = new List<Order>();

            this.connection.Open();
            
            var cmd = new NpgsqlCommand("SELECT orders.id, client_id, total_price, orders.created_at " +
                "FROM orders " +
                "JOIN clients ON orders.client_id = clients.id ORDER BY orders.id DESC", this.connection);
            
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Order order = new Order();
                Client client = new Client();
                order.Id = reader.GetInt32(0);
                client.Id = reader.GetInt32(1);

                order.Client = client;
                
                order.TotalPrice = reader.GetDouble(2);
                order.CreatedAt = (DateTime)reader[3];

                orders.Add(order);
            }

            this.connection.Close();

            return orders;
        }

        // Find order details by order id.
        public Order FindOrderDetailsById(int id)
        {
            this.connection.Open();
           
            var cmd = new NpgsqlCommand("SELECT orders.id, client_id, total_price, orders.created_at, " +
                "clients.first_name, clients.second_name, clients.delivery_address, clients.phone_number " +
                "FROM orders " +
                "JOIN clients on orders.client_id = clients.id " +
                "WHERE orders.id = @id", this.connection);

            cmd.Parameters.AddWithValue("id", id);
            var reader = cmd.ExecuteReader();
            reader.Read(); 

            Order order = new Order();

            // Order columns.
            order.Id = reader.GetInt32(0);
            order.TotalPrice = reader.GetDouble(2);
            order.CreatedAt = (DateTime)reader[3];

            Client client = new Client();

            // Set Client columns
            client.Id = reader.GetInt32(1);
            client.FirstName = (string)reader[4];
            client.SecondName = (string)reader[5];
            client.DeliveryAddress = (string)reader[6];
            client.PhoneNumber = (string)reader[7];

            // Set the client.
            order.Client = client;
            reader.Close();
            this.connection.Close();

            List<OrderDetails> orderDetails = new List<OrderDetails>();

            this.connection.Open();
            // Find all order_details
            var findAll = new NpgsqlCommand("SELECT items.id as item_id, items.item_name, items.price, order_details.quantity_ordered, " +
                "order_details.total_price, orders.id as order_id, orders.total_price as final_price, " +
                "orders.created_at " +
                "FROM order_details " +
                "JOIN orders ON order_details.order_id = orders.id " +
                "JOIN items ON order_details.item_id = items.id " +
                "WHERE order_id = @id", this.connection);

            findAll.Parameters.AddWithValue("id", id);
            var findAllReader = findAll.ExecuteReader();
            
            while (findAllReader.Read())
            {
                OrderDetails orderDetail = new OrderDetails();

                // Set item columns.
                Item item = new Item();
                item.Id = findAllReader.GetInt32(0);
                item.ItemName = (string)findAllReader[1];
                item.Price = findAllReader.GetDouble(2);
                // Set order_details columns.
                orderDetail.QuantityOrdered = findAllReader.GetInt32(3);
                orderDetail.TotalPrice = findAllReader.GetDouble(4);

                orderDetail.Item = item;
                orderDetails.Add(orderDetail);
            }

            order.OrderDetails = orderDetails;
            this.connection.Close();

            return order;
        }

        // Find ordered item by phone number.
        public List<OrderDetails> FindOrderDetailsByPhoneNumber(string phoneNumber)
        {
            List<OrderDetails> orders = new List<OrderDetails>();

            this.connection.Open();

            // Find all order_details
            var cmd = new NpgsqlCommand("SELECT items.id as item_id, items.item_name, items.price, order_details.quantity_ordered, " +
                "orders.total_price as final_price, orders.created_at " +
                "FROM order_details " +
                "JOIN items ON order_details.item_id = items.id " +
                "JOIN orders ON order_details.order_id = orders.id " +
                "JOIN clients ON orders.client_id = clients.id " +
                "WHERE clients.phone_number = @phone_number", this.connection);

            cmd.Parameters.AddWithValue("phone_number", phoneNumber);

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                OrderDetails orderDetails = new OrderDetails();

                // Set item columns.
                Item item = new Item();
                item.Id = reader.GetInt32(0);
                item.ItemName = (string)reader[1];
                item.Price = reader.GetDouble(2);

                // Set order_details columns.
                orderDetails.QuantityOrdered = reader.GetInt32(3);
                orderDetails.TotalPrice = reader.GetDouble(4);

                orderDetails.Item = item;
                orders.Add(orderDetails);
            }

            this.connection.Close();

            return orders;
        }

        // Find orders of item before date.
        public List<OrderDetails> FindOrderDetailsBeforeDate(string itemName, DateTime dateCreated)
        {
            List<OrderDetails> orders = new List<OrderDetails>();

            this.connection.Open();

            // Find all order_details
            var cmd = new NpgsqlCommand("SELECT  order_id, item_id, item_name, items.price, order_details.quantity_ordered," +
                "order_details.total_price, order_details.created_at " +
                "FROM order_details " +
                "JOIN items ON order_details.item_id = items.id " +
                "WHERE order_details.created_at < @created_at AND item_name = @item_name", this.connection);

            cmd.Parameters.AddWithValue("created_at", dateCreated);
            cmd.Parameters.AddWithValue("item_name", itemName);

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                OrderDetails orderDetails = new OrderDetails();

                orderDetails.OrderId = reader.GetInt32(0);
                
                // Set item columns.
                Item item = new Item();
                item.Id = reader.GetInt32(1);
                item.ItemName = (string)reader[2];
                item.Price = reader.GetDouble(3);
                // Set order_details columns.
                orderDetails.QuantityOrdered = reader.GetInt32(4);
                orderDetails.TotalPrice = reader.GetDouble(5);
                orderDetails.CreatedAt = (DateTime)reader[6];

                orderDetails.Item = item;
                orders.Add(orderDetails);
            }

            this.connection.Close();

            return orders;
        }

        public Order Save(int clientId, Dictionary<string, string> itemAndQuantity)
        {
            itemRepository = new ItemRepository(); 
            List<string> itemIds = new List<string>();

            // Collect item ids.
            foreach (string key in itemAndQuantity.Keys)
            {
                itemIds.Add(key);
            }

            Dictionary<double, Dictionary<string, string>> priceItemAndQuantity = new Dictionary<double, Dictionary<string, string>>();
            List<Item> items = itemRepository.FindAllById(itemIds.ToArray());
            Order order = new Order();

            // Contains total price of order 
            double totalPrice = 0;
            
            for (int index = 0; index < items.Count; index++)
            {
                // Calculate quantity of items before SQL update.
                items[index].Quantity -= Int32.Parse(itemAndQuantity[items[index].Id.ToString()]);
                itemRepository.Update(items[index]);

                // calculate price per quantity
                double price = items[index].Price * Int32.Parse(itemAndQuantity[items[index].Id.ToString()]);

                // add to total price
                totalPrice += price;

                Dictionary<string, string> currentPair = new Dictionary<string, string>();
                currentPair.Add(items[index].Id.ToString(), itemAndQuantity[items[index].Id.ToString()]);

                // Save price and item-quantity pair.
                priceItemAndQuantity.Add(price, currentPair);
            }

            this.connection.Open();

            // Set default to 0
            int orderId = 0;

            // Calculate tax.
            totalPrice = totalPrice * 1.2;
            
            using (var cmd = new NpgsqlCommand("INSERT INTO orders(client_id, total_price) " +
                "VALUES(@client_id, @total_price) RETURNING id", this.connection))
            {
                cmd.Parameters.AddWithValue("client_id", clientId);
                cmd.Parameters.AddWithValue("total_price", totalPrice);

                var reader = cmd.ExecuteReader();
                reader.Read();
                orderId = reader.GetInt32(0);
            }

            this.connection.Close();

            // save order details
            this.connection.Open();

            // Save each order details
            foreach (var orderInfo in priceItemAndQuantity)
            {
                string itemId = orderInfo.Value.Keys.ElementAt(0);
                string quantityOrdered = orderInfo.Value[orderInfo.Value.Keys.ElementAt(0)];

                using (var cmd = new NpgsqlCommand("INSERT INTO order_details(order_id, item_id, quantity_ordered, total_price) " +
                    "VALUES(@order_id, @item_id, @quantity_ordered, @total_price)", this.connection))
                {
                    cmd.Parameters.AddWithValue("order_id", orderId);
                    cmd.Parameters.AddWithValue("item_id", Int32.Parse(itemId));
                    cmd.Parameters.AddWithValue("quantity_ordered", Int32.Parse(quantityOrdered));
                    cmd.Parameters.AddWithValue("total_price", orderInfo.Key);

                    cmd.ExecuteNonQuery();
                }
            }

            this.connection.Close();

            return this.FindOrderDetailsById(orderId);
        }
    }
}