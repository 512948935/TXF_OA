using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class UpdateField
    {
        public string Key { get; set; }
        public object Value { get; set; }
        public UpdateField(string key,object value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
}
