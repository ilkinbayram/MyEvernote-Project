using MyEvernote.BussinesLayer.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BussinesLayer
{
    public class BussinessProcessedType<T> where T : class, new()
    {
        public T Entity { get; set; }
        public int ProcessedCount { get; set; }
        public InformingOrError Status { get; set; }
    }
}
