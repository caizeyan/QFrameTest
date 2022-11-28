using System;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.Example
{
    public class ArchitectureInHeartExample : MonoBehaviour
    {
        #region Framework
        
        public interface ICommand
        {
            void Execute();
        }

        public class BindableProperty<T>
        {
            private T value = default;

            public T Value
            {
                get => value;
                set
                {
                    if (this.value != null && this.value.Equals(value) ) return;
                    this.value = value;
                    OnValueChange?.Invoke(this.value);
                }
            }

            public event Action<T> OnValueChange = obj =>{};
        }

        #endregion 
        
        #region Model

        public static class CounterModel
        {
            public static BindableProperty<int> Counter = new BindableProperty<int>(){Value = 0};
        }
        
        #endregion

        #region Command
        public struct IncreaseCountCommand:ICommand
        {
            public void Execute()
            {
                CounterModel.Counter.Value++;
            }
        }
        
        public struct DecreaseCountCommand:ICommand
        {
            public void Execute()
            {
                CounterModel.Counter.Value--;
            }
        }
        
        #endregion

        private void OnGUI()
        {
            if (GUILayout.Button("+"))
            {
                new IncreaseCountCommand().Execute();
            }
            GUILayout.Label(CounterModel.Counter.Value.ToString());
            if (GUILayout.Button("-"))
            {
                new DecreaseCountCommand().Execute();
            }
        }
    }
}