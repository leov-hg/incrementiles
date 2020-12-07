using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace FM{
    public class RateBoxUI : MonoBehaviour {

        [SerializeField] Animator showAnimator;

        [SerializeField] TextMeshProUGUI titleText;
        [SerializeField] TextMeshProUGUI messageText;
        [SerializeField] GameObject optOutButton;
        [SerializeField] TextMeshProUGUI optOutText;
        [SerializeField] GameObject laterButton;
        [SerializeField] TextMeshProUGUI laterText;
        [SerializeField] Button rateButton;
        [SerializeField] TextMeshProUGUI rateText;
        [SerializeField] Image[] stars;
        [SerializeField] Color emptyColor;
        [SerializeField] Color filledColor;

        int rating = 0;

        public void SetUIText(string title, string message, string optOut, string later, string rate){
            titleText.text = title;
            messageText.text = message;
            optOutText.text = optOut;
            laterText.text = later;
            rateText.text = rate;

            rateButton.interactable = false;

            if(optOut == ""){
                optOutButton.SetActive(false);
            }
            if(later == ""){
                laterButton.SetActive(false);
            }
        }
        
        public void Rate(){
            showAnimator.SetTrigger("Hide");
            RateBox.Rate(rating);
        }

        public void Later(){
            showAnimator.SetTrigger("Hide");
            RateBox.Later();
        }

        public void OptOut(){
            showAnimator.SetTrigger("Hide");
            RateBox.OptOut();
        }

        public void Die(){
            Destroy(gameObject);
        }

        public void Stars(int _rating){
            rating = _rating;
            if(_rating > 0){
                rateButton.interactable = true;
            }else{
                rateButton.interactable = false;
            }
            for(int i=0; i<rating; i++){
                stars[i].color = filledColor;
            }
            for(int i=rating; i<stars.Length; i++){
                stars[i].color = emptyColor;
            }
        }

    }
}