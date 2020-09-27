using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loot : MonoBehaviour
{
    static float value = 10f;
    public Material hoverMaterial;
    private Material[] hoverMaterials;
    private Material[] originalMaterials;
    public new Renderer renderer;
    public ItemClass item;
    public GameObject LootTextPrefab;
    private GameObject LootObject;

    private void OnMouseEnter()
    {
        LootObject.SetActive(true);
        //renderer.materials = hoverMaterials;
        LootObject.transform.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnMouseExit()
    {
        //renderer.materials = originalMaterials;
        LootObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (Vector3.Distance(transform.position, transform.position) < 1f)
        {
            ItemScript newItem = ItemDatabase.Instance.ItemEquipPool.GetObject().GetComponent<ItemScript>();
            newItem.SetItemObject(item);
            ItemScript.SetSelectedItem(newItem);
        }
        Destroy(gameObject);
    }

    IEnumerator Start()
    {
        bool stillFlying = true;
        Transform child = transform.GetChild(0);
        Quaternion originalRotation = child.rotation;
        originalMaterials = new Material[renderer.materials.Length];
        renderer.materials.CopyTo(originalMaterials, 0);
        hoverMaterials = new Material[originalMaterials.Length];
        for (int i = 0; i < hoverMaterials.Length; i++)
            hoverMaterials[i] = hoverMaterial;
        Vector3 Velocity = new Vector3(Random.Range(-value, value), 10f, Random.Range(-value, value));
        LootObject = Instantiate(LootTextPrefab, ItemDatabase.Instance.LootParent);
        Text lootText = LootObject.transform.GetChild(0).GetComponent<Text>();
        lootText.text = item.TypeName;
        lootText.color = item.Quality.Colour;
        LootObject.SetActive(false);

        while (stillFlying)
        {
            Vector3 pos = transform.position + Velocity * Time.deltaTime;

            child.Rotate(Velocity * 0.1f);

            if (Velocity.y < 0)
            {
                if (pos.y < 0f)
                {
                    pos.y = 0f;
                    stillFlying = false;
                    child.rotation = originalRotation;
                }
            }

            transform.position = pos;

            Velocity += Vector3.up * -30.0f * Time.deltaTime;

            yield return null;
        }
    }

    private void OnDestroy()
    {
        GameObject.Destroy(LootObject);
    }
}