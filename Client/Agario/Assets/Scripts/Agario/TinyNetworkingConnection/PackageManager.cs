using System;
using System.Collections.Generic;
using TNC.DataStructures;

namespace TNC
{
    /// <summary>
    /// Handles running subscriber actions when their subscribed package has been received.
    /// </summary>
    public class PackageManager
    {
        private readonly Dictionary<Type, Delegate> listeners = new();

        /// <summary>
        /// Subscribe to all packages of the provided type.
        /// </summary>
        /// <param name="actionOnReceivedPackage"></param>
        /// <typeparam name="TPackageType"></typeparam>
        public void SubscribeToPackage<TPackageType>(Action<TPackageType> actionOnReceivedPackage) where TPackageType : NetPackage
        {
            
        }

        /// <summary>
        /// Unsubscribe to all packages of the provided type.
        /// </summary>
        /// <param name="actionOnReceivedPackage"></param>
        /// <typeparam name="TPackageType"></typeparam>
        public void UnSubscribeToPackage<TPackageType>(Action<TPackageType> actionOnReceivedPackage) where TPackageType : NetPackage
        {
            
        }
    }
}