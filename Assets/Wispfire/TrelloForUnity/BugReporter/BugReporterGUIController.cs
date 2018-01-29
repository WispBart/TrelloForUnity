using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Wispfire.BugReporting
{
    public class BugReporterGUIController : MonoBehaviour
    {
        [System.Serializable]
        public class BugReportSendEvent : UnityEvent<BugReportUserData> { };

        public InputField Title;
        public InputField Description;
        public InputField Email;

        [SerializeField]
        public BugReportSendEvent OnSubmit = new BugReportSendEvent();

        public bool MenuState { get; private set; }
        /// <summary>
        /// Turn menu on or off.
        /// </summary>
        /// <param name="state"></param>
        public void SetMenuState(bool state)
        {
            MenuState = state;
            gameObject.SetActive(state);
            AudioListener.pause = state;
            Time.timeScale = state ? 0f : 1f;
            if (state)
            {
                //EventSystem.current.SetSelectedGameObject(Title.gameObject);
            }
        }

        public void Clear()
        {
            Title.text = "";
            Description.text = "";
            Email.text = "";
        }

        public void Submit()
        {
            OnSubmit.Invoke(new BugReportUserData(Title.text, Description.text, Email.text));
            SetMenuState(false);
            Clear();
        }
    }
}
