using UnityEngine;

namespace HomaGames.Internal.DataBank.Samples
{
    [CreateAssetMenu(fileName = nameof(AudioService), menuName = nameof(ServiceBase) + "/" + nameof(AudioService))]
    public class AudioService : Service<AudioService>
    {
        public float volume;
        public int channels;

        public void SomeAPI()
        {
            Debug.Log("Calling some API in my Audio Service.");
        }

        protected override void OnStart()
        {
            Debug.Log("AudioService Starting ...");
            GameObject g = new GameObject("Some audioservice instantiate");
        }

        protected override void OnUpdate()
        {
            
        }

        protected override void OnFixedUpdate()
        {

        }
    }
}
