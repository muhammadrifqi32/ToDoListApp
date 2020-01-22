using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ConnectionString
{
    public class ConnectionStrings
    {
        public ConnectionStrings(string value) => Value = value; //to get value

        public string Value { get; }
    }
}
