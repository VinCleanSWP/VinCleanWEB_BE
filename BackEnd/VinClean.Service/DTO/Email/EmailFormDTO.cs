﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Service.DTO.Email
{
    public class EmailFormDTO
    {
        public int ProcessId { get; set; }
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty; 
        public string Body { get; set; } = string.Empty;
    }
}
