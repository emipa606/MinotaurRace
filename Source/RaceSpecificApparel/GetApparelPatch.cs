using HarmonyLib;
using RimWorld;
using Verse;

namespace RaceSpecificApparel;

[HarmonyPatch(typeof(ApparelGraphicRecordGetter), "TryGetGraphicApparel")]
internal static class GetApparelPatch
{
    public static string NextRaceDefName = null;

    private static void Postfix(Apparel apparel, BodyTypeDef bodyType, ref ApparelGraphicRecord rec, bool __result)
    {
        if (NextRaceDefName == null)
        {
            return;
        }

        if (!__result)
        {
            return;
        }

        var extension = apparel.def.GetModExtension<RaceSpecific>();

        var data = extension?.TryGetRaceData(NextRaceDefName);
        if (data == null)
        {
            return;
        }

        var styleName = apparel.StyleDef?.defName;
        styleName = styleName?.Substring(0, styleName.IndexOf('_'));
        var doStyles = styleName != null && !string.IsNullOrEmpty(apparel.StyleDef.wornGraphicPath) && data.autoStyle &&
                       data.customPath != null && data.AllowStyle(styleName);

        // Copied from regular vanilla method: path calculation.
        // Ideally I would have liked to extract this from the record for extra compatibility,
        // but it seems that isn't possible.
        string path;
        if (apparel.def.apparel.LastLayer == ApparelLayerDefOf.Overhead ||
            apparel.def.apparel.LastLayer == ApparelLayerDefOf.EyeCover || PawnRenderer.RenderAsPack(apparel) ||
            apparel.WornGraphicPath == BaseContent.PlaceholderImagePath ||
            apparel.WornGraphicPath == BaseContent.PlaceholderGearImagePath)
        {
            path = data.customPath ?? apparel.WornGraphicPath + $"_{NextRaceDefName}";
            if (doStyles)
            {
                path += $"_{styleName}";
            }
        }
        else
        {
            if (data.customPath == null)
            {
                path = apparel.WornGraphicPath + $"_{NextRaceDefName}" + $"_{bodyType.defName}";
            }
            else
            {
                path = data.customPath + $"_{bodyType.defName}";
            }

            if (doStyles)
            {
                path += $"_{styleName}";
            }
        }

        var shader = ShaderDatabase.Cutout;
        if (apparel.def.apparel.useWornGraphicMask)
        {
            shader = ShaderDatabase.CutoutComplex;
        }

        // Try to find the race-specific graphic. If it can't be found,
        // fall back to the existing (human) one.

        Graphic graphic;
        try
        {
            graphic = GraphicDatabase.Get<Graphic_Multi>(path, shader,
                data.customSize ?? apparel.def.graphicData.drawSize, data.customColor ?? apparel.DrawColor);
        }
        catch
        {
            graphic = null;
        }

        if (graphic == null || graphic.MatSingle == null)
        {
            Log.Warning(
                $"[RaceSpecificApparel] Looked for '{path}' but failed to find that texture for race '{NextRaceDefName}'. Apparel is {apparel.def.defName} ({apparel.def.fileName}).");
            return;
        }

        rec = new ApparelGraphicRecord(graphic, apparel);
    }
}