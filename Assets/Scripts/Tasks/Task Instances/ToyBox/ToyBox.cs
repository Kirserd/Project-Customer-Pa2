using UnityEngine;

public class ToyBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Toy")
        {
            Destroy(collision.gameObject);
            (Task.Instance as ToyTask).AddToy();
        }
    }
}
