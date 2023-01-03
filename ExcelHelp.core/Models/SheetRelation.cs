namespace ExcelHelp.core.Models
{
    /// <summary>
    /// Stores data from Excel file/xl/_rels/workbook.xml.rels
    /// </summary>
    public class SheetRelation
    {
        /// <summary>
        /// Target is worksheet.xml file from inside excel file
        /// </summary>
        public string Target { get; set; } = string.Empty;
        /// <summary>
        /// id is second id used in workbook.xml from inside excel file
        /// </summary>
        public string Id { get; set; } = string.Empty;
    }
}
