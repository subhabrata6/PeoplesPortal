namespace TestEmployeeApp.Model
{
    public class LeaveModule
    {
        public int Id { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int LeaveType { get; set; }
        public string? LeaveTypeName { get; set; }
        public int LeaveTime { get; set; }
        public int NumberOfDays { get; set; }
        public string? Reason { get; set; }
        public string CreatedBy { get; set; }
        public string? CreatedOn { get; set; }
        public string? Status { get; set; }
        public string? ApprovedBy { get; set; }
        public string? ApprovedOn { get; set; }

    }

}
