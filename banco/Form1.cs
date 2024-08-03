/* DANIEL ALBERTO VAZQUEZ RAMIREZ
ALGORITMO DEL BANQUERO
SSP USO ADAPTACION EXPLOTACION DE SISTEMAS OPERAIVOS*/


using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace banco
{
    public partial class Form1 : Form
    {

        int saldo = 50;  //Saldo total del banco
        int limite = 15;  //Cual es limite menor q pueden aceptar
        bool status = true;  //status del banco

        List<int> cuentas = new List<int>(); //cuentas de los clientes 

        private System.Windows.Forms.Timer miTimer = new System.Windows.Forms.Timer(); //Controlar la peticion

        public void comenzar()
        {
            miTimer.Start();
            Random random = new Random();
            int opc = random.Next(2); //Opcion de obtener o regresar dinero
            int n = random.Next(0, 5); //numero de cliente

            if (opc == 0) //Pedir dinero
            {
                getMoney(n);
            }
            if (opc == 1) //Devolver dinero
            {
                backMoney(n);
            }

            actualizarsaldos();
        }



        public void actualizarsaldos()
        {
            //Actualiza informacion de los TextBox
            txt1.Text = cuentas[0].ToString();
            txt2.Text = cuentas[1].ToString();
            txt3.Text = cuentas[2].ToString();
            txt4.Text = cuentas[3].ToString();
            txt5.Text = cuentas[4].ToString();
            //Mostrar el saldo en el textbox
            txtSaldo.Text = saldo.ToString();

            int a = 0;
            //Para mostrar el dinero en circulacion
            for (int i = 0; i < 5; i++)
            {
                a = a + cuentas[i];
            }
            txtCircula.Text = a.ToString();

            //Mostrar el status en el form y el color del status
            if (saldo == limite)
            {
                txtStatus.Text = "Bloqueado";
                txtStatus.ForeColor = Color.Red;
            }
            if (saldo > limite)
            {
                txtStatus.Text = "Disponible";
                txtStatus.ForeColor = Color.Green;
            }
        }

        public void getMoney(int e) //Obtener dinero del banco 
        {
            Random random = new Random();

            if (status == true) //El banco puede otorgar credito
            {
                int c = random.Next(1, 11); //Cantidad random 
                //Verificar si el banco me puede prestar la solicitado
                if (limite < (saldo - c))
                {
                    cuentas[e] = cuentas[e] + c; //incrementa el saldo en la cuenta individual
                    saldo = saldo - c;
                }
            }
        }

        public void backMoney(int e)//regresar la inversion
        {
            Random random = new Random();
            //Verificar si el cliente tiene dinero para devolver despues de su inversion
            if (cuentas[e] != 0)
            {
                int c = random.Next(1, 11);
                if (c <= cuentas[e])//Verificar si el cliente tiene mas de lo que quiere devolver
                {
                    cuentas[e] = cuentas[e] - c;
                    saldo = saldo + c;
                    //Se actualizan saldos
                    if (status == false)
                    {
                        status = true;
                    }
                }
            }

        }
        public Form1()
        {
            InitializeComponent();

            miTimer.Interval = 50; //Cada cuanto tiempo un C o P quiere ejecutarse
            miTimer.Tick += new EventHandler(MiTimer_Tick);

            //Asignar a 5 clientes inicialmente el valor de $0
            for (int i = 0; i < 5; i++)
            {
                cuentas.Add(0);
            }

            textBox4.Text = saldo.ToString();

            actualizarsaldos();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //iniciar las pedideras o devueltas de dinero
            comenzar();
        }

        private void MiTimer_Tick(object sender, EventArgs e)
        {
            comenzar();
        }
    }
}