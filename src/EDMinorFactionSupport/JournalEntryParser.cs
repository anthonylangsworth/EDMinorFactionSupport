﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EDMinorFactionSupport
{
    public class JournalEntryParser
    {
        public JournalEntryParser()
        {
            // Do nothing
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
