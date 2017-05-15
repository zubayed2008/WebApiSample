using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeServices.ModelView
{
    public class Item
    {
        public enum grade
        {
            A = 1,
            B, 
            C,
            D
        }

        public grade gradetype { get; set; }
    }
}