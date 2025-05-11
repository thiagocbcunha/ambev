using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Sale entity representing a sale transaction.
/// </summary>
public class Sale
{
    // EF constructor
    public Sale() { }

    /// <summary>
    /// Initializes a new instance of the Sale class.
    /// </summary>
    /// <param name="saleNumber"></param>
    /// <param name="saleDate"></param>
    /// <param name="customerId"></param>
    /// <param name="branchId"></param>
    /// <param name="branchName"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public Sale(int saleNumber, DateTime saleDate, Guid sallerId, Guid customerId, string branchId, string branchName)
    {
        SaleDate = saleDate;
        SallerId = sallerId;
        SaleNumber = saleNumber;
        CustomerId = customerId;

        BranchId = branchId 
            ?? throw new ArgumentNullException(nameof(branchId));

        BranchName = branchName 
            ?? throw new ArgumentNullException(nameof(branchName));

    }

    /// <summary>
    /// Unique identifier for the sale.
    /// </summary>
    [Key]
    public Guid Id { get; private set; } = Guid.NewGuid();

    /// <summary>
    /// Unique sale number.
    /// </summary>
    [Required]
    public int SaleNumber { get; private set; }

    /// <summary>
    /// Date of the sale.
    /// </summary>
    [Required]
    public DateTime SaleDate { get; private set; }

    /// <summary>
    /// Unique identifier for the branch.
    /// External identity: Branch
    /// </summary>
    [Required]
    public string BranchId { get; private set; } = null!;

    /// <summary>
    /// Name of the branch.
    /// </summary>
    [Required]
    public string BranchName { get; private set; } = null!;

    /// <summary>
    /// Indicates whether the sale is cancelled.
    /// </summary>
    [Required]
    public bool IsCancelled { get; private set; } = false;

    /// <summary>
    /// Collection of sale items associated with the sale.
    /// </summary>
    public ICollection<SaleItem> Items { get; private set; } = [];

    /// <summary>
    /// User identifier of the customer associated with the sale.
    /// </summary>
    [Required]
    public Guid CustomerId { get; private set; }

    /// <summary>
    /// User identifier of the customer associated with the sale.
    /// </summary>
    [ForeignKey(nameof(CustomerId))]
    public User Customer { get; private set; } = null!;

    /// <summary>
    /// User identifier of the seller associated with the sale.
    /// </summary>
    [Required]
    public Guid SallerId { get; private set; }

    /// <summary>
    /// User identifier of the seller associated with the sale.
    /// </summary>
    [ForeignKey(nameof(SallerId))]
    public User Saller { get; private set; } = null!;

    /// <summary>
    /// Calculates the total amount of the sale.
    /// </summary>
    [NotMapped]
    public decimal TotalAmount => CalculateTotalAmount();

    /// <summary>
    /// Adds an item to the sale.
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="productName"></param>
    /// <param name="quantity"></param>
    /// <param name="unitPrice"></param>
    /// <param name="discount"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void AddItem(Guid productId, string productName, int quantity, decimal unitPrice, decimal discount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(discount);
        ArgumentOutOfRangeException.ThrowIfNegative(unitPrice);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

        Items.Add(new SaleItem(productId, productName, quantity, unitPrice, discount));
    }

    /// <summary>
    /// Cancels the sale.
    /// </summary>
    public void CancelSale()
    {
        IsCancelled = true;
    }

    /// <summary>
    /// Calculates the total amount of the sale by summing the total amounts of all items.
    /// </summary>
    /// <returns></returns>
    private decimal CalculateTotalAmount()
    {
        decimal total = 0;
        foreach (var item in Items)
            total += item.TotalAmount;
        
        return total;
    }

    public void Update(int saleNumber, DateTime saleDate, Guid sallerId, Guid customerId, string branchId, string branchName)
    {
        SaleDate = saleDate;
        SallerId = sallerId;
        CustomerId = customerId;
        SaleNumber = saleNumber;

        BranchId = branchId 
            ?? throw new ArgumentNullException(nameof(branchId));

        BranchName = branchName 
            ?? throw new ArgumentNullException(nameof(branchName));
    }
}
