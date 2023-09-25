using System.IO;
using EngineLibrary.Extensions;
using System.Threading.Tasks;
using EngineLibrary.ECS;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using System.Linq;
using EngineLibrary.ECS.Components;
using EngineLibrary.ECS.Components.Colliders;
using EngineLibrary.Math;

namespace EngineLibrary.Memory
{
    public static class Serializer
    {
        private static Dictionary<string, object> ConvertDynamicToDictionary(IDictionary<string, object> value) 
        {
            return value.ToDictionary
                (
                p => p.Key,
                p => 
                    {
                        if (p.Value is IDictionary<string, object> dict)
                            return ConvertDynamicToDictionary(dict);

                        if (p.Value is IEnumerable<object> list)
                        {
                            if (list.Any(obj => obj is System.Dynamic.ExpandoObject))
                            {
                                return list.Where(obj => obj is System.Dynamic.ExpandoObject).Select(obj => ConvertDynamicToDictionary((System.Dynamic.ExpandoObject)obj));
                            }
                        }

                        return p.Value;
                    }
                );
        }

        public static async Task<bool> SerializeSceneAsync(string name = "DefaultScene") 
        {
            using (FileStream fs = new FileStream(Project.Path + "\\Assets\\Scenes\\" + name + ".escene", FileMode.OpenOrCreate))
            {
                await System.Text.Json.JsonSerializer.SerializeAsync(fs, Game.Entities).OnFailure((str) => Debug.LogError("Resources> Exception: " + str.Message));
            }

            return true;
        }

        public static async Task<Dictionary<uint, Entity>> DeserializeSceneAsync(string name = "DefaultScene")
        {
            string input = "";
            using (StreamReader reader = new StreamReader(Project.Path + "\\Assets\\Scenes\\" + name + ".escene"))
            {
                input = await reader.ReadToEndAsync();
            }

            dynamic data = JsonConvert.DeserializeObject<dynamic>(input);

            int count = (int)data.Total;
            if (count <= 0)
            {
                Debug.LogError("Resources> There are 0 entities on the scene!");
                return new Dictionary<uint, Entity>();
            }

            int size = (int)data.Size;

            Dictionary<uint, Entity> entities = new Dictionary<uint, Entity>(size + 1);

            Dictionary<uint, List<uint>> childParents = new Dictionary<uint, List<uint>>();

            int i = 0;
            for (; i < count; i++)
            {
                var id = (uint)data["Id" + i.ToString()];

                Entity entity = new Entity(id)
                {
                    Name = (string)data["Name" + i.ToString()],
                };

                int parentId = (int)data["ParentId" + i.ToString()];

                if (parentId != -1)
                {
                    if (childParents.TryGetValue((uint)parentId, out List<uint> list))
                        list.Add(id);
                    else
                        childParents.Add((uint)parentId, new List<uint>() { id });
                }

                try
                {   if(data["Transform" + i.ToString()] != null)
                    {
                        var transform = entity.CreateComponentInternal<Transform>();
                        transform.SetPosition(new Math.Vector3D((float)data["Transform" + i.ToString()].PositionX, (float)data["Transform" + i.ToString()].PositionY, (float)data["Transform" + i.ToString()].PositionZ), false);
                        transform.SetRotation(new Math.Vector3D((float)data["Transform" + i.ToString()].RotationX, (float)data["Transform" + i.ToString()].RotationY, (float)data["Transform" + i.ToString()].RotationZ), false);
                        transform.SetScale(new Math.Vector3D((float)data["Transform" + i.ToString()].ScaleX, (float)data["Transform" + i.ToString()].ScaleY, (float)data["Transform" + i.ToString()].ScaleZ), false);
                    }
                }
                catch (Exception)
                {

                }

                try
                {
                    if (data["Rigidbody" + i.ToString()] != null)
                    {
                        var rb = entity.CreateComponentInternal<Rigidbody>();
                        rb.RigidbodyType = (RigidbodyType)((int)data["Rigidbody" + i.ToString()].Type);
                        rb.Mass = (uint)data["Rigidbody" + i.ToString()].Mass;
                    }
                }
                catch (Exception)
                {

                }

                try
                {
                    if (data["AudioSource" + i.ToString()] != null)
                    {
                        var asource = entity.CreateComponentInternal<AudioSource>();
                    }
                }
                catch (Exception)
                {

                }

                try
                {
                    if (data["AudioListener" + i.ToString()] != null)
                    {
                        var alistener = entity.CreateComponentInternal<AudioListener>();
                    }
                }
                catch (Exception)
                {

                }

                try
                {
                    if (((string)data["Path" + i.ToString()]).Length > 1)
                    {
                        var mesh = entity.CreateComponentInternal<MeshRenderer>();
                    }
                }
                catch (Exception)
                {

                }

                try
                {
                    if (data["DirLight" + i.ToString()] != null)
                    {
                        var json = data["DirLight" + i.ToString()];
                        var coljson = json["Color"];
                        var dirjson = json["Direction"];
                        var ambjson = json["Ambient"];
                        Color color = new Color((float)coljson["ColorR"], (float)coljson["ColorG"], (float)coljson["ColorB"], 1.0f);

                        Vector3D ambient = new Vector3D((float)ambjson["AmbientR"], (float)ambjson["AmbientG"], (float)ambjson["AmbientB"]);
                        Vector3D direction = new Vector3D((float)dirjson["DirectionX"], (float)dirjson["DirectionY"], (float)dirjson["DirectionZ"]);

                        DirLightData lightdata = new DirLightData(color, direction, ambient);

                        var light = entity.CreateComponentInternal<DirLight>();
                        light.LightData = lightdata;
                    }
                }
                catch (Exception)
                {

                }

                try
                {
                    if (data["SpotLight" + i.ToString()] != null)
                    {
                        var json = data["DirLight" + i.ToString()];
                        var coljson = json["Color"];
                        var posjson = json["Position"];
                        var attjson = json["Attenuation"];
                        var dirjson = json["Direction"];
                        Color color = new Color((float)coljson["ColorR"], (float)coljson["ColorG"], (float)coljson["ColorB"], 1.0f);

                        Vector3D att = new Vector3D((float)attjson["AttenuationR"], (float)attjson["AttenuationG"], (float)attjson["AttenuationB"]);
                        Vector3D pos = new Vector3D((float)posjson["PositionX"], (float)posjson["PositionY"], (float)posjson["PositionZ"]);
                        Vector3D dir = new Vector3D((float)dirjson["DirectionX"], (float)dirjson["DirectionY"], (float)dirjson["DirectionZ"]);
                        float range = (float)json["Range"];
                        float inner = (float)json["Inner"];
                        float outter = (float)json["Outer"];

                        SpotLightData lightdata = new SpotLightData(color, dir, pos, att, range, inner, outter);

                        var light = entity.CreateComponentInternal<SpotLight>();
                        light.LightData = lightdata;
                    }
                }
                catch (Exception)
                {

                }

                try
                {
                    if (data["SpotLight" + i.ToString()] != null)
                    {
                        var json = data["DirLight" + i.ToString()];
                        var coljson = json["Color"];
                        var posjson = json["Position"];
                        var attjson = json["Attenuation"];
                        Color color = new Color((float)coljson["ColorR"], (float)coljson["ColorG"], (float)coljson["ColorB"], 1.0f);

                        Vector3D att = new Vector3D((float)attjson["AttenuationR"], (float)attjson["AttenuationG"], (float)attjson["AttenuationB"]);
                        Vector3D pos = new Vector3D((float)posjson["PositionX"], (float)posjson["PositionY"], (float)posjson["PositionZ"]);
                        
                        float range = (float)json["Range"];

                        PointLightData lightdata = new PointLightData(color, pos, att, range);

                        var light = entity.CreateComponentInternal<PointLight>();
                        light.LightData = lightdata;
                    }
                }
                catch (Exception)
                {

                }

                try
                {
                    if (data["Collider" + i.ToString()] != null)
                    {
                        var coll = data["Collider" + i.ToString()];
                        var type = (ColliderType)coll["Type"];
                        Collider collider = null;
                        if (type == ColliderType.Box)
                        {
                            collider = entity.CreateComponentInternal<BoxCollider>();
                        }
                        else if (type == ColliderType.Capsule)
                        {
                            collider = entity.CreateComponentInternal<CapsuleCollider>();
                        }
                        else if (type == ColliderType.Sphere)
                        {
                            collider = entity.CreateComponentInternal<SphereCollider>();
                        }
                        else if (type == ColliderType.ConvexMesh || type == ColliderType.TriangleMesh)
                        {
                            collider = entity.CreateComponentInternal<MeshCollider>();
                        }
                        else
                            throw new Exception("Wrong Collider Type");

                        collider.V1 = (float)coll["V1"];
                        collider.V2 = (float)coll["V2"];
                        collider.V3 = (float)coll["V3"];
                    }
                }
                catch (Exception)
                {

                }

                entities.Add(id, entity);
            }

            foreach (var pair in childParents)
            {
                int ind = 0, childCount = pair.Value.Count;
                for(; ind < childCount; ind++)
                    entities[pair.Key].AddChild(entities[pair.Value[ind]]);
            }
            
            return entities;
        }

