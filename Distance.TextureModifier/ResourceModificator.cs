#pragma warning disable CS0618 // Type or member is obsolete
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

            //Entry.Instance.Logger.Info(material.name);

            Texture texture = textureLoader_.GetRandomTexture();

            //material.shader = Shader.Find(Declarations.StandardShader);
            material.mainTexture = texture;
            //material.color = Colors.white;

            foreach (var property in Declarations.materialTextureProperties)
            {
                if (material.HasProperty(property))
                {
                    try
                    {
                        material.SetTexture(property, texture);
                        //material.SetTextureOffset(property, new Vector2(0, 0));
                        //material.SetTextureScale(property, new Vector2(1, 1));
                    }
                    catch (System.Exception)
                    {

                    }
                }
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
            foreach (var renderer in gameObject.GetComponents<Renderer>().Concat(gameObject.GetComponentsInChildren<Renderer>(true)))
            {
                PatchRenderer(renderer);
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
                    break;
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
