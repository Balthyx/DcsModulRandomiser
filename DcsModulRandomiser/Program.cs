using System;
using System.Collections.Generic;
using System.Xml;

namespace DcsModulRandomiser
{
    class Program
    {
        static void Main(string[] args)// arg0 doc, then reroll, forcemap mapname
        {
            Random random = new Random();
            XmlDocument doc = new XmlDocument();
            bool reroll = false;
            string forceMap = null;
            try
            {
                doc.Load(args[0]);
                if(args.Length >= 2)
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
                XmlNode currentRollNode = doc.SelectSingleNode("/ Root / CurrentRoll");
                if(currentRollNode.Attributes.GetNamedItem("date").Value == "")
                {
                    return true;
                }
                DateTime date = DateTime.Parse(currentRollNode.Attributes.GetNamedItem("date").Value);
                return DateTime.Today > date;
            }

            string getRandomModule(string forcedMap)
            {
                Console.WriteLine("Picking a random module");
                XmlNode map;
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

                XmlNode module = RecGetRandNode(map);

                string currentRoll = map.Attributes.GetNamedItem("name").Value.ToString() + " : " + module.Attributes.GetNamedItem("name").Value.ToString();

                //Set CurrentRoll Node
                XmlNode CurrentRollNode = doc.SelectSingleNode("/ Root / CurrentRoll");

                CurrentRollNode.Attributes.GetNamedItem("name").Value = currentRoll;

                //Set a random date
                XmlNode ParametersNode = doc.SelectSingleNode("/ Root / Parameters");
                int dayMin = int.Parse(ParametersNode.Attributes.GetNamedItem("dayMin").Value);
                int dayMax = int.Parse(ParametersNode.Attributes.GetNamedItem("dayMax").Value);

                DateTime rdmDate = DateTime.Today;
                int rdm = (random.Next(dayMin, dayMax));
                rdmDate = rdmDate.AddDays(rdm);

                CurrentRollNode.Attributes.GetNamedItem("date").Value = rdmDate.ToShortDateString();
                doc.Save(args[0]);

                return currentRoll;
            }

            string getCurrentModule()
            {
                Console.WriteLine("Picking last module");
                return doc.SelectSingleNode("/ Root / CurrentRoll").Attributes.GetNamedItem("name").Value.ToString();
            }

            XmlNode GetRandMap()
            {
                XmlNode mapNode = doc.SelectSingleNode("/ Root / List");
                int rdm = (random.Next(0, mapNode.ChildNodes.Count));
                return mapNode.ChildNodes.Item(rdm);
            }

            XmlNode GetMapByName(string mapName)
            {
                XmlNode mapNode = doc.SelectSingleNode("/ Root / List");
                foreach (XmlNode chMap in mapNode)
                {
                    if (chMap.Attributes.GetNamedItem("name").Value == mapName)
                    {
                        return chMap;
                    }
                }
                return null;
            }

            XmlNode RecGetRandNode(XmlNode node)
            {
                if (node.HasChildNodes)
                {
                    List<XmlNode> weightedCount = new List<XmlNode>();
                    foreach(XmlNode mapNode in node.ChildNodes)
                    {
                        if(mapNode.Attributes.GetNamedItem("weight") != null)
                        {
                            for (int i = 0; i < int.Parse(mapNode.Attributes.GetNamedItem("weight").Value); i++)
                            {
                                weightedCount.Add(mapNode);
                            }
                        }
                        else
                        {
                            weightedCount.Add(mapNode);
                        }
                        
                    }

                    int rdm = random.Next(0, weightedCount.Count);
                    return RecGetRandNode(weightedCount[rdm]);
                }
                else
                {
                    return node;
                }
            }

        }

        
    }
}
