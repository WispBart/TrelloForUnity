using UnityEngine;

namespace Wispfire.TrelloForUnity
{
    [CreateAssetMenu(fileName = "TrelloConnectionData", menuName = "Wispfire/Trello Connection Data")]
    public class TrelloConnectionData : TrelloAuthenticator
    {
        [SerializeField]
        private string APIKey;
        [SerializeField]
        private string AuthToken;
        [SerializeField]
        private string ApplicationName;


        public override string GetAuthString()
        {
            return string.Format("?key={0}&token={1}", APIKey, AuthToken);
        }

        public string GetAppName() { return ApplicationName; }
    }
}
