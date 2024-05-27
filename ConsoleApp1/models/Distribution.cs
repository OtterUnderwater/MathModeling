using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.models
{
    /// <summary>
    /// model 1
    /// </summary>
    public class Distribution
    {
        public int KeyRate { get; set; }
        public List<(int, int)>? SplitRate { get; set; }
        public Distribution(int keyRate, List<(int, int)>? splitRate)
        {
            KeyRate = keyRate;
            SplitRate = splitRate;
        }
        public Distribution() { }
    }
}
