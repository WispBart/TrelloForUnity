using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Wispfire.TrelloForUnity
{
    public class GetTrelloObjectTask<T> : TrelloTask where T : ITrelloObject
    {
        public T Result;

        public GetTrelloObjectTask(string SourceURL)
        {
            _internalRoutine = getTrelloObjectRoutine(SourceURL);
        }

        private IEnumerator getTrelloObjectRoutine(string URL)
        {
            UnityWebRequest getObject = UnityWebRequest.Get(URL);
            
#if UNITY_2017_2_OR_NEWER
            yield return getObject.SendWebRequest();
#else
            yield return getObject.Send();
#endif

            if (!string.IsNullOrEmpty(getObject.error))
            {
                error = getObject.error;
                if (!string.IsNullOrEmpty(getObject.downloadHandler.text))
                {
                    error += "\nServer returned: " + getObject.downloadHandler.text;
                }
                yield break;
            }
            else if (string.IsNullOrEmpty(getObject.downloadHandler.text))
            {
                error = ErrorStrings.GETReturnedNullOrEmpty;
            }
            else
            {
                try
                {
                    Result = JsonUtility.FromJson<T>(getObject.downloadHandler.text);
                    if (Result == null) { error = ErrorStrings.UnknownError; }
                }
                catch(ArgumentException)
                {
                    error = ErrorStrings.GETReturnsInvalidJSON;
                }
            }
        }
    }

    public class GetTrelloMemberTask : GetTrelloObjectTask<Member>
    {
        static string getMemberURL = baseURL + "/members/";
        static string options = "";
        public GetTrelloMemberTask(string memberID, string authString) : base(getMemberURL + memberID + authString + options) { }

    }

    public class GetTrelloBoardTask : GetTrelloObjectTask<Board>
    {
        static string getBoardURL = baseURL + "/boards/";
        static string options = "&lists=open&list_fields=name&fields=name,desc";

        public GetTrelloBoardTask(string boardID, string authString) : base(getBoardURL + boardID + authString + options) { }
    }

    public class GetTrelloListTask : GetTrelloObjectTask<TrelloList>
    {
        static string getListURL = baseURL + "/lists/";
        static string options = "&cards=open&list_fields=name";

        public GetTrelloListTask(string listID, string authString) : base(getListURL + listID + authString + options) { }
    }

    public class GetTrelloCardTask : GetTrelloObjectTask<Card>
    {
        static string getCardURL = baseURL + "/cards/";
        static string options = "";

        public GetTrelloCardTask(string cardID, string authString) : base(getCardURL + cardID + authString + options) { }
    }
}