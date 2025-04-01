using Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices;

namespace DocumentsLibrary
{
    public class WordService : IDisposable
    {
        private Application _wordApp;
        private Document _document;

        public void CreatePdf(string fileName, string title, string body)
        {
            _wordApp = new Application();
            _wordApp.Visible = false;
            _document = _wordApp.Documents.Add();

            AddParagraph(title, 1, _document);
            AddParagraph("", 2, _document);
            AddParagraph(body, 3, _document);

            var titleRange = _document.Paragraphs[1].Range;
            titleRange.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            titleRange.Font.Size = 16;
            titleRange.Font.Bold = 1;

            for (int i = 2; i < _document.Paragraphs.Count; i++)
            {
                var range = _document.Paragraphs[i].Range;
                range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;
                range.Font.Size = 12;
            }

            _document.SaveAs(fileName, WdSaveFormat.wdFormatPDF);
        }

        private static void AddParagraph(string text, int number, Document document)
        {
            var paragraph = document.Paragraphs.Add();
            var range = document.Paragraphs[number].Range;
            range.Text = text;
        }

        public void Dispose()
        {
            if (_document != null)
            {
                _document.Close(false);

                Marshal.ReleaseComObject(_document);
                _document = null;
            }

            if (_wordApp != null)
            {
                _wordApp.Quit();

                Marshal.ReleaseComObject(_wordApp);
                _wordApp = null;
            }
        }
    }
}
