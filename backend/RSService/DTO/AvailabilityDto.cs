using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.DTO
{
    public class AvailabilityDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DayOfWeek { get; set; }
        public int AvailabilityType { get; set; }
        public int? RoomId { get; set; }

        public AvailabilityDto(int id, DateTime startDate, DateTime endDate, int dayOfWeek,  int availabilityType, int? roomId)
        {
            Id = id;
            StartDate = startDate;
            EndDate = endDate;
            DayOfWeek = dayOfWeek;
            AvailabilityType = availabilityType;
            RoomId = roomId;
        }

        public AvailabilityDto(int id, DateTime startDate, DateTime endDate, int dayOfWeek, int availabilityType)
        {
            Id = id;
            StartDate = startDate;
            EndDate = endDate;
            DayOfWeek = dayOfWeek;
            AvailabilityType = availabilityType;
        }
    }
}
