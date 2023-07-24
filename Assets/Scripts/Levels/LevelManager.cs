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

namespace Levels {
    public class LevelManager : MonoBehaviour {
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
        private void Update() {
            if (_input.IsRestartPressed) {
                _input.IsRestartPressed = false;

                var currentExit = levelsData[currentLevel].entryDoor.transform.position;
                StartCoroutine(PerformLevelRestart(currentExit));
            }
        }


        private void Start() {
            _input = InputManager.Instance;
            levelsData = new Dictionary<int, LevelData>();
            var player = GameObject.FindGameObjectWithTag("Player");
            _playerTransform = player.transform;
            _colliderScript = player.GetComponent<PlayerColliderScript>();

            _exitDoors = GameObject.FindGameObjectWithTag("ExitDoorsTilemap").GetComponent<Tilemap>();
            _fadeImage = GameObject.FindGameObjectWithTag("FadeImage").GetComponent<Image>();
            _currentConfiner = GameObject.FindGameObjectWithTag("VirtualCam").GetComponent<CinemachineConfiner2D>();
            _currentConfiner.m_BoundingShape2D = levels[currentLevel];


            // set LevelData for every level
            foreach (var (levelNo, levelArea) in levels) {
                var levelDataObj = new LevelData(levelArea);
                levelDataObj.SetLevelObjects();

                levelsData.Add(levelNo, levelDataObj);
            }

            // Set the initial alpha value to 0 for the fade image
            SetFadeImageAlpha(0f);
        }


        public void MoveToNextLevel(Vector3 targetDoor) {
            currentLevel += 1;
            StartCoroutine(PerformLevelTransition(targetDoor));
        }


        private IEnumerator PerformLevelTransition(Vector3 targetDoor) {
            // begin transition
            _colliderScript.isTransitioning = true;

            // fade out section
            var fadeTimer = 0f;

            // perform increase of alpha key
            while (fadeTimer < fadeDuration) {
                fadeTimer += Time.deltaTime;
                var alpha = Mathf.Clamp01(fadeTimer / fadeDuration);
                SetFadeImageAlpha(alpha);
                yield return null;
            }

            // Change Polygon Collider
            _currentConfiner.m_BoundingShape2D = levels[currentLevel];
            // set the player position
            _playerTransform.position = TileMapUtils.GetWorldToCell(_exitDoors, targetDoor);

            // fade in section
            fadeTimer = -0.1f; // HACK reset timer to -0.1f to avoid glitches when moving camera to new level polygon

            // perform decrease of alpha key
            while (fadeTimer < fadeDuration) {
                fadeTimer += Time.deltaTime;
                var alpha = 1f - Mathf.Clamp01(fadeTimer / fadeDuration);
                SetFadeImageAlpha(alpha);
                yield return null;
            }

            // stop transition
            _colliderScript.isTransitioning = false;

            StopCoroutine(nameof(PerformLevelTransition));
        }


        // TODO: consolidate with the above one somehow
        private IEnumerator PerformLevelRestart(Vector3 targetDoor) {
            // begin transition
            _colliderScript.isTransitioning = true;

            // fade out section
            var fadeTimer = 0f;

            // perform increase of alpha key
            while (fadeTimer < fadeDuration) {
                fadeTimer += Time.deltaTime;
                var alpha = Mathf.Clamp01(fadeTimer / fadeDuration);
                SetFadeImageAlpha(alpha);
                yield return null;
            }

            // restart current level
            levelsData[currentLevel].ResetLevel();

            // set the player position
            _playerTransform.position = TileMapUtils.GetWorldToCell(_exitDoors, targetDoor);

            // fade in section
            fadeTimer = -0.1f; // HACK reset timer to -0.1f to avoid glitches when moving camera to new level polygon

            // perform decrease of alpha key
            while (fadeTimer < fadeDuration) {
                fadeTimer += Time.deltaTime;
                var alpha = 1f - Mathf.Clamp01(fadeTimer / fadeDuration);
                SetFadeImageAlpha(alpha);
                yield return null;
            }

            // stop transition
            _colliderScript.isTransitioning = false;

            StopCoroutine(nameof(PerformLevelTransition));
        }


        // set alpha key 
        private void SetFadeImageAlpha(float alpha) {
            Color fadeColor = _fadeImage.color;
            fadeColor.a = alpha;
            _fadeImage.color = fadeColor;
        }
    }
}