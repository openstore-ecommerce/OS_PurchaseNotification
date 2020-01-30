using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Roles;
using NBrightCore.common;
using NBrightDNN;
using Nevoweb.DNN.NBrightBuy.Components;

namespace OpenStore.Providers.OS_PurchaseNotification
{
    public static class LocalUtils
    {

        public static string GetData(string lang, string razortemplate = "admin.cshtml")
        {
            var objCtrl = new NBrightBuyController();
            var info = objCtrl.GetPluginSinglePageData("OS_PurchaseNotificationDATA", "OS_PurchaseNotificationDATA", lang);
            return NBrightBuyUtils.RazorTemplRender(razortemplate, 0, "", info, "/DesktopModules/NBright/OS_PurchaseNotification", "config", lang, StoreSettings.Current.Settings());
        }

        public static Boolean CheckRights()
        {
            if (UserController.Instance.GetCurrentUserInfo().IsInRole(StoreSettings.ManagerRole) || UserController.Instance.GetCurrentUserInfo().IsInRole(StoreSettings.EditorRole) || UserController.Instance.GetCurrentUserInfo().IsInRole("Administrators"))
            {
                return true;
            }
            return false;
        }

        public static void OutputTestEmail()
        {
            var objCtrl = new NBrightBuyController();
            var l = objCtrl.GetList(PortalSettings.Current.PortalId, -1, "ORDER", " and NB1.XMLdata.value('(genxml/productlimitflag)[1]','nvarchar(max)') = 'True' ", "", 1);
            if (l.Count > 0)
            {
                var nbi = l.First();
                var orderData = new OrderData(nbi.ItemID);
                var passSettings = new Dictionary<string, string>();
                var emailBody = NBrightBuyUtils.RazorTemplRender("EmailHtmlOutput.cshtml", 0, "", orderData, "/DesktopModules/NBright/OS_PurchaseNotification", "config", orderData.Lang, passSettings);
                Utils.SaveFile(PortalSettings.Current.HomeDirectoryMapPath + "\\testemail.html", emailBody);
            }
            else
            {
                Utils.SaveFile(PortalSettings.Current.HomeDirectoryMapPath + "\\testemail.html", "NO ORDERS with purchase notification");
            }
        }
    }
}
