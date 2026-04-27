# WPF Data Binding

Celem zadania jest stworzenie aplikacji typu Kanban Board do zarządzania zadaniami. Aplikacja powinna umożliwiać dodawanie, edytowanie, usuwanie oraz przenoszenie zadań między kolumnami: *To Do* (do zrobienia), *In Progress* (w trakcie) oraz *Done* (zrobione).

W przypadkach wątpliwych, gdy specyfikacja nie jest jasna lub brakuje szczegółów, należy naśladować działanie dostarczonej aplikacji przykładowej, z wyłączeniem jej potencjalnych błędów.

## Punktacja i wymagania

### Układ i parametry okna (1.5 pkt)
- Parametry okna (0.5 pkt)
    - tytuł okna: *Kanban board*
    - początkowe wymiary okna: 1000 x 800
    - minimalne wymiary okna: 1000 x 500
- Główny układ (0.5 pkt)
    - Okno podzielone na cztery kolumny równej szerokości
    - Każda kolumna na pogrubiony nagłówek rozmiaru 16
    - Pierwsze trzy kolumny to odpowiednio *To Do*, *In Progress*  oraz *Done*
    - Czwarta kolumna służy do dodawnia i edycji zadań (*Add task*)
- Panel dodawani zadań (0.5 pkt)
    - Pod nagłówiem znajduje się pole tekstowe z paddingiem 6, marginesami 10 i rozmiarem czcionki 12
    - Pod polem tesktowym znajdują się dwa przyciski: *Save* i *Cancel*
        - Szerokość: 90
        - Margines: 2
        - Padding: 10
        - Brak obramowania
    - Pierwszy przycisk (*Save*) ma zielone tło (`#4CAF50`) i biały napis

### Dodawanie zadań (2.5 pkt)
- Po kliknięciu przycisku *Save* do listy *To Do* zostaje dodane nowe zadanie (1 pkt)
- Na każdej liście zadania wyświetlane są jedno pod drugim, każde zadanie jest wyświetlane wewnątrz ramki: (1 pkt)
    - Zaokrąglone krawędzie (`CornerRadius`)
    - Padding: 8
    - Margines: 5
    - Szara ramka grubości 1
    - Jeśli treść zadania jest dłuższa niż szerokość ramki, teskt zostaje zawinięty (`TextWrapping`) 
- Zadania są wyświetlane z użyciem `DataTemplate` (0.5 pkt)

### Przenoszenie i usuwanie zadań (2.5 pkt)
- Pod treścią zadania znajdują się cztery przyciski (0.5 pkt)
    - *<* (szerokość: 30)
    - *Edit* (szerokość: 40)
    - *>* (szerokość: 30)
    - *Delete* (szerokość: 60) 
- Przyciski mają następujący wygląd: (0.5 pkt)
    - Margines: 2
    - Padding: 6
    - Brak obramowania
- Przyciski *<* i *>* przenoszą zadanie pomiędzy kolumnami (1 pkt)
- Przycisk *Delete* usuwa zadanie (0.5 pkt)
    - Przycisk ma czerwone (`#E74C3C`) tło i biały napis

### Edycja zadań (1.5 pkt)
- Kliknięcie przycisku *Edit* włącza tryb edycji zadania: (1 pkt)
    - W polu tesktowym pojawia się treść zadania
    - Po kliknięciu *Save* treść zadania zostaje zaktualizowana i edycja zostaje zakończona
    - Po kliknięciu *Cancel* edycja zostaje zakończona, a treść zadania się nie zmienia
- Gdy zadanie jest edytowane, nagłówke *Add task* zmienia się na *Edit task* (0.5 pkt)

## Wskazówki
- Układ: `Grid`, `StackPanel`
- Kolumny: `ListBox`
- `TextBlock`, `Border`, `Label`, `Button`
- `UpdateSourceTrigger=PropertyChanged`
- `INotifyPropertyChanged`
- `ObservableCollection`

### Dostęp do danych w zdarzeniu Click
Aby uzyskać dostęp do obiektu zadania (TaskItem) powiązanego z przyciskiem wewnątrz DataTemplate, należy wykorzystać właściwość DataContext nadawcy zdarzenia, np:
```
var button = sender as Button;
var task = button?.DataContext as TaskItem;
```
