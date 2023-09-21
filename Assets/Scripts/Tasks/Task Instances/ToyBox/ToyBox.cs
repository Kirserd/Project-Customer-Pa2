using UnityEngine;

public class ToyBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Toy")
        {
            AudioManager.Source.PlayOneShot(AudioManager.Clips["TBToBox"], 1f);
            Destroy(collision.gameObject);
            (Task.Instance as ToyTask).AddToy();
        }
    }
}
