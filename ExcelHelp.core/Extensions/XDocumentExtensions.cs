using ExcelHelp.core.Models;
using ExcelHelp.core.Models.Item;
using System.Xml.Linq;

namespace ExcelHelp.core.Extensions
{
    public static class XDocumentExtensions
    {
        //Namespace of second id attribute on sheet node inside sheets node inside workbook.xml
        private const string NameSpace = "{http://schemas.openxmlformats.org/officeDocument/2006/relationships}";
        //Type of node inside Woorkbook.xml.rels file
        private const string WorksheetTypeName = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet";
        /// <summary>
        /// Search for all nodes inside Node "sheets"
        /// </summary>
        /// <param name="doc">Woorkbook.xml file from inside xlsx or xlsm file</param>
        /// <returns>List of <see cref="ItemBase"/> objects list of worksheets</returns>
        public static List<ItemBase> GetSheets(this XDocument doc)
        {
            return doc.Descendants()
                    .Where(d => d.Name.LocalName == "sheets")
                    .Descendants()
                    .Select(d => new Sheet() { Name = d.Attribute("name")!.Value, Id = d.Attribute(NameSpace + "id")!.Value } as ItemBase)
                    .ToList();
        }
        /// <summary>
        /// Looks for worksheet.xml file name and id linked with
        /// </summary>
        /// <param name="doc">Woorkbook.xml.rels file from inside xlsx or xlsm file</param>
        /// <returns>List of <see cref="SheetRelation"/> used to link sheet object from workbook sheets with worksheets.xml files</returns>
        public static List<SheetRelation> GetSheetRelations(this XDocument doc)
        {
            return doc.Descendants()
                .Where(d => d.Attribute("Type") != null)
                .Where(d => d.Attribute("Type")!.Value == WorksheetTypeName)
                .Select(d => new SheetRelation() { Target = d.Attribute("Target")!.Value, Id = d.Attribute("Id")!.Value })
                .ToList();
        }
    }
}
