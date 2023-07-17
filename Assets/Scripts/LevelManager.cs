using AYellowpaper.SerializedCollections;
using Cinemachine;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    private CinemachineConfiner2D _currentConfiner;

    [SerializedDictionary("Level", "Cam Confiner 2D")]
    public SerializedDictionary<int, PolygonCollider2D> levels;

    
    public void MoveToLevel(int i) {
        _currentConfiner.m_BoundingShape2D = levels[i];
    }

    
    private void Start() {
        _currentConfiner = GameObject.FindGameObjectWithTag("VirtualCam").GetComponent<CinemachineConfiner2D>();
        _currentConfiner.m_BoundingShape2D = levels[1];
    }
}