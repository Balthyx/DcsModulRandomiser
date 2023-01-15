using DCSModuleRandomiser;
using DCSModulRandomiser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

public static class Randomisator
{
    private static readonly Random random = new Random();

    /// <summary>
    /// Get the current roll, or get a new one
    /// </summary>
    /// <param name="profile_path"></param>
    /// <param name="reroll">force reroll</param>
    /// <returns></returns>
    public static string Get(string profile_path, bool reroll = false)// arg0 doc, then reroll, forcemap mapname
    {
        DMRProfile dMRProfile = Serializer.Deserialize(profile_path);

        string roll;

        if (IsDateExpired(dMRProfile) || reroll)
        {
            roll = RollNewModule(dMRProfile);
        }
        else
        {
            roll = dMRProfile.currentRollName;
        }

        //Save
        Serialize(profile_path, dMRProfile);
        return roll;
    }

    private static bool IsDateExpired(DMRProfile dmr_profile)
    {
        if (dmr_profile.currentRollDate == null)
        {
            return true;
        }
        return DateTime.Today > dmr_profile.currentRollDate;
    }

    private static string RollNewModule(DMRProfile dmr_profile)
    {
        //Get profiles
        List<ServerProfile> serverProfiles = new List<ServerProfile>();
        foreach(string serverName in dmr_profile.serverProfiles)
        {
            string jsonString = File.ReadAllText(serverName);
            serverProfiles.Add(JsonConvert.DeserializeObject<ServerProfile>(jsonString));
        }

        //Merge profiles by modules

        List<ModulMergedProfil> mergedProfils = ProfilUtils.MergeSrvProfiles(serverProfiles);


        //Picking a random module

        KeyValuePair<Module, string> module = GetRandomModule(mergedProfils);


        //Set CurrentRoll Node
        dmr_profile.currentRollName = module.Key.name + " : " + module.Value;

        //Set a random date

        DateTime rdmDate = DateTime.Today;
        float rdm = (random.Next(dmr_profile.dayMin, dmr_profile.dayMax) * module.Key.time_multiplier);
        dmr_profile.currentRollDate = rdmDate.AddDays(rdm);

        

        return dmr_profile.currentRollName;
    }

    /// <summary>
    /// Write the new roll in the profil.json
    /// </summary>
    /// <param name="path"></param>
    /// <param name="dmr_profile"></param>
    private static void Serialize(string path, DMRProfile dmr_profile)
    {
        string jsonString = JsonConvert.SerializeObject(dmr_profile, Formatting.Indented);
        File.WriteAllText(path, jsonString);
    }

    private static KeyValuePair<Module, string> GetRandomModule(List<ModulMergedProfil> merged_profils)
    {
        //Get TotalWeight
        float totalWeight = 0;
        foreach(ModulMergedProfil module in merged_profils)
        {
            totalWeight += module.module.weight;
        }

        //Pick random
        float picked = (float)(random.NextDouble() * totalWeight);

        ModulMergedProfil pickedModule = merged_profils[^1];

        foreach (ModulMergedProfil module in merged_profils)
        {
            picked -= module.module.weight;
            if (picked <= 0)
            {
                pickedModule = module;
                break;
            }
        }
        return pickedModule.GetRandomServer();
    }
}
