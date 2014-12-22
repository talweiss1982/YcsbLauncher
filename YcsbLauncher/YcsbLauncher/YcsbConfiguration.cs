using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace YcsbLauncher
{
    public class YcsbConfiguration
    {
        public String WorkloadDir { get; set; }
        public String DbBindingClass { get; set; }
        public String YcsbWorkingDirectory { get; set; }
        public String ResultsDirectory { get; set; }

        public YcsbConfiguration()
        {
        }

        public YcsbConfiguration(String xmlConfiguration)
        {
            if (!File.Exists(xmlConfiguration)) throw new FileNotFoundException(String.Format("Could not find file:{0}",xmlConfiguration));
            XDocument doc = XDocument.Load(xmlConfiguration);
            var root = doc.Element("YcsbConfiguration");
            var workDir = root.Element("WorkloadDir");
            WorkloadDir = workDir.Value;
            var binding = root.Element("DbBindingClass");
            DbBindingClass = binding.Value;
            var workdir = root.Element("YcsbWorkingDirectory");
            YcsbWorkingDirectory = workdir.Value;
            var resdir = root.Element("ResultsDirectory");
            ResultsDirectory = resdir.Value;
        }
    }
}
