using System;
using System.Collections;

namespace Wispfire.TrelloForUnity
{
    public abstract class TrelloTask : IEnumerator
    {
        static protected string baseURL = "https://trello.com/1";
        public string error;

        protected IEnumerator _internalRoutine;

        public object Current
        {
            get
            {
                return _internalRoutine.Current;
            }
        }

        public bool MoveNext()
        {
            return _internalRoutine.MoveNext();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

    }
}
