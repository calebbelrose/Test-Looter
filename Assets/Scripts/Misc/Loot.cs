using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Loot : MonoBehaviour
{
    public ItemClass Item;

    [SerializeField] private Material HoverMaterial;
    [SerializeField] private Renderer Renderer;
    [SerializeField] private GameObject LootTextPrefab;
    [SerializeField] private Rigidbody Rigidbody;

    private GameObject LootObject;
    private Material[] hoverMaterials;
    private Material[] originalMaterials;
    private Transform child;
    private Quaternion originalRotation;

    private static float value = 250f;

    //Highlights loot and displays its name
    private void OnMouseEnter()
    {
        LootObject.SetActive(true);
        Renderer.materials = hoverMaterials;
        LootObject.transform.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    //Removes highlight from loot and hides its name
    private void OnMouseExit()
    {
        Renderer.materials = originalMaterials;
        LootObject.SetActive(false);
    }

    //Picks up loot
    private void OnMouseDown()
    {
        if (Vector3.Distance(transform.position, transform.position) < 1f)
        {
            ItemScript newItem = ItemDatabase.Instance.ItemEquipPool.GetItemScript();

            newItem.SetItemObject(Item);
            
            if (InvenGridManager.Instance.isActiveAndEnabled)
            {
                ItemScript.SetSelectedItem(newItem);
                Destroy(gameObject);
            }
            else if (InvenGridManager.Instance.StoreLoot(newItem))
                Destroy(gameObject);
            else
                StartCoroutine("DropLoot");
        }
    }

    //Drops loot
    IEnumerator DropLoot()
    {
        Vector3 Velocity;
        bool stillFlying = true;

        Velocity = new Vector3(0f, 10f, 0f);

        while (stillFlying)
        {
            Vector3 pos = transform.position + Velocity * Time.deltaTime;

            child.Rotate(Velocity * 0.1f);

            if (Velocity.y < 0 && pos.y < 0f)
            {
                pos.y = 0f;
                stillFlying = false;
                child.rotation = originalRotation;
            }

            transform.position = pos;
            Velocity += Vector3.up * -30.0f * Time.deltaTime;
            yield return null;
        }
    }

    //Sets up loot and drops it
    void Start()
    {
        Text lootText;

        child = transform.GetChild(0);
        originalRotation = child.rotation;
        originalMaterials = new Material[Renderer.materials.Length];
        Renderer.materials.CopyTo(originalMaterials, 0);
        hoverMaterials = new Material[originalMaterials.Length];
        Rigidbody.AddForce(Random.Range(-value, value), 0f, Random.Range(-value, value));
        LootObject = ItemDatabase.Instance.CreateLoot(LootTextPrefab);
        lootText = LootObject.transform.GetChild(0).GetComponent<Text>();
        lootText.text = Item.TypeName;
        lootText.color = Item.Quality.Colour;
        LootObject.SetActive(false);

        for (int i = 0; i < hoverMaterials.Length; i++)
            hoverMaterials[i] = HoverMaterial;

        StartCoroutine("DropLoot");
    }

    //Destroys gameobject
    private void OnDestroy()
    {
        GameObject.Destroy(LootObject);
    }
}