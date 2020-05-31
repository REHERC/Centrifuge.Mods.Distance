#pragma warning disable CS0618 // Type or member is obsolete
#pragma warning disable CS0162 // unreachable code detected
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Distance.TextureModifier
{
    public class ResourceModificator
    {
        private readonly TextureLoader textureLoader_;

        public ResourceModificator(TextureLoader textureLoader)
        {
            textureLoader_ = textureLoader;
        }

        public void Patch(object any)
        {
            switch (any)
            {
                case Material _material:
                    PatchMaterial(_material);
                    break;
                case GameObject _gameObject:
                    PatchGameObject(_gameObject);
                    break;
                case Renderer _renderer:
                    PatchRenderer(_renderer);
                    break;
            }
        }

        public void PatchCollection(IEnumerable collection)
        {
            foreach (var element in collection)
            {
                Patch(element);
            }
        }

        public void PatchMaterial(Material material)
        {
            if (Declarations.IsBlacklisted(material))
            {
                return;
            }

            bool applyPatch = false;

            foreach (var property in Declarations.materialTextureProperties)
            {
                if (material.HasProperty(property))
                {
                    applyPatch = true;
                    break;
                }
            }

            if (applyPatch)
            {
                material.CopyPropertiesFromMaterial(textureLoader_.GetRandomMaterial());
            }
        }

        public void PatchMaterials(IEnumerable<Object> materials)
        {
            foreach (var material in materials)
            {
                PatchMaterial(material as Material);
            }
        }

        public void PatchGameObject(GameObject gameObject)
        {
            if (gameObject)
            {
                foreach (var renderer in gameObject.GetComponents<Renderer>().Concat(gameObject.GetComponentsInChildren<Renderer>(true)))
                {
                    PatchRenderer(renderer);
                }
            }
        }

        public void PatchGameObjects(IEnumerable<Object> gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                PatchGameObject(gameObject as GameObject);
            }
        }

        public void PatchRenderer(Renderer renderer)
        {
            if (renderer)
            {
                switch (renderer)
                {
                    case MeshRenderer _:
                    case SkinnedMeshRenderer _:
                    case ParticleRenderer _:
                    case ParticleSystemRenderer _:
                    default:
                        PatchMaterial(renderer.material);
                        PatchMaterials(renderer.materials);
                        PatchMaterial(renderer.sharedMaterial);
                        PatchMaterials(renderer.sharedMaterials);

                        /*
                        Material material = textureLoader_.GetRandomMaterial();

                        ReplaceMaterials(renderer.materials, material);
                        ReplaceMaterials(renderer.sharedMaterials, material);

                        renderer.material = material;
                        renderer.sharedMaterial = material;
                        */
                        break;
                }
            }
        }

        public void ReplaceMaterials(Material[] collection, Material replacement)
        {
            for(int index = 0; index < collection.Length; index++)
            {
                collection[index] = replacement;
            }
        }

        public void PatchRenderers(IEnumerable<Object> renderers)
        {
            foreach (var renderer in renderers)
            {
                PatchRenderer(renderer as Renderer);
            }
        }
    }
}
