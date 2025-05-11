using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.ChangeSale;

/// <summary>
/// Profile for mapping ChangeSaleCommand and Sale entities.
/// </summary>
public class ChangeSaleProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeSaleProfile"/> class.
    /// </summary>
    public ChangeSaleProfile()
    {
        CreateMap<ChangeSaleCommand, Sale>()
            .ForMember(dest => dest.Items, opt => opt.Ignore());
        CreateMap<ChangeSaleItemDto, SaleItem>();
        CreateMap<Sale, ChangeSaleResult>();
    }
}
