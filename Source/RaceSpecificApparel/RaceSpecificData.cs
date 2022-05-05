using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RaceSpecificApparel;

public class RaceSpecificData
{
    private static HashSet<string> _dis;

    private readonly List<string> disallowStyle = new List<string>();
    public bool autoStyle = true;
    public Color? customColor;
    public string customPath;
    public Vector2? customSize;
    public string race;

    public bool AllowStyle(string styleName)
    {
        if (_dis != null)
        {
            return !_dis.Contains(styleName);
        }

        _dis = new HashSet<string>();
        _dis.AddRange(disallowStyle);

        return !_dis.Contains(styleName);
    }
}