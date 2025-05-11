using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.DeleteSaleItem;

/// <summary>
/// Profile for mapping DeleteSale feature requests to commands.
/// </summary>
public class DeleteSaleItemProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for DeleteSale feature.
    /// </summary>
    public DeleteSaleItemProfile()
    {
        CreateMap<Guid, DeleteSaleItemCommand>()
            .ConstructUsing(id => new DeleteSaleItemCommand(id));
    }
}
