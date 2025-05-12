using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.CreateSaleItem;

/// <summary>
/// Profile for mapping CreateSaleCommand and Sale entities.
/// </summary>
public class CreateSaleItemProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the CreateSaleProfile class.
    /// </summary>
    public CreateSaleItemProfile()
    {
        CreateMap<CreateSaleItemCommand, SaleItem>();
        CreateMap<SaleItem, CreateSaleItemResult>();
    }
}
