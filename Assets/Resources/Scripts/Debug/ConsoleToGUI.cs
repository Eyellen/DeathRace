#if DEBUG_BUILD

using UnityEngine;

namespace MyDebug
{
    [DisallowMultipleComponent]
    public class ConsoleToGUI : MonoBehaviour
    {
        public static ConsoleToGUI Instance { get; private set; }

        [SerializeField] private bool _isActive;

        static string myLog = "DEBUG LOG:\n\n";
        private string output;
        private string stack;

        private void Start()
        {
            if (!Instance)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.LogError($"Trying to create another one {nameof(ConsoleToGUI)} on {transform.name} when it is a Singleton. " +
                    $"Destroyed {transform.name} to prevent this.");
                Destroy(gameObject);
            }
        }

        void OnEnable()
        {
            Application.logMessageReceived += Log;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= Log;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                _isActive = !_isActive;
            }
        }

        public void Log(string logString, string stackTrace, LogType type)
        {
            output = $"[{type}] " + logString;
            stack = stackTrace;
            myLog = output + "\n" + stack + "\n" + myLog;
            //if (myLog.Length > 5000)
            //{
            //    myLog = myLog.Substring(0, 4000);
            //}
        }

        void OnGUI()
        {
            if (!_isActive) return;

            {
                myLog = GUI.TextArea(new Rect(0, 0, Screen.width, Screen.height), myLog);
            }
        }
    }
}

#endif