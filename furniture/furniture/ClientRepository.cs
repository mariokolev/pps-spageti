using furniture.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace furniture
{
    public class ClientRepository
    {
        private NpgsqlConnection connection;

        public ClientRepository()
        {
            this.connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString);
        }

        public List<Client> FindAll()
        {
            List<Client> clients = new List<Client>();

            this.connection.Open();
            var cmd = new NpgsqlCommand("SELECT * from clients", this.connection);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Client client = new Client();
                client.Id = reader.GetInt32(0);
                client.FirstName = (string) reader[1];
                client.SecondName = (string) reader[2];
                client.PhoneNumber = (string) reader[3];
                client.DeliveryAddress = (string)reader[4];
                client.CreatedAt = (DateTime)reader[5];

                clients.Add(client);
            }

            this.connection.Close();

            return clients;
        }

        public Client FindById(long id)
        {
            this.connection.Open();
            var cmd = new NpgsqlCommand("SELECT * from clients where id=@id", this.connection);
            cmd.Parameters.AddWithValue("id", id);
            var reader = cmd.ExecuteReader();
            Client client = new Client();

            while (reader.Read())
            {
                client.Id = reader.GetInt32(0);
                client.FirstName = (string)reader[1];
                client.SecondName = (string)reader[2];
                client.PhoneNumber = (string)reader[3];
                client.DeliveryAddress = (string)reader[4];
                client.CreatedAt = (DateTime)reader[5];
            }

            this.connection.Close();

            return client;
        }

        public void Update(Client client)
        {
            this.connection.Open();

            using (var cmd = new NpgsqlCommand("UPDATE clients SET first_name = @first_name, second_name = @second_name, phone_number = @phone_number, delivery_address = @delivery_address"
            + " WHERE id = @id", this.connection))
            {
                cmd.Parameters.Add(new NpgsqlParameter("@first_name", NpgsqlTypes.NpgsqlDbType.Text));
                cmd.Parameters["@first_name"].Value = client.FirstName;

                cmd.Parameters.Add(new NpgsqlParameter("@second_name", NpgsqlTypes.NpgsqlDbType.Text));
                cmd.Parameters["@second_name"].Value = client.SecondName;

                cmd.Parameters.Add(new NpgsqlParameter("@phone_number", NpgsqlTypes.NpgsqlDbType.Text));
                cmd.Parameters["@phone_number"].Value = client.PhoneNumber;

                cmd.Parameters.Add(new NpgsqlParameter("@delivery_address", NpgsqlTypes.NpgsqlDbType.Text));
                cmd.Parameters["@delivery_address"].Value = client.DeliveryAddress;

                cmd.Parameters.Add(new NpgsqlParameter("@id", NpgsqlTypes.NpgsqlDbType.Integer));
                cmd.Parameters["@id"].Value = client.Id;

                cmd.ExecuteNonQuery();
            }

            this.connection.Close();
        }

        public void Save(Client client)
        {
            this.connection.Open();

            using (var cmd = new NpgsqlCommand("INSERT INTO clients(first_name, second_name, phone_number, delivery_address)" +
                "VALUES(@first_name, @second_name, @phone_number, @delivery_address)", this.connection))
            {
                cmd.Parameters.AddWithValue("first_name", client.FirstName);
                cmd.Parameters.AddWithValue("second_name", client.SecondName);
                cmd.Parameters.AddWithValue("phone_number", client.PhoneNumber);
                cmd.Parameters.AddWithValue("delivery_address", client.DeliveryAddress);

                cmd.ExecuteNonQuery();
            }

            this.connection.Close();
        }
        
        public List<Client> FindAllByDeliveryAddress(string deliveryAddress)
        {
            this.connection.Open();

            var cmd = new NpgsqlCommand("SELECT * from clients WHERE delivery_address LIKE @delivery_address", this.connection);
            cmd.Parameters.AddWithValue("delivery_address", "%" + deliveryAddress + "%");
            var reader = cmd.ExecuteReader();

            List<Client> clients = new List<Client>();

            while (reader.Read())
            {
                Client client = new Client();
                client.Id = reader.GetInt32(0);
                client.FirstName = (string)reader[1];
                client.SecondName = (string)reader[2];
                client.PhoneNumber = (string)reader[3];
                client.DeliveryAddress = (string)reader[4];
                client.CreatedAt = (DateTime)reader[5];

                clients.Add(client);
            }

            this.connection.Close();

            return clients;
        }

        public ClientMostOrdersDTO FindClientWithMostOrders()
        {
            this.connection.Open();

            var cmd = new NpgsqlCommand("SELECT count(1) as cnt, clients.first_name, clients.second_name, clients.delivery_address, clients.phone_number " +
                "FROM public.orders " +           
                "INNER JOIN clients ON orders.client_id = clients.id " +
                "GROUP BY clients.id, clients.first_name, clients.second_name, clients.delivery_address, " +
                "clients.phone_number ORDER BY cnt DESC LIMIT 1", this.connection);
            
            var reader = cmd.ExecuteReader();
            ClientMostOrdersDTO client = new ClientMostOrdersDTO();

            while (reader.Read())
            {
                client.NumberOfOrders = reader.GetInt32(0);
                client.FirstName = (string)reader[1];
                client.SecondName = (string)reader[2];
                client.DeliveryAddress = (string)reader[3];
                client.PhoneNumber = (string)reader[4];
            }

            this.connection.Close();

            return client;
        }
    }
}