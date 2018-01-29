using System;
using System.Collections;
using UnityEngine;

namespace Wispfire.TrelloForUnity
{
    public class TrelloTester : MonoBehaviour
    {
        public Trello TrelloClient;
        public TrelloTestData Data;

        void Start()
        {
            TrelloClient = GetComponent<Trello>();
        }

        public void GetBoards()
        {
            if (TrelloClient != null)
            {
                StartCoroutine(GetMemberAndBoardInfo());
            }
            else
            {
                Debug.LogError("No Trello client detected.");
            }
        }

        IEnumerator GetMemberAndBoardInfo()
        {
            string memberName = "me"; // get 'This' member.
            var getMember = TrelloClient.GetMember(memberName);
            yield return getMember;
            var member = getMember.Result;

            if (getMember.Result == null) { yield break; }

            int items = member.idBoards.Length;
            var boards = new Board[items];

            for (int i = 0; i < items; i++)
            {
                var getBoard = TrelloClient.GetBoard(member.idBoards[i]);
                yield return getBoard;
                boards[i] = getBoard.Result;
                Debug.Log(boards[i].name);
            }
            Data.Boards = boards;
            Data.TrelloUser = member;
        }

        public void CreateCard(string listId, string title, string description)
        {
            TrelloClient.CreateTrelloCard(listId, title, description, onCardCreated);
        }
        
        public void onCardCreated(Card card)
        {
            Debug.Log("Card with id \"" + card.id + "\" created!");
        }
    }

    [Serializable]
    public class TrelloTestData
    {
        public Member TrelloUser;
        public Board[] Boards;
    }
}
