using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace RM.Lib.Wpf.Common.ViewModel
{
	public abstract class BindableBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;

		public bool SetProperty<T>(ref T field, T value, Expression<Func<T>> property)
		{
			return SetProperty(ref field, value, GetName(property));
		}

		protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null!)
		{
			if (!Equals(storage, value))
			{
				storage = value;
				RaisePropertyChanged(propertyName);

				return true;
			}

			return false;
		}
		
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null!)
		{
			RaisePropertyChanged(propertyName);
		}
		
		protected void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
		{
			RaisePropertyChanged(GetName(propertyExpression));
		}
		
		protected static string GetName<T>(Expression<Func<T>> propertyExpression)
		{
			if (propertyExpression == null)
			{
				throw new ArgumentNullException(nameof(propertyExpression));
			}

			if (propertyExpression.Body is MemberExpression { Member: PropertyInfo { GetMethod.IsStatic: false } } memberExpression)
			{
				return memberExpression.Member.Name;
			}

			throw new ArgumentException($"Incorrect expression type '{propertyExpression.GetType()}'", nameof(propertyExpression));
		}

		private void RaisePropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
