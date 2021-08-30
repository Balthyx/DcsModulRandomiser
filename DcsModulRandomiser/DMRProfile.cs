using System;
using System.Collections.Generic;

namespace DcsModulRandomiser
{
    public class DMRProfile
    {
        public int dayMin;
        public int dayMax;
        public string currentRollName;
        public DateTime currentRollDate;

        public List<Map> maps;
    }

    public class Map
    {
        public string mapName;
        public List<Module> planeSet;
    }

    public class Module
    {
        public string name;
        public List<Module> childs;
        public bool IsALeave { get => childs.Count == 0; }
    }
}