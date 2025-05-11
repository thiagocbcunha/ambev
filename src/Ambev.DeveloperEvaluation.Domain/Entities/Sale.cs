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
    /// Unique identifier for the customer.
    /// External identity: Customer
    /// </summary>
    [Required]
    public string CustomerId { get; private set; } = null!;

    /// <summary>
    /// Name of the customer.
    /// </summary>
    [Required]
    public string CustomerName { get; private set; } = null!;

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
    /// Calculates the total amount of the sale.
    /// </summary>
    [NotMapped]
    public decimal TotalAmount => CalculateTotalAmount();

    /// <summary>
    /// Initializes a new instance of the Sale class.
    /// </summary>
    /// <param name="saleNumber"></param>
    /// <param name="saleDate"></param>
    /// <param name="customerId"></param>
    /// <param name="customerName"></param>
    /// <param name="branchId"></param>
    /// <param name="branchName"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public Sale(int saleNumber, DateTime saleDate, string customerId, string customerName, string branchId, string branchName)
    {
        SaleNumber = saleNumber;
        SaleDate = saleDate;
        BranchId = branchId ?? throw new ArgumentNullException(nameof(branchId));
        BranchName = branchName ?? throw new ArgumentNullException(nameof(branchName));
        CustomerId = customerId ?? throw new ArgumentNullException(nameof(customerId));
        CustomerName = customerName ?? throw new ArgumentNullException(nameof(customerName));
    }

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
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegative(unitPrice);
        ArgumentOutOfRangeException.ThrowIfNegative(discount);

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
        {
            total += item.TotalAmount;
        }
        return total;
    }

    public void UpdateDetails(int saleNumber, DateTime saleDate, string customerId, string customerName, string branchId, string branchName)
    {
        SaleNumber = saleNumber;
        SaleDate = saleDate;
        CustomerId = customerId ?? throw new ArgumentNullException(nameof(customerId));
        CustomerName = customerName ?? throw new ArgumentNullException(nameof(customerName));
        BranchId = branchId ?? throw new ArgumentNullException(nameof(branchId));
        BranchName = branchName ?? throw new ArgumentNullException(nameof(branchName));
    }

    public void ClearItems()
    {
        Items.Clear();
    }
}
