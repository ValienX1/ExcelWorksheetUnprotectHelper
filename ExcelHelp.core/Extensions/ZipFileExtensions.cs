using ExcelHelp.core.Extensions;
using System.IO.Compression;
using System.Xml.Linq;
using ExcelHelp.core.Models;
using ExcelHelp.core.Models.Item;
using System.Diagnostics;

namespace ExcelHelp.core.Extensions
{
    public static class ZipFileExtensions
    {
        /// <summary>
        /// Looks voor all worksheets inside xslx or xlsm file.
        /// </summary>
        /// <param name="zipFile">xslx or xlsm file</param>
        /// <returns>List of <see cref="Drive"/> or <see cref="Dir"/> and <see cref="Models.Item.File"/> or <see cref="Sheet"/>
        /// or list with one item type <see cref="Error"/></returns>
        public static async Task<List<ItemBase>> GetWorksheetsAsync(this ZipArchive zipFile)
        {
            try
            {
                var workbookEntry = zipFile.GetEntry("xl/workbook.xml");
                if (workbookEntry == null)
                    throw new Exception("Could not frind xl/workbook.xml file inside this workbook");
                var relationsEntry = zipFile.GetEntry("xl/_rels/workbook.xml.rels");
                if (relationsEntry == null)
                    throw new Exception("Could not frind xl/_rels/workbook.xml.rels file inside this workbook");
                using Stream relsStream = relationsEntry.Open();
                using Stream stream = workbookEntry.Open();
                XDocument doc = await XDocument.LoadAsync(stream, LoadOptions.None, CancellationToken.None);
                var sheets = doc.GetSheets();
                if (sheets.Count == 0)
                    return new() { new Error() { Name = "Exception. Workbook.xml containes no sheet elements." } };
                sheets = await XDocument.Load(relsStream).GetSheetRelations().UpdateSheets(zipFile, sheets);
                return sheets!;
            }
            catch (Exception ex)
            {
                return new() { new Error() { Name = ex.Message } };
            }
        }
    }
}
