using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.IO;

namespace SharpEngine
{
    class Game
    {

        static Thread tr;
        static Scene CurrentScene = null;
        static List<Scene> scene_list = new List<Scene>();
        public static int cur_sc_id = 0;

        
        private Game() { }
    

        public static void Main(string[] args)
        {
            AddScene(new Scene_frst());
            AddScene(new Scene_sec());
            AddScene(new Scene_thrd());
            Initialize(0);
        }

        private static void Initialize(int pos)
        {
            LoadScene(0);
            if (CurrentScene == null) return;
           
            tr = new Thread(GetInput);
            tr.Priority = ThreadPriority.Lowest;
            tr.Start();
            FrameNumerator();
        }
        public static void FrameNumerator()
        {
            while (true)
            {
                CurrentScene.Update();
            }

        }
        public static void Debug(string Message)
        {
            ConsoleGraphics.ResetCursor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(Message);
        }
        static void GetInput()
        {
            string val = "";
            while (true)
            {
                Input.ReadKey(Console.ReadKey().KeyChar.ToString());

            }

        }


        public static void AddScene(Scene s)
        {
            scene_list.Add(s);
        }
        public static void LoadScene(int id)
        {
            if (id < 0 || id > scene_list.Count - 1) return;
            cur_sc_id = id;
           
            CurrentScene = scene_list[id];
            CurrentScene.Start();
        }

    }


    public class Shape
    {
        public int height = 0;
        public int width = 0;
        ConsoleColor c = ConsoleColor.Green;
        public ConsoleColor[,] colors;


        public Shape(int height, int width, ConsoleColor c)
        {
            this.height = height;
            this.width = width;
            this.c = c;
            Create();
        }

        public Shape(int height, int width)
        {
            this.height = height;
            this.width = width;
            Create();
        }

        protected virtual void Create()
        {
            colors = new ConsoleColor[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {

                    colors[x, y] = c;

                }
            }
        }

        public ConsolePicture BakeConsolePicture()
        {
            ConsolePicture c = new ConsolePicture(this, 0, 0);
            return c;
        }

    }


    class ConsoleGraphics
    {
        public static int BLACK = 0;

        public static int WHITE = 755;
        private static int c_x = 0;
        private static int c_y = 0;
        private static Vector2 drawPos = new Vector2(0, 0);
        public static ConsoleColor Skybox = ConsoleColor.Black;


        public static ConsoleColor ColorToConsole(int red, int green, int blue)
        {

            if ((255 - red) > 0 && (255 - red) < (255 / 2))
            {
                red = 255;
            }
            else
            {
                red = 0;
            }

            if ((255 - blue) > 0 && (255 - blue) < (255 / 2))
            {
                blue = 255;
            }
            else
            {
                blue = 0;
            }

            if ((255 - green) > 0 && (255 - green) < (255 / 2))
            {
                green = 255;
            }
            else
            {
                green = 0;
            }


            if (red + green + blue == WHITE)
            {
                return ConsoleColor.White;
            }


            if (red + green + blue == BLACK)
                return ConsoleColor.Black;

            if (red + green + blue == 510)
            {
                if (red + green == 510)
                    return ConsoleColor.Yellow;

                if (red + blue == 510)
                    return ConsoleColor.Magenta;

                if (green + blue == 510)
                    return ConsoleColor.Cyan;

            }

            if (red + green + blue == 255)
            {
                if (red == 255)
                    return ConsoleColor.Red;

                if (green == 255)
                    return ConsoleColor.Green;
                if (blue == 255)
                    return ConsoleColor.Blue;
            }


            return ConsoleColor.White;

        }

