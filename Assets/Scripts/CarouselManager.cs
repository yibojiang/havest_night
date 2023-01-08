using UnityEngine;
using System.Collections.Generic;

class CarouselManager : MonoBehaviour
{
    [SerializeField] private GameObject[] items;
    private float itemWidth;
    private int currentLeft;

    public void Start()
    {
        itemWidth = (items[items.Length - 1].transform.position.x - items[0].transform.position.x) / items.Length;
        currentLeft = 0;
    }

    public void Update()
    {
        //update background
        if (Camera.main.WorldToViewportPoint(items[(currentLeft + 1)% items.Length].transform.position).x < 0f)
        {
            Vector3 newPosition = new Vector3(items[(currentLeft - 1 + items.Length) % items.Length].transform.position.x + itemWidth, 
                items[currentLeft].transform.position.y, 
                items[currentLeft].transform.position.z);
            items[currentLeft].transform.position = newPosition;

            currentLeft = (currentLeft + 1) % items.Length;
        }
    }
}

