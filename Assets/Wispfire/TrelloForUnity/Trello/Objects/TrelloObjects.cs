using System;

namespace Wispfire.TrelloForUnity
{
    // These are C# representations of json objects from trello.
    public interface ITrelloObject { }

    [Serializable]
    public class TrelloObjectID
    {
        public string id;
        public string name;
    }

    [Serializable]
    public class Member : ITrelloObject
    {
        public string id;
        public string username;
        public string url;
        public string[] idBoards;
    }

    [Serializable]
    public class Board : ITrelloObject
    {
        public string id;
        public string name;
        public string desc;
        public TrelloObjectID[] lists;
    }

    [Serializable]
    public class TrelloList : ITrelloObject
    {
        public string id;
        public string name;
        public TrelloObjectID[] cards;
    }

    [Serializable]
    public class Attachment : ITrelloObject
    {
        public string id;
        public string name;
        public string mimeType;
        public byte[] file;
        public bool Uploaded;
    }

    [System.Serializable]
    public class Card : ITrelloObject
    {
        public string id;
        public string name;
        public string desc;
        public TrelloObjectID[] Attachments;
    }
}
