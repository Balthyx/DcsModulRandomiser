using DCSModulRandomiser;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;

public static class Randomisator
{
    static Random random = new Random();
    public static string Get(string profile_path, bool reroll = false)// arg0 doc, then reroll, forcemap mapname
    {
        Random random = new Random();

        try
        {
            string jsonString = File.ReadAllText(profile_path);
            //dMRProfile = JsonSerializer.Deserialize<DMRProfile>(jsonString);
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
        catch (Exception e)
        {
            Console.WriteLine(e);
            MessageBox.Show(e.Message);
            return null;
        }

    }

    static bool IsDateExpired(DMRProfile dMRProfile)
    {
        if (dMRProfile.currentRollDate == null || dMRProfile.currentRollDate == "")
        {
            return true;
        }
        return DateTime.Today > DateTime.Parse(dMRProfile.currentRollDate);
    }

    static string RollNewModule(DMRProfile dMRProfile)
    {
        //Picking a random module

        Module module = GetRandomModule(dMRProfile);


        //Set CurrentRoll Node
        dMRProfile.currentRollName = GetRandomPeriode(module) + " : " + module.name;

        //Set a random date

        DateTime rdmDate = DateTime.Today;
        float rdm = (random.Next(dMRProfile.dayMin, dMRProfile.dayMax) * module.time_multiplier);
        dMRProfile.currentRollDate = rdmDate.AddDays(rdm).ToString();

        

        return dMRProfile.currentRollName;
    }

    static void Serialize(string Path, DMRProfile dMRProfile)
    {
        string jsonString = JsonConvert.SerializeObject(dMRProfile, Formatting.Indented);
        File.WriteAllText(Path, jsonString);
    }

    static Module GetRandomModule(DMRProfile dMR_Profile)
    {
        //Get TotalWeight
        float totalWeight = 0;
        foreach(Module module in dMR_Profile.Modules)
        {
            totalWeight += module.weight;
        }

        //Pick random
        float picked = (float)(random.NextDouble() * totalWeight);

        foreach (Module module in dMR_Profile.Modules)
        {
            picked -= module.weight;
            if (picked <= 0)
                return module;
        }
        return dMR_Profile.Modules[dMR_Profile.Modules.Count - 1];
    }

    static string GetRandomPeriode(Module module)
    {
        //Get TotalWeight
        float totalWeight = module.ww2_weight + module.corea_weight + module.cW60_weight + module.cW80_weight + module.modern_weight;
        float[] weights = new float[5] { module.ww2_weight, module.corea_weight, module.cW60_weight, module.cW80_weight, module.modern_weight };
        string[] strPeriod = new string[5] { "WW2", "Corea", "CW60", "CW80", "Modern" };

        //Pick random
        float picked = (float)(random.NextDouble() * totalWeight);
        for(int i=0; i< weights.Length; i++)
        {
            picked -= weights[i];
            if (picked <= 0)
                return strPeriod[i];
        }

        return strPeriod[strPeriod.Length - 1];
    }
}
