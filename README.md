# ED Mission Summary

Creates a summary of pro- and anti-minor faction work by players for Elite:Dangerous.

Requires .Net Core installed.

Instructions:
1. Compile the executable.
2. Run the executable after a day of play. It will scan the gane's journal files and output to the console window a summary.

I recommend piping the results to the Windows clipboard by using "clip", e.g. "edmissionsummary | clip". You can then paste the result in Discord easily.

What does it support?
1. Missions. It correctly detects missions that improve "EDA Kunti League" influence. However, it flags any mission that does not improving that minor faction's incluce as against that faction. This fix is planned but not simple.
2. Bounties. It ignores bounties handed into non "EDA Kunti League Factions".

Important things to fix:
1. A better display (e.g. counting up the number of influence missions instead of listing them separately)
2. Fixing anti-minor faction mission detection.
3. Cartography data.
4. Trades.
5. Internal fixes like a better folder structure and automated tests.

If this tool proves useful we may automate the collation of these further, e.g. by pasting the results to Discord directly or a similar tighter integration.

# License
 
(TODO, probably MIT or similar)