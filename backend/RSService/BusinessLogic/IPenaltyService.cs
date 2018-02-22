using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.BusinessLogic
{
    public interface IPenaltyService
    {
        void AddPenalty(DateTime startDate, int eventId, int attendeeId, int roomId);

        bool HasPenalty(int attendeeId, DateTime newDate, int roomId);
    }
}
