using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    [Serializable]
    public class Pointer
    {
        public string word;
        public List<int> pages;
        public Pointer()
        { }

        public Pointer(string word, List<int> pages)
        {
            this.word = word;
            if (pages.Count > 0 && pages.Count <= 10)
                this.pages = pages;
            else
                throw new IndexOutOfRangeException();
        }

        public void Return_fields(out string word, out List<int> pages)
        {
            word = this.word;
            pages = this.pages;
        }
        
    }
}
