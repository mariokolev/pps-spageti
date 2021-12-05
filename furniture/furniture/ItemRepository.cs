using furniture;
using furniture.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace FurnitureShop
{
    class ItemRepository
    {
        private NpgsqlConnection connection;

        public ItemRepository()
        {
            this.connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString);
        }

        public List<Item> FindAll()
        {
            List<Item> items = new List<Item>();

            this.connection.Open();
            var cmd = new NpgsqlCommand("SELECT * from items", this.connection);
            var reader = cmd.ExecuteReader();

            while(reader.Read())
            {
                Item item = new Item();
                item.Id = reader.GetInt32(0);
                item.ItemName = (string)reader[1];
                item.Description = (string)reader[2];
                item.Price = (double)reader[3];
                item.DateOfManufacture = (DateTime)reader[4];
                item.Quantity = (int)reader[5];
                item.CreatedAt = (DateTime)reader[6];

                /*
                if (reader[7] != null) {
                    item.UpdatedAt = (DateTime)reader[7];
                }*/
                   
                items.Add(item);
            }

            this.connection.Close();

            return items;
        }

        public Item FindById(long id)
        {
            this.connection.Open();
            var cmd = new NpgsqlCommand("SELECT * from items where id=@id", this.connection);
            cmd.Parameters.AddWithValue("id", id);
            var reader = cmd.ExecuteReader();
            Item item = new Item();

            while (reader.Read())
            {

                item.Id = reader.GetInt32(0);
                item.ItemName = (string)reader[1];
                item.Description = (string)reader[2];
                item.Price = (double)reader[3];
                item.DateOfManufacture = (DateTime)reader[4];
                item.Quantity = (int)reader[5];
                item.CreatedAt = (DateTime)reader[6];

                /*
                if (reader[7] != null) {
                    item.UpdatedAt = (DateTime)reader[7];
                }*/
            }

            this.connection.Close();

            return item;
        }

        public void Save(Item item)
        {
            this.connection.Open();

            using (var cmd = new NpgsqlCommand("INSERT INTO items(item_name, description, price, date_of_manufacture, quantity)" +
                "VALUES(@item_name, @description, @price, @date_of_manufacture, @quantity)", this.connection))
            {
                cmd.Parameters.AddWithValue("item_name", item.ItemName);
                cmd.Parameters.AddWithValue("description", item.Description);
                cmd.Parameters.AddWithValue("price", item.Price);
                cmd.Parameters.AddWithValue("date_of_manufacture", item.DateOfManufacture);
                cmd.Parameters.AddWithValue("quantity", item.Quantity);

                cmd.ExecuteNonQuery();
            }

            this.connection.Close();
        }

        internal MostOrderedItemDTO FindMostOrderedByDeliveryAddress(string city)
        {
            this.connection.Open();
            /*
             * SELECT count(item_id) as cnt, item_id, item_name, clients.delivery_address
FROM public.order_details 
inner join items on order_details.item_id = items.id
inner join orders on order_details.order_id = orders.id
inner join clients on orders.client_id = clients.id
where clients.delivery_address like '%burgas%'
group by item_id, item_name, clients.delivery_address order by cnt desc limit 1
             * 
             */

            var cmd = new NpgsqlCommand("SELECT count(item_id) as cnt, item_id, item_name, clients.delivery_address " +
                "FROM public.order_details " +
                "INNER JOIN items ON order_details.item_id = items.id " +
                "INNER JOIN orders ON order_details.order_id = orders.id " +
                "INNER JOIN clients ON orders.client_id = clients.id " +
                "GROUP BY item_id, item_name, clients.delivery_address ORDER BY cnt DESC LIMIT 1", this.connection);

            var reader = cmd.ExecuteReader();

            MostOrderedItemDTO item = new MostOrderedItemDTO();

            reader.Read();

            item.NumberOfOrders = reader.GetInt32(0);
            item.ItemId = reader.GetInt32(1);
            item.ItemName = (string)reader[2];
            item.DeliveryAddress = (string)reader[3];

            this.connection.Close();

            return item;
        }

        public void Update(Item item)
        {
            this.connection.Open();

            using (var cmd = new NpgsqlCommand("UPDATE items SET item_name = @item_name, description = @description, price = @price, date_of_manufacture = @date_of_manufacture, quantity = @quantity"
            +" WHERE id = @id", this.connection))
            {
                //cmd.Parameters.AddWithValue("id", item.Id);
                

                cmd.Parameters.Add(new NpgsqlParameter("@item_name", NpgsqlTypes.NpgsqlDbType.Text));
                cmd.Parameters["@item_name"].Value = item.ItemName;

                cmd.Parameters.Add(new NpgsqlParameter("@description", NpgsqlTypes.NpgsqlDbType.Text));
                cmd.Parameters["@description"].Value = item.Description;

                cmd.Parameters.Add(new NpgsqlParameter("@price", NpgsqlTypes.NpgsqlDbType.Double));
                cmd.Parameters["@price"].Value = item.Price;

                cmd.Parameters.Add(new NpgsqlParameter("@date_of_manufacture", NpgsqlTypes.NpgsqlDbType.Timestamp));
                cmd.Parameters["@date_of_manufacture"].Value = item.DateOfManufacture;

                cmd.Parameters.Add(new NpgsqlParameter("@quantity", NpgsqlTypes.NpgsqlDbType.Integer));
                cmd.Parameters["@quantity"].Value = item.Quantity;

                cmd.Parameters.Add(new NpgsqlParameter("@id", NpgsqlTypes.NpgsqlDbType.Integer));
                cmd.Parameters["@id"].Value = item.Id;
                
                cmd.ExecuteNonQuery();
            }

            this.connection.Close();
        }

        public MostOrderedItemDTO FindMostOrdered()
        {
            this.connection.Open();

            var cmd = new NpgsqlCommand("SELECT count(item_id) as cnt, item_id, item_name " +
                "FROM public.order_details " +
                "INNER JOIN items ON order_details.item_id = items.id " + 
                "INNER JOIN orders ON order_details.order_id = orders.id " +
                "GROUP BY item_id, item_name order by cnt desc limit 1", this.connection);

            var reader = cmd.ExecuteReader();
            
            MostOrderedItemDTO item = new MostOrderedItemDTO();

            reader.Read();

            item.NumberOfOrders = reader.GetInt32(0);
            item.ItemId = reader.GetInt32(1);
            item.ItemName = (string) reader[2];

            this.connection.Close();

            return item;
        }

        public List<Item> FindAllById(string[] ids)
        {
            this.connection.Open();

            List<Item> items = new List<Item>();
            string id = string.Join(", ", ids);
            var cmd = new NpgsqlCommand("SELECT * from items where id in ( " + id + " )", this.connection);
            // cmd.Parameters.AddWithValue("id", id);
            var reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                Item item = new Item();

                item.Id = reader.GetInt32(0);
                item.ItemName = (string)reader[1];
                item.Description = (string)reader[2];
                item.Price = (double)reader[3];
                item.DateOfManufacture = (DateTime)reader[4];
                item.Quantity = (int)reader[5];
                item.CreatedAt = (DateTime)reader[6];

                items.Add(item);
            }

            this.connection.Close();

            return items;
        }
    }
}
