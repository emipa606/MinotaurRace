using HarmonyLib;
using RimWorld;
using Verse;

namespace RaceSpecificApparel;

[HarmonyPatch(typeof(PawnGraphicSet), "ResolveApparelGraphics")]
public static class PawnGraphicsSetPatch
{
    private static void Prefix(PawnGraphicSet __instance)
    {
        GetApparelPatch.NextRaceDefName =
            __instance.pawn?.def == ThingDefOf.Human ? null : __instance.pawn?.def?.defName;
    }
}