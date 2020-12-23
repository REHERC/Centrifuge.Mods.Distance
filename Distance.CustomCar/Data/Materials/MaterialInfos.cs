using UnityEngine;

namespace Distance.CustomCar.Data.Materials
{
	public class MaterialInfos
	{
		public Material material;
		public int diffuseIndex = -1;
		public int normalIndex = -1;
		public int emitIndex = -1;

		public void ReplaceMaterialInRenderer(Renderer renderer, int materialIndex)
		{
			if (material == null || renderer == null || materialIndex >= renderer.materials.Length)
			{
				return;
			}

			ref Material currentMaterial = ref renderer.materials[materialIndex];

			Material mat = Object.Instantiate(material);

			if (diffuseIndex >= 0)
			{
				mat.SetTexture(diffuseIndex, currentMaterial.GetTexture("_MainTex")); //diffuse
			}

			if (emitIndex >= 0)
			{
				mat.SetTexture(emitIndex, currentMaterial.GetTexture("_EmissionMap")); //emissive
			}

			if (normalIndex >= 0)
			{
				mat.SetTexture(normalIndex, currentMaterial.GetTexture("_BumpMap")); //normal
			}

			renderer.materials[materialIndex] = mat;
		}
	}
}
