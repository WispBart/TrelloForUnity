using System.Collections;
using UnityEngine;

namespace Wispfire.BugReporting
{
    [RequireComponent(
        typeof(SessionLogger),
        typeof(BugReportToTrelloCard)
        )]
    public class BugReporter : MonoBehaviour
    {
        public BugReporterGUIController ReportInterface;
        public KeyCode ToggleUIHotKey = KeyCode.F8;

        private Texture2D cachedScreenshot;
        private SessionLogger sessionLog;
        private BugReportToTrelloCard client;

        void Start()
        {
            sessionLog = GetComponent<SessionLogger>();
            client = GetComponent<BugReportToTrelloCard>();

            if (ReportInterface != null)
            {
                ReportInterface.OnSubmit.AddListener(OnBugSubmit);
            }
        }

        void Update()
        {
            if (Input.GetKeyUp(ToggleUIHotKey))
            {
                ReportInterface.SetMenuState(!ReportInterface.MenuState);
            }
        }


        void OnBugSubmit(BugReportUserData data)
        {
            StartCoroutine(generateBugReport(data));
        }

        IEnumerator generateBugReport(BugReportUserData data)
        {
            var grabScreenshot = StartCoroutine(screenGrabAndCache());
            yield return grabScreenshot;

            Debug.Log("Submitting bug report.");

            string version = Application.version; // This gets the version number from Project Settings->Player under Other Settings.
           
            var report = BugReport.CreateInstance<BugReport>();
            report.SetData(
                data,
                "",
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
                version,
                Application.platform);
            report.AddScreenshot("screenshot" + System.DateTime.UtcNow.ToShortTimeString(), cachedScreenshot);
            report.AddTextAttachment("SystemInfo", SystemInformation.GetDebugSystemInfo(), "txt");
            report.AddTextAttachment("SessionInfo", sessionLog.PrintLog(), "txt");

            Send(report);
        }

        public void Send(BugReport report)
        {
            client.HandleBugReport(report, OnBugReportSent);
        }

        void OnBugReportSent()
        {
            Debug.Log("Bug report sent!");
        }
 

        IEnumerator screenGrabAndCache()
        {
            yield return new WaitForEndOfFrame();
            // Create a texture the size of the screen, RGB24 format
            int width = Screen.width;
            int height = Screen.height;
            var tex = new Texture2D(width, height, TextureFormat.RGB24, false);

            // Read screen contents into the texture
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();
            cachedScreenshot = tex;
        }

    }
    public struct BugReportUserData
    {
        public string Title;
        public string Description;
        public string Email;

        public BugReportUserData(string title, string description, string email)
        {
            Title = title;
            Description = description;
            Email = email;
        }
    }
}
