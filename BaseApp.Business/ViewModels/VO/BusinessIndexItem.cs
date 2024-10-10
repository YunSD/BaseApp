using CommunityToolkit.Mvvm.ComponentModel;

namespace MaterialDemo.ViewModels.Pages.Business.VObject
{
    public partial class BusinessIndexItem : ObservableObject
    {

        public long? LocationId { get; set; }

        public string? LocationInfo { get; set; }

        public SlotStatusEnums LSlotStatus { get; set; }

        public SlotStatusEnums RSlotStatus { get; set; }

    }


    public enum SlotStatusEnums {
        CHARGE,FULL,EMPTY
    }
}
