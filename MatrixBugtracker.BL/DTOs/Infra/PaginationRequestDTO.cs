namespace MatrixBugtracker.BL.DTOs.Infra
{
    public class PaginationRequestDTO
    {
        public int Number { get; set; }
        public int Size { get; set; }

        public static readonly PaginationRequestDTO Infinity = new PaginationRequestDTO { Number = 1, Size = int.MaxValue };
    }
}
