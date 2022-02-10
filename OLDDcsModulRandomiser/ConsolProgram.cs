using System;
using System.IO;
using Newtonsoft.Json;

namespace DcsModulRandomiser
{
    class ConsolProgram
    {
        void Main(string[] args)// arg0 doc, then reroll, forcemap mapname
        {
            Random random = new Random();
            
            DMRProfile dMRProfile;
            bool reroll = false;
            string forceMap = null;
            try
            {

                if(args.Length < 1)
                {
                    Console.WriteLine("No profil file given.");
                    Console.ReadLine();
                    return;
                }

                string jsonString = File.ReadAllText(args[0]);
                //dMRProfile = JsonSerializer.Deserialize<DMRProfile>(jsonString);
                dMRProfile = JsonConvert.DeserializeObject<DMRProfile>(jsonString);


                if (args.Length >= 2)
                {
                    for(int i=1; i<args.Length; i++)
                    {
                        switch(args[i])
                        {
                            case "reroll":
                                reroll = true;
                                break;

                            case "forcemap":
                                if(args.Length >= i + 2)
                                {
                                    forceMap = args[i + 1];
                                }
                                else
                                {
                                    Console.WriteLine("forcemap : No argument.");
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
                if(dMRProfile.currentRollDate == null || dMRProfile.currentRollDate == "")
                {
                    return true;
                }
                return DateTime.Today > DateTime.Parse(dMRProfile.currentRollDate);
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
                dMRProfile.currentRollName = map.mapName + " : " + module.name;

                //Set a random date

                DateTime rdmDate = DateTime.Today;
                float rdm = (random.Next(dMRProfile.dayMin, dMRProfile.dayMax) * module.time_multiplier);
                dMRProfile.currentRollDate = rdmDate.AddDays(rdm).ToString();

                //Save
                Serialize(args[0]);

                return dMRProfile.currentRollName;
            }

            void Serialize(string Path)
            {
                string jsonString = JsonConvert.SerializeObject(dMRProfile, Formatting.Indented);
                File.WriteAllText(Path, jsonString);
            }

            string getCurrentModule()
            {
                Console.WriteLine("Picking last module");
                return dMRProfile.currentRollName;
            }

            Map GetRandMap()
            {
                int rdm = (random.Next(0, dMRProfile.Maps.Count));
                return dMRProfile.Maps[rdm];
            }

            Map GetMapByName(string mapName)
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

            Module RecGetRandNode(Module node)
            {
                if (node.IsALeave())
                {
                    return node;
                }

                int rdm = random.Next(0, node.Childs.Count);
                return RecGetRandNode(node.Childs[rdm]);
            }

            Module GetRandNodeInMap(Map map)
            {
                int rdm = random.Next(0, map.planeSet.Count);
                return RecGetRandNode(map.planeSet[rdm]);
            }
        }
    }
}
