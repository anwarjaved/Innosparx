namespace Framework.Web.Mvc
{
    using System.Security;
    using System.Web.Mvc;

    
    public class ImportModelStateFromTempData : ModelStateTempDataTransfer
    {
        
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ModelStateDictionary modelState = filterContext.Controller.TempData[Key] as ModelStateDictionary;

            if (modelState != null)
            {
                //Only Import if we are viewing
                if (filterContext.Result is ViewResult)
                {
                    filterContext.Controller.ViewData.ModelState.Merge(modelState);
                }
                else
                {
                    //Otherwise remove it.
                    filterContext.Controller.TempData.Remove(Key);
                }
            }

            base.OnActionExecuted(filterContext);
        }
    }
}
