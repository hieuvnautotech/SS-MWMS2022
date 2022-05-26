using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.APIRequest
{
    public class RequestAPI<T>
    {
        public RequestAPI(T data)
        {
            Data = data;
        }
        public T Data;
        public Pageing paging { get; set; }  
    }
}