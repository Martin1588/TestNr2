using Assets.Scripts.HandheldControler.Weapons.WeaponSettings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Assets.Scripts.HandheldControler.Weapons
{
    public class DamageProfile
    {
        public float physicalDamage { get; set; }
        public float fireDamage { get; set; }
        public float iceDamage { get; set; }
        public float lightningDamage { get; set; }

        public Tuple<string, float> dominantDamageType { get; set; }

        public DamageProfile() { }

        public DamageProfile(float phys, float fire, float ice, float lightning)
        {
            setDamageValue(this.GetType().GetProperty("physicalDamage"), phys);
            fireDamage = fire;
            iceDamage = ice;
            lightningDamage = lightning;
        }

        public void setDamageValue(PropertyInfo property, Tuple<int, float> value)
        {
            property.SetValue(property, value.Item2);
        }

        public void setDamageValue(PropertyInfo property, float value)
        {
            property.SetValue(property, value);
            if (value > dominantDamageType.Item2) dominantDamageType = new Tuple<string, float>(property.Name, value);
            
        }



    }
}