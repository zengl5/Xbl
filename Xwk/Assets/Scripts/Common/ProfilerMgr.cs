using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class ProfilerMgr {
    private static bool showProfiler = true;
	 public static void BeginSample(string name)
    {
        if (!showProfiler)
        {
            return;
        }
       Profiler.BeginSample(name);
    }
    public static void EndProfile()
    {
        if (!showProfiler)
        {
            return;
        }
        Profiler.EndSample();
    }
}
