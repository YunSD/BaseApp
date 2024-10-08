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
    public partial class BusinessLocationSlotEditorViewModel : ObservableValidator
    {
        [ObservableProperty]
        private bool editModel = true;

        private BusinessLocationSlot entity;

        [ObservableProperty]
        private long? boxId;

        [ObservableProperty]
        private string? boxInfo;

        [ObservableProperty]
        private long? locationId;

        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        private string? locationInfo;

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
        public string? slotAddress;
        partial void OnSlotAddressChanged(string? value) => ValidateProperty(value, nameof(SlotAddress));

        [ObservableProperty]
        private string? remark;

        private FormSubmitEventHandler<BusinessLocationSlot> SubmitEvent;

        public BusinessLocationSlotEditorViewModel(BusinessLocationSlotInfo entity, FormSubmitEventHandler<BusinessLocationSlot> submitEvent)
        {

            this.SubmitEvent = submitEvent;
            this.entity = entity;

            if (entity.SlotId.HasValue)
            {
                editModel = false;
            }
            this.BoxId = entity.BoxId;
            this.BoxInfo = entity.BoxInfo;
            this.LocationId = entity.LocationId;
            this.LocationInfo = entity.LocationInfo;

            this.Code = entity.Code;
            this.Name = entity.Name;
            this.SlotAddress = entity.SlotAddress;
            this.Remark = entity.Remark;
        }

        [RelayCommand]
        private void Submit()
        {
            if (!DialogHost.IsDialogOpen(BaseConstant.BaseDialog)) return;
            ValidateAllProperties();
            if (HasErrors) return;

            entity.BoxId = this.BoxId;
            entity.LocationId = this.LocationId;
            entity.Code = this.Code;
            entity.Name = this.Name;
            entity.SlotAddress = this.SlotAddress;
            entity.Remark = this.Remark;

            SubmitEvent(entity);
        }

    }
}
