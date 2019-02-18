using System;
using System.Linq;

namespace WebEas.ServiceModel.Types
{
    public class DbEnvironment
    {
        public string Environment { get; set; }

        public DateTime? DeployTime { get; set; }
    }
}