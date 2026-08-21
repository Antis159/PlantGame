using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace DTT.UI.ProceduralUI.Editor
{
    /// <summary>
    /// Displays the section in the inspector for <see cref="Border"/> where the user 
    /// can adjust the border settings.
    /// </summary>
    public class BorderSection : Section<Border>
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override string HeaderName => "Border";

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override bool OpenFoldoutOnEnter => true;

        /// <summary>
        /// The selected unit property of <see cref="Border"/>.
        /// </summary>
        private SerializedProperty _selectedUnit;

        /// <summary>
        /// The color property of <see cref="Border"/>
        /// </summary>
        private SerializedProperty _color;

        /// <summary>
        /// The border thickness property of <see cref="Border"/>
        /// </summary>
        private SerializedProperty _borderThickness;

        /// <summary>
        /// The render outside property of <see cref="Border"/>
        /// </summary>
        private SerializedProperty _renderOutside;

        /// <summary>
        /// Draws the section for the border settings.
        /// </summary>
        protected override void DrawSection()
        {
            EditorGUILayout.PropertyField(_color);

            _borderThickness.floatValue = DrawSlider((RoundingUnit)_selectedUnit.enumValueIndex, _borderThickness.floatValue);

            EditorGUILayout.PropertyField(_renderOutside);
        }

        /// <summary>
        /// Creates a new border section.
        /// </summary>
        /// <param name="border">The border instance to apply this to.</param>
        /// <param name="repaint">When called should repaint the inspector.</param>
        /// <param name="properties">Contains all the properties relevant to draw <see cref="Border"/></param>
        /// <param name="selectedUnit"></param>
        public BorderSection(Border border, UnityAction repaint, BorderSerializedProperties properties) : base(border, repaint)
        {
            _selectedUnit = properties.selectedUnit;
            _color = properties.color;
            _borderThickness = properties.borderThickness;
            _renderOutside = properties.renderOutside;
        }
    }
}
