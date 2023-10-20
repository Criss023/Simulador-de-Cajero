using System;
using System.Collections.Generic;
using System.IO;
class Program
{
    static void Main()
    {
        string ruta = "C:\\Users\\crist\\source\\repos\\SimuladorCajero\\archivos";
        string saldoFilePath = Path.Combine(ruta, "saldo.txt");
        string cuentaFilePath = Path.Combine(ruta, "cuenta.txt");
        string contraFilePath = Path.Combine(ruta, "contra.txt");

        double saldo = LeerSaldo(saldoFilePath);
        int cuenta = LeerCuenta(cuentaFilePath);
        int contra = LeerContra(contraFilePath);

        Cajero cajeroUno = new Cajero(saldo, cuenta, contra);
        Datos DatosUsuario = new Datos();

        int cuentaUsuario;
        bool cuentaCorrecta = false;
        do
        {
            cuentaUsuario = cajeroUno.SolicitarCuenta();
            cuentaCorrecta = DatosUsuario.ValidarCuenta(cuentaUsuario, cajeroUno.ObtenerCuenta());
            if (!cuentaCorrecta)
            {
                Console.WriteLine("Cuenta incorrecta. Vuelva a intentar.");
            }
        } while (!cuentaCorrecta);

        int contraUsuario;
        bool contraCorrecta = false;
        do
        {
            contraUsuario = cajeroUno.SolicitarContra();
            contraCorrecta = DatosUsuario.ValidarContra(contraUsuario, cajeroUno.ObtenerContra());
            if (!contraCorrecta)
            {
                Console.WriteLine("Contraseña incorrecta. Vuelva a intentar.");
            }
        }
        while (!contraCorrecta);

        cajeroUno.DesplegarMenu();
    }
    

 //lectura de archivos
    static double LeerSaldo(string filePath)
    {
        if (File.Exists(filePath))
        {
            string contenido = File.ReadAllText(filePath);
            if (double.TryParse(contenido, out double saldo))
            {
                return saldo;
            }
        }
        return 0.0;
    }

    static int LeerCuenta(string filePath)
    {
        if (File.Exists(filePath))
        {
            string contenido = File.ReadAllText(filePath);
            if (int.TryParse(contenido, out int cuenta))
            {
                return cuenta;
            }
        }
        return 0;
    }

    static int LeerContra(string filePath)
    {
        if (File.Exists(filePath))
        {
            string contenido = File.ReadAllText(filePath);
            if (int.TryParse(contenido, out int contra))
            {
                return contra;
            }
        }
        return 0;
    }
}

 //validacion de datos
