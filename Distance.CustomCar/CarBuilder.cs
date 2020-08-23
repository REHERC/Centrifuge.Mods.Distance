using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Reactor.API;
using Reactor.API.Storage;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CustomCar
{
    public class CarBuilder
    {
        private CarInfos _infos;

        public void createCars(CarInfos infos)
        {
            _infos = infos;
            var cars = loadAssetsBundle();

            var carsInfos = new List<CreateCarReturnInfos>();

            foreach (var car in cars) carsInfos.Add(createCar(car));

            var profileManager = G.Sys.ProfileManager_;
            var oldCars = profileManager.carInfos_.ToArray();
            profileManager.carInfos_ = new CarInfo[oldCars.Length + cars.Count];

            var unlocked = (Dictionary<string, int>)profileManager.GetType()
                .GetField("unlockedCars_", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(profileManager);
            var knowCars = (Dictionary<string, int>)profileManager.GetType()
                .GetField("knownCars_", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(profileManager);

            for (var i = 0; i < profileManager.carInfos_.Length; i++)
            {
                if (i < oldCars.Length)
                {
                    profileManager.carInfos_[i] = oldCars[i];
                    continue;
                }

                var index = i - oldCars.Length;

                var car = new CarInfo();
                car.name_ = carsInfos[index].car.name;
                car.prefabs_ = new CarPrefabs();
                car.prefabs_.carPrefab_ = carsInfos[index].car;
                car.colors_ = carsInfos[index].colors;
                profileManager.carInfos_[i] = car;
                unlocked.Add(car.name_, i);
                knowCars.Add(car.name_, i);
            }

            var carColors = new CarColors[oldCars.Length + cars.Count];
            for (var i = 0; i < carColors.Length; i++)
                carColors[i] = G.Sys.ProfileManager_.carInfos_[i].colors_;

            for (var i = 0; i < profileManager.ProfileCount_; i++)
            {
                var p = profileManager.GetProfile(i);

                var oldColorList =
                    p.GetType().GetField("carColorsList_", BindingFlags.Instance | BindingFlags.NonPublic)
                        .GetValue(p) as CarColors[];
                for (var j = 0; j < oldColorList.Length && j < carColors.Length; j++)
                    carColors[j] = oldColorList[j];

                var field = p.GetType().GetField("carColorsList_", BindingFlags.Instance | BindingFlags.NonPublic);
                field.SetValue(p, carColors);
            }
        }

        private List<GameObject> loadAssetsBundle()
        {
            var dirStr = Path.Combine(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location),
                Defaults.PrivateAssetsDirectory);
            var files = Directory.GetFiles(dirStr);

            var cars = new List<GameObject>();

            foreach (var f in files)
            {
                var index = f.LastIndexOf('/');
                if (index < 0)
                    index = f.LastIndexOf('\\');
                var name = f.Substring(index + 1);
                var asset = new Assets(name);

                GameObject car = null;
                var bundle = (AssetBundle)asset.Bundle;
                foreach (var n in bundle.GetAllAssetNames())
                    if (car == null && n.EndsWith(".prefab"))
                    {
                        car = bundle.LoadAsset<GameObject>(n);
                        break;
                    }

                if (car == null)
                {
                    ErrorList.add("Can't find a prefab in the asset bundle " + f);
                }
                else
                {
                    car.name = name;
                    cars.Add(car);
                }
            }

            return cars;
        }

        private CreateCarReturnInfos createCar(GameObject car)
        {
            var infos = new CreateCarReturnInfos();

            var obj = Object.Instantiate(_infos.baseCar);
            obj.name = car.name;
            Object.DontDestroyOnLoad(obj);
            obj.SetActive(false);

            removeOldCar(obj);
            var newCar = addNewCarOnPrefab(obj, car);
            setCarDatas(obj, newCar);
            infos.car = obj;

            infos.colors = loadDefaultColors(newCar);

            return infos;
        }

        private void removeOldCar(GameObject obj)
        {
            var wheelsToRemove = new List<GameObject>();
            for (var i = 0; i < obj.transform.childCount; i++)
            {
                var c = obj.transform.GetChild(i).gameObject;
                if (c.name.ToLower().Contains("wheel"))
                    wheelsToRemove.Add(c);
            }

            if (wheelsToRemove.Count != 4)
                ErrorList.add("Found " + wheelsToRemove.Count + " wheels on base prefabs, expected 4");

            var refractor = obj.transform.Find("Refractor");
            if (refractor == null)
            {
                ErrorList.add("Can't find the Refractor object on the base car prefab");
                return;
            }

            Object.Destroy(refractor.gameObject);
            foreach (var w in wheelsToRemove)
                Object.Destroy(w);
        }

        private GameObject addNewCarOnPrefab(GameObject obj, GameObject car)
        {
            return Object.Instantiate(car, obj.transform);
        }

        private void setCarDatas(GameObject obj, GameObject car)
        {
            setColorChanger(obj.GetComponent<ColorChanger>(), car);
            setCarVisuals(obj.GetComponent<CarVisuals>(), car);
        }

        private void setColorChanger(ColorChanger colorChanger, GameObject car)
        {
            if (colorChanger == null)
            {
                ErrorList.add("Can't find the ColorChanger component on the base car");
                return;
            }

            colorChanger.rendererChangers_ = new ColorChanger.RendererChanger[0];
            foreach (var r in car.GetComponentsInChildren<Renderer>())
            {
                replaceMaterials(r);
                if (colorChanger != null)
                    addMaterialColorChanger(colorChanger, r.transform);
            }
        }

        private void replaceMaterials(Renderer r)
        {
            var matNames = new string[r.materials.Length];
            for (var i = 0; i < matNames.Length; i++)
                matNames[i] = "wheel";
            var matProperties = new List<MaterialPropertyExport>[r.materials.Length];
            for (var i = 0; i < matProperties.Length; i++)
                matProperties[i] = new List<MaterialPropertyExport>();

            fillMaterialInfos(r, matNames, matProperties);

            var materials = r.materials.ToArray();
            for (var i = 0; i < r.materials.Length; i++)
            {
                MaterialInfos matInfo = null;
                if (!_infos.materials.TryGetValue(matNames[i], out matInfo))
                {
                    ErrorList.add("Can't find the material " + matNames[i] + " on " + r.gameObject.FullName());
                    continue;
                }

                if (matInfo == null || matInfo.material == null)
                    continue;
                var mat = Object.Instantiate(matInfo.material);
                if (matInfo.diffuseIndex >= 0)
                    mat.SetTexture(matInfo.diffuseIndex, r.materials[i].GetTexture("_MainTex"));
                if (matInfo.normalIndex >= 0)
                    mat.SetTexture(matInfo.normalIndex, r.materials[i].GetTexture("_BumpMap"));
                if (matInfo.emitIndex >= 0)
                    mat.SetTexture(matInfo.emitIndex, r.materials[i].GetTexture("_EmissionMap"));

                foreach (var p in matProperties[i])
                    copyMaterialProperty(r.materials[i], mat, p);

                materials[i] = mat;
            }

            r.materials = materials;
        }

        private void fillMaterialInfos(Renderer r, string[] matNames, List<MaterialPropertyExport>[] matProperties)
        {
            var childCount = r.transform.childCount;

            for (var i = 0; i < childCount; i++)
            {
                var name = r.transform.GetChild(i).name.ToLower();
                if (!name.StartsWith("#"))
                    continue;
                name = name.Remove(0, 1);
                var s = name.Split(';');
                if (s.Length == 0)
                    continue;
                if (s[0].Contains("mat"))
                {
                    if (s.Length != 3)
                    {
                        ErrorList.add(s[0] + " property on " + r.gameObject.FullName() + " must have 2 arguments");
                        continue;
                    }

                    var index = 0;
                    if (!int.TryParse(s[1], out index))
                    {
                        ErrorList.add("First argument of " + s[0] + " on " + r.gameObject.FullName() +
                                      " property must be a number");
                        continue;
                    }

                    if (index < matNames.Length)
                        matNames[index] = s[2];
                }
                else if (s[0].Contains("export"))
                {
                    if (s.Length != 5)
                    {
                        ErrorList.add(s[0] + " property on " + r.gameObject.FullName() + " must have 4 arguments");
                        continue;
                    }

                    var index = 0;
                    if (!int.TryParse(s[1], out index))
                    {
                        ErrorList.add("First argument of " + s[0] + " on " + r.gameObject.FullName() +
                                      " property must be a number");
                        continue;
                    }

                    if (index >= matNames.Length)
                        continue;
                    var p = new MaterialPropertyExport();
                    var found = false;

                    foreach (MaterialPropertyExport.PropertyType pType in Enum.GetValues(
                        typeof(MaterialPropertyExport.PropertyType)))
                        if (s[2] == pType.ToString().ToLower())
                        {
                            found = true;
                            p.type = pType;
                            break;
                        }

                    if (!found)
                    {
                        ErrorList.add("The property " + s[2] + " on " + r.gameObject.FullName() + " is not valid");
                        continue;
                    }

                    if (!int.TryParse(s[3], out p.fromID))
                        p.fromName = s[3];
                    if (!int.TryParse(s[4], out p.toID))
                        p.toName = s[4];

                    matProperties[index].Add(p);
                }
            }
        }

        private void copyMaterialProperty(Material from, Material to, MaterialPropertyExport property)
        {
            var fromID = property.fromID;
            if (fromID == -1)
                fromID = Shader.PropertyToID(property.fromName);
            var toID = property.toID;
            if (toID == -1)
                toID = Shader.PropertyToID(property.toName);

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

        private void addMaterialColorChanger(ColorChanger colorChanger, Transform t)
        {
            var r = t.GetComponent<Renderer>();
            if (r == null)
                return;

            var uniformChangers = new List<ColorChanger.UniformChanger>();

            for (var i = 0; i < t.childCount; i++)
            {
                var o = t.GetChild(i).gameObject;
                var name = o.name.ToLower();
                if (!name.StartsWith("#"))
                    continue;

                name = name.Remove(0, 1); //remove #

                var s = name.Split(';');
                if (s.Length == 0)
                    continue;
                if (!s[0].Contains("color"))
                    continue;
                if (s.Length != 6)
                {
                    ErrorList.add(s[0] + " property on " + t.gameObject.FullName() + " must have 5 arguments");
                    continue;
                }

                var uniformChanger = new ColorChanger.UniformChanger();
                var materialIndex = 0;
                int.TryParse(s[1], out materialIndex);
                uniformChanger.materialIndex_ = materialIndex;
                uniformChanger.colorType_ = colorType(s[2]);
                uniformChanger.name_ = uniformName(s[3]);
                float multiplier = 0;
                float.TryParse(s[4], out multiplier);
                uniformChanger.mul_ = multiplier;
                uniformChanger.alpha_ = s[5].ToLower() == "true";

                uniformChangers.Add(uniformChanger);
            }

            if (uniformChangers.Count == 0)
                return;

            var renderChanger = new ColorChanger.RendererChanger();
            renderChanger.renderer_ = r;
            renderChanger.uniformChangers_ = uniformChangers.ToArray();

            var renders = colorChanger.rendererChangers_.ToList();
            renders.Add(renderChanger);
            colorChanger.rendererChangers_ = renders.ToArray();
        }

        private ColorChanger.ColorType colorType(string name)
        {
            name = name.ToLower();
            if (name == "primary")
                return ColorChanger.ColorType.Primary;
            if (name == "secondary")
                return ColorChanger.ColorType.Secondary;
            if (name == "glow")
                return ColorChanger.ColorType.Glow;
            if (name == "sparkle")
                return ColorChanger.ColorType.Sparkle;
            return ColorChanger.ColorType.Primary;
        }

        private MaterialEx.SupportedUniform uniformName(string name)
        {
            name = name.ToLower();
            if (name == "color")
                return MaterialEx.SupportedUniform._Color;
            if (name == "color2")
                return MaterialEx.SupportedUniform._Color2;
            if (name == "emitcolor")
                return MaterialEx.SupportedUniform._EmitColor;
            if (name == "reflectcolor")
                return MaterialEx.SupportedUniform._ReflectColor;
            if (name == "speccolor")
                return MaterialEx.SupportedUniform._SpecColor;
            return MaterialEx.SupportedUniform._Color;
        }

        private void setCarVisuals(CarVisuals visuals, GameObject car)
        {
            if (visuals == null)
            {
                ErrorList.add("Can't find the CarVisuals component on the base car");
                return;
            }

            var skinned = car.GetComponentInChildren<SkinnedMeshRenderer>();
            MakeMeshSkinned(skinned);
            visuals.carBodyRenderer_ = skinned;

            var boostJets = new List<JetFlame>();
            var wingJets = new List<JetFlame>();
            var rotationJets = new List<JetFlame>();

            PlaceJets(car, boostJets, wingJets, rotationJets);
            visuals.boostJetFlames_ = boostJets.ToArray();
            visuals.wingJetFlames_ = wingJets.ToArray();
            visuals.rotationJetFlames_ = rotationJets.ToArray();
            visuals.driverPosition_ = findCarDriver(car.transform);

            placeCarWheelsVisuals(visuals, car);
        }

        private void MakeMeshSkinned(SkinnedMeshRenderer renderer)
        {
            var mesh = renderer.sharedMesh;
            if (mesh == null)
            {
                ErrorList.add("The mesh on " + renderer.gameObject.FullName() + " is null");
                return;
            }

            if (!mesh.isReadable)
            {
                ErrorList.add("Can't read the car mesh " + mesh.name + " on " + renderer.gameObject.FullName() +
                              "You must allow reading on it's unity inspector !");
                return;
            }

            if (mesh.vertices.Length == mesh.boneWeights.Length)
                return;

            var bones = new BoneWeight[mesh.vertices.Length];
            for (var i = 0; i < bones.Length; i++)
                bones[i].weight0 = 1;
            mesh.boneWeights = bones;
            var t = renderer.transform;
            var bindPoses = new Matrix4x4[1] {t.worldToLocalMatrix * renderer.transform.localToWorldMatrix};
            mesh.bindposes = bindPoses;
            renderer.bones = new Transform[1] {t};
        }

        private void PlaceJets(GameObject obj, List<JetFlame> boostJets, List<JetFlame> wingJets,
            List<JetFlame> rotationJets)
        {
            var childNb = obj.transform.childCount;
            for (var i = 0; i < childNb; i++)
            {
                var child = obj.GetChild(i).gameObject;
                var name = child.name.ToLower();
                if (_infos.boostJet != null && name.Contains("boostjet"))
                {
                    var jet = Object.Instantiate(_infos.boostJet, child.transform);
                    jet.transform.localPosition = Vector3.zero;
                    jet.transform.localRotation = Quaternion.identity;
                    boostJets.Add(jet.GetComponentInChildren<JetFlame>());
                }
                else if (_infos.wingJet != null && name.Contains("wingjet"))
                {
                    var jet = Object.Instantiate(_infos.wingJet, child.transform);
                    jet.transform.localPosition = Vector3.zero;
                    jet.transform.localRotation = Quaternion.identity;
                    wingJets.Add(jet.GetComponentInChildren<JetFlame>());
                    wingJets.Last().rotationAxis_ = JetDirection(child.transform);
                }
                else if (_infos.rotationJet != null && name.Contains("rotationjet"))
                {
                    var jet = Object.Instantiate(_infos.rotationJet, child.transform);
                    jet.transform.localPosition = Vector3.zero;
                    jet.transform.localRotation = Quaternion.identity;
                    rotationJets.Add(jet.GetComponentInChildren<JetFlame>());
                    rotationJets.Last().rotationAxis_ = JetDirection(child.transform);
                }
                else if (_infos.wingTrail != null && name.Contains("wingtrail"))
                {
                    var trail = Object.Instantiate(_infos.wingTrail, child.transform);
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
            var nb = t.childCount;
            for (var i = 0; i < nb; i++)
            {
                var n = t.GetChild(i).gameObject.name.ToLower();
                if (!n.StartsWith("#"))
                    continue;
                n = n.Remove(0, 1);
                var s = n.Split(';');
                if (s.Length == 0)
                    continue;
                if (!s[0].Contains("dir"))
                    continue;
                if (s.Length < 2)
                    continue;
                if (s[1] == "front")
                    return new Vector3(-1, 0, 0);
                if (s[1] == "back")
                    return new Vector3(1, 0, 0);
                if (s[1] == "left")
                    return new Vector3(0, 1, -1);
                if (s[1] == "right")
                    return new Vector3(0, -1, 1);
                if (s.Length != 4)
                    continue;
                if (!s[0].Contains("dir"))
                    continue;
                var v = Vector3.zero;
                float.TryParse(s[1], out v.x);
                float.TryParse(s[2], out v.y);
                float.TryParse(s[3], out v.z);
                return v;
            }

            return Vector3.zero;
        }

        private Transform findCarDriver(Transform parent)
        {
            for (var i = 0; i < parent.childCount; i++)
            {
                var t = parent.GetChild(i);
                if (t.gameObject.name.ToLower().Contains("driverposition"))
                    return t;
                return findCarDriver(t);
            }

            return null;
        }

        private void placeCarWheelsVisuals(CarVisuals visual, GameObject car)
        {
            for (var i = 0; i < car.transform.childCount; i++)
            {
                var c = car.transform.GetChild(i).gameObject;
                var name = c.name.ToLower();
                if (name.Contains("wheel"))
                {
                    var comp = c.AddComponent<CarWheelVisuals>();
                    foreach (var r in c.GetComponentsInChildren<MeshRenderer>())
                        if (r.gameObject.name.ToLower().Contains("tire"))
                        {
                            comp.tire_ = r;
                            break;
                        }

                    if (name.Contains("front"))
                    {
                        if (name.Contains("left"))
                            visual.wheelFL_ = comp;
                        else if (name.Contains("right"))
                            visual.wheelFR_ = comp;
                    }
                    else if (name.Contains("back"))
                    {
                        if (name.Contains("left"))
                            visual.wheelBL_ = comp;
                        else if (name.Contains("right"))
                            visual.wheelBR_ = comp;
                    }
                }
            }
        }

        private CarColors loadDefaultColors(GameObject car)
        {
            for (var i = 0; i < car.transform.childCount; i++)
            {
                var c = car.transform.GetChild(i).gameObject;
                var n = c.name.ToLower();
                if (n.Contains("defaultcolor"))
                    for (var j = 0; j < c.transform.childCount; j++)
                    {
                        var o = c.transform.GetChild(j).gameObject;
                        var name = o.name.ToLower();

                        if (!name.StartsWith("#"))
                            continue;

                        name = name.Remove(0, 1); //remove #

                        var s = name.Split(';');
                        if (s.Length != 2)
                            continue;

                        CarColors cc;
                        var color = ColorEx.HexToColor(s[1]);
                        color.a = 1;
                        if (s[0] == "primary")
                            cc.primary_ = color;
                        else if (s[0] == "secondary")
                            cc.secondary_ = color;
                        else if (s[0] == "glow")
                            cc.glow_ = color;
                        else if (s[0] == "sparkle")
                            cc.sparkle_ = color;
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