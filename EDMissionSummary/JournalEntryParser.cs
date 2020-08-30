using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EDMissionSummary
{
    public class JournalEntryParser
    {
        public JournalEntryParser()
        {

        }

        public JObject Parse(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                throw new ArgumentException("Cannot be null or empty", nameof(line));
            }

            return JObject.Parse(line);
        }
    }
}
