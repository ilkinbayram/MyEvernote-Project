using MyEvernote.BussinesLayer.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BussinesLayer
{
    public class BussinessError
    {
        public string Subject { get; set; }
        public InformingOrError ErrorCode { get; set; }
        public string Detail { get; set; }
        public string AlertColor { get; set; }
    }
}
