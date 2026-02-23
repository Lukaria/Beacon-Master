// using UnityEditor;
// using UnityEngine;
//
// namespace CustomEditor
// {
// #if UNITY_EDITOR
//     [UnityEditor.CustomEditor(typeof(Data.GSheetMonoTool))]
//     class GSheetMonoToolEditor : Editor {
//         public override void OnInspectorGUI()
//         {
//             var gSheetParser = (Data.GSheetMonoTool)target;
//             base.OnInspectorGUI();
//             if (GUILayout.Button("Parse"))
//             {
//                 gSheetParser.Parse();
//             }
//         }
//     }
// #endif
// }