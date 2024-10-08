﻿using BaseApp.Core.Domain;
using BaseApp.Core.Utils;
using BaseApp.Upms.ViewModels.VO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using System.ComponentModel.DataAnnotations;

namespace BaseApp.Upms.ViewModels
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
            SubmitEvent = submitEvent;
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

            SysMenu new_entity = MapperUtil.Map<SysMenu, SysMenu>(entity);
            new_entity.ParentId = ParentId == null ? 0 : ParentId;
            new_entity.Name = Name;
            new_entity.Icon = Icon;
            new_entity.Router = Router;
            new_entity.Position = Position;
            new_entity.Seq = Seq;
            new_entity.Remark = Remark;
            SubmitEvent(new_entity);
        }
    }
}
