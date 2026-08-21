using DTT.Utils.EditorUtilities;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DTT.UI.ProceduralUI.Editor
{
    /// <summary>
	/// Contains all the serialized field names of <see cref="Border"/>.
	/// Can be used for getting serialized properties 
	/// via <see cref="UnityEditor.SerializedProperty.FindPropertyRelative(string)"/>.
	/// </summary>
    public class BorderSerializedProperties : SerializedPropertyCache
    {
        /// <summary>
		/// Name of the selected unit property of <see cref="Border"/>.
		/// </summary>
        public SerializedProperty selectedUnit => base[nameof(selectedUnit)];

        /// <summary>
		/// Name of the color property of <see cref="Border"/>.
		/// </summary>
        public SerializedProperty color => base[nameof(color)];

        /// <summary>
		/// Name of the border thickness property of <see cref="Border"/>.
		/// </summary>
        public SerializedProperty borderThickness => base[nameof(borderThickness)];

        /// <summary>
		/// Name of the render outside property of <see cref="Border"/>.
		/// </summary>
        public SerializedProperty renderOutside => base[nameof(renderOutside)];

        public BorderSerializedProperties(SerializedObject serializedObject) : base(serializedObject) { }
    }
}
