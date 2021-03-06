namespace WebBazar.API.DTOs.Message
{
    public class MessageParams
    {
        private const int MaxPageSize = 15;
        private int pageSize = 5;

        public int PageNumber { get; set; } = 1;

        public int PageSize 
        { 
            get { return pageSize; } 
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; } 
        }

        public string MessageFilter { get; set; }
    }
}