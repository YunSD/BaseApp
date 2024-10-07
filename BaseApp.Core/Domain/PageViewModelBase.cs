
using BaseApp.Core.UnitOfWork.Collections;

namespace BaseApp.Core.Domain
{
    public partial class PageViewModelBase<T> : ObservableObject
    {
        [ObservableProperty]
        public IList<T> dataList;

        // 每页显示多少条
        [ObservableProperty]
        public int pageSize = 20;

        // 总条目数
        [ObservableProperty]
        public int totalCount = 0;

        // 当前页
        [ObservableProperty]
        public int pageIndex = 0;

        // 页面总数
        [ObservableProperty]
        public int totalPage = 0;

        public PageViewModelBase()
        {
            dataList = new List<T>();
        }

        protected void RefreshPageInfo(List<T> items)
        {
            this.DataList = items;
        }

        protected void RefreshPageInfo(IPagedList<T> pagedList)
        {
            this.DataList = pagedList.Items;
            this.TotalCount = pagedList.TotalCount;
            this.PageIndex = pagedList.PageIndex;
            this.TotalPage = pagedList.TotalPages;
        }

        protected void RefreshPageInfo<A>(IPagedList<A> pagedList, List<T> items)
        {
            this.DataList = items;
            this.TotalCount = pagedList.TotalCount;
            this.PageIndex = pagedList.PageIndex;
            this.TotalPage = pagedList.TotalPages;
        }


    }
}
