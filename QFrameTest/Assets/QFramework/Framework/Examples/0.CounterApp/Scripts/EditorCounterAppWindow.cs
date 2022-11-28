#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace QFramework.Example
{
    public class EditorCounterAppWindow :EditorWindow,IController    
    {
        [MenuItem("MyQFrame/EditorCounterAppWindow")]
        static void Open()
        {
            GetWindow<EditorCounterAppWindow>();
        }

        private ICounterAppModel model;

        private void OnEnable()
        {
            model = this.GetModel<ICounterAppModel>();
        }

        private void OnDisable()
        {
            model = null;
        }

        private void OnGUI()
        {
            if (GUILayout.Button("+"))
            {
                this.SendCommand<IncreaseCountCommand>();
            }
            GUILayout.Label(model.Count.Value.ToString());
            if (GUILayout.Button("-"))
            {
                this.SendCommand<DecreaseCountCommand>();
            }
        }
        

        public IArchitecture GetArchitecture()
        {
            return CounterApp.Interface;
        }
    }
}

#endif