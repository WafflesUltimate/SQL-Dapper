using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDapper
{
    public class Department 
    {
 
        public Department() { }
        public int DepartmentID { get; set; }
        public string Name { get; set; }
    }
}
