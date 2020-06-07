using MyEvernote.BussinesLayer.Managers;
using MyEvernote.EntitiesLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace MyEvernote.Web.Models
{
    public class CacheHelper
    {
        public static List<Category> GetCategoryListFromCache(CacheHelperKeys cacheKey = CacheHelperKeys.constCategoryList)
        {
            List<Category> result = WebCache.Get(cacheKey.ToString()) as List<Category>;

            if (result == null)
            {
                CategoryManager categoryManager = new CategoryManager();
                result = categoryManager.List(x => x.IsDeleted == false);
                WebCache.Set(cacheKey.ToString(), result, 20, true);
            }

            return result;
        }

        public static void RemoveCache(CacheHelperKeys cacheKey = CacheHelperKeys.constCategoryList)
        {
            WebCache.Remove(cacheKey.ToString());
        }
    }


    public enum CacheHelperKeys
    {
        constCategoryList = 0
    }
}