using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YcsbLauncher
{
    public enum RequestDistribution
    {
        exponential,
        uniform,
        zipfian,
        latest,
        hotspot
    }

    public enum InsertOrder
    {
        ordered,
        hashed
    }
    public class YcsbWorkloadConfiguration
    {
        public long RecordCount { get { return _recordCount; } set { if (value > 0) _recordCount = value; } }
        public long OperationCount { get { return _operationCount; } set { if (value > 0) _operationCount = value; } }
        public bool ReadAllFields { get { return _readAllFields; } set { _readAllFields = value; } }

        public double ReadPrOperation { get { return _readprOportion; } }
        public double UpdatePrOperation { get { return _updateprOportion; } }
        public double ScanPrOperation { get { return _scanprOportion; } }
        public double InsertPrOperation { get { return _insertprOportion; } }

        public RequestDistribution RequestDistribution { get { return _requestDistribution; } set { _requestDistribution = value;} }

        public InsertOrder InsertOrder { get { return _insertOrder; } set { _insertOrder = value; } }

        public void SetRequestOperationProbability(double read, double update, double scan, double insert)
        {
            var normalizer = read + update + scan + insert;
            _readprOportion = read/normalizer;
            _updateprOportion = update/normalizer;
            _scanprOportion = scan/normalizer;
            _insertprOportion = insert/normalizer;
        }

        public string WriteToFile(string outputDir, string fileName = null)
        {
            var fileFullPath = Path.Combine(outputDir, fileName ?? BuildFileName());
            if (File.Exists(fileFullPath)) return fileFullPath;
            return WriteToFileCore(fileFullPath);
        }

        private string WriteToFileCore(string fileFullPath)
        {
            var fi = new FileInfo(fileFullPath);
            var writer = fi.CreateText();
            writer.WriteLine(String.Format("{0}={1}", RecordCountToken,RecordCount));
            writer.WriteLine(String.Format("{0}={1}",OperationCountToken,OperationCount));
            writer.WriteLine(String.Format("{0}={1}",WorkloadToken,Workload));
            writer.WriteLine(String.Format("{0}={1}",ReadallfieldsToken,ReadAllFields));
            writer.WriteLine(String.Format("{0}={1}",ReadOperationToken,ReadPrOperation));
            writer.WriteLine(String.Format("{0}={1}",UpdateOperationToken,UpdatePrOperation));
            writer.WriteLine(String.Format("{0}={1}",ScanOperationToken,ScanPrOperation));
            writer.WriteLine(String.Format("{0}={1}",InsertOperationToken,InsertPrOperation));
            writer.Write(String.Format("{0}=",HostsToken));
            for (int i = 0; i < _hosts.Length - 1; ++i)
            {
                writer.Write(String.Format("{0},",_hosts[i]));
            }
            writer.WriteLine(_hosts[_hosts.Length-1]);
            writer.WriteLine(String.Format("{0}={1}",RequestDistributionToken,RequestDistribution));
            writer.WriteLine(String.Format("{0}={1}",InsertOrderToken,InsertOrder));
            writer.Close();
            return fileFullPath;
        }

        private string BuildFileName()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Workload_");
            sb.Append(RecordCount);
            sb.Append("_");
            sb.Append(OperationCount);
            sb.Append(ReadAllFields ? "_T_" : "_F_");
            sb.Append("_R");
            sb.Append((int) _readprOportion * 10);
            sb.Append("_U");
            sb.Append((int)_updateprOportion * 10);
            sb.Append("_S");
            sb.Append((int)_scanprOportion * 10);
            sb.Append("_I");
            sb.Append((int)_insertprOportion * 10);
            sb.Append("_");
            sb.Append(_insertOrder);
            return sb.ToString();
        }

        private readonly String Workload = "com.yahoo.ycsb.workloads.CoreWorkload";
        private readonly String WorkloadToken = "workload";
        private readonly String RecordCountToken = "recordcount";
        private readonly String OperationCountToken = "operationcount";
        private readonly String ReadallfieldsToken = "readallfields";
        private readonly String ReadOperationToken = "readproportion";
        private readonly String UpdateOperationToken = "updateproportion";
        private readonly String ScanOperationToken = "scanproportion";
        private readonly String InsertOperationToken = "insertproportion";
        private readonly String HostsToken = "hosts";
        private readonly String RequestDistributionToken = "requestdistribution";
        private readonly String InsertOrderToken = "insertorder";

        long _recordCount = 1000;
        private long _operationCount = 1000;
        private bool _readAllFields = true;
        private double _readprOportion = 0.5;
        private double _updateprOportion = 0.5;
        private double _scanprOportion = 0;
        private double _insertprOportion = 0;
        private String[] _hosts = new[] {"127.0.0.1"};
        private RequestDistribution _requestDistribution = RequestDistribution.zipfian;
        private InsertOrder _insertOrder = InsertOrder.ordered;

    }
}
