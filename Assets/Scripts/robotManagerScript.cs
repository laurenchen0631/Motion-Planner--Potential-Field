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
        Configuration goal = robot.configuration;
        List<Configuration> goalConfigList = new List<Configuration>();

        for (int i = 0; i < robot.getNumControls(); i++)
        {
            Vector2 controlPos = robot.getControlPos(i, true);
            Configuration config = new Configuration(controlPos.x, controlPos.y, goal.theta);
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
        List<Configuration>[] configList = new List<Configuration>[255]; //Li, i=0,1,...,254, is a list of configurations; it is initially empty.
        
        //U(goal)=0; insert goal in L0
        bitmap[(int)goal.x, (int)goal.y] = 0;
        configList[0].Add(goal);

        //for i=0,1, .., until Li is empty do
        //for every q in Li do
        for (int i = 0, j = 0; i < 254; j++)
        {
            //for every 1-neighbor q' in GCfree do
            for (int direction = 0; direction < 4; direction++)
            {
                Configuration q = (configList[i])[j];
                if (direction == 0) q.x += 1; //Right
                else if (direction == 1) q.y -= 1;//Down
                else if (direction == 2) q.x -= 1;//Left
                else q.y += 1;// Up    

                // if U(q') = M then
                if (bitmap[(int)q.x, (int)q.y] == M)
                {
                    //U(q') = i+1
                    bitmap[(int)(1f + q.x), (int)(1f + q.y)] = (byte)(i + 1);
                    //insert q' at the end of Li+1
                    configList[i + 1].Add(q);
                }
            }

            //until Li is empty
            if (j == configList[i].Count - 1) i++; j = 0;
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

        //The overall potential value U is computed by composing the potential values from p1 to pn
        for (int i = 0; i < 130; i++)
        {
            for (int j = 0; j < 130; j++)
            {
                for (int n = 0; n < bitmaps.Count; n++)
                    BITMAP[i, j] += (int)(bitmaps[n])[i, j];
            }
        }

        return BITMAP;
    }

    private void BFS(int[,] bitmap, Configuration start, Configuration goal)
    {
        int[,] BITMAP = bitmap;
        const int M = 510;
        //install Xinit in T; [initially, T is the empty tree]
        Configuration initX = goal;
        NTree<Configuration> T = new NTree<Configuration>(initX);

        //INSERT(Xinit, OPEN); mark Xinit visited;
        //[initially, all the points in the grid are marked “unvisited”]
        LinkedList<Configuration>[] OPEN = new LinkedList<Configuration>[512];
        bool[,] isVisited = new bool[130, 130];
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
        obstacleBitmap = GameObject.Find("Obstacle Manager").GetComponent<obstacleManagerScript>().initBitmap();

        for (int i = 0; i < numOfRobot; i++)
        {
            BFS(Arbitration(i), robotList[i].getConfiguration(), getOriginGoal(i));
        }
    }
}

public class Robot
{
    private int numOfPolygon = 0;
    private List<Polygon> polygonList = new List<Polygon>();

    private int numOfControl = 0;
    private List<Vector2> controlList = new List<Vector2>();

    private Configuration configuration = new Configuration();

    public GameObject gameobject = new GameObject();
    private float UNIT = 8.0f / 128.0f;

    public Robot()
    {
        setupGameobject();
    }

    public Robot(int nPolygons, Polygon[] polygons)
    {
        setupGameobject();
        for (int i = 0; i < nPolygons; i++)
            addPolygon(polygons[i]);
    }

    private void setupGameobject()
    {
        gameobject.name = "Robot";
        gameobject.tag = "Robot";
        gameobject.layer = LayerMask.NameToLayer("Robot"); ;
        gameobject.AddComponent<objectEditor>();
    }

    public void addPolygon(Polygon newPolygon)
    {
        numOfPolygon++;
        polygonList.Add(newPolygon);
        newPolygon.gameobject.transform.parent = this.gameobject.transform;
        newPolygon.gameobject.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public Configuration getConfiguration()
    {
        return gameobject.GetComponent<robotDetailScript>().configuration;
    }

    public void setConfiguration(float[] config)
    {
        configuration.setConfig(config[0], config[1], config[2]);
    }

    public void addControlPoint(Vector2 newPoint)
    {
        numOfControl++;
        controlList.Add(newPoint);
    }

    public void setControlPoint(int index, Vector2 newPoint)
    {
        controlList[index] = newPoint;
    }

    public Vector2 getControlPoint(int index)
    {
        return controlList[index];
    }

    public List<Vector2> getControls()
    {
        return controlList;
    }

    public void modifyPolygon(int index, Polygon newPolygon)
    {
        if (index >= 0 && index < numOfPolygon)
            polygonList[index] = newPolygon;
    }

    public void applyTransform()
    {
        gameobject.transform.localScale = new Vector3(UNIT, 1, UNIT);
        gameobject.transform.position = new Vector3(configuration.x * UNIT, 0, configuration.y * UNIT);
        gameobject.transform.Rotate(Vector3.up * -configuration.theta);
    }

    private void ChangeLayers(Transform trans)
    {
        trans.gameObject.layer = LayerMask.NameToLayer("Robot");
        foreach (Transform child in trans)
        {
            ChangeLayers(child);
        }
    }

    private void setupRigidbody()
    {
        Rigidbody rigid = gameobject.AddComponent<Rigidbody>();
        rigid.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void setupCollider()
    {
        MeshCollider collider = this.gameobject.AddComponent(typeof(MeshCollider)) as MeshCollider;
        MeshFilter[] meshFilters = this.gameobject.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.worldToLocalMatrix;
            i++;
        }
        Mesh mesh = new Mesh();
        mesh.CombineMeshes(combine);
        collider.sharedMesh = mesh;
    }

    public void Draw3D(int index)
    {
        for (int i = 0; i < numOfPolygon; i++)
            polygonList[i].threeDMesh(Color.red);

        gameobject.name = "Robot " + index.ToString();
        robotDetailScript script = gameobject.AddComponent<robotDetailScript>();
        script.setControls(controlList);
        ChangeLayers(gameobject.transform);
        setupRigidbody();
        applyTransform();
    }

    public void Draw2D(int index)
    {
        for (int i = 0; i < numOfPolygon; i++)
            polygonList[i].updateMesh(Color.blue);

        gameobject.name = "Robot " + index.ToString();
        setupCollider();
        applyTransform();
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
}

public class Configuration
{
    public float x;
    public float y;
    public float theta;

    public Configuration()
    {
        x = 0;
        y = 0;
        theta = 0;
    }

    public Configuration(float x, float y, float theta)
    {
        setConfig(x, y, theta);
    }

    public void setConfig(float x,float y,float theta)
    {
        this.x = x;
        this.y = y;
        this.theta = theta;
    }
}