using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Manager")]
    [SerializeField] private List<LevelControl> levelPrefabs;// Will be changed to addressables.
    private LevelControl currentLevel;
    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = 120;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel(int index) 
    {
        // Disable Menu.
        // Enable Ortho.
        currentLevel = levelPrefabs[index];
        currentLevel.Init();
    }

    public void LoadLevel(LevelControl prefab)// Will be changed to addressables.
    {

    }
}
