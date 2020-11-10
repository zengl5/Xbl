using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class ChangeShader :  EditorWindow {
	
  static TextAsset textAsset;
  static  string [] lineArray;

  
   [MenuItem ("Custom/Changer Shader")]	
    static void AddWindow ()
	{       
		textAsset = AssetDatabase.LoadAssetAtPath("Assets/Resources/shader.txt", typeof(TextAsset)) as TextAsset;

        lineArray = textAsset.text.Split(new char[]{'\n'}, System.StringSplitOptions.RemoveEmptyEntries);
		
		Rect  wr = new Rect (0,0,500,300);
        ChangeShader window = (ChangeShader)EditorWindow.GetWindowWithRect (typeof (ChangeShader),wr, true,"Change Shader");	
		window.Show();

    }
	
	private Shader shader;
	
	void OnGUI ()
	{
		for(int i =0; i<lineArray.Length; i++)
		{
			string[] text = lineArray[i].Split(new char[]{'&'}, System.StringSplitOptions.RemoveEmptyEntries);
            if (text.Length != 2) continue;
			if(GUILayout.Button(lineArray[i].Replace("&", "to")))
			{
				Change(text[0].Trim(), text[1].Trim());		
			}
		}
	}
	

	void Change(string srcShader, string destShader)
	{
		if(Selection.activeGameObject != null)
		{
			foreach(GameObject g in Selection.gameObjects)
			{
				Renderer []renders = g.GetComponentsInChildren<Renderer>();
				foreach(Renderer r in renders)
				{
					if(r  !=  null)
					{
						foreach(Material o in r.sharedMaterials)
						{
							if (o.shader.name == srcShader)
							{
								string path = AssetDatabase.GetAssetPath(o);
								Material m = AssetDatabase.LoadAssetAtPath(path,typeof(Material)) as Material;
								m.shader = Shader.Find(destShader);

								this.ShowNotification(new GUIContent("from " + srcShader + " to " + destShader + " success."));
							}
                            else if (srcShader == "All")
                            {
                                o.shader = Shader.Find(destShader);
                            }
						}
					}
				}
			}	
		}else
		{
			this.ShowNotification(new GUIContent("没有在Hierarchy视图中选择对象"));

		}
	}
	
}
