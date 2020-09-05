using System;
using System.Collections.Generic;

namespace EDMinorFactionSupport
{
    public class Mission
    {
        public Mission(long id, string name, string influence)
        {
            Name = name;
            Influence = influence;
            Id = id;
        }

        public string Name
        {
            get;
        }
        public string Influence 
        { 
            get; 
        }

        public long Id
        {
            get;
        }
    }
}