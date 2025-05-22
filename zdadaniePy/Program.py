from datetime import datetime

class Transakcja:
    def __init__(self, typ, kwota, opis):
        self.data = datetime.now()
        self.typ = typ
        self.kwota = kwota
        self.opis = opis

    def __str__(self):
        return f"{self.data}: {self.typ} {self.kwota} - {self.opis}"

class Konto:
    def __init__(self, wlasc, saldo=0):
        self.wlasc = wlasc
        self.saldo = saldo
        self.transakcje = []

    def wplata(self, kwota):
        self.saldo += kwota
        self.transakcje.append(Transakcja("Wplata", kwota, "Wplata srodkow"))

    def wyplata(self, kwota):
        if kwota > self.saldo:
            print("Blad: brak srodkow")
        else:
            self.saldo -= kwota
            self.transakcje.append(Transakcja("Wyplata", kwota, "Wyplata srodkow"))

    def przelew_do(self, odbiorca, kwota):
        if kwota > self.saldo:
            print("Blad: brak srodkow")
        else:
            self.saldo -= kwota
            odbiorca.saldo += kwota
            self.transakcje.append(Transakcja("Przelew Wychodzacy", kwota, f"Do {odbiorca.wlasc}"))
            odbiorca.transakcje.append(Transakcja("Przelew Przychodzacy", kwota, f"Od {self.wlasc}"))

    def historia(self):
        print(f"Historia transakcji konta: {self.wlasc}")
        for t in self.transakcje:
            print(t)
        print(f"Saldo: {self.saldo} zl")
def wybierz_konto(nazwa, konta):
    return konta.get(nazwa)

def main():
    konta = {
        "Test1": Konto("Test1", 1000),
        "Test2": Konto("Test2", 2000)
    }

    while True:
        print("\n--- MENU ---")
        print("1. Wplata")
        print("2. Wyplata")
        print("3. Przelew")
        print("4. Historia transakcji")
        print("5. Wyjscie")
        wybor = input("Wybierz opcje (1-5): ")

        match wybor:
            case "1":
                nazwa = input("Konto (Test1/Test2): ")
                konto = wybierz_konto(nazwa, konta)
                if konto:
                    kwota = float(input("Kwota: "))
                    konto.wplata(kwota)
                    print("Wplata wykonana")
                else:
                    print("Niepoprawna nazwa konta")

            case "2":
                nazwa = input("Konto (Test1/Test2): ")
                konto = wybierz_konto(nazwa, konta)
                if konto:
                    kwota = float(input("Kwota: "))
                    konto.wyplata(kwota)
                    print("Wyplata wykonana")
                else:
                    print("Niepoprawna nazwa konta")

            case "3":
                nadawca = input("Nadawca (Test1/Test2): ")
                odbiorca = input("Odbiorca (Test1/Test2): ")
                konto_od = wybierz_konto(nadawca, konta)
                konto_do = wybierz_konto(odbiorca, konta)
                if konto_od and konto_do and konto_od != konto_do:
                    kwota = float(input("Kwota: "))
                    konto_od.przelew_do(konto_do, kwota)
                    print("Przelew wykonany")
                else:
                    print("Blad: nieprawidlowe konta")

            case "4":
                nazwa = input("Konto (Test1/Test2): ")
                konto = wybierz_konto(nazwa, konta)
                if konto:
                    konto.historia()
                else:
                    print("Niepoprawna nazwa konta")

            case "5":
                print("Wyjscie z programu")
                break

            case _:
                print("Nieprawidlowy wybor")

if __name__ == "__main__":
    main()
