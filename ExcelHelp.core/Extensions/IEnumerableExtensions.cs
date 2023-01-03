using ExcelHelp.core.Models;
using ExcelHelp.core.Models.Item;
using System.IO.Compression;
using System.Xml.Linq;

namespace ExcelHelp.core.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Updates Sheets list with additional data like if sheet is protected, sheet name inside excel file.
        /// </summary>
        /// <param name="list">List of worksheet relationships</param>
        /// <param name="zipFile">excel file</param>
        /// <param name="sheets">list of sheets</param>
        /// <returns>updated list of sheets</returns>
        public async static Task<List<ItemBase>> UpdateSheets(this IEnumerable<SheetRelation> list, ZipArchive zipFile, List<ItemBase> sheets)
        {
            for (int i = 0; i < list.Count(); i++)
            {
                var sheetEntry = zipFile.GetEntry("xl/" + list.ElementAt(i).Target);
                if (sheetEntry == null)
                    return new() { new Error() { Name = "This workbook is broken. Sheet relations ids match not with sheets ids" } };
                using var sheetStream = sheetEntry.Open();
                List<XElement> node;
                try
                {
                    node = (await XDocument.LoadAsync(sheetStream, LoadOptions.None, CancellationToken.None))
                        .Descendants()
                        .Where(d => d.Name.LocalName == "sheetProtection")
                        .ToList();
                }
                catch (Exception ex)
                {
                    return new() { new Error { Name = $"{ex.GetType().Name} thrown in {list.ElementAt(i).Target}" } };
                }
                var sheet = sheets.First(s => ((Sheet)s).Id == list.ElementAt(i).Id);
                (sheet as Sheet)!.SheetName = list.ElementAt(i).Target;
                sheet.AdditionalInfo = node.Count == 0 ? AdditionalInfo.NotProtected : (node.Any(n => n.Attribute("algorithmName") != null && n.Attribute("hashValue") != null) ? AdditionalInfo.PasswordProtected : AdditionalInfo.Protected);
            }
            return sheets;
        }
    }
}
