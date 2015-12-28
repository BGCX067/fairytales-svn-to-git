using System;
using System.Text.RegularExpressions;
using WatiN.Core;
using WatiN.Core.Constraints;
using WatiN.Core.Interfaces;

namespace ConeFabric.FairyTales.Tests
{
    public class FakeBrowser : IBrowser
    {
        public bool DisposeCalled;
        public bool WaitForCompleteCalled;

        public static IBrowser WithSpan(ISpan span)
        {
            var browser = new FakeBrowser();
            browser.span = span;
            return browser;
        }

        public static FakeBrowser WithButton(FakeButton button)
        {
            var browser = new FakeBrowser();
            browser.button = button;
            return browser;
        }

        #region IBrowser Members

        public void Back()
        {
            throw new NotImplementedException();
        }

        public void BringToFront()
        {
            throw new NotImplementedException();
        }

        public BrowserType BrowserType
        {
            get { throw new NotImplementedException(); }
        }

        public bool ContainsText(Regex regex)
        {
            throw new NotImplementedException();
        }

        public bool ContainsText(string text)
        {
            throw new NotImplementedException();
        }

        public string Eval(string javaScriptCode)
        {
            throw new NotImplementedException();
        }

        public string FindText(Regex regex)
        {
            throw new NotImplementedException();
        }

        public void Forward()
        {
            throw new NotImplementedException();
        }

        public NativeMethods.WindowShowStyle GetWindowStyle()
        {
            throw new NotImplementedException();
        }

        public void GoTo(Uri url)
        {
            throw new NotImplementedException();
        }

        public void GoTo(string url)
        {
            this.url = url;
        }

        public void PressTab()
        {
            throw new NotImplementedException();
        }

