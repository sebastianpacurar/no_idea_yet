using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Cinemachine;
using Input;
using Knight;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Utils;


namespace Levels
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private float fadeDuration;
        [Space(5)]
        public int currentLevel;
        [SerializedDictionary("Level", "Polygon Collider")]
        public SerializedDictionary<int, PolygonCollider2D> levels;

        public Dictionary<int, LevelData> levelsData;


        private InputManager _input;
        private Tilemap _exitDoors;
        private CinemachineConfiner2D _currentConfiner;
        private PlayerColliderScript _colliderScript;
        private Transform _playerTransform;
        private Image _fadeImage;

        private bool _isFading;


        // check for restart level input
        private void Update()
        {
            if (_input.IsRestartPressed)
            {
                _input.IsRestartPressed = false;

                // relocate player to current entry door
                var entryDoor = levelsData[currentLevel].entryDoor.transform.position;
                TransitionToSameLevel(entryDoor);
            }
        }


        private void Awake()
        {
            _input = InputManager.Instance;
            levelsData = new Dictionary<int, LevelData>();

            _exitDoors = GameObject.FindGameObjectWithTag("ExitDoorsTilemap").GetComponent<Tilemap>();
            _fadeImage = GameObject.FindGameObjectWithTag("FadeImage").GetComponent<Image>();
            _currentConfiner = GameObject.FindGameObjectWithTag("VirtualCam").GetComponent<CinemachineConfiner2D>();

            SetLevelData();
        }


        private void Start()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            _playerTransform = player.transform;
            _colliderScript = player.GetComponent<PlayerColliderScript>();

            _currentConfiner.m_BoundingShape2D = levels[currentLevel];

            SetFadeImageAlpha(0f);
        }


        public void TransitionToNextLevel(Vector3 targetDoor)
        {
            StartCoroutine(PerformTransition(targetDoor, false));
        }


        private void TransitionToSameLevel(Vector3 targetDoor)
        {
            StartCoroutine(PerformTransition(targetDoor, true));
        }


        private IEnumerator PerformTransition(Vector3 targetDoor, bool isResetLevel)
        {
            // begin transition
            _colliderScript.isTransitioning = true;

            // fade out section
            var fadeTimer = 0f;

            // perform increase of alpha key
            while (fadeTimer < fadeDuration)
            {
                fadeTimer += Time.deltaTime;
                var alpha = Mathf.Clamp01(fadeTimer / fadeDuration);
                SetFadeImageAlpha(alpha);
                yield return null;
            }

            if (isResetLevel)
            {
                ResetLevelData(targetDoor);
            } else
            {
                SetNextLevelData(targetDoor);
            }

            // fade in section
            fadeTimer = -0.1f; // HACK reset timer to -0.1f to avoid glitches when moving camera to new level polygon

            // perform decrease of alpha key
            while (fadeTimer < fadeDuration)
            {
                fadeTimer += Time.deltaTime;
                var alpha = 1f - Mathf.Clamp01(fadeTimer / fadeDuration);
                SetFadeImageAlpha(alpha);
                yield return null;
            }

            // stop transition
            _colliderScript.isTransitioning = false;

            StopCoroutine(nameof(PerformTransition));
        }


        // set LevelData for every level
        private void SetLevelData()
        {
            levelsData = levels.ToDictionary(pair => pair.Key, pair => new LevelData(pair.Value));
        }


        // triggered when performing Reset Level transitions
        private void ResetLevelData(Vector3 targetDoor)
        {
            levelsData[currentLevel].ResetLevel(); // restart current level
            _playerTransform.position = TileMapUtils.GetWorldToCell(_exitDoors, targetDoor); // set the player position
        }


        // triggered when performing Next Level transition
        private void SetNextLevelData(Vector3 targetDoor)
        {
            currentLevel += 1;
            _currentConfiner.m_BoundingShape2D = levelsData[currentLevel].levelArea; // change Polygon Collider
            _playerTransform.position = TileMapUtils.GetWorldToCell(_exitDoors, targetDoor); // set the player position
        }


        // set alpha key 
        private void SetFadeImageAlpha(float alpha)
        {
            var fadeColor = _fadeImage.color;
            fadeColor.a = alpha;
            _fadeImage.color = fadeColor;
        }
    }
}