using System.Collections.Generic;
using Verse;

namespace RaceSpecificApparel;

public class RaceSpecific : DefModExtension
{
    private Dictionary<string, RaceSpecificData> dataMap;
    private readonly List<RaceSpecificData> races = new List<RaceSpecificData>();

    public RaceSpecificData TryGetRaceData(string raceDefName)
    {
        if (dataMap == null)
        {
            dataMap = new Dictionary<string, RaceSpecificData>();
            foreach (var item in races)
            {
                if (string.IsNullOrWhiteSpace(item.race))
                {
                    Log.Error("Missing <race> tag in race specific data!");
                    continue;
                }

                if (dataMap.ContainsKey(item.race))
                {
                    Log.Error($"There is already apparel data for race '{item.race}'. Do not add it twice!");
                    continue;
                }

                // Try to find race...
                var found = DefDatabase<ThingDef>.GetNamed(item.race, false);
                if (found == null)
                {
                    Log.Warning(
                        $"[RaceSpecificApparel] Failed to find race called '{item.race}'. Perhaps the corresponding race mod is not installed?");
                }

                dataMap.Add(item.race, item);
            }
        }

        if (raceDefName == null)
        {
            return null;
        }

        return dataMap.TryGetValue(raceDefName);
    }
}