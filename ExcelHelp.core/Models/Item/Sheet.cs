namespace ExcelHelp.core.Models.Item
{
    public class Sheet : ItemBase
    {
        /// <summary>
        /// Worksheet name from inside excelffile/xl/_rels/workbook.xml.rels
        /// </summary>
        public string SheetName { get; set; } = string.Empty;
        /// <summary>
        /// Worksheet Id from inside excelffile/xl/_rels/workbook.xml.rels
        /// </summary>
        public string? Id { get; set; } = null;
    }
}
