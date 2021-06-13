using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  private Rigidbody playerRb;
  private Animator playerJumpAnimation;
  public ParticleSystem dirtParticle;
  public ParticleSystem playerExplosion;
  public AudioClip jumpSound;
  public AudioClip crashSound;
  private AudioSource playerAudio;
  private float jumpForce = 500.0f;
  private float gravityModifier = 2.0f;
  public bool isOnGround = true;
  public bool isOnAir = true;
  public bool gameOver = false;
  public bool doubleJumpUsed = false;
  private float doubleJumpForce = 400.0f;
  public bool doubleSpeed = false;


  // Start is called before the first frame update
  void Start()
  {
    playerRb = GetComponent<Rigidbody>();
    playerJumpAnimation = GetComponent<Animator>();
    playerAudio = GetComponent<AudioSource>();
    Physics.gravity *= gravityModifier;
  }

  // Update is called once per frame
  void Update()
  {

    if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver)
    {
      playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
      isOnGround = false;
      playerJumpAnimation.SetTrigger("Jump_trig");
      dirtParticle.Stop();
      playerAudio.PlayOneShot(jumpSound, 1.0f);
      doubleJumpUsed = false;
    }
    else if (Input.GetKeyDown(KeyCode.Space) && !isOnGround && !doubleJumpUsed)
    {
      doubleJumpUsed = true;
      playerRb.AddForce(Vector3.up * doubleJumpForce, ForceMode.Impulse);
      playerJumpAnimation.Play("Running_Jump", 3, 0f);
      playerAudio.PlayOneShot(jumpSound, 1.0f);
    }

    if (Input.GetKey(KeyCode.LeftShift))
    {
      doubleSpeed = true;
      playerJumpAnimation.SetFloat("Speed_Multiplier", 2.0f);
    }
    else if (doubleSpeed)
    {
      doubleSpeed = false;
      playerJumpAnimation.SetFloat("Speed_Multiplier", 1.0f);
    }


  }


  private void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.CompareTag("Ground"))
    {
      isOnGround = true;
      dirtParticle.Play();
    }
    else if (collision.gameObject.CompareTag("Obstacle"))
    {
      Debug.Log("Game Over!");
      gameOver = true;
      playerJumpAnimation.SetBool("Death_b", true);
      playerJumpAnimation.SetInteger("DeathType_int", 1);
      dirtParticle.Stop();
      playerExplosion.Play();
      playerAudio.PlayOneShot(crashSound, 1.0f);

    }
  }
}
