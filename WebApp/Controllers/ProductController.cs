using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ProductController : Controller
    {
        IConfiguration config;
        HttpClient client;
        Uri baseAddress;
        public ProductController(IConfiguration _config)
        {
            config = _config;

            baseAddress = new Uri(config["ApiAddress"]);
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<ProductModel> model = new List<ProductModel>();
            //asynchronous
            var response = await client.GetAsync(client.BaseAddress + "/product/getproducts");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                model = JsonSerializer.Deserialize<IEnumerable<ProductModel>>(data);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(ProductModel model)
        {
            string strData = JsonSerializer.Serialize(model);
            StringContent content = new StringContent(strData, Encoding.UTF8, "application/json");
            var response = client.PostAsync(client.BaseAddress + "/product/addproduct", content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Edit(int id)
        { 
            ProductModel model = new ProductModel();
            var response = client.GetAsync(client.BaseAddress + "/product/getproduct/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                model = JsonSerializer.Deserialize<ProductModel>(data);
            }
            return View("Create", model);
        }

        [HttpPost]
        public IActionResult Edit(ProductModel model)
        {
            string strData = JsonSerializer.Serialize(model);
            StringContent content = new StringContent(strData, Encoding.UTF8, "application/json");
            var response = client.PutAsync(client.BaseAddress + "/product/updateproduct/" + model.ProductId, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View("Create", model);
        }

        public IActionResult Delete(int id)
        {
            var response = client.DeleteAsync(client.BaseAddress + "/product/deleteproduct/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
