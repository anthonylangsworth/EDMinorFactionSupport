using System;
using System.Collections.Generic;
using System.Text;

namespace EDMissionSummary
{
    /// <summary>
    /// The result of a mission or other activity on the supported minor faction.
    /// </summary>
    public enum FactionSupport
    {
        /// <summary>
        /// The action improves or supports the minor faction.
        /// </summary>
        Support = 0,
        /// <summary>
        /// The action undermines or goes against the minor faction.
        /// </summary>
        Undermine = 1,
        /// <summary>
        /// The action has no impact on the minor faction.
        /// </summary>
        None = 2
    }
}
