using MaterialDemo.Domain;
using MaterialDemo.Domain.Enums;
using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.Domain.Models.Entity.Upms;
using MaterialDemo.Utils;
using System.ComponentModel.DataAnnotations;

namespace BaseApp.Upms.ViewModels.Upms
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

            entity.RoleId = RoleId;
            entity.Username = Username;
            entity.Name = Name;
            entity.RoleId = RoleId;
            entity.Phone = Phone;
            entity.InfoCard = InfoCard;
            entity.Email = Email;
            entity.Remark = Remark;
            entity.LockFlag = LockFlag;

            SubmitEvent(entity);
        }

    }
}
