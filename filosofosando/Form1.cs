//Universidad de Guadalajara 
//Centro Universitario de Ciencias Exactas e Ingenierias
//Daniel Alberto Vazquez Ramirez
//Programa que resuelve el problema de los filosofos comensales

using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;



namespace filosofosando
{
    public partial class Form1 : Form
    {

        //Crear lista enlazada
        ListaEnlazadaCircular lista = new ListaEnlazadaCircular();

        //Clase de tipo filosofo donde tiene ID, nombre (innecesario), estado, tenedor izq, tenedor der.
        public class Filosofo
        {
            //Caracteristicas del filosofo
            public int id { get; set; }
            public string Nombre { get; set; } //Nombre del filosofo
            public int Status { get; set; }
            public bool TenedorIzquierdo { get; set; }
            public bool TenedorDerecho { get; set; }
            public int Comidas { get; set; } //cantidad de comidas hechas

            // Constructor para inicializar las variables del filosofo
            public Filosofo(bool tenedorIzquierdo, bool tenedorDerecho, int status, int comidas, int ide, string nombre)
            {
                TenedorIzquierdo = tenedorIzquierdo; //Inicial todos tienen 1, tienen un asignado
                TenedorDerecho = tenedorDerecho; //Inicial todos tienen false, no tienen asignado
                Status = status;
                Comidas = comidas;
                id = ide;
                Nombre = nombre;
            }
        }
        //Clase nodo que conecta la lista para que sea enlazada
        public class Nodo
        {
            public Filosofo Valor { get; set; }
            public Nodo Siguiente { get; set; }
            public Nodo(Filosofo valor)
            {
                Valor = valor;
                Siguiente = null;
            }
        }
        //Lista circular de filosofos, para que el filosofo 1 pueda revisar lo del filosofo 5 tambien
        public class ListaEnlazadaCircular
        {
            private Nodo cabeza;
            int cantidad = 5; //Cantidad de elementos
            public ListaEnlazadaCircular()
            {
                cabeza = null; //inicia la lista
            }
            public void AgregarAlFinal(Filosofo filo) //Agregar filosofo al ultimo de la lista
            {
                Nodo nuevoNodo = new Nodo(filo);

                if (cabeza == null)
                {
                    cabeza = nuevoNodo;
                    cabeza.Siguiente = cabeza; // Hacemos que el único nodo apunte a sí mismo.
                }
                else
                {
                    Nodo ultimo = ObtenerUltimoNodo();
                    nuevoNodo.Siguiente = cabeza;
                    ultimo.Siguiente = nuevoNodo;
                }
            }
            public Filosofo BuscarElemento(int i) //buscar filosofo por ID
            {
                if (cabeza != null)
                {
                    Nodo actual = cabeza;
                    do
                    {
                        if (actual.Valor.Status == i)
                        {
                            return actual.Valor;
                        }
                        actual = actual.Siguiente;
                    } while (actual != cabeza);
                }

                return null; // Elemento no encontrado
            }
            //Para consultar filosfo mando un filosofo actual y busco antes de dar con el mismo
            public Filosofo ConsultarAnterior(Filosofo f) //Consultar filosofo anterior
            {
                if (cabeza != null)
                {
                    Nodo actual = cabeza;

                    while (actual.Siguiente != null && actual.Siguiente.Valor != f)
                    {
                        actual = actual.Siguiente;
                    }

                    if (actual.Siguiente != null)
                    {
                        return actual.Valor;
                    }
                }

                return null;
            }
            //Consultar al filosofo siguiente
            public Filosofo ConsultarSiguiente(Filosofo f)
            {
                Nodo nodoActual = cabeza;

                while (nodoActual != null)
                {
                    if (nodoActual.Valor == f)
                    {
                        if (nodoActual.Siguiente != null)
                        {
                            return nodoActual.Siguiente.Valor;
                        }
                        else
                        {
                            return null; // Si es la última persona en la lista
                        }
                    }

                    nodoActual = nodoActual.Siguiente;
                }

                return null; // Si la persona no se encuentra en la lista
            }
            //Se obtiene el primer filosofo de la lista
            public Filosofo ObtenerPrimerElemento()
            {
                if (cabeza != null)
                {
                    return cabeza.Valor;
                }
                else
                {
                    throw new InvalidOperationException("La lista está vacía");
                }
            }
            //Actualizar datos del filosofo dentro de la lista enlazada, mando filosofo antes y el despues
            public void ModificarFilosofo(Filosofo filosofoant, Filosofo nuevoFilosofo)
            {
                Nodo nodoActual = cabeza;

                while (nodoActual != null)
                {
                    if (nodoActual.Valor == filosofoant)
                    {
                        nodoActual.Valor = nuevoFilosofo;
                        break; // Una vez que se ha modificado se sale del ciclo
                    }
                    nodoActual = nodoActual.Siguiente;
                }
            }
            //Obtener un filosofo aleatorio para darle tenedores o liberar tenedores, obtener recursos/liberar recursos
            public Filosofo ObtenerElementoAleatorio()
            {
                if (cabeza == null)
                {
                    throw new InvalidOperationException("La lista está vacía");
                }

                Random random = new Random();
                int indiceAleatorio = random.Next(cantidad);
                Nodo nodoActual = cabeza;

                for (int i = 0; i < indiceAleatorio; i++)
                {
                    nodoActual = nodoActual.Siguiente;
                }

                return nodoActual.Valor;
            }
            //Para obtener el ultimo filosofo de la lista
            public Nodo ObtenerUltimoNodo()
            {
                if (cabeza == null)
                {
                    return null;
                }

                Nodo actual = cabeza;
                while (actual.Siguiente != cabeza)
                {
                    actual = actual.Siguiente;
                }

                return actual;
            }

        }
        //declaro 2 timers para manejar los dos procesos de obtener tenedores y liberar tenedores 
        private System.Windows.Forms.Timer miTimer = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer miTimer2 = new System.Windows.Forms.Timer();
        public Form1()
        {
            InitializeComponent();
            miTimer.Interval = 500; // Cada 2 segundos busca asignar a alguien a comer
            miTimer.Tick += new EventHandler(MiTimer_Tick);

            miTimer2.Interval = 700; // Cada 3 segundos busca liberar tenedores
            miTimer2.Tick += new EventHandler(MiTimer2_Tick);
        }
        private void MiTimer_Tick(object sender, EventArgs e)
        {
            Filosofo f = lista.ObtenerElementoAleatorio(); //Se obtiene un filosofo random
            obtenerTenedor(f); //Se busca ponerlo a comer
        }
        private void MiTimer2_Tick(object sender, EventArgs e)
        {
            //Filosofo f = lista.ObtenerElementoAleatorio();
            Filosofo f = lista.BuscarElemento(1); //Se busca el primer filosofo que este comiendo para retirar tenedores
            liberarTenedor(f);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            crearList(); //Crear lisra al abrir el programa y mostrar en el datagrid
        }
        //Esta funcion cambia el color de los textbox para mostrar graficamente el funcionamiento de comer, pensar y esperar
        public void cambiarColor(Filosofo f)
        {
            int id = f.id;
            int status = f.Status;

            if (status == 0) //Espera ----- amarillo
            {
                switch (id)
                {
                    case 0: textBox0.BackColor = Color.Yellow; break;
                    case 1: textBox1.BackColor = Color.Yellow; break;
                    case 2: textBox2.BackColor = Color.Yellow; break;
                    case 3: textBox3.BackColor = Color.Yellow; break;
                    case 4: textBox4.BackColor = Color.Yellow; break;
                    default:
                        break;
                }
            }
            if (status == 1) //Comiendo ------ verde
            {
                switch (id)
                {
                    case 0: textBox0.BackColor = Color.Green; break;
                    case 1: textBox1.BackColor = Color.Green; break;
                    case 2: textBox2.BackColor = Color.Green; break;
                    case 3: textBox3.BackColor = Color.Green; break;
                    case 4: textBox4.BackColor = Color.Green; break;
                    default:
                        break;
                }
            }
            if (status == 2) //Pensando ------- rojo
            {
                switch (id)
                {
                    case 0: textBox0.BackColor = Color.Red; break;
                    case 1: textBox1.BackColor = Color.Red; break;
                    case 2: textBox2.BackColor = Color.Red; break;
                    case 3: textBox3.BackColor = Color.Red; break;
                    case 4: textBox4.BackColor = Color.Red; break;
                    default:
                        break;
                }
            }
        }

