using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBase
{
    public override void AddDamage(int damage)
    {
        if (_hp > damage)
        {
            _hp -= damage;
            return;
        }
        OnDead();
    }

    protected override void OnDead()
    {
        Debug.Log("Player Dead");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
