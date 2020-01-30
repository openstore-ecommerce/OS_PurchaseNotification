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
    public class Events : Nevoweb.DNN.NBrightBuy.Components.Interfaces.EventInterface
    {
        public override NBrightInfo ValidateCartBefore(NBrightInfo cartInfo)
        {
            return cartInfo;
        }
        public override NBrightInfo ValidateCartAfter(NBrightInfo cartInfo)
        {
            return cartInfo;
        }

        public override NBrightInfo ValidateCartItemBefore(NBrightInfo cartItemInfo)
        {
            return cartItemInfo;
        }

        public override NBrightInfo ValidateCartItemAfter(NBrightInfo cartItemInfo)
        {
            return cartItemInfo;
        }

        public override NBrightInfo AfterCartSave(NBrightInfo nbrightInfo)
        {
            return nbrightInfo;
        }

        public override NBrightInfo AfterCategorySave(NBrightInfo nbrightInfo)
        {
            return nbrightInfo;
        }

        public override NBrightInfo AfterProductSave(NBrightInfo nbrightInfo)
        {
            return nbrightInfo;
        }

        public override NBrightInfo AfterSavePurchaseData(NBrightInfo nbrightInfo)
        {
            return nbrightInfo;
        }

        public override NBrightInfo BeforeOrderStatusChange(NBrightInfo nbrightInfo)
        {
            return nbrightInfo;
        }

        public override NBrightInfo AfterOrderStatusChange(NBrightInfo nbrightInfo)
        {
            return nbrightInfo;
        }

        public override NBrightInfo BeforePaymentOK(NBrightInfo nbrightInfo)
        {
            return nbrightInfo;
        }

        public override NBrightInfo AfterPaymentOK(NBrightInfo nbrightInfo)
        {
            return PurchaseNotify(nbrightInfo);
        }
        private NBrightInfo PurchaseNotify(NBrightInfo nbrightInfo)
        {
            var objCtrl = new NBrightBuyController();
            var purhcaseNotifyInfo = objCtrl.GetPluginSinglePageData("OS_PurchaseNotificationDATA", "OS_PurchaseNotificationDATA", Utils.GetCurrentCulture());
            if (purhcaseNotifyInfo.GetXmlPropertyBool("genxml/checkbox/turnoff")) return nbrightInfo;

            var orderData = new OrderData(nbrightInfo.ItemID);

            // remove and products that have reached the client purchase limit
            var cartitemList = orderData.GetCartItemList();
            var sendEmailFlag = false;

            foreach (var cartItemInfo in cartitemList)
            {
                var productid = cartItemInfo.GetXmlPropertyInt("genxml/productid");
                if (productid > 0)
                {
                    var modelid = cartItemInfo.GetXmlProperty("genxml/modelid");
                    var productData = new ProductData(productid, Utils.GetCurrentCulture(), true);
                    var model = productData.GetModel(modelid);
                    if (model != null)
                    {
                        var purchaseNotificationFlag = model.GetXmlPropertyBool("genxml/checkbox/purchasenotificationflag");
                        if (purchaseNotificationFlag)
                        {
                            sendEmailFlag = true;  // Just put a flag, most of work done in Razor Template.
                        }
                    }
                }
            }

            if (sendEmailFlag)
            {
                var passSettings = new Dictionary<string, string>();
                var emailBody = NBrightBuyUtils.RazorTemplRender("EmailHtmlOutput.cshtml", 0, "", orderData, "/DesktopModules/NBright/OS_PurchaseNotification", "config", orderData.Lang, passSettings);

                var emailList = StoreSettings.Current.ManagerEmail + ",";
                emailList += purhcaseNotifyInfo.GetXmlProperty("genxml/textbox/emailcsv");
                emailList = emailList.TrimEnd(',');

                var emailSubject = purhcaseNotifyInfo.GetXmlProperty("genxml/textbox/emailsubject");

                NBrightBuyUtils.SendEmail(emailBody, emailList, "", orderData.GetInfo(), emailSubject, "", orderData.Lang);
            }

            return nbrightInfo;
        }
        public NBrightInfo SendOrderEmail(string emailtype, int orderId, string emailsubjectresxkey, string fromEmail, string emailmsg, string emailList, NBrightInfo objInfo)
        {
            return objInfo;
        }

        public override NBrightInfo BeforePaymentFail(NBrightInfo nbrightInfo)
        {
            return nbrightInfo;
        }

        public override NBrightInfo AfterPaymentFail(NBrightInfo nbrightInfo)
        {
            return nbrightInfo;
        }

        public override NBrightInfo BeforeSendEmail(NBrightInfo nbrightInfo, string emailsubjectrexkey)
        {
            return nbrightInfo;
        }

        public override NBrightInfo AfterSendEmail(NBrightInfo nbrightInfo, string emailsubjectrexkey)
        {
            return nbrightInfo;
        }


    }

}
