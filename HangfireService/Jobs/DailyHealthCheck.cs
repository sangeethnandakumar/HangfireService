using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangfireService.Jobs
{
    public class DailyHealthCheck
    {
        public void Run()
        {
            Console.WriteLine("Running daily health checks...");
        }
    }
}