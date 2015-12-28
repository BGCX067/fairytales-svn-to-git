using System;
using WatiN.Core.Interfaces;

namespace ConeFabric.FairyTales.Tests
{
    public class FakeButton : IButton
    {
        public bool clickCalled;

        #region IButton Members
        public string Text
        {
            get { throw new NotImplementedException(); }
        }

        public string Value
        {
            get { throw new NotImplementedException(); }
        }

        public string ClassName
        {
            get { throw new NotImplementedException(); }
        }

        public void Click()
        {
            clickCalled = true;
        }

        public bool Enabled
        {
            get { throw new NotImplementedException(); }
        }

        public bool Exists
        {
            get { return true; }
        }

        public void FireEvent(string eventName)
        {
            throw new NotImplementedException();
        }

        public void FireEventNoWait(string eventName)
        {
            throw new NotImplementedException();
        }

        public void Focus()
        {
            throw new NotImplementedException();
        }

        public string GetAttributeValue(string attributeName)
        {
            throw new NotImplementedException();
        }

        public string Id
        {
            get { throw new NotImplementedException(); }
        }

        public string InnerHtml
        {
            get { throw new NotImplementedException(); }
        }

        public IElement NextSibling
        {
            get { throw new NotImplementedException(); }
        }

        public IElement Parent
        {
            get { throw new NotImplementedException(); }
        }

        public IElement PreviousSibling
        {
            get { throw new NotImplementedException(); }
        }

        public string TagName
        {
            get { throw new NotImplementedException(); }
        }

        public string Title
        {
            get { throw new NotImplementedException(); }
        }
        #endregion
    }
}