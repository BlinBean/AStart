using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AStartNodeManage  
{
    static AStartNodeManage instance;

    public AStartNode[,] map;

    List<AStartNode> openList=new List<AStartNode>();
    List<AStartNode> closeList=new List<AStartNode>();

    int mapHight;
    int mapWidth;

    //单例
    public static AStartNodeManage Instance
    {
        get {
            if (instance == null)
            {
                instance = new AStartNodeManage();
            }
            return instance;
        }
    }

    //初始化
    public void InitMapInfo(int hight,int width)
    {
        this.mapHight = hight;
        this.mapWidth = width;
        map = new AStartNode[hight, width];
        for (int w=0; w<hight; w++)
        {
            for (int h=0; h<width; h++)
            {
                map[w, h] = new AStartNode(w,h,Random.Range(0,100)<20 ? AStartNodeType.Stop : AStartNodeType.Walk);
            }
        }
    }

    //寻路
    public List<AStartNode> FindPath(Vector2 start, Vector2 end)
    {
        //判断起点终点是否在范围内
        if  ((start.x>mapWidth || start.y>mapHight || 
            start.x<0 || start.y<0) ||
            (end.x>mapWidth || end.y>mapHight ||
            end.x<0 || end.y<0))
        {
            Debug.Log("不在地图内");
            return null;
        }

        //得到起点终点对饮的格子
        AStartNode startNode = map[(int)start.x, (int)start.y];
        AStartNode endNode = map[(int)end.x, (int)end.y];

        //判断对应的格子类型是否为阻挡
        if (startNode.nodeType == AStartNodeType.Stop ||
            endNode.nodeType == AStartNodeType.Stop)
        {
            return null;
        }

        //---------------------------------起点终点判断完成

        //清空开启和关闭列表//这个函数不止执行一次

        openList.Clear();
        closeList.Clear();

        //设置关闭列表的下标
        int closeIndex = 0;

        //设置起点的属性//清空
        startNode.father = null;
        startNode.g = 0;
        startNode.h = 0;
        startNode.f = 0;

        //向关闭列表中添加起点
        closeList.Add(startNode);

        //--------------------------------

        while (true)
        {
            FindNearlyNodeToOpenList(startNode.x - 1, startNode.y - 1, 1.4f, startNode, endNode);//左上
            FindNearlyNodeToOpenList(startNode.x - 1, startNode.y    , 1f  , startNode, endNode);//左
            FindNearlyNodeToOpenList(startNode.x - 1, startNode.y + 1, 1.4f, startNode, endNode);//左下
            FindNearlyNodeToOpenList(startNode.x    , startNode.y - 1, 1f  , startNode, endNode);//上
            FindNearlyNodeToOpenList(startNode.x    , startNode.y + 1, 1f  , startNode, endNode);//下
            FindNearlyNodeToOpenList(startNode.x + 1, startNode.y - 1, 1.4f, startNode, endNode);//右上
            FindNearlyNodeToOpenList(startNode.x + 1, startNode.y    , 1f  , startNode, endNode);//右
            FindNearlyNodeToOpenList(startNode.x + 1, startNode.y + 1, 1.4f, startNode, endNode);//右下

            if (openList.Count == 0)
            {
                Debug.Log("死路");
                return null;
            }

            //排序
            openList.Sort((x, y) => x.f.CompareTo(y.f));

            //将消耗最低的移植关闭列表
            closeList.Add(openList[0]);
            startNode = openList[0];
            openList.RemoveAt(0);

            if (startNode == endNode)
            {
                List<AStartNode> path = new List<AStartNode>();
                path.Add(endNode);
                while (endNode.father!=null)
                {
                    path.Add(endNode.father);
                    endNode = endNode.father;
                }
                path.Reverse();
                return path;
            }

        }

    }


    void FindNearlyNodeToOpenList(int x, int y, float g, AStartNode fatherNode, AStartNode endNode)
    {
        //判断坐标
        if (x>= mapWidth ||
            y>= mapHight ||
            x< 0 ||
            y< 0)
        {
            return;
        }
        //Debug.Log(x+","+y);
        //判断格子类型
        if (map[x, y].nodeType == AStartNodeType.Stop)
        {
            return;
        }

        //设置临时变量
        AStartNode findNodeTemp = map[x, y];

        //判断是否存在开启列表
        if (openList.Contains(findNodeTemp) || closeList.Contains(findNodeTemp))
        {
            return;
        }
        //-------------------------------------------

        //设置父节点
        findNodeTemp.father = fatherNode;

        //设置离起点距离
        findNodeTemp.g = fatherNode.g + g;

        //设置离终点距离
        findNodeTemp.h = Mathf.Abs(findNodeTemp.x - endNode.x) + Mathf.Abs(findNodeTemp.y - endNode.y);

        //设置总消耗
        findNodeTemp.f = findNodeTemp.g + findNodeTemp.h;

        //向开启列表添加节点
        openList.Add(findNodeTemp);
    }
}
