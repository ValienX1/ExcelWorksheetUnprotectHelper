using ExcelHelp.core.Models.Item;
using ExcelHelp.core.Services.Interfaces;
using ExcelHelp.core.ViewModels.Base;
using System.Windows.Input;

namespace ExcelHelp.core.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Private members

        private IRepository? _repository;
        private Stack<string> _stack = new();

        #endregion

        #region Public Properties

        public string UnprotectText { get; set; } = "Unprotect"; //Tekst of unprotect button
        public string CloseText { get; set; } = "Close"; //Tekst of Cloes button
        public string CurrentDir { get; set; } = "This device";//Current directory listed in UI List
        public string NameText { get; set; } = "Name"; //UI List header
        public string TypeText { get; set; } = "Type";//UI List header
        public string BackChar { get; set; } = "\uf060"; //Tekst of the back button (arrow left in fonr awesome font)
        public string AdditionaInfoText { get; set; } = "AdditionalInfo";
        public List<ItemBase>? CurrentDirChildren { get; set; } // Current directory children list
        #endregion

        #region Commands

        /// <summary>
        /// Unprotects selected worksheet
        /// </summary>
        public ICommand UnprotectCommand { get; set; }
        /// <summary>
        /// Load children of selected item
        /// </summary>
        public ICommand ListZoomInCommand { get; set; }
        /// <summary>
        /// Going back to parent directory
        /// </summary>
        public ICommand BackCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor with IRepository injection
        /// </summary>
        /// <param name="repository">IRepository injection <see cref="Services.Repository"/> </param>
        public MainWindowViewModel(IRepository repository)
        {
            UnprotectCommand = new RelayParameterizedCommand(p => OnUnprotect(p));
            ListZoomInCommand = new RelayParameterizedCommand((p) => OnListZoomIn(p));
            BackCommand = new RelayCommand(OnBack);
            if (repository != null)
            {
                _repository = repository;
                LoadDrives();
                _stack.Push(string.Empty);
            }
        }

        #endregion

        #region Command Helpers

        /// <summary>
        /// Helper of <see cref="BackCommand"/>
        /// </summary>
        private async void OnBack()
        {
            if (_stack.Count() > 0)
            {
                CurrentDir = _stack.Pop();
                CurrentDirChildren = await _repository!.GetChildrenAsync(CurrentDir);
                if (CurrentDir == string.Empty) CurrentDir = "This device";
            }
        }
        /// <summary>
        /// Helper of <see cref="ListZoomInCommand"/>
        /// </summary>
        /// <param name="p">Selected item of <see cref="CurrentDirChildren"/> in UI list</param>
        private async void OnListZoomIn(object p)
        {
            if (p is Drive drive)
            {
                _stack.Push(string.Empty);
                CurrentDir = drive.Name!;
                CurrentDirChildren = await _repository!.GetChildrenAsync(CurrentDir);
            }
            if (p is Dir dir)
            {
                _stack.Push(CurrentDir);
                CurrentDir += "\\" + dir.Name;
                CurrentDirChildren = await _repository!.GetChildrenAsync(CurrentDir);
            }
            else if (p is Models.Item.File file)
            {
                if (file.Extension == ".xlsm" || file.Extension == ".xlsx")
                {
                    CurrentDirChildren = await _repository!.GetWorksheetsAsync(CurrentDir + "\\" + file.Name);
                }
                /*var nameSpace = "{http://schemas.openxmlformats.org/officeDocument/2006/relationships}";
                using (var zipFile = ZipFile.Open(CurrentDir + "\\" + name, ZipArchiveMode.Update))
                {
                    var entry = zipFile.GetEntry("xl/workbook.xml")!;
                    using Stream stream = entry!.Open();
                    XDocument doc = XDocument.Load(stream);
                    stream.Close();
                    var sheet = doc.Descendants().Where(d => d.Name.LocalName == "sheets").Descendants().Where(d => d.Attribute(nameSpace + "id")!.Value == "rId1").First();
                    sheet.Attribute("name")!.Value = "NewExsion555";
                    entry!.Delete();
                    Stream s = new MemoryStream();
                    doc.Save(s);
                    ZipArchiveEntry newEntry = zipFile.CreateEntry("xl/workbook.xml");
                    using var entryStream = newEntry.Open();
                    {
                        s.Position = 0;
                        s.CopyTo(entryStream);
                    }
                }
                {
                    var zipFile = ZipFile.Open(CurrentDir + "\\" + name, ZipArchiveMode.Update);

                    var e = zipFile.Entries.ToArray()
                        .Where(e => new string(e.ToString().Take(14).ToArray()) == "xl/worksheets/" && new string(e.ToString().Take(19).ToArray()) != "xl/worksheets/_rels")
                        .Select(e => e.ToString().Replace("xl/worksheets/", ""));
                    var wbData = zipFile.Entries.First(e => e.ToString() == "xl/workbook.xml");
                    XDocument doc;
                    using (Stream stream = wbData.Open())
                        doc = XDocument.Load(stream); // Async method available
                    var sheets = doc.Descendants().Where(d => d.Name.LocalName == "sheets").Descendants().Select(d => d.Attribute("name")!.Value).ToList();
                    if (sheets.Any())
                    {
                        _stack.Push(CurrentDir);
                        CurrentDirChildren = sheets.Select(s => new DirChild() { Name = s, Type = ItemType.Worksheet }).ToList();
                        CurrentDir = CurrentDir + "\\" + ((DirChild)p).Name!;
                    }
                }*/
            }
        }
        /// <summary>
        /// Helper of <see cref="UnprotectCommand"/>
        /// </summary>
        private async void OnUnprotect(object p)
        {
            if((await _repository!.UnprotectAsync((ItemBase)p)))
            {
                CurrentDirChildren = await _repository!.GetWorksheetsAsync(((ItemBase)p).CurrentPath);
            }
        }

        #endregion

        #region Private helpers

        /// <summary>
        /// Loads device drives when view is loaded.
        /// </summary>
        private async void LoadDrives()
        {
            CurrentDirChildren = await _repository!.GetChildrenAsync(string.Empty);
        }

        #endregion
    }
}
