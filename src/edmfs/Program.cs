using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EDMinorFactionSupport;
using EDMinorFactionSupport.JournalEntryProcessors;
using EDMinorFactionSupport.SummaryEntries;
using EDMinorFactionSupport.JournalSources;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.CodeDom.Compiler;
using EDMinorFactionSupport.OutputFormatters;
using CommandLine;
using System.Globalization;
using CommandLine.Text;

namespace edmfs
{
    class Program
    {
        static void Main(string[] args)
        {
            //try
            //{

            Parser parser = new Parser(ps => ps.ParsingCulture = CultureInfo.CurrentCulture);
            ParserResult<Options> parserResult = parser.ParseArguments<Options>(args);
            parserResult
                  .WithParsed(options =>
                    {
                        Pipeline pipeline = new ServiceCollection()
                            .AddEDMinorFactionSupport(options.Date, "EDA Kunti League", options.Verbose)
                            .BuildServiceProvider()
                            .GetService<Pipeline>();
                        pipeline.Run(Console.Out);
                    })
                  .WithNotParsed(errors => Console.Error.Write(HelpText.AutoBuild(parserResult)));

            //}
            //catch(Exception ex)
            //{
            //    Console.Error.Write(ex.ToString());
            //}
        }
    }
}
