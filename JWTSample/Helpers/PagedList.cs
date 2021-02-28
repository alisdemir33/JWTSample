using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1
{

    public class PagedListContainer<T>
    {
        public PagedList<T> InnerList { get; set; }

        public MetaData PagedListMetaData { get; set; }


        public PagedListContainer(PagedList<T> list)
        {
            InnerList = list;
            PagedListMetaData = list.PagingMetaData;        
        }
    }

    public class MetaData {

        public int RecordCount { get;  set; }

        /// <summary>
        /// Sayfa sayısı
        /// </summary>
        public int PageCount { get;  set; }

        /// <summary>
        ///seçilen sayfanın numarası 
        /// </summary>
        public int PageNumber { get;  set; }

        /// <summary>
        /// Sayfanın kaç kayıt göstereceği
        /// </summary>
        public int PageSize { get;  set; }

    }


    public class PagedList<T> : List<T>
    {

        public MetaData PagingMetaData { get; set; }
        
        /// <summary>
        /// Toplam kayıt sayısı
        /// </summary>
        public int RecordCount { get; private set; }

        /// <summary>
        /// Sayfa sayısı
        /// </summary>
        public int PageCount { get; private set; }
        
        /// <summary>
        ///seçilen sayfanın numarası 
        /// </summary>
        public int PageNumber { get; private set; }

        /// <summary>
        /// Sayfanın kaç kayıt göstereceği
        /// </summary>
        public int PageSize { get; private set; }

        public int SkipSize { get { return (PageNumber - 1) * PageSize; } }
      //  public List<SearchFilter> Filters { get; set; }

        public PagedList()
        {
            
        }
        public PagedList(IEnumerable<T> source)
        {
            RecordCount = source.Count();
            PageCount = 1;
            PageNumber = 1;
            PageSize = RecordCount;
            SetMetaData();
            AddRange(source);
        }

        private void SetMetaData()
        {
            PagingMetaData = new MetaData()
            {
                RecordCount = this.RecordCount,
                PageCount = this.PageCount,
                PageNumber = this.PageNumber,
                PageSize = this.PageSize
            };
        }

        public PagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            RecordCount = source.Count();
            PageCount = GetPageCount(pageSize, RecordCount);
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize;
            SetMetaData();

            AddRange(source.Skip(SkipSize).Take(PageSize).ToList());
        }

        public PagedList(IEnumerable<T> source,MetaData metaData)
        {
            this.PagingMetaData = metaData;
            AddRange(source);
        }


        protected PagedList(int pageNumber, int pageSize, int recordCount)
        {
            RecordCount = recordCount;
            PageCount = GetPageCount(pageSize, RecordCount);
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize;
            SetMetaData();
        }

        private int GetPageCount(int pageSize, int totalCount)
        {
            if (pageSize == 0)
                return 0;

            var remainder = totalCount % pageSize;
            return (totalCount / pageSize) + (remainder == 0 ? 0 : 1);
        }
    }
}


//public class PagedList<T> : List<T>
//{
//    public int RecordCount { get; private set; }
//    public int PageCount { get; private set; }
//    public int PageNumber { get; private set; }
//    public int PageSize { get; private set; }
//    public int SkipSize { get { return (PageNumber - 1) * PageSize; } }
//    public List<SearchFilter> Filters { get; set; }

//    public PagedList()
//    {

//    }
//    public PagedList(IEnumerable<T> source)
//    {
//        RecordCount = source.Count();
//        PageCount = 1;
//        PageNumber = 1;
//        PageSize = RecordCount;
//        Filters = null;
//        AddRange(source);
//    }

//    public PagedList(IQueryable<T> source, int pageNumber, int pageSize, List<SearchFilter> filters)
//    {
//        RecordCount = source.Count();
//        PageCount = GetPageCount(pageSize, RecordCount);
//        PageNumber = pageNumber < 1 ? 1 : pageNumber;
//        PageSize = pageSize;
//        Filters = filters;

//        AddRange(source.Skip(SkipSize).Take(PageSize).ToList());
//    }

//    public PagedList(IQueryable<T> source, int pageNumber, int pageSize)
//    {
//        RecordCount = source.Count();
//        PageCount = GetPageCount(pageSize, RecordCount);
//        PageNumber = pageNumber < 1 ? 1 : pageNumber;
//        PageSize = pageSize;

//        AddRange(source.Skip(SkipSize).Take(PageSize).ToList());
//    }


//    protected PagedList(int pageNumber, int pageSize, int recordCount)
//    {
//        RecordCount = recordCount;
//        PageCount = GetPageCount(pageSize, RecordCount);
//        PageNumber = pageNumber < 1 ? 1 : pageNumber;
//        PageSize = pageSize;
//    }

//    protected PagedList(int pageNumber, int pageSize, int recordCount, List<SearchFilter> filters)
//    {
//        RecordCount = recordCount;
//        PageCount = GetPageCount(pageSize, RecordCount);
//        PageNumber = pageNumber < 1 ? 1 : pageNumber;
//        PageSize = pageSize;
//        Filters = filters;
//    }

//    private int GetPageCount(int pageSize, int totalCount)
//    {
//        if (pageSize == 0)
//            return 0;

//        var remainder = totalCount % pageSize;
//        return (totalCount / pageSize) + (remainder == 0 ? 0 : 1);
//    }
//}