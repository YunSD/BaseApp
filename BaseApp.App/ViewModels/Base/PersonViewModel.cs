using BaseApp.App.Views;
using BaseApp.Core.Domain;
using BaseApp.Core.Security;
using BaseApp.Core.Security.Messages;
using BaseApp.Core.UnitOfWork;
using BaseApp.Core.Utils;
using BaseApp.Upms.Services;
using BaseApp.Upms.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using log4net;
using MaterialDesignThemes.Wpf;
using System.ComponentModel.DataAnnotations;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace BaseApp.App.ViewModels
{
    public partial class PersonViewModel : ObservableValidator, INavigationAware
    {

        private ILog logger = LogManager.GetLogger(nameof(PersonViewModel));

        private readonly BaseUserDetailsService baseUserDetailsService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<SysUser> repository;

        public PersonViewModel(IUnitOfWork unitOfWork, BaseUserDetailsService baseUserDetailsService)
        {
            this._unitOfWork = unitOfWork;
            this.baseUserDetailsService = baseUserDetailsService;
            this.repository = _unitOfWork.GetRepository<SysUser>();
        }


        #region View Field
        private long userId;

        [ObservableProperty]
        private string? _username;

        [ObservableProperty]
        private string? _roleName;

        [Required(ErrorMessage = "名称不能为空")]
        [ObservableProperty]
        private string? _name;

        [ObservableProperty]
        private string? _phone;

        [ObservableProperty]
        private string? _email;

        [ObservableProperty]
        private string? _avaster;

        [ObservableProperty]
        private string? _password;

        [ObservableProperty]
        private string? _repeatPassword;

        #endregion


        [RelayCommand]
        private void UpdateInfo()
        {
            this.ValidateAllProperties();
            if (HasErrors) return;

            SysUser user = repository.Find(userId);
            if (user == null)
            {
                SnackbarService.ShowError("用户信息更新失败,请退出后重试。");
                return;
            }

            user.Name = this.Name;
            user.Phone = this.Phone;
            user.Email = this.Email;
            user.Avatar = this.Avaster;
            repository.Update(user);
            _unitOfWork.SaveChanges();
            SnackbarService.ShowSuccess("用户信息更新成功。");
            repository.ChangeEntityState(user, Microsoft.EntityFrameworkCore.EntityState.Detached);
            WeakReferenceMessenger.Default.Send(new RefreshUserMessage(baseUserDetailsService.LoadSecurityUser(user)));
        }

        [RelayCommand]
        private void ResetInfo()
        {
            this.OnNavigatedTo();
        }

        [RelayCommand]
        private void UpdatePassword()
        {
            if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(RepeatPassword))
            {
                SnackbarService.ShowError("密码不能为空");
                return;
            }
            if (!Password.Equals(RepeatPassword))
            {
                SnackbarService.ShowError("两次输入不一致");
                return;
            }
            if (Password.Length < 6)
            {
                SnackbarService.ShowError("密码长度至少六位");
                return;
            }

            SysUser user = repository.Find(userId);
            if (user == null)
            {
                SnackbarService.ShowError("用户信息更新失败,请退出后重试。");
                return;
            }
            user.Password = SecurityUtil.Encrypt(Password);
            repository.Update(user);
            _unitOfWork.SaveChanges();
            SnackbarService.ShowSuccess("用户密码更新成功。");
            repository.ChangeEntityState(user, Microsoft.EntityFrameworkCore.EntityState.Detached);
        }


        [RelayCommand]
        private void OpenFaceRegistionPage()
        {
            PersonFaceRegistration page = new PersonFaceRegistration();
            DialogHost.Show(page, BaseConstant.BaseDialog);
        }


        public void OnNavigatedFrom()
        {
        }

        public void OnNavigatedTo()
        {
            SecurityUser? user = BaseApp.Security.SecurityContext.Singleton.GetUserInfo();
            userId = user?.UserId ?? 0;
            Username = user?.UserName;
            RoleName = user?.RoleName;
            Name = user?.Name;
            Phone = user?.Phone;
            Email = user?.Email;
            Avaster = user?.Avatar;
        }

    }
}
