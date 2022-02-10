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

        public float[] periodWeights = new float[5] { 1, 1, 1, 1, 1 };
    }

    public enum TimePeriod
    {
        WW2,
        Corea,
        ColdWar60,
        ColdWar80,
        Modern
    }
}