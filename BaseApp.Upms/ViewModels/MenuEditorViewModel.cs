using MaterialDemo.Domain;
using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.Utils;
using MaterialDemo.ViewModels.Pages.Upms.VObject;
using System.ComponentModel.DataAnnotations;

namespace MaterialDemo.ViewModels.Pages.Upms
{
    public partial class MenuEditorViewModel : ObservableValidator
    {
        [ObservableProperty]
        private bool editModel = true;

        [ObservableProperty]
        private IList<SysMenuViewInfo> parents;

        private SysMenu entity;

        [ObservableProperty]
        private long? parentId;

        [ObservableProperty]
        private string? parentName;

        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        private string? name;

        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        private string? router;

        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        public string? icon;

        [ObservableProperty]
        private MenuPositionEnum position = MenuPositionEnum.TOP;

        [ObservableProperty]
        private int? seq;

        [ObservableProperty]
        private string? remark;


        private FormSubmitEventHandler<SysMenu> SubmitEvent;

        public MenuEditorViewModel(SysMenuViewInfo entity, IList<SysMenuViewInfo> sysMenuVOs, FormSubmitEventHandler<SysMenu> submitEvent)
        {
            this.SubmitEvent = submitEvent;
            this.entity = entity;

            if (entity.MenuId.HasValue)
            {
                editModel = false;
            }
            this.Parents = sysMenuVOs;
            this.ParentId = entity.ParentId;
            this.ParentName = entity.ParentName;
            this.Name = entity.Name;
            this.Router = entity.Router;
            this.Icon = entity.Icon;
            this.Position = entity.Position;
            this.Seq = entity.Seq;
            this.Remark = entity.Remark;
        }

        [RelayCommand]
        private void Submit()
        {
            if (!DialogHost.IsDialogOpen(BaseConstant.BaseDialog)) return;
            ValidateAllProperties();

            if (HasErrors) return;

            this.entity.ParentId = ParentId == null ? 0 : ParentId;
            this.entity.Name = Name;
            this.entity.Icon = Icon;
            this.entity.Router = Router;
            this.entity.Position = Position;
            this.entity.Seq = Seq;
            this.entity.Remark = Remark;
            SubmitEvent(entity);
        }
    }
}