        //Crear lista normal para mostrar en datagrid
        public void crearList()
        {
            //Asignar los valores a los filosofillos  
            //tenedor izquierdo, tenedor derecho, status, comida, id,nombre
            Filosofo nuevoFilosofo0 = new Filosofo(true, false, 0, 0, 0, "Socrates"); //0
            Filosofo nuevoFilosofo1 = new Filosofo(true, false, 0, 0, 1, "Platon"); //1
            Filosofo nuevoFilosofo2 = new Filosofo(true, false, 0, 0, 2, "Aristoteles"); //2
            Filosofo nuevoFilosofo3 = new Filosofo(true, false, 0, 0, 3, "Kant"); //3
            Filosofo nuevoFilosofo4 = new Filosofo(true, false, 0, 0, 4, "Nietzsche"); //
            //status 0 = esperando con un tenedor
            //status 1 = comiendo con 2 tenedores
            //status 2 = pensando sin tenedores

            //Se agregan filosofos a la lista
            lista.AgregarAlFinal(nuevoFilosofo0);
            lista.AgregarAlFinal(nuevoFilosofo1);
            lista.AgregarAlFinal(nuevoFilosofo2);
            lista.AgregarAlFinal(nuevoFilosofo3);
            lista.AgregarAlFinal(nuevoFilosofo4);

            //Mostrar informacion de los filosofos
            List<Filosofo> listaFilos = new List<Filosofo>();
            Nodo actual = lista.ObtenerUltimoNodo().Siguiente;

            do
            {
                listaFilos.Add(actual.Valor);
                actual = actual.Siguiente;
            } while (actual != lista.ObtenerUltimoNodo().Siguiente);

            dataGridFilosofos.DataSource = null;
            dataGridFilosofos.DataSource = new BindingList<Filosofo>(listaFilos);

        }//Crearlist
        //Comenzar lista
        
