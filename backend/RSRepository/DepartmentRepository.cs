using System;
using System.Collections.Generic;
using System.Text;
using RSData.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace RSRepository
{
    class DepartmentRepository : IDepartmentRepository
    {
        private RoomPlannerDevContext _context;
        private DbSet<Department> _departments;

        public DepartmentRepository(RoomPlannerDevContext context)
        {
            _context = context;
            _departments = context.Set<Department>();
        }
        
        public void AddDepartment(Department department)
        {
            if (department == null)
            {
                throw new ArgumentNullException("Add a null department");
            }
            _departments.Add(department);
            _context.SaveChanges();
        }

        public void DeleteDepartment(Department department)
        {
            if (department == null)
            {
                throw new ArgumentNullException("Cannot delete a null department");
            }
            _departments.Remove(department);
            _context.SaveChanges();
        }

        public Department GetDepartmentById(int id)
        {
            return _departments.SingleOrDefault(s => s.Id == id);
        }

        public List<Department> GetDepartments()
        {
            return _departments.ToList();
        }


        public void UpdateDepartment(Department department)
        {
            if (department == null)
            {
                throw new ArgumentNullException("Update a null department");
            }
            _context.Entry(department).State = EntityState.Modified;
        }
    }
}
