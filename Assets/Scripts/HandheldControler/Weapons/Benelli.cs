using Assets.Scripts.HandheldControler.Interfaces;
using Assets.Scripts.HandheldControler.SkillChips;
using Assets.Scripts.HandheldControler.Weapons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Benelli : Gun
{
    public Benelli() {
        recoilForce = 1f;
        strength = 40f;
        damage = 46f;
        aimingSpeed = 10f;

        adsPosition = new Vector3(-.0815f, -.41f, .4f);
        activePosition = new Vector3(.2f, -.5f, .8f);        

        this.PrefabPath ="Weapons/Bennelli_M4";
        this.Prefab = Resources.Load("Weapons/Bennelli_M4") as GameObject;
        Debug.Log("Benn.GO:" + Prefab);
    }

    /*
        if(HandheldItem == null) HandheldItem = new GameItem();

        var test = Resources.Load("Weapons/Bennelli_M4") as GameObject;
        HandheldItem.gameObject = GameObject.Instantiate(test);
        HandheldItem.gameObject.transform.parent = transform;
        HandheldItem.gameObject.transform.localPosition = new Vector3(.2f, -.5f, .8f);
        HandheldItem.gameObject.transform.rotation = Quaternion.Euler(0, 180,0);
        HandheldItem.gameObject.SetActive(true);

        HandheldItem.PrefabPath = "Bennelli_M4";

        HandheldItem.Itemtype = typeof(Gun);

        HandheldItem.gameObject.GetComponent<ItemManager>().Initialize(HandheldItem);
     */
}
