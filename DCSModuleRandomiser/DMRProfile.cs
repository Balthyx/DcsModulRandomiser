using System.Collections.Generic;

namespace DCSModulRandomiser
{
    public class DMRProfile
    {
        public int dayMin;
        public int dayMax;
        public string currentRollName;
        public string currentRollDate;

        public List<string> serverProfiles;
    }

    public class ServerProfile
    {
        public string name;
        public List<Module> modules;
    }

    public class Module
    {
        public string name;
        public float time_multiplier = 1;
        public float weight = 1;
    }
}