using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour
{
    [SerializeField] private List<Spore> _spores;

    private void OnMouseDown()
    {
        if (_spores.Count > 0)
        {
            var pos = Random.onUnitSphere * 0.25f;
            pos.z = 0;
            pos += transform.position;
            var obj = Instantiate(_spores[Random.Range(0, _spores.Count)], pos, transform.rotation);
        }
    }
}
