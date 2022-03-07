namespace Tweetbook.Contracts.V1.Requests.Queries
{
    public class PaginationQuery
    {
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

        public int PageNumer { get; set; }

        public int PageSize { get; set; }
    }
}
