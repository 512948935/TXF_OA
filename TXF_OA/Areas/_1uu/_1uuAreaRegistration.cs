using System.Web.Mvc;

namespace TXF_MVC.Areas._1uu
{
    public class _1uuAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "_1uu";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "_1uu_default",
                "_1uu/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
