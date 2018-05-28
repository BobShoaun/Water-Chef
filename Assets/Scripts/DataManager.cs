// ***Copyright © 2017 Doxel aka Ng Bob Shoaun. All Rights Reserved.***

using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;
using Doxel.Utility;

public class DataManager : Singleton<DataManager> {
	
	private readonly string dataPath;
	private readonly string jsonDataPath;
	private readonly string xmlDataPath;
	private Dictionary<string, object> dataDictionary;
	private readonly BinaryFormatter binaryFormatter;
	private bool clear;

	public DataManager () {
		dataPath = Application.persistentDataPath + "/GameSave.bob";
		jsonDataPath = Application.streamingAssetsPath + "/{0}.json";
		xmlDataPath = Application.streamingAssetsPath + "/{0}.xml";
		dataDictionary = new Dictionary<string, object> ();
		var surrogateSelector = new SurrogateSelector ();
		surrogateSelector.AddSurrogate (typeof (Vector2), new StreamingContext (), new Vector2Surrogate ());
		surrogateSelector.AddSurrogate (typeof (Vector3), new StreamingContext (), new Vector3Surrogate ());
		binaryFormatter = new BinaryFormatter { SurrogateSelector = surrogateSelector };
	}

	public void Save<T> (string dataKey, T dataToSave) {		
		if (dataDictionary.ContainsKey (dataKey))
			dataDictionary [dataKey] = dataToSave;
		else
			dataDictionary.Add (dataKey, dataToSave);
	}

	public bool Load<T> (string dataKey, ref T dataToLoad) {
		object data;
		if (dataDictionary.TryGetValue (dataKey, out data)) {
			dataToLoad = (T) data;
			return true;
		}
		dataDictionary.Add (dataKey, dataToLoad);
		return false;
	}

	public T Load<T> (string dataKey) {
		object data;
		if (dataDictionary.TryGetValue (dataKey, out data))
			return (T) data;
		dataDictionary.Add (dataKey, default (T));
		return default (T);
	}

	public void SaveAll () {
		if (clear)
			clear = true;
		else {
			foreach (var saveLoad in Utility.FindObjectsOfNonUnityType<ISaveLoad> ())
				saveLoad.SaveData (this);
		}
	}

	public void LoadAll () {
		foreach (var saveLoad in Utility.FindObjectsOfNonUnityType<ISaveLoad> ())
			saveLoad.LoadData (this);
	}

	public void ClearAll () {
		dataDictionary.Clear ();
		clear = true;
	}

	public void SaveToFile () {
		using (FileStream fileStream = File.Create (dataPath))
			binaryFormatter.Serialize (fileStream, dataDictionary);
	}

	public bool LoadFromFile () {
		if (!File.Exists (dataPath))
			return false;
		using (FileStream fileStream = File.Open (dataPath, FileMode.Open))
			dataDictionary = binaryFormatter.Deserialize (fileStream) as Dictionary<string, object>;
		return true;
	}

	public void DeleteFile () {
		File.Delete (dataPath);
	}

	public void SaveToJson<T> (string jsonFileName, T dataToSave, bool format = false) {
		File.WriteAllText (string.Format (jsonDataPath, jsonFileName), JsonUtility.ToJson (dataToSave, format));
	}

	public bool LoadFromJson<T> (string jsonFileName, ref T dataToLoad) {
		var jsonFilePath = string.Format (jsonDataPath, jsonFileName);
		if (!File.Exists (jsonFilePath))
			return false;
		JsonUtility.FromJsonOverwrite (File.ReadAllText (jsonFilePath), dataToLoad);
		return true;
	}

	public T LoadFromJson<T> (string jsonFileName) {
		var jsonFilePath = string.Format (jsonDataPath, jsonFileName);
		if (File.Exists (jsonFilePath))
			return JsonUtility.FromJson<T> (File.ReadAllText (jsonFilePath));
		return default (T);
	}

	public void DeleteJson (string jsonFileName) {
		File.Delete (string.Format (jsonDataPath, jsonFileName));
	}

	public void SaveToXml<T> (string xmlFileName, T dataToSave) {
		var xmlSerializer = new XmlSerializer (typeof (T));
		using (FileStream fileStream = File.Create (string.Format (xmlDataPath, xmlFileName)))
			xmlSerializer.Serialize (fileStream, dataToSave);
	}

	public bool LoadFromXml<T> (string xmlFileName, ref T dataToLoad) {
		var xmlFilePath = string.Format (xmlDataPath, xmlFileName);
		if (!File.Exists (xmlFilePath))
			return false;
		var xmlSerializer = new XmlSerializer (typeof (T));
		using (FileStream fileStream = File.Open (xmlFilePath, FileMode.Open))
			dataToLoad = (T) xmlSerializer.Deserialize (fileStream);
		return true;
	}

	public T LoadFromXml<T> (string xmlFileName) {
		var xmlFilePath = string.Format (xmlDataPath, xmlFileName);
		if (!File.Exists (xmlFilePath))
			return default (T);
		var xmlSerializer = new XmlSerializer (typeof (T));
		using (FileStream fileStream = File.Open (xmlFilePath, FileMode.Open))
			return (T) xmlSerializer.Deserialize (fileStream);
	}

	public void DeleteXml (string xmlFileName) {
		File.Delete (string.Format (xmlDataPath, xmlFileName));
	}
		
}

[Serializable]
public class DataTable<TKey,TValue> {

	private List<TKey> keys;
	private List<TValue> values;

	public DataTable () {
		keys = new List<TKey> ();
		values = new List<TValue> ();
	}

	public TValue this [TKey key] {
		get {
			return GetValue (key);
		}
		set {
			SetValue (key, value);
		}
	}

	public void Add (TKey key, TValue value) {
		keys.Add (key);
		values.Add (value);
	}

	public void Clear () {
		keys.Clear ();
		values.Clear ();
	}

	public bool ContainsKey (TKey key) {
		return GetIndex (key) > -1;
	}

	private TValue GetValue (TKey key) {
		int index = GetIndex (key);
		if (index > -1)
			return values [index];
		throw new KeyNotFoundException (key.ToString ());
	}

	private void SetValue (TKey key, TValue valueToSet) {
		int index = GetIndex (key);
		if (index > -1)
			values [index] = valueToSet;
		else
			throw new KeyNotFoundException (key.ToString ());
	}

	private int GetIndex (TKey key) {
		return keys.FindIndex (i => EqualityComparer<TKey>.Default.Equals (i, key));
	}

}

[Serializable]
public struct Vector2Surrogate : ISerializationSurrogate {

	public void GetObjectData (object obj, SerializationInfo info, StreamingContext context) {
		var vector2 = (Vector2) obj;
		info.AddValue ("X", vector2.x);
		info.AddValue ("Y", vector2.y);
	}

	public object SetObjectData (object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector) {
		return new Vector2 (info.GetSingle ("X"), info.GetSingle ("Y"));
	}

}

[Serializable]
public struct Vector3Surrogate : ISerializationSurrogate {

	public void GetObjectData (object obj, SerializationInfo info, StreamingContext context) {
		var vector3 = (Vector3) obj;
		info.AddValue ("X", vector3.x);
		info.AddValue ("Y", vector3.y);
		info.AddValue ("Z", vector3.z);
	}

	public object SetObjectData (object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector) {
		return new Vector3 (info.GetSingle ("X"), info.GetSingle ("Y"), info.GetSingle ("Z"));
	}
	
}