﻿using FluentValidation.Attributes;
using RSService.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.DTO
{
    public class EditEventDto
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int EventType { get; set; }

        public int RoomId { get; set; }

        public string Notes { get; set; }

        public int? HostId { get; set; }

        public int AttendeeId { get; set; }

        public int EventStatus { get; set; }

    }
}
