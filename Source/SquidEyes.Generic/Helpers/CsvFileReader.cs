using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using System.IO;
using System.Collections;

namespace SquidEyes.Generic
{
    public class CsvFileReader: IEnumerable<string[]>
    {
        private StreamReader reader;

        public CsvFileReader(string fileName)
        {
            reader = new StreamReader(fileName);
        }

        public IEnumerator<string[]> GetEnumerator()
        {
            string line;
            
            while ((line = reader.ReadLine()) != null)
                yield return line.Split(',');

            yield return null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
