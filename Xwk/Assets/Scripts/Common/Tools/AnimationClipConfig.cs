using Assets.Scripts.C_Framework;
using UnityEngine;  
using System.Collections;  
using System.Collections.Generic; 
using System.IO;

public static class AnimationClipConfig  
{  
	public static List<ModelInfo> modelList = new List<ModelInfo>();  

	//private static FileSystemWatcher watcher;
	private static System.DateTime lastWriteTime;
	public static void Init()  
	{
		string fileName = Application.dataPath + "/Resources/animation.json";
		FileInfo fileInfo = new FileInfo(fileName);
		if (lastWriteTime.CompareTo(fileInfo.LastWriteTime) != 0)
		{
            if (!LoadConfigFile())
            {
                Debug.LogError("解析/Resources/animation.json文本出错");
            }
			lastWriteTime = fileInfo.LastWriteTime;
		}
	}  
	
	#region ClipList  
	public class ClipInfo  
	{  
		public string name;  
		public int firstFrame;  
		public int lastFrame;  
		public bool isloop;  
		
		public ClipInfo(string _n,int _f,int _l,bool _i) {  
			name = _n;  
			firstFrame = _f;  
			lastFrame = _l;  
			isloop = _i;  
		}  
	}  
	
	public class ModelInfo
	{  
		public string name;  
		public ClipInfo[] clipList;  
	}  
	#endregion 

	// 从Json 文件中读取模型的动画配置信息 // 
	static private bool LoadConfigFile()
	{
		bool success = false;
		TextAsset ta = Resources.Load<TextAsset>("animation");
		JsonData data = JsonMapper.ToObject(ta.text);
		JsonData items = data["Animation"];

		for (int n = 0; n < items.Count; ++n)
		{
			JsonData d = items[n];
			ModelInfo modelInfo = new ModelInfo();
			modelInfo.name = d["modelName"].ToString();

			JsonData clips = d["clipInfo"];
			modelInfo.clipList = new ClipInfo[clips.Count];
			for (int i = 0; i < clips.Count; ++i)
			{
				JsonData clip = clips[i];
				modelInfo.clipList[i] = new ClipInfo(clip["name"].ToString(), (int)clip["firstFrame"], (int)clip["lastFrame"], (bool)clip["loop"]);
			}

			modelList.Add(modelInfo);
		}

		success = true;
		return success;
	}
} 