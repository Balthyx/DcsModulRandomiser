using DCSModuleRandomiser;
using DCSModulRandomiser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

public static class Randomisator
{
    static Random random = new Random();
    public static string Get(string profile_path, bool reroll = false)// arg0 doc, then reroll, forcemap mapname
    {
        Random random = new Random();

        string jsonString = File.ReadAllText(profile_path);
        DMRProfile dMRProfile = JsonConvert.DeserializeObject<DMRProfile>(jsonString);
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

    static bool IsDateExpired(DMRProfile dMRProfile)
    {
        if (dMRProfile.currentRollDate == null)
        {
            return true;
        }
        return DateTime.Today > dMRProfile.currentRollDate;
    }

    static string RollNewModule(DMRProfile dMRProfile)
    {
        //Get profiles
        List<ServerProfile> serverProfiles = new List<ServerProfile>();
        foreach(string serverName in dMRProfile.serverProfiles)
        {
            string jsonString = File.ReadAllText(serverName);
            serverProfiles.Add(JsonConvert.DeserializeObject<ServerProfile>(jsonString));
        }

        //Merge profiles

        List<ModulMergedProfil> mergedProfils = ProfilUtils.MergeSrvProfiles(serverProfiles);


        //Picking a random module

        KeyValuePair<Module, string> module = GetRandomModule(dMRProfile, mergedProfils);


        //Set CurrentRoll Node
        dMRProfile.currentRollName = module.Key.name + " : " + module.Value;

        //Set a random date

        DateTime rdmDate = DateTime.Today;
        float rdm = (random.Next(dMRProfile.dayMin, dMRProfile.dayMax) * module.Key.time_multiplier);
        dMRProfile.currentRollDate = rdmDate.AddDays(rdm);

        

        return dMRProfile.currentRollName;
    }

    static void Serialize(string Path, DMRProfile dMRProfile)
    {
        string jsonString = JsonConvert.SerializeObject(dMRProfile, Formatting.Indented);
        File.WriteAllText(Path, jsonString);
    }

    static KeyValuePair<Module, string> GetRandomModule(DMRProfile dMR_Profile, List<ModulMergedProfil> mergedProfils)
    {
        //Get TotalWeight
        float totalWeight = 0;
        foreach(ModulMergedProfil module in mergedProfils)
        {
            totalWeight += module.module.weight;
        }

        //Pick random
        float picked = (float)(random.NextDouble() * totalWeight);

        ModulMergedProfil pickedModule = mergedProfils[mergedProfils.Count - 1];

        foreach (ModulMergedProfil module in mergedProfils)
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
