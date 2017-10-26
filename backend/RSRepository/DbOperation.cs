using System;
using System.Collections.Generic;
using System.Text;

namespace RSRepository
{
    public class DbOperation : IDbOperation
    {
        private RoomPlannerDevContext context;
       //private IEventRepository eventRepository;

        public DbOperation(RoomPlannerDevContext context)
        {
            this.context = context;
        }

        public void Commit()
        {
            context.SaveChanges();
        }

    }
}
