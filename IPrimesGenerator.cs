using System.Collections.Generic;

namespace Demo
{
    interface IPrimesGenerator
    {
        IEnumerable<int> GetAll();
    }
}
