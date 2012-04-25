using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CollectionUtils.Extenders;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IList<int> l = new List<int>();
            l.Add(42);
            IList<object> l2 = l.CastList<object, int>();
            l2.Remove(42);
            Console.ReadLine();
        }
    }
}
