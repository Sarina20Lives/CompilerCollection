using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace CompilerCollection.CompilerCollection.Interprete
{
    class CollectionNonSQL
    {
        private const string STORAGE = "C:\\FilesCompilerCollection\\JsonFiles";


        public static void create(string name, string[] columns)
        {
            JObject collection =
                new JObject(
                    new JProperty("coleccion", name),
                    new JProperty("nocolumnas", columns.Length),
                    new JProperty("columnas", new JArray(columns)),
                    new JProperty("almacen", new JArray())
                );
            string file = getPathToCollection(name);
            if(File.Exists(file))
            {
                //Error, ya existe una colección con ese nombre
                return;
            }
            File.WriteAllText(file, collection.ToString());
        }


        public static string select(string name, string[] columns)
        {
            string result = "";
            JObject collection = getCollection(name);
            if (collection == null) return result;
            foreach (var column in columns)
            {
                result += "| " + String.Format("{0, -20}",column);
            }
            result += "|\n";
            JArray storageData = (JArray)collection["almacen"];
            foreach (JObject data in storageData)
            {
                foreach (var column in columns)
                {
                    result += "| " + String.Format("{0, -20}",data[column]);
                }
                result += "|\n";
            }
            return result;
        }


        public static void insert(string name, string[] columns, string[] values)
        {
            if (columns.Length != values.Length)
            {
                //Error, la cantidad de valores y columnas no coincide
                return;
            }
            JObject collection = getCollection(name);
            if (collection == null) return;
            JObject data = new JObject();
            for (int i = 0; i < values.Length; i++)
            {
                data.Add(columns[i], values[i]);
            }
            JArray storageData = (JArray)collection["almacen"];
            storageData.Add(data);
            collection["almacen"] = storageData;
            File.WriteAllText(getPathToCollection(name), collection.ToString());
        }


        public static void update(string name, string column, string currentValue, string newValue)
        {
            JObject collection = getCollection(name);
            if (collection == null) return;
            JArray storageData = (JArray)collection["almacen"];
            JArray newData = new JArray();
            foreach (JObject data in storageData)
            {
                if ((string)data[column] == currentValue)
                {
                    data[column] = newValue;
                }
                newData.Add(data);
            }
            collection["almacen"] = newData;
            File.WriteAllText(getPathToCollection(name), collection.ToString());
        }


        public static void delete(string name, string column, string currentValue)
        {
            JObject collection = getCollection(name);
            if (collection == null) return;
            JArray storageData = (JArray)collection["almacen"];
            JArray newData = new JArray();
            foreach (JObject data in storageData)
            {
                if ((string)data[column] != currentValue)
                {
                    newData.Add(data);
                }
            }
            collection["almacen"] = newData;
            File.WriteAllText(getPathToCollection(name), collection.ToString());
        }


        public static void drop(string name)
        {
            string file = getPathToCollection(name);
            if (!File.Exists(file))
            {
                //Error, no existe una colección con ese nombre
                return;
            }
            File.Delete(file);
        }


        private static string getPathToCollection(string collection)
        {
            return STORAGE + "\\" + collection + ".db";
        }


        private static JObject getCollection(string name)
        {
            string file = getPathToCollection(name);
            if (!File.Exists(file))
            {
                //Error, no existe una colección con ese nombre
                return null;
            }
            return JObject.Parse(File.ReadAllText(file));
        }


    }
}
