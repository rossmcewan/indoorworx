using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace IndoorWorx.Library.Controls
{
    /// <summary>
    /// FishEyePanel
    /// </summary>

    public class FishEyePanel : Panel
    {
        public FishEyePanel()
        {
            Background = new SolidColorBrush(Color.FromArgb(00, 00, 00, 00));
            MouseMove += FishEyePanelMouseMove;
            MouseEnter += FishEyePanelMouseEnter;
            MouseLeave += FishEyePanelMouseLeave;
        }

        public double Magnification
        {
            get { return (double)GetValue(MagnificationProperty); }
            set { SetValue(MagnificationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Magnification.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MagnificationProperty =
            DependencyProperty.Register("Magnification", typeof(double), typeof(FishEyePanel), new PropertyMetadata(2d));

        public int AnimationMilliseconds
        {
            get { return (int)GetValue(AnimationMillisecondsProperty); }
            set { SetValue(AnimationMillisecondsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AnimationMilliseconds.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnimationMillisecondsProperty =
            DependencyProperty.Register("AnimationMilliseconds", typeof(int), typeof(FishEyePanel), new PropertyMetadata(125));


        // If set true we scale different sized children to a constant width
        public bool ScaleToFit
        {
            get { return (bool)GetValue(ScaleToFitProperty); }
            set { SetValue(ScaleToFitProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScaleToFit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScaleToFitProperty =
            DependencyProperty.Register("ScaleToFit", typeof(bool), typeof(FishEyePanel), new PropertyMetadata(true));

        private bool _animating;
        private Size _ourSize;
        private double _totalChildWidth;
        private bool _wasMouseOver;
        private Point _mousePosition;
        private bool _isMouseOver;

        void FishEyePanelMouseMove(object sender, MouseEventArgs e)
        {
            if (!_animating)
            {
                _mousePosition = e.GetPosition(this);
                _isMouseOver = true;
                InvalidateArrange();
            }
        }

        void FishEyePanelMouseEnter(object sender, MouseEventArgs e)
        {
            _mousePosition = e.GetPosition(this);
            _isMouseOver = true;
            InvalidateArrange();
        }

        void FishEyePanelMouseLeave(object sender, MouseEventArgs e)
        {
            _mousePosition = e.GetPosition(this);
            _isMouseOver = false;
            InvalidateArrange();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var idealSize = new Size(0, 0);

            // Allow children as much room as they want - then scale them
            var size = new Size(Double.PositiveInfinity, Double.PositiveInfinity);
            foreach (var child in Children)
            {
                child.Measure(size);
                idealSize.Width += child.DesiredSize.Width;
                idealSize.Height = Math.Max(idealSize.Height, child.DesiredSize.Height);
            }

            // EID calls us with infinity, but framework doesn't like us to return infinity
            if (double.IsInfinity(availableSize.Height) || double.IsInfinity(availableSize.Width))
                return idealSize;

            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Children == null || Children.Count == 0)
                return finalSize;

            _totalChildWidth = 0;

            foreach (var child in Children)
            {
                // If this is the first time we've seen this child, add our transforms
                if (child.RenderTransform as CompositeTransform == null)
                {
                    child.RenderTransformOrigin = new Point(0, 0.5);
                    var compositeTransform = new CompositeTransform();
                    child.RenderTransform = compositeTransform;
                }

                child.Arrange(new Rect(0, 0, child.DesiredSize.Width, child.DesiredSize.Height));
                _totalChildWidth += child.DesiredSize.Width;
            }

            finalSize.Width = _totalChildWidth;
            _ourSize = finalSize;
            AnimateAll();

            return finalSize;
        }

        void AnimateAll()
        {
            if (Children == null || Children.Count == 0)
                return;

            _animating = true;

            double childWidth = _ourSize.Width / Children.Count;
            // Scale the children so they fit in our size
            double overallScaleFactor = _ourSize.Width / _totalChildWidth;

            UIElement prevChild = null;
            UIElement theChild = null;
            UIElement nextChild = null;

            double widthSoFar = 0;
            double theChildX = 0;
            double ratio = 0;

            if (_isMouseOver)
            {
                double x = _mousePosition.X;
                foreach (UIElement child in Children)
                {
                    if (theChild == null)
                        theChildX = widthSoFar;

                    widthSoFar += (ScaleToFit ? childWidth : child.DesiredSize.Width * overallScaleFactor);

                    if (x < widthSoFar && theChild == null)
                        theChild = child;

                    if (theChild == null)
                        prevChild = child;

                    if (theChild == child || theChild == null) continue;

                    nextChild = child;

                    break;
                }
                if (theChild != null)
                    ratio = (x - theChildX) / (ScaleToFit ? childWidth : (theChild.DesiredSize.Width * overallScaleFactor));    // Range 0-1 of where the mouse is inside the child
            }

            // These next few lines took two of us hours to write!
            double mag = Magnification;
            double extra = 0;
            if (theChild != null)
                extra += (mag - 1);

            if (prevChild == null)
                extra += (ratio * (mag - 1));
            else if (nextChild == null)
                extra += ((mag - 1) * (1 - ratio));
            else
                extra += (mag - 1);

            double prevScale = Children.Count * (1 + ((mag - 1) * (1 - ratio))) / (Children.Count + extra);
            double theScale = (mag * Children.Count) / (Children.Count + extra);
            double nextScale = Children.Count * (1 + ((mag - 1) * ratio)) / (Children.Count + extra);
            double otherScale = Children.Count / (Children.Count + extra);       // Applied to all non-interesting children

            // Adjust for different sized children - we overmagnify large children, so shrink the others
            if (!ScaleToFit && _isMouseOver)
            {
                double bigWidth = 0;
                double actualWidth = 0;
                if (prevChild != null)
                {
                    bigWidth += prevScale * prevChild.DesiredSize.Width * overallScaleFactor;
                    actualWidth += prevChild.DesiredSize.Width;
                }
                if (theChild != null)
                {
                    bigWidth += theScale * theChild.DesiredSize.Width * overallScaleFactor;
                    actualWidth += theChild.DesiredSize.Width;
                }
                if (nextChild != null)
                {
                    bigWidth += nextScale * nextChild.DesiredSize.Width * overallScaleFactor;
                    actualWidth += nextChild.DesiredSize.Width;
                }
                double w = (_totalChildWidth - actualWidth) * overallScaleFactor * otherScale;
                otherScale *= (_ourSize.Width - bigWidth) / w;
            }

            widthSoFar = 0;
            double duration = 0;
            if (_wasMouseOver != _isMouseOver)
                duration = AnimationMilliseconds;

            foreach (UIElement child in Children)
            {
                if (child == null) continue;

                double scale = otherScale;
                if (child == prevChild)
                {
                    scale = prevScale;
                }
                else if (child == theChild)
                {
                    scale = theScale;
                }
                else if (child == nextChild)
                {
                    scale = nextScale;
                }

                if (ScaleToFit)
                {
                    // Now scale each individual child so it is a standard width
                    scale *= childWidth / child.DesiredSize.Width;
                }
                else
                {
                    // Apply overall scale so all children fit our width
                    scale *= overallScaleFactor;
                }

                AnimateTo(child, widthSoFar, (_ourSize.Height - child.DesiredSize.Height) / 2, scale, duration);

                //Debug.WriteLine("Obj: " + Children.IndexOf(child) + " widthSoFar(X): " + widthSoFar + " Y:" +
                //                (_ourSize.Height - child.DesiredSize.Height) / 2 + " scale: " + scale +
                //                " overallScaleFactor: " + overallScaleFactor);
                widthSoFar += child.DesiredSize.Width * scale;
            }

            _wasMouseOver = _isMouseOver;
        }

        private void AnimateTo(UIElement child, double x, double y, double s, double duration)
        {
            var compositeTransform = (CompositeTransform)child.RenderTransform;

            var buttonStoryboard = new Storyboard { Duration = TimeSpan.FromMilliseconds(duration) };

            buttonStoryboard.Children.Add(CreateDoubleAnimation(compositeTransform, "TranslateX", x,
                                                                                TimeSpan.FromMilliseconds(duration),
                                                                                true));
            buttonStoryboard.Children.Add(CreateDoubleAnimation(compositeTransform, "TranslateY", y,
                                                                                TimeSpan.FromMilliseconds(duration),
                                                                                true));
            buttonStoryboard.Children.Add(CreateDoubleAnimation(compositeTransform, "ScaleY", s,
                                                                                TimeSpan.FromMilliseconds(duration),
                                                                                true));
            buttonStoryboard.Children.Add(CreateDoubleAnimation(compositeTransform, "ScaleX", s,
                                                                                TimeSpan.FromMilliseconds(duration),
                                                                                true));
            buttonStoryboard.Completed += ButtonStoryboardCompleted;

            buttonStoryboard.Begin();
        }

        private void ButtonStoryboardCompleted(object sender, EventArgs e)
        {
            _animating = false;
            ((Storyboard)sender).Completed -= ButtonStoryboardCompleted;
        }

        internal static DoubleAnimation CreateDoubleAnimation(DependencyObject element, string property,
                                                            double to, Duration duration, bool addEasing)
        {
            var da = new DoubleAnimation { To = to, Duration = duration };
            //if (addEasing)
            //da.EasingFunction = new BackEase {Amplitude = 0.5, EasingMode = EasingMode.EaseOut};
            //da.EasingFunction = new PowerEase { EasingMode = EasingMode.EaseIn, Power = 9 };

            Storyboard.SetTargetProperty(da, new PropertyPath(property));
            Storyboard.SetTarget(da, element);
            return da;
        }

        internal static DoubleAnimationUsingKeyFrames CreateDoubleAnimationKeyFrames(DependencyObject element, string property,
                                                            double to, Duration duration, bool addEasing)
        {
            var da = new DoubleAnimationUsingKeyFrames { BeginTime = TimeSpan.FromSeconds(0) };
            var spline = new SplineDoubleKeyFrame { KeyTime = KeyTime.FromTimeSpan(duration.TimeSpan), Value = to };

            da.KeyFrames.Add(spline);

            Storyboard.SetTargetProperty(da, new PropertyPath(property));
            Storyboard.SetTarget(da, element);
            return da;
        }
    }
}
