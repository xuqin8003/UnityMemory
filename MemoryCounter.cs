using System;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling;

public class MemoryCounter : IDisposable
{
    private const char BAR_CHAR = '█';
    private const int BAR_CHAR_COUNT = 75;
    private StringBuilder log = new StringBuilder();

    public MemoryCounter(string title = "")
    {
        log.AppendLine(string.Format("Memory {0} Information\n", title));
        log.AppendLine("Before");
        AddMemoryInfo();
    }

    private string GetBytesFormated(double bytes)
    {
        var order = 0;
        string[] bytesSizes = new string[] { "B", "KB", "MB", "GB" };

        while (bytes >= 1024 && ++order < bytesSizes.Length)
            bytes = bytes / 1024;

        return string.Format("{0:0.##}{1}", bytes, bytesSizes[order]);
    }

    private void AddMemoryInfo()
    {
        GC.Collect();
        GC.Collect();

        var memorySize = SystemInfo.systemMemorySize * 1024d * 1024d;
        var totalMemory = Profiler.GetTotalReservedMemoryLong() + Profiler.GetMonoHeapSizeLong();
        var usedMemory = Profiler.GetTotalAllocatedMemoryLong() + Profiler.GetMonoUsedSizeLong();

        var totalMemoryPercent = totalMemory / memorySize;
        var usedMemoryPercent = usedMemory / memorySize;

        log.Append("<color=#9575cd>");

        for (var i = 0; i < BAR_CHAR_COUNT; i++)
        {
            log.Append(BAR_CHAR);

            if (i == (int)(usedMemoryPercent * BAR_CHAR_COUNT))
                log.Append("</color><color=#ffcdd2>");

            if (i == (int)(totalMemoryPercent * BAR_CHAR_COUNT))
                log.Append("</color>");
        }

        log.AppendFormat("\nMemory Usage: {0:00.0%} ({1})\n", usedMemoryPercent, GetBytesFormated(usedMemory));
        log.AppendFormat("Memory Allocated: {0:00.0%} ({1})\n", totalMemoryPercent, GetBytesFormated(totalMemory));
    }

    public void Dispose()
    {
        log.AppendLine("\nAfter");
        AddMemoryInfo();
        Debug.LogFormat("MemoryCounter : {0}", log.ToString());
    }
}