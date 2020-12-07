using HomaGames.Internal.DataBank.BasicTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HomaGames.Internal.DataBank.Samples
{
    public class BasicSample : MonoBehaviour
    {
        private void Start()
        {
            SimpleDataSample();
            SimpleEventsSample();
            SimpleServiceSample();
        }

        private void SimpleEventsSample()
        {
            // Register a simple event
            EventProvider.RegisterEvent("MyAwesomeEventName", () => Debug.Log("hello hello"));
            // Register a float event
            EventProvider<float>.RegisterEvent("MyAwesomeFloatEventName", (f) => Debug.Log("My awesome float is " + f));
            // Invoke the simple event
            EventProvider.Invoke("MyAwesomeEventName");
            // Invoke the float event
            EventProvider<float>.Invoke("MyAwesomeFloatEventName", 42);
        }

        private void SimpleDataSample()
        {
            DataProvider<float>.SetValue("myAwesomeValue", 42);
            DataProvider<float>.GetValue("myAwesomeValue");
            DataProvider<float>.RegisterOnValueChange("myAwesomeValue",
                (f) => Debug.Log("myAwesomeValue changed to " + f));
            DataProvider<float>.SetValue("myAwesomeValue", 41);
        }

        private void SimpleServiceSample()
        {
            ServiceProvider<MyAwesomeService>.Set(new MyAwesomeService());
            ServiceProvider<MyAwesomeService>.Value.SomeFunction();
        }

        private class MyAwesomeService : IService
        {
            public void SomeFunction()
            {
                Debug.Log("Hello from my service !");
            }
        }
    }
}