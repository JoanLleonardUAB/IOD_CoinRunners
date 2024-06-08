using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _startingYPosition;

    [SerializeField]
    private float _yPositionToExplode;

    [SerializeField]
    private GameObject _explosionEffect;

    private void OnEnable()
    {
        var pos = transform.position;
        pos.y = _startingYPosition;
        transform.position = pos;
    }

    private void Update()
    {
        var pos = transform.position;
        pos.y -= _speed * Time.deltaTime;
        transform.position = pos;

        if (transform.position.y < _yPositionToExplode)
        {
            var go = Instantiate(_explosionEffect);
            pos.y = _yPositionToExplode;
            go.transform.position = pos;

            CoinBehaviour cb = GetComponent<CoinBehaviour>();
            cb.CoinGot();
        }
    }
}
