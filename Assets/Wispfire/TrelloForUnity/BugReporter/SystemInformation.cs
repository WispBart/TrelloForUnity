using UnityEngine;

namespace Wispfire.BugReporting
{
    public class SystemInformation
    {
        public static string GetDebugSystemInfo()
        {
            var output = string.Empty;
            output += deviceInfo() + "\n";

            output += "Operating System \n";
            output += SystemInfo.operatingSystem + "\n\n";

            output += graphicsDeviceInfo();

            return output;
        }

        public static string deviceInfo()
        {
            var output = string.Empty;
            output += "Device Info: \n";
            //output += SystemInfo.deviceName + "\n";
            output += SystemInfo.deviceModel + "\n";
            output += SystemInfo.deviceType + "\n";
            output += "Hardware threads: " + SystemInfo.processorCount + "\n";
            output += "Clock speed: " + SystemInfo.processorFrequency + "MHz \n";
            output += "System Memory: " + MBtoGB(SystemInfo.systemMemorySize) + " \n";
            return output;
        }

        public static string graphicsDeviceInfo()
        {
            var output = string.Empty;
            output += "Graphics Device Info: \n";
            output += "Device ID: " + SystemInfo.graphicsDeviceID + "\n";
            output += SystemInfo.graphicsDeviceName + "\n";
            output += SystemInfo.graphicsDeviceVersion + "\n";
            output += "Graphics Memory: " + SystemInfo.graphicsMemorySize + "MB \n";
            output += "Shader level: " + ShaderLevelToReadable(SystemInfo.graphicsShaderLevel) + "\n";

            return output;
        }

        //make the shader level more readable
        static string ShaderLevelToReadable(int level)
        {
            switch (level)
            {
                case 20: return "Shader Model 2.x";
                case 30: return "Shader Model 3.0";
                case 40: return "Shader Model 4.0 ( DX10.0 )";
                case 41: return "Shader Model 4.1 ( DX10.1 )";
                case 50: return "Shader Model 5.0 ( DX11.0 )";
                default: return "";
            }
        }

        //unity returns a weird amount of available MB, this unifies it
        static string MBtoGB(int mb)
        {
            return Mathf.Ceil(mb / 1024f).ToString() + "GB";
        }

    }
}

