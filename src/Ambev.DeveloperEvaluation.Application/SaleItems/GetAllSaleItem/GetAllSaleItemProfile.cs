using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.GetAllSaleItem;

/// <summary>
/// Profile for mapping Sale entity to GetSaleResult.
/// </summary>
public class GetAllSaleItemProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetSale feature.
    /// </summary>
    public GetAllSaleItemProfile()
    {
        CreateMap<SaleItem, GetSaleItemResult>();
    }
}

