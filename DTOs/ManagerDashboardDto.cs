namespace Reimbursement_API.DTOs
{
    public class ManagerDashboardDto
    {
        public int TotalPendingThisMonth { get; set; }
        public int TotalApproveThisMonth { get; set; }
        public int TotalRejectedThisMonth { get; set; }
        public int TotalRequestThisMonth { get; set; }
    }
}
