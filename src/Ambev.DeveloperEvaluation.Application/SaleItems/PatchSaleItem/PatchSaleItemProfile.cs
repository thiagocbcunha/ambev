using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.PatchSaleItem;

/// <summary>
/// Profile for mapping ChangeSaleCommand and Sale entities.
/// </summary>
public class PatchSaleItemProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PatchSaleItemProfile"/> class.
    /// </summary>
    public PatchSaleItemProfile()
    {
        CreateMap<PatchSaleItemCommand, SaleItem>();
        CreateMap<SaleItem, PatchSaleItemResult>();
    }
}