public class Datos
{
    public bool ValidarCuenta(int cuenta, int cuentaCajero)
    {
        if (cuentaCajero == cuenta)
        {
            Console.WriteLine("Cuenta correcta");
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool ValidarContra(int contra, int contraCajero)
    {
        if (contraCajero == contra)
        {
            Console.WriteLine("Contraseña correcta. Bienvenido");
            return true;
        }
        else
        {
            return false;
        }
    }
}

//almacenamiento de datos
public class Cajero
{
    private List<Transaccion> Transacciones = new List<Transaccion>();
    private double saldo;
    private int cuenta;
    private int contra;
    public Cajero(double saldoInicial, int cuentaInicial, int contraInicial)
    {
        saldo = saldoInicial;
        cuenta = cuentaInicial;
        contra = contraInicial;
    }

 //solicitud de datos
    public int ObtenerCuenta()
    {
        return cuenta;
    }
    public int ObtenerContra()
    {
        return contra;
    }
    public int SolicitarCuenta()
    {
        Console.WriteLine("<<<Bienvenido a su Cajero Automatico>>>");
        Console.WriteLine("Por favor ingrese el numero de su cuenta: ");
        int cuenta = int.Parse(Console.ReadLine());
        return cuenta;
    }
    public int SolicitarContra()
    {
        Console.WriteLine("Ingrese su contraseña: ");
        int contra = int.Parse(Console.ReadLine());
        return contra;
    }

 //Menu
    public void DesplegarMenu()
    {
        int opcion;
        do
        {
            Console.WriteLine("Seleccione la transaccion a realizar");
            Console.WriteLine("1. Consulta de saldo");
            Console.WriteLine("2. Retiro de efectivo");
            Console.WriteLine("3. Deposito de efectivo");
            Console.WriteLine("4. Ver historial de transacciones");
            Console.WriteLine("5. Crear cuenta nueva");
            Console.WriteLine("6. Cerrar Sesion");
            opcion = int.Parse(Console.ReadLine());
            if (opcion == 6)
            {
                Console.WriteLine("Ha cerrado su sesion exitosamente");
                GuardarSaldo("C:\\Users\\crist\\source\\repos\\SimuladorCajero\\archivos\\saldo.txt", saldo);
                Console.ReadKey();
                break;
            }
        }
        while (opcion < 1 || opcion > 8);

        switch (opcion)
        {
            case 1:
                ConsultaSaldo(saldo);
                break;
            case 2:
                OpcionesRetiroEfectivo(saldo);
                break;
            case 3:
                DepositoEfectivo(saldo);
                break;
            case 4:
                VerTransacciones();
                break;
            case 5:
                CrearNuevaCuenta();
                break;
            default:
                Console.WriteLine("Opcion no disponible");
                break;
        }
    }

 //Metodos para las transacciones
    public void ConsultaSaldo(double saldo)
    {
        Console.WriteLine("Tu saldo es: " + saldo);
        OtraTransaccion();
    }

    public void OpcionesRetiroEfectivo(double saldo)
    {
        Console.WriteLine("Tu saldo es: " + saldo);
        Console.WriteLine("Cuanto requiere retirar? (ingrese la cantidad)");
        double retiro = double.Parse(Console.ReadLine());
        if (retiro < 0)
        {
            Console.WriteLine("Por favor ingrese una cantidad valida");
        }
        else
        if (retiro <= saldo)
        {
            this.saldo -= retiro;
            Transaccion nuevaTransaccion = new Transaccion(retiro, "Retiro");
            Transacciones.Add(nuevaTransaccion);
            Console.WriteLine("Reciba su dinero");
            Console.WriteLine("Su nuevo saldo es " + this.saldo);
        }
        else
        {
            Console.WriteLine("Saldo insuficiente para la transaccion");
        }
        GuardarSaldo("C:\\Users\\crist\\source\\repos\\SimuladorCajero\\archivos\\saldo.txt", saldo);
        OtraTransaccion();
    }

    public void DepositoEfectivo(double saldo)
    {
        Console.WriteLine("Tu saldo es: " + saldo);
        Console.WriteLine("Cuanto requiere depositar? (ingrese la cantidad)");
        double deposito = double.Parse(Console.ReadLine());
        if (deposito < 0)
        {
            Console.WriteLine("Por favor ingrese una cantidad valida");
        }
        else
        {
            this.saldo += deposito;
            Transaccion nuevaTransaccion = new Transaccion(deposito, "Deposito");
            Transacciones.Add(nuevaTransaccion);
            Console.WriteLine("Su nuevo saldo es " + this.saldo);
        }
        GuardarSaldo("C:\\Users\\crist\\source\\repos\\SimuladorCajero\\archivos\\saldo.txt", saldo);
        OtraTransaccion();
    }
    public void VerTransacciones()
    {
        Console.WriteLine("Historial de transacciones: ");
        foreach (Transaccion transaccion in Transacciones)
        {
            Console.WriteLine(transaccion);
        }
        OtraTransaccion();
    }
    public void CrearNuevaCuenta()
    {
        Console.WriteLine("Ingrese el número de la nueva cuenta: ");
        int nuevaCuenta = int.Parse(Console.ReadLine());

        Console.WriteLine("Ingrese una nueva contraseña: ");
        int nuevaContra = int.Parse(Console.ReadLine());

        string cuentaFilePath = Path.Combine("C:\\Users\\crist\\source\\repos\\SimuladorCajero\\archivos", $"cuenta_{nuevaCuenta}.txt");
        string contraFilePath = Path.Combine("C:\\Users\\crist\\source\\repos\\SimuladorCajero\\archivos", $"contra_{nuevaCuenta}.txt");

        File.WriteAllText(cuentaFilePath, nuevaCuenta.ToString());
        File.WriteAllText(contraFilePath, nuevaContra.ToString());

        Console.WriteLine("Ingrese el monto de depósito inicial: ");
        double depositoInicial = double.Parse(Console.ReadLine());

        string saldoFilePath = Path.Combine("C:\\Users\\crist\\source\\repos\\SimuladorCajero\\archivos", $"saldo_{nuevaCuenta}.txt");
        File.WriteAllText(saldoFilePath, depositoInicial.ToString());

        Console.WriteLine("Nueva cuenta creada con éxito. Por favor inicie sesión nuevamente.");
        Console.ReadKey();
    }


    public void OtraTransaccion()
    {
        int opcion;
        do
        {
            Console.WriteLine("Quiere realizar otra transacción? : ");
            Console.WriteLine("1. Si         2. No");
            opcion = int.Parse(Console.ReadLine());
        }
        while (opcion < 1 || opcion > 2);
        if (opcion == 1)
        {
            DesplegarMenu();
        }
        else
        {
            Console.WriteLine("Gracias por tu preferencia! Saliendo");
            GuardarSaldo("C:\\Users\\crist\\source\\repos\\SimuladorCajero\\archivos\\saldo.txt", saldo);
            Console.ReadKey();
        }
    }
    public void GuardarSaldo(string filePath, double saldo)
    {
        File.WriteAllText(filePath, saldo.ToString());
    }
    public class Transaccion
    {
        public double Monto { get; set; }
        public DateTime FechaHora { get; set; }
        public string Tipo { get; set; }
        public Transaccion(double monto, string tipo)
        {
            Monto = monto;
            FechaHora = DateTime.Now;
            Tipo = tipo;
        }
        public override string ToString()
        {
            return $"Fecha y Hora: {FechaHora}, Transaccion: {Tipo}, Monto: {Monto}";
        }
    }
}

