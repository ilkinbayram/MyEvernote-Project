using MyEvernote.DataAccessLayer.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    // SINGLETON PATTERN
    public class RepositoryBaseSingletonContext
    {
        protected static MyEvernoteDBContext context;   // Protected verilen deyisen miras alan terefinden kullanila bilir
        private static object _lockSync = new object();   // Lock mekanizmasinin calisabilmesi icin object turunden herhangi nesne ister

        public RepositoryBaseSingletonContext()    // New'lendiyi zaman icerigindekiler olusturulmus olur
        {
            CreateDb();
        }
        private static MyEvernoteDBContext CreateDb()
        {

            if (context == null)
            {
                lock (_lockSync)
                {
                    if (context == null)
                    {
                        context = new MyEvernoteDBContext();
                    }
                }
            }
            return context;
        }
    }
}
