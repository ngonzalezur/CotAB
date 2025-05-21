using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Linq;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AvoidHero", story: "[Enemy] stay on [x] and avoid [hero]", category: "Action", id: "0f6617abab883fd6215932331959d460")]
public partial class AvoidHeroAction : Action
{
    [SerializeReference] public BlackboardVariable<BaseUnit> Enemy;
    [SerializeReference] public BlackboardVariable<int> X;
    [SerializeReference] public BlackboardVariable<BaseUnit> Hero;

    private float _timer = 0f;
    private int _direction = 1;

    private const int MinRow = 0;
    private const int MaxRow = 5;

    protected override Status OnStart()
    {
        // Buscar al héroe si no está asignado
        if (Hero.Value == null)
        {
            Hero.Value = GameObject.FindObjectsByType<BaseUnit>(FindObjectsSortMode.None)
                .FirstOrDefault(u => u.isPersistentHero) ?? GameObject.FindFirstObjectByType<BaseHero>();
        }

        _timer = 1f;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Enemy.Value == null || Hero.Value == null || Enemy.Value.OccupiedTile == null || Hero.Value.OccupiedTile == null)
            return Status.Failure;

        int enemyX = Enemy.Value.OccupiedTile.PositionXTile();
        int enemyY = Enemy.Value.OccupiedTile.PositionYTile();
        int heroY = Hero.Value.OccupiedTile.PositionYTile();

        // Paso 1: mantener al enemigo en la columna fija
        if (enemyX < X.Value)
        {
            Enemy.Value.MoveRight();
            return Status.Success;
        }
        else if (enemyX > X.Value)
        {
            Enemy.Value.MoveLeft();
            return Status.Success;
        }

        // Paso 2: si el héroe está en la misma fila, moverse inmediatamente
        if (enemyY == heroY)
        {
            MoverEnemigoVertical();
            _timer = 2f; // Reiniciar temporizador
            return Status.Success;
        }

        // Paso 3: si no hay héroe en la fila, patrullar cada 2 segundos
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            MoverEnemigoVertical();
            _timer = 2f;
        }

        return Status.Running;
    }

    private void MoverEnemigoVertical()
    {
        int enemyY = Enemy.Value.OccupiedTile.PositionYTile();

        // Si llega al límite superior o inferior, cambiar dirección
        if (enemyY <= MinRow)
            _direction = 1; // subir
        else if (enemyY >= MaxRow)
            _direction = -1; // bajar

        if (_direction == 1)
            Enemy.Value.MoveUp();
        else
            Enemy.Value.MoveDown();
    }

    protected override void OnEnd()
    {
    }
}

