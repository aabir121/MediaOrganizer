using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaOrganizer
{
    internal class RunConfig
    {
        public string SourceDirectory { get; set; } = "";
        public bool RenameSimilar { get; set; }
        public bool RemoveEmptyDirectory { get; set; }
    }
}
