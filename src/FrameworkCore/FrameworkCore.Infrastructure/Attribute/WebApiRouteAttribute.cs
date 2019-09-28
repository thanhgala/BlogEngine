using System;

namespace FrameworkCore.Infrastructure.Attribute
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class WebApiRouteAttribute : System.Attribute
    {
        #region Constructors and Destructors

        public string[] ParamList;

        public string Route { get; set; }

        public WebApiRouteAttribute(string route) => Route = route;

        #endregion

        #region Public Properties


        #endregion

        #region Public Methods and Operators


        #endregion
    }
}
