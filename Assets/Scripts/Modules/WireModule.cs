using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe pour le module avec des fils
/// </summary>
public class WireModule : Module
{
    private const int nbWiresMin = 3;
    private const int nbWiresMax = 5;

    private Vector3 wireOffset = new(.05f, 0, 0);
    private readonly float[] wireYOffsets = { -.3f, -.15f, 0, .15f, .3f };

    private int nbWires;

    [SerializeField] private Material[] wireMaterials;
    private GameObject[] wiresPrefabs;
    private GameObject[] brokenWiresPrefabs;

    private GameObject[] placedWires;

    public override void SetupModule()
    {
        wiresPrefabs = Resources.LoadAll<GameObject>("Wires/Normal");
        brokenWiresPrefabs = Resources.LoadAll<GameObject>("Wires/BrokenWires");

        nbWires = Random.Range(nbWiresMin, nbWiresMax + 1);
        placedWires = new GameObject[nbWires];

        List<float> yOffsets = new(wireYOffsets);

        for (int i = 0; i < nbWires; i++)
        {
            float yOffset = yOffsets[Random.Range(0, yOffsets.Count)];
            yOffsets.Remove(yOffset);
            Vector3 pos = transform.position + wireOffset + new Vector3(0, yOffset, 0);
            int wireType = Random.Range(0, wiresPrefabs.Length);
            Material mat = wireMaterials[Random.Range(0, wireMaterials.Length)];
            GameObject wire = Instantiate(wiresPrefabs[wireType], pos, Quaternion.identity);
            wire.transform.SetParent(transform);
            wire.name = "Wire" + (i + 1);
            wire.GetComponent<MeshRenderer>().material = mat;
            wire.GetComponent<Wire>().OnWireClicked.AddListener(() => ReplaceWire(i, wireType, mat));
            placedWires[i] = wire;
        }
    }

    private void ReplaceWire(int wireIndex, int wireType, Material mat)
    {
        GameObject wire = placedWires[wireIndex];
        placedWires[wireIndex] = Instantiate(brokenWiresPrefabs[wireType], wire.transform.position, wire.transform.rotation);
        placedWires[wireIndex].transform.SetParent(transform);
        placedWires[wireIndex].name = "Wire" + (wireIndex + 1);
        placedWires[wireIndex].GetComponent<MeshRenderer>().material = mat;
        Destroy(wire);
    }
}
