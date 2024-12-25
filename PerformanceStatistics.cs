using System.Text;
using UnityEngine;
using UnityEngine.Profiling;

/// <summary>
/// GUI stats.
/// 性能统计窗口
/// </summary>
public class PerformanceStatistics : MonoBehaviour
{
    private Rect windowRect = new Rect(50, 20, 250, 250);
    private GUIStyle labelStyle;
    private GUIStyle fpsStyle;
    private GUIStyle memStyle;
    private GUIStyle sysStyle;
    private string sysinfo = "";
    private bool IsShowMem = false;
    private bool IsShowScreen = false;
    private bool IsShowSysInfo = false;

    private void Start()
    {
        labelStyle = new GUIStyle();
        labelStyle.normal.background = null;
        labelStyle.normal.textColor = new Color(0.0f, 1.0f, 0.0f);
        labelStyle.fontSize = 12;

        fpsStyle = new GUIStyle();
        fpsStyle.normal.background = null;
        fpsStyle.normal.textColor = new Color(1.0f, 0.0f, 0.0f);
        fpsStyle.fontSize = 12;

        memStyle = new GUIStyle();
        memStyle.normal.background = null;
        memStyle.normal.textColor = new Color(1.0f, 0.3f, 0.0f);
        memStyle.fontSize = 12;

        sysStyle = new GUIStyle();
        sysStyle.normal.background = null;
        sysStyle.normal.textColor = new Color(1.0f, 1.0f, 1.0f);
        sysStyle.fontSize = 12;

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("设备信息");
        sb.AppendLine("操作系统名称: " + SystemInfo.operatingSystem);
        sb.AppendLine("处理器名称: " + SystemInfo.processorType);
        sb.AppendLine("处理器数量: " + SystemInfo.processorCount);
        sb.AppendLine("当前系统内存大小: " + SystemInfo.systemMemorySize + "MB");
        sb.AppendLine("当前显存大小: " + SystemInfo.graphicsMemorySize + "MB");
        sb.AppendLine("显卡名字: " + SystemInfo.graphicsDeviceName);
        sb.AppendLine("显卡厂商: " + SystemInfo.graphicsDeviceVendor);
        sb.AppendLine("显卡的标识符代码: " + SystemInfo.graphicsDeviceID);
        sb.AppendLine("显卡厂商的标识符代码: " + SystemInfo.graphicsDeviceVendorID);
        sb.AppendLine("该显卡所支持的图形API版本: " + SystemInfo.graphicsDeviceVersion);
        sb.AppendLine("图形设备着色器性能级别: " + SystemInfo.graphicsShaderLevel);
        sb.AppendLine("是否支持内置阴影: " + SystemInfo.supportsShadows);
        sb.AppendLine("设备唯一标识符: " + SystemInfo.deviceUniqueIdentifier);
        sb.AppendLine("用户定义的设备的名称: " + SystemInfo.deviceName);
        sb.AppendLine("设备的模型: " + SystemInfo.deviceModel);
        sysinfo = sb.ToString();
    }

    private void OnGUI()
    {
        windowRect.width = 150;
        windowRect.height = 25;
        //宽度和高度根据内容自适应
        windowRect = GUILayout.Window(0, windowRect, DrawWindow, "<color=yellow>Statistics</color>", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
    }

    private void DrawWindow(int windowID)
    {
        GUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
        DrawFPS();
        if (IsShowMem)
            DrawMem();
        if (IsShowScreen)
            DrawScreen();
        if (IsShowSysInfo)
            DrawSysInfo();
        DrawBottom();
        GUILayout.EndVertical();
        GUI.DragWindow();
    }

    private void DrawBottom()
    {
        GUILayout.BeginHorizontal();
        IsShowMem = GUILayout.Toggle(IsShowMem, "内存信息");
        IsShowScreen = GUILayout.Toggle(IsShowScreen, "屏幕信息");
        IsShowSysInfo = GUILayout.Toggle(IsShowSysInfo, "设备信息");
        GUILayout.EndHorizontal();
    }

    private void DrawFPS()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("FPS: " + (int)(1 / Time.deltaTime), fpsStyle);
        if (Profiler.supported)
        {
            long TotalAllocatedMemory = Profiler.GetTotalAllocatedMemoryLong() / 1024 / 1024; //"MB"
            int systemMemorySize = SystemInfo.systemMemorySize;
            GUILayout.Label("Mem: " + TotalAllocatedMemory + "/" + systemMemorySize + "MB", fpsStyle);
        }
        GUILayout.EndHorizontal();
    }

    private void DrawMem()
    {
        if (Profiler.supported)
        {
            long TotalReservedMemory = Profiler.GetTotalReservedMemoryLong() / 1024 / 1024; //"MB"
            long TotalAllocatedMemory = Profiler.GetTotalAllocatedMemoryLong() / 1024 / 1024; //"MB"
            long TotalUnusedReservedMemory = Profiler.GetTotalUnusedReservedMemoryLong() / 1024 / 1024; //"MB"
            long MonoHeapSize = Profiler.GetMonoHeapSizeLong() / 1024 / 1024; //"MB"
            long MonoUsedSize = Profiler.GetMonoUsedSizeLong() / 1024 / 1024; //"MB"
            long TempAllocatorSize = Profiler.GetTempAllocatorSize() / 1024 / 1024; //"MB"

            GUILayout.Label("TotalReservedMemory: " + TotalReservedMemory + "MB", memStyle);
            GUILayout.Label("TotalAllocatedMemory: " + TotalAllocatedMemory + "MB", memStyle);
            GUILayout.Label("TotalUnusedReservedMemory: " + TotalUnusedReservedMemory + "MB", memStyle);
            GUILayout.Label("MonoHeapSize: " + MonoHeapSize + "MB", memStyle);
            GUILayout.Label("MonoUsedSize: " + MonoUsedSize + "MB", memStyle);
            GUILayout.Label("TempAllocatorSize: " + TempAllocatorSize + "MB", memStyle);
        }
    }

    private void DrawScreen()
    {
        Resolution resolution = Screen.currentResolution;
        GUILayout.Label("Resolution: " + resolution.ToString(), labelStyle);
        GUILayout.Label("Screen: " + Screen.width + "x" + Screen.height, labelStyle);
        GUILayout.Label("dpi: " + Screen.dpi, labelStyle);
        GUILayout.Label("sleepTimeout: " + Screen.sleepTimeout, labelStyle);
    }

    private void DrawSysInfo()
    {
        GUILayout.Box(sysinfo, sysStyle);
    }
}