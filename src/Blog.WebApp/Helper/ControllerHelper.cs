using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.WebApp.Helper
{
    public static class ControllerHelper
    {
        public static string ControllerName(this Type type)
        {
            var name = type.Name;
            return name.EndsWith("Controller") ? name.Substring(0, name.Length - 10) : name;
        }
    }
}
