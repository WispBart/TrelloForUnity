using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Wispfire.TrelloForUnity
{
    public class DeleteTrelloObjectTask<T> : TrelloTask
    {

        public DeleteTrelloObjectTask(string URL)
        {
            _internalRoutine = deleteTrelloObjectRoutine(URL);
        }

        private IEnumerator deleteTrelloObjectRoutine(string URL)
        {
            UnityWebRequest deleteObject = UnityWebRequest.Delete(URL);
#if UNITY_2017_2_OR_NEWER
            yield return deleteObject.SendWebRequest();
#else
            yield return deleteObject.Send();
#endif

            if (!string.IsNullOrEmpty(deleteObject.error))
            {
                Debug.Log(deleteObject.error);
                if (!string.IsNullOrEmpty(deleteObject.downloadHandler.text))
                {
                    error += "\nServer returned: " + deleteObject.downloadHandler.text;
                }
                yield break;
            }
        }
    }

    public class DeleteTrelloCardTask : DeleteTrelloObjectTask<Card>
    {
        static string DeleteCardURL(string cardID) { return baseURL + "/cards/" + cardID; }

        public DeleteTrelloCardTask(string cardID, string authstring) : base(DeleteCardURL(cardID) + authstring) { }
    }
}
