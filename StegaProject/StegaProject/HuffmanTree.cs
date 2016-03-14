using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanTreeTestingProject
{
    class HuffmanTree {
        static LinkedList<LinkedList<string>> DHTLists;

        public string value { get; protected set; }
        public string addr { get; protected set; }
        public int lvl { get; protected set; }
        public bool leaf { get; protected set; }

        public HuffmanTree left;
        public HuffmanTree right;

        private string[] _DHT;

        public HuffmanTree(string DHT) : this("", DHT) {

        }

        public HuffmanTree(string binaddr, string DHT) {
            this.lvl = binaddr.Length;
            this.addr = binaddr;
            this._DHT = DHT.Split(' ');

            if (this.lvl == 0) {
                this.populateLists(DHT);
            }

            if (!makeMeLeaf() && this.lvl < 16) {
                this.left = new HuffmanTree(binaddr + "0", DHT);
                this.right = new HuffmanTree(binaddr + "1", DHT);
            }
        }

        public String SearchFor(String binAddr) {
            if (this.leaf) {
                if (this.addr == binAddr) {
                    return this.value;
                } else {
                    return "";
                }
            } else {
                String result;
                if (this.left == null) {
                    result = "";
                } else {
                    result = this.left.SearchFor(binAddr);
                }
                if (this.right == null) {
                    result += "";
                } else {
                    result += this.right.SearchFor(binAddr);
                }
                return result;
            }

        }

        public void printAddresses() {
            if (this.leaf) {
                Console.WriteLine("{0} - {1}", this.addr, this.value);
            }

            if (this.left != null) {
                this.left.printAddresses();
            }
            if (this.right != null) {
                this.right.printAddresses();
            }

        }

        public void populateLists(string DHT) {
            HuffmanTree.DHTLists = new LinkedList<LinkedList<string>> { };
            String[] dhtsplit = DHT.Split(' ');
            int valueIndex = 17;
            for (int i = 0; i < 17; i++) {

                int dhtamount;
                int.TryParse(dhtsplit[i], out dhtamount);
                LinkedList<string> valuesList = new LinkedList<string> { };
                for (int d = valueIndex; d < valueIndex + dhtamount; d++) {
                    valuesList.AddLast(dhtsplit[d]);
                }
                valueIndex += dhtamount;
                HuffmanTree.DHTLists.AddLast(valuesList);
            }

        }

        public LinkedList<string> levelList() {
            return HuffmanTree.DHTLists.ElementAt(this.lvl);
        }



        public bool makeMeLeaf() {
            if (this.lvl - 1 >= 0 && this.levelList().Count() > 0) {

                this.value = this.levelList().First.Value;
                this.leaf = true;
                HuffmanTree.DHTLists.ElementAt(this.lvl).RemoveFirst();
                return true;
            } else {
                return false;
            }
        }

    }
}
