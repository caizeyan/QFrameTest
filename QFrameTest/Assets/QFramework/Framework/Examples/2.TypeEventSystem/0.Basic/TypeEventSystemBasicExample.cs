using System;
using UnityEngine;

namespace QFramework.Example
{
    public class TypeEventSystemBasicExample : MonoBehaviour
    {
        public struct TestEventA
        {
            public int Age;
        }

        private void Start()
        {
            TypeEventSystem.Global.Register<TestEventA>(e =>
            {
                Debug.Log(e.Age);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                TypeEventSystem.Global.Send(new TestEventA(){Age = 18});
            }

            if (Input.GetMouseButtonDown(1))
            {
                TypeEventSystem.Global.Send(new TestEventA());
            }
        }
    }
}