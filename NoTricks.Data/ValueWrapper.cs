namespace NoTricks.Data {
    /// <summary>
    /// Used for DI to inject a primitive string.
    /// </summary>
    /// <typeparam name="T">The value type to wrap</typeparam>
    public class ValueWrapper<T> {
        public T Value { get; }
        
        public ValueWrapper(T value) {
            Value = value;
        }
        
    }
}