        public static string HexPicture(Bitmap b)
        {
            int height = b.Height;
            int width = b.Width;
            string quote = "\"";
            string res = "[";
            for (int y = 0; y < height; y++)
            {
                res += "[";
                for (int x = 0; x < width; x++)
                {
                    Color c = b.GetPixel(x, y);
                    string c_s = HexConverter(c);

                    res += quote + c_s + quote +",";
                }
                res += "],";
            }

            res += "];";
            string textFromFile = "";
            using (FileStream fstream = File.OpenRead(@"C:\Users\Artem\renderer\SharpEngine\SharpEngine\SharpEngine\bin\Debug\index.html"))
            {
                // преобразуем строку в байты
                byte[] array = new byte[fstream.Length];
                // считываем данные
                fstream.Read(array, 0, array.Length);
                // декодируем байты в строку
                textFromFile = System.Text.Encoding.Default.GetString(array);
            }


            using (FileStream fstream = new FileStream(@"C:\Users\Artem\renderer\SharpEngine\SharpEngine\SharpEngine\bin\Debug\test.html", FileMode.OpenOrCreate))
            {
                // преобразуем строку в байты
                textFromFile.Replace("#matrix",res);
              
                byte[] array = System.Text.Encoding.Default.GetBytes(textFromFile);
                // запись массива байтов в файл
                fstream.Write(array, 0, array.Length);
                Console.WriteLine("Текст записан в файл");
            }


            return res;
        }

        private static String HexConverter(Color c)
        {
            return  "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        public static void Draw(ConsolePicture picture, bool HasBounds)
        {

            Console.BackgroundColor = ConsoleGraphics.Skybox;
            ConsoleColor cc = ConsoleColor.Black;
            int a_s = 0;
            int a_e = 0;

            if (picture.Anim_pic)
            {

                a_s = picture.offsetX * picture.CurrentFrame;
                a_e = a_s;
                if (a_s >= picture.offsetX * picture.Frames)
                {

                    a_s = 0;
                    a_e = 0;
                    picture.CurrentFrame = 0;

                }
            }
            else
            {
                a_s = picture.width;
                a_e = 0;
            }

            for (int y = 0; y < picture.height; y++)
            {


                for (int x = a_e; x < a_s + picture.offsetX; x++)
                {


                    cc = picture.colors[x, y];

                    Console.ForegroundColor = cc;
                    Console.BackgroundColor = cc;
                    if (!HasBounds)
                        Console.Write("0");
                    else
                        Console.Write("1");

                }


                SetDrawPosition(drawPos.x, drawPos.y + 1);
            }
            if (picture.Anim_pic)
                picture.CurrentFrame++;

        }

        public static void BakeSky(int width, int height)
        {
            for (int y = 0; y < height; y++)
            {


                for (int x = 0; x < width; x++)
                {


                    Console.ForegroundColor = Skybox;
                    Console.BackgroundColor = Skybox;
                    Console.Write(".");

                }



            }

        }


        public static void SetCursor(int x, int y)
        {
            c_x = x;
            c_y = y;
        }

        public static void SetDrawPosition(int x, int y)
        {
            drawPos = new Vector2(x, y);
            Console.SetCursorPosition(drawPos.x, drawPos.y);
        }

        public static void ResetCursor()
        {
            Console.SetCursorPosition(c_x, c_y);

        }




    }


    public class ConsolePicture
    {
        public int width = 0;
        public int height = 0;
        public Bitmap b;
        public bool Anim_pic = false;
        public ConsoleColor[,] colors;
        public int Frames = 0;
        public int offsetX = 0;
        public int offsetY = 0;
        public int CurrentFrame = 0;

        public ConsolePicture(Bitmap b, int Frames, int offsetY)
        {
            this.width = b.Width;
            this.height = b.Height;
            this.offsetY = offsetY;
            this.b = b;
            this.Frames = Frames;
            Create();
        }

        public ConsolePicture(Shape s, int Frames, int offsetY)
        {
            this.width = s.width;
            this.height = s.height;
            this.offsetY = offsetY;
            this.Frames = Frames;
            colors = s.colors;

        }


        private void Create()
        {
            colors = new ConsoleColor[width, height];
            height = height - offsetY;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    ConsoleColor cc = ConsoleColor.Black;
                    Color c = b.GetPixel(x, y);

                    cc = ConsoleGraphics.ColorToConsole(c.R, c.G, c.B);

                    colors[x, y] = cc;


                }
            }

