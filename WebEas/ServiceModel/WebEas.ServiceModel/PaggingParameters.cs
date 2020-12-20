namespace WebEas.ServiceModel
{
    public class PaggingParameters
    {
        private int pageNumber;
        private int pageSize;

        public int PageNumber
        {
            get
            {
                return pageNumber == 0 ? 1 : pageNumber;
            }
            set => pageNumber = value;
        }

        public int PageSize
        {
            get
            {
                return pageSize == 0 ? 100 : pageSize;
            }
            set => pageSize = value;
        }

        public bool NotDefined => (pageSize == 0 && pageNumber == 0);

        public int RecordsCount { get; set; }
    }
}
