namespace MatrixBugtracker.DAL.Models
{
    public class PaginationResult<T>
    {
        public List<T> Items { get; private set; }
        public int TotalCount { get; private set; }

        public PaginationResult(IEnumerable<T> items, int totalCount)
        {
            Items = items.ToList();
            TotalCount = totalCount;
        }
    }
}
