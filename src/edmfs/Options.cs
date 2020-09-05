using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace edmfs
{
    public class Options
    {
        public Options()
        {
            Date = DateTime.Today;
        }

        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.", Default = false)]
        public bool Verbose { get; set; }

        [Option('d', "date", Required = false, HelpText = "Show minor faction support on this date. Defaults to today if omitted.")]
        public DateTime Date { get; set; }

        [Option('f', "minor-faction", Required = false, HelpText = "The minor faction to support. This must match the name in the journal exactly, including case.", Default = "EDA Kunti League")]
        public string MinorFaction { get; set; }
    }
}