        public static async Task<bool> SerializePrefabAsync(Entity entity)
        {
            using (FileStream fs = new FileStream(Project.Path + "\\Assets\\Prefabs\\" + entity.Name + ".eprefab", FileMode.OpenOrCreate))
            {
                await System.Text.Json.JsonSerializer.SerializeAsync(fs, entity).OnFailure((str) => Debug.LogError("Resources> Exception: " + str.Message));
            }

            return true;
        }

        public static async Task<Entity> DeserializePrefabAsync(string name = "DefaultPrefab")
        {
            Entity entity = null;
            using (FileStream fs = new FileStream(Project.Path + "\\Assets\\Prefabs\\" + name + ".eprefab", FileMode.Open))
            {
                entity = (Entity)await System.Text.Json.JsonSerializer.DeserializeAsync(fs, typeof(Entity)).ConfigureAwait(false);
                return entity;
            }
        }

        public static bool HasKey(string key) 
        {
            using (FileStream fs = new FileStream(Project.Path + "\\Data\\Preferences\\" + key + ".edata", FileMode.Open))
            {
                if(fs.CanRead)
                    return true;
            }

            return false;
        }

        public static async Task<bool> SetIntAsync(string key, int value) 
        {
            using (FileStream fs = new FileStream(Project.Path + "\\Data\\Preferences\\" + key + ".edata", FileMode.OpenOrCreate))
            {
                await System.Text.Json.JsonSerializer.SerializeAsync(fs, value).ConfigureAwait(false);
            }

            return true;
        }

        public static async Task<int> GetIntAsync(string key)
        {
            using (FileStream fs = new FileStream(Project.Path + "\\Data\\Preferences\\" + key + ".edata", FileMode.Open))
            {
                int value = (int)await System.Text.Json.JsonSerializer.DeserializeAsync(fs, typeof(int)).ConfigureAwait(false);
                return value;
            }
        }

        public static void DeleteKey(string key)
        {
            File.Delete(Project.Path + "\\Data\\Preferences\\" + key + ".edata");
        }
    }
}
