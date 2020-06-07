using MyEvernote.DataAccessLayer.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BussinesLayer
{
    public class Test
    {
        public Test()
        {
            MyEvernoteDBContext dbc = new MyEvernoteDBContext();
            dbc.Categories.ToList();
        }
    }
}
