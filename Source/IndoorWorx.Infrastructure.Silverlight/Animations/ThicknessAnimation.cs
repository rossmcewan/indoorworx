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

namespace IndoorWorx.Infrastructure.Animations
{
    public class ThicknessAnimation
    {
        // The time along the animation from 0-1
        public static DependencyProperty TimeProperty = DependencyProperty.RegisterAttached("Time", typeof(double), typeof(DoubleAnimation), new PropertyMetadata(OnTimeChanged));
        // The object being animated
        public static DependencyProperty TargetProperty = DependencyProperty.RegisterAttached("Target", typeof(DependencyObject), typeof(ThicknessAnimation), null);
        // The thickness we're animating to
        public static DependencyProperty FromProperty = DependencyProperty.RegisterAttached("From", typeof(Thickness), typeof(DependencyObject), null);
        // The tickness we're animating from
        public static DependencyProperty ToProperty = DependencyProperty.RegisterAttached("To", typeof(Thickness), typeof(DependencyObject), null);
        // The target property to animate to.  Should have a property type of Thickness
        public static DependencyProperty TargetPropertyProperty = DependencyProperty.RegisterAttached("TargetProperty", typeof(DependencyProperty), typeof(DependencyObject), null);

        /// <summary>
        /// Creates a Timeline used to animate the thickness of an object
        /// </summary>
        /// <param name="target">The object to animate</param>
        /// <param name="targetProperty">The property on the object to animate</param>
        /// <param name="duration">The length of the animation</param>
        /// <param name="from">The begining thickness</param>
        /// <param name="to">The final thickness</param>
        /// <returns>A timeline object that can be added to a storyboard</returns>
        public static Timeline Create(DependencyObject target, DependencyProperty targetProperty, Duration duration, Thickness from, Thickness to)
        {
            DoubleAnimation timeAnimation = new DoubleAnimation() { From = 0, To = 1, Duration = duration };
            timeAnimation.AutoReverse = false;
            timeAnimation.SetValue(TargetProperty, target);
            timeAnimation.SetValue(TargetPropertyProperty, targetProperty);
            timeAnimation.SetValue(FromProperty, from);
            timeAnimation.SetValue(ToProperty, to);
            Storyboard.SetTargetProperty(timeAnimation, new PropertyPath("(ThicknessAnimation.Time)"));
            Storyboard.SetTarget(timeAnimation, timeAnimation);
            return timeAnimation;
        }

        /// <summary>
        /// Silverlight's animation system is animating time from 0 to 1.  When time changes we update the thickness to be time
        /// percent between from and to
        /// </summary>
        private static void OnTimeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            DoubleAnimation animation = (DoubleAnimation)sender;
            double time = GetTime(animation);
            Thickness from = (Thickness)sender.GetValue(FromProperty);
            Thickness to = (Thickness)sender.GetValue(ToProperty);
            DependencyProperty targetProperty = (DependencyProperty)sender.GetValue(TargetPropertyProperty);
            DependencyObject target = (DependencyObject)sender.GetValue(TargetProperty);
            var left = (to.Left - from.Left) * time + from.Left;
            var top = (to.Top - from.Top) * time + from.Top;
            var right = (to.Right - from.Right) * time + from.Right;
            var bottom = (to.Bottom - from.Bottom) * time + from.Bottom;
            target.SetValue(targetProperty, new Thickness(left,
                                                          top,
                                                          right,
                                                          bottom));

        }

        public static double GetTime(DoubleAnimation animation)
        {
            return (double)animation.GetValue(TimeProperty);
        }

        public static void SetTime(DoubleAnimation animation, double value)
        {
            animation.SetValue(TimeProperty, value);
        }
    }
}
