namespace ExpressMapper.Tests.Models.ViewModels
{
    using System.Collections.Generic;

    public class GuiViewModel
    {
        public IEnumerable<BaseControlViewModel> ControlViewModels { get; set; }
    }
}