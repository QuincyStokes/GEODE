using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTowerScript : MonoBehaviour
{
    public float detectionRadius = 5f;
    public float detectionAngle = 45f;
    public LayerMask enemyLayer;
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float fireRate = 1f;
    public float rotationSpeed = 5f;
    public Transform tower;


    private float nextFireTime = 0f;
    private Quaternion targetRotation;
    private bool isRotating = false;

    void Update() {
        Transform closestEnemy = GetClosestEnemy();

        if (closestEnemy != null) {
            SetTargetRotation(closestEnemy);
            RotateTowardsTarget();
            if(Time.time >= nextFireTime && isRotationComplete()) {
                Shoot(closestEnemy);
                nextFireTime = Time.time + 1f /fireRate;
            }
            
        }
    }


    Transform GetClosestEnemy() {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);
        Transform closestEmemy = null;
        float closestDistance = Mathf.Infinity;

        foreach(Collider2D enemy in enemiesInRange) {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            Vector2 directionToEnemy = (enemy.transform.position - transform.position).normalized;
            float angleToEnemy = Vector2.Angle(transform.right, directionToEnemy);
            if(distanceToEnemy < closestDistance && angleToEnemy <= detectionAngle / 2) {
                closestDistance = distanceToEnemy;
                closestEmemy = enemy.transform;
            }
        }

        return closestEmemy;
    }


    void Shoot(Transform enemy) {
        Vector2 direction = (enemy.position - firePoint.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, targetRotation); //might have to change rotation of the projectile
        print(direction);

        projectile.GetComponent<Rigidbody2D>().velocity = direction * projectile.GetComponent<Projectile>().speed; //projectile needs a speed attribute
        print(projectile.GetComponent<Rigidbody2D>().velocity);
    }

    void SetTargetRotation(Transform enemy) {
        Vector2 direction = (enemy.position - tower.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if(angle <= detectionAngle / 2) {
            targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            isRotating = true;
        }
        
    }

    void RotateTowardsTarget() {
        if (isRotating) {
            tower.rotation = Quaternion.Slerp(tower.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    bool isRotationComplete() {
        if(Quaternion.Angle(tower.rotation, targetRotation) < 1f) {
            isRotating = false;
            return true;
        }
        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Vector3 leftBoundary = Quaternion.Euler(0, 0, -detectionAngle / 2) * transform.right * detectionRadius;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, detectionAngle / 2) * transform.right * detectionRadius;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
    }
}
