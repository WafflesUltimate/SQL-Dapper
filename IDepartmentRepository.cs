using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDapper
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department> GetAllDepartments();

        IEnumerable<Product> GetAllProducts();

        void CreateProduct(string name, double price, int categoryID, int ProductID);
        void UpdateProduct(string name, double price, int categoryID);
        void DeleteProduct();
    }
}
