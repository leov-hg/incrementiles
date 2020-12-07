using UnityEngine;

namespace HomaGames.Internal.DataBank.Samples
{
    [CreateAssetMenu(fileName = nameof(CustomDataEvent), menuName = nameof(DataBank) + "/" + nameof(Event) + "/" + nameof(CustomDataEvent))]
    public class CustomDataEvent : Event<MyAwesomeData>
    {

    }
}
