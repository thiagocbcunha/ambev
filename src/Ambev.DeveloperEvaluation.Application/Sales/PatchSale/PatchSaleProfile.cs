using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.PatchSale;

/// <summary>
/// Profile for mapping ChangeSaleCommand and Sale entities.
/// </summary>
public class PatchSaleProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PatchSaleProfile"/> class.
    /// </summary>
    public PatchSaleProfile()
    {
        CreateMap<PatchSaleCommand, Sale>()
            .ForMember(dest => dest.Items, opt => opt.Ignore());
        CreateMap<Sale, PatchSaleResult>();
    }
}
