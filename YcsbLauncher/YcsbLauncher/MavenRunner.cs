using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace YcsbLauncher
{
    public class MavenRunner
    {
        public void Run(String workload, String DbBindingClass = "com.yahoo.ycsb.db.RavenDBClient", String DbWorkingDir = "C:/work/YCSB/ravenDb-binding",String outputPath = @"c:\work\Tests\Ycsb\")
        {
            var process = new Process();
            // Configure the process using the StartInfo properties.
            process.StartInfo.FileName = "mvn.bat";
            process.StartInfo.Arguments = string.Format("exec:java -Dexec.mainClass=com.yahoo.ycsb.Client \"-Dexec.args=-db {0} -P {1} -s -t\"",DbBindingClass, workload);
            process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            process.StartInfo.WorkingDirectory = DbWorkingDir;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            WorkloadResultParser.Parse(process.StandardOutput, outputPath,workload);            
            process.WaitForExit();// Waits here for the process to exit.
        }
    }
}
