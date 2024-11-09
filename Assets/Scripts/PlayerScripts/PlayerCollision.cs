using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    [SerializeField] private float recoilSpeed;
    [SerializeField] private float recoilDuration;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private PlayerMovement pm;
    // Start is called before the first frame update

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        pm = GetComponent<PlayerMovement>();
    }
    public void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {   
            print("PLAYER GOT HIT, RECOILING");
            //should recoil away from the enemy
            StartCoroutine(Recoil(other.gameObject.transform.position));
            StartCoroutine(DamageColorChange());
        }
    }

    private IEnumerator Recoil(Vector3 position) {
        pm.SetCanMove(false);
        Vector3 direction = position - transform.position;
        direction.z = 0f;
        direction *= -1;
        Vector3 recoilDirection = direction.normalized;

        rb.isKinematic = false;
        rb.velocity = recoilDirection * recoilSpeed;
        rb.isKinematic = true;
        yield return new WaitForSeconds(recoilDuration);
        rb.velocity = Vector3.zero;
        pm.SetCanMove(true);
    }

    private IEnumerator DamageColorChange() {
        Color originalColor = sr.color;
        sr.color = new Color(255, 0, 0, 255);
        yield return new WaitForSeconds(.2f);
        sr.color = originalColor;
    }
}
