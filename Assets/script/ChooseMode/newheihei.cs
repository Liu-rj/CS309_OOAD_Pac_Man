using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class newheihei : MonoBehaviour
{
    // public Camera camera_one;
    public Rigidbody rd;
    public Canvas cv1;
    public Canvas cv_exit;
    public Canvas dialog;
    public Text content;
    public Canvas cv_error;
    public Canvas cv_error2;
    public Canvas cv_error3;
    public Transform up_t;
    public Transform down_t;
    public Transform right_t;
    public Transform left_t;
    public Text[] ranks = new Text[5];
    public Text[] scores = new Text[5];
    public static string[,] Maze;
    public static int Width;
    public static int Height;

    private Transform[] transcube = new Transform[4];
    private Vector3[] direct = new Vector3[4];
    private int speed = 6;
    private int curdirect;
    private float totaltime;
    private int act;
    private int cur;
    private int order;
    private int[] limit = {6, 5, 4,5,8};
    private String[] guidetxt = {"紫色的藤蔓漫过了卷王们的角斗场\n按Enter继续",
                              "神圣的塑像镇守者迷雾之门\n按Enter继续",
                              "AI在蓝色的轮回之门中搏杀\n按Enter继续",
                              "白银之阵变幻难测\n按Enter继续",
                              "黄金之门可以随汝意志而变\n按Enter继续",
                              "年轻人，选择你的道路吧。\n按Enter从头开始"};
    private String[] p2ptxt = {"人人对战模式\n按Enter继续",
                            "相传，由麦尔肯.习之.史蒂夫.张长老所创。\n按Enter继续",
                            "玩家可以通过局域网进行双人对战\n按Enter继续",
                            "最初是为了让学生之间切磋技艺\n按Enter从头开始"};
    private String[] e2etxt = {"机机对战模式\n按Enter继续",
                               "相传，由盖乌斯.家睿.尤里乌斯.骆长老所创。\n按Enter继续",
                               "玩家可以上传AI脚本来操控吃豆人\n按Enter继续",
                               "最初是为了让学生体验卷的快感\n按Enter从头开始"};
    private String[] difogtxt = {"哦，年轻人你好呀。\n按Enter继续",
                               "今天的天气真是不错。\n按Enter继续",
                               "我的意思是，为什么不来一局双人迷雾模式呢？\n按Enter继续",
                               "相传，由此模式阿尔芒.琳凯.普莱西.彭长老所创。\n按Enter继续",
                               "两个玩家可以在同一台电脑上竞技\n按Enter继续",
                               "虽然你不能像普通迷雾模式上一样切换视角了\n按Enter继续",
                               "但我保证，这将更加有趣\n按Enter继续",
                               "是不是很不错呢\n按Enter进入游戏"};
    private String[] fogtxt = {"欢迎来到真正的迷雾模式。\n按Enter继续",
                                "相传，由此模式由五大长老合力所创。\n按Enter继续",
                                "你的视野范围极小\n按Enter继续",
                                "好在你可以看到大力丸的位置\n按Enter继续",
                                "你敢挑战吗\n按Enter进入游戏"};
    private String[] p2etxt = {"自定义地图模式\n按Enter继续",
                               "相传，由卡尔特.硕.佛瑞德里希.黄长老所创。\n按Enter继续",
                               "玩家可以上传地图脚本\n按Enter继续",
                               "相比于随机模式,允许添加更多的道具\n按Enter继续",
                               "最初是为了让学生们自己选择想要训练的技能\n按Enter从头开始"};
    private String[] randomtxt = {"随机地图模式\n按Enter继续",
                                  "相传，由马斯特.仁杰.加巴斯.刘长老所创。\n按Enter继续",
                                  "地图由程序随机生成\n按Enter继续",
                                  "最初是为了让学生们适应多变的考试环境\n按Enter从头开始"};
    public bool moveable = true;
    private bool talk;

    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody>();
        rd.freezeRotation = true;
        // am = GetComponent<Animation>();
        direct[0] = new Vector3(1, -0.1f, 0) * speed;
        direct[1] = new Vector3(0, -0.1f, -1) * speed;
        direct[2] = new Vector3(-1, -0.1f, 0) * speed;
        direct[3] = new Vector3(0, -0.1f, 1) * speed;
        transcube[0] = up_t;
        transcube[1] = right_t;
        transcube[2] = down_t;
        transcube[3] = left_t;
        cv1.enabled = false;
        cv_exit.gameObject.SetActive(false);
        dialog.gameObject.SetActive(false);
        cv_error.gameObject.SetActive(false);
        cv_error2.gameObject.SetActive(false);
        cv_error3.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (talk&&Input.GetKeyDown(KeyCode.Return))
        {
            if (cur<limit[order]-1)
            {
                cur += 1;
            }
            else if (order==4)
            {
                
                ServerConnector.SendData("3");
                var signal = ServerConnector.ReceiveData();
                if (signal == "y")
                {
                    SceneManager.LoadScene(8);
                }
                cur = 0;
            }
            else if (order==3)
            {
                ServerConnector.SendData("3");
                var signal = ServerConnector.ReceiveData();
                if (signal == "y")
                {
                    SceneManager.LoadScene(11);
                }
                cur = 0;
            }
            else
            {
                cur = 0;
            }
        }
        if (Input.GetMouseButtonDown(0)||Input.GetMouseButtonDown(1))
        {
            cv_error.gameObject.SetActive(false);
            cv_error2.gameObject.SetActive(false);
            cv_error3.gameObject.SetActive(false);
        }

        if (moveable)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            if (v > 0)
            {
                curdirect = 3;
                rd.velocity = direct[curdirect];
                transform.LookAt(transcube[curdirect]);
            }
            else if (v < 0)
            {
                curdirect = 1;
                rd.velocity = direct[curdirect];
                transform.LookAt(transcube[curdirect]);
            }
            else if (h > 0)
            {
                curdirect = 0;
                rd.velocity = direct[curdirect];
                transform.LookAt(transcube[curdirect]);
            }
            else if (h < 0)
            {
                curdirect = 2;
                rd.velocity = direct[curdirect];
                transform.LookAt(transcube[curdirect]);
            }
        }

        if (notInGround())
        {
            var velocity = rd.velocity;
            velocity = new Vector3(velocity.x, -5, velocity.z);
            rd.velocity = velocity;
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            cv_exit.gameObject.SetActive(true);
        }
    }

    private bool notInGround()
    {
        Vector3 pos = transform.position;
        return !Physics.Linecast(pos, pos + new Vector3(0, -0.5f, 0));
    }

    public void change_moveable(bool move)
    {
        moveable = move;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("p2e"))
        {
            var filePath = FolderBrowserHelper.SelectFile(FolderBrowserHelper.PYFILTER);
            Debug.Log(filePath);
            if (filePath != "")
            {
                Process process = new Process();
                process.StartInfo.FileName = @"python.exe";
                // string path = Application.dataPath;
                // path += "/script/python_interface/py_import_test.py" + " --file " + filePath;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.Arguments = filePath;
                process.Start();
                process.BeginOutputReadLine();
                process.OutputDataReceived += DataReceiver;
                Console.ReadLine();
                process.WaitForExit();
                if (Maze[0, 0] == "c")
                {
                    //SceneManager.LoadScene(2);
                    cv_error3.gameObject.SetActive(true);
                }
                else
                {
                    SceneManager.LoadScene(3);
                }
                
            }
            else
            {
                cv_error.gameObject.SetActive(true);
            }
        }
        else if (col.gameObject.CompareTag("p2e_random"))
        {
            ServerConnector.SendData("3");
            var signal = ServerConnector.ReceiveData();
            if (signal == "y")
            {
                SceneManager.LoadScene(5);
            }
        }
        else if (col.gameObject.CompareTag("p2p"))
        {
            SceneManager.LoadScene(9);
        }
        else if (col.gameObject.CompareTag("e2e"))
        {
            ServerConnector.SendData("8");
            var signal = ServerConnector.ReceiveData();
            if (signal == "y")
            {
                SetRank();
            }
            cv1.enabled = true;
        }
    }

    void DataReceiver(object sender, DataReceivedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Data))
        {
            string rawData = e.Data;
            string[] ds = rawData.Split(' ');
            Width = int.Parse(ds[1]);
            Height = int.Parse(ds[2]);
            var chars = ds[0].ToCharArray();
            Maze = new string[Height, Width];
            var index = 0;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Maze[i, j] = chars[index++].ToString();
                }
            }

            Maze = checkValidation(Maze);
        }
    }
    
    private string[,] checkValidation(string[,] board)
    {
        
        (int, int) startPosition = (0, 0);
        int[] four_ghost = {0,0,0,0};
        int width = board.GetLength(0);
        if (width <= 0)
        {
            //print('width')
            string[,] return_array = {{"c"}};
            return return_array;
        }

        int height = board.GetLength(1);
        for (int i = 0; i < width; i = i + 1)
        {
            for (int j = 0; j < height; j = j + 1)
            {
                if (board[i, j].Equals("4") || board[i, j].Equals("5") ||
                    board[i, j].Equals("6") || board[i, j].Equals("7"))
                {
                    four_ghost[board[i, j][0] - '4'] += 1;
                }
            }
        }

        for (int i = 0; i < 4; i++)
        {
            if (four_ghost[i] != 1)
            {
                string[,] return_array = {{"c"}};
                return return_array;
            }
        }

        for (int i = 0; i < width; i++)
        {
            if ((!board[i, 0].Equals("1")) || (!board[i, height - 1].Equals("1")))
            {
                //print('wall 1')
                string[,] return_array = {{"c"}};
                return return_array;
            }
        }

        for (int j = 0; j < height; j++)
        {
            if ((!board[0, j].Equals("1")) || (!board[width - 1, j].Equals("1")))
            {
                //print('wall 2')
                string[,] return_array = {{"c"}};
                return return_array;
            }
        }

        int numOfPac = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if ((board[i, j].Equals("8")) || (board[i, j].Equals("9")))
                {
                    startPosition = (i, j);
                    numOfPac += 1;
                }
            }
        }

        if (numOfPac != 1)
        {
            //print('wrong pacman')
            string[,] return_array = {{"c"}};
            return return_array;
        }

        int[,] visitedMap = new int[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                visitedMap[i, j] = board[i, j][0] - '0';
            }
        }
        Queue<(int, int)> queue = new Queue<(int, int)>();
        queue.Enqueue(startPosition);
        visitedMap[0, 0] = 1;
        while (queue.Count > 0)
        {
            (int, int) pos;
            pos = queue.Dequeue();
            if ((pos.Item1 > 0) && (visitedMap[pos.Item1 - 1, pos.Item2] != 1))
            {
                visitedMap[pos.Item1 - 1, pos.Item2] = 1;
                queue.Enqueue((pos.Item1 - 1, pos.Item2));
            }
            if ((pos.Item1 > 0) && (visitedMap[pos.Item1, pos.Item2 - 1] != 1))
            {
                visitedMap[pos.Item1, pos.Item2 - 1] = 1;
                queue.Enqueue((pos.Item1, pos.Item2 - 1));
            }
            if ((pos.Item1 < width - 1) && (visitedMap[pos.Item1 + 1, pos.Item2] != 1))
            {
                visitedMap[pos.Item1 + 1, pos.Item2] = 1;
                queue.Enqueue((pos.Item1 + 1, pos.Item2));
            }

            if ((pos.Item1 < height - 1) && (visitedMap[pos.Item1, pos.Item2 + 1] != 1))
            {
                visitedMap[pos.Item1, pos.Item2 + 1] = 1;
                queue.Enqueue((pos.Item1, pos.Item2 + 1));
            }
        }
        bool flag = true;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (visitedMap[i, j] != 1)
                {
                    //print('bu lian tong')
                    flag = false;
                    break;
                }
            }
        }

        if (flag == false)
        {
            board[0, 0] = "c";
        }

        return board;
    }
    
    private void SetRank()
    {
        var rawData = ServerConnector.ReceiveData();
        var seg = rawData.Split(' ');
        var length = seg.Length / 2;
        for (int i = 0; i < length; i++)
        {
            ranks[i].text = seg[2 * i];
            scores[i].text = seg[2 * i + 1];
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.CompareTag("guide"))
        {
            talk = true;
            order = 0;
            content.text = guidetxt[cur];
            dialog.gameObject.SetActive(true);
        }


        if (col.gameObject.CompareTag("beforefog"))
        {
            talk = true;
            order = 4;
            if (cur>7)
            {
                cur = 0;
            }
            content.text = difogtxt[cur];
            dialog.gameObject.SetActive(true);
        }
        
        if (col.gameObject.CompareTag("beforetruefog"))
        {
            talk = true;
            order = 3;
            if (cur>4)
            {
                cur = 0;
            }
            content.text = fogtxt[cur];
            dialog.gameObject.SetActive(true);
        }
        
        if (col.gameObject.CompareTag("beforep2p"))
        {
            talk = true;
            order = 2;
            content.text = p2ptxt[cur];
            dialog.gameObject.SetActive(true);
        }
        
        if (col.gameObject.CompareTag("beforee2e"))
        {
            talk = true;
            order = 2;
            content.text = e2etxt[cur];
            dialog.gameObject.SetActive(true);
        }
        
        if (col.gameObject.CompareTag("beforep2e"))
        {
            talk = true;
            order = 1;
            content.text = p2etxt[cur];
            dialog.gameObject.SetActive(true);
        }
        
        if (col.gameObject.CompareTag("beforep2e_random"))
        {
            talk = true;
            order = 2;
            content.text = randomtxt[cur];
            dialog.gameObject.SetActive(true);
        }
        
        if (col.gameObject.CompareTag("e2e"))
        {
            order = 2;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("guide")
            || col.gameObject.CompareTag("beforetruefog")
            || col.gameObject.CompareTag("beforefog")
            || col.gameObject.CompareTag("beforep2e_random")
            || col.gameObject.CompareTag("beforep2e")
            || col.gameObject.CompareTag("beforee2e")
            ||col.gameObject.CompareTag("beforep2p"))
        {
            talk = false;
            cur = 0;
            dialog.gameObject.SetActive(false);
        }
        if (col.gameObject.CompareTag("e2e"))
        {
            cv1.enabled = false;
        }
    }
}