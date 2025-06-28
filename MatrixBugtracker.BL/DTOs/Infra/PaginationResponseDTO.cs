namespace MatrixBugtracker.BL.DTOs.Infra
{
    public class PaginationResponseDTO<T> : ResponseDTO<List<T>>
    {
        public int? Count { get; private set; }

        public PaginationResponseDTO(List<T> response, int count, int httpStatusCode = 200) : base(response, httpStatusCode)
        {
            Count = count;
        }

        public static new PaginationResponseDTO<T> Error<T1>(ResponseDTO<T1> response)
        {
            var err = Error(response.HttpStatusCode, response.ErrorMessage);
            return new PaginationResponseDTO<T>(null, 0, err.HttpStatusCode)
            {
                Success = false,
                Count = null,
                ErrorMessage = err.ErrorMessage,
                ErrorFields = err.ErrorFields
            };
        }
    }
}
