﻿namespace AppWord.Panel.API.Extensions
{
    public class LanguageRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {

            if (!values.ContainsKey("culture"))
                return false;

            var culture = values["culture"].ToString();
            return culture == "en" || culture == "tr";
        }
    }
}
