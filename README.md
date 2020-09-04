# ED Mission Summary

Creates a summary of pro- and anti-minor faction work by players for Elite:Dangerous used by the "Elite Dangerous AU & NZ" squadron.

Requires .Net Core installed.

Instructions:
1. Compile the executable.
2. Run the executable after a day of play. It will scan the gane's journal files and output to the console window a summary.

I recommend piping the results to the Windows clipboard by using the built-in "clip" command, e.g. "edmissionsummary | clip". You can then paste the result in Discord easily (although it requires some tweaking still).

What does it support?
1. Missions. It correctly detects missions that improve "EDA Kunti League" influence. However, it flags any mission that does not improve that minor faction's incluce as against that faction. This fix is planned but not simple.
2. Bounties, combat bonds and other "vouchers". It ignores bounties handed into non "EDA Kunti League Factions".

Important things to fix (not necessarily in this order):
1. A better display (e.g. counting up the number of influence missions instead of listing them separately)
2. Fixing anti-minor faction mission detection.
3. Cartography data.
4. Trades.
5. Mission failure, abandonment and redirection.
6. Internal fixes like a better folder structure, design improvements and automated tests.
7. Error handling and command line argument support, e.g. for the day.

If this tool proves useful we may automate the collation of these further, e.g. by pasting the results to Discord directly or sending them to a central source for consolidated reporting.

TODO: Add Frontier disclaimer.
TODO: Add reference to documentation (http://hosting.zaonce.net/community/journal/v27/Journal-Manual_v27.pdf)

# License
 
(TODO, probably MIT or similar)