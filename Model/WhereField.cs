using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class WhereField
    {
        public string Key { get; set; }
        public object Value { get; set; }
        private string symbol = "=";

        public string Symbol
        {
            get { return symbol; }
            set { symbol = value; }
        }
    }
}
