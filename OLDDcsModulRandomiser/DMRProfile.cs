using System.Collections.Generic;

namespace DcsModulRandomiser
{
    public class DMRProfile
    {
        public int dayMin;
        public int dayMax;
        public string currentRollName;
        public string currentRollDate;

        public List<Map> Maps;
    }

    public class Map
    {
        public string mapName;
        public List<Module> planeSet;
    }

    public class Module
    {
        public string name;
        public float time_multiplier = 1;
        public List<Module> Childs;
        public bool IsALeave() {
            return Childs == null || Childs.Count == 0; 
        }
    }
}