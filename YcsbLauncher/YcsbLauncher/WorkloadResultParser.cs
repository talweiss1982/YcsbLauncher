using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YcsbLauncher
{
    public class WorkloadResultParser
    {
        static public void Parse(StreamReader reader, String outputDir, String WorkloadPath)
        {
            var resFileName = Path.Combine(outputDir,Path.GetFileName(WorkloadPath)+"_res.txt");
            int index = 1;
            while (File.Exists(resFileName))
            {
                resFileName = Path.Combine(outputDir, Path.GetFileName(WorkloadPath) + string.Format("_res({0}).txt",index++));
            }
            var fi = new FileInfo(resFileName);
            var writer = fi.CreateText();
            string line = reader.ReadLine();
            var reportDictionary = new Dictionary<string, Dictionary<string, string>>();
            while (line != null)
            {                
                var tokens = line.Split(CsvSpliter,StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length < 3)
                {
                    line = reader.ReadLine();
                    continue;
                }
                if (tokens[0].Equals(OverallToken))
                {
                    if (!reportDictionary.ContainsKey(tokens[0])) reportDictionary[tokens[0]] = new Dictionary<string, string>();
                    reportDictionary[tokens[0]][tokens[1].Trim()] = tokens[2].Trim();
                }
                if (tokens[1].Trim().Equals(OperationToken))
                {
                    if (!reportDictionary.ContainsKey(tokens[0])) reportDictionary[tokens[0]] = new Dictionary<string, string>();
                    reportDictionary[tokens[0]][tokens[1].Trim()] = tokens[2].Trim();
                }
                if (tokens[1].Trim().Equals(AverageLatencyToken))
                {
                    if (!reportDictionary.ContainsKey(tokens[0])) reportDictionary[tokens[0]] = new Dictionary<string, string>();
                    reportDictionary[tokens[0]][tokens[1].Trim()] = tokens[2].Trim();
                }
                if (tokens[1].Trim().Equals(MinLatencyToken))
                {
                    if (!reportDictionary.ContainsKey(tokens[0])) reportDictionary[tokens[0]] = new Dictionary<string, string>();
                    reportDictionary[tokens[0]][tokens[1].Trim()] = tokens[2].Trim();
                }
                if (tokens[1].Trim().Equals(MaxLatencyToken))
                {
                    if (!reportDictionary.ContainsKey(tokens[0])) reportDictionary[tokens[0]] = new Dictionary<string, string>();
                    reportDictionary[tokens[0]][tokens[1].Trim()] = tokens[2].Trim();
                }
                line = reader.ReadLine();
            }
            writer.WriteLine(String.Format("Overall run time(ms): {0}, throughput(ops/sec): {1}", reportDictionary[OverallToken][RuntimeToken], reportDictionary[OverallToken][ThroughputToken]));
            foreach (var key in reportDictionary.Keys)
            {
                if (key.Equals(OverallToken) || key.Equals(CleanupToken)) continue;                
                var latency = double.Parse(reportDictionary[key][AverageLatencyToken]);
                writer.WriteLine(String.Format("{0}\n Operations: {1}, throughput(ops/sec): {2}", key, reportDictionary[key][OperationToken],
                    1000000 / latency));
            }
            writer.Close();
        }
        private static readonly String CleanupToken ="[CLEANUP]";
        private static readonly String RuntimeToken = "RunTime(ms)";
        private static readonly String ThroughputToken = "Throughput(ops/sec)";
        private static readonly String OverallToken = "[OVERALL]";
        private static readonly String OperationToken = "Operations";
        private static readonly String AverageLatencyToken = "AverageLatency(us)";
        private static readonly String MinLatencyToken = "MinLatency(us)";
        private static readonly String MaxLatencyToken = "MaxLatency(us)";
        private static char[] CsvSpliter = new[] {','};
    }
}
