using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.DTO
{
    // we use this as AddRoomDto as well
    public class EditRoomDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public bool? IsActive { get; set; }
    }
}
