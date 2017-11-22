using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RSData.Models;
using System.Linq;

namespace RSRepository
{
    public class PenaltyRepository : IPenaltyRepository
    {
        private RoomPlannerDevContext context;
        private DbSet<Penalty> penalties;

        public PenaltyRepository(RoomPlannerDevContext context)
        {
            this.context = context;
            penalties = context.Set<Penalty>();

        }

        public List<Penalty> GetPenaltiesByUser(int attendeeId)
        {
            return penalties.Where(p => p.AttendeeId == attendeeId).ToList();
        }

        public List<Penalty> GetPenalties()
        {
            return penalties.ToList();
        }

        public Penalty GetPenaltyById(int id)
        {
            return penalties.SingleOrDefault(s => s.Id == id);
        }

        public void AddPenalty(Penalty penalty)
        {
            if (penalty == null)
            {
                throw new ArgumentNullException("Add a null penalty");
            }
            penalties.Add(penalty);
            context.SaveChanges();
        }

        public void UpdatePenalty(Penalty penalty)
        {
            if (penalty == null)
            {
                throw new ArgumentNullException("Update a null penalty");
            }
            context.SaveChanges();
        }

        public void DeletePenalty(Penalty penalty)
        {
            if (penalty == null)
            {
                throw new ArgumentNullException("Delete null penalty");
            }
            penalties.Remove(penalty);
            context.SaveChanges();
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        
    }
}
