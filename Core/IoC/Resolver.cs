using System;
using System.Collections.Generic;

/// NOTA: most of source code from: Xamarin-Forms-Labs
/// //////////////////////////////////////////////////
using TinyIoC;

namespace AD
{
	/// <summary>
	/// Interface definition for dependency resolver.
	/// </summary>
	public interface IResolver
	{
		/// <summary>
		/// Resolve a dependency.
		/// </summary>
		/// <typeparam name="T">Type of instance to get.</typeparam>
		/// <returns>An instance of {T} if successful, otherwise null.</returns>
		T Resolve<T>() where T : class;

		/// <summary>
		/// Resolve a dependency by type.
		/// </summary>
		/// <param name="type">Type of object.</param>
		/// <returns>An instance to type if found as <see cref="object"/>, otherwise null.</returns>
		object Resolve(Type type);

		/// <summary>
		/// Resolve a dependency.
		/// </summary>
		/// <typeparam name="T">Type of instance to get.</typeparam>
		/// <returns>All instances of {T} if successful, otherwise null.</returns>
		IEnumerable<T> ResolveAll<T>() where T : class;

		/// <summary>
		/// Resolve a dependency by type.
		/// </summary>
		/// <param name="type">Type of object.</param>
		/// <returns>All instances of type if found as <see cref="object"/>, otherwise null.</returns>
		IEnumerable<object> ResolveAll(Type type);

		/// <summary>>
		/// Construct object
		/// </summary>
		/// 
		object Construct (Type type);

	}

	public class Resolver 
	{
		private static volatile IResolver _resolver;
		//private static object _syncRoot = new object ();

		private Resolver() {}

		private static IResolver _Instance {
			get {
				if (_resolver == null) {
					throw new Exception("Resolver not set");
					/*lock (_syncRoot) {
						if (_resolver == null) {

							if (_resolver == null) { // defaults to TinyIoC
								_resolver = new TinyResolver (TinyIoCContainer.Current);
							}
						}
					}*/
				}
				return _resolver;
			}
			set {
				_resolver = value;
			}
		}

		public static void SetResolver(IResolver resolver)
		{
			_Instance = resolver;
		}


		/// <summary>
		/// Resolve a dependency.
		/// </summary>
		/// <typeparam name="T">Type of instance to get.</typeparam>
		/// <returns>An instance of {T} if successful, otherwise null.</returns>
		/// <exception cref="InvalidOperationException">IResolver instance has not been set.</exception>
		public static T Resolve<T>() where T : class
		{
			return _Instance.Resolve<T>();
		}

		/// <summary>
		/// Resolve a dependency by type.
		/// </summary>
		/// <param name="type">Type of object.</param>
		/// <returns>An instance to type if found as <see cref="object"/>, otherwise null.</returns>
		/// <exception cref="InvalidOperationException">IResolver instance has not been set.</exception>
		public static object Resolve(Type type)
		{
			return _Instance.Resolve(type);
		}

		/// <summary>
		/// Resolve a dependency.
		/// </summary>
		/// <typeparam name="T">Type of instance to get.</typeparam>
		/// <returns>All instances of {T} if successful, otherwise null.</returns>
		/// <exception cref="InvalidOperationException">IResolver instance has not been set.</exception>
		public static IEnumerable<T> ResolveAll<T>() where T : class
		{
			return _Instance.ResolveAll<T>();
		}

		/// <summary>
		/// Resolve a dependency by type.
		/// </summary>
		/// <param name="type">Type of object.</param>
		/// <returns>All instances of type if found as <see cref="object"/>, otherwise null.</returns>
		/// <exception cref="InvalidOperationException">IResolver instance has not been set.</exception>
		public static IEnumerable<object> ResolveAll(Type type)
		{
			return _Instance.ResolveAll(type);
		}

		public static object Construct(Type type)
		{
			return _Instance.Construct (type);
		}

	}
}

