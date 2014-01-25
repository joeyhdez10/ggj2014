using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Controls layer visiblity.
/// </summary>
public class LayerManager : MonoBehaviour
{
	#region Public Inspector Fields

	public Material activeBlueMaterial;
	public Material activeGreenMaterial;
	public Material activeRedMaterial;
	public Material activeYellowMaterial;

	public Material inactiveBlueMaterial;
	public Material inactiveGreenMaterial;
	public Material inactiveRedMaterial;
	public Material inactiveYellowMaterial;

	#endregion

	public static Dictionary<LayerColor, Layer> layers { get; private set; }
	public static Dictionary<LayerColor, Material> activeMaterials { get; private set; }
	public static Dictionary<LayerColor, Material> inactiveMaterials { get; private set; }

	void Awake ()
	{
		SetMaterials();
		SetLayers();
	}

	void SetMaterials ()
	{
		activeMaterials = new Dictionary<LayerColor, Material>()
		{
			{ LayerColor.Blue, activeBlueMaterial },
			{ LayerColor.Green, activeGreenMaterial },
			{ LayerColor.Red, activeRedMaterial },
			{ LayerColor.Yellow, activeYellowMaterial },
		};

		inactiveMaterials = new Dictionary<LayerColor, Material>()
		{
			{ LayerColor.Blue, inactiveBlueMaterial },
			{ LayerColor.Green, inactiveGreenMaterial },
			{ LayerColor.Red, inactiveRedMaterial },
			{ LayerColor.Yellow, inactiveYellowMaterial },
		};
	}

	void SetLayers ()
	{
		var layerComponents = GameObject.FindGameObjectsWithTag("Layer")
			.Select(go => go.GetComponent<Layer>())
			.ToArray();

		layers = new Dictionary<LayerColor, Layer>()
		{
			{ LayerColor.Blue, layerComponents.First(layer => layer.color == LayerColor.Blue) },
			{ LayerColor.Green, layerComponents.First(layer => layer.color == LayerColor.Green) },
			{ LayerColor.Yellow, layerComponents.First(layer => layer.color == LayerColor.Yellow) },
			{ LayerColor.Red, layerComponents.First(layer => layer.color == LayerColor.Red) },
		};

		// Ensure all layers start out disabled.
		foreach (var layer in layers.Values) {
			layer.active = false;
		}
	}

	public static void AssignLayer (Player player)
	{
		var inactiveLayers = layers
			.Where(kvp => !kvp.Value.active)
			.Select(kvp => kvp.Value)
			.ToArray();

		var layer2 = inactiveLayers[Random.Range(0, inactiveLayers.Length)];
		layer2.active = true;
		player.layer = layer2;
	}

	public static void ToggleLayer (Player player, Layer newLayer)
	{
		if (newLayer.active) {
			Debug.LogWarning("Trying to switch to layer that's already active.");
			return;
		}

		var oldLayer = player.layer;

		Debug.Log(string.Format("Toggling layer from {0} to {1}.", oldLayer.color, newLayer.color));

		oldLayer.active = false;
		newLayer.active = true;
		player.layer = newLayer;
	}
}
