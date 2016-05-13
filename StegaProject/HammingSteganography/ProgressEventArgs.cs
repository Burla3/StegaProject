using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HammingSteganography {
    public class ProgressEventArgs : EventArgs {
        public int Progress { get; }
        public ProgressEventArgs(int progress) {
            Progress = progress;
        }
    }
}
