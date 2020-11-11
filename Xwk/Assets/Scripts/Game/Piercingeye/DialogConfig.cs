using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public struct SpriteTransform
{
    public Vector3 Position;
    public Vector3 erlerAngles;
    public Vector3 localScale;
    public void InitSpTransform(Vector3 ps, Vector3 el, Vector3 ls)
    {
        Position = ps;
        erlerAngles = el;
        localScale = ls;
    }
}
/// <summary>
/// 精灵配置类 [精灵假体名字，精灵真身名字]
/// </summary>
public class SpritConfig
{        
    //真身
    public string Real_spritePath;//真身mesh路径
    public string Real_AnimCtrlPath;//动画控制器路径
    public SpriteTransform Real_SpTransfrom;
    public string normalTex;//默认解锁之后的贴图

    public Vector3 SpriteGroundPos;
    public Vector3 GyroCamPos=Vector3.zero;//相机位置

    //假体
    public SpriteTransform SpTransfrom_Jiati;
    public string spritePath_Jiati;//假体mesh路径
    public string AnimCtrlPath_Jiati;//动画控制器路径

    public string SpNameSound;//精灵自我介绍声音
    public string RolelockingMat;
    public string RoleNormalMat;

    

    //移动位置
    public Vector3 MovePostion;
    public Vector3 MoveEulerAngles;
 
   
}
/// <summary>
/// 对话框管理类
/// </summary>
public class DialogInfo
{
    
     
    Vector3[] texPos = new Vector3[] { new Vector3(450, 35, 0), new Vector3(450, -35, 0), new Vector3(450, -343, 0) };
    public void AddTextCompnent(Text tex)
    {
        TextList.Add(tex);
    }
    public Vector3 GetTextPos(int id)
    {
        if (id < texPos.Length)
            return texPos[id];
        else return Vector3.zero;
    }
    public Text GetText(int id)
    {
        if (id < TextList.Count)
            return TextList[id];
        else
            return null;
    }
    public string Tpinfo;
    public string Ctinfo;
    public string Ltinfo;

    public string SpInfoSound;

    public string TitleTex;

    public Color TpLtcolor = new Color(242 / 255f, 175 / 255f, 89 / 255f);
    public Color Ctcolor = new Color(219 / 255f, 199 / 255f, 152 / 255f);
    List<Text> TextList = new List<Text>();
}
public class DialogConfig : C_Singleton<DialogConfig>
{
    public string fireTop = "百音精灵";
    public string fireCenter = "她可以听到遥远的声音。只要和她做朋友就能学会分辨世界上所有的声音。";
    public string fireLast = "特殊能力：可以听到遥远的声音。";

    public string huluTop = "百变葫芦";
    public string huluCenter = "他是太上老君的法宝——紫金葫芦。他的肚子能变出各种各样的物品。只要和他做朋友就可以认识到各种新鲜事物。";
    public string huluLast = "特殊能力：能变出千奇百怪的东西。";

    public string xlnTop = "小龙女";
    public string xlnCenter = "她是海底龙宫的守护者。只要和她做朋友就能听懂鱼儿说的话。";
    public string xlnLast = "特殊能力：运用法术控制海底风暴。";

    public string lingwaTop = "灵蛙";
    public string lingwaCenter = "他能利用灵气预测不久的未来。只要和他做朋友就能知道未来要发生的事情。";
    public string lingwaLast = "特殊能力：预测未来。";

    public string xcwwTop = "仙草娃娃";
    public string xcwwCenter = "他能治好各种各样的疾病。只要和他做朋友就永远都不会生病啦！";
    public string xcwwLast = "特殊能力：变出包治百病的神草。";

    public string hxjlTop = "海宝";
    public string hxjlCenter = "他的肚子能装下大量的东西。只要和他做朋友就能进入他肚子里获得他的保护。";
    public string hxjlLast = "特殊能力：超级防护。";

    //新增
    public string Top1 = "大嘴兽";
    public string Center1 = "他能把吃下去的魔气变成灵气。只要和他做朋友就不用害怕魔气啦！";
    public string Last1 = "特殊能力：将魔气变成灵气。";

    public string Top2 = "梦梦喵";
    public string Center2 = "他能用眼睛催眠让人进入梦境。只要和他做朋友就能天天做美梦。";
    public string Last2 = "特殊能力：快速催眠。";


    public string Top3 = "火火龙";
    public string Center3 = "他能吐出消灭魔气的火焰。只要和他做朋友，他就能帮你消灭魔气。";
    public string Last3 = "特殊能力：吐出无敌火焰。";


    public string Top4 = "咻咻蜗牛";
    public string Center4 = "他是世界上跑的最快的蜗牛。只要和他做朋友，他就能带你体验极速飞行。";
    public string Last4 = "特殊能力：极速飞行。";

    public string Top5 = "牛小仙";
    public string Center5 = "他能用灵气生产让人感到快乐的牛角包。吃了他的牛角包就能哈哈大笑。";
    public string Last5 = "特殊能力：变出快乐牛角包。";


    public string Top6 = "西西狼";
    public string Center6 = "他能感应到散落在世界各地的灵气。只要和他做朋友就能找到一些神秘的灵气宝藏。";
    public string Last6 = "特殊能力：感应到灵气宝藏。";

    public string Top7 = "计算鸡";
    public string Center7 = "他能凭空变出大量灵气。只要和他做朋友就能获得很多很多灵气。";
    public string Last7 = "特殊能力：生产灵气。";



    public string Top8 = "年兽宝宝";
    public string Center8 = "他大年三十就会出来捣乱。只要赶走他就能开开心心过大年。";
    public string Last8 = "特殊能力：可以让时间停留在大年三十。";

    public string Top9 = "红包精灵";
    public string Center9 = "他能发出无数的大红包。只要和他做朋友就能每天收到大红包啦！";
    public string Last9 = "特殊能力：可以发出无数的红包。";
}
