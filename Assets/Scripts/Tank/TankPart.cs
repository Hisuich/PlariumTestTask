using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PartType
{
    Gun,
    Body,
    Tower
}

abstract public class TankPart : ScriptableObject
{

    [SerializeField]
    protected Sprite playerSprite;
    [SerializeField]
    protected Sprite enemySprite;

    protected PartType partType;

    public PartType type
    { 
        get { return partType; }
    }


    public Sprite PlayerSprite
    {
        get { return playerSprite; }
    }

    public Sprite EnemySprite
    { 
        get { return enemySprite; }
    }

    public Sprite GetSprite(Team team)
    {
        if (team == Team.Player)
            return playerSprite;
        else if (team == Team.Enemy)
            return enemySprite;
        return playerSprite;
    }

}
