using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangfireService.Jobs
{
    public class ServiceStartupNotifier
    {
        public void Run()
        {
            Console.WriteLine("Service started...");
        }
    }
}