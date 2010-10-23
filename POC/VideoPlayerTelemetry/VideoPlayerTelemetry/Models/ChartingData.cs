using System;
using System.Net;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows;
using Codeplex.Dashboarding;

namespace VideoPlayerTelemetry.Models
{
    public class ChartingData : BaseModel
    {
        #region Constructor

        public static ChartingData Create(Color textColor)
        {
            return new ChartingData
            {
                CurrentValue = 50,
                FaceTextColor = textColor,
                FaceTextFormat = "{0:0}",
                FaceTextVisibility = Visibility.Visible,
                ValueTextColor = textColor,
                ValueTextFormat = "{0:0}",
                ValueTextVisibility = Visibility.Visible,
                FaceColorRange = new ColorPointCollection(),
                NeedleColorRange = new ColorPointCollection()
            };
        }

        #endregion

        #region face text properties

        private string faceTextFormat;
        /// <summary>
        /// Gets or sets the face text format.
        /// </summary>
        /// <value>The face text format.</value>
        public string FaceTextFormat
        {
            get { return faceTextFormat; }
            set { faceTextFormat = value; OnPropertyChanged("FaceTextFormat"); }
        }

        private Color faceTextColor;
        /// <summary>
        /// Gets or sets the color of the face text.
        /// </summary>
        /// <value>The color of the face text.</value>
        public Color FaceTextColor
        {
            get { return faceTextColor; }
            set { faceTextColor = value; OnPropertyChanged("FaceTextColor"); }
        }

        private Visibility faceTextVisible;
        /// <summary>
        /// Gets or sets the face text visiblity.
        /// </summary>
        /// <value>The face text visiblity.</value>
        public Visibility FaceTextVisibility
        {
            get { return faceTextVisible; }
            set { faceTextVisible = value; OnPropertyChanged("FaceTextVisibility"); }
        }
        #endregion

        #region value text properties

        private string _valueTextFormat;
        /// <summary>
        /// Gets or sets the value text format.
        /// </summary>
        /// <value>The value text format.</value>
        public string ValueTextFormat
        {
            get { return _valueTextFormat; }
            set { _valueTextFormat = value; OnPropertyChanged("ValueTextFormat"); }
        }

        private Color _valueTextColor;
        /// <summary>
        /// Gets or sets the color of the value text.
        /// </summary>
        /// <value>The color of the value text.</value>
        public Color ValueTextColor
        {
            get { return _valueTextColor; }
            set { _valueTextColor = value; OnPropertyChanged("ValueTextColor"); }
        }

        private Visibility _valueTextVisible;
        /// <summary>
        /// Gets or sets the value text visible.
        /// </summary>
        /// <value>The value text visible.</value>
        public Visibility ValueTextVisibility
        {
            get { return _valueTextVisible; }
            set { _valueTextVisible = value; OnPropertyChanged("ValueTextVisibility"); }
        }

        #endregion

        #region Other

        private double currentValue;
        /// <summary>
        /// Current vlaue property, raises the on property changed event
        /// </summary>
        public double CurrentValue
        {
            get { return currentValue; }
            set
            {
                currentValue = value;
                OnPropertyChanged("CurrentValue");
            }
        }

        private bool isBidirectional;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is bidirectional.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is bidirectional; otherwise, <c>false</c>.
        /// </value>
        public bool IsBidirectional
        {
            get { return isBidirectional; }
            set { isBidirectional = value; OnPropertyChanged("IsBidirectional"); }
        }

        private double minValue = 0;
        /// <summary>
        /// Gets or sets the min value.
        /// </summary>
        /// <value>The min value.</value>
        public double Minimum
        {
            get { return minValue; }
            set
            {
                minValue = value;
                OnPropertyChanged("Minimum");
            }

        }

        private double maxValue = 100;
        /// <summary>
        /// Gets or sets the max value.
        /// </summary>
        /// <value>The max value.</value>
        public double Maximum
        {
            get { return maxValue; }
            set
            {
                maxValue = value;
                OnPropertyChanged("Maximum");
            }

        }

        private TimeSpan animationDuration = new TimeSpan(0, 0, 0, 0, 750);
        /// <summary>
        /// Gets or sets the duration of the animation.
        /// </summary>
        /// <value>The duration of the animation.</value>
        public TimeSpan AnimationDuration
        {
            get { return animationDuration; }
            set { animationDuration = value; OnPropertyChanged("AnimationDuration"); }
        }


        private ColorPointCollection faceColorRange;
        public ColorPointCollection FaceColorRange
        {
            get { return faceColorRange; }
            set { faceColorRange = value; OnPropertyChanged("FaceColorRange"); }
        }

        private ColorPointCollection needleColorRange;
        public ColorPointCollection NeedleColorRange
        {
            get { return needleColorRange; }
            set { needleColorRange = value; OnPropertyChanged("NeedleColorRange"); }
        }

        #endregion

    }
}
