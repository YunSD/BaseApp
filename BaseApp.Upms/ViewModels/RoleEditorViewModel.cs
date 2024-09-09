using MaterialDemo.Domain;
using MaterialDemo.Domain.Models.Entity.Upms;
using MaterialDemo.Utils;
using System.ComponentModel.DataAnnotations;

namespace MaterialDemo.ViewModels.Pages.Upms
{
    public partial class RoleEditorViewModel : ObservableValidator
    {
        [ObservableProperty]
        private bool editModel = true;

        private SysRole entity;

        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        private string? name;

        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        private string? code;

        [ObservableProperty]
        private string? remark;


        public delegate bool SaveEventHandler(object sender, DialogOpenedEventArgs eventArgs);

        private FormSubmitEventHandler<SysRole> SubmitEvent;

        public RoleEditorViewModel(SysRole entity, FormSubmitEventHandler<SysRole> submitEvent)
        {
            this.SubmitEvent = submitEvent;
            this.entity = entity;

            if (entity.RoleId.HasValue)
            {
                editModel = false;
            }
            this.Name = entity.Name;
            this.Code = entity.Code;
            this.Remark = entity.Remark;
        }

        [RelayCommand]
        private void Submit()
        {
            if (!DialogHost.IsDialogOpen(BaseConstant.BaseDialog)) return;
            ValidateAllProperties();

            if (HasErrors) return;

            this.entity.Name = Name;
            this.entity.Code = Code;
            this.entity.Remark = Remark;

            SubmitEvent(entity);
        }
    }
}
