using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegaProject {
    class Steganogrify {

        public void changeLSB(List<EntropyComponent> entropyComponents) {
            foreach (EntropyComponent entropyComponent in entropyComponents) {
                change(entropyComponent);
            }
        }

        public void change(EntropyComponent entropyComponent) {
            string temp = entropyComponent.Amplitude;
            char MSB = temp[0];
            if (MSB == '1') {
                temp = "0" + temp.Remove(0, 1);
            }
            else {
                temp = "1" + temp.Remove(0, 1);
            }

            entropyComponent.Amplitude = temp;
        }
    }
}
