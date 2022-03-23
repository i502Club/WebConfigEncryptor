/*
' Copyright (c) 2022 i502 Club
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using DotNetNuke.Abstractions.Logging;
using DotNetNuke.Services.Localization;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using System.Configuration;
using System.Web.Mvc;

namespace i502Club.Modules.WebConfigEncryptor.Controllers
{
    [DnnHandleError]
    public class ItemController : DnnController
    {
        private readonly IEventLogService _eventLogService;
        private static readonly string _logName = "WebConfigEncyrptor";
        public ItemController(IEventLogService eventLogService)
        {
            _eventLogService = eventLogService;
        }

        // Open the Web.config file
        private static readonly System.Configuration.Configuration _config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");

        // Set the connectionStrings _section
        private static readonly System.Configuration.ConnectionStringsSection _section = _config.GetSection("connectionStrings") as ConnectionStringsSection;

        private void ToggleWebEncrypt()
        {
            // Toggle encryption. 
            if (_section.SectionInformation.IsProtected)
            {
                _section.SectionInformation.UnprotectSection();
                _eventLogService.AddLog(_logName, Localization.GetString("LogMsgDecrypted", LocalResourceFile), PortalSettings, -1, DotNetNuke.Abstractions.Logging.EventLogType.ADMIN_ALERT);
            }
            else
            {
                _section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                _eventLogService.AddLog(_logName, Localization.GetString("LogMsgEncrypted", LocalResourceFile), PortalSettings, -1, DotNetNuke.Abstractions.Logging.EventLogType.ADMIN_ALERT);
            }

            _config.Save();
        }

        public ActionResult Index()
        {
            // set localized string vars to use in the view
            var operationTypeEncrypt = Localization.GetString("OperationTypeEncrypt", LocalResourceFile);
            var operationTypeDecrypt = Localization.GetString("OperationTypeDecrypt", LocalResourceFile);
            var msgToUserEncrypt = Localization.GetString("MsgToUserEncrypt", LocalResourceFile);
            var msgToUserDecrypt = Localization.GetString("MsgToUserDecrypt", LocalResourceFile);

            // add some properties to the view bag
            ViewBag.isWebConfigEncrypted = _section.SectionInformation.IsProtected;
            ViewBag.operationType = ViewBag.isWebConfigEncrypted == true ? operationTypeDecrypt : operationTypeEncrypt;
            ViewBag.msgToUser = ViewBag.isWebConfigEncrypted == true ? msgToUserDecrypt : msgToUserEncrypt;
            ViewBag.isSuperUser = User.IsSuperUser;

            // handle post back from btn and toggle encryption
            if (Request.RequestType == "POST" && Request.Form["btnToggleEncryption"] != null && User.IsSuperUser)
            {
                if (_section.SectionInformation.IsProtected)
                {
                    ViewBag.operationType = operationTypeEncrypt;
                    ViewBag.msgToUser = msgToUserEncrypt;
                }
                else
                {
                    ViewBag.operationType = operationTypeDecrypt;
                    ViewBag.msgToUser = msgToUserDecrypt;
                }

                ToggleWebEncrypt();
            }

            return View();
        }
    }
}
