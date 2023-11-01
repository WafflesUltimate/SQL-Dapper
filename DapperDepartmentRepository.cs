using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Xml.Linq;


namespace SQLDapper
{
    internal class DapperDepartmentRepository : IDepartmentRepository
    {
        private readonly IDbConnection _connection;

        public DapperDepartmentRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public void InsertDepartment(string newDepartmentName)
        {
            _connection.Execute("INSERT INTO DEPARTMENTS (Name) VALUES (@departmentName);",
             new { departmentName = newDepartmentName });
        }

        public IEnumerable<Department> GetAllDepartments()
        {
            return _connection.Query<Department>("SELECT * FROM Departments;");
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _connection.Query<Product>("SELECT * FROM Products;");
        }

        public void CreateProduct(string name, double price, int categoryID, int ProductID)
        {
            _connection.Execute("INSERT INTO PRODUCTS (Name, Price, CategoryID, ProductID) VALUES (@productName, @productPrice, @productCategoryID, @productID);",
            new { productName = name, productPrice = price, productCategoryID = categoryID, productID = ProductID });
        }

        public void UpdateProduct(string name, double price, int categoryID)
        {
            //created new sql access
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            string connString = config.GetConnectionString("DefaultConnection");
            IDbConnection conn = new MySqlConnection(connString);
            var repo = new DapperDepartmentRepository(conn);
            var products = repo.GetAllProducts();


            //collect and verify productID for modification
            Console.WriteLine("Give me a product ID for the product you wish to update.");

            var inputValidater = int.TryParse(Console.ReadLine(), out var productID);
            var productToUpdate = new Product();

            foreach (var product in products)
            {
                if (product.ProductID == productID) { productToUpdate = product; break; }
            }

            while (inputValidater == false || productToUpdate == null)
            {
                Console.WriteLine("Not a valid ProductID.");

                inputValidater = int.TryParse(Console.ReadLine(), out productID);

                foreach (var product in products)
                {
                    if (product.ProductID == productID) { productToUpdate = product; break; }
                }
            }

            //update product information with arguments passed
            _connection.Execute("UPDATE PRODUCTS SET Name = @productName, Price = @productPrice, CategoryID = @productCategoryID WHERE ProductID = @productID;",
            new { productName = name, productPrice = price, productCategoryID = categoryID, productID = productID });
        }

        public void DeleteProduct()
        {
            //created new sql access
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            string connString = config.GetConnectionString("DefaultConnection");
            IDbConnection conn = new MySqlConnection(connString);
            var repo = new DapperDepartmentRepository(conn);
            var products = repo.GetAllProducts();


            //collect and verify productID for deletion
            Console.WriteLine("Give me a product ID for the product you wish to delete.");

            var inputValidater = int.TryParse(Console.ReadLine(), out var productID);
            var productToDelete = new Product();

            foreach (var product in products)
            {
                if (product.ProductID == productID) { productToDelete = product; break; }
            }

            while (inputValidater == false || productToDelete == null)
            {
                Console.WriteLine("Not a valid ProductID.");

                inputValidater = int.TryParse(Console.ReadLine(), out productID);

                foreach (var product in products)
                {
                    if (product.ProductID == productID) { productToDelete = product; break; }
                }
            }
            _connection.Execute("DELETE FROM PRODUCTS WHERE ProductID = @productID;",
            new { productID = productID });

            _connection.Execute("DELETE FROM REVIEWS WHERE ProductID = @productID;",
            new { productID = productID });
        }
    }
}
