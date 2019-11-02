namespace NoTricks.Data {
    /// <summary>
    /// Connection String used for NoTricks.Data Repos 
    /// </summary>
    public class NoTricksConnectionString : ValueWrapper<string> {
        public NoTricksConnectionString(string value) : base(value) { }
    }
}