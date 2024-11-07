using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe pour le module avec des fils
/// </summary>
public class WireModule : Module
{


    private Vector3 wireOffset = new(.05f, 0, 0);
    private readonly float[] wireYOffsets = { -.3f, -.15f, 0, .15f, .3f };

    private int nbWires;

    [SerializeField] private Material[] wireMaterials;
    private GameObject[] wiresPrefabs;
    private GameObject[] brokenWiresPrefabs;

    private GameObject[] placedWires;

    public override void SetupModule(RuleHolder rules)
    {
        wiresPrefabs = Resources.LoadAll<GameObject>("Wires/Normal");
        brokenWiresPrefabs = Resources.LoadAll<GameObject>("Wires/BrokenWires");

        nbWires = Random.Range(rules.wireRuleGenerator.GetNbWiresMin(), rules.wireRuleGenerator.GetNbWiresMax());

        WireRule[] wireRules = rules.wireRuleGenerator.GetRulesFromNbWire(nbWires);

        placedWires = new GameObject[nbWires];

        List<float> yOffsets = new(wireYOffsets);

        for (int i = 0; i < nbWires; i++)
        {
            float yOffset = yOffsets[Random.Range(0, yOffsets.Count)];
            yOffsets.Remove(yOffset);
            int wireIndex = i;
            Vector3 pos = transform.position + wireOffset + new Vector3(0, yOffset, 0);
            int wireType = Random.Range(0, wiresPrefabs.Length);
            Material mat = wireMaterials[Random.Range(0, wireMaterials.Length)];
            GameObject wire = Instantiate(wiresPrefabs[wireType], pos, Quaternion.identity);
            wire.transform.SetParent(transform);
            wire.name = "Wire" + (wireIndex + 1);
            wire.GetComponent<MeshRenderer>().material = mat;
            wire.GetComponent<Wire>().OnWireClicked.AddListener(() => ReplaceWire(wireIndex, wireType, mat));
            placedWires[wireIndex] = wire;
        }
    }

    /// <summary>
    /// Remplace un fil par son équivalent cassé et vérifie si c'était un bon fil
    /// </summary>
    /// <param name="wireIndex"></param>
    /// <param name="wireType"></param>
    /// <param name="mat"></param>
    private void ReplaceWire(int wireIndex, int wireType, Material mat)
    {
        GameObject wire = placedWires[wireIndex];
        placedWires[wireIndex] = Instantiate(brokenWiresPrefabs[wireType], wire.transform.position, wire.transform.rotation);
        placedWires[wireIndex].transform.SetParent(transform);
        placedWires[wireIndex].name = "Wire" + (wireIndex + 1);
        placedWires[wireIndex].GetComponent<MeshRenderer>().material = mat;
        Destroy(wire);
    }

    public override void ModuleInteract(Ray rayInteract)
    {
        if (Physics.Raycast(rayInteract, out RaycastHit hit, 10))
        {
            if (hit.collider.TryGetComponent(out Wire wire))
            {
                wire.OnWireClicked.Invoke();
            }
        }
        GetComponent<Collider>().enabled = true;
    }
}
