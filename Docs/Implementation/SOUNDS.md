# Техническая документация - Звуки

## Содержание

1. [Обзор системы](#обзор-системы)
2. [SoundID](#soundid)
3. [SoundProfile](#soundprofile)
4. [Настройка звуков для врагов](#настройка-звуков-для-врагов)
5. [SoundProfileContainer](#soundprofilecontainer)
6. [Глобальные звуки и музыка](#глобальные-звуки-и-музыка)

---

## Обзор системы

Система звуков разделена на:
- **Локальные звуки** - привязаны к конкретным объектам (игроку, врагам)
- **Глобальные звуки** - фоновые звуки и музыка

### Структура файлов

```
Sound/
├── LocalSounds/
│   ├── SoundID.cs           # Идентификаторы звуков
│   ├── SoundProfile.cs     # Профиль звуков (ScriptableObject)
│   ├── SoundPlayer.cs      # Проигрыватель локальных звуков
│   └── SoundProfileContainer.cs # Контейнер профилей
├── GlobalSoundTrigger.cs    # Триггер глобальных звуков
├── GlobalSoundEntry.cs      # Запись глобального звука
├── MusicPlayer.cs           # Проигрыватель музыки
└── SoundProfile.cs          # (дубликат, использовать LocalSounds/SoundProfile.cs)
```

---

## SoundID

Список доступных идентификаторов звуков: Assets/Scripts/Sound/LocalSounds/SoundID.cs

```csharp
public enum SoundID
{
    Attack,        // Атака
    TakeDamage,    // Получение урона
    Interact,      // Взаимодействие
    Footstep,      // Шаги
    Jump,          // Прыжок
    UI_Click,      // Клик UI
    UI_Hover,      // Наведение на UI
    Pickup,        // Подбор предмета
    Door_Open,     // Открытие двери
    Door_Close     // Закрытие двери
}
```

### Добавление новых звуков

1. Откройте файл SoundID.cs
2. Добавьте новый элемент в enum:
```csharp
public enum SoundID
{
    // Существующие...
    MyNewSound, // Добавьте сюда
}
```

---

## SoundProfile

Профиль звуков - ScriptableObject, содержащий набор звуков для объекта.

### Создание SoundProfile

1. В Unity: правый клик → GameAudio → Sound Profile
2. В появившемся окне:
   - Нажмите "+" для добавления звука
   - Выберите SoundID из выпадающего списка
   - Перетащите AudioClip в поле Clip
   - При необходимости настройте Volume (0-1)

### Параметры SoundEntry

| Параметр | Описание |
|----------|----------|
| Sound | Идентификатор SoundID |
| Clip | AudioClip файл звука |
| Volume | Громкость (0.0 - 1.0) |

---

## Настройка звуков для врагов

### Шаг 1: Создайте SoundProfile

Создайте SoundProfile как описано выше. Рекомендуется создать отдельный профиль для каждого типа врага.

### Шаг 2: Добавьте SoundProfileContainer

Добавьте компонент `SoundProfileContainer` на объект врага.

### Шаг 3: Назначьте профиль

В инспекторе SoundProfileContainer:
1. Нажмите "+" в списке Profiles
2. Перетащите созданный SoundProfile в поле Profile

### Пример использования в коде

```csharp
// Получение профиля
SoundProfile profile = _soundManager.GetProfile();

// Проигрывание звука
GameManager.Instance.FXSoundPlayer.PlaySound(SoundID.Attack, profile, transform);
```

---

## SoundProfileContainer

Компонент для хранения профилей звуков (Assets/Scripts/Sound/LocalSounds/SoundProfileContainer.cs).

### Использование

```csharp
private SoundProfileContainer _soundManager;

void Start()
{
    _soundManager = GetComponent<SoundProfileContainer>();
}

// Получение профиля
SoundProfile profile = _soundManager.GetProfile();
```

---

## Глобальные звуки и музыка

### MusicPlayer

Компонент для проигрывания фоновой музыки.

**Настройка:**
1. Добавьте MusicPlayer на объект в сцене
2. Назначьте AudioSource в инспекторе
3. Установите loop = true для бесконечного воспроизведения

### GlobalSoundTrigger

Компонент для запуска звуков при входе игрока в зону.

**Настройка:**
1. Создайте пустой объект с BoxCollider2D (IsTrigger = true)
2. Добавьте компонент GlobalSoundTrigger
3. Настройте звуки в списке Sound Entries:
   - Clip - аудиофайл
   - Volume - громкость
   - IsMusic - является ли это музыкой

### Изменение музыки в игре

```csharp
// Пример изменения музыки при входе в зону
public void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        // Смена музыки
        musicPlayer.PlayNewTrack(newMusicClip);
    }
}
```

---

## Рекомендации

1. **Организуйте звуки по типам** - создайте отдельные SoundProfile для игрока, разных врагов, окружения

2. **Используйте подходящие форматы**:
   - .wav или .ogg для музыки
   - .wav для коротких звуков

3. **Настройте громкость** - в SoundProfile для каждого звука

4. **Тестируйте в игре** - звуки должны быть слышны, но не перекрывать друг друга