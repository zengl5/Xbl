using UnityEngine;

public class AnimatedTextureUV : MonoBehaviour
{

    //vars for the whole sheet
    public int colCount = 8;
    public int rowCount = 8;

    //vars for animation
    public int rowNumber = 0; //Zero Indexed   从第几行开始读取贴图
    public int colNumber = 0; //Zero Indexed   从第几列开始读取贴图 
    public int totalCells = 8;                 //读去几列贴图
    public int fps = 8;

    public bool runOnce = true;
    private bool stop = false;


    //Maybe this should be a private var
    //private Vector2 offset;
    private Renderer cachedRenderer;
    void Start()
    {
        cachedRenderer = GetComponent<Renderer>();
    }

    //Update
    void Update()
    {
       
         SetSpriteAnimation(colCount, rowCount, rowNumber, colNumber, totalCells, fps);
       
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (++rowNumber > rowCount - 1)
                rowNumber = 0;
            stop = false;
        }
    }

    //SetSpriteAnimation
    void SetSpriteAnimation(int colCount, int rowCount, int rowNumber, int colNumber, int totalCells, int fps)
    {
        if (stop)
        {
            return;
        }

        // Calculate index
        int index = (int)(Time.time * fps);
        // Repeat when exhausting all cells
        index = index % totalCells;

        // Size of every cell
        float sizeX = 1.0f / colCount;
        float sizeY = 1.0f / rowCount;
        Vector2 size = new Vector2(sizeX, sizeY);

        // split into horizontal and vertical index
        var uIndex = index % colCount;
        var vIndex = index / colCount;

        // build offset
        // v coordinate is the bottom of the image in opengl so we need to invert.
        float offsetX = (uIndex + colNumber) * size.x;
        //Debug.Log(colNumber.ToString());
        float offsetY = (1.0f - size.y) - (vIndex + rowNumber) * size.y;
        Vector2 offset = new Vector2(offsetX, offsetY);

        cachedRenderer.material.SetTextureOffset("_MainTex", offset);
        cachedRenderer.material.SetTextureScale("_MainTex", size);

       // print(index);

        if (runOnce)
        {
            //int number = colCount * colNumber + rowCount * rowNumber + totalCells;
            if (index == totalCells - 1)
            {
                stop = true;
            }
                
        }
    }
}