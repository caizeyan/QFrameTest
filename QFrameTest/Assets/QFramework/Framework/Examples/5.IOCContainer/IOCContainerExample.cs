using UnityEngine;

namespace QFramework.Example
{
    public class IOCContainerExample : MonoBehaviour
    {
        public class SomeService
        {
            public void Say()
            {
                Debug.LogError("???");
            }
        }
        
        public interface INetworkService
        {
            void Connet();
        }
        
        public class NetworkService:INetworkService
        {
            public void Connet()
            {
                Debug.LogError("net connet");
            }
        }

        private void Start()
        {
            var container = new IOCContainer();
            container.Register(new SomeService());
            container.Register<INetworkService>(new NetworkService());
            
            container.Get<SomeService>().Say();
            container.Get<INetworkService>().Connet();
        }
    }
}
