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
        public string Symbol { get; set; }
        public WhereField() { }
        public WhereField(string key, object value, string symbol = "=")
        {
            this.Key = key;
            this.Value = value;
            this.Symbol = symbol;
        }
    }
}
