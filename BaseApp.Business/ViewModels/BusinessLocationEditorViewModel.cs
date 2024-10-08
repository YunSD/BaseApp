using BaseApp.Business.Domain;
using BaseApp.Business.ViewModels.VO;
using BaseApp.Core.Domain;
using BaseApp.Core.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using System.ComponentModel.DataAnnotations;

namespace BaseApp.Business.ViewModels
{
    public partial class BusinessLocationEditorViewModel : ObservableValidator
    {
        [ObservableProperty]
        private bool editModel = true;

        private BusinessLocation entity;

        [ObservableProperty]
        private IList<BusinessBox> boxes;

        [ObservableProperty]
        private long? boxId;

        [ObservableProperty]
        private string? boxInfo;

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
        public string? lightAddress;
        partial void OnLightAddressChanged(string? value) => ValidateProperty(value, nameof(LightAddress));


        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        public string? lockAddress;
        partial void OnLockAddressChanged(string? value) => ValidateProperty(value, nameof(LockAddress));


        [ObservableProperty]
        private string? remark;



        private FormSubmitEventHandler<BusinessLocation> SubmitEvent;

        public BusinessLocationEditorViewModel(BusinessLocationInfo entity, IList<BusinessBox> boxes, FormSubmitEventHandler<BusinessLocation> submitEvent)
        {

            this.SubmitEvent = submitEvent;
            this.entity = entity;

            if (entity.BoxId.HasValue)
            {
                editModel = false;
            }
            this.Boxes = boxes;
            this.BoxId = entity.BoxId;
            this.BoxInfo = entity.BoxInfo;

            this.Code = entity.Code;
            this.Name = entity.Name;
            this.LightAddress = entity.LightAddress;
            this.LockAddress = entity.LockAddress;
            this.Remark = entity.Remark;
        }

        [RelayCommand]
        private void Submit()
        {
            if (!DialogHost.IsDialogOpen(BaseConstant.BaseDialog)) return;
            ValidateAllProperties();
            if (HasErrors) return;

            this.entity.BoxId = this.BoxId;
            this.entity.Code = this.Code;
            this.entity.Name = this.Name;
            this.entity.LightAddress = this.LightAddress;
            this.entity.LockAddress = this.LockAddress;
            this.entity.Remark = this.Remark;

            SubmitEvent(entity);
        }

    }
}
