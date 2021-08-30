using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace DcsModulRandomiser
{
    class Program
    {
        static void Main(string[] args)// arg0 doc, then reroll, forcemap mapname
        {
            Random random = new Random();
            
            DMRProfile dMRProfile;
            bool reroll = false;
            string forceMap = null;
            try
            {
                string jsonString = File.ReadAllText(args[0]);
                dMRProfile = JsonSerializer.Deserialize<DMRProfile>(jsonString);

                if (args.Length >= 2)
                {
                    for(int i=1; i<args.Length; i++)
                    {
                        switch(args[i])
                        {
                            case "reroll":
                                reroll = true;
                                break;

                            case "forceMap":
                                if(args.Length >= i + 2)
                                {
                                    forceMap = args[i + 1];
                                }
                                else
                                {
                                    Console.WriteLine("ForceMap : No argument.");
                                }
                                break;
                                
                        }
                    }
                }

                if (IsDateExpired() || reroll)
                {
                    Console.WriteLine( getRandomModule(forceMap) );
                }
                else
                {
                    Console.WriteLine(getCurrentModule());
                }
                
                Console.ReadLine();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
            }

            bool IsDateExpired()
            {
                if(dMRProfile.currentRollDate == null)
                {
                    return true;
                }
                return DateTime.Today > dMRProfile.currentRollDate;
            }

            string getRandomModule(string forcedMap)
            {
                Console.WriteLine("Picking a random module");
                Map map;
                if (forcedMap != null)
                {
                    map = GetMapByName(forcedMap);
                    if (map == null)
                    {
                        Console.WriteLine("ForceMap : Map not found : " + forcedMap);
                        return null;
                    }
                }
                else
                {
                    map = GetRandMap();
                }

                Module module = GetRandNodeInMap(map);


                //Set CurrentRoll Node
                dMRProfile.currentRollName = module.name;

                //Set a random date

                DateTime rdmDate = DateTime.Today;
                int rdm = (random.Next(dMRProfile.dayMin, dMRProfile.dayMax));
                dMRProfile.currentRollDate = rdmDate.AddDays(rdm);

                //Save
                Serialize(args[0]);

                return dMRProfile.currentRollName;
            }

            void Serialize(string Path)
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(dMRProfile, options);
                File.WriteAllText(Path, jsonString);
            }

            string getCurrentModule()
            {
                Console.WriteLine("Picking last module");
                return dMRProfile.currentRollName;
            }

            Map GetRandMap()
            {
                int rdm = (random.Next(0, dMRProfile.maps.Count));
                return dMRProfile.maps[rdm];
            }

            Map GetMapByName(string mapName)
            {
                foreach (Map chMap in dMRProfile.maps)
                {
                    if (chMap.mapName == mapName)
                    {
                        return chMap;
                    }
                }
                return null;
            }

            Module RecGetRandNode(Module node)
            {
                if (node.IsALeave)
                {
                    return node;
                }

                int rdm = random.Next(0, node.childs.Count);
                return RecGetRandNode(node.childs[rdm]);
            }

            Module GetRandNodeInMap(Map map)
            {
                int rdm = random.Next(0, map.planeSet.Count);
                return RecGetRandNode(map.planeSet[rdm]);
            }
        }
    }
}
