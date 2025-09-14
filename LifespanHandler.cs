using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Powercore
{
    internal class LifespanHandler : ILifeSpanHandler
    {
        public mainWnd _mainWnd;
        public LifespanHandler(mainWnd mainWnd)
        {
            _mainWnd = mainWnd;
        }

        public bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            //throw new NotImplementedException();
            return false;
        }

        public void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            //throw new NotImplementedException();
        }

        public void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            //throw new NotImplementedException();
        }

        public bool OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            if (Powercore.Properties.Settings.Default.popups)
            {
                _mainWnd.AddNewTab(targetUrl);
                newBrowser = null;


                return true;
            } else
            {
                newBrowser = new ChromiumWebBrowser(targetUrl);
                return false;
            }
        }
    }
}