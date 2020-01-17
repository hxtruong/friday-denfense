﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MyObject
{
    public EnemyDelegate Delegate;
    public Price AwardPrice;
    public int AwardGold;

    protected bool Automation = false;
    protected bool Victory = false;
    protected Map Map;
    protected int NextIndexStation;

    public override void SetUpInStart()
    {
        base.SetUpInStart();
    }

    public override void UpdatePerFrame()
    {
        base.UpdatePerFrame();

        AutoMove();
    }

    public virtual void AutoMove(Map map)
    {
        Automation = true;

        Map = map;

        NextIndexStation = 1;
        transform.position = Map.Station(0);
    }

    protected virtual void AutoMove()
    {
        if (Automation && Map != null && isAlive() && !Victory)
        {
            Vector3 nextStation = Map.Station(NextIndexStation);
            if (!CanMoveTo(nextStation))
            {
                NextIndexStation++;
                nextStation = Map.Station(NextIndexStation);
            }

            if (NextIndexStation == Map.Stations.Count)
                Victory = true;

            MoveTo(nextStation);
        }
    }

    public override void TakeDamage(Damage TakeDamage)
    {
        Hp.TakeDamage(TakeDamage.Physical + TakeDamage.Magic);
        if (!isAlive())
        {
            Die();
        }
    }

    protected virtual void Set_Animation_Move()
    {
    }

    protected virtual void Set_Animation_Defend()
    {
    }

    protected virtual void Set_Animation_TakeDamage()
    {
    }

    protected virtual void Set_Animation_Die()
    {
        GetComponent<Animator>().SetTrigger(Constants.ENEMY_DIE);
    }

    protected virtual void Set_Animation_Attack()
    {
    }

    protected virtual void Set_Animation_Jump()
    {
    }

    protected override void Die()
    {
        if (Delegate != null)
        {
            gameObject.GetComponent<AudioSource>().Play();
            Set_Animation_Die();
            Delegate.Die(this);
            Destroy(gameObject, 1.5f);
        }
    }
}