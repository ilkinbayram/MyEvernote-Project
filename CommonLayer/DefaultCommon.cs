using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.CommonLayer
{
    public class DefaultCommon : ICommon
    {
        public static string UserName { get; set; }

        public DefaultCommon()
        {
            UserName = "System";
        }
        public string GetCurrentUsername()
        {
            return "System";
        }
    }
}
