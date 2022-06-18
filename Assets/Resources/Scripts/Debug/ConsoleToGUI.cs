using UnityEngine;

namespace DebugStuff
{
    public class ConsoleToGUI : MonoBehaviour
    {
        [SerializeField] private bool _isActive;

        //#if !UNITY_EDITOR
        static string myLog = "";
        private string output;
        private string stack;

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
            if (Input.GetKeyDown(KeyCode.Escape))
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

            //if (!Application.isEditor) //Do not display in editor ( or you can use the UNITY_EDITOR macro to also disable the rest)
            {
                myLog = GUI.TextArea(new Rect(10, Screen.height / 2 - 10, Screen.width - 10, Screen.height / 2), myLog);
            }
        }
        //#endif
    }
}