        public void obtenerTenedor(Filosofo f) //Funcion para tomar tenedores y poner a comer
        {
            Filosofo sig = lista.ConsultarAnterior(f);
            Filosofo g = f; //copiar filosofo que recibo
            Filosofo sig2 = sig; //copiar filosofo siguiente

            if (f.TenedorIzquierdo == true && f.TenedorDerecho == false && f.Status == 0) //Revisar si el filosofo actual tiene el tenedor izquierdo disponible
            {
                if (sig.TenedorIzquierdo == true && sig.Status == 0 && sig.TenedorDerecho == false) //Revisar si el filsofo siguiente tiene el tenedor izquierdo disponible, y que no este comiendo
                {
                    g.TenedorIzquierdo = true;
                    g.TenedorDerecho = true;
                    g.Status = 1; //se pone a comer
                    g.Comidas = g.Comidas + 1; //Se incrementan las comidas

                    sig2.TenedorIzquierdo = false;
                    sig2.Status = 2; //Se pone a pensar
                    //Cambiar color de textbox
                    cambiarColor(g);
                    cambiarColor(sig2);
                    //Actualizar lista y datagrids
                    lista.ModificarFilosofo(f, g);
                    lista.ModificarFilosofo(sig, sig2);

                    ActualizarDataGrid();
                }
            }

        }
        //Liberar tenedores y recursos
        public void liberarTenedor(Filosofo f)
        {
            Filosofo sig = lista.ConsultarAnterior(f);//consultar el filosofo anterior
            Filosofo g = f; //copiar filosofo que recibo
            Filosofo sig2 = sig; //copiar filosofo siguiente

            //revisar si se puede liberar tenedores
            if (f.TenedorIzquierdo == true)
            {
                if (f.TenedorDerecho == true && f.Status == 1)
                {
                    //asignar nuevos valores
                    g.TenedorDerecho = false;
                    g.Status = 0;
                    g.TenedorIzquierdo = true;

                    sig2.TenedorIzquierdo = true;
                    sig2.Status = 0;
                    //cambiar color de textbox
                    cambiarColor(g);
                    cambiarColor(sig2);
                    //Modificar lista y actualizar datagrid
                    lista.ModificarFilosofo(f, g);
                    lista.ModificarFilosofo(sig, sig2);

                    ActualizarDataGrid();
                }
            }

        }

        //comenzar el programa
        public void comenzar()
        {
            miTimer.Start(); //comienza el timer de obtener tenedores
            miTimer2.Start(); //Comienza el timer de liberar tenedores
        }//comenzar funcion del boton

        public void ActualizarDataGrid() //funcion para actualizar datagrid
        {

            List<Filosofo> listaFilos = new List<Filosofo>();
            Nodo actual = lista.ObtenerUltimoNodo().Siguiente;
            //hago una lista enlazada a lista simple
            do
            {
                listaFilos.Add(actual.Valor);
                actual = actual.Siguiente;
            } while (actual != lista.ObtenerUltimoNodo().Siguiente);
            //El datagrid solo muestra listas simples
            dataGridFilosofos.DataSource = null;
            dataGridFilosofos.DataSource = new BindingList<Filosofo>(listaFilos);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            comenzar();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            miTimer.Stop(); //Detengo los procesos
            miTimer2.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Environment.Exit(0); //Finalizar programa
        }
    }//partial form
} //Clase filosofando