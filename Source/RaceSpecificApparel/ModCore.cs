using System;
using HarmonyLib;
using Verse;

namespace RaceSpecificApparel;

public class ModCore : Mod
{
    public ModCore(ModContentPack content) : base(content)
    {
        var harmony = new Harmony("Epicguru.RaceSpecificApparel");

        try
        {
            harmony.PatchAll();
            Log.Message(
                $"Successfully patched {harmony.GetPatchedMethods().EnumerableCount()} methods for RaceSpecificApparel.");
        }
        catch (Exception e)
        {
            Log.Error("Failed to patch 1 or more method for RaceSpecificApparel!");
            Log.Error(e.ToString());
        }
    }
}