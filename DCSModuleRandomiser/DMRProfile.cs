using System.Collections.Generic;

namespace DCSModulRandomiser
{
    public class DMRProfile
    {
        public int dayMin;
        public int dayMax;
        public string currentRollName;
        public string currentRollDate;

        public List<Module> Modules;
    }

    public class Module
    {
        public string name;
        public float time_multiplier = 1;
        public float weight = 1;

        public float ww2_weight = 1;
        public float corea_weight = 1;
        public float cW60_weight = 1;
        public float cW80_weight = 1;
        public float modern_weight = 1;
    }
}