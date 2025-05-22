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

if __name__ == "__main__":
    main()
