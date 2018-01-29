using System.Collections.Generic;
using UnityEngine;
using Wispfire.TrelloForUnity;

namespace Wispfire.BugReporting
{
    public class BugReport : ScriptableObject
    {

        public string Title;
        public string Username;
        public string Email;
        [TextArea(5, 10)]
        public string Description;
        public string SceneName;
        public string Version;
        public string Platform;

        public List<ImageAttachment> Screenshots = new List<ImageAttachment>();
        public List<TextAttachment> TextFiles = new List<TextAttachment>();


        public void SetData(BugReportUserData userdata, string username, string sceneName, string version, RuntimePlatform platform)
        {
            Description = userdata.Description;
            Email = userdata.Email;
            Title = userdata.Title;
            SceneName = sceneName;
            Username = username;
            Version = version;
            Platform = getPlatformString(platform);
        }

        public void AddTextAttachment(string name, string content, string extension)
        {
            TextFiles.Add(new TextAttachment(name, content, extension));
        }

        public void AddScreenshot(string name, Texture2D screenshot)
        {
            Screenshots.Add(new ImageAttachment(name, screenshot, ImageAttachment.TextureEncoding.jpg));
        }

        void OnDestroy()
        {
            for (int i = 0; i < Screenshots.Count; i++)
            {
                Screenshots[i].OnDestroy();
            }
        }

        string getPlatformString(RuntimePlatform platform)
        {
            switch (platform)
            {
                case RuntimePlatform.XboxOne:
                    return "xBoxOne";
                case RuntimePlatform.PS4:
                    return "PS4";
                case RuntimePlatform.OSXDashboardPlayer:
                case RuntimePlatform.OSXPlayer:
                    return "macOS";                  
                case RuntimePlatform.LinuxPlayer:
                    return "Linux";
                case RuntimePlatform.WindowsPlayer:
                    return "Windows";
                case RuntimePlatform.LinuxEditor:
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.WindowsEditor:
                    return "Editor";
                default:
                    return "UnknownPlatform";
            }
        }

    }
}
