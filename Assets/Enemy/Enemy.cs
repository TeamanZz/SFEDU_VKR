using Photon.Pun;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health = 100;
    [SerializeField] private float speed = 35;
    [SerializeField] private GameObject hitParticles;
    [SerializeField] private GameObject deathParticles;

    private Animator animator;
    private Transform target;
    private Rigidbody2D RB;
    private bool isDamageAnimationPlaying;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        RB = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        FollowTarget(target.position, speed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out HeroController heroController))
        {
            LevelManager.Instance.Restart();
        }
    }

    [PunRPC]
    public void Initialize(float newHealth, float newSpeed, Transform newTarget)
    {
        health = newHealth;
        speed = newSpeed;
        target = newTarget;
        transform.localScale = Vector3.one * (health / 100 / 2);
    }

    private void FollowTarget(Vector3 target, float speed)
    {
        if (Vector3.Distance(transform.position, target) > 0)
        {
            transform.LookAt(target);
            var direction = target - transform.position;
            RB.AddRelativeForce(direction.normalized * speed, ForceMode2D.Force);
        }
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (!isDamageAnimationPlaying)
            animator.SetTrigger("Take Damage");

        Instantiate(hitParticles, transform.position, Quaternion.identity);

        if (health <= 0)
        {
            Instantiate(deathParticles, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            animator.SetTrigger("Die");
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}