using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BaseApp.Upms.ViewModels.Upms.VO
{
    public class MenuTreeViewInfo : SysMenu, INotifyPropertyChanged
    {

        public bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
                if (Children != null)
                {
                    foreach (var item in Children)
                    {
                        item.IsSelected = value;
                    }
                }
            }
        }
        public ObservableCollection<MenuTreeViewInfo>? Children { get; set; } // 子节点集合

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static List<MenuTreeViewInfo> build(List<SysMenu> menus, long? root = 0, List<long?> selected = null)
        {
            var list = menus.Where(m => m.ParentId == root).OrderBy(m => m.Seq)
                .Select(m =>
                {
                    MenuTreeViewInfo info = MapperUtil.Map<SysMenu, MenuTreeViewInfo>(m);
                    if (selected != null && info.MenuId != null && selected.Contains(info.MenuId.Value)) info._isSelected = true;
                    List<MenuTreeViewInfo> list = build(menus, m.MenuId, selected);
                    if (list.Any()) info.Children = new ObservableCollection<MenuTreeViewInfo>(list);
                    return info;
                }).ToList();
            return list;
        }

        public IEnumerable<MenuTreeViewInfo> GetAllNodes()
        {
            yield return this;
            if (Children != null)
                foreach (var child in Children)
                {
                    foreach (var node in child.GetAllNodes())
                    {
                        yield return node;
                    }
                }
        }

        //private static ObservableCollection<MenuTreeViewInfo> findNodes(MenuTreeViewInfo menu, ObservableCollection<SysMenu> menus) {
        //   var list = menus.Where(m => m.MenuId == menu.MenuId).OrderBy(m => m.Seq)
        //        .Select(m=> {
        //            MenuTreeViewInfo info = MapperUtil.Map<SysMenu, MenuTreeViewInfo>(m);
        //            info.Children = findNodes(info, menus);
        //            return info;
        //        }).ToList();

        //    return new ObservableCollection<MenuTreeViewInfo>(list);
        //}
    }
}
