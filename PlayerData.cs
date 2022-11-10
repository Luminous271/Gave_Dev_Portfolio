using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public string playerName;
    public float health;
    public int level;

    public PlayerData(string playerName, float health, int level)
    {
        this.playerName = playerName;
        this.health = health;
        this.level = level;
    }

    public override string ToString()
    {
        return $"{playerName} is at {health} HP. They are at level {level}";
    }
     

}
