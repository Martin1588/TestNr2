using Assets.Scripts.HandheldControler.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M1911 : Gun
{
    public M1911()
    {
        recoilForce = 1f;
        strength = 40f;
        damage = 46f;
        adsPosition = new Vector3(-.0815f, -.41f, .4f);
        aimingSpeed = 10f;

        this.PrefabPath = "Weapons/M1911";
        this.Prefab = Resources.Load("Weapons/M1911") as GameObject;
    }
}
