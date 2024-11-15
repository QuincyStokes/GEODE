using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerAnimation))]
public class PlayerAttack : MonoBehaviour
{
    public GameObject swingable;
    public Camera cam;
    public static ItemScriptableObject currentItem;

    private bool isCoroutineRunning = false;
    private SpriteRenderer swingableSR;
    private PlayerAnimation playerAnimation;

    void Start()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        swingableSR = swingable.GetComponent<SpriteRenderer>();

        ItemManager.instance.OnItemChanged += UpdateCurrentItem;
    }

    public void Attack()
    {
        currentItem = ItemManager.instance.GetCurrentItem();
        print("Attacking");
        print(currentItem == null);
        if (currentItem != null && currentItem.swingable && !isCoroutineRunning && !UICursorDetector.IsPointerOverUI())
        {
            StartCoroutine(Swing());
        }
    }

    private IEnumerator Swing()
    {
        isCoroutineRunning = true;
        AudioManager.instance.Play("swordslash", 0.5f, false);

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;

        swingable.SetActive(true);
        Quaternion initialRotation, targetRotation;

        if (mousePos.x < transform.position.x)
        {
            swingable.transform.localRotation = Quaternion.Euler(0, 0, angle - currentItem.swingRadius);
            initialRotation = swingable.transform.localRotation;
            targetRotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            swingable.transform.localRotation = Quaternion.Euler(0, 0, angle + currentItem.swingRadius / 3);
            initialRotation = swingable.transform.localRotation;
            targetRotation = Quaternion.Euler(0, 0, angle - (currentItem.swingRadius / 3) * 2);
        }

        float elapsedTime = 0f;
        while (elapsedTime < currentItem.swingSpeed)
        {
            float t = elapsedTime / currentItem.swingSpeed;
            t = Mathf.SmoothStep(0, 1, t);
            swingable.transform.localRotation = Quaternion.Lerp(initialRotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        swingable.transform.localRotation = targetRotation;
        isCoroutineRunning = false;
        swingable.SetActive(false);
    }

    void UpdateCurrentItem(ItemScriptableObject newItem)
    {
        currentItem = newItem;
    }
}