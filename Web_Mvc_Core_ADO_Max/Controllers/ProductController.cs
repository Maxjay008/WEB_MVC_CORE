using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Web_Mvc_Core_ADO_Max.Models;
using System.Data.SqlClient; //Добавить через NuGet

namespace Web_Mvc_Core_ADO_Max.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        String connectionString = @"Data Source=DESKTOP-E1P82GJ\SQLEXPRESS;Initial Catalog=BD_Max;Integrated Security=True";


        public ActionResult Select_Product()
        {
            List<Product_Max> prod = new List<Product_Max>(); ;
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCmd = new SqlCommand("Select * from Product_Max", sqlConnection);
                SqlDataReader reader = sqlCmd.ExecuteReader();
                while (reader.Read())
                {
                    prod.Add(new Product_Max() { ID = reader.GetInt32(0), Name = reader.GetString(1), Description = reader.GetString(2), Image_Name = reader.GetString(3), Price = reader.GetDecimal(4) });
                }

                sqlConnection.Close();
            }


            return View(prod);
        }
        public ActionResult Create()
        {
           return View();
       }
        [HttpPost]
        public ActionResult Create(Product_Max product)
        {
            try
            {
                // Creating Connection  
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // Insert query  
                    string query = "INSERT INTO Product_Max(Name,Description,Image_Name,Price) VALUES( @name, @description, @image_Name, @price)";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        // Passing parameter values
                         
                        cmd.Parameters.AddWithValue("@name", product.Name);
                        cmd.Parameters.AddWithValue("@description", product.Description);
                        cmd.Parameters.AddWithValue("@Image_Name", product.Image_Name);
                        cmd.Parameters.AddWithValue("@Price", product.Price);
                        cmd.ExecuteNonQuery();
                    }
                    return RedirectToAction("Create");
                }
            }
            catch
            {
                return View();
            }

        }
        public ActionResult Edit(int id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Edit(Product_Max product)
        {
            try
            {
                // Creating Connection  
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // Update query  
                    string query = "UPDATE Product_Max SET Name = @name,Description = @description,Image_Name = @image_Name,Price = @price WHERE Id = @id";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        // Passing parameter values  
                        cmd.Parameters.AddWithValue("@id", product.ID);
                        cmd.Parameters.AddWithValue("@name", product.Name);
                        cmd.Parameters.AddWithValue("@description", product.Description);
                        cmd.Parameters.AddWithValue("@Image_Name", product.Image_Name);
                        cmd.Parameters.AddWithValue("@Price", product.Price);
                        cmd.ExecuteNonQuery();
                    }
                    return RedirectToAction("Select_Product");
                }
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Delete(int id)
        {
            string sqlExpression = "DELETE FROM Product_Max WHERE Id = " + id.ToString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                int number = command.ExecuteNonQuery();
                Console.WriteLine("Удалено объектов: {0}", number);
            }
            return RedirectToAction("Select_Product");
        }


    }
}



