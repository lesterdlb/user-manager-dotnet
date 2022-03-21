using Microsoft.AspNetCore.Mvc;

namespace UserManager.Helpers
{
    public static class NavigationIndicatorHelper
    {
        public static string MakeActiveClass(this IUrlHelper urlHelper, string controller, string action)
        {
            try
            {
                var className = "active";

                var controllerName = urlHelper.ActionContext.RouteData.Values["controller"]?.ToString();
                var actionName = urlHelper.ActionContext.RouteData.Values["action"]?.ToString();

                if (string.IsNullOrEmpty(controllerName)) return string.Empty;

                if (controllerName.Equals(controller, StringComparison.OrdinalIgnoreCase))
                {
                    if (actionName is not null && actionName.Equals(action, StringComparison.OrdinalIgnoreCase))
                    {
                        return className;
                    }
                }
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
