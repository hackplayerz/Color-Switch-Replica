using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    #region Singleton
    
    public static MapGenerator Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    [SerializeField] private Transform _mapTransform;
    
    List<Rolling> _mapContainer = new List<Rolling>();
    private int _count = 0;
    private bool _isCircle;
    private Rolling _circlePrefab;
    private Rolling _squarePrefab;

    
    private void Start()
    {
        _circlePrefab = Resources.Load<Rolling>("Circle").GetComponent<Rolling>();
        _squarePrefab = Resources.Load<Rolling>("Square").GetComponent<Rolling>();
        _isCircle = Random.Range(0, 100)/2  > 25;
        
        for(int i=0;i<10;i++)
            InitMap();
    }

    /// <summary>
    /// Initialize Map
    /// </summary>
    /// <returns>If is circle,true</returns>
    void InitMap()
    {
        if (_isCircle)
        {
            var circle = Instantiate(_circlePrefab);
            _mapContainer.Add(circle);
            circle.transform.SetParent(_mapTransform);
            circle.gameObject.SetActive(false);
        }
        else
        {
            var square = Instantiate(_squarePrefab);
            _mapContainer.Add(square);
            square.transform.SetParent(_mapTransform);
            square.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Pop object Cycle
    /// </summary>
    public void Pop()
    {
        Vector3 beforePosition = Vector3.zero;
        beforePosition = _count == 0 ? new Vector3(0,1,0) : _mapContainer[_count - 1].transform.position;

        if (_count > _mapContainer.Count - 1)
        {
            beforePosition = _mapContainer[_count - 1].transform.position;
            _count         = 0;

        }

        _mapContainer[_count].gameObject.SetActive(true);
        _mapContainer[_count].EnablePoint();
        var rand = _mapContainer[_count].Change();
        _mapContainer[_count].gameObject.transform.position = beforePosition +  Vector3.up * rand * (_isCircle ? 7 : 10);
        _count++;
    }
}
