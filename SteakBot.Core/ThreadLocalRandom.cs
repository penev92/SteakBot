using System;
using System.Threading;

namespace SteakBot.Core
{
    // https://codeblog.jonskeet.uk/2009/11/04/revisiting-randomness/
    /// <summary>
    /// Convenience class for dealing with randomness.
    /// </summary>
    public static class ThreadLocalRandom
    {
        /// <summary>
        /// Random number generator used to generate seeds,
        /// which are then used to create new random number
        /// generators on a per-thread basis.
        /// </summary>
        private static readonly Random GlobalRandom = new Random(Guid.NewGuid().GetHashCode());
        private static readonly object GlobalLock = new object();

        /// <summary>
        /// Random number generator
        /// </summary>
        private static readonly ThreadLocal<Random> ThreadRandom = new ThreadLocal<Random>(NewRandom);

        /// <summary>
        /// Creates a new instance of Random. The seed is derived
        /// from a global (static) instance of Random, rather
        /// than time.
        /// </summary>
        public static Random NewRandom()
        {
            lock (GlobalLock)
            {
                return new Random(GlobalRandom.Next());
            }
        }

        /// <summary>
        /// Returns an instance of Random which can be used freely
        /// within the current thread.
        /// </summary>
        public static Random Instance { get { return ThreadRandom.Value; } }
    }
}
