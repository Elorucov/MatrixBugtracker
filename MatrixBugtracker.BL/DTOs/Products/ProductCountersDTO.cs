namespace MatrixBugtracker.BL.DTOs.Products
{
    public class ProductCountersDTO
    {
        public int Members { get; init; }
        public int TotalReports { get; init; }
        public int OpenReports { get; init; }
        public int WorkingReports { get; init; }
        public int FixedReports { get; init; }
    }
}
