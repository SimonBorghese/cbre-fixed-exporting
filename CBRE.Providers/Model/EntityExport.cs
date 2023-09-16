using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBRE.Providers.Model {
    internal class EntityExport {
        public string ClassName { get; set; }
        public string Origin { get; set; }

        public Dictionary<string, string> Flags { get; set; }
    }
}
