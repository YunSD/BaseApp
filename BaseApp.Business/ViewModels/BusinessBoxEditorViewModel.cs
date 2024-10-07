using BaseApp.Business.Domain;
using BaseApp.Core.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDemo.Domain;
using MaterialDesignThemes.Wpf;
using System.ComponentModel.DataAnnotations;

namespace BaseApp.Business.ViewModels
{
    public partial class BusinessBoxEditorViewModel : ObservableValidator
    {
        [ObservableProperty]
        private bool editModel = true;

        private BusinessBox entity;

        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        private string? name;
        partial void OnNameChanged(string? value) => ValidateProperty(value, nameof(Name));

        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        private string? code;
        partial void OnCodeChanged(string? value) => ValidateProperty(value, nameof(Code));

        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        public string? lightControlAddress;
        partial void OnLightControlAddressChanged(string? value) => ValidateProperty(value, nameof(LightControlAddress));


        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        public string? lockControlAddress;
        partial void OnLockControlAddressChanged(string? value) => ValidateProperty(value, nameof(LockControlAddress));


        [ObservableProperty]
        private string? remark;



        private FormSubmitEventHandler<BusinessBox> SubmitEvent;

        public BusinessBoxEditorViewModel(BusinessBox entity, FormSubmitEventHandler<BusinessBox> submitEvent)
        {

            this.SubmitEvent = submitEvent;
            this.entity = entity;

            if (entity.BoxId.HasValue)
            {
                editModel = false;
            }
            this.Code = entity.Code;
            this.Name = entity.Name;

            this.LightControlAddress = entity.LightControlAddress;
            this.LockControlAddress = entity.LockControlAddress;
            this.Remark = entity.Remark;
        }

        [RelayCommand]
        private void Submit()
        {
            if (!DialogHost.IsDialogOpen(BaseConstant.BaseDialog)) return;
            ValidateAllProperties();
            if (HasErrors) return;

            this.entity.Code = this.Code;
            this.entity.Name = this.Name;
            this.entity.LightControlAddress = this.LightControlAddress;
            this.entity.LockControlAddress = this.LockControlAddress;
            this.entity.Remark = this.Remark;

            SubmitEvent(entity);
        }

    }
}
