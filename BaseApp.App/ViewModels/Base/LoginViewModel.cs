using BaseApp.Core.Domain;
using BaseApp.Core.Enums;
using BaseApp.Core.Security.Messages;
using BaseApp.Core.UnitOfWork;
using BaseApp.Core.Utils;
using BaseApp.Upms.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace BaseApp.App.ViewModels
{

    /// <summary>
    /// Defines the <see cref="LoginViewModel" />
    /// </summary>
    public partial class LoginViewModel : ObservableObject
    {

        private readonly IUnitOfWork _unitOfWork;

        private readonly BaseUserDetailsService userDetailsService;


        [ObservableProperty]
        public string? username;

        [ObservableProperty]
        public LoginType loginType = LoginType.PASSWORD;

        public LoginViewModel(IUnitOfWork unitOfWork, BaseUserDetailsService userDetailsService)
        {
            this._unitOfWork = unitOfWork;
            this.userDetailsService = userDetailsService;
        }

        public async Task<bool> Login(string password)
        {
            SysUser? user = new SysUser();
            bool status = await Task.Delay(1500).ContinueWith((t) =>
            {
                if (Username == null) return false;
                user = LoadUser(Username);
                if (user == null || !SecurityUtil.Verify(password, user.Password)) return false;
                if (user.IsLocked()) return false;
                return true;
            });

            if (status) WeakReferenceMessenger.Default.Send(new LoginCompletedMessage(userDetailsService.LoadSecurityUser(user)));
            return status;
        }

        private SysUser LoadUser(string username)
        {
            return _unitOfWork.GetRepository<SysUser>().GetFirstOrDefault(predicate: u => u.Username != null && u.Username.Equals(username));
        }

        [RelayCommand]
        private void SwitchLoginType(string key) {
            LoginType type = (LoginType)Enum.Parse(typeof(LoginType), key);
            if (this.LoginType != type) {
                this.LoginType = type;
                WeakReferenceMessenger.Default.Send(new SwitchLoginMessage(type));
            } 
        }



    }
}
