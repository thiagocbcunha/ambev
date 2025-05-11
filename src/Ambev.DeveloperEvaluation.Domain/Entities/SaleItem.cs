using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem
{
    internal SaleItem() { }

    /// <summary>
    /// Initializes a new instance of the SaleItem class.
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="productName"></param>
    /// <param name="quantity"></param>
    /// <param name="unitPrice"></param>
    /// <param name="discount"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public SaleItem(Guid productId, string productName, int quantity, decimal unitPrice, decimal discount)
    {
        Quantity = quantity;
        Discount = discount;
        ProductId = productId;
        UnitPrice = unitPrice;
        ProductName = productName ?? throw new ArgumentNullException(nameof(productName));
    }
    /// <summary>
    /// Unique identifier for the sale item.
    /// </summary>
    [Key]
    public Guid Id { get; private set; } = Guid.NewGuid();

    /// <summary>
    /// Unique identifier for the product.
    /// </summary>
    [Required]
    public Guid ProductId { get; private set; }

    /// <summary>
    /// Name of the product.
    /// </summary>
    [Required]
    public string ProductName { get; private set; } = null!;

    /// <summary>
    /// Quantity of the product sold.
    /// </summary>

    [Required]
    public int Quantity { get; private set; }

    /// <summary>
    /// Unit price of the product.
    /// </summary>
    [Required]
    public decimal UnitPrice { get; private set; }

    /// <summary>
    /// Discount applied to the product.
    /// </summary>
    [Required]
    public decimal Discount { get; private set; }

    /// <summary>
    /// Unique identifier for the sale.
    /// </summary>
    [Required]
    public Guid SaleId { get; private set; }

    /// <summary>
    /// Navigation property to the associated sale.
    /// </summary>
    [ForeignKey(nameof(SaleId))]
    public Sale Sale { get; private set; } = null!;

    [NotMapped]
    public decimal TotalAmount => (UnitPrice * Quantity) - Discount;
}
