using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStartTest : MonoBehaviour
{
    public int beginX;
    public int beginY;

    public int offsetX=2;
    public int offsetY=2;

    public int mapW=5;
    public int mapH=5;

    Vector2 beginPos = Vector2.right * -1;
    Dictionary<string,GameObject> cubes=new Dictionary<string,GameObject>();
    List<AStartNode> path = new List<AStartNode>();

    void Start()
    {
        AStartNodeManage.Instance.InitMapInfo(mapH,mapW);
        for(int i=0; i < mapW; i++)
        {
            for (int j=0;j<mapH;j++)
            {
                //实例化格子
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                //设置位置
                obj.transform.position = new Vector3(beginX + offsetX * i, beginY + offsetY * j, 0);
                //重命名
                obj.name = i + "_" + j;
                //存入字典中
                cubes.Add(obj.name,obj);

                //--------------------------------------------

                //建立对应的A*管理地图
                AStartNode node = AStartNodeManage.Instance.map[i, j];
                if (node.nodeType == AStartNodeType.Stop)
                {
                    obj.GetComponent<Renderer>().material.color = Color.red;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //射线射中的物体的信息
            RaycastHit info;

            //以相机屏幕向鼠标位置新建射线
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray,out info,1000))
            {
                //如果还没点击过
                if (beginPos==Vector2.right*-1)
                {
                    //清除上一次的
                    foreach(var item in path)
                    {
                        cubes[item.x + "_" + item.y].GetComponent<Renderer>().material.color = Color.white;
                    }
                    path.Clear();

                    string[] cubeName = info.transform.gameObject.name.Split('_');

                    beginPos = new Vector2(int.Parse(cubeName[0]),int.Parse(cubeName[1]));

                    info.transform.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                }
                else
                {
                    string[] cubeName = info.transform.gameObject.name.Split('_');

                    Vector2 endPos = new Vector2(int.Parse(cubeName[0]), int.Parse(cubeName[1]));

                    path=AStartNodeManage.Instance.FindPath(beginPos,endPos);

                    if (path != null)
                    {
                        foreach (var item in path)
                        {
                            cubes[item.x + "_" + item.y].GetComponent<Renderer>().material.color = Color.green;
                        }
                    }

                    beginPos = Vector2.right * -1;
                }
            }
        }
    }
}
