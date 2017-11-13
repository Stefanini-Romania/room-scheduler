using RSData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSRepository
{
    public interface IPenaltyRepository
    {
        IEnumerable<Penalty> GetPenaltiesByUser(int attendeeId);
        IEnumerable<Penalty> GetPenalties();
        Penalty GetPenaltyById(int id);
        void AddPenalty(Penalty penalty);
        void UpdatePenalty(Penalty penalty);
        void DeletePenalty(Penalty penalty);
        void SaveChanges();
    }
}
