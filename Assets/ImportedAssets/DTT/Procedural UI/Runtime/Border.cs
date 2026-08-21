using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;

namespace DTT.UI.ProceduralUI
{
    /// <summary>
    /// This class adds a border as a hidden child to the rounded image.
    /// </summary>
    [RequireComponent(typeof(RoundedImage))]
    [AddComponentMenu("UI/Border")]
    [DisallowMultipleComponent]
    [ExecuteAlways]
    public class Border : MonoBehaviour
    {
        /// <summary>
        /// The unit that is currently being used.
        /// </summary>
        [SerializeField]
        private RoundingUnit _selectedUnit = RoundingUnit.PERCENTAGE;

        /// <summary>
        /// The color of the border.
        /// </summary>
        [SerializeField]
        private Color _color = Color.black;

        /// <summary>
        /// The border thickness amount.
        /// </summary>
        [SerializeField]
        private float _borderThickness = 0.5f;

        /// <summary>
        /// Whether or not the border should be rendered outside of the rounded image.
        /// </summary>
        [SerializeField]
        private bool _renderOutside;

        /// <summary>
        /// Whether a property has changed.
        /// </summary>
        private bool _propertyChanged;

        /// <summary>
        /// The reference to the parent rounded image.
        /// </summary>
        private RoundedImage _parentRoundedImage;

        /// <summary>
        /// The reference to the border rounded image.
        /// </summary>
        private RoundedImage _borderRoundedImage;

        /// <summary>
        /// The reference to the parent rect transform.
        /// </summary>
        private RectTransform _parentRectTransform;

        /// <summary>
        /// The reference to the border rect transform.
        /// </summary>
        private RectTransform _borderRectTransform;

        /// <summary>
        /// The reference to the parent rounded image.
        /// </summary>
        public RoundedImage ParentRoundedImage => _parentRoundedImage;

        /// <summary>
        /// The reference to the border rounded image.
        /// </summary>
        public RoundedImage BorderRoundedImage => _borderRoundedImage;

        /// <summary>
        /// The color of the border.
        /// </summary>
        public Color Color
        {
            get => _color;
            set
            {
                if (_color != value)
                    _propertyChanged = true;

                _color = value;
            }
        }

        /// <summary>
        /// Whether or not the border should be rendered outside of the rounded image.
        /// </summary>
        public bool RenderOutside
        {
            get => _renderOutside;
            set
            {
                if (_renderOutside != value)
                    _propertyChanged = true;

                _renderOutside = value;
            }
        }

        /// <summary>
        /// This event gets invoked when the border is updated.
        /// </summary>
        public Action OnUpdate;

        /// <summary>
        /// The border thickness amount.
        /// </summary>
        public float BorderThickness
        {
            get
            {
                switch (_selectedUnit)
                {
                    case RoundingUnit.PERCENTAGE:
                        return _borderThickness;
                    case RoundingUnit.WORLD:
                        return _borderThickness / _parentRectTransform.rect.GetShortLength() * 2;
                    default:
                        throw new NotSupportedException("This unit is not supported " +
                                                        "for getting border thickness.");
                }
            }
            set
            {
                switch (_selectedUnit)
                {
                    case RoundingUnit.PERCENTAGE:
                        if (_borderThickness != value)
                            _propertyChanged = true;

                        _borderThickness = value;
                        break;
                    case RoundingUnit.WORLD:
                        if (_borderThickness != value)
                            _propertyChanged = true;

                        _borderThickness = value * _parentRectTransform.rect.GetShortLength() * 0.5f;
                        break;
                    default:
                        throw new NotSupportedException("This unit is not supported " +
                                                        "for setting border thickness.");
                }
            }
        }

        /// <summary>
        /// The unit that is currently being used.
        /// </summary>
        public RoundingUnit RoundingUnit
        {
            get => _selectedUnit;
            set => _selectedUnit = value;
        }

        /// <summary>
        /// Whether a property has changed.
        /// </summary>
        public bool PropertyChanged
        {
            get => _propertyChanged;
            set => _propertyChanged = value;
        }

