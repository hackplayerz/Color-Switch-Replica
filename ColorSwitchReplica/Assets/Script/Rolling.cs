using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rolling : MonoBehaviour
{
    [SerializeField] private float _speed = 40f;
    private Transform _sliceTransform;
    [SerializeField] private Transform _pointTransform;

    private void Awake()
    {
        _sliceTransform = GetComponentsInChildren<Transform>()[1];
    }

    private void OnEnable()
    {
        StartCoroutine(Roll());
    }
    
    /// <summary>
    /// Change Rolling object's setting
    /// </summary>
    /// <returns>Changed random value</returns>
    public float Change()
    {
        float random = Random.Range(.7f, 1.6f);
        
        _speed = Random.Range(40f, 150f);
        _sliceTransform.localScale = new Vector3(random,random,1);

        return random;
    }

    public void EnablePoint()
    {
        _pointTransform.gameObject.SetActive(true);
    }

    IEnumerator Roll()
    {
        while (enabled)
        {
            _sliceTransform.Rotate(Vector3.forward * _speed * Time.deltaTime);
            
            yield return null;
        }
    }
}
