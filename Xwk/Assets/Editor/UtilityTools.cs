using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class UtilityTools {

    public static void DropToTextFiled(Rect rect, ref string path)
    {
        //如果鼠标正在拖拽中或拖拽结束时，并且鼠标所在位置在文本输入框内  
        if (/*(Event.current.type == EventType.DragUpdated
          || Event.current.type == EventType.DragExited)*/

           rect.Contains(Event.current.mousePosition))
        {
            //改变鼠标的外表  
            DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
            if (Event.current.type == EventType.DragPerform && DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
            {
                path = DragAndDrop.paths[0];
            }
        }
    }
}
