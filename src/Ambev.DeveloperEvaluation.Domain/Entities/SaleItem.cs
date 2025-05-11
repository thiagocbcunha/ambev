using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem
{
    public const decimal MaxDiscount = 0.2m;
    public const decimal MinDiscount = 0.1m;

    public const int MaxQuantity = 20;
    public const int AverageQuantity = 10;
    public const int MinQuantityForDiscount = 4;

    public SaleItem() { }

    /// <summary>
    /// Initializes a new instance of the SaleItem class.
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="productName"></param>
    /// <param name="quantity"></param>
    /// <param name="unitPrice"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public SaleItem(string productId, string productName, int quantity, decimal unitPrice)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(unitPrice);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

        Quantity = quantity;
        ProductId = productId;
        UnitPrice = unitPrice;

        ProductName = productName 
            ?? throw new ArgumentNullException(nameof(productName));

        ThrowIfIsInvalidSaleItem();
    }

    /// <summary>
    /// Initializes a new instance of the SaleItem class.
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="productName"></param>
    /// <param name="quantity"></param>
    /// <param name="unitPrice"></param>
    /// <param name="saleId"></param>
    public SaleItem(string productId, string productName, int quantity, decimal unitPrice, Guid saleId) 
        : this(productId, productName, quantity, unitPrice)
            => SaleId = saleId;


    /// <summary>
    /// Unique identifier for the sale item.
    /// </summary>
    [Key]
    public Guid Id { get; private set; } = Guid.NewGuid();

    /// <summary>
    /// Unique identifier for the product.
    /// </summary>
    [Required]
    public string ProductId { get; private set; } = null!;

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

    public bool IsCancelled { get; private set; }

    /// <summary>
    /// Updates the sale item with new values.
    /// </summary>
    /// <param name="productName"></param>
    /// <param name="quantity"></param>
    /// <param name="unitPrice"></param>
    public void Patch(string? productId, string? productName, int? quantity, decimal? unitPrice)
    {
        if(unitPrice is not null)
            ArgumentOutOfRangeException.ThrowIfNegative(unitPrice.Value);

        if(quantity is not null)
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity.Value);

        Quantity = quantity ?? Quantity;
        UnitPrice = unitPrice ?? UnitPrice;
        ProductId = productId ?? ProductId;
        ProductName = productName ?? ProductName;

        ThrowIfIsInvalidSaleItem();

        CalculateDiscount();
    }

    /// <summary>
    /// Validates the sale item.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void ThrowIfIsInvalidSaleItem()
    {
        ArgumentOutOfRangeException.ThrowIfNegative(UnitPrice);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(Quantity);

        if (Quantity > MaxQuantity)
            throw new ArgumentOutOfRangeException(nameof(Quantity), "Quantity cannot exceed 20.");
    }

    /// <summary>
    /// Calculates the discount based on the quantity of items sold.
    /// </summary>
    private void CalculateDiscount()
    {
        var discount = 0m;

        if(Quantity >= MinQuantityForDiscount && Quantity < AverageQuantity)
            discount = MinDiscount;

        if(Quantity >= AverageQuantity && Quantity <= MaxQuantity)
            discount = MaxDiscount;

        Discount = UnitPrice * Quantity * discount;
    }

    /// <summary>
    /// Cancels the sale Item.
    /// </summary>
    public void CancelSaleItem()
    {
        IsCancelled = true;
    }
}
