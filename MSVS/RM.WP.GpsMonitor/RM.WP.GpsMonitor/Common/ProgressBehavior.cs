using System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Microsoft.Xaml.Interactivity;

namespace RM.WP.GpsMonitor.Common
{

	public class ProgressBehavior : DependencyObject, IBehavior
	{
		private DependencyObject _associatedObject;
		private StatusBar _currentStatus;
		private string _presetText;
		private bool? _presetVisible;
		private bool _isValuePreset;
		private double? _presetValue;

		public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
																						"Text",
																						typeof(string),
																						typeof(ProgressBehavior),
																						new PropertyMetadata(null, OnTextChanged)
																					);

		public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.Register(
																						"IsVisible",
																						typeof(bool),
																						typeof(ProgressBehavior),
																						new PropertyMetadata(false, OnIsVisibleChanged)
																					);

		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
																						"Value",
																						typeof(object),
																						typeof(ProgressBehavior),
																						new PropertyMetadata(null, OnValueChanged)
																					);

		public DependencyObject AssociatedObject => _associatedObject;

		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		public bool IsVisible
		{
			get { return (bool)GetValue(IsVisibleProperty); }
			set { SetValue(IsVisibleProperty, value); }
		}

		public double? Value
		{
			get { return (double?)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		public void Attach(DependencyObject associatedObject)
		{
			_associatedObject = associatedObject;
			_currentStatus = StatusBar.GetForCurrentView();
		}

		public void Detach()
		{
			_associatedObject = null;
			_currentStatus = null;
			_presetText = null;
		}

		private static void OnIsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var isVisible = (bool)e.NewValue;
			var pb = GetCurrent(d);

			if (pb?._currentStatus != null)
			{
				var indicator = pb._currentStatus.ProgressIndicator;

				if (!String.IsNullOrEmpty(pb._presetText))
				{
					SetText(indicator, pb._presetText);
					pb._presetText = null;
				}

				if (pb._isValuePreset)
				{
					SetValue(indicator, pb._presetValue);
					pb._isValuePreset = false;
					pb._presetValue = null;
				}

				SetVisibility(indicator, isVisible);
			}
		}

		private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var text = String.Empty;
			var pb = GetCurrent(d);

			if (pb != null)
			{
				if (e.NewValue != null)
				{
					text = e.NewValue.ToString();
				}

				if (pb._currentStatus != null)
				{
					var indicator = pb._currentStatus.ProgressIndicator;

					if (pb._presetVisible.HasValue)
					{
						SetVisibility(indicator, pb._presetVisible.Value);
					}

					if (pb._isValuePreset)
					{
						SetValue(indicator, pb._presetValue);
						pb._isValuePreset = false;
						pb._presetValue = null;
					}

					SetText(indicator, text);
				}
				else
				{
					pb._presetText = text;
				}
			}
		}

		private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var val = (double?)e.NewValue;
			var pb = GetCurrent(d);

			if (pb != null)
			{
				if (pb._currentStatus != null)
				{
					var indicator = pb._currentStatus.ProgressIndicator;

					if (pb._presetVisible.HasValue)
					{
						SetVisibility(indicator, pb._presetVisible.Value);
						pb._presetVisible = null;
					}

					if (!String.IsNullOrEmpty(pb._presetText))
					{
						SetText(indicator, pb._presetText);
						pb._presetText = null;
					}

					SetValue(indicator, val);
				}
				else
				{
					pb._isValuePreset = true;
					pb._presetValue = val;
				}
			}
		}

		private static void SetVisibility(StatusBarProgressIndicator progressIndicator, bool isVisible)
		{
			if (isVisible)
			{
				progressIndicator.ShowAsync();
			}
			else
			{
				progressIndicator.HideAsync();
			}
		}

		private static void SetText(StatusBarProgressIndicator progressIndicator, string text)
		{
			progressIndicator.Text = text;
		}

		private static void SetValue(StatusBarProgressIndicator progressIndicator, double? value)
		{
			progressIndicator.ProgressValue = value;
		}

		private static ProgressBehavior GetCurrent(DependencyObject obj)
		{
			return obj as ProgressBehavior;
		}
	}
}
