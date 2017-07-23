using System;
using System.Linq;
using System.Web.Mvc;

namespace KakashiService.Web.Utilities
{
    public static class ModelStateExtension
    {
        public static object ErrorModelStateToJSON(this ModelStateDictionary modelState)
        {
            var keys = from modelstate in modelState.AsQueryable().Where(f => f.Value.Errors.Count > 0)
                       select modelstate.Key;
            var listErrors = from modelstate in modelState.AsQueryable().Where(f => f.Value.Errors.Count > 0)
                             select modelstate.Value.Errors.FirstOrDefault(a => !String.IsNullOrEmpty(a.ErrorMessage));

            var result = new { success = false, keys = keys.ToList(), listErrors = listErrors.Where(a => a != null).Select(a => a.ErrorMessage).ToList() };

            return result;
        }
    }
}