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
            yield return deleteObject.Send();

            if (!string.IsNullOrEmpty(deleteObject.error))
            {
                Debug.Log(deleteObject.error);
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
