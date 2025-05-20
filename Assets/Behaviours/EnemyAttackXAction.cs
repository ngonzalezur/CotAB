using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Collections;
using System.Collections.Generic;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "EnemyAttackX", story: "[Enemy] cast attack [x] in [target]", category: "Action", id: "a57098f21ec0a9f97363e87e9c5bbd9b")]
public partial class EnemyAttackXAction : Action
{
    [SerializeReference] public BlackboardVariable<BaseUnit> Enemy;
    [SerializeReference] public BlackboardVariable<int> X;
    [SerializeReference] public BlackboardVariable<List<Tile>> Target;
    UnitManager uManager;
    Animator animator;
    protected override Status OnStart()
    {
        uManager = UnitManager.Instance;
        animator = Enemy.Value.GetComponentInChildren<Animator>();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Enemy.Value != null && Enemy.Value.Attacks[X] != null)
        {
            DoAttack(Enemy.Value, Enemy.Value.Attacks[X], Target.Value);
        }
        return Status.Success;
    }

    void DoAttack(BaseUnit unit, BaseAttack attack, List<Tile> target)
    {
        if (target != null)
        {
            if(X % 2 == 0)
            {
                animator.SetTrigger("Attack1");
            }
            else if (X % 2 == 1)
            {
                animator.SetTrigger("Attack2");
            }
            if (attack.type == BaseAttack.AttType.dashMelee)
            {
                uManager.StartCoroutine(uManager.TeleportMeleeDash(unit, attack, unit.OccupiedTile));
                uManager.PrecastDelete(target);
            }
            else if (attack.type == BaseAttack.AttType.parry)
            {
                uManager.StartCoroutine(uManager.ActivateParry(unit));
                uManager.PrecastDelete(target);
            }
            else if (attack.type == BaseAttack.AttType.gancho)
            {
                uManager.HabilidadGancho(unit, attack);
                uManager.PrecastDelete(target);
            }
            else if (attack.type == BaseAttack.AttType.atractor)
            {
                uManager.HabilidadAtraer(unit, attack);
                uManager.PrecastDelete(target);
            }
            else if (attack.type == BaseAttack.AttType.areaLast2Columns)
            {
                uManager.MoveFront(attack, target);
                uManager.PrecastDelete(target);
            }
            else if (attack.type == BaseAttack.AttType.barridofilainverso)
            {
                uManager.StartCoroutine(uManager.Barrer(target, attack));
                uManager.PrecastDelete(target);
            }
            else if (attack.type == BaseAttack.AttType.barridocolumnainverso)
            {
                uManager.StartCoroutine(uManager.Barrer(target, attack));
                uManager.PrecastDelete(target);
            }
            else if (attack.type == BaseAttack.AttType.areadelay)
            {
                uManager.SetAttacksInTiles(target, attack);
                uManager.StartCoroutine(uManager.ExtraAttack(target, attack.ExtraAttack));
                uManager.PrecastDelete(target);
            }
            else if (attack.type == BaseAttack.AttType.invocacion)
            {
                if (uManager.Invocaciones.Count <= 1)
                {
                    if (uManager.Invocaciones.Count == 0)
                    {
                        uManager.InstanciarInvocacion(attack, target);
                    }
                    else if (uManager.Invocaciones[0] != null && uManager.Invocaciones[0].UnitName != attack.invocacion.UnitName)
                    {
                        uManager.InstanciarInvocacion(attack, target);
                    }

                }
                uManager.PrecastDelete(target);
            }
            else
            {
                uManager.SetAttacksInTiles(target, attack);
                uManager.PrecastDelete(target);
            }
        }
    }

    protected override void OnEnd()
    {
    }
}

