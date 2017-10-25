using RSData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSRepository
{
    interface IDepartmentRepository
    {
        IEnumerable<Department> GetDepartments();
        Department GetDepartmentById(int id);
        void AddDepartment(Department department);
        void UpdateDepartment(Department department);
        void DeleteDepartment(Department department);
    }
}
