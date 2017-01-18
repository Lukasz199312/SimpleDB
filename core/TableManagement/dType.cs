using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDB.core.TableManagement
{
    public enum DataType
    {
        NUMBER,
        TEXT_8 = 8,
        TEXT_16 = 16,
        TEXT_32 = 32,
        TEXT_64 = 64,
        TEXT_128 = 128,
        TEXT_STRING
    };

    public enum DataOptions
    {
        REQUIRE,
        PRIMARY_KEY,
        AUTO_INCREMENT,
        FOREIGN_KEY
    };
}
