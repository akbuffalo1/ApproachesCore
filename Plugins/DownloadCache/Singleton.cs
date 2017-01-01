#if _DL_CACHE_
using System;
using System.Collections.Generic;
using System.Text;

namespace AD.Plugins.DownloadCache
{
    public abstract class Singleton : IDisposable
    {
        ~Singleton()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract void Dispose(bool isDisposing);

        private static readonly List<Singleton> Singletons = new List<Singleton>();

        protected Singleton()
        {
            lock (Singletons)
            {
                Singletons.Add(this);
            }
        }

        public static void ClearAllSingletons()
        {
            lock (Singletons)
            {
                foreach (var s in Singletons)
                {
                    s.Dispose();
                }

                Singletons.Clear();
            }
        }
    }

    public abstract class Singleton<TInterface>
        : Singleton
        where TInterface : class
    {
        protected Singleton()
        {
            if (Instance != null)
                throw new Exception("You cannot create more than one instance of Singleton");

            Instance = this as TInterface;
        }

        public static TInterface Instance { get; private set; }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                Instance = null;
            }
        }
    }
}
#endif