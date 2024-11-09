using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealthManager : MonoBehaviour
{
    //temporary
    public TMP_Text healthText;
    [SerializeField] private float playerMaxHealth;
    [SerializeField] private GameObject HealthBarGreen;

    void Start() {
        Health = playerMaxHealth;
        //healthText.text = playerMaxHealth.ToString();
    }
    private float Health {
        set {
            health = value;
            if (health <= 0)
            {
                //dead
            }
        } get {
            return health;
        }
    }
    private float health;

    public void TakeDamage(float damage) 
    {
        Health -= damage;
        UpdateHealthbar();
        float hitDamagePercent = damage / playerMaxHealth;
        HealthBarGreen.transform.localScale = new Vector3(HealthBarGreen.transform.localScale.x - hitDamagePercent, HealthBarGreen.transform.localScale.y, HealthBarGreen.transform.localScale.z);

    }

    private void UpdateHealthbar() {
        //healthText.text = Health.ToString();
    }
}
