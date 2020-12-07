using HomaGames.Internal.DataBank.BasicTypes;
using HomaGames.Internal.DataBank.Samples;
using UnityEngine;

namespace HomaGames.Internal.DataBank
{
    public class ScriptableSample : MonoBehaviour
    {
        [Header("Events")]
        public Event Event;
        public FloatEvent FloatEvent;
        [Header("Data")]
        public IntData intData;
        [Header("Custom Data")]
        public CustomData customData;
        public CustomDataEvent customDataEvent;
        [Header("Services")]
        public AudioService audioService;


        void Start()
        {
            Event.OnInvoked += EventOnOnInvoked;
            FloatEvent.OnInvoked += FloatEventOnOnInvoked;
            intData.OnValueChange += IntDataOnValueChange;
            customData.OnValueChange += CustomDataOnValueChange;
            customDataEvent.OnInvoked += CustomDataEventOnInvoked;
            audioService.SomeAPI();
        }

        private void CustomDataEventOnInvoked(MyAwesomeData obj)
        {
            Debug.Log($"My custom data event is invoked with value {obj}");
        }

        private void CustomDataOnValueChange(MyAwesomeData obj)
        {
            Debug.Log($"My awesome data value changed to {obj} !");
        }

        private void IntDataOnValueChange(int obj)
        {
            Debug.Log($"My int data value changed to {obj} !");
        }

        private void FloatEventOnOnInvoked(float obj)
        {
            Debug.Log($"My float event is invoked with value {obj} !");
        }

        private void EventOnOnInvoked()
        {
            Debug.Log("My event is invoked !");
        }
    }
}