using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Wispfire.TrelloForUnity
{
    public class CreateTrelloObjectTask<T> : TrelloTask where T : ITrelloObject
    {
        public T Result;

        public CreateTrelloObjectTask(string SourceURL, WWWForm data)
        {
            _internalRoutine = PutTrelloObjectRoutine(SourceURL, data);
        }


        private IEnumerator PutTrelloObjectRoutine(string URL, WWWForm data)
        {
            UnityWebRequest sendObject = UnityWebRequest.Post(URL, data);
            sendObject.chunkedTransfer = false;
            
            #if UNITY_2017_2_OR_NEWER
            yield return sendObject.SendWebRequest();
            #else
            yield return sendObject.Send();
            #endif

            if (!string.IsNullOrEmpty(sendObject.error))
            {
                error = sendObject.error;
                if (!string.IsNullOrEmpty(sendObject.downloadHandler.text))
                {
                    error += "\nServer returned: " + sendObject.downloadHandler.text;
                }
                yield break;
            }
            else if (string.IsNullOrEmpty(sendObject.downloadHandler.text))
            {
                error = ErrorStrings.POSTReturnedNullOrEmpty;
            }
            else
            {
                try
                {
                    Result = JsonUtility.FromJson<T>(sendObject.downloadHandler.text);
                    if (Result == null) { error = ErrorStrings.UnknownError; }
                }
                catch
                {
                    error = ErrorStrings.POSTReturnsInvalidJSON;
                }
            }
        }
    }

    public class CreateTrelloCardTask : CreateTrelloObjectTask<Card>
    {
        static string createCardURL = baseURL + "/cards";
        static string options = "";

        public CreateTrelloCardTask(string addToListID, string title, string description, string authstring) 
            : base (createCardURL + authstring + options, 
                  CreateCardForm(addToListID, title, description)) {}

        public static WWWForm CreateCardForm(string targetList, string name, string desc)
        {
            WWWForm data = new WWWForm();
            data.AddField("name", name);
            data.AddField("desc", desc);
            data.AddField("idList", targetList);
            return data;
        }
    }

    public class CreateAttachmentTask : CreateTrelloObjectTask<Attachment>
    {
        static string createAttachmentURL(string cardID) { return baseURL + "/cards/" + cardID + "/attachments"; }
        static string options = "";

        public CreateAttachmentTask(string CardID, string name, byte[] file, string authString, string mimeType = null) 
            : base (createAttachmentURL(CardID) + authString + options, 
                  AddAttachmentForm(name, file, mimeType)) { }

        public static WWWForm AddAttachmentForm(string name, byte[] file, string mimeType = null)
        {
            WWWForm data = new WWWForm();
            data.AddBinaryData("file", file, name, mimeType);
            return data;
        }
    }
}
