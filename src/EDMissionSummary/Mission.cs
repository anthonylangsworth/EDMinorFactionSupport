namespace EDMissionSummary
{
    public class Mission
    {
        public Mission(string missonId, string name, string sourceMinorFactionName, string targetMinorFactionName, string destinationSystem)
        {
            TargetMinorFactionName = targetMinorFactionName;
            SourceMinorFactionName = sourceMinorFactionName;
            DestinationSystem = destinationSystem;
            Name = name;
            MissonId = missonId;
        }

        public string TargetMinorFactionName
        {
            get;
        }

        public string SourceMinorFactionName
        {
            get;
        }

        public string DestinationSystem
        {
            get;
        }

        public string Name
        {
            get;
        }

        public string MissonId
        {
            get;
        }
    }
}