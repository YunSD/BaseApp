using BaseApp.Business.Domain;
using BaseApp.Core.Domain;
using BaseApp.Core.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using System.ComponentModel.DataAnnotations;

namespace BaseApp.Business.ViewModels
{
    public partial class BusinessGoodsEditorViewModel : ObservableValidator
    {
        [ObservableProperty]
        private bool editModel = true;

        private BusinessGoods entity;

        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        private string? name;

        partial void OnNameChanged(string? value) => ValidateProperty(value, nameof(Name));


        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        private string? code;
        partial void OnCodeChanged(string? value) => ValidateProperty(value, nameof(Code));

        [ObservableProperty]
        public string? model;

        [ObservableProperty]
        private string? unit;


        [ObservableProperty]
        private string? image;

        [ObservableProperty]
        private string? remark;


        private FormSubmitEventHandler<BusinessGoods> SubmitEvent;

        public BusinessGoodsEditorViewModel(BusinessGoods entity, FormSubmitEventHandler<BusinessGoods> submitEvent)
        {

            this.SubmitEvent = submitEvent;
            this.entity = entity;

            if (entity.GoodsId.HasValue)
            {
                editModel = false;
            }

            this.Name = entity.Name;
            this.Code = entity.Code;
            this.model = entity.Model;
            this.unit = entity.Unit;
            this.Image = entity.Image;
            this.Remark = entity.Remark;
        }

        [RelayCommand]
        private void submit()
        {

            if (!DialogHost.IsDialogOpen(BaseConstant.BaseDialog)) return;

            ValidateAllProperties();
            if (HasErrors) return;

            this.entity.Name = this.Name;
            this.entity.Code = this.Code;
            this.entity.Model = this.Model;
            this.entity.Unit = this.Unit;
            this.entity.Image = this.Image;
            this.entity.Remark = this.Remark;

            SubmitEvent(entity);
        }

    }
}
