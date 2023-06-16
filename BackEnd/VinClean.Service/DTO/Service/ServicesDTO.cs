﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Service.DTO.Service
{
    public class ServicesDTO
    {
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "CostPerSlot is required")]
        public decimal CostPerSlot { get; set; }

        [Required(ErrorMessage = "MinimalSlot is required")]
        public byte MinimalSlot { get; set; }

        public string Description { get; set; }

        public int TypeId { get; set; }

        public string Status { get; set; }

        public bool Avaiable { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int ModifiedBy { get; set; }
    }
}
