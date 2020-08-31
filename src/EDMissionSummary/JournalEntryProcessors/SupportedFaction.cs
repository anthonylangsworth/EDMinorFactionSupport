using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDMissionSummary.JournalEntryProcessors
{
    public class SupportedFaction
    {
        public SupportedFaction(string name, IEnumerable<string> systemNames, IEnumerable<string> systemIds)
        {
            Name = name;
            SystemNames = systemNames.ToList();
            SystemIds = systemIds.ToList();
        }

        public string Name { get; }
        public IEnumerable<string> SystemNames { get; }
        public IEnumerable<string> SystemIds { get; }
    }
}
