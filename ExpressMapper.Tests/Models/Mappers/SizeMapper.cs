namespace ExpressMapper.Tests.Models.Mappers
{
    using ExpressMapper.Tests.Models.Classes;
    using ExpressMapper.Tests.Models.ViewModels;

    public class SizeMapper : ICustomTypeMapper<Size, SizeViewModel>
    {
        public SizeViewModel Map(IMappingContext<Size, SizeViewModel> context)
        {
            var sizeViewModel = context.Destination ?? new SizeViewModel();

            sizeViewModel.Id = context.Source.Id;
            sizeViewModel.Alias = context.Source.Alias;
            sizeViewModel.Name = context.Source.Name;
            sizeViewModel.SortOrder = context.Source.SortOrder;
            return sizeViewModel;
        }
    }
}