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
using System.Windows.Media.Effects;

namespace IndoorWorx.Infrastructure.Effects
{
    public class WetFloorEffect : ShaderEffect
    {
        public static readonly DependencyProperty ReflectionDepthProperty =
            DependencyProperty.Register("ReflectionDepth",
            typeof(double), typeof(WetFloorEffect),
            new PropertyMetadata(1.0, PixelShaderConstantCallback(0)));

        public double ReflectionDepth
        {
            get { return ((double)(GetValue(ReflectionDepthProperty))); }
            set
            {
                if (value <= 0.0)
                    value = 0.00001; // Avoid divide-by-zero errors in HLSL
                if (value > 1.0)
                    value = 1.0;
                SetValue(ReflectionDepthProperty, value);
            }
        }

        public static readonly DependencyProperty SourceHeightProperty =
            DependencyProperty.Register("SourceHeight",
            typeof(double), typeof(WetFloorEffect),
            new PropertyMetadata(0.0, OnSourceHeightChanged));

        public double SourceHeight
        {
            get { return (double)GetValue(SourceHeightProperty); }
            set
            {
                if (value < 0.0)
                    throw new ArgumentOutOfRangeException
                        ("SourceHeight", "SourceHeight cannot be negative");
                SetValue(SourceHeightProperty, value);
            }
        }

        static void OnSourceHeightChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        {
            ((WetFloorEffect)obj).PaddingBottom = (double)e.NewValue;
        }

        public WetFloorEffect()
        {
            PixelShader = new PixelShader()
            {
                UriSource =
                    new Uri("/CustomShaderDemo;component/WetFloorEffect.ps",
                    UriKind.Relative)
            };
            UpdateShaderValue(ReflectionDepthProperty);
            UpdateShaderValue(SourceHeightProperty);
        }
    }
}
