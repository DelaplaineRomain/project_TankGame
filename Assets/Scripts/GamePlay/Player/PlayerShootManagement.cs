using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerShootManagement : MonoBehaviour
{
    // Player info
    [HideInInspector]
    public int PlayerId;
    [HideInInspector]
    public PlayerXsettings CurrentPlayerSettings;

    // Bullet stats
    [SerializeField]
    private GameObject bullet;
    public float shootForce, upwardForce;

    // Gun stats
    public float timeBetweenShooting, reloadTime, timeBetweenShots;
    public int magazineSize;
    public bool allowButtonHold;

    [SerializeField]
    private int bulletsLeft;

    // bools
    bool shooting, readyToShoot, reloading;

    // Reference
    public Transform spawnPoint;

    // Graphics
    public GameObject muzzleFlash;
    public Slider AmmoSlider;
    public Image FillImage;             // The fill image component of the slider

    // bug fixing
    private bool allowInvoke = true;

    // Animation variables
    private float fillTime = 0;

    // Fps variables
    public float updateInterval = 0.5f; //How often should the number update
    float accum = 0.0f;
    int frames = 0;
    float timeleft;
    float fps;

    // Power Up variables
    private float RateShooting = 1.5f;
    private int NewMagazineSize = 10;
    private bool AutoFireActivated = false;


    void Awake()
    {
        // Default UserInputSettings 
        if (PlayerId == 1 && UserInputSettings.Player1Settings == null)
        {
            UserInputSettings.Player1Settings = new PlayerXsettings(KeyCode.W, KeyCode.S, KeyCode.D, KeyCode.A, KeyCode.Space);
        }
        else if (PlayerId == 2 && UserInputSettings.Player2Settings == null)
        {
            UserInputSettings.Player2Settings = new PlayerXsettings(KeyCode.Keypad8, KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad4, KeyCode.Keypad0);
        }

        bulletsLeft = magazineSize;
        readyToShoot = true;

        // Set UI element
        AmmoSlider.maxValue = magazineSize;
        AmmoSlider.minValue = 0.0f;
        UpdateAmmoUI();

        timeleft = updateInterval;
    }

    public void UpdateAmmoUI()
    {
        // Set the slider's value appropriately.
        AmmoSlider.value = bulletsLeft;
    }

    void Update()
    {
        // Counting FPS
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0)
        {
            // display two fractional digits (f2 format)
            fps = (accum / frames);
            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }

        // Detecting input
        MyInput();

        if (reloading)
        {
            ReloadAnimation(fps);
        }

        if (AutoFireActivated)
        {
            if (bullet)
            {
                
            }
        }
    }

    private void MyInput()
    {
        if (PlayerId == 1)
        {
            CurrentPlayerSettings = UserInputSettings.Player1Settings;
        }
        else if (PlayerId == 2)
        {
            CurrentPlayerSettings = UserInputSettings.Player2Settings;
        }

        // Check the hold possibility
        if (allowButtonHold)
        {
            shooting = Input.GetKey(CurrentPlayerSettings.Shoot);
        }
        else
        {
            shooting = Input.GetKeyDown(CurrentPlayerSettings.Shoot);
        }

        // Reloading automatically
        if (readyToShoot &&  shooting && bulletsLeft <= 0 && !reloading)
        {
            Reload();
        }

        // Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            Shoot();
        }

    }

    private void Shoot()
    {
        readyToShoot = false;

        // Instanciate the bullet projectile
        GameObject currentBullet = Instantiate(bullet, spawnPoint.position, Quaternion.identity);
        currentBullet.transform.forward = this.transform.forward.normalized;
        currentBullet.GetComponent<CustomBullet>().PlayerId = PlayerId;

        // Add force for movement
        currentBullet.GetComponent<Rigidbody>().AddForce(this.transform.forward.normalized * shootForce, ForceMode.Impulse);


        // Instantiate muzzle flash if I have one
        if (muzzleFlash != null)
        {
            Instantiate(muzzleFlash, spawnPoint.position, Quaternion.identity);
        }
        
        bulletsLeft--;

        // Update UI element
        UpdateAmmoUI();

        // Invoke resetShot function (if not already invoked)
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
        fillTime = 0;
        UpdateAmmoUI();

        if (AutoFireActivated)
        {
            PU_AutoFireOff();
        }
    }

    private void ReloadAnimation (float fps)
    {
        AmmoSlider.value = fillTime;
        fillTime += (AmmoSlider.maxValue - AmmoSlider.minValue) / (reloadTime * fps);
    }

    public void PU_AutoFireOn()
    {
        AutoFireActivated = true;
        allowButtonHold = true;
        bulletsLeft = NewMagazineSize;
        AmmoSlider.maxValue = NewMagazineSize;
        UpdateAmmoUI();
        timeBetweenShooting /= RateShooting;
    }

    public void PU_AutoFireOff()
    {
        AutoFireActivated = false;
        allowButtonHold = false;
        AmmoSlider.maxValue = magazineSize;
        UpdateAmmoUI();
        timeBetweenShooting *= RateShooting;
    }
}
