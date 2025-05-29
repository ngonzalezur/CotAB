using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Tile : MonoBehaviour
{
    [SerializeField] private Material _baseColor, _offsetColor;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] public GameObject _highlight;
    [SerializeField] public GameObject _precast;
    [SerializeField] private bool _isWalkable;
    public Faction Faction;

    public int x, y;

    public BaseUnit OccupiedUnit;
    public BaseAttack OccupiedAttack;

    public int Burning = 0;

    public Tile HeroTile;
    public Tile EnemyTile;
    public Tile NeutralTile;
    public bool Walkable => _isWalkable && OccupiedUnit == null;

    Coroutine moveUnitCoroutine = null;

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

    IEnumerator StartBurning(int i)
    {
        Burning = i;
        //activar vfx
        yield return new WaitForSeconds(8);
        Burning = 0;
        //apagar vfx
    }

    public void StartCoroutineBurning(int i)
    {
        StartCoroutine(StartBurning(i));
    }

    public void SetUnit(BaseUnit unit)
    {
        if (unit == null || unit.Faction != Faction || this.OccupiedUnit != null) return;
        // Asegurar que la casilla anterior detenga su animación
        int pj = 0;
        if(unit.UnitName == "Druid")
        {
            pj = 1;
        }
        //string msj = "" + pj + "," + x + "," + y + "," + Time.time;
        AnalyticsManager.Instance.SendCustomEventMovement("" + pj + "," + x + "," + y + "," + Time.time);
        if (unit.OccupiedTile != null)
        {
            unit.OccupiedTile.StopMovingCoroutine();
        }

        // Detener cualquier corrutina en ejecución en esta tile
        StopMovingCoroutine();

        // Iniciar la nueva animación
        moveUnitCoroutine = StartCoroutine(SetUnitCoroutine(0.2f, 0.2f, unit.transform.position, transform.position + new Vector3(0, 0, -1), unit));
        if (unit.OccupiedTile != null)
        {
            //ChangeHighlight(unit);
            unit.OccupiedTile.OccupiedUnit = null;
        }

        OccupiedUnit = unit;
        unit.OccupiedTile = this;
    }

    IEnumerator SetUnitCoroutine(float delay, float duration, Vector3 from, Vector3 to, BaseUnit unit)
    {
        
        yield return new WaitForSeconds(delay);
        //Debug.Log(this);
        float startTime = Time.time;
        float elapsedTime = 0f;

        while (elapsedTime < duration && unit != null)
        {
            elapsedTime = Time.time - startTime;
            float normalizedTime = elapsedTime / duration;
            unit.transform.position = Vector3.Lerp(from, to, Mathf.Clamp01(normalizedTime));

            yield return null;
        }
        if (unit == null)
        {
            StopCoroutine(moveUnitCoroutine);
            yield return null;
        }

        // Asegurar que la posición final sea exacta
        unit.transform.position = to;

        // Actualizar referencias
        //if (unit.OccupiedTile != null)
        //{
        //    ChangeHighlight(unit);
        //    unit.OccupiedTile.OccupiedUnit = null;
        //}

        //OccupiedUnit = unit;
        //unit.OccupiedTile = this;

        StopMovingCoroutine();

        // Limpiar la referencia de la corrutina
        moveUnitCoroutine = null;
    }

    public void StopMovingCoroutine()
    {
        if (moveUnitCoroutine != null)
        {
            StopCoroutine(moveUnitCoroutine);
            moveUnitCoroutine = null;
        }
    }

    public void ChangeHighlight(BaseUnit unit)
    {
       // unit.GetHighlightHero()._highlight.SetActive(false);
    }

    public void InstantSetUnit(BaseUnit unit)
    {
        if(moveUnitCoroutine != null)
        {
            StopCoroutine(moveUnitCoroutine);
        }        
        unit.transform.position = transform.position + new Vector3(0, 0, -1);
    }


    //public void SetUnit(BaseUnit unit)
    //{
    //    //if (unit.OccupiedTile != null) unit.OccupiedTile.OccupiedUnit = null;
    //    //unit.transform.position = transform.position + new Vector3(0,0,-1);       
    //    //if(unit.OccupiedTile != null && unit.OccupiedTile.OccupiedUnit != null)
    //    //{
    //    //    unit.OccupiedTile.StopMovingCoroutine();
    //    //}
    //    if (moveUnitCoroutine != null)
    //    {
    //        StopCoroutine(moveUnitCoroutine);
    //        moveUnitCoroutine = null;
    //    }
    //    moveUnitCoroutine = StartCoroutine(SetUnitCoroutine(0.2f, 0.5f, unit.transform.position, transform.position + new Vector3(0, 0, -1), unit));
    //    //StopCoroutine(moveUnitCoroutine);
    //    //OccupiedUnit = unit;
    //    //unit.OccupiedTile = this;
    //}

    //IEnumerator SetUnitCoroutine(float delay, float duration, Vector3 from, Vector3 to, BaseUnit unit)
    //{
    //    yield return new WaitForSeconds(delay);
    //    float time = Time.time;
    //    while(true)
    //    {
    //        float normalizedtime = (Time.time - time)/duration;            
    //        Vector3 position = Vector3.Lerp(from, to, normalizedtime);
    //        unit.transform.position = position;
    //        if (normalizedtime >= 1)
    //        {
    //            //Debug.Log(this);
    //            unit.transform.position = to;
    //            if (unit.OccupiedTile != null) unit.OccupiedTile.OccupiedUnit = null;
    //            OccupiedUnit = unit;
    //            unit.OccupiedTile = this;
    //            StopMovingCoroutine();
    //            yield break;
    //        }
    //        yield return null;            
    //    }
    //}

    //public void StopMovingCoroutine()
    //{
    //    if(moveUnitCoroutine != null)
    //    {
    //        StopCoroutine(moveUnitCoroutine);
    //        moveUnitCoroutine = null;
    //    }
    //}

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
        if (this != null && this.x < GridManager.Instance._width - 1 && GridManager.Instance.GetTileAtPosition(new Vector2(this.x + 1, this.y)) != null)
        {
            return GridManager.Instance.GetTileAtPosition(new Vector2(this.x + 1, this.y));
        }
        return null;
    }
    public Tile LeftTile()
    {
        if (this != null && this.x > 0 && GridManager.Instance.GetTileAtPosition(new Vector2(this.x - 1, this.y)) != null)
        {
            return GridManager.Instance.GetTileAtPosition(new Vector2(this.x - 1, this.y));
        }
        return this;
    }
    public Tile UpTile()
    {
        if (this != null && this.y < GridManager.Instance._height- 1 && GridManager.Instance.GetTileAtPosition(new Vector2(this.x, this.y+1)) != null)
        {
            return GridManager.Instance.GetTileAtPosition(new Vector2(this.x, this.y + 1));
        }
        return this;
    }
    public Tile DownTile()
    {
        if (this != null && this.y > 0 && GridManager.Instance.GetTileAtPosition(new Vector2(this.x, this.y-1)) != null)
        {
            return GridManager.Instance.GetTileAtPosition(new Vector2(this.x, this.y - 1));
        }
        return this;
    }
}