using DCSModulRandomiser;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;

public static class Randomisator
{
    static DMRProfile dMRProfile;
    static Random random = new Random();
    public static string Get(string profile_path, bool reroll = false, string forceMap = null)// arg0 doc, then reroll, forcemap mapname
    {
        Random random = new Random();

        try
        {
            string jsonString = File.ReadAllText(profile_path);
            //dMRProfile = JsonSerializer.Deserialize<DMRProfile>(jsonString);
            dMRProfile = JsonConvert.DeserializeObject<DMRProfile>(jsonString);


            if (IsDateExpired() || reroll)
            {
                return getRandomModule(forceMap, profile_path);
            }
            else
            {
                return getCurrentModule();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            MessageBox.Show(e.Message);
            return null;
        }
        
    }

    static bool IsDateExpired()
    {
        if (dMRProfile.currentRollDate == null || dMRProfile.currentRollDate == "")
        {
            return true;
        }
        return DateTime.Today > DateTime.Parse(dMRProfile.currentRollDate);
    }

    static string getRandomModule(string forcedMap, string profile_path)
    {
        //Console.WriteLine("Picking a random module");
        Map map;
        if (forcedMap != null)
        {
            map = GetMapByName(forcedMap);
            if (map == null)
            {
                //Console.WriteLine("ForceMap : Map not found : " + forcedMap);
                return null;
            }
        }
        else
        {
            map = GetRandMap();
        }

        Module module = GetRandNodeInMap(map);


        //Set CurrentRoll Node
        dMRProfile.currentRollName = map.mapName + " : " + module.name;

        //Set a random date

        DateTime rdmDate = DateTime.Today;
        float rdm = (random.Next(dMRProfile.dayMin, dMRProfile.dayMax) * module.time_multiplier);
        dMRProfile.currentRollDate = rdmDate.AddDays(rdm).ToString();

        //Save
        Serialize(profile_path);

        return dMRProfile.currentRollName;
    }

    static void Serialize(string Path)
    {
        string jsonString = JsonConvert.SerializeObject(dMRProfile, Formatting.Indented);
        File.WriteAllText(Path, jsonString);
    }

    static string getCurrentModule()
    {
        Console.WriteLine("Picking last module");
        return dMRProfile.currentRollName;
    }

    static Map GetRandMap()
    {
        int rdm = (random.Next(0, dMRProfile.Maps.Count));
        return dMRProfile.Maps[rdm];
    }

    static Map GetMapByName(string mapName)
    {
        foreach (Map chMap in dMRProfile.Maps)
        {
            if (chMap.mapName == mapName)
            {
                return chMap;
            }
        }
        return null;
    }

    static Module RecGetRandNode(Module node)
    {
        if (node.IsALeave())
        {
            return node;
        }

        int rdm = random.Next(0, node.Childs.Count);
        return RecGetRandNode(node.Childs[rdm]);
    }

    static Module GetRandNodeInMap(Map map)
    {
        int rdm = random.Next(0, map.planeSet.Count);
        return RecGetRandNode(map.planeSet[rdm]);
    }
}