            if (Frames != 0)
                offsetX = width / Frames;


        }

    }


    class Input
    {
        public static int Hor = 0;


        public static bool ReadKey(string KeyValue)
        {
            bool res = false;

            switch (KeyValue)
            {
                case "2":
                    Hor = 1;
                    res = true;
                    Console.Beep(523, 100);
                    break;

                case "1":
                    Hor = -1;
                    res = true;
                    Console.Beep(523, 100);
                    break;

                case "":
                    Hor = 0;
                    res = false;

                    break;
            }

            return res;

        }

        public static int GetHor()
        {
            int res = Hor;
            Hor = 0;
            return res;

        }
    }

    abstract class ConsoleBehaviour
    {

        public bool Enabled = false;
        public ConsolePicture material;
        public Vector2 position = new Vector2(0, 0);
        public Collider _C = null;
        public RigidBody rigidBody = null;
        public int index = 0;
        public ConsoleBehaviour()
        {
            Enabled = true;
        }


        public abstract void Start();
        public abstract void Update(); 

        public void SetActive(bool value)
        {
            Enabled = value;
        }


        public void Destroy()
        {
           

        }

    }

    class Vector2
    {
        public int x = 0;
        public int y = 0;

        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;

        }


    }

    class Collider
    {
        public int width = 0;

        public int height = 0;
        //	private ConsolePicture c = null;

        public Collider(ref ConsolePicture c)
        {

            width = c.width;
            height = c.height;

            Init(ref c);
            //Debug(width.ToString());
        }

        protected void Init(ref ConsolePicture c)
        {
            for (int i = 0; i < width; i++)
            {
                c.colors[i, height - 1] = ConsoleColor.Green;

                c.colors[i, 0] = ConsoleColor.Black;
            }

            for (int i = 0; i < height; i++)
            {
                c.colors[width - 1, i] = ConsoleColor.Black;

                c.colors[0, i] = ConsoleColor.Black;
            }
        }

        public void OnCollissionEnter()
        {
        }

    }

    class Physics
    {
        public static int _GRAVITY = 1;


    }

    class RigidBody
    {
        private ConsoleBehaviour _cb = null;

        public RigidBody(ref ConsoleBehaviour _cb)
        {
            this._cb = _cb;
        }

        public void GravityOn()
        {
            _cb.position.y += Physics._GRAVITY;
        }

    }



    //Game Objects

    class Person : ConsoleBehaviour
    {
        int h = 0;
        int w = 0;
        Bitmap bt;
        ConsolePicture hero;
        int Frames = 1;
        string path = "Spruce.png";
        int speed = 3;

        public override void Start()
        {

            Init();
            position = new Vector2(60, 10);

        }

        public override void Update()
        {





        }



        private void Init()
        {


            bt = (Bitmap)Image.FromFile(path);
     
            //  h = bt.Height;
            //  w = bt.Width/Frames;	
            hero = new ConsolePicture(bt, 0, 0);
            //_C = new Collider(ref hero);
            //ConsoleBehaviour _cache = this;
            // rigidBody = new RigidBody(ref _cache);
            hero.Anim_pic = false;
            material = hero;
            //hero.Anim_pic = true;


        }


    }


    class House : ConsoleBehaviour
    {
        int h = 0;
        int w = 0;
        Bitmap bt;
        ConsolePicture hero;
        int Frames = 1;
        string path = "house.jpg";
        int speed = 3;

        public override void Start()
        {

            Init();
            position = new Vector2(10, 10);

        }

        public override void Update()
        {





        }



        private void Init()
        {


            bt = (Bitmap)Image.FromFile(path);
            hero = new ConsolePicture(bt, 0, 0);
            hero.Anim_pic = false;
            material = hero;


        }


    }


    class Tree : ConsoleBehaviour
    {

        Bitmap bt;
        ConsolePicture hero;
        int Frames = 1;
        string path = "santa2.jpg";
        int delta = -1;
        int speed = 3;
        

        public override void Start()
        {

            Init();
            position = new Vector2(0, 10);
          

        }

        public override void Update()
        {
            position.x = position.x + Input.GetHor() * speed;
            if (position.x > 45)
            {
                position.x = 0;
                ConsoleGraphics.BakeSky(75, 80);
                Console.Beep(783, 200);
                Game.LoadScene(Game.cur_sc_id + 1);
            }
            if (position.x < 0)
            {
                position.x = 50;
                ConsoleGraphics.BakeSky(75, 80);
                Console.Beep(783, 200);
            }


        }

        private void Init()
        {
            bt = (Bitmap)Image.FromFile(path);
            hero = new ConsolePicture(bt, 0, 0);
            material = hero;
            ConsoleGraphics.HexPicture(bt);
            //hero.Anim_pic = true;


        }




    }


    public class ConsoleSoftwareRendering
    {
        static int h = 0;
        static int w = 0;
        static Bitmap Render3D;
        static Shape wall;
        static string path = "Lab_1_2.png";
        static ConsolePicture material;


        private ConsoleSoftwareRendering()
        {


        }

        public static void Render()
        {
            Render3D = (Bitmap)Image.FromFile(path);
            h = Render3D.Height;
            w = Render3D.Width;
            material = new ConsolePicture(Render3D, 0, 15);
            int a = 0;


            for (int i = 0; i < w - 1; i++)
            {
                a = i;
                int sh = 15;
                for (int k = 0; k < h - 1; k++)
                {

                    if (sh <= 0) sh = 1;
                    if (material.colors[i, k] == ConsoleColor.Green)
                    {

                        //ConsoleGraphics.Draw(material,true);
                        wall = new Shape(sh, 2);
                        ConsoleGraphics.SetDrawPosition(i, sh);
                        ConsoleGraphics.Draw(wall.BakeConsolePicture(), true);
                        break;
                    }
                    else
                    {
                        wall = new Shape(sh, 2, ConsoleColor.White);
                        ConsoleGraphics.SetDrawPosition(i, sh);
                        ConsoleGraphics.Draw(wall.BakeConsolePicture(), true);
                    }
                    sh--;
                }
            }

        }

    }

    class Scene_frst : Scene
    {

        Tree t = new Tree();


        protected override void FrameEvent()
        {
            
        }

        protected override void InitEvent()
        {
            AddToObjects(t);
        }

    }


    class Scene_sec : Scene
    {
        Person p = new Person();
        Tree t = new Tree();
     

        protected override void FrameEvent()
        {

        }

        protected override void InitEvent()
        {
            AddToObjects(p);
            AddToObjects(t);
        }

    }

    class Scene_thrd : Scene
    {

        House h = new House();
        Tree t = new Tree();
      
        protected override void FrameEvent()
        {

        }

        protected override void InitEvent()
        {
            AddToObjects(h);
            AddToObjects(t);
        }

    }



    class Scene : ConsoleBehaviour
    {
        List<ConsoleBehaviour> MainList = new List<ConsoleBehaviour>();
        bool NeedFree = false;
        int FreeIndex = 0;

        public override void Start()
        {
            InitEvent();
            int size = MainList.Count;
            if (size == 0) return;
            for (int i = 0; i < size; i++)
            {
                MainList[i].Start();
            }

            if (MainList.Count > size)
            {
               

            }
            

        }

        public override void Update()
        {
            if (MainList.Count == 0) return;
            if (NeedFree)
            {
                NeedFree = false;
                MainList[FreeIndex].Enabled = false;
                MainList.RemoveAt(FreeIndex);

            }

            for (int i = 0; i < MainList.Count; i++)
            {
                MainList[i].Update();
                if (MainList[i].rigidBody != null)
                    MainList[i].rigidBody.GravityOn();

                if (MainList[i].material != null)
                {
                    ConsoleGraphics.SetDrawPosition(MainList[i].position.x, MainList[i].position.y);
                    ConsoleGraphics.Draw(MainList[i].material, MainList[i]._C != null);
                }

            }

            ConsoleGraphics.ResetCursor();
            FrameEvent();

        }

        protected virtual void InitEvent() { }
        protected virtual void FrameEvent() { }

        public int AddToObjects(ConsoleBehaviour cb)
        {
            MainList.Add(cb);

            return (MainList.Count - 1);

        }

        public void DeleteObjects(int index)
        {
            NeedFree = true;
            FreeIndex = index;

        }



    }







}

