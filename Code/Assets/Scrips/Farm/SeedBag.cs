using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//种子背包
public class SeedBag
{
    public List<Seed> Seeds;
    //private SeedBagWindow _window;

    //public void SetWindow(SeedbagWindow win)
    //{
    //    _window = win;
    //}

    public SeedBag()
    {
        Seeds = new List<Seed>();
    }

    public SeedBag(List<SeedEntity> entity)
    {
        Seeds = new List<Seed>();
        foreach (SeedEntity e in entity)
        {
            Seed seed = new Seed(e.SeedId, e.Count);
            Seeds.Add(seed);
        }
    }

    public List<SeedEntity> GetEntity()
    {
        List<SeedEntity> ret = new List<SeedEntity>();
        for (int i = 0, max = Seeds.Count; i < max;i++ )
        {
            SeedEntity entity = new SeedEntity();
            entity.SeedId = Seeds[i].Table.Id;
            entity.Count = Seeds[i].Count;
            ret.Add(entity);
            //Debug.Log("entity " + entity.SeedId + " :" + entity.Count);
        }
        return ret;
    }

    public void AddSeed(int id,int count = 1)
    {
        bool isHasThisKindOfSeed = false;
        
        for (int i = 0, max = Seeds.Count; i < max; i++)
        {
            if (Seeds[i].Table.Id == id) //found
            {
                Seeds[i].Count += count;

                isHasThisKindOfSeed = true;
                break;
            }
        }

        if (!isHasThisKindOfSeed)
        {
            Seed seed = new Seed(id, count);
            Seeds.Add(seed);
            Debug.Log("New Seed");
        }

        FarmGameData.Instance.Save();
    }

    public void CostSeed(int id,int count = 1)
    {
        bool isHasThisKindOfSeed = false;
        for (int i = 0, max = Seeds.Count; i < max; i++)
        {
            if (Seeds[i].Table.Id == id) //found
            {
                if (Seeds[i].Count >= count)
                {
                    Seeds[i].Count -= count;
                }
                else
                {
                    Debug.LogError(string.Format("Not Enough Seed Count, Need:{0} | Have:{1}",count,Seeds[i].Count));
                }

                isHasThisKindOfSeed = true;
                break;
            }
        }
        if (!isHasThisKindOfSeed)
        {
            Debug.LogError("No Such Kind Of Seed : " + id);
        }
        FarmGameData.Instance.Save();
    }

    public Seed Get(int seedId)
    {
        Seed seed = null;
        for (int i = 0, max = Seeds.Count; i < max; i++)
        {
            if (Seeds[i].Table.Id == seedId)
            {
                seed = Seeds[i];
                break;
            }
        }

        return seed;
    }

}
