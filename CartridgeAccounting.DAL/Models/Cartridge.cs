using System;

namespace CartridgeAccounting.DAL.Models
{
    public class Cartridge: IDomainObject
    {
        public int Id { get; set; }
        public DateTime Creation { get; set; }
        public DateTime DateOfChange { get; set; }
        public CartridgeType Type { get; set; }
        public string Model { get; set; }
        public string CompatiblePrinter { get; set; }
        public string Color { get; set; }
        public string Resource { get; set; }
        public bool WriteOff { get; set; }
    }
}
