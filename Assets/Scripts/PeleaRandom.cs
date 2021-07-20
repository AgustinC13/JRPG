using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class PeleaRandom : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            switch (Random.Range(0, 4))
            {
                case 0:
                    EditorSceneManager.LoadScene("Batalla");
                    break;
                default:
                    
                    break;
            }
        }
    }
}
