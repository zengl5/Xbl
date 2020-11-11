using UnityEngine;
using System.Collections;

namespace QFramework
{
    /// <summary>
    /// 帧率计算器
    /// </summary>
    public class QFPSCounter
    {
        // 帧率计算频率
        private const float calcRate = 0.5f;
        // 本次计算频率下帧数
        private int frameCount = 0;
        // 频率时长
        private float rateDuration = 0f;
        // 显示帧率
        private int fps = 0;

        private float _LastInterval;  

        public QFPSCounter(QConsole console)
        {
            console.onUpdateCallback += Update;
            console.onGUICallback += OnGUI;
        }

        void Start()
        {
            this.frameCount = 0;
            this.rateDuration = 0f;
            this.fps = 0;
            _LastInterval = Time.realtimeSinceStartup;
        }

        void Update()
        {
            ++this.frameCount;
           // this.rateDuration += Time.deltaTime;
            //if (this.rateDuration > calcRate)
            if (Time.realtimeSinceStartup > _LastInterval + calcRate)  
            {
                // 计算帧率
               // this.fps = (int)(this.frameCount / this.rateDuration);
                this.fps = (int)(this.frameCount / (Time.realtimeSinceStartup - _LastInterval));
                this.frameCount = 0;
                this.rateDuration = 0f;

                _LastInterval = Time.realtimeSinceStartup;  
            }
        }

        void OnGUI()
        {
            GUI.color = Color.black;
            GUI.Label(new Rect(80, 20, 120, 20), "fps:" + this.fps.ToString());
        }
    }

}