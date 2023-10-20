using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
 

public abstract class GameItem : MonoBehaviour {
    //public IItemActionManager ActionInterface { get; }
    //public ItemManager ItemManager { get; }
    public GameObject Prefab {  get; set; }
    public GameObject activeObject {  get; set; }
    public string PrefabPath { get; set; }
    public Type Itemtype { get; set; }
    public string Itemname { get; set; } = "Test";
    public string Description { get; set; }
    public Vector3 stowedPosition { get; set; } = new Vector3(0f, 0f, 0f);
    public Vector3 activePosition { get; set; } = new Vector3(.2f, -.5f, .8f);

    //public Transform PlayerLocation { get; set; }

    public GameItem() {
        
    }

    public virtual void ExecutePrimaryAction() { }
    public virtual void ExecuteSecondaryAction() { }

    public virtual void Instantiate()
    {
        Debug.Log("Benn.GO:" + Prefab);
        activeObject = Instantiate(Prefab);
        activeObject.gameObject.transform.parent =  FindObjectOfType<Camera>().transform;
        activeObject.gameObject.transform.localPosition = stowedPosition + (new Vector3(0f,.7f,0f));
        activeObject.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
    }
}