using Framework.Models;

namespace Framework.Web.Api
{
    using System;
    using System.Collections.Specialized;
    using System.Security;
    using System.Web;
    using System.Web.Http.Controllers;
    using System.Web.Http.ModelBinding;

    
    public class DataTableParamModelBinder : IModelBinder
    {
        
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType == typeof(DataTableParamModel))
            {
                DataTableParamModel paramModel = new DataTableParamModel();

                paramModel.PageSize = bindingContext.GetValueOrDefault<int>("iDisplayLength");
                paramModel.Echo = bindingContext.GetValueOrDefault("sEcho");
                paramModel.SearchText = bindingContext.GetValueOrDefault("sSearch");
                paramModel.DisplayStart = bindingContext.GetValueOrDefault<int>("iDisplayStart");
                paramModel.NoofColumns = bindingContext.GetValueOrDefault<int>("iColumns");
                int sortColumn = bindingContext.GetValueOrDefault<int>("iSortCol_0");
                paramModel.Columns = bindingContext.GetValueOrDefault("sColumns").Split(new[] { "," }, StringSplitOptions.None);
                paramModel.SortColumn = paramModel.Columns[sortColumn];
                paramModel.SortDirection = bindingContext.GetValueOrDefault("sSortDir_0");
                paramModel.PageNumber = paramModel.DisplayStart / paramModel.PageSize;

                bindingContext.Model = paramModel;

                return true;
            }

            return false;
        }
    }
}
