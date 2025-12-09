// using UnityEngine;
// using UnityEditor;

// /// <summary>
// /// Редактор для массового добавления SpriteFadeOcclusion на дочерние объекты
// /// </summary>
// [CustomEditor(typeof(Transform))]
// public class SpriteFadeOcclusionHelper : Editor
// {
//     public override void OnInspectorGUI()
//     {
//         DrawDefaultInspector();

//         Transform transform = (Transform)target;

//         // Показываем кнопку только если есть дочерние объекты
//         if (transform.childCount > 0)
//         {
//             EditorGUILayout.Space(10);
//             EditorGUILayout.HelpBox("Инструмент для настройки прозрачности и сортировки объектов", MessageType.Info);

//             if (GUILayout.Button("🎨 Настроить статичные объекты (Fade + Sorting)", GUILayout.Height(40)))
//             {
//                 SetupStaticObjectsWithFadeAndSorting(transform);
//             }

//             EditorGUILayout.Space(5);

//             if (GUILayout.Button("➕ Добавить только SpriteFadeOcclusion", GUILayout.Height(30)))
//             {
//                 AddSpriteFadeOcclusionToChildren(transform);
//             }

//             EditorGUILayout.Space(5);

//             if (GUILayout.Button("🗑️ Удалить Fade + Sorting с дочерних объектов", GUILayout.Height(30)))
//             {
//                 RemoveComponentsFromChildren(transform);
//             }
//         }
//     }

//     private void SetupStaticObjectsWithFadeAndSorting(Transform parent)
//     {
//         int setupCount = 0;
//         int skippedCount = 0;

//         // Проходим по всем прямым детям
//         foreach (Transform child in parent)
//         {
//             // Проверяем есть ли SpriteRenderer на объекте или его детях
//             SpriteRenderer[] sprites = child.GetComponentsInChildren<SpriteRenderer>();
//             if (sprites.Length == 0)
//             {
//                 skippedCount++;
//                 continue;
//             }

//             // Добавляем SpriteFadeOcclusion если его нет
//             SpriteFadeOcclusion fadeComponent = child.GetComponent<SpriteFadeOcclusion>();
//             if (fadeComponent == null)
//             {
//                 fadeComponent = Undo.AddComponent<SpriteFadeOcclusion>(child.gameObject);
//             }

//             // Добавляем YSortingOrder если его нет
//             YSortingOrder sortingComponent = child.GetComponent<YSortingOrder>();
//             if (sortingComponent == null)
//             {
//                 sortingComponent = Undo.AddComponent<YSortingOrder>(child.gameObject);
//             }

//             // Настраиваем YSortingOrder для статичного объекта
//             if (sortingComponent != null)
//             {
//                 SerializedObject so = new SerializedObject(sortingComponent);
//                 so.FindProperty("_applyToChildren").boolValue = true;
//                 so.FindProperty("_isStatic").boolValue = true;
//                 so.FindProperty("_orderOffset").intValue = 0;
//                 so.ApplyModifiedProperties();
//             }

//             setupCount++;
//         }

//         if (setupCount > 0)
//         {
//             EditorUtility.DisplayDialog("Успешно!",
//                 $"✅ Настроено объектов: {setupCount}\n" +
//                 $"⏭️ Пропущено объектов: {skippedCount}\n\n" +
//                 $"Каждый объект теперь имеет:\n" +
//                 $"• SpriteFadeOcclusion (прозрачность)\n" +
//                 $"• YSortingOrder (сортировка, статичный режим)",
//                 "OK");
//         }
//         else
//         {
//             EditorUtility.DisplayDialog("Внимание",
//                 $"Не найдено объектов для настройки.\n" +
//                 $"Пропущено: {skippedCount}\n\n" +
//                 $"Убедитесь что у дочерних объектов есть SpriteRenderer.",
//                 "OK");
//         }
//     }

//     private void AddSpriteFadeOcclusionToChildren(Transform parent)
//     {
//         int addedCount = 0;
//         int skippedCount = 0;

//         // Проходим по всем прямым детям (не внукам)
//         foreach (Transform child in parent)
//         {
//             // Пропускаем если уже есть компонент
//             if (child.GetComponent<SpriteFadeOcclusion>() != null)
//             {
//                 skippedCount++;
//                 continue;
//             }

//             // Проверяем есть ли SpriteRenderer на объекте или его детях
//             SpriteRenderer[] sprites = child.GetComponentsInChildren<SpriteRenderer>();
//             if (sprites.Length > 0)
//             {
//                 Undo.AddComponent<SpriteFadeOcclusion>(child.gameObject);
//                 addedCount++;
//             }
//             else
//             {
//                 skippedCount++;
//             }
//         }

//         if (addedCount > 0)
//         {
//             EditorUtility.DisplayDialog("Успешно!",
//                 $"✅ Добавлено компонентов: {addedCount}\n" +
//                 $"⏭️ Пропущено объектов: {skippedCount}\n\n" +
//                 $"Компоненты добавлены на дочерние объекты с SpriteRenderer.",
//                 "OK");
//         }
//         else
//         {
//             EditorUtility.DisplayDialog("Внимание",
//                 $"Не найдено объектов для добавления компонента.\n" +
//                 $"Пропущено: {skippedCount}\n\n" +
//                 $"Убедитесь что у дочерних объектов есть SpriteRenderer.",
//                 "OK");
//         }
//     }

//     private void RemoveComponentsFromChildren(Transform parent)
//     {
//         if (!EditorUtility.DisplayDialog("Подтверждение",
//             "Удалить все компоненты SpriteFadeOcclusion и YSortingOrder с дочерних объектов?",
//             "Да", "Отмена"))
//         {
//             return;
//         }

//         int removedCount = 0;

//         foreach (Transform child in parent)
//         {
//             SpriteFadeOcclusion fadeComponent = child.GetComponent<SpriteFadeOcclusion>();
//             if (fadeComponent != null)
//             {
//                 Undo.DestroyObjectImmediate(fadeComponent);
//                 removedCount++;
//             }

//             YSortingOrder sortingComponent = child.GetComponent<YSortingOrder>();
//             if (sortingComponent != null)
//             {
//                 Undo.DestroyObjectImmediate(sortingComponent);
//                 removedCount++;
//             }
//         }

//         EditorUtility.DisplayDialog("Готово",
//             $"Удалено компонентов: {removedCount}",
//             "OK");
//     }
// }
