﻿using System.Windows;
using System.Windows.Controls;
using Microsoft.Xna.Framework;
using VKDiplom.Engine;

namespace VKDiplom
{
    public partial class MainPage
    {
        private void FunctionDrawingSurface_OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            //            var drawingSurface = sender as DrawingSurface;
            //            if (drawingSurface != null && _functionScene != null)
            //                _functionScene.Camera.AspectRatio = (float) drawingSurface.ActualWidth/(float) drawingSurface.ActualHeight;
            //            _mouseDeltaScale = 300.0 / Math.Min(drawingSurface.ActualWidth, drawingSurface.ActualHeight);
            SizeChanged(sender as DrawingSurface, _functionScene);
        }

        private void FirstDerivationDrawingSurface_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            SizeChanged(sender as DrawingSurface, _firstDerScene);
        }

        private void SecondDerivationDrawingSurface_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            SizeChanged(sender as DrawingSurface, _secondDerScene);
        }

        private new void SizeChanged(DrawingSurface drawingSurface, Scene scene)
        {
            if (drawingSurface == null || _functionScene == null) return;

            scene.Camera.AspectRatio = (float) drawingSurface.ActualWidth/
                                       (float) drawingSurface.ActualHeight;

            //_mouseDeltaScale = 300.0/Math.Min(drawingSurface.ActualWidth, drawingSurface.ActualHeight);
        }

        //private readonly double TressholdToDefault = 0.025 * (ZScaleSlider.Maximum - scaler.Minimum);
        private void ScaleSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var scaler = sender as Slider;
            if (scaler == null) return;
            if (_functionScene == null) return;
            //if (scaler.Value < 2 && scaler.Value > -2)
            //{
            //    scaler.Value = 0;
            //}
            var tresholdToDefault = 0.025 * (ZScaleSlider.Maximum - scaler.Minimum);
            //if ((e.OldValue < 1-tresholdToDefault && e.NewValue > 1-tresholdToDefault) || (e.OldValue > 1+tresholdToDefault && e.NewValue < 1+tresholdToDefault))
            if (e.NewValue > 1 - tresholdToDefault && e.NewValue < 1 + tresholdToDefault)
                scaler.Value = 1;
            ScenesAction(scene=>scene.Scale = new Vector3(1,1,(float)scaler.Value));
        }
    }
}