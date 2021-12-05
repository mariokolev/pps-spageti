using furniture.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace furniture.Controllers
{
    public class ClientController : Controller
    {
        ClientRepository clientRepository;

        // GET: Client
        public ActionResult Index(bool? is_order)
        {
            clientRepository = new ClientRepository();
            List<Client> clients = clientRepository.FindAll();

            ViewData["clients"] = clients;
            ViewBag.Title = "Клиенти";

            if (is_order == true)
            {
                ViewData["is_order"] = true;
                ViewBag.Title = "Избери клиент";
            } 

            return View(clients);
        }

        // GET: Client/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Client/Create
        public ActionResult Create()
        {
            ViewBag.Title = "Добавяне на клиент";

            return View();
        }

        // POST: Client/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            this.clientRepository = new ClientRepository();

            try
            {
                Client client = new Client();
                client.FirstName = collection["FirstName"];
                client.SecondName = collection["SecondName"];
                client.PhoneNumber = collection["PhoneNumber"];
                client.DeliveryAddress = collection["DeliveryAddress"];
                
                this.clientRepository.Save(client);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Client/Edit/5
        public ActionResult Edit(int id)
        {
            this.clientRepository = new ClientRepository();
            Client client  = this.clientRepository.FindById(id);

            ViewData["client"] = client;
            ViewBag.Title = "Редактиране на данни за клиент";

            return View(client);
        }

        // POST: Client/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            this.clientRepository = new ClientRepository();

            try
            {
                Client client = clientRepository.FindById(id);
                client.FirstName = collection["FirstName"];
                client.SecondName = collection["SecondName"];
                client.PhoneNumber = collection["PhoneNumber"];
                client.DeliveryAddress = collection["DeliveryAddress"];

                this.clientRepository.Update(client);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Client/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Client/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
