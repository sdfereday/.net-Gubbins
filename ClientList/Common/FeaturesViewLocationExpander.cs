using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientList.Common.Mvc
{
    /// <summary>
    /// Custom view locator, view in feature folder
    /// </summary>
    public class FeaturesViewLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context)
        {
            context.Values["customviewlocation"] = nameof(FeaturesViewLocationExpander);
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            var viewLocationFormats = new[]
            {
                // Root/ActionParentDir/View.cshtml
                "~/Shared/{0}.cshtml",
                "~/Features/{1}/{0}.cshtml"
            };

            return viewLocationFormats;
        }
    }
}