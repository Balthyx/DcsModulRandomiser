using DCSModulRandomiser;
using System;
using System.Collections.Generic;
using System.Text;

namespace DCSModuleRandomiser
{
    class ModulMergedProfil
    {
        Random random = new Random();

        public Module module;
        public Dictionary<Module, string> servers;
        public string Name => module.name;

        public ModulMergedProfil(string name, float weight)
        {
            module = new Module();
            module.name = name;
            module.weight = weight;
            servers = new Dictionary<Module, string>();
        }

        public KeyValuePair<Module, string> GetRandomServer()
        {
            //Get TotalWeight
            float totalWeight = 0;
            foreach (KeyValuePair<Module, string> module in servers)
            {
                totalWeight += module.Key.weight;
            }

            //Pick random
            float picked = (float)(random.NextDouble() * totalWeight);

            foreach (KeyValuePair<Module, string> module in servers)
            {
                picked -= module.Key.weight;
                if (picked <= 0)
                {
                    return module;
                }
            }
            return new KeyValuePair<Module, string>();
        }
    }
}
