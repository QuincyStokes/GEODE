using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableScript : MonoBehaviour
{
    public float maxHealth = 10f;
    private float currentHealth;

    public GameObject HealthBar;
    public GameObject HealthBarGreen;
    public SpriteRenderer spriteRenderer;
    //public GameObject gameObject;
    private Color originalColor;

    private Vector3 initialHealthBarScale;
    private bool isShaking;
    private float shakeSpeed = 1.0f;
    private float shakeAmount = 0.05f;
    private float startingX;
    private float startingY;


    void Start() {
        currentHealth = maxHealth;
        initialHealthBarScale = HealthBarGreen.transform.localScale;
        HealthBar.SetActive(false);
        originalColor = spriteRenderer.material.color; 
        startingX = transform.localPosition.x;
        startingY = transform.localPosition.y;
    }

    void Update(){
        if(isShaking) {
            float newX = startingX + Mathf.Sin(Time.time * shakeSpeed )* shakeAmount;
            float newY = startingY + Mathf.Sin(Time.time * shakeSpeed )* shakeAmount;
            transform.position = new Vector3(newX, newY, transform.localPosition.z);
        }
    }

    public void TakeDamage(float damage) {
        currentHealth -= damage;
        HealthBar.SetActive(true);
        //Change the HealthBarGreen's scale to currentHealth/maxHealth%
        float healthRatio = currentHealth/ maxHealth;
        HealthBarGreen.transform.localScale = new Vector3(HealthBarGreen.transform.localScale.x *healthRatio, HealthBarGreen.transform.localScale.y, HealthBarGreen.transform.localScale.z);
        //HealthBarGreen.transform.localPosition = new Vector3((healthRatio - 1) * initialHealthBarScale.x / 2, HealthBarGreen.transform.localPosition.y, HealthBarGreen.transform.localPosition.z);
        StopAllCoroutines();
        StartCoroutine(HitColorChange());
        AudioManager.instance.PlayAtPosition("treehit", transform.position);
        if (currentHealth <= 0f) {
            DestroyTree();
        }
    }

    private IEnumerator HitColorChange(){
        isShaking = true;
        spriteRenderer.color = new Color (255, 255, 255, 0);

        yield return new WaitForSeconds(0.05f);
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.05f);
        spriteRenderer.color = originalColor;
        isShaking = false;
        transform.position = new Vector3(startingX, startingY, transform.localPosition.z);
    }

    private void DestroyTree() {
        Destroy(gameObject);
    }


}
