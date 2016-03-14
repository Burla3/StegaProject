using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegaProject {
    class Steganogrify {

        public void changeLSB(List<EntropyComponent> entropyComponents) {
            foreach (EntropyComponent entropyComponent in entropyComponents) {
                entropyComponent.LSB = 0;
            }
        }
    }
}
