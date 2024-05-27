using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.models
{
    /// <summary>
    /// model 2
    /// </summary>
    public class SplitProfitAndRate
    {
        public List<int>? SplitProfit { get; set; }
        public int RateMaxProfit { get; set; }
        public SplitProfitAndRate(List<int>? splitProfit, int rateMaxProfit)
        {
            SplitProfit = splitProfit;
            RateMaxProfit = rateMaxProfit;
        }
        public SplitProfitAndRate() { }
    }
}
