using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private float _radius;

    [SerializeField]
    private float _stunTime = 3f;

    private void Start()
    {
        var playerMovements = FindObjectsOfType<Movement>();
        foreach(var movement in playerMovements)
        {
            var distance = Vector3.Distance(movement.transform.position, transform.position);
            Debug.Log($"Distance to player is {distance}");
            if (distance < _radius)
            {
                Debug.Log("Player stunned!");
                movement.Stun(_stunTime);
            }
        }
    }
}
