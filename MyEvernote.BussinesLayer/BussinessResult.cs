using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BussinesLayer
{
    public class BussinessResult<T> where T:class, new()
    {
        public List<BussinessError> Errors { get; set; }
        public List<BussinessError> Successes { get; set; }
        public T Result { get; set; }
        public bool IsAdmin { get; set; }

        public BussinessResult()
        {
            Errors = new List<BussinessError>();
            Successes = new List<BussinessError>();
        }
    }

}
