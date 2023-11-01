using System;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System.Dynamic;

namespace SQLDapper
{
    public class Program
    {
        //Global variables
        public static int productID = 0;
        public static double productPrice = 0;
        public static int productCategoryID = 0;

        //Global Methods
        static void GetProductInfo()
        {
            //adding a new product price as double
            Console.WriteLine("Now give me a price for the product.");

            var inputValidater = double.TryParse(Console.ReadLine(), out productPrice);

            while (inputValidater == false)
            {
                Console.WriteLine("Not a valid entry for product price.");

                inputValidater = double.TryParse(Console.ReadLine(), out productPrice);
            }

            //adding a new product productCategoryID as int
            Console.WriteLine("Now give me a categoryID for the product.");

            inputValidater = int.TryParse(Console.ReadLine(), out productCategoryID);

            while (inputValidater == false)
            {
                Console.WriteLine("Not a valid entry for product categoryID.");

                inputValidater = int.TryParse(Console.ReadLine(), out productCategoryID);
            }

            //adding a new product productID as int
            Console.WriteLine("Now give me a productID for the product.");

            inputValidater = int.TryParse(Console.ReadLine(), out productID);

            while (inputValidater == false)
            {
                Console.WriteLine("Not a valid entry for ProductID.");

                inputValidater = int.TryParse(Console.ReadLine(), out productID);
            }
        }

        static void GetProductInfoForUpdate()
        {
            //adding a new product price as double
            Console.WriteLine("Now give me a price for the product.");

            var inputValidater = double.TryParse(Console.ReadLine(), out productPrice);

            while (inputValidater == false)
            {
                Console.WriteLine("Not a valid entry for product price.");

                inputValidater = double.TryParse(Console.ReadLine(), out productPrice);
            }

            //adding a new product productCategoryID as int
            Console.WriteLine("Now give me a categoryID for the product.");

            inputValidater = int.TryParse(Console.ReadLine(), out var productCategoryID);

            while (inputValidater == false)
            {
                Console.WriteLine("Not a valid entry for product categoryID.");

                inputValidater = int.TryParse(Console.ReadLine(), out productCategoryID);
            }
        }

        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            string connString = config.GetConnectionString("DefaultConnection");
            IDbConnection conn = new MySqlConnection(connString);
            var repo = new DapperDepartmentRepository(conn);

            //getting all departments
            var departments = repo.GetAllDepartments();

            //adding department Name
            Console.WriteLine("Type a new Department name.");
            var newDepartmentName = Console.ReadLine(); 

            //inserting department name into sql
            repo.InsertDepartment(newDepartmentName);

            Console.WriteLine($"Departments: ");
            foreach (var department in departments)
            {
                Console.WriteLine(department.Name);
            }

            Console.ReadLine();
            Console.WriteLine("Getting All Products...");

            //getting all products
            var products = repo.GetAllProducts();

            Console.WriteLine($"Products:");
            foreach (var product in products)
            {
                Console.WriteLine($"Product Name {product.Name} : Product Price {product.Price} : Product CategoryID {product.CategoryID} : ProductID {product.ProductID}");
            }

            //adding a new product name
            Console.WriteLine("Create A new Product. Let's Start with a name.");
            var productName = Console.ReadLine();

            GetProductInfo();

            repo.CreateProduct(productName, productPrice, productCategoryID, productID);

            //updating a product 
            Console.WriteLine("Updating Product. Give me a new name.");
            productName = Console.ReadLine();

            GetProductInfoForUpdate();

            repo.UpdateProduct(productName, productPrice, productCategoryID);

            //deleting a product
            repo.DeleteProduct();

        }
    }
}
