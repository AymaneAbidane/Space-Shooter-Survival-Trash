using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField, SceneObjectsOnly] private GameStateManager gameStateManager;
    [SerializeField, SceneObjectsOnly] private PlayerHealthManager playerHealthManager;
    [SerializeField, AssetsOnly] private Path[] gamePathsVarriants;
    [SerializeField, ChildGameObjectsOnly] private Transform spawnPositiontransform;

    [SerializeField, AssetsOnly] private Enemy smallEnemyVarriant;
    [SerializeField] private float delayBetweenEachSpawn;
    [SerializeField] private int numberOfSpawnedShipsOnPool;

    private bool isSpawning;
    private List<Enemy> enemysPoolList = new();
    private int enemysPoollIndex = 0;
    private Coroutine spawnCor;

    private void Awake()
    {
        gameStateManager.onGameStateChanged += GameStateManager_onGameStateChanged;
        playerHealthManager.onPlayerhealthReachZero += PlayerHealthManager_onPlayerhealthReachZero;
    }


    private void OnDestroy()
    {
        gameStateManager.onGameStateChanged -= GameStateManager_onGameStateChanged;
        playerHealthManager.onPlayerhealthReachZero -= PlayerHealthManager_onPlayerhealthReachZero;
    }

    private void Start()
    {
        ManageShipsPool();
    }


    private void PlayerHealthManager_onPlayerhealthReachZero(object sender, EventArgs e)
    {
        Reset();
    }

    private void Reset()
    {
        ManageShipsPool(false);
        StopCoroutine(spawnCor);
        isSpawning = false;
        enemysPoollIndex = 0;
        ManageShipsPool();
    }

    private void GameStateManager_onGameStateChanged(object sender, GameStateManager.GameState e)
    {
        if (e == GameStateManager.GameState.GamePlay)
        {
            isSpawning = true;
            spawnCor = StartCoroutine(COR_SpawnEnemys());
        }
    }

    private IEnumerator COR_SpawnEnemys()
    {
        while (isSpawning)
        {
            Enemy enemyShip = enemysPoolList[enemysPoollIndex];
            enemyShip.SetPath(GetRandomPath());
            enemyShip.StartMoving();
            if (enemysPoollIndex == enemysPoolList.Count - 1)
            {
                enemysPoollIndex = 0;
            }
            else
            {
                enemysPoollIndex++;
            }

            yield return new WaitForSeconds(delayBetweenEachSpawn);
        }
    }

    private Path GetRandomPath()
    {
        Path path = gamePathsVarriants[UnityEngine.Random.Range(0, gamePathsVarriants.Length)];

        float coinFlip = UnityEngine.Random.value;

        if (coinFlip <= 0.5f)
        {
            path.redirectedPath = false;
        }
        else
        {
            path.redirectedPath = true;
        }

        return path;
    }
    private void ManageShipsPool(bool AddToPool = true)
    {
        if (AddToPool == true)
        {
            this.AddToPool();
        }
        else
        {
            ClearingPool();
        }

    }

    private void ClearingPool()
    {
        foreach (var ship in enemysPoolList)
        {
            // ship.gameObject.SetActive(false);
            Destroy(ship.gameObject);
        }

        enemysPoolList.Clear();
    }

    private void AddToPool()
    {

        for (int i = 0; i < numberOfSpawnedShipsOnPool; i++)
        {
            var copy = Instantiate(smallEnemyVarriant);
            copy.transform.position = spawnPositiontransform.position;
            copy.SetInstantiationPosition(spawnPositiontransform.position);
            enemysPoolList.Add(copy);
        }
    }

    public List<Enemy> GetPoolShips()
    {
        return enemysPoolList;
    }

}
