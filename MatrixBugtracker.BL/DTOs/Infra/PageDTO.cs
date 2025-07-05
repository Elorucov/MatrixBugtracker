namespace MatrixBugtracker.BL.DTOs.Infra
{
    public class PageDTO<T>
    {
        public int? TotalCount { get; private set; }
        public List<T> Items { get; private set; }

        public PageDTO(List<T> items, int? count)
        {
            TotalCount = count;
            Items = items;
        }
    }
}