        /// <summary>
        /// Creates the border rounded image and sets all the properties.
        /// </summary>
        private void Awake()
        {
            // Get the parent rounded image and rect transfom.
            _parentRectTransform = GetComponent<RectTransform>();
            _parentRoundedImage = GetComponent<RoundedImage>();

            GameObject borderObject;

            // Check if the border object already exists. If not, create a new one.
            Transform borderTransform = transform.Find("Border");
            if (borderTransform == null)
            {
                // Create a hidden border rounded image.
                borderObject = new GameObject("Border");
                _borderRoundedImage = borderObject.AddComponent<RoundedImage>();
            }
            else
            {
                borderObject = borderTransform.gameObject;
                _borderRoundedImage = borderObject.GetComponent<RoundedImage>();

            }

            borderObject.transform.SetParent(transform, false);
            
            // Set all the border rect transform properties.
            _borderRectTransform = _borderRoundedImage.GetComponent<RectTransform>();
            _borderRectTransform.anchorMin = Vector2.zero;
            _borderRectTransform.anchorMax = Vector2.one;
            _borderRectTransform.pivot = new Vector2(0.5f, 0.5f);
            _borderRectTransform.sizeDelta = Vector2.zero;

            // Hide the border rounded image from the hierarchy.
            borderObject.hideFlags = HideFlags.HideAndDontSave;
            //borderObject.hideFlags = HideFlags.DontSave;

            // Set this border on the parent rounded image.
            _parentRoundedImage.Border = this;
        }

        /// <summary>
        /// Adds a listener to the update event of the parent rounded image.
        /// </summary>
        private void OnEnable() => _parentRoundedImage.OnUpdate += UpdateBorder;

        /// <summary>
        /// Removes the listener from the update event of the parent rounded image.
        /// </summary>
        private void OnDisable() => _parentRoundedImage.OnUpdate -= UpdateBorder;

        /// <summary>
        /// Checks if the border needs to be updated.
        /// </summary>
        private void Update()
        {
            if (_propertyChanged)
            {
                UpdateBorder();
                _propertyChanged = false;

                OnUpdate?.Invoke();
            }
        }

        /// <summary>
        /// Destroys the hidden border rounded image.
        /// </summary>
        private void OnDestroy() => DestroyImmediate(_borderRoundedImage.gameObject);

        /// <summary>
        /// Copy all the properties from rounded image to the target rounded image.
        /// </summary>
        /// <param name="from">The rounded image to copy the properties from.</param>
        /// <param name="to">The rounded image to copy the properties to.</param>
        public void CopyImageProperties(RoundedImage from, RoundedImage to)
        {
            // Copy all the properties from the current rounded image to the target rounded image.
            to.material = from.material;
            to.DistanceFalloff = from.DistanceFalloff;
            to.raycastTarget = from.raycastTarget;
            to.UseHitboxOutside = from.UseHitboxOutside;
            to.UseHitboxInside = from.UseHitboxInside;
            to.maskable = from.maskable;

            // Copy all corner rounding values.
            ReadOnlyDictionary<Corner, float> corners = from.GetCornerRounding();
            foreach (KeyValuePair<Corner, float> corner in corners)
                to.SetCornerRounding(corner.Key, corner.Value);
        }

        /// <summary>
        /// Updates the border.
        /// </summary>
        public void UpdateBorder() => StartCoroutine(UpdateBorderTask());

        /// <summary>
        /// Updates the border.
        /// </summary>
        private IEnumerator UpdateBorderTask()
        {
            yield return new WaitWhile(() => CanvasUpdateRegistry.IsRebuildingGraphics());

            CopyImageProperties(_parentRoundedImage, _borderRoundedImage);

            // Set custom properties.
            _borderRoundedImage.color = _color;
            _borderRoundedImage.Mode = RoundingMode.BORDER;
            _borderRoundedImage.RoundingUnit = _selectedUnit;

            if (_renderOutside)
            {
                // The thickness set on the border component in percentage.
                float borderPercentage = BorderThickness;

                // The size of the border in world units.
                float borderSize = borderPercentage * _parentRectTransform.rect.GetShortLength();

                // The total size of the image and the border in world units.
                float totalSize = _parentRectTransform.rect.GetShortLength() + borderSize;

                // The actual thickness of the border in percentage.
                float borderFill = borderSize / totalSize;

                // Get all the coners rounding value.
                var targetCorners = _borderRoundedImage.GetCornerRounding();

                // Set the size of the border rect transform.
                _borderRectTransform.sizeDelta = new Vector2(borderSize, borderSize);

                // Adjust the corner roundings.
                foreach (var corner in targetCorners)
                {
                    // The new corner rounding value in world units.
                    float cornerValueWorldUnits = corner.Value * _parentRectTransform.rect.GetShortLength() + (corner.Value * _parentRectTransform.rect.GetShortLength() * BorderThickness);

                    // Convert the corner rounding value to a percentage.
                    float endValue = Mathf.Clamp(cornerValueWorldUnits / totalSize, 0, 1);

                    // Set the adjusted corner rounding.
                    _borderRoundedImage.SetCornerRounding(corner.Key, endValue);
                }

                _borderRoundedImage.BorderThickness = borderFill;
            }
            else
            {
                // Set the size of the border to match the parent size.
                _borderRectTransform.sizeDelta = Vector2.zero;

                _borderRoundedImage.BorderThickness = BorderThickness;
            }
        }
    }
}
