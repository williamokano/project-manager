using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Katapoka.DAO
{
    public class JsonResponse
    {
        public int Status { get; set; }
        public object Data { get; set; }

        public JsonResponse()
        {
            this.Status = 999;
            this.Data = "Nothing happened.";
        }

        public JsonResponse(int status, object data)
        {
            this.Status = status;
            this.Data = data;
        }

    }
}
