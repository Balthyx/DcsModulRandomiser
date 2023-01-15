using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DCSModulRandomiser
{
    public class DMRProfile
    {
        public int dayMin;
        public int dayMax;
        public string currentRollName;
        public DateTime currentRollDate;

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

    public static class Serializer
    {
        public static DMRProfile Deserialize(string path)
        {
            string jsonString = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<DMRProfile>(jsonString);
        }
    }
}