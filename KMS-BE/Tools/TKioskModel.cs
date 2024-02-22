using System;
using System.Collections.Generic;

namespace KMS.Tools
{
    public partial class TkioskModel
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string? KioskName { get; set; }
        public int? StationCode { get; set; }
        public string? Processor { get; set; }
        public string? Osname { get; set; }
        public string? Osversion { get; set; }
        public double? TotalMemory { get; set; }
        public double? AvailableMemory { get; set; }
        public double? DiskSizeC { get; set; }
        public double? FreeSpaceC { get; set; }
        public double? DiskSizeD { get; set; }
        public double? FreeSpaceD { get; set; }
        public string? IpAddress { get; set; }
        public string? UpTime { get; set; }
        public string? Location { get; set; }
        public string? MacAddress { get; set; }
        public string? RefCode { get; set; }
        public string? WebServices { get; set; }
        public int? SlidePackage { get; set; }
        public string? VersionApp { get; set; }
        public int? AutoUpdate { get; set; }
        public int? KioskStatus { get; set; }
        public int? PrinterStatus { get; set; }
        public int? CameraStatus { get; set; }
        public int? ScannerStatus { get; set; }
        public int? CashDepositStatus { get; set; }
        public string? PrinterPort { get; set; }
        public string? CameraPort { get; set; }
        public string? ScannerPort { get; set; }
        public string? CashDepositPort { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? IsActive { get; set; }
        public string? Osplatform { get; set; }
        public double? KioskRemainingMoney { get; set; }
        public DateTime? OnlineTime { get; set; }
        public DateTime? OfflineTime { get; set; }
        public string? Component { get; set; }
    }
}
