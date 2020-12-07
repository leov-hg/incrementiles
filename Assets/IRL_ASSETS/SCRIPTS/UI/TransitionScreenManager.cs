using HomaGames.Internal.GameState;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TransitionScreenManager : MonoBehaviour
{
    [SerializeField] private StateMachineDescription stateMachine;

    [Separator("General Settings", true)]
    [SerializeField] private bool outOnAwake = false;
    [SerializeField] private TransitionScreen transitionScreen;

    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;

        if (transitionScreen == null)
        {
            transitionScreen = GetComponentInChildren<TransitionScreen>();
        }

        transitionScreen.Init();
    }

    public void TriggerTransition()
    {
        canvas.enabled = true;
        transitionScreen.gameObject.SetActive(true);
        StartCoroutine(TransitionCoroutine());
    }
    public void TriggerTransition(Action optionalFunction)
    {
        canvas.enabled = true;
        transitionScreen.gameObject.SetActive(true);
        StartCoroutine(TransitionCoroutine(optionalFunction));
    }
    public void TriggerTransition(HomaGames.Internal.DataBank.Event optionalEvent)
    {
        canvas.enabled = true;
        transitionScreen.gameObject.SetActive(true);
        StartCoroutine(TransitionCoroutine(optionalEvent));
    }
    public void TriggerTransition(State optionalState)
    {
        canvas.enabled = true;
        transitionScreen.gameObject.SetActive(true);
        StartCoroutine(TransitionCoroutine(optionalState));
    }

    private IEnumerator TransitionCoroutine()
    {        
        yield return new WaitForSeconds(transitionScreen.StartIn());

        yield return new WaitForSeconds(transitionScreen.StartOut());
        transitionScreen.gameObject.SetActive(false);
    }
    private IEnumerator TransitionCoroutine(Action optionalFunction)
    {
        yield return new WaitForSeconds(transitionScreen.StartIn());

        optionalFunction();

        yield return new WaitForSeconds(transitionScreen.StartOut());
        transitionScreen.gameObject.SetActive(false);
    }
    private IEnumerator TransitionCoroutine(HomaGames.Internal.DataBank.Event optionalEvent)
    {
        yield return new WaitForSeconds(transitionScreen.StartIn());

        optionalEvent?.Invoke();

        yield return new WaitForSeconds(transitionScreen.StartOut());
        transitionScreen.gameObject.SetActive(false);
    }
    private IEnumerator TransitionCoroutine(State optionalState)
    {
        yield return new WaitForSeconds(transitionScreen.StartIn());

        stateMachine.StartTransition(optionalState);

        yield return new WaitForSeconds(transitionScreen.StartOut());
        transitionScreen.gameObject.SetActive(false);
    }
}
