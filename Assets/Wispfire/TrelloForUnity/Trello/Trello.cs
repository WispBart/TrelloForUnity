using System;
using System.Collections;
using UnityEngine;

namespace Wispfire.TrelloForUnity
{
    [ExecuteInEditMode]
    public class Trello : MonoBehaviour
    {

        [SerializeField]
        private TrelloAuthenticator authenticator;

        string authString()
        {
            if (authenticator == null) { Debug.LogError("Autenticator not found!"); return string.Empty; }
            return authenticator.GetAuthString();
        }


        public GetTrelloMemberTask GetMember(string memberID)
        {
            return new GetTrelloMemberTask(memberID, authString());
        }

        public GetTrelloBoardTask GetBoard(string boardID)
        {
            return new GetTrelloBoardTask(boardID, authString());
        }

        public GetTrelloListTask GetList(string listID)
        {
            return new GetTrelloListTask(listID, authString());
        }

        public GetTrelloCardTask GetCard(string cardID)
        {
            return new GetTrelloCardTask(cardID, authString());
        }
#region Create
        /// <summary>
        /// Starts a create trello card task and returns a taskobject that can be yielded to.
        /// </summary>
        /// <param name="listId">The list this card will be created in.</param>
        /// <param name="title">The title of the card.</param>
        /// <param name="description">The description.</param>
        /// <returns>Returns a CreateTrelloCardTask object, which can be used as a coroutine, similar to a UnityWebRequest.</returns>
        public CreateTrelloCardTask CreateTrelloCard(string listId, string title, string description)
        {
            return new CreateTrelloCardTask(listId, title, description, authString());
        }

        /// <summary>
        /// Starts a create a trello card task with an onComplete callback.
        /// </summary>
        /// <param name="listId">The list this card will be created in.</param>
        /// <param name="title">The title of the card.</param>
        /// <param name="description">The description.</param>
        /// <param name="onComplete">The callback function that is executed when the card has been created.</param>
        public void CreateTrelloCard(string listId, string title, string description, Action<Card> onComplete)
        {
            var task = new CreateTrelloCardTask(listId, title, description, authString());
            StartCoroutine(createTrelloObject(task, onComplete));
        }

        /// <summary>
        /// Add an attachment to an existing card.
        /// </summary>
        /// <param name="cardID">You can use an existing Card ID, or get the Card ID from the CreateTrelloCardTask once it has returned.</param>
        /// <param name="name">The file name. Trello appears to use the extension to figure out what type of file it is.</param>
        /// <param name="file">The attachment as bytes[].</param>
        /// <returns>Returns a CreateAttachmentTask object, which can be used as a coroutine, similar to a UnityWebRequest.</returns>
        public CreateAttachmentTask AddAttachmentToCard(string cardID, string name, byte[] file)
        {
            return new CreateAttachmentTask(cardID, name, file, authString());
        }
        #endregion

#region Delete
        public DeleteTrelloCardTask DeleteTrelloCard(string cardID)
        {
            return new DeleteTrelloCardTask(cardID, authString());
        }

        public void DeleteTrelloCard(string cardID, Action onComplete = null)
        {
            var task = new DeleteTrelloCardTask(cardID, authString());
            StartCoroutine(deleteTrelloObject(task, onComplete));
        }
#endregion

        IEnumerator getTrelloObject<T>(GetTrelloObjectTask<T> task, Action<T> onComplete) where T : ITrelloObject
        {
            yield return task; // wait for GetTask to complete
            if (!string.IsNullOrEmpty(task.error)) { Debug.Log(string.Format("Error getting {0}: \"{1}\"", typeof(T).Name, task.error)); }
            onComplete(task.Result);
        }
        IEnumerator createTrelloObject<T>(CreateTrelloObjectTask<T> task, Action<T> onComplete) where T : ITrelloObject
        {
            yield return task;
            if (!string.IsNullOrEmpty(task.error)) { Debug.Log(string.Format("Error creating {0}: \"{1}\"", typeof(T).Name, task.error)); }
            onComplete(task.Result);
        }
        IEnumerator deleteTrelloObject<T>(DeleteTrelloObjectTask<T> task, Action onComplete = null) where T : ITrelloObject
        {
            yield return task;
            if (!string.IsNullOrEmpty(task.error)) { Debug.Log(string.Format("Error deleting {0}: \"{1}\"", typeof(T).Name, task.error)); }
            if (onComplete != null) onComplete();
        }
    }
}
