using System;
using UnityEngine;

namespace HomaGames.Internal.DataBank.Samples
{
    [CreateAssetMenu(fileName = nameof(CustomData), menuName = nameof(DataBank) + "/Data/" + nameof(CustomData))]
    public class CustomData : DataBankTyped<MyAwesomeData>
    {

    }
    [Serializable]
    public struct MyAwesomeData
    {
        public string hello;
        public bool toggle;
        public override string ToString()
        {
            return $"{hello} : {toggle}";
        }
    }
}