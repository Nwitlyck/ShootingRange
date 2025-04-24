using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;

    public float spreadIntensity;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;

    public GameObject muzzelFlash;
    public Animator animator;

    public int magazineSize, bulletsLeft;
    public float reloadTime;
    public bool isReloding;

    public GameObject wallLV1;
    public GameObject wallLV2;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;

    public void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();
        bulletsLeft = magazineSize;
    }

    void Update()
    {
        if(bulletsLeft == 0 && isShooting){
            SoundManager.Instance.outOfAmmoM1911.Play();
        }

        if (currentShootingMode == ShootingMode.Auto)
        {
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
        {
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if(Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && isReloding == false){
            Reload();
        }

        //Automatic Reload On Zero
        //if(readyToShoot && isShooting == false && isReloding == false && bulletsLeft <= 0) Reload();

        if(readyToShoot && isShooting  && bulletsLeft > 0)
        {
            burstBulletsLeft = bulletsPerBurst;
            FireWeapon();
        }

        if(AmmoManager.Instance.ammoDisplay != null){
            AmmoManager.Instance.ammoDisplay.text = $"{bulletsLeft/bulletsPerBurst}/{magazineSize/bulletsPerBurst}";
        }
    }

    //Resuelve problemas con el muzzel flash
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.09f);
        muzzelFlash.gameObject.SetActive(false);
    }

    private void FireWeapon()
    {
        bulletsLeft--;

        muzzelFlash.gameObject.SetActive(true);
        muzzelFlash.GetComponent<ParticleSystem>().Play();
        StartCoroutine("Wait");

        animator.SetTrigger("Recoil");

        SoundManager.Instance.shoothingM1911.Play();

        readyToShoot = false;
        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        //Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        bullet.tag = "Bullet";

        //Pointing the bullet to face the shooting direction
        bullet.transform.forward = shootingDirection;

        //Shoot the bullet
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

        //Destroy the bullet after some time
        StartCoroutine(DestroyBuletAfterTime(bullet, bulletPrefabLifeTime));

        //Checking if we are done shooting
        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        //Burst mode
        if(currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }

        if(PlayerPrefs.GetInt("Points") == 10) wallLV1.SetActive(false);
        if(PlayerPrefs.GetInt("Points") == 20) wallLV2.SetActive(false);
    }

    private void Reload(){

        SoundManager.Instance.reloadingM1911.Play();
        isReloding = true;
        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted(){
        bulletsLeft = magazineSize;
        isReloding = false;
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBuletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
