using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private BoxCollider2D collider;
    [SerializeField] private float moveSpeed;

    private ItemScriptableObject item;

    public void Initialize(ItemScriptableObject item) {
        this.item = item;
        sr.sprite = item.item_sprite;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            bool canAdd = InventoryManager.instance.AddItem(item);
            if(canAdd) {
                StartCoroutine(MoveAndCollect(other.transform));
            }
        }
    }

    private IEnumerator MoveAndCollect(Transform target) {
        Destroy (collider);
        while(transform.position !=  target.position) {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            yield return 0;
        }
        AudioManager.instance.Play("pickuppop", .3f);
        Destroy(gameObject);
    }
}
