using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace Dao.DataProvider
{
    internal class TransConnection
    {
        public TransConnection()
        {
            this.Deeps = 0;
        }

        public DbTransaction DBTransaction { get; set; }

        public int Deeps { get; set; }
    }
}
