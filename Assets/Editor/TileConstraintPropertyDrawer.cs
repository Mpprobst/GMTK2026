using UnityEngine;
using UnityEditor;
using System.Drawing.Printing;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(TileData.TileConstraint))]
public class TileConstraintPropertyDrawer : PropertyDrawer
{
    private Vector2 scrollPosition;
    SerializedProperty direction_prop, allowedTypes_prop;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        direction_prop = property.FindPropertyRelative("direction");
        allowedTypes_prop = property.FindPropertyRelative("allowedTypes");


        var directionRect = new Rect(position.x, position.y, 100, EditorGUIUtility.singleLineHeight);
        var labelRect = new Rect(position.x + directionRect.width + 5, position.y, 50, EditorGUIUtility.singleLineHeight);
        var tileTypeRect = new Rect(labelRect.position.x + labelRect.width + 5, position.y, 100, EditorGUIUtility.singleLineHeight);

        Rect windowRect = EditorGUILayout.GetControlRect(false, 30, GUILayout.ExpandWidth(true));

        EditorGUILayout.BeginHorizontal(GUILayout.Width(1000));
        EditorGUI.PropertyField(directionRect, direction_prop, GUIContent.none);
        EditorGUI.LabelField(labelRect, "types: ");

        float availableWidth = EditorGUIUtility.currentViewWidth - labelRect.position.x - labelRect.width;
        int colCt = Mathf.CeilToInt(availableWidth / tileTypeRect.width);

        // allowed types: a horizontal scrolling list like a regular array
        string[] tileTypeOptions = System.Enum.GetNames(typeof(TILE_TYPE));
        for (int i = 0; i < allowedTypes_prop.arraySize; i++)
        {
            int rowCt = Mathf.FloorToInt(i * tileTypeRect.width / availableWidth);
            float x = tileTypeRect.position.x;
            if (i % colCt == 0)
                x = labelRect.position.x + labelRect.width;
            float y = labelRect.position.y + EditorGUIUtility.singleLineHeight * rowCt;
            
            var type = allowedTypes_prop.GetArrayElementAtIndex(i);
            var elementRect = new Rect(x, y, tileTypeRect.width, tileTypeRect.height);
            EditorGUI.PropertyField(elementRect, type, GUIContent.none);
            if (tileTypeRect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown && Event.current.button == 1)
            {
                allowedTypes_prop.DeleteArrayElementAtIndex(i);
            }

            // if first of this row, reset the position x
            tileTypeRect = new Rect(x + tileTypeRect.width, y, tileTypeRect.width, tileTypeRect.height);
        }

        // button to add a new element to the array
        Rect addElementRect = new Rect(tileTypeRect.position.x, tileTypeRect.position.y, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);
        if (GUI.Button(addElementRect, "+"))
        {
            allowedTypes_prop.arraySize += 1;
        }
        if (GUI.Button(new Rect(addElementRect.position.x + addElementRect.width, addElementRect.position.y, 30, EditorGUIUtility.singleLineHeight), "ALL"))
        {
            allowedTypes_prop.arraySize = tileTypeOptions.Length;
            for (int i = 0; i < allowedTypes_prop.arraySize; i++)
            {
                var type = allowedTypes_prop.GetArrayElementAtIndex(i);
                type.enumValueIndex = i;
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float availableWidth = EditorGUIUtility.currentViewWidth - 105 - 50 - 30;
        allowedTypes_prop = property.FindPropertyRelative("allowedTypes");

        int colCt = Mathf.CeilToInt(availableWidth / 100);
        int rowCt = allowedTypes_prop.arraySize / colCt;

        return EditorGUIUtility.singleLineHeight * rowCt;
    }
}
