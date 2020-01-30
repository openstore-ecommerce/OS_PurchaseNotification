using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Compilation;
using System.Xml;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Services.Localization;
using NBrightCore.common;
using NBrightDNN;
using Nevoweb.DNN.NBrightBuy.Components;
using Nevoweb.DNN.NBrightBuy.Components.Products;
using Nevoweb.DNN.NBrightBuy.Components.Interfaces;
using RazorEngine.Compilation.ImpromptuInterface.InvokeExt;
using DotNetNuke.Entities.Users;

namespace OpenStore.Providers.OS_PurchaseNotification
{
    public class AjaxProvider : AjaxInterface
    {
        public override string Ajaxkey { get; set; }

        public override string ProcessCommand(string paramCmd, HttpContext context, string editlang = "")
        {
            if (!LocalUtils.CheckRights())
            {
                return "Security Error.";
            }

            var ajaxInfo = NBrightBuyUtils.GetAjaxFields(context);
            var lang = NBrightBuyUtils.SetContextLangauge(ajaxInfo); // Ajax breaks context with DNN, so reset the context language to match the client.
            var objCtrl = new NBrightBuyController();

            var strOut = "OS_PurchaseNotification Ajax Error";

            // NOTE: The paramCmd MUST start with the plugin ref. in lowercase. (links ajax provider to cmd)
            switch (paramCmd)
            {
                case "os_purchasenotification_getdata":
                    strOut = LocalUtils.GetData(editlang, "datafields.cshtml");
                    break;
                case "os_purchasenotification_deleterecord":
                    var infoDel = objCtrl.GetPluginSinglePageData("OS_PurchaseNotificationDATA", "OS_PurchaseNotificationDATA", Utils.GetCurrentCulture());
                    objCtrl.Delete(infoDel.ItemID);
                    break;
                case "os_purchasenotification_savedata":
                    strOut = objCtrl.SavePluginSinglePageData(context);
                    break;
                case "os_purchasenotification_selectlang":
                    objCtrl.SavePluginSinglePageData(context);
                    strOut = "";
                    break;
                case "os_purchasenotification_testemail":
                    LocalUtils.OutputTestEmail();
                    strOut = LocalUtils.GetData(editlang, "datafields.cshtml");
                    break;
            }

            return strOut;

        }

        public override void Validate()
        {
        }


    }
}
