using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.APIRespones
{
    public class ResponeApi<T>
    {
        public ResponeApi(T data)
        {
            Data = data;
        }
        public string Message { get; set; }
        public int Status { get; set; }
        public T Data { get; set; }
        public MetaData Meta { get; set; }
    }
}