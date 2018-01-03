using System.Web.Mvc;

namespace XBT.Presentation.Areas.DigitalArchive
{
    public class DigitalArchiveAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DigitalArchive";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "DigitalArchive_default",
                "DigitalArchive/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
