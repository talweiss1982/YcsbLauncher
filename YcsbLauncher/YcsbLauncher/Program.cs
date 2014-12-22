using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YcsbLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            var runner = new MavenRunner();
            var workloadConfig = new YcsbWorkloadConfiguration();
            var ycsbConfig = new YcsbConfiguration("YcsbConfig.xml");
            var workloadPath = workloadConfig.WriteToFile(ycsbConfig.WorkloadDir);            
            runner.Run(workloadPath, ycsbConfig.DbBindingClass, ycsbConfig.YcsbWorkingDirectory, ycsbConfig.ResultsDirectory);
        }
    }
}
