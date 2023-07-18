using System.Collections;
using AYellowpaper.SerializedCollections;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour {
    [SerializeField] private float _fadeDuration;
    [Space(5)]
    [SerializedDictionary("Level", "Polygon Collider")]
    public SerializedDictionary<int, PolygonCollider2D> levels;

    [Space(5)]
    [Header("Debug")]
    [SerializeField] private int currentLevel;


    private CinemachineConfiner2D _currentConfiner;
    private Transform _playerTransform;
    private Image _fadeImage;

    private bool _isFading;


    private void Awake() {
        currentLevel = 1;
    }


    private void Start() {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _fadeImage = GameObject.FindGameObjectWithTag("FadeImage").GetComponent<Image>();
        _currentConfiner = GameObject.FindGameObjectWithTag("VirtualCam").GetComponent<CinemachineConfiner2D>();
        _currentConfiner.m_BoundingShape2D = levels[currentLevel];


        // Set the initial alpha value to 0 for the fade image
        SetFadeImageAlpha(0f);
    }


    public void MoveToLevel(int i, Vector3 targetDoor) {
        currentLevel = i;
        StartCoroutine(PerformLevelTransition(i, targetDoor));
    }


    private IEnumerator PerformLevelTransition(int i, Vector3 targetDoor) {
        // fade out section
        var fadeTimer = 0f;

        // perform increase of alpha key
        while (fadeTimer < _fadeDuration) {
            fadeTimer += Time.deltaTime;
            var alpha = Mathf.Clamp01(fadeTimer / _fadeDuration);
            SetFadeImageAlpha(alpha);
            yield return null;
        }

        // Change Polygon Collider
        _currentConfiner.m_BoundingShape2D = levels[i];
        // set the player position
        _playerTransform.position = targetDoor;

        // fade in section
        fadeTimer = -0.1f; // HACK reset timer to -0.1f to avoid glitches when moving camera to new level polygon

        // perform decrease of alpha key
        while (fadeTimer < _fadeDuration) {
            fadeTimer += Time.deltaTime;
            var alpha = 1f - Mathf.Clamp01(fadeTimer / _fadeDuration);
            SetFadeImageAlpha(alpha);
            yield return null;
        }

        StopCoroutine(nameof(PerformLevelTransition));
    }


    // set alpha key 
    private void SetFadeImageAlpha(float alpha) {
        Color fadeColor = _fadeImage.color;
        fadeColor.a = alpha;
        _fadeImage.color = fadeColor;
    }
}