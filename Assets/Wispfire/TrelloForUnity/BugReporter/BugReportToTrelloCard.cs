using System;
using System.Collections;
using UnityEngine;
using Wispfire.TrelloForUnity;

namespace Wispfire.BugReporting
{
    public class BugReportToTrelloCard : MonoBehaviour
    {
        public Trello Client;

        [SerializeField]
        private string BugReportListID;

        public void HandleBugReport(BugReport report, Action OnDone)
        {
            StartCoroutine(handleBugReport(report, OnDone, BugReportListID));
        }

        IEnumerator handleBugReport(BugReport report, Action OnDone, string targetList)
        {
            // Turn bug report info into card
            string title = string.Empty;
            title += "(" + report.SceneName + ") ";
            title += report.Title;
            title += " #" + removeWhiteSpace(report.Version);
            title += " #" + removeWhiteSpace(report.Platform);

            string description = string.Empty;
            description += "Username: " + report.Username + "\n";
            description += "E-Mail: " + report.Email + "\n";
            description += "\n";
            description += "Report: \n" + report.Description + "\n";

            //create card
            var CreateCard = Client.CreateTrelloCard(targetList, title, description);
            yield return CreateCard;

            if (!string.IsNullOrEmpty(CreateCard.error))
            {
                Debug.LogError("Creating trello card failed with error: " + CreateCard.error);
                yield break;
            }

            // Add attachments
            for (int i = 0; i < report.TextFiles.Count; i++)
            {
                var addText = Client.AddAttachmentToCard(CreateCard.Result.id, report.TextFiles[i].GetFilename(), report.TextFiles[i].GetBytes());
                yield return addText;
            }
            for (int i = 0; i < report.Screenshots.Count; i++)
            {
                var addScreenshot = Client.AddAttachmentToCard(CreateCard.Result.id, report.Screenshots[i].GetFilename(), report.Screenshots[i].GetBytes());
                yield return addScreenshot;
            }
            if (OnDone != null) { OnDone(); }
        }

        string removeWhiteSpace(string source)
        {
            return source.Replace(" ", "");
        }
    }
}

