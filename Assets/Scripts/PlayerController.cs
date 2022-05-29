using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float _speed = 5;
    public float _baseSpeed = 5;
    public float _boostedSpeed = 8;
    public float _baseBoostedSpeed = 8;
    public float _shiftSpeed = 8;
    public float _shiftBoostedSpeed = 12;
    public float fireRate = 0.1f;
    private float nextFire = -1f;
    private float _maxThruster = 5;
    public float currentThruster;

    public int maximumLives = 3;
    public int currentLives = 3;
    public int score;
    public int shieldHealth;
    public int currentAmmo;
    public int maxAmmo = 30;


    public GameObject _laser;
    public GameObject _giantLaser;
    public GameObject _tripleshot;
    public GameObject _playerShield;
    public GameObject damagedLeft;
    public GameObject damagedRight;
    public GameObject fullShield;
    public GameObject damagedShield;
    public GameObject criticalShield;
    public GameObject tractorBeam;

    public CameraShake camerashake;

    public Slider thrusterSlider;
    public Text ammoCountText;



    [SerializeField]
    private GameObject _thrusters;

    public bool isTripleShotActive = false;
    public bool isSpeedBoostActive = false;
    public bool isGiantLaserActive = false;
    public bool isThrusterOn = false;
    public bool isStunActive = false;

    private AudioSource audioSource;
    public AudioClip laserSound;
    public AudioClip giantLaserSound;
    public AudioClip noAmmoSound;

    private UIManager uIManager;
    void Start()
    {
        uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        audioSource = GetComponent<AudioSource>();
        currentAmmo = 15;

        currentThruster = _maxThruster;
        thrusterSlider.maxValue = _maxThruster;
    }

    void Update()
    {
        Movement();
        PlayerBoundaries();
        FireLaser();
        Thrusters();
        TractorBeam();
    }

    public void Movement()
    { 
        float horizinput = Input.GetAxis("Horizontal");
        float vertinput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizinput, vertinput, 0);

        if (isSpeedBoostActive == true)
        {
            transform.Translate(direction * _boostedSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }
    }
    public void PlayerBoundaries()
    {
        if (transform.position.x > 11)
        {
            transform.position = new Vector3(-11f, transform.position.y, 0);
        }
        if (transform.position.x < -11)
        {
            transform.position = new Vector3(11f, transform.position.y, 0);
        }
        if (transform.position.y > 2)
        {
            transform.position = new Vector3(transform.position.x, 2, 0);
        }
        if (transform.position.y < -4)
        {
            transform.position = new Vector3(transform.position.x, -4, 0);
        }
    }
    public void FireLaser()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > nextFire && isTripleShotActive == false && currentAmmo > 0 && isGiantLaserActive == false)
        {
            currentAmmo--;
            nextFire = Time.time + fireRate;
            Instantiate(_laser, transform.position + new Vector3(0,0.8f,0), Quaternion.identity);
            AudioSource.PlayClipAtPoint(laserSound, transform.position);

        }
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > nextFire && isTripleShotActive == true && currentAmmo > 0 && isGiantLaserActive == false)
        {
            currentAmmo--;
            nextFire = Time.time + fireRate;
            Instantiate(_tripleshot, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(laserSound, transform.position);
        }
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > nextFire && isTripleShotActive == false && currentAmmo > 0 && isGiantLaserActive == true)
        {
            currentAmmo--;
            nextFire = Time.time + fireRate + 3;
            _giantLaser.SetActive(true);
            StartCoroutine(GiantLaserTimer());
            AudioSource.PlayClipAtPoint(giantLaserSound, transform.position);

        }
            if (currentAmmo > 30)
        {
            currentAmmo = maxAmmo;
        }

        ammoCountText.text = "Ammo:" + currentAmmo;

        if (currentAmmo == 0)
        {
            ammoCountText.color = Color.red;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                AudioSource.PlayClipAtPoint(noAmmoSound, transform.position);
            }
        }
    }

    public void PlayerDamage()
    {

        if (shieldHealth == 3)
        {
            shieldHealth--;
            fullShield.SetActive(false);
            damagedShield.SetActive(true);
            return;
        }

        if (shieldHealth == 2)
        {
            shieldHealth--;
            damagedShield.SetActive(false);
            criticalShield.SetActive(true);
            return;
        }
        if (shieldHealth == 1)
        {
            shieldHealth = 0;
            criticalShield.SetActive(false);
            return;
        }

        currentLives--;
        StartCoroutine(camerashake.ShaketheCamera(.05f, .4f));
        if (currentLives == 2)
        {
            damagedRight.SetActive(true);
        }
        if (currentLives == 1)
        {
            damagedLeft.SetActive(true);
        }
        uIManager.UpdateLives(currentLives);


        if (currentLives <= 0)
        {
            uIManager.GameOver();
            Destroy(this.gameObject);
        }
    }
    public void AddScore(int points)
    {
        score += points;
        uIManager.UpdateScore(score);
    }

    public void TripleShotActivated()
    {
        isTripleShotActive = true;
        isGiantLaserActive = false;
        currentAmmo = currentAmmo + 10;
        StartCoroutine(TripleShotCooldown());
    }
    public void GiantLaserActivated()
    {
        isGiantLaserActive = true;
        isTripleShotActive = false;
        currentAmmo = currentAmmo + 10;
    }

    public void SpeedBoostActivated()
    {
        isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostCooldown());
    }
    public void ShieldActive()
    {
        shieldHealth = 3;
        fullShield.SetActive(true);
        damagedShield.SetActive(false);
        criticalShield.SetActive(false);


    }
    public void CollectedAmmo()
    {
        currentAmmo = currentAmmo + 15;
        ammoCountText.color = Color.green;
    }
    public void CollectedStunHazard()
    {
        shieldHealth = 0;
        isSpeedBoostActive = false;
        fullShield.SetActive(false);
        damagedShield.SetActive(false);
        criticalShield.SetActive(false);
        isStunActive = true;
        isThrusterOn = false;
        _thrusters.SetActive(false);
        currentThruster = 0;
        _speed = 0;
        _shiftSpeed = 0;
        thrusterSlider.value = currentThruster;
        StartCoroutine(StunActive());

        IEnumerator StunActive()
        {
            yield return new WaitForSeconds(3.0f);
            isStunActive = false;
            _speed = 5;
            _shiftSpeed = 8;
        }
    }
    public void AddLife()
    {
        currentLives = currentLives + 1;
        if (currentLives >= 3)
        {
            currentLives = 3;
            damagedLeft.SetActive(false);
            damagedRight.SetActive(false);
        }
        if (currentLives == 2)
        {
            damagedRight.SetActive(true);
            damagedLeft.SetActive(false);
        }

        uIManager.UpdateLives(currentLives);
    }
   public  void Thrusters()
    {
        if (Input.GetKey(KeyCode.LeftShift) && currentThruster > 1 && isStunActive == false)
        {
            _speed = _shiftSpeed;
            _boostedSpeed = _shiftBoostedSpeed;
            _thrusters.SetActive(true);
            isThrusterOn = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) | currentThruster == 0 && isStunActive == false)
        {
            _thrusters.SetActive(false);
            _speed = _baseSpeed;
            _boostedSpeed = _baseBoostedSpeed;
            isThrusterOn = false;
        }

        if (isThrusterOn == true)
        {
            currentThruster -= Time.deltaTime;
        }
        else
        {
            currentThruster += Time.deltaTime;
        }

        if (currentThruster > 5)
        {
            currentThruster = _maxThruster;
        }

        if (currentThruster < 0)
        {
            currentThruster = 0;
        }
        thrusterSlider.value = currentThruster;


    }

    public void TractorBeam()
    {
        if (Input.GetKey(KeyCode.C))
        {
            tractorBeam.SetActive(true);
        }
        else
        {
            tractorBeam.SetActive(false);

        }
    }
    public IEnumerator TripleShotCooldown()
    {
        yield return new WaitForSeconds(5f);
        isTripleShotActive = false;
    }
    public IEnumerator SpeedBoostCooldown()
    {
        yield return new WaitForSeconds(7f);
        isSpeedBoostActive = false;
    }
    public IEnumerator GiantLaserTimer()
    {
        yield return new WaitForSeconds(3);
        _giantLaser.SetActive(false);
        isGiantLaserActive = false;
    }
}
