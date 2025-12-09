// using UnityEngine;
// using UnityEditor;

// /// <summary>
// /// Улучшенный редактор для YSortingOrder с подсказками и кнопками быстрой настройки
// /// </summary>
// [CustomEditor(typeof(YSortingOrder))]
// public class YSortingOrderEditor : Editor
// {
//     public override void OnInspectorGUI()
//     {
//         DrawDefaultInspector();

//         EditorGUILayout.Space(10);
//         EditorGUILayout.HelpBox("Быстрая настройка для типичных случаев:", MessageType.Info);

//         YSortingOrder script = (YSortingOrder)target;

//         EditorGUILayout.BeginHorizontal();

//         // Кнопка для персонажа
//         if (GUILayout.Button("Настроить для Персонажа", GUILayout.Height(30)))
//         {
//             Undo.RecordObject(target, "Setup for Player");

//             SerializedObject so = new SerializedObject(target);
//             so.FindProperty("_orderOffset").intValue = 0;
//             so.FindProperty("_updateInterval").floatValue = 0f;
//             so.FindProperty("_applyToChildren").boolValue = false;
//             so.FindProperty("_isStatic").boolValue = false;
//             so.ApplyModifiedProperties();

//             EditorUtility.SetDirty(target);
//         }

//         EditorGUILayout.EndHorizontal();

//         EditorGUILayout.BeginHorizontal();

//         // Кнопка для оружия
//         if (GUILayout.Button("Настроить для Оружия\n(с руками/магазином)", GUILayout.Height(40)))
//         {
//             Undo.RecordObject(target, "Setup for Weapon");

//             SerializedObject so = new SerializedObject(target);
//             so.FindProperty("_orderOffset").intValue = 1;
//             so.FindProperty("_updateInterval").floatValue = 0f;
//             so.FindProperty("_applyToChildren").boolValue = true;
//             so.FindProperty("_isStatic").boolValue = false;
//             so.ApplyModifiedProperties();

//             EditorUtility.SetDirty(target);
//         }

//         EditorGUILayout.EndHorizontal();

//         EditorGUILayout.BeginHorizontal();

//         // Кнопка для статичных объектов
//         if (GUILayout.Button("Настроить для Дерева/\nСтатичного объекта", GUILayout.Height(40)))
//         {
//             Undo.RecordObject(target, "Setup for Static");

//             SerializedObject so = new SerializedObject(target);
//             so.FindProperty("_orderOffset").intValue = 0;
//             so.FindProperty("_updateInterval").floatValue = 0f;
//             so.FindProperty("_applyToChildren").boolValue = false;
//             so.FindProperty("_isStatic").boolValue = true;
//             so.ApplyModifiedProperties();

//             EditorUtility.SetDirty(target);
//         }

//         EditorGUILayout.EndHorizontal();

//         EditorGUILayout.Space(5);

//         // Подсказки
//         EditorGUILayout.HelpBox(
//             "• Apply To Children - включить для оружия с дочерними объектами (руки, магазин)\n" +
//             "• Is Static - включить для деревьев и других статичных объектов (оптимизация)",
//             MessageType.None);
//     }
// }
