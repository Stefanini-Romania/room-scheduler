using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.DTO
{
    public class AvailabilityDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DayOfWeek { get; set; }
        public int AvailabilityType { get; set; }
        public int? RoomId { get; set; }

        public AvailabilityDto(DateTime startDate, DateTime endDate, int dayOfWeek,  int availabilityType, int? roomId)
        {
            StartDate = startDate;
            EndDate = endDate;
            DayOfWeek = dayOfWeek;
            AvailabilityType = availabilityType;
            RoomId = roomId;
        }

        public AvailabilityDto(DateTime startDate, DateTime endDate, int dayOfWeek, int availabilityType)
        {
            StartDate = startDate;
            EndDate = endDate;
            DayOfWeek = dayOfWeek;
            AvailabilityType = availabilityType;
        }
    }
}
