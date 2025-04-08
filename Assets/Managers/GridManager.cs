using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] public int _width, _height;

    [SerializeField] private Tile tile;

    [SerializeField] private Transform _cam;

    private Dictionary<Vector2, Tile> _tiles;

    public static GridManager Instance;

    void Awake()
    {
        Instance = this;
    }

    //void Start()
    //{
    //    GenerateGrid();
    //}

    public void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var typeoftile = tile;
                if(x < _width / 2)
                {
                    typeoftile.EnemyTile.gameObject.SetActive(false);
                    typeoftile.HeroTile.gameObject.SetActive(true);
                    typeoftile.Faction = Faction.Hero;

                }else if(x >= _width / 2)
                {
                    typeoftile.HeroTile.gameObject.SetActive(false);
                    typeoftile.EnemyTile.gameObject.SetActive(true);
                    typeoftile.Faction = Faction.Enemy;
                }
                //var typeOfTile = x < _width / 2 ? _grassfile : _enemytile;
                var spawnedTile = Instantiate(typeoftile, new Vector3(x, y), Quaternion.Euler(90, 0, 0));
                spawnedTile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.HeroTile.Init(isOffset);
                spawnedTile.EnemyTile.Init(isOffset);

                _tiles[new Vector2(x, y)] = spawnedTile;
                spawnedTile.x = x;
                spawnedTile.y = y;
            }
        }

        //_cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
        GameManager.Instance.ChangeState(GameState.SpawnHeroes);
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }
}