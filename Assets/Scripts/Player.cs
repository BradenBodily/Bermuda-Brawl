using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public enum eType
    {
        PLAYER_ONE,
        PLAYER_TWO
    }

    [SerializeField] eType playerType;

    [HeaderAttribute("Ship")]
    [SerializeField] float speed;
    [SerializeField] float rotateSpeed;
    public Vector3 velocity = Vector3.zero;
    public float hitPoints;
    float maxHitPoints;
    public int goldCount = 0;

    [HeaderAttribute("Cannons")]
    [SerializeField] GameObject cannonBall;
    [SerializeField] List<GameObject> portCannons;
    [SerializeField] List<GameObject> starboardCannons;
    [SerializeField] GameObject cannonContainer;
    [SerializeField] float cannonSpeed;
    [SerializeField] float cannonFireWaitTime;
    float cannonTimer;

    [HeaderAttribute("Fire")]
    [SerializeField] List<GameObject> fires;

    [HeaderAttribute("Audio")]
    [SerializeField] AudioClip cannonShotClip;
    [SerializeField] AudioClip coinClip;
    [SerializeField] AudioClip chestClip;
    [SerializeField] AudioClip woodBreakingClip;
    AudioSource audioSource;

	void Start () {
        maxHitPoints = hitPoints;
        SetHitPoints();
        SetGold();
        cannonTimer = cannonFireWaitTime;
        audioSource = GetComponent<AudioSource>();
	}
	
	void Update () {

        cannonTimer -= Time.deltaTime;

        if(hitPoints <= (maxHitPoints * 2) / 3)
        {
            fires[0].SetActive(true);
        }
        if(hitPoints <= maxHitPoints / 3)
        {
            fires[2].SetActive(true);
        }

        if(hitPoints <= 0.0f)
        {
            fires[1].SetActive(true);
            velocity = Vector3.down * (speed / 3.0f);
            velocity = transform.rotation * velocity;
            transform.position = transform.position + (velocity * Time.deltaTime);
        }
        else
        {
            velocity = Vector3.forward * speed;

            Vector3 rotation = Vector3.zero;

            if (playerType == eType.PLAYER_ONE)
            {
                rotation.y = Input.GetAxis("P1TurnBoat") * rotateSpeed;
            }
            else if (playerType == eType.PLAYER_TWO)
            {
                rotation.y = Input.GetAxis("P2TurnBoat") * rotateSpeed;
            }

            transform.rotation = transform.rotation * Quaternion.Euler(rotation * Time.deltaTime);

            if (hitPoints / maxHitPoints < .5f)
            {
                velocity /= 2.0f;
            }

            velocity = transform.rotation * velocity;
            velocity += Wind.Instance.GetWindVector();
            transform.position = transform.position + (velocity * Time.deltaTime);


            if ((playerType == eType.PLAYER_ONE && Input.GetButtonDown("P1FireCannons") && cannonTimer <= 0.0f) ||
                   (playerType == eType.PLAYER_TWO && Input.GetButtonDown("P2FireCannons") && cannonTimer <= 0.0f))
            {
                cannonTimer = cannonFireWaitTime;

                audioSource.Play();
                ParticleSystem particles;

                for (int i = 0; i < portCannons.Count; i++)
                {
                    GameObject newCannonBall = Instantiate(cannonBall, portCannons[i].transform.position, Quaternion.identity, cannonContainer.transform);
                    Quaternion cannonRotation = transform.rotation * Quaternion.Euler(-20.0f, -90.0f, 0.0f);
                    Quaternion smokeRotation = transform.rotation * Quaternion.Euler(-20.0f, 90.0f, 0.0f);
                    //Debug.DrawRay(transform.position, cannonRotation * (Vector3.forward * 20.0f), Color.red, 5.0f);
                    Vector3 direction = Vector3.forward;
                    direction = cannonRotation * direction;
                    //Debug.DrawRay(transform.position, direction * 5.0f, Color.blue, 5.0f);
                    newCannonBall.GetComponent<Rigidbody>().AddForce(direction * cannonSpeed);
                    particles = newCannonBall.GetComponentInChildren<ParticleSystem>();
                    particles.transform.rotation = smokeRotation;
                }

                for (int j = 0; j < starboardCannons.Count; j++)
                {
                    GameObject newCannonBall = Instantiate(cannonBall, starboardCannons[j].transform.position, Quaternion.identity, cannonContainer.transform);
                    Quaternion cannonRotation = transform.rotation * Quaternion.Euler(-20.0f, 90.0f, 0.0f);
                    Quaternion smokeRotation = transform.rotation * Quaternion.Euler(-20.0f, -90.0f, 0.0f);
                    Vector3 direction = Vector3.forward;
                    direction = cannonRotation * direction;
                    newCannonBall.GetComponent<Rigidbody>().AddForce(direction * cannonSpeed);
                    particles = newCannonBall.GetComponentInChildren<ParticleSystem>();
                    particles.transform.rotation = smokeRotation;
                }

                audioSource.clip = cannonShotClip;
                audioSource.Play();
            }
        }        
	}

    public void PutOutFires()
    {
        foreach(GameObject fire in fires)
        {
            fire.GetComponent<AudioSource>().Stop();
            fire.SetActive(false);
        }
    }

    private void SetHitPoints()
    {
        if(hitPoints >= 0.0f)
        {
            if (playerType == eType.PLAYER_ONE)
            {
                UIManager.Instance.SetPlayerOneHP((int)hitPoints);
            }
            else if (playerType == eType.PLAYER_TWO)
            {
                UIManager.Instance.SetPlayerTwoHP((int)hitPoints);
            }
        }        
    }

    private void SetGold()
    {
        if (playerType == eType.PLAYER_ONE)
        {
            UIManager.Instance.SetPlayerOneGold(goldCount);
        }
        else if (playerType == eType.PLAYER_TWO)
        {
            UIManager.Instance.SetPlayerTwoGold(goldCount);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "CannonBall")
        {
            DamageDealer cannonBall = other.gameObject.GetComponent<DamageDealer>();
            hitPoints -= cannonBall.Damage;
            SetHitPoints();
            audioSource.clip = woodBreakingClip;
            audioSource.Play();
            Debug.Log("We've been hit!");
        }
        else if (other.tag == "SandBar" || other.tag == "Rock")
        {
            DamageDealer sandBar = other.gameObject.GetComponent<DamageDealer>();
            hitPoints -= sandBar.Damage;
            transform.eulerAngles = transform.eulerAngles + (180 * Vector3.up);
            SetHitPoints();
            audioSource.clip = woodBreakingClip;
            audioSource.Play();
            if(other.tag == "SandBar")
            {
                Debug.Log("We ran aground!");
            }
            else
            {
                Debug.Log("We smashed into a rock!");
            }
        }
        else if (other.tag == "Gold")
        {
            goldCount++;
            other.gameObject.SetActive(false);
            audioSource.clip = coinClip;
            audioSource.Play();
            SetGold();
        }
        else if (other.tag == "Chest")
        {
            goldCount += 5;
            other.gameObject.SetActive(false);
            SetGold();
            audioSource.clip = chestClip;
            audioSource.Play();
        }
        else if (other.tag == "Island")
        {
            if(playerType == eType.PLAYER_ONE)
            {
                UIManager.Instance.IslandEndGame("Player One");
            }
            else
            {
                UIManager.Instance.IslandEndGame("Player Two");
            }

        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.tag == "SandBar")
    //    {
    //        Debug.Log("IT WORKED");
    //    }
    //}
}
