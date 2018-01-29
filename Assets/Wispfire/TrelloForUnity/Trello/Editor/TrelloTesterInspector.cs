using System.Collections;
using UnityEngine;
using UnityEditor;
using Wispfire.TrelloForUnity;

[CustomEditor(typeof(TrelloTester))]
public class TrelloTesterInspector : Editor
{
    string deletionListID;
    CreateCard createCard;
    TrelloTester trelloTester;

    private void OnEnable()
    {
        trelloTester = target as TrelloTester;
    }

    public override void OnInspectorGUI()
    {
        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Running the Async actions below when not in play mode can lead to unpredictable results", MessageType.Warning);
        }
        if (GUILayout.Button("Populate Board Data"))
        {
            trelloTester.GetBoards();
        }
        EditorGUILayout.LabelField("Create Card:");
        EditorGUI.indentLevel++;
        createCard.Title = EditorGUILayout.TextField(new GUIContent("Title"), createCard.Title);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Description");
        createCard.Description = EditorGUILayout.TextArea(createCard.Description, GUILayout.ExpandHeight(false), GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * 2));
        EditorGUILayout.EndHorizontal();
        createCard.ListID = EditorGUILayout.TextField(new GUIContent("Add to List ID: "), createCard.ListID);
        EditorGUI.indentLevel--;
        if (GUILayout.Button("Create New Card"))
        {
            if (!string.IsNullOrEmpty(createCard.ListID))
            {
                trelloTester.CreateCard(createCard.ListID, createCard.Title, createCard.Description);
                createCard.Title = "";
                createCard.Description = "";
            }
        }
        deletionListID = EditorGUILayout.TextField(new GUIContent("Delete cards on list:"), deletionListID);
        if (GUILayout.Button("Delete all cards on this list!"))
        {
            trelloTester.StartCoroutine(destroyCardsForList(deletionListID)); // Only using trelloTester to run the Coroutine from
        }

        base.OnInspectorGUI();
    }

    public IEnumerator destroyCardsForList(string listID)
    {
        var getList = trelloTester.TrelloClient.GetList(listID);

        yield return getList;
        var result = getList.Result;

        if (EditorUtility.DisplayDialog("Are you sure you want to do this?", "This will delete all cards on the list '" + result.name + "'!\nAre you sure you want to continue?", "Yes", "Cancel"))
        {
            for (int i = 0; i < result.cards.Length; i++)
            {
                var deletethis = trelloTester.TrelloClient.DeleteTrelloCard(result.cards[i].id);
                yield return deletethis;
            }
            Debug.Log("Operation Finished!");
        }
    }

    public struct CreateCard
    {
        public string ListID;
        public string Title;
        public string Description;
    }
}

