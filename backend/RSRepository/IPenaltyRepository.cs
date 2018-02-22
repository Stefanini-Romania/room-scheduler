using RSData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSRepository
{
    public interface IPenaltyRepository
    {
        List<Penalty> GetPenaltiesByUser(int attendeeId);
        List<Penalty> GetPenalties();
        Penalty GetPenaltyById(int id);
        void AddPenalty(Penalty penalty);
        void SaveChanges();
    }
}
