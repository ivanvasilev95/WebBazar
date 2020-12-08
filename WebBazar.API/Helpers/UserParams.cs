namespace WebBazar.API.Helpers
{
    public class UserParams // rename AdParams
    {
        private const int MaxPageSize = 10;
        private int pageSize = 5;
        
        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }
        
        public string SearchText { get; set; }

        public int CategoryId { get; set; }
        
        public string SortCriteria { get; set; }
    }
}