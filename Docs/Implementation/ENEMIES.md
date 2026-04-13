# Техническая документация - Враги

## Содержание

1. [Обзор системы](#обзор-системы)
2. [Структура NPC](#структура-npc)
3. [Зоны обнаружения](#зоны-обнаружения)
4. [Состояния врагов](#состояния-врагов)
5. [Враги в головоломках](#враги-в-головоломках)

---

## Обзор системы

Система врагов основана на конечном автомате состояний (State Machine). Каждый враг имеет:
- NPCStateMachine - основной компонент
- Триггер-зоны для обнаружения игрока
- Набор состояний (Idle, Chase, Attack, RangedAttack и т.д.)

---

## Структура NPC

### Компоненты врага

```
NPC GameObject
├── NPCStateMachine (основной скрипт)
├── NavMeshAgent (навигация)
├── Animator (анимации)
├── SpriteRenderer (спрайт)
├── SoundProfileContainer (звуки)
├── VisionZone (GameObject с NPCTriggerProxy)
├── AttackZone (GameObject с NPCTriggerProxy)
└── RangedAttackZone (GameObject с NPCTriggerProxy, опционально)
```

### Основные параметры NPCStateMachine

- **Init State** - начальное состояние врага
- **Available States** - список доступных состояний
- **Vision/Attack/RangedAttack Zone** - ссылки на зоны обнаружения

---

## Зоны обнаружения

### NPCTriggerProxy

Компонент зоны обнаружения (Assets/Scripts/Character/StatePattern/NPC/NPCTriggerProxy.cs).

### Параметры

| Параметр | Описание |
|----------|----------|
| Zone Type | Тип зоны (Vision, Attack, RangedAttack) |
| Dependent on Vision | Учитывать видимость игрока |
| Vision Blocking Layers | Слои, блокирующие видимость (стены) |
| Check Interval | Интервал проверки видимости (по умолчанию 0.1с) |

### Настройка зон

1. **Vision Zone** (зона видимости):
   - Обычно включена опция "Dependent on Vision"
   - Настройте Vision Blocking Layers (слои стен)
   - Размер определяется CircleCollider2D

2. **Attack Zone** (зона атаки ближнего боя):
   - Обычно опция "Dependent on Vision" выключена
   - Меньший радиус, чем у Vision Zone

3. **Ranged Attack Zone** (зона атаки дальнего боя):
   - Только для врагов с дальней атакой
   - Больший радиус

---

## Состояния врагов

### Доступные состояния

Состояния определены в `NPCStateID` (Assets/Scripts/Character/StatePattern/NPC/NPCState.cs):
- Idle - ожидание
- Move - патрулирование
- Chase - преследование
- Attack - атака ближнего боя
- RangedAttack - атака дальнего боя
- TakeDamage - получение урона
- Dying - смерть

### Создание нового состояния

1. Создайте класс, наследующий от `NPCState`
2. Реализуйте необходимые методы:

```csharp
[CreateAssetMenu(menuName = "NPC/States/Название")]
public class MyState : NPCState
{
    public override void Enter()
    {
        // Выполняется при входе в состояние
    }

    public override void UpdateState()
    {
        // Выполняется каждый кадр
    }

    public override void OnZoneEnter(NPCTriggerZoneType zone, Collider2D other)
    {
        // При входе в зону
    }

    public override void OnZoneExit(NPCTriggerZoneType zone, Collider2D other)
    {
        // При выходе из зоны
    }

    public override void Exit()
    {
        // Выполняется при выходе из состояния
    }
}
```

3. Добавьте новое состояние в NPCStateID
4. Добавьте состояние в список состояний врага в инспекторе

---

## Враги в головоломках

### Интеграция с системой головоломок

Враги реализуют интерфейс IPuzzleElement:
- **PuzzleState.On** - враг жив
- **PuzzleState.Off** - враг мёртв

### Создание головоломки с убийством врагов

1. Добавьте `PuzzleManager` на сцену
2. В Puzzle Parts добавьте врагов
3. Установите Required State = Off для каждого врага
4. Настройте Puzzle Targets (например, открытие двери)

### Как это работает

При убийстве врага (через бой или напрямую через SetState(Off)):
1. Враг переходит в состояние Dying
2. Текущее состояние становится Off
3. PuzzleManager проверяет условия
4. Если все враги мертвы - головоломка решена

### Принудительное убийство врага

```csharp
// Получить компонент врага
NPCStateMachine enemy = other.GetComponent<NPCStateMachine>();

// Установить состояние Off (враг умрёт)
enemy.SetState(PuzzleState.Off);
```

---

## Примеры настройки

### Простой враг ближнего боя

1. NPCStateMachine с состояниями: Idle, Chase, Attack, TakeDamage, Dying
2. Vision Zone (большой радиус)
3. Attack Zone (малый радиус)
4. Нет RangedAttackZone

### Враг дальнего боя

1. NPCStateMachine с состояниями: Idle, Chase, RangedAttack, TakeDamage, Dying
2. Vision Zone
3. RangedAttack Zone (большой радиус)
4. Дополнительный компонент RangedAttackManager

### Босс

1. Много состояний
2. Несколько зон атаки
3. Может включать головоломку как часть боя