using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegaProject {
    class LSBComponent {
        public int IndexInEntropyComponents { get; private set; }
        public int LSB { get; set; }

        public LSBComponent(int lsb, int index) {
            IndexInEntropyComponents = index;
            LSB = lsb;
        }
    }
}
