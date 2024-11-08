using System.Collections.Generic;
using System.Linq;
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

    private List<Material> mandatoryMaterials;
    private List<Material> forbiddenMaterials;
    private List<int> mandatoryTypes;
    private List<int> forbiddenTypes;

    private GameObject[] placedWires;

    public override void SetupModule(RuleHolder rules)
    {
        wiresPrefabs = Resources.LoadAll<GameObject>("Wires/Normal");
        brokenWiresPrefabs = Resources.LoadAll<GameObject>("Wires/BrokenWires");

        nbWires = Random.Range(rules.wireRuleGenerator.GetNbWiresMin(), rules.wireRuleGenerator.GetNbWiresMax());

        WireRule[] wireRules = rules.wireRuleGenerator.GetRulesFromNbWire(nbWires);

        WireRule targetRule = wireRules[Random.Range(0, wireRules.Length)];

        mandatoryMaterials = new List<Material>();
        forbiddenMaterials = new List<Material>();
        mandatoryTypes = new List<int>();
        forbiddenTypes = new List<int>();

        switch (targetRule.condition)
        {
            //TODO : Ajouter les autres conditions
            case WireConditionTarget.Material:
                if (targetRule.invertCondition)
                {
                    forbiddenMaterials.Add(wireMaterials[(int)(WireMaterials)targetRule.targetType]);
                }
                else
                {
                    mandatoryMaterials.Add(wireMaterials[(int)(WireMaterials)targetRule.targetType]);
                }
                break;
            case WireConditionTarget.Type:
                if (targetRule.invertCondition)
                {
                    forbiddenTypes.Add((int)(WireType)targetRule.targetType);
                }
                else
                {
                    mandatoryTypes.Add((int)(WireType)targetRule.targetType);
                }
                break;

        }

        foreach (WireRule constraint in targetRule.constraints)
        {
            switch (constraint.condition)
            {
                //TODO : Ajouter les autres conditions
                case WireConditionTarget.Material:
                    if (constraint.invertCondition)
                    {
                        forbiddenMaterials.Add(wireMaterials[(int)(WireMaterials)constraint.targetType]);
                    }
                    else
                    {
                        mandatoryMaterials.Add(wireMaterials[(int)(WireMaterials)constraint.targetType]);
                    }
                    break;
                case WireConditionTarget.Type:
                    if (constraint.invertCondition)
                    {
                        forbiddenTypes.Add((int)(WireType)constraint.targetType);
                    }
                    else
                    {
                        mandatoryTypes.Add((int)(WireType)constraint.targetType);
                    }
                    break;

            }
        }

        Debug.Log("Target Rule : " + targetRule.GetRuleString());

        Debug.Log("Mandatory Materials : " + string.Join(", ", mandatoryMaterials));
        Debug.Log("Forbidden Materials : " + string.Join(", ", forbiddenMaterials));
        Debug.Log("Mandatory Types : " + string.Join(", ", mandatoryTypes));
        Debug.Log("Forbidden Types : " + string.Join(", ", forbiddenTypes));

        placedWires = new GameObject[nbWires];

        // Prepare lists of available materials and types, excluding forbidden items
        List<Material> availableMaterials = new(wireMaterials);
        availableMaterials.RemoveAll(mat => forbiddenMaterials.Contains(mat));

        List<int> availableTypes = new();
        for (int i = 0; i < wiresPrefabs.Length; i++)
        {
            if (!forbiddenTypes.Contains(i))
                availableTypes.Add(i);
        }

        List<float> yOffsets = new(wireYOffsets);
        List<Material> selectedMaterials = new();
        List<int> selectedTypes = new();

        // Ensure all mandatory materials and types are selected
        selectedMaterials.AddRange(mandatoryMaterials);
        selectedTypes.AddRange(mandatoryTypes);

        // Randomly fill up remaining slots with available materials and types
        while (selectedMaterials.Count < nbWires)
        {
            selectedMaterials.Add(availableMaterials[Random.Range(0, availableMaterials.Count)]);
        }

        while (selectedTypes.Count < nbWires)
        {
            selectedTypes.Add(availableTypes[Random.Range(0, availableTypes.Count)]);
        }

        // Shuffle lists to randomize order
        selectedMaterials = selectedMaterials.OrderBy(x => Random.value).ToList();
        selectedTypes = selectedTypes.OrderBy(x => Random.value).ToList();

        // Place wires at random positions with selected materials and types
        for (int i = 0; i < nbWires; i++)
        {
            float yOffset = yOffsets[Random.Range(0, yOffsets.Count)];
            yOffsets.Remove(yOffset);  // Remove used offset

            int wireIndex = i;
            Vector3 pos = transform.position + wireOffset + new Vector3(0, yOffset, 0);

            // Select a material and type for this wire
            Material mat = selectedMaterials[i];
            int wireType = selectedTypes[i];

            // Instantiate and setup the wire GameObject
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
