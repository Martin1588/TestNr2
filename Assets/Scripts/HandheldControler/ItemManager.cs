using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager
{
    public IItemActionManager WeaponInterface;

    public ItemManager()   
    {}

    public void Initialize(GameItem item)
    {
        Debug.Log(item.Itemtype.ToString() + " ItemClass");
        Debug.Log(item.gameObject.name + " ItemName");
        this.WeaponInterface = (IItemActionManager)Activator.CreateInstance(item.Itemtype, new object[] { item.gameObject });            
    }

    public void PrimaryAction() 
    {
        this.WeaponInterface.ExecutePrimaryAction();
    }

    public void SecondaryAction() 
    {
        this.WeaponInterface.ExecuteSecondaryAction();
    }


}