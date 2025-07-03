namespace MatrixBugtracker.BL.DTOs.Infra
{
    public class PaginationRequestDTO
    {
        public int Number { get; init; }
        public int Size { get; init; }

        public static readonly PaginationRequestDTO Infinity = new PaginationRequestDTO { Number = 1, Size = int.MaxValue };
    }
}
