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

            BusinessGoods new_entity = MapperUtil.Map<BusinessGoods, BusinessGoods>(entity);
            new_entity.Name = this.Name;
            new_entity.Code = this.Code;
            new_entity.Model = this.Model;
            new_entity.Unit = this.Unit;
            new_entity.Image = this.Image;
            new_entity.Remark = this.Remark;

            SubmitEvent(new_entity);
        }

    }
}
