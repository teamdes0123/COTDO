using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COTDO.Models
{
    public class Response
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public object Data { get; set; }
    }
}