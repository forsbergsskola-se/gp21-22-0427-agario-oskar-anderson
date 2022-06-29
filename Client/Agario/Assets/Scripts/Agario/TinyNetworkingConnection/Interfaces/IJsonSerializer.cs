namespace TNC.Interfaces
{
    public interface IJsonSerializer
    {
        /// <summary>
        /// Serialize the provided object to json.
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <typeparam name="T">Object type</typeparam>
        /// <returns>json string based on provided object</returns>
        public string Serialize<T>(T obj);

        /// <summary>
        /// DeSerialize the provided json string into a predefined object type.
        /// </summary>
        /// <param name="json">Json string</param>
        /// <typeparam name="T">Target object type</typeparam>
        /// <returns>Object of the requested type with values from the json string</returns>
        public T DeSerialize<T>(string json);
    }
}