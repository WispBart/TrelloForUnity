using UnityEngine;

namespace Wispfire.TrelloForUnity
{
    public abstract class TrelloAuthenticator : ScriptableObject
    {
        public abstract string GetAuthString();
    }
}

