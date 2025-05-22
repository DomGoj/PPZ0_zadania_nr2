using System;

class Transakcja
{
    public DateTime data { get; }
    public string typ { get; }
    public decimal kwota { get; }
    public string opis { get; }

    public Transakcja(string typ, decimal kwota, string opis)
    {
        data = DateTime.Now;
        this.typ = typ;
        this.kwota = kwota;
        this.opis = opis;
    }

    public override string ToString()
    {
        return $"{data}: {typ} {kwota} - {opis}";
    }
}

class Konto
{
    public string wlasc { get; }
    public decimal saldo { get; private set; }

    private Transakcja[] transakcje = new Transakcja[100];
    private int liczbaTransakcji = 0;

    public Konto(string wlasc, decimal poczatkoweSaldo = 0)
    {
        this.wlasc = wlasc;
        saldo = poczatkoweSaldo;
    }

    public void Wplata(decimal kwota)
    {
        saldo += kwota;
        DodajTransakcje(new Transakcja("Wplata", kwota, "Wplata srodkow"));
    }

    public void Wyplata(decimal kwota)
    {
        if (kwota > saldo)
        {
            Console.WriteLine("Blad: brak srodkow");
            return;
        }

        saldo -= kwota;
        DodajTransakcje(new Transakcja("Wyplata", kwota, "Wyplata srodkow"));
    }

    public void PrzelewDo(Konto odbiorca, decimal kwota)
    {
        if (kwota > saldo)
        {
            Console.WriteLine("Blad: brak srodkow");
            return;
        }

        saldo -= kwota;
        odbiorca.saldo += kwota;

        DodajTransakcje(new Transakcja("Przelew Wychodzacy", kwota, $"Do {odbiorca.wlasc}"));
        odbiorca.DodajTransakcje(new Transakcja("Przelew Przychodzacy", kwota, $"Od {wlasc}"));
    }

    private void DodajTransakcje(Transakcja t)
    {
        if (liczbaTransakcji < transakcje.Length)
        {
            transakcje[liczbaTransakcji++] = t;
        }
        else
        {
            Console.WriteLine("Limit transakcji przekroczony");
        }
    }

    public void Historia()
    {
        Console.WriteLine($"Historia transakcji konta: {wlasc}");
        for (int i = 0; i < liczbaTransakcji; i++)
        {
            Console.WriteLine(transakcje[i].ToString());
        }
    }
}

class Program
{
    static void Main()
    {
        Konto test1 = new Konto("Test1", 1000);
        Konto test2 = new Konto("Test2", 2000);

        while (true)
        {
            Console.WriteLine("\n=====MENU======");
            Console.WriteLine("1. Wplata");
            Console.WriteLine("2. Wyplata");
            Console.WriteLine("3. Przelew");
            Console.WriteLine("4. Historia transakcji");
            Console.WriteLine("5. Wyjscie");
            Console.Write("Wybierz opcje (1-5): ");
            string wybor = Console.ReadLine();

            switch (wybor)
            {
                case "1":
                    Console.Write("Konto (Test1/Test2): ");
                    string wpKonto = Console.ReadLine();
                    Konto kontoWplata = WybierzKonto(wpKonto, test1, test2);
                    if (kontoWplata != null)
                    {
                        Console.Write("Kwota: ");
                        decimal kw = decimal.Parse(Console.ReadLine());
                        kontoWplata.Wplata(kw);
                        Console.WriteLine("Wplata wykonana");
                    }
                    else Console.WriteLine("Niepoprawna nazwa konta");
                    break;

                case "2":
                    Console.Write("Konto (Test1/Test2): ");
                    string wypKonto = Console.ReadLine();
                    Konto kontoWyplata = WybierzKonto(wypKonto, test1, test2);
                    if (kontoWyplata != null)
                    {
                        Console.Write("Kwota: ");
                        decimal kw = decimal.Parse(Console.ReadLine());
                        kontoWyplata.Wyplata(kw);
                        Console.WriteLine("Wyplata wykonana");
                    }
                    else Console.WriteLine("Niepoprawna nazwa konta");
                    break;

                case "3":
                    Console.Write("Nadawca (Test1/Test2): ");
                    Konto kontoOd = WybierzKonto(Console.ReadLine(), test1, test2);
                    Console.Write("Odbiorca (Test1/Test2): ");
                    Konto kontoDo = WybierzKonto(Console.ReadLine(), test1, test2);
                    if (kontoOd != null && kontoDo != null && kontoOd != kontoDo)
                    {
                        Console.Write("Kwota: ");
                        decimal kw = decimal.Parse(Console.ReadLine());
                        kontoOd.PrzelewDo(kontoDo, kw);
                        Console.WriteLine("Przelew wykonany");
                    }
                    else Console.WriteLine("Blad: nieprawidlowe konta");
                    break;

                case "4":
                    Console.Write("Konto (Test1/Test2): ");
                    Konto kontoHist = WybierzKonto(Console.ReadLine(), test1, test2);
                    if (kontoHist != null)
                    {
                        kontoHist.Historia();
                        Console.WriteLine($"Saldo: {kontoHist.saldo} zl");
                    }
                    else Console.WriteLine("Niepoprawna nazwa konta");
                    break;

                case "5":
                    Console.WriteLine("Wyjscie z programu");
                    return;

                default:
                    Console.WriteLine("Nieprawidlowy wybor");
                    break;
            }
        }
    }

    static Konto WybierzKonto(string nazwa, Konto t1, Konto t2)
    {
        if (nazwa == "Test1") return t1;
        if (nazwa == "Test2") return t2;
        return null;
    }
} 
