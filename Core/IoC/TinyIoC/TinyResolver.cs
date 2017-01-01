using System;
using System.Collections.Generic;
using TinyIoC;

namespace AD.IoC.TinyIoC
{
    /// <summary>
    /// The tiny resolver.
    /// </summary>
    public class TinyResolver : IResolver
    {
        private readonly TinyIoCContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyResolver"/> class.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        public TinyResolver(TinyIoCContainer container)
        {
            this.container = container;
        }

        #region IResolver Members

        /// <summary>
        /// Resolve a dependency.
        /// </summary>
        /// <typeparam name="T">Type of instance to get.</typeparam>
        /// <returns>An instance of {T} if successful, otherwise null.</returns>
        public T Resolve<T>() where T : class
        {
            try
            {
                return this.container.Resolve<T>();
            }
            catch (TinyIoCResolutionException ex)
            {
                if (ex.InnerException != null)
                {
                    throw ex.InnerException;
                }

                return null;
            }
        }

        /// <summary>
        /// Resolve a dependency by type.
        /// </summary>
        /// <param name="type">Type of object.</param>
        /// <returns>An instance to type if found as <see cref="object"/>, otherwise null.</returns>
        public object Resolve(Type type)
        {
            try
            {
                return this.container.Resolve(type);
            }
            catch (TinyIoCResolutionException ex)
            {
                if (ex.InnerException != null)
                {
                    throw ex.InnerException;
                }

                return null;
            }
        }

        /// <summary>
        /// Resolve a dependency.
        /// </summary>
        /// <typeparam name="T">Type of instance to get.</typeparam>
        /// <returns>All instances of {T} if successful, otherwise null.</returns>
        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            return this.container.ResolveAll<T>();
        }

        /// <summary>
        /// Resolve a dependency by type.
        /// </summary>
        /// <param name="type">Type of object.</param>
        /// <returns>All instances of type if found as <see cref="object"/>, otherwise null.</returns>
        public IEnumerable<object> ResolveAll(Type type)
        {
            return this.container.ResolveAll(type);
        }

		public object Construct(Type type)
		{
			var instance = this.container.Resolve (type);
			this.container.BuildUp (instance);
			return instance;
		}

        #endregion
    }
}
