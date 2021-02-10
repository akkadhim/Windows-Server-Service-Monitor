using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SystemMoniter.Models
{
    public class AdminPanelGInfoViewModel
    {

        [System.ComponentModel.DefaultValue(0)]
        public float TotalPhyemory { get; set; }
        public long MemoryUsedPer { get; set; }
        public long MemoryUsage { get; set; }

        public double CPuLoad { get; set; }
        public double systemUptime { get; set; }
        public int ProcessCount { get; set; }
        public int ThreadsCount { get; set; }
        public Int64 HandlesCount { get; set; }

        public long HDDTotalSpace { get; set; }
        public long HDDUsage { get; set; }
        public long HDDUsedPer { get; set; }


    }
}
