/*
 Daniel Alberto Vazquez Ramirez
 Problema de concurrencia
 Seminario de Solucion de Problemas de Explotacion
 adaptacion y uso de Sistemas operativos
    act 11
 */
using static carros.Form1;

namespace carros
{
    public partial class Form1 : Form
    {
        //Timers para coordinar el semaforo
        private System.Windows.Forms.Timer miTimer = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer miTimer2 = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer miTimer3 = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer miTimer4 = new System.Windows.Forms.Timer();
        //Dos listas de trafico para camino en norte y sur
        Queue<int> colaDeAutosN = new Queue<int>();
        Queue<int> colaDeAutosS = new Queue<int>();

        public Form1()
        {

            InitializeComponent();
            miTimer.Interval = 6000; // Cada n segundos cambia de color
            miTimer.Tick += new EventHandler(MiTimer_Tick);

            miTimer2.Interval = 1500; // Para formarse
            miTimer2.Tick += new EventHandler(MiTimer2_Tick);

            miTimer3.Interval = 700; // Para pasar
            miTimer3.Tick += new EventHandler(MiTimer3_Tick);

            miTimer4.Interval = 1000; // Para liberar puente
            miTimer4.Tick += new EventHandler(MiTimer4_Tick);

            miTimer.Start();
            trafico();

        }
        private void MiTimer_Tick(object sender, EventArgs e)
        {
            cambiosemaforo();

        }
        private void MiTimer2_Tick(object sender, EventArgs e)
        {
            formarse();
        }
        private void MiTimer3_Tick(object sender, EventArgs e)
        {
            pasa();
        }
        private void MiTimer4_Tick(object sender, EventArgs e)
        {
            libera();
        }

        public void cambiosemaforo()
        {
            if (semaforo == 1) //En verde pasa a rojo
            {
                textBox2.BackColor = Color.Red;
                textBox4.BackColor = Color.Black;
                semaforo = 0;
                return;
            }
            if (semaforo == 0)//En rojo pasa a verde
            {
                textBox2.BackColor = Color.Black;
                textBox4.BackColor = Color.Green;
                semaforo = 1;
                return;
            }
        }

        int semaforo = 1; //rojo

        private void button1_Click(object sender, EventArgs e)
        {
            iniciar();
        }

        public void iniciar()
        {
            miTimer2.Start();
            miTimer3.Start();
            miTimer4.Start();
            formarse();
            pasa();
        }
        //Crear listas con trafico de autos de diferentes lugares, de tipo entero
        //para emular los colores
        public void trafico()
        {
            Random rand = new Random();
            int color = rand.Next(1, 5);
            int color2 = rand.Next(1, 5);
            for (int i = 0; i < 16; i++)
            {
                color = rand.Next(1, 5);
                colaDeAutosN.Enqueue(color);
                color2 = rand.Next(1, 5);
                colaDeAutosS.Enqueue(color2);
            }
        }

        int n = 0; //posicion de entrar a calle falsa
        int s = 0; //posicionn de entrar a calle de sur falsa
        public void formarse()
        {
            //Elegir un auto desencolar y poner al puente
            Random rand = new Random();

            if (colaDeAutosN != null && colaDeAutosS != null)
            {
                int d = rand.Next(2);
                if (d == 0 && n == 0) //Auto de Norte a Sur
                {
                    n = 1;
                    int c = colaDeAutosN.Peek();
                    switch (c)
                    {
                        case 1: txtN.BackColor = Color.Red; break;
                        case 2: txtN.BackColor = Color.Green; break;
                        case 3: txtN.BackColor = Color.Blue; break;
                        case 4: txtN.BackColor = Color.Magenta; break;
                        default: break;
                    }
                }
                if (d == 1 && s == 0)//Auto de Sur a Norte
                {
                    s = 1;
                    int c = colaDeAutosS.Peek();
                    switch (c)
                    {
                        case 1: txtS1.BackColor = Color.Red; break;
                        case 2: txtS1.BackColor = Color.Green; break;
                        case 3: txtS1.BackColor = Color.Blue; break;
                        case 4: txtS1.BackColor = Color.Magenta; break;
                        default: break;
                    }
                }

            }
        }

        public void pasa()
        {
            //Semaforo en verde el auto de sur a norte puede pasar por puente
            if (semaforo == 1 && s == 1)
            {
                txtS1.BackColor = Color.White;
                int c = colaDeAutosS.Peek();
                colorp = c;
                //se actualiza color de textbox intermedio en puente
                switch (c)
                {
                    case 1: txtpaso.BackColor = System.Drawing.Color.Red; break;
                    case 2: txtpaso.BackColor = Color.Green; break;
                    case 3: txtpaso.BackColor = Color.Blue; break;
                    case 4: txtpaso.BackColor = Color.Magenta; break;
                    default: break;
                }
                colaDeAutosS.Dequeue(); //se desencola auto
                s = 0;
                p = 0;
                //se actualizan valores de espera y p que indica el color en el puente del auto
            }


            //Semaforo en rojo el auto de norte a sur puede pasar por puente
            if (semaforo == 0 && n == 1)
            {
                txtN.BackColor = Color.White;
                int c = colaDeAutosN.Peek();
                colorp = c;
                switch (c)
                {
                    case 1: txtpaso.BackColor = System.Drawing.Color.Red; break;
                    case 2: txtpaso.BackColor = Color.Green; break;
                    case 3: txtpaso.BackColor = Color.Blue; break;
                    case 4: txtpaso.BackColor = Color.Magenta; break;
                    default: break;
                }
                colaDeAutosN.Dequeue(); //se desencola auto
                n = 0;
                p = 1;
                //se actualizan valores de espera y p que indica el color en el puente del auto
            }

        }
        int p = 0;
        int colorp = 0;
        public void libera()
        {
            txtpaso.BackColor = Color.White;
            if (p == 0) // si viene de sur a norte, cambiar el color del textbox y simular que avanza
            {
                switch (colorp)
                {
                    case 1: txtS2.BackColor = System.Drawing.Color.Red; break;
                    case 2: txtS2.BackColor = Color.Green; break;
                    case 3: txtS2.BackColor = Color.Blue; break;
                    case 4: txtS2.BackColor = Color.Magenta; break;
                    default: break;
                }
            }
            if (p == 1) //si viene de norte a sur
            {
                switch (colorp)
                {
                    case 1: txtN2.BackColor = System.Drawing.Color.Red; break;
                    case 2: txtN2.BackColor = Color.Green; break;
                    case 3: txtN2.BackColor = Color.Blue; break;
                    case 4: txtN2.BackColor = Color.Magenta; break;
                    default: break;
                }
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

    }
}