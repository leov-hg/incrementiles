using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class MoneyFeedBack : MonoBehaviour
{
    #region Configuration Variables
    [Separator("Settings", true)]
    [SerializeField] private GameObject cashGain;
    [SerializeField] private ParticleSystem coinFX;
    [SerializeField] private int poolCount = 1;
    [SerializeField] private bool worldSpace = false;
    #endregion

    #region Private Variables
    private List<GameObject> cashGainGOs = new List<GameObject>();
    private List<Animation> cashGainAnimators = new List<Animation>();
    private List<Transform> cashGainTransforms = new List<Transform>();
    private List<TextMeshProUGUI> cashGainTexts = new List<TextMeshProUGUI>();

    private int cashGainsIndex = 0;
    private WaitForSeconds delay = new WaitForSeconds(1);
    private ParticleSystem.MainModule coinFX_main;
    #endregion


    private void Awake()
    {
        for (int i = 0; i < poolCount; i++)
        {
            cashGainGOs.Add(Instantiate(cashGain, transform.position, transform.rotation, transform));
            cashGainGOs[i].SetActive(false);
            cashGainAnimators.Add(cashGainGOs[i].GetComponent<Animation>());
            cashGainTransforms.Add(cashGainGOs[i].transform);
            cashGainTexts.Add(cashGainGOs[i].GetComponentInChildren<TextMeshProUGUI>());
        }

        coinFX_main = coinFX.main;
        if (worldSpace)
            coinFX_main.simulationSpace = ParticleSystemSimulationSpace.World;
        else
            coinFX_main.simulationSpace = ParticleSystemSimulationSpace.Local;
    }

    //public void FilsDePuteEncule()
    //{
    //    Debug.Log(cashGainGOs.Count);  Et oui... Il était une fois, Liam, qui ne linka pas ses objets, et qui perdi 1h15 de sa journée... c'est triste...
    //}

    public void ShowFeedback(float cash)
    {
        cashGainGOs[cashGainsIndex].SetActive(true);
        cashGainTexts[cashGainsIndex].text = UnitConversion.CashUnit(cash);

        if (worldSpace)
        {
            cashGainTransforms[cashGainsIndex].position = transform.position;
            cashGainTransforms[cashGainsIndex].SetParent(null);
            StartCoroutine(ResetParent(cashGainsIndex));
        }

        cashGainAnimators[cashGainsIndex].Play();

        coinFX.gameObject.SetActive(true);
        coinFX.Play();

        cashGainsIndex++;
        if (cashGainsIndex == cashGainGOs.Count)
        {
            cashGainsIndex = 0;
        }
    }

    private IEnumerator ResetParent(int index)
    {
        yield return delay;

        cashGainGOs[index].gameObject.SetActive(false);

        if (!coinFX.IsAlive(true))
        {
            coinFX.gameObject.SetActive(false);
        }
    }
}
