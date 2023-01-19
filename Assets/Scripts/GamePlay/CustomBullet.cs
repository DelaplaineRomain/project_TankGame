using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBullet : MonoBehaviour
{
    // Reference
    public int PlayerId;

    // Assignables
    public Rigidbody rb;
    public LayerMask whatIsEnemies;

    // Stats 
    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;

    // Dammage
    public float explosiondammage;
    public float explosionRange;

    // Lifetime
    public int maxCollisions;
    public float maxLifetime;
    public bool explodeOnTouch = true;

    public int collisions = 0;
    PhysicMaterial physicsMaterial;

    // Bool
    private bool hasExploded;

    // Reference for particles and audio
    public ParticleSystem ExplosionParticles;         // Reference to the particles that will play on explosion.
    public AudioSource ExplosionAudio;                // Reference to the audio that will play on explosion.
    private float ExplosionStartingVolume = 1f;       // Default volume of the bullet explosion at the begining of the game

    private void Awake()
    {
        // Register the starting folume of explosion audio
        ExplosionStartingVolume = ExplosionAudio.volume;

        // Set up the audio according to the audio factor
        SetUpAudio(); 
    }

    private void SetUpAudio()
    {
        // Adjust audio of all audio source :
        ExplosionAudio.volume = ExplosionStartingVolume * GameManager.VolumeFactor;
        ExplosionAudio.Play();
    }

    private void Start()
    {
        Setup();
        hasExploded = false;
    }

    private void Update()
    {
        // Count down the lifetime
        maxLifetime -= Time.deltaTime;

        // Check if bullet has not already exploded
        if (!hasExploded)
        {
            // When the bullet should explode
            if (collisions >= maxCollisions)
            {
                Explode();
            }
            else if (maxLifetime <= 0)
            {
                Explode();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Count up collision with physics element
        // if (!collision.collider.CompareTag("PowerUp"))
        // {
        //     collisions++;
        //     Debug.Log("collision with : "+collision.collider.tag);
        // }
        collisions++;

        // Explode when bullet hit other player
        if (!hasExploded && explodeOnTouch && collision.collider.CompareTag("Player"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        // Signal that bullet has already exploded once
        hasExploded = true;

        // Check for enemies
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].GetComponent<PlayerShootManagement>().PlayerId != PlayerId)
            {
                Debug.Log("Other player hit");
                //TODO get component of enemy and call take damage
                enemies[i].GetComponent<PlayerResourceManagement>().TakeDammage(explosiondammage); 
            }
        }

        // Unparent the particles from the shell.
        ExplosionParticles.transform.parent = null;

        // Play the particle system.
        ExplosionParticles.Play();

        // Play the explosion sound effect.
        ExplosionAudio.Play();

        // Once the particles have finished, destroy the gameobject they are on.
        ParticleSystem.MainModule mainModule = ExplosionParticles.main;
        Destroy(ExplosionParticles.gameObject, mainModule.duration);

        // Add a little delay before destroying obgect to avoid bug
        Invoke("Delay", 0.01f);
    }

    private void Delay()
    {
        Destroy(this.gameObject);
    }

    private void Setup()
    {
        // Create a new physics material
        physicsMaterial = new PhysicMaterial();
        physicsMaterial.bounciness = bounciness;
        physicsMaterial.staticFriction = 0;
        physicsMaterial.dynamicFriction = 0;
        physicsMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
        physicsMaterial.bounceCombine = PhysicMaterialCombine.Maximum;

        // Assign material to collider
        GetComponent<CapsuleCollider>().material = physicsMaterial;

        // Set gravity
        rb.useGravity = useGravity;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
