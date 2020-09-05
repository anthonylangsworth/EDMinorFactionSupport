using EDMinorFactionSupport.SummaryEntries;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EDMinorFactionSupport.OutputFormatters
{
    public class StandardOutputFormatter : OutputFormatter
    {
        public override void Format(IEnumerable<SummaryEntry> summary, TextWriter output)
        {
            if (summary is null)
            {
                throw new ArgumentNullException(nameof(summary));
            }
            if (output is null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            IndentedTextWriter indentedTextWriter = new IndentedTextWriter(output, "  ");

            foreach (IGrouping<string, SummaryEntry> group in summary.GroupBy(se => se.SystemName))
            {
                indentedTextWriter.WriteLine(group.Key);
                indentedTextWriter.Indent++;
                DisplayByFactionSupport(group, indentedTextWriter);
                indentedTextWriter.Indent--;
            }
        }

        protected void DisplayByFactionSupport(IEnumerable<SummaryEntry> summary, IndentedTextWriter indentedTextWriter)
        {
            foreach (IGrouping<bool, SummaryEntry> group in summary.GroupBy(se => se.IncreasesInfluence))
            {
                indentedTextWriter.WriteLine(group.Key ? "PRO" : "CON");
                indentedTextWriter.Indent++;
                DisplayMissions(group.Where(se => se is MissionSummaryEntry).Cast<MissionSummaryEntry>(), indentedTextWriter);
                indentedTextWriter.WriteLine();
                DisplayVouchers(group.Where(se => se is RedeemVoucherSummaryEntry).Cast<RedeemVoucherSummaryEntry>(), indentedTextWriter);
                indentedTextWriter.Indent--;
            }
        }

        protected void DisplayMissions(IEnumerable<MissionSummaryEntry> missionSummaryEntries, IndentedTextWriter indentedTextWriter)
        {
            foreach (IGrouping<string, MissionSummaryEntry> group in missionSummaryEntries.GroupBy(mse => mse.Influence))
            {
                indentedTextWriter.WriteLine("{0} Inf{1}", group.Count(), group.Key);
            }
        }

        protected void DisplayVouchers(IEnumerable<RedeemVoucherSummaryEntry> redeemVoucherSummaryEntries, IndentedTextWriter indentedTextWriter)
        {
            foreach (IGrouping<string, RedeemVoucherSummaryEntry> group in redeemVoucherSummaryEntries.GroupBy(mse => mse.VoucherType))
            {
                indentedTextWriter.WriteLine("{0} redemption for {1:n0} CR", SplitOnUpper(group.Key), group.Sum(rvse => rvse.Amount));
            }
        }

        static string SplitOnUpper(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentException($"'{nameof(s)}' cannot be null or empty", nameof(s));
            }

            StringBuilder stringBuilder = new StringBuilder();
            s.Take(1).Aggregate(stringBuilder, (sb, c) => sb.Append(char.ToUpper(c)));
            s.Skip(1).Aggregate(stringBuilder, (sb, c) => sb.Append(char.IsUpper(c) ? $" { c }" : $"{ c }"));
            return stringBuilder.ToString();
        }
    }
}
