using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CM.Core
{
    public class EnumCodeAttribute : Attribute
    {
        public string Code { get; protected set; }

        public EnumCodeAttribute(string value)
        {
            this.Code = value;
        }
    }
}
