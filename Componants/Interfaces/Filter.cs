﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DotNetNuke.Entities.Portals;
using NBrightDNN;
using Nevoweb.DNN.NBrightBuy.Components;

namespace OpenStore.Providers.OS_PurchaseNotification
{
    public class Filter : Nevoweb.DNN.NBrightBuy.Components.Interfaces.FilterInterface
    {
        public override string GetFilter(string currentFilter, NavigationData navigationData, ModSettings setting, NBrightInfo ajaxInfo)
        {
            throw new NotImplementedException();
        }

    }
}
