using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTSample.AuxClass
{
    public class ServiceResult<T>
    {
        int resultCode;
        string resultExplanation;
        public int ResultCode { get => resultCode; set => resultCode = value; }
        public string ResultExplanation { get => resultExplanation; set => resultExplanation = value; }
        public T ResultData { get; set; }// isSuccessfull true ise
        public bool isSuccessfull { get; set; }
    }
}
