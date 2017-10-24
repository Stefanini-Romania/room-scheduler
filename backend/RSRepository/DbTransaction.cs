using System;
using System.Collections.Generic;
using System.Text;

namespace RSRepository
{
    public class DbTransaction : IDbTransaction
    {
        private RoomPlannerDevContext context;
       //private IEventRepository eventRepository;

        public DbTransaction(RoomPlannerDevContext context)
        {
            this.context = context;
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public void Roolback()
        {
            context.Database.RollbackTransaction();
        }
    }
}
