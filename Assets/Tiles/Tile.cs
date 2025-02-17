using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    [SerializeField] private Material _baseColor, _offsetColor;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] public GameObject _highlight;
    [SerializeField] private bool _isWalkable;

    public int x, y;

    public BaseUnit OccupiedUnit;
    public BaseAttack OccupiedAttack;
    public bool Walkable => _isWalkable && OccupiedUnit == null;

    public void Init(bool isOffset)
    {
        _renderer.material = isOffset ? _offsetColor : _baseColor;
    }

    void OnMouseEnter()
    {
        //_highlight.SetActive(true);
    }

    void OnMouseExit()
    {
       // _highlight.SetActive(false);
    }

    public void SetUnit(BaseUnit unit)
    {
        if (unit.OccupiedTile != null) unit.OccupiedTile.OccupiedUnit = null;
        unit.transform.position = transform.position + new Vector3(0,0,-1);
        OccupiedUnit = unit;
        unit.OccupiedTile = this;
    }

    public void SetAttack(BaseAttack attack)
    {
        if (attack.OccupiedTile != null) attack.OccupiedTile.OccupiedAttack = null;
        attack.transform.position = transform.position + new Vector3(0, 0, -1);
        OccupiedAttack = attack;
        attack.OccupiedTile = this;
    }

    public int PositionXTile()
    {
        return x;
    }

    public int PositionYTile()
    {
        return y;
    }

    public Tile RightTile()
    {
        if (this != null && this.x < GridManager.Instance._width - 1)
        {
            return GridManager.Instance.GetTileAtPosition(new Vector2(this.x + 1, this.y));
        }
        return this;
    }
    public Tile LeftTile()
    {
        if (this != null && this.x > 0)
        {
            return GridManager.Instance.GetTileAtPosition(new Vector2(this.x - 1, this.y));
        }
        return this;
    }
    public Tile UpTile()
    {
        if (this != null && this.y < GridManager.Instance._height- 1)
        {
            return GridManager.Instance.GetTileAtPosition(new Vector2(this.x, this.y + 1));
        }
        return this;
    }
    public Tile DownTile()
    {
        if (this != null && this.x > 0)
        {
            return GridManager.Instance.GetTileAtPosition(new Vector2(this.x, this.y - 1));
        }
        return this;
    }
}