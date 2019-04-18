using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieEffectController : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(RestartProcess());
    }

    IEnumerator RestartProcess()
    {
        yield return UIManager.Instance.OpenGameOver(1f);
        yield return GameManager.Instance.Restart();
    }
}
