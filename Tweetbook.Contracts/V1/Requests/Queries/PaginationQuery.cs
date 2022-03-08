namespace Tweetbook.Contracts.V1.Requests.Queries
{
    public class PaginationQuery
    {
        public int PageNumer { get; set; }

        public int PageSize { get; set; }

        public PaginationQuery()
        {
            PageNumer = 1;
            PageSize = 100;
        }

        public PaginationQuery(int pageNumber, int pageSize)
        {
            PageNumer = pageNumber;
            PageSize = pageSize;
        }
    }
}
