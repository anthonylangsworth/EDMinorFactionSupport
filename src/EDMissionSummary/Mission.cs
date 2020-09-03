namespace EDMissionSummary
{
    public class Mission
    {
        public Mission(string missonId, string name, string sourceFactionName, string targetFactionName, string destinationSystem, string destinationStation)
        {
            TargetFactionName = targetFactionName;
            SourceFactionName = sourceFactionName;
            DestinationSystem = destinationSystem;
            DestinationStation = destinationStation;
            Name = name;
            MissonId = missonId;
        }

        public string TargetFactionName
        {
            get;
        }

        public string SourceFactionName
        {
            get;
        }

        public string DestinationSystem
        {
            get;
        }

        public string DestinationStation
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