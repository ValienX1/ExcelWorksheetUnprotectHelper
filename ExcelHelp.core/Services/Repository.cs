using ExcelHelp.core.Extensions;
using ExcelHelp.core.Models.Item;
using ExcelHelp.core.Services.Interfaces;
using System.IO.Compression;
using System.Xml.Linq;

namespace ExcelHelp.core.Services
{
    /// <summary>
    /// Methods used to get directories/files or worksheets
    /// </summary>
    public class Repository : IRepository
    {
        /// <summary>
        /// Search for directories and files inside given path
        /// </summary>
        /// <param name="path">Path to display</param>
        /// <returns>List of <see cref="ItemBase"/></returns>
        public async Task<List<ItemBase>> GetChildrenAsync(string path)
        {
            return await GetChildren(path);
        }

        /// <summary>
        /// Helper method to GetChildrenAsync
        /// </summary>
        /// <param name="path">Same as in <see cref="GetChildrenAsync(string)"/></param>
        /// <returns>same as in <see cref="GetChildrenAsync(string)"/></returns>
        private Task<List<ItemBase>> GetChildren(string path)
        {
            try
            {
                if (path != string.Empty)
                {
                    var files = Directory.EnumerateFiles(path, "*")
                        .Select(f => new Models.Item.File() { Name = Path.GetFileName(f), Extension = Path.GetExtension(Path.GetFileName(f)) })
                        .OrderBy(f => f.Name).ToList();
                    var dirs = Directory.EnumerateDirectories(path)
                        .Select(d => new Dir() { Name = Path.GetFileName(d) } as ItemBase)
                        .OrderBy(d => d.Name).ToList();
                    return Task.FromResult(dirs.Concat(files).ToList());
                }
                else
                {
                    return Task.FromResult(DriveInfo.GetDrives().Select(d => new Drive() { Name = d.Name } as ItemBase).ToList());
                }
            }
            catch (Exception e)
            {
                return Task.FromResult(new List<ItemBase>() { new Error() { Name = e.Message } });
            }
        }

        /// <summary>
        /// Looks for worksheets inside excel file
        /// </summary>
        /// <param name="path">Path to excel file</param>
        /// <returns>List of worksheets</returns>
        public async Task<List<ItemBase>> GetWorksheetsAsync(string path)
        {
            using var zipFile = ZipFile.Open(path, ZipArchiveMode.Update);
            return (await zipFile.GetWorksheetsAsync())
                .Select(s => new Sheet()
                {
                    Id = ((Sheet)s).Id,
                    Name = s.Name,
                    AdditionalInfo = s.AdditionalInfo,
                    CurrentPath = path,
                    SheetName = s is Sheet sheet ? sheet.SheetName : string.Empty
                } as ItemBase)
                .ToList();
        }

        /// <summary>
        /// Unprotecteds worksheet
        /// </summary>
        /// <param name="item">Worksheet as <see cref="ItemBase"/></param>
        /// <returns>true is succes false if failes</returns>
        public async Task<bool> UnprotectAsync(ItemBase item)
        {
            if (item is not Sheet sheet)
                return false;
            else
            {
                if (sheet.AdditionalInfo != Models.AdditionalInfo.Protected && sheet.AdditionalInfo != Models.AdditionalInfo.PasswordProtected)
                    return false;
                else
                {
                    try
                    {
                        using var zipFile = ZipFile.Open(sheet.CurrentPath, ZipArchiveMode.Update);
                        var sheetEntry = zipFile.GetEntry("xl/" + sheet.SheetName);
                        using Stream stream = sheetEntry!.Open();
                        XDocument doc = await XDocument.LoadAsync(stream, LoadOptions.None, CancellationToken.None);
                        stream.Close();
                        doc.Descendants().Where(d => d.Name.LocalName == "sheetProtection").Remove();
                        sheetEntry.Delete();
                        Stream s = new MemoryStream();
                        doc.Save(s);
                        ZipArchiveEntry newEntry = zipFile.CreateEntry("xl/" + sheet.SheetName);
                        using var entryStream = newEntry.Open();
                        {
                            s.Position = 0;
                            s.CopyTo(entryStream);
                        }
                        sheet.AdditionalInfo = Models.AdditionalInfo.NotProtected;
                        return true;
                    }
                    catch(Exception)
                    {
                        return false;
                    }
                }
            }
        }
    }
}

