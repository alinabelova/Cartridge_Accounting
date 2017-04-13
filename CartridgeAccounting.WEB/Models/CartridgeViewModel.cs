using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CartridgeAccounting.WEB.Models
{
    public class CartridgeViewModel
    {
        public int Id { get; set; }
        // public DateTime Creation { get; set; }
        //public DateTime DateOfChange { get; set; }
        [DisplayName("Тип")]
        public string Type { get; set; }

        [Required]
        [DisplayName("Модель")]
        public string Model { get; set; }

        [Required]
        [DisplayName("Совместимый принтер")]
        public string CompatiblePrinter { get; set; }

        [Required]
        [DisplayName("Цвет")]
        public string Color { get; set; }

        [DisplayName("Ресурсоемкость(стр)")]
        public string Resource { get; set; }

        [DisplayName("Списан")]
        public bool WriteOff { get; set; }

        [Required]
        [Display(Name = "Тип")]
        public IEnumerable<int> TypeIds { get; set; }
    }
}