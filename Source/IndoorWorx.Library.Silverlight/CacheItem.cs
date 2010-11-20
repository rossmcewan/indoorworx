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

namespace IndoorWorx.Library
{
    /// Defines an item that's stored in the Cache.
    /// </summary>
    public class CacheItem
    {
        private readonly DateTime _creationDate = DateTime.Now;

        private readonly TimeSpan _validDuration;

        /// <summary>
        /// Constructs a cache item with for the data with a validity of validDuration.
        /// </summary>
        /// <param name="data">The data for the cache.</param>
        /// <param name="validDuration">The duration for the data being valid in the cache.</param>
        public CacheItem(object data, TimeSpan validDuration)
        {
            _validDuration = validDuration;
            Data = data;
        }

        /// <summary>
        /// The data in the Cache.
        /// </summary>
        public object Data { set; private get; }

        /// <summary>
        /// Gets the Data typed.
        /// </summary>
        /// <typeparam name="T">The Type for which the data is set. If the type is wrong null will be returned.</typeparam>
        /// <returns>The data typed.</returns>
        public T GetData<T>() where T : class
        {
            return Data as T;
        }

        /// <summary>
        /// Gets the Data untyped.
        /// </summary>
        /// <returns>The data untyped.</returns>
        public object GetData()
        {
            return Data;
        }

        /// <summary>
        /// Check if the Data is still valid.
        /// </summary>
        /// <returns>Valid if the validDuration hasn't passed.</returns>
        public bool IsValid()
        {
            return _creationDate.Add(_validDuration) > DateTime.Now;
        }
    }
}
