using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Transform spawnPoint;

    public void Reset()
    {
        player.transform.position = spawnPoint.transform.position;
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }
}
