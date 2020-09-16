public enum AStartNodeType
{
    Walk,
    Stop,
}

public class AStartNode 
{
    public AStartNode father;

    public float g;
    public float h;
    public float f;

    public int x;
    public int y;

    public AStartNodeType nodeType;

    public AStartNode(int x, int y , AStartNodeType type)
    {
        this.x = x;
        this.y = y;
        this.nodeType = type;
    }
}
