using UnityEngine;
using UnityEngine.SceneManagement;

namespace Vladimir.Utils
{
    public static class SceneHelper
    {
        /// <summary>
        /// Перемещает объект в сцену по указанному пути, если она загружена.
        /// </summary>
        /// <param name="targetObject">Объект для перемещения</param>
        /// <param name="scenePath">Путь к целевой сцене (например, "Assets/Scenes/Main.unity")</param>
        public static void MoveObjectToScene(GameObject targetObject, string scenePath)
        {
            if (targetObject == null) return;

            // 1. Находим сцену
            Scene targetScene = SceneManager.GetSceneByPath(scenePath);

            // 2. Проверяем, загружена ли сцена
            if (targetScene.IsValid() && targetScene.isLoaded)
            {
                // 3. У объекта не должно быть родителя для перемещения между сценами
                if (targetObject.transform.parent != null)
                {
                    targetObject.transform.SetParent(null);
                }

                // 4. Переносим объект
                SceneManager.MoveGameObjectToScene(targetObject, targetScene);
            }
            else
            {
                Debug.LogError($"Не удалось переместить: сцена по пути '{scenePath}' не загружена.");
            }
        }

        /// <summary>
        /// Если сцена по указанному пути загружена, деактивирует все её корневые объекты.
        /// </summary>
        /// <param name="scenePath">Полный путь к сцене (например, "Assets/Scenes/MyLevel.unity")</param>
        public static void DeactivateSceneIfLoaded(string scenePath)
        {
            // 1. Находим сцену по её пути в проекте
            Scene targetScene = SceneManager.GetSceneByPath(scenePath);

            // 2. Проверяем, валидна ли ссылка на сцену и загружена ли она в данный момент
            if (targetScene.IsValid() && targetScene.isLoaded)
            {
                // 3. Получаем все корневые (верхние в иерархии) объекты этой сцены
                GameObject[] rootObjects = targetScene.GetRootGameObjects();

                // 4. Проходим по каждому объекту и выключаем его
                foreach (GameObject obj in rootObjects)
                {
                    obj.SetActive(false);
                }
            }
            else
            {
                Debug.LogWarning($"Сцена по пути '{scenePath}' не загружена или не найдена.");
            }
        }

        public static void PauseSceneIfLoaded(string scenePath)
        {
            Scene targetScene = SceneManager.GetSceneByPath(scenePath);

            if (targetScene.IsValid() && targetScene.isLoaded)
            {
                GameObject[] rootObjects = targetScene.GetRootGameObjects();

                foreach (GameObject obj in rootObjects)
                {
                    MonoBehaviour[] behaviours = obj.GetComponentsInChildren<MonoBehaviour>(true);
                    foreach (MonoBehaviour behaviour in behaviours)
                    {
                        behaviour.enabled = false;
                    }

                    Rigidbody2D[] rigidbodies = obj.GetComponentsInChildren<Rigidbody2D>(true);
                    foreach (Rigidbody2D rb in rigidbodies)
                    {
                        rb.simulated = false;
                    }
                }
            }
            else
            {
                Debug.LogWarning($"Сцена по пути '{scenePath}' не загружена или не найдена.");
            }
        }

        public static void ResumeSceneIfLoaded(string scenePath)
        {
            Scene targetScene = SceneManager.GetSceneByPath(scenePath);

            if (targetScene.IsValid() && targetScene.isLoaded)
            {
                GameObject[] rootObjects = targetScene.GetRootGameObjects();

                foreach (GameObject obj in rootObjects)
                {
                    MonoBehaviour[] behaviours = obj.GetComponentsInChildren<MonoBehaviour>(true);
                    foreach (MonoBehaviour behaviour in behaviours)
                    {
                        behaviour.enabled = true;
                    }

                    Rigidbody2D[] rigidbodies = obj.GetComponentsInChildren<Rigidbody2D>(true);
                    foreach (Rigidbody2D rb in rigidbodies)
                    {
                        rb.simulated = true;
                    }
                }
            }
            else
            {
                Debug.LogWarning($"Сцена по пути '{scenePath}' не загружена или не найдена.");
            }
        }
    }
}