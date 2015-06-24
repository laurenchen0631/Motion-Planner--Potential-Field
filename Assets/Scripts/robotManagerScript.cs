using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class robotManagerScript : MonoBehaviour
{
    private int numOfRobot = 0;
    private List<Robot> robotList = new List<Robot>();
    public List<Configuration> goalConfigList = new List<Configuration>();
    public List<GameObject> goalGameobjects = new List<GameObject>();
    private float UNIT = 8.0f / 128.0f;
    public bool isStarting = false;
    byte[,] obstacleBitmap = new byte[130, 130];

    public void drawRobots()
    {
        for (int i = 0; i < numOfRobot; i++)
        {
            drawRobot(i);
            drawGoal(i);
        }
    }

    private void drawRobot(int index)
    {
        robotList[index].Draw3D(index);
    }

    private void drawGoal(int index)
    {
        GameObject go = GameObject.Instantiate(robotList[index].gameobject);
        goalGameobjects.Add(go);
        go.name = "Robot " + index + " Goal";
        go.transform.parent = this.gameObject.transform;
        go.transform.localScale = new Vector3(UNIT, 0.5f, UNIT);

        Configuration configuration = goalConfigList[index];
        go.transform.position = new Vector3(configuration.x * UNIT, 0, configuration.y * UNIT);
        go.transform.Rotate(Vector3.up * -configuration.theta);

        //robotDetailScript script = go.AddComponent<robotDetailScript>();
        //script.setControls(robotList[index].getControls());

        foreach (MeshRenderer render in goalGameobjects[index].GetComponentsInChildren<MeshRenderer>())
            render.material.color = Color.blue;
    }

    public void addRobot(Robot newRobot)
    {
        numOfRobot++;
        robotList.Add(newRobot);
        newRobot.gameobject.transform.parent = this.gameObject.transform;
    }

    public void addGoal(float[] newGoal)
    {
        if (newGoal.Length == 3)
            goalConfigList.Add(new Configuration(newGoal[0], newGoal[1], newGoal[2]));
    }

    /*
    public byte[,] initBitmap(int index)
    {
        byte[,] bitmap = new byte[130, 130];

        for (int i = 0; i < 130; i++)
        {
            for (int j = 0; j < 130; j++)
            {
                RaycastHit hit;
                if (i == 0 | j == 0 | i == 129 | j == 129)
                    bitmap[i, j] = 255;
                else if (Physics.Raycast(new Vector3(UNIT / 2.0f + (j - 1) * UNIT, 2.0f, UNIT / 2.0f + (i - 1) * UNIT),
                     Vector3.down, out hit, 1.5f))
                {
                    if (hit.collider.gameObject.transform.parent.tag == "Obstacle")
                    {
                        bitmap[i, j] = 255;
                        print("Hit " + hit.collider.name + " at (" + i + ", " + j + ")");
                    }
                }
                else
                {
                    bitmap[i, j] = 254;
                    //Debug.Log("No Obstacle");
                }

            }
        }

        //List<Vector2> controls = goalGameobjects[index].GetComponent<robotDetailScript>().getControls();

        return bitmap;
        //Debug.Log("Complete");
    }
    */

    private List<Configuration> getGoalConfigs(int index)
    {
        robotDetailScript robot = goalGameobjects[index].GetComponent<robotDetailScript>();
        //Configuration goal = robot.configuration;
        List<Configuration> goalConfigList = new List<Configuration>();

        for (int i = 0; i < robot.getNumControls(); i++)
        {
            Vector2 controlPos = robot.getControlPos(i, true);
            Configuration config = new Configuration(controlPos.x, controlPos.y, robot.configuration.theta);
            goalConfigList.Add(config);
        }

        return goalConfigList;
    }

    private Configuration getOriginGoal(int index)
    {
        return goalGameobjects[index].GetComponent<robotDetailScript>().configuration;
    }

    public byte[,] NF1(Configuration goal)
    {
        byte[,] bitmap = new byte[130, 130];
        System.Array.Copy(obstacleBitmap, bitmap, obstacleBitmap.Length);
        const byte M = 254;
        Queue<Configuration>[] configList = new Queue<Configuration>[255]; //Li, i=0,1,...,254, is a list of configurations; it is initially empty.
        for (int i = 0; i < 255; i++) configList[i] = new Queue<Configuration>();

        //U(goal)=0; insert goal in L0
        bitmap[Mathf.FloorToInt(1f + goal.y), Mathf.FloorToInt(1f + goal.x)] = 0;
        configList[0].Enqueue(goal);

        //for i=0,1, .., until Li is empty do
        //for every q in Li do
        for (int i = 0; i < 254 && configList[i].Count > 0; )
        {
            Configuration point = configList[i].Dequeue();

            //for every 1-neighbor q' in GCfree do
            for (int direction = 0; direction < 4; direction++)
            {
                Configuration q = new Configuration(point);
                if (direction == 0) q.x += 1f; //Right
                else if (direction == 1) q.y -= 1f;//Down
                else if (direction == 2) q.x -= 1f;//Left
                else q.y += 1f;//Up
                
                // if U(q') = M then
                if (bitmap[Mathf.FloorToInt(1f + q.y), Mathf.FloorToInt(1f + q.x)] == M)
                {
                    //U(q') = i+1
                    bitmap[Mathf.FloorToInt(1f + q.y), Mathf.FloorToInt(1f + q.x)] = (byte)(i + 1);
                    //if(i<5)
                    //    Debug.Log("bitmap[" + Mathf.FloorToInt(q.y) + "," + Mathf.FloorToInt(q.x) + "] = " + (i + 1));
                    //insert q' at the end of Li+1
                    configList[i + 1].Enqueue(q);
                }
            }

            //until Li is empty
            if (configList[i].Count == 0) { i++; /*print("i:" + i + ", Count: " + configList[i].Count);*/ }
        }

        return bitmap;
    }

    private int[,] Arbitration(int index)
    {
        List<Configuration> goals = getGoalConfigs(index);
        List<byte[,]> bitmaps = new List<byte[,]>();
        int[,] BITMAP = new int[130, 130];

        //Select s control point pi in A.
        //For every point pi compute a local-minimum-free potential Vi(x) over the workspace, with a global minimum at pi(qgoal).
        for (int i = 0; i < goals.Count; i++)
            bitmaps.Add( NF1(goals[i]) );

        for (int i = 0; i < goals.Count; i++)
            print("(" + goals[i].x + "," + goals[i].y + ")");

        GameObject.Find("Canva Bound").GetComponent<drawBitmap>().draw(bitmaps[0]);

        //The overall potential value U is computed by composing the potential values from p1 to pn
        for (int i = 0; i < 130; i++)
        {
            for (int j = 0; j < 130; j++)
            {
                for (int n = 0; n < bitmaps.Count; n++)
                    BITMAP[i, j] += (int)(bitmaps[n][i, j]);
            }
        }

        return BITMAP;
    }

    private void BFS(int[,] bitmap, Configuration start, Configuration goal)
    {
        int[,] BITMAP = bitmap;
        const int M = 510;
        LinkedList<Configuration>[] OPEN = new LinkedList<Configuration>[512];
        for (int i = 0; i < OPEN.Length; i++) OPEN[i] = new LinkedList<Configuration>();
        bool[,] isVisited = new bool[128, 128];

        //install Xinit in T; [initially, T is the empty tree]
        Configuration initX = goal;
        NTree<Configuration> T = new NTree<Configuration>(initX);

        //INSERT(Xinit, OPEN); mark Xinit visited;
        //[initially, all the points in the grid are marked “unvisited”]
        int _index = BITMAP[(int)(1 + initX.x), (int)(1 + initX.y)];
        OPEN[_index].AddFirst(initX);
        isVisited[(int)initX.x, (int)initX.y] = true;
        
        //SUCCESS ← false;
        bool SUCCESS = false;

        //while ┐ EMPTY(OPEN) and ┐SUCCESS do
        while (!EMPTY<Configuration>(OPEN) && !SUCCESS)
        {
            //x ← FIRST(OPEN);
            LinkedListNode<Configuration> x = OPEN[_index].First;
            //OPEN[_index++].RemoveFirst();
            
            //for every neighbor x’ of x in the grid do
            for (int direction = 0; direction < 4; direction++)
            {
                Configuration y = new Configuration(x.Value.x, x.Value.y, x.Value.theta);
                if (direction == 0)         y.x += 1; //Right
                else if (direction == 1)    y.y -= 1; //Down
                else if (direction == 2)    y.x -= 1; //Left
                else                        y.y += 1; // Up

                //if U(x’) < M and x’ is not visited then
                if (BITMAP[(int)(1 + y.x), (int)(1 + y.y)] < M && !isVisited[(int)y.x, (int)y.y])
                {
                    //install x’ in T with a pointer toward x;
                    
                    //INSERT(x’, OPEN); mark x’ visited;
                    //OPEN[_index].AddFirst(y);
                    isVisited[(int)y.x, (int)y.y] = true;

                    //if x’ = Xgoal then SUCCESS ← true;
                    if (BITMAP[(int)(1 + y.x), (int)(1 + y.y)] == 0)
                        SUCCESS = true;
                }
            }
        }

        //if SUCCESS then
        //    return the constructed path by tracing the pointers in T from xgoal back to xinit
        //else return failure;

    }

    private bool EMPTY<T>(LinkedList<T>[] OPEN)
    {
        for (int i = 0; i < OPEN.Length; i++)
            if (OPEN[i].Count != 0) return false;
        return true;
    }

    public void resolvePotential()
    {
        isStarting = true;
        obstacleBitmap = GameObject.Find("Obstacle Manager").GetComponent<obstacleManagerScript>().initBitmap();
        Arbitration(0);

        for (int i = 0; i < numOfRobot; i++)
        {
            //Arbitration(i);
            //BFS(Arbitration(i), robotList[i].getConfiguration(), getOriginGoal(i));
        }
    }
}



delegate void TreeVisitor<T>(T nodeData);

class NTree<T>
{
    private T data;
    private LinkedList<NTree<T>> children;
    private NTree<T> parent;

    public NTree(T data)
    {
        this.data = data;
        children = new LinkedList<NTree<T>>();
    }

    public NTree(T data, NTree<T> parent)
    {
        this.data = data;
        this.parent = parent;
        children = new LinkedList<NTree<T>>();
    }

    public void AddChild(T data)
    {
        children.AddFirst(new NTree<T>(data));
    }

    public NTree<T> GetChild(int i)
    {
        foreach (NTree<T> n in children)
            if (--i == 0)
                return n;
        return null;
    }

    public void Traverse(NTree<T> node, TreeVisitor<T> visitor)
    {
        visitor(node.data);
        foreach (NTree<T> kid in node.children)
            Traverse(kid, visitor);
    }
}

class potentialNode
{
    byte potential;
    NTree<potentialNode> T;
    int heuristic;
    int step;
}