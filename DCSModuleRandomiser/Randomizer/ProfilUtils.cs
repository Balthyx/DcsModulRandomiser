using DCSModulRandomiser;
using System;
using System.Collections.Generic;
using System.Text;

namespace DCSModuleRandomiser
{
    static class ProfilUtils
    {
        public static List<ModulMergedProfil> MergeSrvProfiles(List<ServerProfile> server_profiles)
        {
            List<ModulMergedProfil> outProfiles = new List<ModulMergedProfil>();

            ModulMergedProfil currentModule;
            foreach(ServerProfile serverProfile in server_profiles)
            {
                foreach(Module module in serverProfile.modules)
                {
                    currentModule = (outProfiles.Find((x) => x.module.name == module.name));
                    //if the module is already listed
                    if (currentModule != null)
                    {
                        currentModule.module.weight += module.weight;
                    }
                    else
                    {
                        currentModule = new ModulMergedProfil(module.name, module.weight);
                        outProfiles.Add(currentModule);
                    }
                    currentModule.servers.Add(module, serverProfile.name);
                }
            }

            foreach(ModulMergedProfil modulMerged in outProfiles)
            {
                modulMerged.module.weight /= outProfiles.Count;
            }

            return outProfiles;
        }
    }
}
