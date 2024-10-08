using BaseApp.Business.Domain;
using BaseApp.Core.Domain;
using BaseApp.Core.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

            BusinessBox new_entity = MapperUtil.Map<BusinessBox, BusinessBox>(entity);
            new_entity.Code = this.Code;
            new_entity.Name = this.Name;
            new_entity.LightControlAddress = this.LightControlAddress;
            new_entity.LockControlAddress = this.LockControlAddress;
            new_entity.Remark = this.Remark;

            SubmitEvent(new_entity);
        }

    }
}
