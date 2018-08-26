using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Comment m'utilisé : 
 * 		Dans l'inspecteur, remplir poolArray avec les informations pour chaque Object a pooler
 * 		
 * 		PoolManager.instance.GetObject("nom de l'objet");
 * 		Une faute d'orthographe va résulté a un objet null + une erreur
 * 		ENJOY LE SPAM
 * 
 * 		bisoux, 
 * 		dominqu
 */


public class PoolManager : MonoBehaviour {
	#region singleton
	public static PoolManager instance;
	void Awake()
	{
		instance = this;
	}
	#endregion

	[SerializeField]PoolObject[] poolArray;

	private Dictionary<string, PoolObject> pool = new Dictionary<string, PoolObject>();

	// Use this for initialization
	void Start () 
	{
		for (int i = 0; i < poolArray.Length; i++)
		{
			PoolObject obj = poolArray[i];
			obj.Initialise();
			pool.Add(obj.name, obj);
		}
		poolArray = new PoolObject[0];
	}

	public GameObject GetObject(string name)
	{
		PoolObject obj;
		//Place requested Object in obj, if its null, pop error
		if(!pool.TryGetValue(name, out obj))
		{	
			Debug.LogError("<color=red>WRONG KEY : " + name + "</color>");
			return null;
		}
		return obj.GetNext();
	}

    public void ReturnObject(string name, GameObject gameObject)
    {
        if(pool.ContainsKey(name))
        {
            pool[name].ReturnObject(gameObject);
        }
        else
        {
            Debug.LogError("<color=red>WRONG KEY : " + name + "</color>");
        }
    }

}

[System.Serializable]
public class PoolObject
{
	public string name;
	public GameObject prefab;
	public int quantity;

	private Queue<GameObject> queue;
    GameObject objParent;

    public void Initialise()
	{
		objParent = new GameObject(name);
        queue = new Queue<GameObject>();
		
		for (int i = 0; i < quantity; i++) 
		{
            AddNewObject();
        }
	}

    void AddNewObject()
    {
        GameObject go = MonoBehaviour.Instantiate(prefab, Vector3.zero, Quaternion.identity);

        go.transform.SetParent(objParent.transform);
        go.SetActive(false);
        queue.Enqueue(go);
    }

    public void ReturnObject(GameObject gameObject)
    {
        queue.Enqueue(gameObject);
    }

	public GameObject GetNext()
	{
        if(queue.Count == 0)
            AddNewObject();

        GameObject currentObj = queue.Dequeue();
		currentObj.SetActive(true);
		return currentObj;
	}
}
