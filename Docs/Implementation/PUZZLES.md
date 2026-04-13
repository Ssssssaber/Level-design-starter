# Техническая документация - Головоломки

## Содержание

1. [Обзор системы](#обзор-системы)
2. [PuzzleManager](#puzzlemanager)
3. [IPuzzleElement](#ipuzzleelement)
4. [Интеграция объектов](#интеграция-объектов)
5. [Примеры головоломок](#примеры-головоломок)

---

## Обзор системы

Система головоломок позволяет создавать связи между объектами, где изменение состояния одних объектов влияет на другие.

### Основные компоненты

| Компонент | Файл | Назначение |
|-----------|------|------------|
| PuzzleManager | Puzzle/PuzzleManager.cs | Управление головоломкой |
| IPuzzleElement | Puzzle/IPuzzleElement.cs | Интерфейс для объектов головоломки |
| PuzzleState | Puzzle/PuzzleState.cs | Состояния (On/Off) |
| PuzzleEvents | Puzzle/PuzzleEvents.cs | События изменения состояний |

---

## PuzzleManager

Компонент для создания и управления головоломками (Assets/Scripts/Puzzle/PuzzleManager.cs).

### Создание головоломки

1. Создайте пустой GameObject в сцене
2. Добавьте компонент `PuzzleManager`
3. Настройте параметры:
   - **Puzzle Parts** - объекты, которые должны изменить состояние
   - **Puzzle Targets** - объекты, которые изменятся при решении
   - **Check On Part Changed** - проверять при изменении частей
   - **Reset On Unsolved** - сбрасывать при нарушении условий

### Настройка Puzzle Parts

Каждая часть головоломки содержит:
- **Puzzle Object** - ссылка на объект
- **Required State** - требуемое состояние (On или Off)

Головоломка считается решённой, когда ВСЕ части находятся в требуемых состояниях.

### Настройка Puzzle Targets

Каждая цель содержит:
- **Target Object** - ссылка на объект
- **Target State** - состояние после решения головоломки

---

## IPuzzleElement

Интерфейс для объектов, участвующих в головоломках (Assets/Scripts/Puzzle/IPuzzleElement.cs).

```csharp
public interface IPuzzleElement
{
    PuzzleState CurrentState { get; }    // Текущее состояние
    PuzzleState InitialState { get; }     // Начальное состояние
    void SetState(PuzzleState state);     // Установить состояние
}
```

### PuzzleState

Перечисление состояний (Assets/Scripts/Puzzle/PuzzleState.cs):
- **On** - включено (по умолчанию)
- **Off** - выключено

---

## Интеграция объектов

### Существующие объекты с IPuzzleElement

| Объект | Файл | Состояния |
|--------|------|-----------|
| FloorTorch | InteractableObjects/FloorTorch.cs | On/Off (включён/выключен) |
| PuzzleDoor | InteractableObjects/PuzzleDoor.cs | On/Off (открыт/закрыт) |
| Trap | HealthManipulation/Trap.cs | On/Off (активна/неактивна) |
| NPCStateMachine | Character/StatePattern/NPC/NPCStateMachine.cs | On/Off (жив/мёртв) |

### Пример реализации IPuzzleElement

```csharp
public class MyObject : MonoBehaviour, IPuzzleElement
{
    [SerializeField] private bool _isActive = true;
    
    public PuzzleState CurrentState => _isActive ? PuzzleState.On : PuzzleState.Off;
    public PuzzleState InitialState => PuzzleState.On;
    
    public void SetState(PuzzleState state)
    {
        _isActive = (state == PuzzleState.On);
        // Логика изменения состояния
    }
}
```

### Уведомление об изменении

При изменении состояния объекта необходимо вызвать:

```csharp
PuzzleEvents.NotifyStateChanged(this);
```

Это уведомит PuzzleManager о изменении и он проверит условия головоломки.

---

## Примеры головоломок

### Пример 1: Факелы и дверь

**Задача:** Открыть дверь, зажгрев все факелы.

**Настройка:**
1. Создайте PuzzleManager
2. Добавьте факелы в Puzzle Parts:
   - Puzzle Object: факел 1, Required State: On
   - Puzzle Object: факел 2, Required State: On
3. Добавьте дверь в Puzzle Targets:
   - Target Object: дверь, Target State: On (открыта)

**Как это работает:**
1. Игрок взаимодействует с факелами, зажигая их
2. При каждом изменении вызывается PuzzleEvents.NotifyStateChanged
3. PuzzleManager проверяет - если все факелы горят (On), дверь открывается

### Пример 2: Убийство врагов и открытие прохода

**Задача:** Убить всех врагов, чтобы открылся проход.

**Настройка:**
1. Добавьте врагов в Puzzle Parts:
   - Puzzle Object: враг 1, Required State: Off (мёртв)
   - Puzzle Object: враг 2, Required State: Off
2. Добавьте дверь в Puzzle Targets:
   - Target Object: дверь, Target State: On

**Как это работает:**
1. При убийстве врага он переходит в состояние Off
2. PuzzleManager проверяет - если все враги мертвы, дверь открывается

### Пример 3: Ловушки

**Задача:** Отключить ловушки для прохода.

**Настройка:**
1. Ловушка - это IPuzzleElement с состояниями On (активна) / Off (неактивна)
2. Можно управлять через PuzzleManager:
   - При решении другой головоломки - ловушки отключаются
   - При нарушении - ловушки включаются обратно (если Reset On Unsolved = true)

---

## Восстановление состояний

### Параметр Reset On Unsolved

Если включено (по умолчанию):
- При изменении состояния частей обратно на неправильное
- Все цели возвращаются в InitialState

Это полезно для:
- Ловушек, которые должны включиться обратно
- Дверей, которые должны закрыться
- Врагов, которые должны возродиться

### Пример

1. Головоломка решена - дверь открыта (SetState(On))
2. Игрок случайно погасил факел - дверь закрывается (Reset к InitialState)