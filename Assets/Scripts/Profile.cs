using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Profile
{
    private static Dictionary<string, float> times = new Dictionary<string, float>();
    public static void Start(string name)
    {
        times.Add(name, Time.time);
    }

    public static void End(string name)
    {
        if (!times.ContainsKey(name)) return;
        float delta = Time.time - times[name];
        Debug.Log(delta);
    }
}
