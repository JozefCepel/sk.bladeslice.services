using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebEas.ServiceModel
{
    public class PaggingParameters
    {
        private int pageNumber;
        private int pageSize;
        private int recordsCount;

        public int PageNumber
        {
            get
            {
                if (pageNumber == 0)
                    pageNumber = 1; // default page number
                return pageNumber;
            }
            set
            {
                pageNumber = value;
            }
        }
        
        public int PageSize
        {
            get
            {
                if (pageSize == 0)
                    //TODO: neskor treba asi znizit ked FE spravi strankovanie
                    pageSize = 100;  // default page size
                return pageSize;
            }
            set
            {
                pageSize = value;
            }
        }

        public int RecordsCount
        {
            get
            {
                return recordsCount;
            }
            set
            {
                recordsCount = value;
            }
        }
    }
}
