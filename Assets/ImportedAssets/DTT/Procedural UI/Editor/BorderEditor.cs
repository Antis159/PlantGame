using DTT.PublishingTools;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;
using UnityEngine.Events;
using System.Linq;

namespace DTT.UI.ProceduralUI.Editor
{
    [CustomEditor(typeof(Border)), CanEditMultipleObjects]
    [DTTHeader("dtt.proceduralui")]
    public class BorderEditor : DTTInspector
    {
        /// <summary>
        /// The border object.
        /// </summary>
        private Border _border;

        /// <summary>
        /// All the borders currently selected.
        /// </summary>
        private Border[] _borders;

        /// <summary>
        /// The serialized object's for the border.
        /// </summary>
        private BorderSerializedProperties _serializedProperties;

        /// <summary>
        /// All the sections that inspector should draw.
        /// </summary>
        private readonly List<IDrawable> _sections = new List<IDrawable>();

        protected override void OnEnable()
        {
            base.OnEnable();
            serializedObject.Update();

            // Obtain references.
            _border = (Border)target;
            _borders = targets.Cast<Border>().ToArray();
            _border.UpdateBorder();
            _serializedProperties = new BorderSerializedProperties(serializedObject);

            CreateSections();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUI.BeginChangeCheck();

            serializedObject.Update();

            // Draw all the sections.
            for (int i = 0; i < _sections.Count; i++)
                _sections[i].Draw();

            if (EditorGUI.EndChangeCheck() ||
                (Event.current.type == EventType.ValidateCommand &&
                 Event.current.commandName == "UndoRedoPerformed"))
            {
                serializedObject.ApplyModifiedProperties();
                foreach (Border effect in _borders)
                    effect.PropertyChanged = true;
            }
        }

        /// <summary>
        /// Creates the different sections that are in the inspector.
        /// </summary>
        private void CreateSections()
        {
            UnityAction repaintAction = new UnityAction(Repaint);

            // Create the Unit Settings section.
            _sections.Add(new UnitSettingsSection(_border.ParentRoundedImage, repaintAction,
                _serializedProperties.selectedUnit,
                _serializedProperties.borderThickness
            ));

            // Create the Border Settings section.
            _sections.Add(new BorderSection(_border, repaintAction,
                _serializedProperties
                ));
        }
    }
}
