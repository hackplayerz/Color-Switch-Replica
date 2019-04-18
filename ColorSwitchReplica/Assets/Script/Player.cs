using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    enum ColorData
    {
        Cyan = 0,
        Yellow = 1,
        Magenta = 2,
        Purple = 3
    }
    
    #region Variable
    
    [Header("Player Setting")]
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private ColorData _nowColor;
    
    [Header("Particle Prefab")]
    [SerializeField] private GameObject _pointParticlePrefab;
    [SerializeField] private GameObject _dieParticlePrefab;

    [Header("Sound FX")]
    [SerializeField] private AudioClip _pointClip;


    private AudioSource _audioSource;
    private readonly List<GameObject> _pointParticles = new List<GameObject>();
    private int _pointCount;
    
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _renderer;
    private Vector3 _lastPosition = Vector3.down * 10;

    #endregion
    
    #region Unity Function

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _renderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX;
        _rigidbody2D.freezeRotation = true;
        if(_dieParticlePrefab == null)
            _dieParticlePrefab = Resources.Load<GameObject>("DieEffect");
        if (_pointParticlePrefab == null)
            _pointParticlePrefab = Resources.Load<GameObject>("PointEffect");
        
    }

    private void Update()
    {
        if (GameManager.Instance.State.Equals(GameManager.PlayState.Gaming))
        {
            if (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0))
            {
                _rigidbody2D.velocity = Vector2.up * _jumpForce;
            }
        }
        if (transform.position.y < _lastPosition.y - 10)
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Point"))
        {
            _lastPosition = transform.position;
            ChangeColorRandom();
            other.gameObject.SetActive(false);
            MapGenerator.Instance.Pop();
            UIManager.Instance.ScoreUp();
            CreatePointEffect(other.transform.position);
            _audioSource.PlayOneShot(_pointClip);
        }
        else if (other.tag.Equals("Border"))
        {
            if (!other.name.Equals(_nowColor.ToString()))
            {
                // Game over
                Die();
            }
        }
    }

    #endregion

    #region Effect Object Pool
    
    /// <summary>
    /// Initialize Point Effect Pool
    /// </summary>
    void InitPointPool()
    {
        GameObject efx = Instantiate(_pointParticlePrefab);
        _pointParticles.Add(efx);
        efx.SetActive(false);
    }

    /// <summary>
    /// Create Point Effect at spawnPosition
    /// </summary>
    /// <param name="spawnPosition">To Spawn Position</param>
    void CreatePointEffect(Vector3 spawnPosition)
    {
        if (_pointCount > _pointParticles.Count - 1)
        {
            InitPointPool();
        }
        else
        {
            _pointCount = 0;
        }

        _pointParticles[_pointCount].transform.position = spawnPosition;
        _pointParticles[_pointCount].SetActive(true);
        StartCoroutine(DeleteEffect(_pointParticles[_pointCount],1));
        _pointCount++;
    }

    IEnumerator DeleteEffect(GameObject effect, float remainTime)
    {
        yield return new WaitForSeconds(remainTime);
        effect.SetActive(false);
    }
   
    #endregion
    
    
    
    /// <summary>
    /// Player die
    /// </summary>
    private void Die()
    {
        var stopPosition = transform.position;
        Instantiate(_dieParticlePrefab, stopPosition, Quaternion.identity);
        GameManager.Instance.State = GameManager.PlayState.GameOver;
        
        gameObject.SetActive(false);
    }
    
    /// <summary>
    /// Change player color random
    /// </summary>
    void ChangeColorRandom()
    {
        var rand = Random.Range(0, 4);
        switch (rand)
        {
            case (int) ColorData.Cyan:
                _renderer.color = Color.cyan;
                _nowColor = ColorData.Cyan;
                break;
            case (int) ColorData.Yellow:
                _renderer.color = Color.yellow;
                _nowColor = ColorData.Yellow;
                break;
            case (int) ColorData.Magenta:
                _renderer.color = Color.magenta;
                _nowColor = ColorData.Magenta;
                break;
            case (int) ColorData.Purple:
                _renderer.color = new Color32(121, 0, 255, 255);
                _nowColor = ColorData.Purple;
                break;
        }
    }
}
