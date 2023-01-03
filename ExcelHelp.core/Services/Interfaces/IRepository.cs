using ExcelHelp.core.Models.Item;

namespace ExcelHelp.core.Services.Interfaces
{
    /// <summary>
    /// Interface with get and update methods
    /// </summary>
    public interface IRepository
    {
        Task<List<ItemBase>> GetChildrenAsync(string path);
        Task<List<ItemBase>> GetWorksheetsAsync(string path);
        Task<bool> UnprotectAsync(ItemBase sheet);
    }
}
