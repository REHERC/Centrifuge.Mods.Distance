using Reactor.API;
using Reactor.API.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CustomCar.Legacy
{
    public class CarBuilder
    {
        private CarInfos _infos;

        public void CreateCars(CarInfos infos)
        {
            _infos = infos;
            List<GameObject> cars = LoadAssetsBundle();

            List<CreateCarReturnInfos> carsInfos = new List<CreateCarReturnInfos>();

            foreach (GameObject car in cars)
            {
                carsInfos.Add(CreateCar(car));
            }

            ProfileManager profileManager = G.Sys.ProfileManager_;
            CarInfo[] oldCars = profileManager.carInfos_.ToArray();
            profileManager.carInfos_ = new CarInfo[oldCars.Length + cars.Count];

            Dictionary<string, int> unlocked = profileManager.unlockedCars_;
            Dictionary<string, int> knowCars = profileManager.knownCars_;

            for (int i = 0; i < profileManager.carInfos_.Length; i++)
            {
                if (i < oldCars.Length)
                {
                    profileManager.carInfos_[i] = oldCars[i];
                    continue;
                }

                int index = i - oldCars.Length;

                CarInfo car = new CarInfo
                {
                    name_ = carsInfos[index].car.name,
                    prefabs_ = new CarPrefabs
                    {
                        carPrefab_ = carsInfos[index].car
                    },
                    colors_ = carsInfos[index].colors
                };
                profileManager.carInfos_[i] = car;
                unlocked.Add(car.name_, i);
                knowCars.Add(car.name_, i);
            }

            CarColors[] carColors = new CarColors[oldCars.Length + cars.Count];
            for (int i = 0; i < carColors.Length; i++)
            {
                carColors[i] = G.Sys.ProfileManager_.carInfos_[i].colors_;
            }

            for (int i = 0; i < profileManager.ProfileCount_; i++)
            {
                Profile p = profileManager.GetProfile(i);

                CarColors[] oldColorList = p.carColorsList_;
                for (int j = 0; j < oldColorList.Length && j < carColors.Length; j++)
                {
                    carColors[j] = oldColorList[j];
                }

                p.carColorsList_ = carColors;
            }
        }

        private List<GameObject> LoadAssetsBundle()
        {
            string dirStr = Path.Combine(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location), Defaults.PrivateAssetsDirectory);
            string[] files = Directory.GetFiles(dirStr);

            List<GameObject> cars = new List<GameObject>();

            foreach (string f in files)
            {
                int index = f.LastIndexOf('/');
                if (index < 0)
                {
                    index = f.LastIndexOf('\\');
                }
                string name = f.Substring(index + 1);
                Assets asset = new Assets(name);

                GameObject car = null;
                AssetBundle bundle = (AssetBundle)asset.Bundle;

                foreach (string n in bundle.GetAllAssetNames())
                {
                    if (car == null && n.EndsWith(".prefab"))
                    {
                        car = bundle.LoadAsset<GameObject>(n);
                        break;
                    }
                }

                if (car == null)
                {
                    ErrorList.Add("Can't find a prefab in the asset bundle " + f);
                }
                else
                {
                    car.name = name;
                    cars.Add(car);
                }
            }

            return cars;
        }

        private CreateCarReturnInfos CreateCar(GameObject car)
        {
            CreateCarReturnInfos infos = new CreateCarReturnInfos();

            GameObject obj = Object.Instantiate(_infos.baseCar);
            obj.name = car.name;
            Object.DontDestroyOnLoad(obj);
            obj.SetActive(false);

            RemoveOldCar(obj);
            GameObject newCar = AddNewCarOnPrefab(obj, car);
            SetCarDatas(obj, newCar);
            infos.car = obj;

            infos.colors = LoadDefaultColors(newCar);

            return infos;
        }

        private void RemoveOldCar(GameObject obj)
        {
            List<GameObject> wheelsToRemove = new List<GameObject>();
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                GameObject c = obj.transform.GetChild(i).gameObject;
                if (c.name.ToLower().Contains("wheel"))
                {
                    wheelsToRemove.Add(c);
                }
            }

            if (wheelsToRemove.Count != 4)
            {
                ErrorList.Add("Found " + wheelsToRemove.Count + " wheels on base prefabs, expected 4");
            }

            Transform refractor = obj.transform.Find("Refractor");
            if (refractor == null)
            {
                ErrorList.Add("Can't find the Refractor object on the base car prefab");
                return;
            }

            Object.Destroy(refractor.gameObject);
            foreach (GameObject w in wheelsToRemove)
            {
                Object.Destroy(w);
            }
        }

        private GameObject AddNewCarOnPrefab(GameObject obj, GameObject car)
        {
            return Object.Instantiate(car, obj.transform);
        }

        private void SetCarDatas(GameObject obj, GameObject car)
        {
            SetColorChanger(obj.GetComponent<ColorChanger>(), car);
            SetCarVisuals(obj.GetComponent<CarVisuals>(), car);
        }

        private void SetColorChanger(ColorChanger colorChanger, GameObject car)
        {
            if (colorChanger == null)
            {
                ErrorList.Add("Can't find the ColorChanger component on the base car");
                return;
            }

            colorChanger.rendererChangers_ = new ColorChanger.RendererChanger[0];
            foreach (Renderer r in car.GetComponentsInChildren<Renderer>())
            {
                ReplaceMaterials(r);
                if (colorChanger != null)
                {
                    AddMaterialColorChanger(colorChanger, r.transform);
                }
            }
        }

        private void ReplaceMaterials(Renderer r)
        {
            string[] matNames = new string[r.materials.Length];
            for (int i = 0; i < matNames.Length; i++)
            {
                matNames[i] = "wheel";
            }

            List<MaterialPropertyExport>[] matProperties = new List<MaterialPropertyExport>[r.materials.Length];
            for (int i = 0; i < matProperties.Length; i++)
            {
                matProperties[i] = new List<MaterialPropertyExport>();
            }

            FillMaterialInfos(r, matNames, matProperties);

            Material[] materials = r.materials.ToArray();
            for (int i = 0; i < r.materials.Length; i++)
            {
                if (!_infos.materials.TryGetValue(matNames[i], out MaterialInfos matInfo))
                {
                    ErrorList.Add("Can't find the material " + matNames[i] + " on " + r.gameObject.FullName());
                    continue;
                }

                if (matInfo == null || matInfo.material == null)
                {
                    continue;
                }

                Material mat = Object.Instantiate(matInfo.material);
                if (matInfo.diffuseIndex >= 0)
                {
                    mat.SetTexture(matInfo.diffuseIndex, r.materials[i].GetTexture("_MainTex"));
                }

                if (matInfo.normalIndex >= 0)
                {
                    mat.SetTexture(matInfo.normalIndex, r.materials[i].GetTexture("_BumpMap"));
                }

                if (matInfo.emitIndex >= 0)
                {
                    mat.SetTexture(matInfo.emitIndex, r.materials[i].GetTexture("_EmissionMap"));
                }

                foreach (MaterialPropertyExport p in matProperties[i])
                {
                    CopyMaterialProperty(r.materials[i], mat, p);
                }

                materials[i] = mat;
            }

            r.materials = materials;
        }

        private void FillMaterialInfos(Renderer r, string[] matNames, List<MaterialPropertyExport>[] matProperties)
        {
            int childCount = r.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                string name = r.transform.GetChild(i).name.ToLower();
                if (!name.StartsWith("#"))
                {
                    continue;
                }

                name = name.Remove(0, 1);

                string[] s = name.Split(';');
                if (s.Length == 0)
                {
                    continue;
                }

                if (s[0].Contains("mat"))
                {
                    if (s.Length != 3)
                    {
                        ErrorList.Add(s[0] + " property on " + r.gameObject.FullName() + " must have 2 arguments");
                        continue;
                    }

                    if (!int.TryParse(s[1], out int index))
                    {
                        ErrorList.Add("First argument of " + s[0] + " on " + r.gameObject.FullName() + " property must be a number");
                        continue;
                    }

                    if (index < matNames.Length)
                    {
                        matNames[index] = s[2];
                    }
                }
                else if (s[0].Contains("export"))
                {
                    if (s.Length != 5)
                    {
                        ErrorList.Add(s[0] + " property on " + r.gameObject.FullName() + " must have 4 arguments");
                        continue;
                    }

                    if (!int.TryParse(s[1], out int index))
                    {
                        ErrorList.Add("First argument of " + s[0] + " on " + r.gameObject.FullName() + " property must be a number");
                        continue;
                    }

                    if (index >= matNames.Length)
                    {
                        continue;
                    }

                    MaterialPropertyExport p = new MaterialPropertyExport();
                    bool found = false;

                    foreach (MaterialPropertyExport.PropertyType pType in Enum.GetValues(typeof(MaterialPropertyExport.PropertyType)))
                    {
                        if (s[2] == pType.ToString().ToLower())
                        {
                            found = true;
                            p.type = pType;
                            break;
                        }
                    }

                    if (!found)
                    {
                        ErrorList.Add("The property " + s[2] + " on " + r.gameObject.FullName() + " is not valid");
                        continue;
                    }

                    if (!int.TryParse(s[3], out p.fromID))
                    {
                        p.fromName = s[3];
                    }

                    if (!int.TryParse(s[4], out p.toID))
                    {
                        p.toName = s[4];
                    }

                    matProperties[index].Add(p);
                }
            }
        }

        private void CopyMaterialProperty(Material from, Material to, MaterialPropertyExport property)
        {
            int fromID = property.fromID;

            if (fromID == -1)
            {
                fromID = Shader.PropertyToID(property.fromName);
            }

            int toID = property.toID;

            if (toID == -1)
            {
                toID = Shader.PropertyToID(property.toName);
            }

            switch (property.type)
            {
                case MaterialPropertyExport.PropertyType.Color:
                    to.SetColor(toID, from.GetColor(fromID));
                    break;
                case MaterialPropertyExport.PropertyType.ColorArray:
                    to.SetColorArray(toID, from.GetColorArray(fromID));
                    break;
                case MaterialPropertyExport.PropertyType.Float:
                    to.SetFloat(toID, from.GetFloat(fromID));
                    break;
                case MaterialPropertyExport.PropertyType.FloatArray:
                    to.SetFloatArray(toID, from.GetFloatArray(fromID));
                    break;
                case MaterialPropertyExport.PropertyType.Int:
                    to.SetInt(toID, from.GetInt(fromID));
                    break;
                case MaterialPropertyExport.PropertyType.Matrix:
                    to.SetMatrix(toID, from.GetMatrix(fromID));
                    break;
                case MaterialPropertyExport.PropertyType.MatrixArray:
                    to.SetMatrixArray(toID, from.GetMatrixArray(fromID));
                    break;
                case MaterialPropertyExport.PropertyType.Texture:
                    to.SetTexture(toID, from.GetTexture(fromID));
                    break;
                case MaterialPropertyExport.PropertyType.Vector:
                    to.SetVector(toID, from.GetVector(fromID));
                    break;
                case MaterialPropertyExport.PropertyType.VectorArray:
                    to.SetVectorArray(toID, from.GetVectorArray(fromID));
                    break;
            }
        }

        private void AddMaterialColorChanger(ColorChanger colorChanger, Transform t)
        {
            Renderer r = t.GetComponent<Renderer>();

            if (r == null)
            {
                return;
            }

            List<ColorChanger.UniformChanger> uniformChangers = new List<ColorChanger.UniformChanger>();

            for (int i = 0; i < t.childCount; i++)
            {
                GameObject o = t.GetChild(i).gameObject;
                string name = o.name.ToLower();

                if (!name.StartsWith("#"))
                {
                    continue;
                }

                name = name.Remove(0, 1); //remove #
                string[] s = name.Split(';');

                if (s.Length == 0)
                {
                    continue;
                }

                if (!s[0].Contains("color"))
                {
                    continue;
                }

                if (s.Length != 6)
                {
                    ErrorList.Add(s[0] + " property on " + t.gameObject.FullName() + " must have 5 arguments");
                    continue;
                }

                ColorChanger.UniformChanger uniformChanger = new ColorChanger.UniformChanger();
                int.TryParse(s[1], out int materialIndex);
                uniformChanger.materialIndex_ = materialIndex;
                uniformChanger.colorType_ = ColorType(s[2]);
                uniformChanger.name_ = UniformName(s[3]);
                float.TryParse(s[4], out float multiplier);
                uniformChanger.mul_ = multiplier;
                uniformChanger.alpha_ = s[5].ToLower() == "true";

                uniformChangers.Add(uniformChanger);
            }

            if (uniformChangers.Count == 0)
            {
                return;
            }

            ColorChanger.RendererChanger renderChanger = new ColorChanger.RendererChanger
            {
                renderer_ = r,
                uniformChangers_ = uniformChangers.ToArray()
            };

            List<ColorChanger.RendererChanger> renders = colorChanger.rendererChangers_.ToList();
            renders.Add(renderChanger);
            colorChanger.rendererChangers_ = renders.ToArray();
        }

        private ColorChanger.ColorType ColorType(string name)
        {
            name = name.ToLower();

            switch (name)
            {
                case "secondary":
                    return ColorChanger.ColorType.Secondary;
                case "glow":
                    return ColorChanger.ColorType.Glow;
                case "sparkle":
                    return ColorChanger.ColorType.Sparkle;
                case "primary":
                default:
                    return ColorChanger.ColorType.Primary;
            }
        }

        private MaterialEx.SupportedUniform UniformName(string name)
        {
            switch (name)
            {
                case "color2":
                    return MaterialEx.SupportedUniform._Color2;
                case "emitcolor":
                    return MaterialEx.SupportedUniform._EmitColor;
                case "reflectcolor":
                    return MaterialEx.SupportedUniform._ReflectColor;
                case "speccolor":
                    return MaterialEx.SupportedUniform._SpecColor;
                case "color":
                default:
                    return MaterialEx.SupportedUniform._Color;
            }
        }

        private void SetCarVisuals(CarVisuals visuals, GameObject car)
        {
            if (visuals == null)
            {
                ErrorList.Add("Can't find the CarVisuals component on the base car");
                return;
            }

            SkinnedMeshRenderer skinned = car.GetComponentInChildren<SkinnedMeshRenderer>();
            MakeMeshSkinned(skinned);
            visuals.carBodyRenderer_ = skinned;

            List<JetFlame> boostJets = new List<JetFlame>();
            List<JetFlame> wingJets = new List<JetFlame>();
            List<JetFlame> rotationJets = new List<JetFlame>();

            PlaceJets(car, boostJets, wingJets, rotationJets);
            visuals.boostJetFlames_ = boostJets.ToArray();
            visuals.wingJetFlames_ = wingJets.ToArray();
            visuals.rotationJetFlames_ = rotationJets.ToArray();
            visuals.driverPosition_ = FindCarDriver(car.transform);

            PlaceCarWheelsVisuals(visuals, car);
        }

        private void MakeMeshSkinned(SkinnedMeshRenderer renderer)
        {
            Mesh mesh = renderer.sharedMesh;
            if (mesh == null)
            {
                ErrorList.Add("The mesh on " + renderer.gameObject.FullName() + " is null");
                return;
            }

            if (!mesh.isReadable)
            {
                ErrorList.Add("Can't read the car mesh " + mesh.name + " on " + renderer.gameObject.FullName() + "You must allow reading on it's unity inspector !");
                return;
            }

            if (mesh.vertices.Length == mesh.boneWeights.Length)
            {
                return;
            }

            BoneWeight[] bones = new BoneWeight[mesh.vertices.Length];
            for (int i = 0; i < bones.Length; i++)
            {
                bones[i].weight0 = 1;
            }

            mesh.boneWeights = bones;
            Transform t = renderer.transform;
            Matrix4x4[] bindPoses = new Matrix4x4[1] { t.worldToLocalMatrix * renderer.transform.localToWorldMatrix };
            mesh.bindposes = bindPoses;
            renderer.bones = new Transform[1] { t };
        }

        private void PlaceJets(GameObject obj, List<JetFlame> boostJets, List<JetFlame> wingJets, List<JetFlame> rotationJets)
        {
            int childNb = obj.transform.childCount;
            for (int i = 0; i < childNb; i++)
            {
                GameObject child = obj.GetChild(i).gameObject;
                string name = child.name.ToLower();
                if (_infos.boostJet != null && name.Contains("boostjet"))
                {
                    GameObject jet = Object.Instantiate(_infos.boostJet, child.transform);
                    jet.transform.localPosition = Vector3.zero;
                    jet.transform.localRotation = Quaternion.identity;
                    boostJets.Add(jet.GetComponentInChildren<JetFlame>());
                }
                else if (_infos.wingJet != null && name.Contains("wingjet"))
                {
                    GameObject jet = Object.Instantiate(_infos.wingJet, child.transform);
                    jet.transform.localPosition = Vector3.zero;
                    jet.transform.localRotation = Quaternion.identity;
                    wingJets.Add(jet.GetComponentInChildren<JetFlame>());
                    wingJets.Last().rotationAxis_ = JetDirection(child.transform);
                }
                else if (_infos.rotationJet != null && name.Contains("rotationjet"))
                {
                    GameObject jet = Object.Instantiate(_infos.rotationJet, child.transform);
                    jet.transform.localPosition = Vector3.zero;
                    jet.transform.localRotation = Quaternion.identity;
                    rotationJets.Add(jet.GetComponentInChildren<JetFlame>());
                    rotationJets.Last().rotationAxis_ = JetDirection(child.transform);
                }
                else if (_infos.wingTrail != null && name.Contains("wingtrail"))
                {
                    GameObject trail = Object.Instantiate(_infos.wingTrail, child.transform);
                    trail.transform.localPosition = Vector3.zero;
                    trail.transform.localRotation = Quaternion.identity;
                }
                else
                {
                    PlaceJets(child, boostJets, wingJets, rotationJets);
                }
            }
        }

        private Vector3 JetDirection(Transform t)
        {
            int nb = t.childCount;
            for (int i = 0; i < nb; i++)
            {
                string n = t.GetChild(i).gameObject.name.ToLower();
                if (!n.StartsWith("#"))
                {
                    continue;
                }

                n = n.Remove(0, 1);
                string[] s = n.Split(';');
                if (s.Length == 0)
                {
                    continue;
                }

                if (!s[0].Contains("dir"))
                {
                    continue;
                }

                if (s.Length < 2)
                {
                    continue;
                }

                switch (s[1])
                {
                    case "front":
                        return new Vector3(-1, 0, 0);
                    case "back":
                        return new Vector3(1, 0, 0);
                    case "left":
                        return new Vector3(0, 1, -1);
                    case "right":
                        return new Vector3(0, -1, 1);
                }

                if (s.Length != 4)
                {
                    continue;
                }

                if (!s[0].Contains("dir"))
                {
                    continue;
                }

                Vector3 v = Vector3.zero;
                float.TryParse(s[1], out v.x);
                float.TryParse(s[2], out v.y);
                float.TryParse(s[3], out v.z);
                return v;
            }

            return Vector3.zero;
        }

        private Transform FindCarDriver(Transform parent)
        {
            foreach (Transform t in parent.GetChildren())
            {
                if (t.gameObject.name.ToLower().Contains("driverposition"))
                {
                    return t;
                }

                return FindCarDriver(t);
            }

            return null;
        }

        private void PlaceCarWheelsVisuals(CarVisuals visual, GameObject car)
        {
            for (int i = 0; i < car.transform.childCount; i++)
            {
                GameObject c = car.transform.GetChild(i).gameObject;
                string name = c.name.ToLower();
                if (name.Contains("wheel"))
                {
                    CarWheelVisuals comp = c.AddComponent<CarWheelVisuals>();
                    foreach (MeshRenderer r in c.GetComponentsInChildren<MeshRenderer>())
                    {
                        if (r.gameObject.name.ToLower().Contains("tire"))
                        {
                            comp.tire_ = r;
                            break;
                        }
                    }

                    if (name.Contains("front"))
                    {
                        if (name.Contains("left"))
                        {
                            visual.wheelFL_ = comp;
                        }
                        else if (name.Contains("right"))
                        {
                            visual.wheelFR_ = comp;
                        }
                    }
                    else if (name.Contains("back"))
                    {
                        if (name.Contains("left"))
                        {
                            visual.wheelBL_ = comp;
                        }
                        else if (name.Contains("right"))
                        {
                            visual.wheelBR_ = comp;
                        }
                    }
                }
            }
        }

        private CarColors LoadDefaultColors(GameObject car)
        {
            for (int i = 0; i < car.transform.childCount; i++)
            {
                GameObject c = car.transform.GetChild(i).gameObject;
                string n = c.name.ToLower();
                if (n.Contains("defaultcolor"))
                {
                    for (int j = 0; j < c.transform.childCount; j++)
                    {
                        GameObject o = c.transform.GetChild(j).gameObject;
                        string name = o.name.ToLower();

                        if (!name.StartsWith("#"))
                        {
                            continue;
                        }

                        name = name.Remove(0, 1); //remove #

                        string[] s = name.Split(';');
                        if (s.Length != 2)
                        {
                            continue;
                        }

                        CarColors cc;
                        Color color = ColorEx.HexToColor(s[1]);
                        color.a = 1;

                        switch (s[0])
                        {
                            case "primary":
                                cc.primary_ = color;
                                break;
                            case "secondary":
                                cc.secondary_ = color;
                                break;
                            case "glow":
                                cc.glow_ = color;
                                break;
                            case "sparkle":
                                cc.sparkle_ = color;
                                break;
                        }
                    }
                }
            }

            return _infos.defaultColors;
        }

        private class CreateCarReturnInfos
        {
            public GameObject car;
            public CarColors colors;
        }

        private class MaterialPropertyExport
        {
            public enum PropertyType
            {
                Color,
                ColorArray,
                Float,
                FloatArray,
                Int,
                Matrix,
                MatrixArray,
                Texture,
                Vector,
                VectorArray
            }

            public int fromID = -1;

            public string fromName;
            public int toID = -1;
            public string toName;
            public PropertyType type;
        }
    }
}