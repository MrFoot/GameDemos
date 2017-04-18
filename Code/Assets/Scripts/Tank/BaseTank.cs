using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BaseTank{

    public List<CharacterBase> Fishes;

    public BaseTank(Vector3 size)
    {
        Size = size;
        Fishes = new List<CharacterBase>();
    }

    public BaseTank(int x, int y, int z)
    {
        Size = new Vector3(x, y, z);
        Fishes = new List<CharacterBase>();
    }

    //大小  x:宽  y:高  z:深
    public Vector3 Size
    {
        get;
        private set;
    }

    
    public bool IsActive
    {
        get;
        set;
    }

    public virtual void Init()
    {
    }

    public void Add()
    {
        for (int i = 0 ; i < 10 ; i++)
        {
            CharacterBase fish = CharacterFactory.CreateCharacter(0, this);
            Fishes.Add(fish);
        }
    }

    public void Remove(CharacterBase fish)
    {
        Fishes.Remove(fish);
    }


    public void Shock() {
        for (int i = 0, max = Fishes.Count; i < max; i++)
        {
            Fishes[i].CharacterStateManager.FireAction(CharacterAction.Shock);
        }
    }


    public virtual void OnUpdate()
    {
        if (!IsActive)
            return;

        for (int i = 0, max = Fishes.Count; i < max; i++)
        {
            Fishes[i].OnUpdate();
        }
    }
}