        public int ProcessID
        {
            get { throw new NotImplementedException(); }
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        public void Reopen()
        {
            throw new NotImplementedException();
        }

        public void RunScript(string javaScriptCode)
        {
            throw new NotImplementedException();
        }

        public void ShowWindow(NativeMethods.WindowShowStyle showStyle)
        {
            throw new NotImplementedException();
        }

        public void WaitForComplete()
        {
            WaitForCompleteCalled = true;
        }

        public IntPtr hWnd
        {
            get { throw new NotImplementedException(); }
        }

        public IElement ActiveElement
        {
            get { throw new NotImplementedException(); }
        }

        public string Html
        {
            get { throw new NotImplementedException(); }
        }

        public string Text
        {
            get { throw new NotImplementedException(); }
        }

        public string Title
        {
            get { throw new NotImplementedException(); }
        }

        public Uri Uri
        {
            get { throw new NotImplementedException(); }
        }

        public string Url
        {
            get { return url; }
        }

        public IArea Area(BaseConstraint findBy)
        {
            throw new NotImplementedException();
        }

        public IArea Area(Regex id)
        {
            throw new NotImplementedException();
        }

        public IArea Area(string id)
        {
            throw new NotImplementedException();
        }

        public IAreaCollection Areas
        {
            get { throw new NotImplementedException(); }
        }

        public IButton Button(BaseConstraint constraint)
        {
            throw new NotImplementedException();
        }

        public IButton Button(Regex regex)
        {
            throw new NotImplementedException();
        }

        public IButton Button(string id)
        {
            return button;
        }

        public IButtonCollection Buttons
        {
            get { throw new NotImplementedException(); }
        }

        public ICheckBox CheckBox(BaseConstraint findBy)
        {
            throw new NotImplementedException();
        }

        public ICheckBox CheckBox(Regex id)
        {
            throw new NotImplementedException();
        }

        public ICheckBox CheckBox(string id)
        {
            throw new NotImplementedException();
        }

        public ICheckBoxCollection CheckBoxes
        {
            get { throw new NotImplementedException(); }
        }

        public IDiv Div(BaseConstraint findBy)
        {
            throw new NotImplementedException();
        }

        public IDiv Div(Regex id)
        {
            throw new NotImplementedException();
        }

        public IDiv Div(string id)
        {
            throw new NotImplementedException();
        }

        public IDivCollection Divs
        {
            get { throw new NotImplementedException(); }
        }

        public IElement Element(BaseConstraint findBy)
        {
            throw new NotImplementedException();
        }

        public IElement Element(Regex id)
        {
            throw new NotImplementedException();
        }

        public IElement Element(string id)
        {
            throw new NotImplementedException();
        }

        public IWatiNElementCollection Elements
        {
            get { throw new NotImplementedException(); }
        }

        public IForm Form(BaseConstraint findBy)
        {
            throw new NotImplementedException();
        }

        public IForm Form(Regex id)
        {
            throw new NotImplementedException();
        }

        public IForm Form(string id)
        {
            throw new NotImplementedException();
        }

        public IFormsCollection Forms
        {
            get { throw new NotImplementedException(); }
        }

        public IFrame Frame(BaseConstraint findBy)
        {
            throw new NotImplementedException();
        }

        public IFrame Frame(Regex id)
        {
            throw new NotImplementedException();
        }

        public IFrame Frame(string id)
        {
            throw new NotImplementedException();
        }

        public IFrameCollection Frames
        {
            get { throw new NotImplementedException(); }
        }

        public IImage Image(BaseConstraint findBy)
        {
            throw new NotImplementedException();
        }

        public IImage Image(Regex id)
        {
            throw new NotImplementedException();
        }

        public IImage Image(string id)
        {
            throw new NotImplementedException();
        }

        public IImageCollection Images
        {
            get { throw new NotImplementedException(); }
        }

        public ILabel Label(BaseConstraint findBy)
        {
            throw new NotImplementedException();
        }

        public ILabel Label(Regex id)
        {
            throw new NotImplementedException();
        }

        public ILabel Label(string id)
        {
            throw new NotImplementedException();
        }

        public ILabelCollection Labels
        {
            get { throw new NotImplementedException(); }
        }

        public ILink Link(BaseConstraint findBy)
        {
            throw new NotImplementedException();
        }

        public ILink Link(Regex id)
        {
            throw new NotImplementedException();
        }

        public ILink Link(string id)
        {
            throw new NotImplementedException();
        }

        public ILinkCollection Links
        {
            get { throw new NotImplementedException(); }
        }

        public IPara Para(BaseConstraint findBy)
        {
            throw new NotImplementedException();
        }

        public IPara Para(Regex id)
        {
            throw new NotImplementedException();
        }

        public IPara Para(string id)
        {
            throw new NotImplementedException();
        }

        public IParaCollection Paras
        {
            get { throw new NotImplementedException(); }
        }

        public IRadioButton RadioButton(BaseConstraint findBy)
        {
            throw new NotImplementedException();
        }

        public IRadioButton RadioButton(Regex id)
        {
            throw new NotImplementedException();
        }

        public IRadioButton RadioButton(string id)
        {
            throw new NotImplementedException();
        }

        public IRadioButtonCollection RadioButtons
        {
            get { throw new NotImplementedException(); }
        }

        public ISelectList SelectList(BaseConstraint findBy)
        {
            throw new NotImplementedException();
        }

        public ISelectList SelectList(Regex id)
        {
            throw new NotImplementedException();
        }

        public ISelectList SelectList(string id)
        {
            throw new NotImplementedException();
        }

        public ISelectListCollection SelectLists
        {
            get { throw new NotImplementedException(); }
        }

        public ISpan Span(BaseConstraint findBy)
        {
            throw new NotImplementedException();
        }

        public ISpan Span(Regex id)
        {
            throw new NotImplementedException();
        }

        public ISpan Span(string id)
        {
            return span;
        }

        public ISpanCollection Spans
        {
            get { throw new NotImplementedException(); }
        }

        public ITable Table(BaseConstraint findBy)
        {
            throw new NotImplementedException();
        }

        public ITable Table(Regex id)
        {
            throw new NotImplementedException();
        }

        public ITable Table(string id)
        {
            throw new NotImplementedException();
        }

        public ITableBodyCollection TableBodies
        {
            get { throw new NotImplementedException(); }
        }

        public ITableBody TableBody(BaseConstraint findBy)
        {
            throw new NotImplementedException();
        }

        public ITableBody TableBody(Regex id)
        {
            throw new NotImplementedException();
        }

        public ITableBody TableBody(string id)
        {
            throw new NotImplementedException();
        }

        public ITableCell TableCell(BaseConstraint findBy)
        {
            throw new NotImplementedException();
        }

        public ITableCell TableCell(Regex id)
        {
            throw new NotImplementedException();
        }

        public ITableCell TableCell(string id)
        {
            throw new NotImplementedException();
        }

        public ITableCellCollection TableCells
        {
            get { throw new NotImplementedException(); }
        }

        public ITableRow TableRow(BaseConstraint findBy)
        {
            throw new NotImplementedException();
        }

        public ITableRow TableRow(Regex id)
        {
            throw new NotImplementedException();
        }

        public ITableRow TableRow(string id)
        {
            throw new NotImplementedException();
        }

        public ITableRowCollection TableRows
        {
            get { throw new NotImplementedException(); }
        }

        public ITableCollection Tables
        {
            get { throw new NotImplementedException(); }
        }

        public ITextField TextField(BaseConstraint constraint)
        {
            throw new NotImplementedException();
        }

        public ITextField TextField(Regex regex)
        {
            throw new NotImplementedException();
        }

        public ITextField TextField(string id)
        {
            throw new NotImplementedException();
        }

        public ITextFieldCollection TextFields
        {
            get { throw new NotImplementedException(); }
        }

        public void Dispose()
        {
            DisposeCalled = true;
        }

        #endregion

        private IButton button;
        private ISpan span;
        private string url = string.Empty; 
        
    }
}