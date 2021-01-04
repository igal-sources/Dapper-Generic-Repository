using System;

namespace DapperGenericRepository.Models
{
    public class Machines
    {
        public Guid MachineId { get; set; }
        public string Name { get; set; }
        public string MachineIP { get; set; }
        public int TenantId { get; set; }
        public short MachineType { get; set; }
        public short MachineSubType { get; set; }
        public string Domain { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CPUType { get; set; }
        public string RAM { get; set; }
        public int? CachePort { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string Description { get; set; }
        public string AdditionalValues { get; set; }
        public string ChangedBy { get; set; }
        public Guid CITguid { get; set; }
        public int ConfigVersion { get; set; }
    }
}
