using BaseApp.Core.Domain;
using BaseApp.Core.Enums;
using BaseApp.Core.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using System.ComponentModel.DataAnnotations;

namespace BaseApp.Upms.ViewModels
{
    public partial class UserEditorViewModel : ObservableValidator
    {
        [ObservableProperty]
        private bool editModel = true;

        private SysUser entity;

        [ObservableProperty]
        private long? roleId;

        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        private string? username;

        partial void OnUsernameChanged(string? value) => ValidateProperty(value, nameof(Username));

        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        private string? name;
        partial void OnNameChanged(string? value) => ValidateProperty(value, nameof(Name));

        [ObservableProperty]
        public string? infoCard;

        [ObservableProperty]
        private string? email;

        [ObservableProperty]
        private string? phone;

        [ObservableProperty]
        private BaseStatusEnum lockFlag;

        [ObservableProperty]
        private string? remark;

        [ObservableProperty]
        private IList<SysRole> roles;

        private FormSubmitEventHandler<SysUser> SubmitEvent;

        public UserEditorViewModel(SysUser sysUser, IList<SysRole> roles, FormSubmitEventHandler<SysUser> submitEvent)
        {
            SubmitEvent = submitEvent;
            this.roles = roles;
            entity = sysUser;

            if (sysUser.UserId.HasValue)
            {
                editModel = false;
            }

            this.RoleId = sysUser.RoleId;
            this.Username = sysUser.Username;
            this.InfoCard = sysUser.InfoCard;
            this.Name = sysUser.Name;
            this.Email = sysUser.Email;
            this.Phone = sysUser.Phone;
            this.LockFlag = sysUser.LockFlag;
            this.Remark = sysUser.Remark;
        }

        [RelayCommand]
        private void submit()
        {
            if (!DialogHost.IsDialogOpen(BaseConstant.BaseDialog)) return;
            ValidateAllProperties();
            if (HasErrors) return;

            SysUser new_entity = MapperUtil.Map<SysUser, SysUser>(entity);

            new_entity.RoleId = RoleId;
            new_entity.Username = Username;
            new_entity.Name = Name;
            new_entity.RoleId = RoleId;
            new_entity.Phone = Phone;
            new_entity.InfoCard = InfoCard;
            new_entity.Email = Email;
            new_entity.Remark = Remark;
            new_entity.LockFlag = LockFlag;

            SubmitEvent(new_entity);
        }

    }
}
