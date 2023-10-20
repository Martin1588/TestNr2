using Assets.Scripts.HandheldControler.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public GameItem ActiveItem{get; set;}
    public List<GameItem> EquippedItems { get; set;}

    private void Start()
    {
        InitializeEquipment();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Num 1");
            if (!((GameItem)EquippedItems[0]).Equals(ActiveItem))
            {
                if(ActiveItem.activeObject != null)
                    ActiveItem.transform.localPosition = ActiveItem.stowedPosition;                
                ActiveItem = (GameItem)EquippedItems[0];
                Debug.Log("Change to " + ActiveItem.activePosition);
                //ActiveItem.gameObject.SetActive(!ActiveItem.gameObject.activeSelf);
                ActiveItem.transform.localPosition = ActiveItem.activePosition;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (ActiveItem.activeObject != EquippedItems[1].activeObject)
            {
                ActiveItem.transform.localPosition = ActiveItem.stowedPosition;
                ActiveItem = (GameItem)EquippedItems[1];
                //ActiveItem.gameObject.SetActive(!ActiveItem.gameObject.activeSelf);
                ActiveItem.transform.localPosition = ActiveItem.activePosition;
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log(ActiveItem.Itemname);
            Debug.Log(ActiveItem.activeObject.ToString());
            Debug.Log(EquippedItems.First() as Benelli);
            
            if (ActiveItem.activeObject != null)
                ActiveItem.ExecutePrimaryAction();            
        }

        if (Input.GetButton("Fire2"))
        {
            if (ActiveItem.activeObject != null)
                ActiveItem.ExecuteSecondaryAction();
        }
    }

    private void InitializeEquipment()
    {
        EquippedItems = new List<GameItem>() { };
        EquippedItems.Add(new Benelli());
        EquippedItems.Add(new M1911());
        EquippedItems.Add(new M4_8());

        //foreach(v item in EquippedItems)
        //{
        //    item.Instantiate();
        //}

        EquippedItems.OfType<Gun>().ToList().ForEach(x => x.Instantiate());

        ActiveItem = EquippedItems.First() as Benelli;

        Debug.Log(ActiveItem.activePosition.ToString());  

        ActiveItem.activeObject.transform.localPosition = ActiveItem.activePosition;    
    }
}
