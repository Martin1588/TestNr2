using Assets.Scripts.HandheldControler.SkillChips;
using Assets.Scripts.HandheldControler.Weapons;
using Assets.Scripts.Target;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Gun : GameItem
{
    public ParticleSystem Muzzleflash;
    public Camera playerCamera;
    public DamageProfile DamageProfile { get; set; }
    public List<SkillChips> SkillChips { get; set; }

    public Vector3 stowedPosition = new Vector3(0, 0, -5);
    
    //public Vector3 activePosition = new Vector3(.2f, -.5f, .8f);

    public float hipFOV = 60f;
    public float adsFOV = 40f;
    public Vector3 hipPosition;
    public Vector3 adsPosition = new Vector3(-.0815f, -.41f, .4f);
    public float aimingSpeed = 10f;
    public float damage = 46f;
    public float strength;
    public float range = 100f;
    public float fireRate = 1f;

    private bool isAiming = false;

    public Animation recoilAnimation;
    public float recoilForce = 1.0f;

    public Type GunType { get; set; }

    public Gun()
    {
        //this.GunType = type;
        activePosition = new Vector3(.2f, -.2f, 1f);
    }

    void Start()
    {
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    //public override void Instantiate()
    //{
    //    Debug.Log("Benn.GO:" + Prefab);
    //    activeObject = Instantiate(Prefab);
    //    activeObject.gameObject.transform.parent = FindObjectOfType<Player>().transform;
    //    activeObject.gameObject.transform.localPosition = stowedPosition;
    //    activeObject.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
    //}

    public override void ExecutePrimaryAction()
    {
        Shoot();
    }

    public override void ExecuteSecondaryAction()
    {
        Aim();
    }

    public void Shoot()
    {
        Debug.Log("Shoot");
        RaycastHit hit;

        activeObject.GetComponentInChildren<VisualEffect>().Play();

        if (recoilAnimation != null)
            recoilAnimation.Play();

        if (Physics.Raycast(activeObject.transform.position, activeObject.transform.forward * -1, out hit, range))
        {
            Debug.Log(hit.transform.name);

            TargetBehaviour target = hit.transform.GetComponent<TargetBehaviour>();
            if(target != null)
            {
                target.TakeDamage(damage);
            }
        }

        Vector3 recoilDirection = -activeObject.transform.forward;
        activeObject.transform.position += recoilDirection * recoilForce * -1;
    }

    public void Aim()
    {
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        //GameObject.FindGameObjectWithTag("Crosshair").SetActive(false);

        float targetFOV = adsFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * aimingSpeed);

        //Prefab.GetComponent<WeaponSway>().swayAmount = .2f;
        //Prefab.GetComponent<WeaponSway>().maxSwayAmount = .5f;

        //Vector3 targetPosition = new Vector3(-.0815f, -.41f, .4f);
        Vector3 targetPosition = new Vector3(0f, -.1f, 0f);

        activeObject.transform.localPosition = Vector3.Lerp(activeObject.transform.localPosition, targetPosition, Time.deltaTime * aimingSpeed);
    }
}
