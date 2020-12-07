using UnityEngine;
using FM;

public class Sample : MonoBehaviour {

    public void IncreaseEvent(){
        RateBox.IncreaseEventCount();
    }

    public void ShowRatePopup(){
        RateBox.Show();
    }
}
