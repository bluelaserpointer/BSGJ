using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangeOnTimeShift : MonoBehaviour
{
    [SerializeField] private Color32 pastColor;
    [SerializeField] private Color32 currentColor;
    [SerializeField] private List<GameObject> gameObjects;
    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        //switch (WorldManager.Instance.Timeline)
        //{
        //    case Timeline.Past:
        //        foreach (GameObject gameObject in gameObjects) {
        //            gameObject.GetComponent<SpriteRenderer>().color = pastColor;
        //        }
        //        break;
        //    case Timeline.Current:
        //        foreach (GameObject gameObject in gameObjects)
        //        {
        //            gameObject.GetComponent<SpriteRenderer>().color = currentColor;
        //        }
        //        break;
        //}
        foreach (GameObject gameObject in gameObjects)
        {
            switch (WorldManager.Instance.Timeline)
            {
                case Timeline.Past:
                    gameObject.GetComponent<SpriteRenderer>().color = pastColor;
                    break;
                case Timeline.Current:
                    gameObject.GetComponent<SpriteRenderer>().color = currentColor;
                    break;
            }
        }
    }



}
