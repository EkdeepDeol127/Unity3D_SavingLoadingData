using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class XmlManagerScript : MonoBehaviour
{
    public ObjectEntry tempHold;
    public ObjectList ListOfObj;
    public GameObject[] allObjects;
    public GameObject PrefabObject;
    private string PathString;

    public void SaveScene()
    {
        FindGameObjects();
        XmlSerializer serializer = new XmlSerializer(typeof(ObjectList));
        FileStream _stream = new FileStream(Application.dataPath + "/SavedFiles/Objects_Data.xml", FileMode.Create);//overrides current file
        serializer.Serialize(_stream, ListOfObj);
        _stream.Close();
        AssetDatabase.Refresh();//to refresh assets to see newly added stuff right away.
        Debug.Log("Saved Scene");
    }

    public void LoadScene()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(ObjectList));
        if (File.Exists(Application.dataPath + "/SavedFiles/Objects_Data.xml"))
        {
            PathString = EditorUtility.OpenFilePanel("Location Of XML File", Application.dataPath + "/SavedFiles", "xml");//opens file browser and returns a xml type file, the user opened/clicked
            FileStream _stream = new FileStream(PathString, FileMode.Open);
            ListOfObj = serializer.Deserialize(_stream) as ObjectList;
            _stream.Close();
            UpdateData();
            Debug.Log("Loaded Scene");
        }
        else
        {
            Debug.Log("No Saved Scenes...");
        }
    }

    void FindGameObjects()
    {
        ListOfObj.ObjList.Clear();
        allObjects = FindObjectsOfType<GameObject>();//finds all gameobjects in scene and saves them in list
        for (int i = 0; i < allObjects.Length; i++)
        {
            if(allObjects[i].tag == "Saveable")//added in so show that data is being loaded
            {
                tempHold = new ObjectEntry();
                tempHold.ObjectName = allObjects[i].name;
                tempHold.ObjectPosition = allObjects[i].transform.position;
                tempHold.ObjectRotation = allObjects[i].transform.rotation;
                tempHold.ObjectScale = allObjects[i].transform.localScale;
                //AssetDatabase.CreateAsset(allObjects[i], "Assets/Prefabs/");//make it a prefab
                tempHold.ObjectPrefabPath = AssetDatabase.GetAssetPath(allObjects[i].GetInstanceID());//not finding paths?
                Debug.Log(tempHold.ObjectPrefabPath);
                ListOfObj.ObjList.Add(tempHold);
            }
        }
    }

    void UpdateData()//for loading
    {
        GameObject[] allObjects;
        allObjects = FindObjectsOfType<GameObject>();//finds all gameobjects in scene and saves them in list
        for (int i = 0; i < allObjects.Length; i++)
        {
            if (allObjects[i].tag == "Saveable")//removes all previous gameobjects, to make way for new ones
            {
                Destroy(allObjects[i]);
                Debug.Log("Removed");
            }
        }//above temp

        foreach (ObjectEntry data in ListOfObj.ObjList)
        {
            CreateActor(data.ObjectPrefabPath, data.ObjectPosition, data.ObjectRotation, data.ObjectScale, data.ObjectName);
        }
    }

    void CreateActor(string path, Vector3 Pos, Quaternion Rot, Vector3 Sca, string name)//maybe have a switch for different objects?
    {
        GameObject temp = Instantiate(PrefabObject, Pos, Rot);
        temp.transform.localScale = Sca;
        temp.name = name;
        Debug.Log("Created");
        //define as needed
    }
}

[System.Serializable]
public class ObjectEntry
{
    public string ObjectName;
    public Vector3 ObjectPosition;
    public Quaternion ObjectRotation;
    public Vector3 ObjectScale;
    public string ObjectPrefabPath;
}

[System.Serializable]
public class ObjectList
{
    public List<ObjectEntry> ObjList = new List<ObjectEntry>();
}
