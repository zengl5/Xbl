using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class MessageBoxEditor
{

    public static void ShowSaveFileBox(string title ,string msg,string cover,string append,System.Action coverCallback=null,System.Action appendCallback=null)
    {
        if (EditorUtility.DisplayDialog(title, msg, cover, append))
        {
            if (coverCallback != null)
            {
                coverCallback();
            }
        }
        else
        {
            if (appendCallback != null)
            {
                appendCallback();
            }
        }
    }
    public static void ShowSaveFileBoxComplex(string title, string msg, string cover, string append, string cancletxt, System.Action coverCallback = null, System.Action appendCallback = null, System.Action cancle = null)
    {
        int index = EditorUtility.DisplayDialogComplex(title, msg, cover, append, cancletxt);
        switch(index){
            case 0:
                {
                     if (coverCallback != null)
                    {
                        coverCallback();
                    }
                }
                break;
            case 1:
                {
                    if (appendCallback != null)
                    {
                        appendCallback();
                    }
                }
                break;
            case 2:
                {
                    if (cancle != null)
                    {
                        cancle();
                    }
                }
                break;
            default:
                break;

        }
    }
    public static void ShowErrorBox(string title, string errorMsg, string ok, System.Action okCallback = null)
    {
        if (EditorUtility.DisplayDialog(title, errorMsg, ok))
        {
            if (okCallback != null)
            {
                okCallback();
            }
        }
         
    }
    public static void DoFileSave(string path,string SaveContent, System.Action Callback = null)
    {
        MessageBoxEditor.ShowSaveFileBox("林志玲提示", "是否覆盖" + path, "覆盖，先检查下文本内容，点错别怪我", "不覆盖,追加在文本内容最后", () =>
        {
            FileTools.CreateFile(path, SaveContent);
            if (Callback != null)
            {
                Callback();
            }
        }, () =>
        {
            FileTools.CreateFile(path, SaveContent, true);
            if (Callback != null)
            {
                Callback();
            }
        });
    }
    public static void DoFileSaveComplex(string path, string SaveContent)
    {
        MessageBoxEditor.ShowSaveFileBoxComplex("林志玲提示", "是否覆盖" + path, "覆盖，先检查下文本，点错别怪我", "不覆盖,追加在文本内容最后", "取消保存", () =>
        {
            FileTools.CreateFile(path, SaveContent);
            
        }, () =>
        {
            FileTools.CreateFile(path, SaveContent, true);
            
        }, () => {
             
        }
        );
    }
}
