using BaseApp.App.Views;
using BaseApp.Core.Domain;
using BaseApp.Core.Security;
using BaseApp.Core.Security.Messages;
using BaseApp.Core.Utils;
using BaseApp.Upms.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace BaseApp.App.ViewModels
{
    public partial class HomeViewModel : ObservableRecipient, IRecipient<RefreshUserMessage>
    {

        [ObservableProperty]
        private ObservableCollection<object> _menuItems = new();
        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems = new();

        [ObservableProperty]
        private string? _userAvaster;

        [ObservableProperty]
        private string? _userName;

        public HomeViewModel()
        {
            this.IsActive = true;
            loadIndexData();
        }


        [RelayCommand]
        private void ToPersonView() => ServiceProviderUtil.GetRequiredService<INavigationService>().Navigate(typeof(PersonViewPage));

        [RelayCommand]
        private void LogOut() => WeakReferenceMessenger.Default.Send(new LogoutMessage());

        public void Receive(RefreshUserMessage message)
        {
            loadIndexData();
        }

        private void loadIndexData()
        {
            SecurityUser? user = BaseApp.Security.SecurityContext.Singleton.GetUserInfo();
            if (user != null)
            {
                this.UserAvaster = BaseFileUtil.GetOriFilePath(user.Avatar);
                this.UserName = user.Name;
                if (user.menus.Any())
                {
                    Dictionary<long, NavigationViewItem> navigationMaps = new();

                    List<NavigationViewItem> topItem = new();
                    List<NavigationViewItem> footerItem = new();

                    user.menus.OrderBy(menu => menu.Seq).ToList().ForEach(menu =>
                    {
                        string? name = menu.Name;
                        if (name == null) name = "NULL";
                        NavigationViewItem item = new NavigationViewItem(name, BasePageUtil.ParseSymbolIcon(menu.Icon), BasePageUtil.ParseClassType(menu.Router));
                        navigationMaps.Add((long)menu.MenuId, item);
                        if (menu.isRoot() && MenuPositionEnum.TOP == menu.Position) topItem.Add(item);
                        if (menu.isRoot() && MenuPositionEnum.BOTTOM == menu.Position) footerItem.Add(item);
                    });

                    user.menus.ForEach(menu =>
                    {
                        if (menu.isRoot()) return;
                        NavigationViewItem parentItem = navigationMaps[(long)menu.ParentId];
                        NavigationViewItem currentItem = navigationMaps[(long)menu.MenuId];
                        if (parentItem != null && currentItem != null) parentItem.MenuItems.Add(currentItem);
                    });

                    topItem.ForEach(menu => MenuItems.Add(menu));
                    footerItem.ForEach(menu => FooterMenuItems.Add(menu));
                }
            }
        }
    }
}
