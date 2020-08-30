using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EDMissionSummary
{
    public class Journal
    {
        public Journal(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("Journal file not found", fileName);
            }

            FileName = fileName;
        }

        public string FileName
        {
            get;
        }

        public string[] Entries
        {
            get
            {
                return File.ReadAllLines(FileName);
            }
        }
    }
}
